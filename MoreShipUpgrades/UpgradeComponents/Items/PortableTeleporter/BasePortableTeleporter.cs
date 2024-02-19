using GameNetcodeStuff;
using MoreShipUpgrades.Managers;
using System.Collections;
using System.Linq;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MoreShipUpgrades.UpgradeComponents.Items.PortableTeleporter
{
    /// <summary>
    /// Base class which represents an item with the ability of teleporting the player back to the ship through vanila teleport
    /// when activated
    /// </summary>
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

        /// <summary>
        /// Function called when the player activates the item in their hand through left-click
        /// </summary>
        /// <param name="used">Status of the item in terms being used or not</param>
        /// <param name="buttonDown">Pressed down the button or lifted up the press</param>
        public override void ItemActivate(bool used, bool buttonDown = true)
        {
            if (!CanUsePortableTeleporter()) return;

            int playerRadarIndex = SearchForPlayerInRadar();
            ShipTeleporter teleporter = GetShipTeleporter();

            TeleportPlayer(playerRadarIndex, ref teleporter);
        }
        /// <summary>
        /// Triggers the vanila teleport on the player holding the portable teleporter
        /// </summary>
        /// <param name="playerRadarIndex">Index of the player holding the portable teleporter</param>
        /// <param name="teleporter">Teleporter that allows to teleport players back to the ship</param>
        private void TeleportPlayer(int playerRadarIndex, ref ShipTeleporter teleporter)
        {
            if (playerRadarIndex == -1)
            {
                //this shouldn't occur but if it does, this will teleport this client and the server targeted player.
                StartOfRound.Instance.mapScreen.targetedPlayer = playerHeldBy;
                UpgradeBus.Instance.TPButtonPressed = true;
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
        /// <summary>
        /// Search for the player holding the portable teleporter's radar index used to select in the radar screen of the ship
        /// </summary>
        /// <returns>Radar index of the player holding the portable teleporter</returns>
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
        /// <summary>
        /// Checks if the player holding the portable teleporter can use it to teleport back to the ship
        /// </summary>
        /// <returns>Wether the player can use the item or not</returns>
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
        /// <summary>
        /// Searches for the vanila teleporter that allows teleporting players back to the ship
        /// </summary>
        /// <returns>Reference to the vanila teleporter if it exists, false if otherwise</returns>
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
        /// <summary>
        /// Starts the teleporting process of the player holding the portable teleporter and checks if it should break the item due to random chance
        /// </summary>
        /// <param name="tele">Vanila teleporter that allows to teleport players back to the ship</param>
        /// <returns></returns>
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

        /// <summary>
        /// Sets the status of player using the portable teleporter to true
        /// </summary>
        [ServerRpc(RequireOwnership = false)]
        public void ReqUpdateTpDropStatusServerRpc()
        {
            ChangeTPButtonPressedClientRpc();
        }

        /// <summary>
        /// Sets the status of player using the portable teleporter to true
        /// </summary>
        [ClientRpc]
        private void ChangeTPButtonPressedClientRpc()
        {
            UpgradeBus.Instance.TPButtonPressed = true;
        }
    }
}
