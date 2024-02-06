using GameNetcodeStuff;
using MoreShipUpgrades.Managers;
using System.Collections;
using System.Linq;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MoreShipUpgrades.UpgradeComponents.Items.PortableTeleporter
{
    internal class BasePortableTeleporter : GrabbableObject
    {
        /// <summary>
        /// Sounds played when interacting with the portable teleporter
        /// </summary>
        public AudioClip ItemBreak, error, buttonPress;
        /// <summary>
        /// Source of the sounds played
        /// </summary>
        private AudioSource audio;
        /// <summary>
        /// Chance of the portable teleporter breaking when activated
        /// </summary>
        protected float breakChance;
        /// <summary>
        /// Wether the portable teleporter will allow the player who activated it to keep items when teleported
        /// </summary>
        protected bool keepItems;
        /// <summary>
        /// The normal teleporter of the ship (if not bought, it will be null)
        /// </summary>
        private ShipTeleporter shipTeleporter;
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
            if (!CanUsePortableTeleporter()) return;

            int playerRadarIndex = SearchForPlayerInRadar();
            ShipTeleporter teleporter = GetShipTeleporter();

            TeleportPlayer(playerRadarIndex, ref teleporter);
        }
        private void TeleportPlayer(int playerRadarIndex, ref ShipTeleporter teleporter)
        {
            if (playerRadarIndex == -1)
            {
                //this shouldn't occur but if it does, this will teleport this client and the server targeted player.
                StartOfRound.Instance.mapScreen.targetedPlayer = playerHeldBy;
                UpgradeBus.instance.TPButtonPressed = true;
                teleporter.PressTeleportButtonOnLocalClient();
                if (Random.Range(0f, 1f) > breakChance)
                {
                    audio.PlayOneShot(ItemBreak);
                    itemUsedUp = true;
                    playerHeldBy.DespawnHeldObject();
                }
            }
            else
            {
                StartOfRound.Instance.mapScreen.SwitchRadarTargetAndSync(playerRadarIndex);
                StartCoroutine(WaitToTP(teleporter));
            }
        }
        private int SearchForPlayerInRadar()
        {
            int thisPlayersIndex = -1;
            for (int i = 0; i < StartOfRound.Instance.mapScreen.radarTargets.Count(); i++)
            {
                if (StartOfRound.Instance.mapScreen.radarTargets[i].transform.gameObject.GetComponent<PlayerControllerB>() != playerHeldBy) continue;

                thisPlayersIndex = i;
                break;
            }
            return thisPlayersIndex;
        }
        private bool CanUsePortableTeleporter()
        {
            if (!Mouse.current.leftButton.isPressed) return false;
            audio.PlayOneShot(buttonPress);
            if (itemUsedUp)
            {
                audio.PlayOneShot(error);
                return false;
            }
            ShipTeleporter teleporter = GetShipTeleporter();
            if (teleporter == null)
            {
                audio.PlayOneShot(error);
                return false;
            }
            return true;
        }
        private ShipTeleporter GetShipTeleporter()
        {
            if (shipTeleporter != null) return shipTeleporter;

            ShipTeleporter[] tele = FindObjectsOfType<ShipTeleporter>();
            ShipTeleporter NotInverseTele = null;
            foreach (ShipTeleporter shipTeleporter in tele)
            {
                if (shipTeleporter.isInverseTeleporter) continue;

                NotInverseTele = shipTeleporter;
                break;
            }
            shipTeleporter = NotInverseTele;
            return shipTeleporter;
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
