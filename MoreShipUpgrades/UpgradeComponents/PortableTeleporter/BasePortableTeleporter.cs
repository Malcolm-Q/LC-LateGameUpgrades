﻿using GameNetcodeStuff;
using MoreShipUpgrades.Managers;
using System.Collections;
using System.Linq;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MoreShipUpgrades.UpgradeComponents.PortableTeleporter
{
    internal class BasePortableTeleporter : GrabbableObject
    {
        public AudioClip ItemBreak, error, buttonPress;
        private AudioSource audio;
        protected float breakChance;
        protected bool keepItems;
        public override void Start()
        {
            base.Start();
            audio = GetComponent<AudioSource>();
        }
        public override void DiscardItem()
        {
            playerHeldBy.activatingItem = false;
            base.DiscardItem();
        }

        public override void ItemActivate(bool used, bool buttonDown = true)
        {
            base.ItemActivate(used, buttonDown);
            if (!Mouse.current.leftButton.isPressed) return;
            audio.PlayOneShot(buttonPress);
            if (itemUsedUp)
            {
                audio.PlayOneShot(error);
                return;
            }
            ShipTeleporter[] tele = FindObjectsOfType<ShipTeleporter>();
            ShipTeleporter NotInverseTele = null;
            foreach (ShipTeleporter shipTeleporter in tele)
            {
                if (shipTeleporter.isInverseTeleporter) continue;

                NotInverseTele = shipTeleporter;
                break;
            }
            if (NotInverseTele == null)
            {
                audio.PlayOneShot(error);
                return;
            }
            int thisPlayersIndex = -1;
            for (int i = 0; i < StartOfRound.Instance.mapScreen.radarTargets.Count(); i++)
            {
                if (StartOfRound.Instance.mapScreen.radarTargets[i].transform.gameObject.GetComponent<PlayerControllerB>() != playerHeldBy) continue;

                thisPlayersIndex = i;
                break;
            }
            if (thisPlayersIndex == -1)
            {
                //this shouldn't occur but if it does, this will teleport this client and the server targeted player.
                StartOfRound.Instance.mapScreen.targetedPlayer = playerHeldBy;
                UpgradeBus.instance.TPButtonPressed = true;
                NotInverseTele.PressTeleportButtonOnLocalClient();
                if (Random.Range(0f, 1f) > breakChance)
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

        private IEnumerator WaitToTP(ShipTeleporter tele)
        {
            // if we don't do a little wait we'll tp the previously seleccted player.
            yield return new WaitForSeconds(0.15f);
            if (keepItems) ReqUpdateTpDropStatusServerRpc();
            tele.PressTeleportButtonOnLocalClient();
            if (Random.Range(0f, 1f) < breakChance) // 0.9f
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
