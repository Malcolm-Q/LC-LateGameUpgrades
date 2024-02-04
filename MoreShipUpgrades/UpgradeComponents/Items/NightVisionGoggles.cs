using MoreShipUpgrades.Managers;
using MoreShipUpgrades.UpgradeComponents.TierUpgrades;
using UnityEngine.InputSystem;

namespace MoreShipUpgrades.UpgradeComponents.Items
{
    internal class NightVisionGoggles : GrabbableObject
    {
        public override void DiscardItem()
        {
            playerHeldBy.activatingItem = false;
            base.DiscardItem();
        }

        public override void ItemActivate(bool used, bool buttonDown = true)
        {
            base.ItemActivate(used, buttonDown);
            if (!buttonDown) return;
            if (UpgradeBus.instance.nightVision)
            {
                HUDManager.Instance.chatText.text += "<color=#FF0000>Night vision is already active!</color>";
                return;
            }
            if (UpgradeBus.instance.IndividualUpgrades[NightVision.UPGRADE_NAME])
            {
                LGUStore.instance.EnableNightVisionServerRpc();
            }
            else
            {
                UpgradeBus.instance.UpgradeObjects[NightVision.UPGRADE_NAME].GetComponent<NightVision>().EnableOnClient();
            }
            playerHeldBy.DespawnHeldObject();
        }
    }
}
