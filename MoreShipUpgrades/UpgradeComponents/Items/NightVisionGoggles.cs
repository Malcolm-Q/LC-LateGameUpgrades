using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc;
using MoreShipUpgrades.UpgradeComponents.Interfaces;
using MoreShipUpgrades.UpgradeComponents.TierUpgrades;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.InputSystem;

namespace MoreShipUpgrades.UpgradeComponents.Items
{
    internal class NightVisionGoggles : GrabbableObject, IItemWorldBuilding, IDisplayInfo
    {
        internal const string WORLD_BUILDING_TEXT = "Very old military surplus phosphor lens modules, retrofitted for compatibility with modern Company-issued helmets" +
            " and offered to employees of The Company on a subscription plan. The base package comes with cheap batteries, but premium subscriptions offer regular issuances" +
            " of higher-quality energy solutions, ranging from hobby grade to industrial application power banks. The modules also come with DRM that prevents the user from improvising" +
            " with other kinds of batteries.";
        public string GetDisplayInfo()
        {
            string grantStatus = UpgradeBus.instance.cfg.NIGHT_VISION_INDIVIDUAL || !UpgradeBus.instance.cfg.SHARED_UPGRADES ? "one" : "all";
            string loseOnDeath = UpgradeBus.instance.cfg.LOSE_NIGHT_VIS_ON_DEATH ? "be" : "not be";
            return string.Format(AssetBundleHandler.GetInfoFromJSON("Night Vision"), grantStatus, loseOnDeath);
        }

        public string GetWorldBuildingText()
        {
            return WORLD_BUILDING_TEXT;
        }
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
                NightVision.instance.EnableNightVisionServerRpc();
            }
            else
            {
                UpgradeBus.instance.UpgradeObjects[NightVision.UPGRADE_NAME].GetComponent<NightVision>().EnableOnClient();
            }
            playerHeldBy.DespawnHeldObject();
        }
    }
}
