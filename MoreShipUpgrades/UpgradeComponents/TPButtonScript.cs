using GameNetcodeStuff;
using MoreShipUpgrades.Managers;
using System.Collections;
using System.Linq;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MoreShipUpgrades.UpgradeComponents
{
    internal class TPButtonScript : GrabbableObject
    {
        public AudioClip ItemBreak, error, buttonPress;
        private AudioSource audio;

        public override void Start()
        {
            base.Start();
            audio = GetComponent<AudioSource>();
        }
        public override void DiscardItem()
        {
            this.playerHeldBy.activatingItem = false;
            base.DiscardItem();
        }

        public override void ItemActivate(bool used, bool buttonDown = true)
        {
            base.ItemActivate(used, buttonDown);
            if (buttonDown)
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
                    if(thisPlayersIndex == -1)
                    {
                        //this shouldn't occur but if it does, this will teleport this client and the server targeted player.
                        StartOfRound.Instance.mapScreen.targetedPlayer = playerHeldBy;
                        UpgradeBus.instance.TPButtonPressed = true;
                        NotInverseTele.PressTeleportButtonOnLocalClient();
                        if (UnityEngine.Random.Range(0f, 1f) > UpgradeBus.instance.cfg.CHANCE_TO_BREAK)
                        {
                            audio.PlayOneShot(ItemBreak);
                            itemUsedUp = true;
                            playerHeldBy.DespawnHeldObject();
                        }
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
            // if we don't do a little wait we'll tp the previously seleccted player.
            yield return new WaitForSeconds(0.15f);
            if(UpgradeBus.instance.cfg.KEEP_ITEMS_ON_TELE) ReqUpdateTpDropStatusServerRpc();
            tele.PressTeleportButtonOnLocalClient();
            if (UnityEngine.Random.Range(0f, 1f) < UpgradeBus.instance.cfg.CHANCE_TO_BREAK) // 0.9f
            {
                audio.PlayOneShot(ItemBreak);
                itemUsedUp = true;
                HUDManager.Instance.DisplayTip("TELEPORTER BROKE!", "The teleporter button has suffered irreparable damage and destroyed itself!", true, false, "LC_Tip1");
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
