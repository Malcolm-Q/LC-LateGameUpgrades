using GameNetcodeStuff;
using JetBrains.Annotations;
using MoreShipUpgrades.Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

namespace MoreShipUpgrades.UpgradeComponents
{
    internal class AdvTPButtonScript : GrabbableObject
    {
        public AudioClip ItemBreak, error, buttonPress;
        private AudioSource audio;

        public override void Start()
        {
            base.Start();
            audio = GetComponent<AudioSource>();
            gameObject.GetComponent<NetworkObject>().Spawn();
        }

        public override void DiscardItem()
        {
            this.playerHeldBy.activatingItem = false;
            base.DiscardItem();
        }

        public override void ItemActivate(bool used, bool buttonDown = true)
        {
            base.ItemActivate(used, buttonDown);
            if (Mouse.current.leftButton.isPressed)
            {
                audio.PlayOneShot(buttonPress);
                if (!itemUsedUp)
                {
                    ShipTeleporter[] tele = GameObject.FindObjectsOfType<ShipTeleporter>();
                    ShipTeleporter NotInverseTele = null;
                    foreach (ShipTeleporter shipTeleporter in tele)
                    {
                        if (!shipTeleporter.isInverseTeleporter)
                        {
                            NotInverseTele = shipTeleporter;
                            break;
                        }
                    }
                    if (NotInverseTele == null)
                    {
                        audio.PlayOneShot(error);
                        return;
                    }
                    int thisPlayersIndex = -1;
                    for (int i = 0; i < StartOfRound.Instance.mapScreen.radarTargets.Count(); i++)
                    {
                        if (StartOfRound.Instance.mapScreen.radarTargets[i].transform.gameObject.GetComponent<PlayerControllerB>() == playerHeldBy)
                        {
                            thisPlayersIndex = i;
                        }
                    }
                    if (thisPlayersIndex == -1)
                    {
                        StartOfRound.Instance.mapScreen.targetedPlayer = playerHeldBy;
                        UpgradeBus.instance.TPButtonPressed = true;
                        NotInverseTele.PressTeleportButtonOnLocalClient();
                    }
                    else
                    {
                        StartOfRound.Instance.mapScreen.SwitchRadarTargetAndSync(thisPlayersIndex);
                        StartCoroutine(WaitToTP(NotInverseTele));
                    }
                }
                else
                {
                    audio.PlayOneShot(error);
                }
            }
        }

        private IEnumerator WaitToTP(ShipTeleporter tele)
        {
            yield return new WaitForSeconds(0.15f);
            ReqUpdateTpDropStatusServerRpc();
            tele.PressTeleportButtonOnLocalClient();
            if (UnityEngine.Random.Range(0f, 1f) < UpgradeBus.instance.cfg.ADV_CHANCE_TO_BREAK) // 0.1f
            {
                audio.PlayOneShot(ItemBreak);
                itemUsedUp = true;
                HUDManager.Instance.chatText.text += "\n<color=#FF0000>The teleporter button has suffered irreparable damage and destroyed itself!</color>";
                playerHeldBy.DespawnHeldObject();
            }
        }

        [ServerRpc(RequireOwnership = false)]
        public void ReqUpdateTpDropStatusServerRpc()
        {
            ChangeTPButtonPressedClientRpc();
        }

        [ClientRpc]
        private void ChangeTPButtonPressedClientRpc()
        {
            UpgradeBus.instance.TPButtonPressed = true;
        }
    }
}
