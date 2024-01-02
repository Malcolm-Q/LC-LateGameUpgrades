using GameNetcodeStuff;
using MoreShipUpgrades.Managers;
using System.Collections;
using System.Linq;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

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
        }

        public override void DiscardItem()
        {
            this.playerHeldBy.activatingItem = false;
            base.DiscardItem();
        }

        public override void ItemActivate(bool used, bool buttonDown = true)
        {
            base.ItemActivate(used, buttonDown);
            if (!Mouse.current.leftButton.isPressed) return;

            audio.PlayOneShot(buttonPress);
            if (itemUsedUp) { audio.PlayOneShot(error); return; }

            ShipTeleporter[] tele = GameObject.FindObjectsOfType<ShipTeleporter>();
            ShipTeleporter NotInverseTele = null;
            foreach (ShipTeleporter shipTeleporter in tele)
            {
                if (shipTeleporter.isInverseTeleporter) continue;

                NotInverseTele = shipTeleporter;
                break;
            }
            if (!NotInverseTele){ audio.PlayOneShot(error); return; }

            int thisPlayersIndex = -1;
            for (int i = 0; i < StartOfRound.Instance.mapScreen.radarTargets.Count(); i++)
            {
                if (StartOfRound.Instance.mapScreen.radarTargets[i].transform.gameObject.GetComponent<PlayerControllerB>() != playerHeldBy) continue;
                thisPlayersIndex = i;
                break;
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

        private IEnumerator WaitToTP(ShipTeleporter tele)
        {
            yield return new WaitForSeconds(0.15f);
            if(UpgradeBus.instance.cfg.ADV_KEEP_ITEMS_ON_TELE) ReqUpdateTpDropStatusServerRpc();
            tele.PressTeleportButtonOnLocalClient();
            if (UnityEngine.Random.Range(0f, 1f) < UpgradeBus.instance.cfg.ADV_CHANCE_TO_BREAK) // 0.1f
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
