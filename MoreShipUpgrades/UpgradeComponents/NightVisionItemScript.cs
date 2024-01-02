using MoreShipUpgrades.Managers;
using UnityEngine.InputSystem;

namespace MoreShipUpgrades.UpgradeComponents
{
    internal class NightVisionItemScript : GrabbableObject
    {
        public override void DiscardItem()
        {
            this.playerHeldBy.activatingItem = false;
            base.DiscardItem();
        }

        public override void ItemActivate(bool used, bool buttonDown = true)
        {
            base.ItemActivate(used, buttonDown);
            if (!Mouse.current.leftButton.isPressed) return;
            if (UpgradeBus.instance.nightVision) 
            {
                HUDManager.Instance.chatText.text += "<color=#FF0000>Night vision is already active!</color>";
                return;
            }
            if (!UpgradeBus.instance.IndividualUpgrades[nightVisionScript.UPGRADE_NAME])
            {
                LGUStore.instance.EnableNightVisionServerRpc();
            }
            else
            {
                UpgradeBus.instance.UpgradeObjects[nightVisionScript.UPGRADE_NAME].GetComponent<nightVisionScript>().EnableOnClient();
            }
            playerHeldBy.DespawnHeldObject();
        }
    }
}
