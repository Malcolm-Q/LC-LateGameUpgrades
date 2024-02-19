﻿using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc;
using MoreShipUpgrades.Misc.Upgrades;
using MoreShipUpgrades.UpgradeComponents.Interfaces;
using MoreShipUpgrades.UpgradeComponents.TierUpgrades;

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
            string grantStatus = UpgradeBus.Instance.PluginConfiguration.NIGHT_VISION_INDIVIDUAL.Value || !UpgradeBus.Instance.PluginConfiguration.SHARED_UPGRADES.Value ? "one" : "all";
            string loseOnDeath = UpgradeBus.Instance.PluginConfiguration.LOSE_NIGHT_VIS_ON_DEATH.Value ? "be" : "not be";
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
            if (BaseUpgrade.GetActiveUpgrade(NightVision.UPGRADE_NAME))
            {
                HUDManager.Instance.chatText.text += "<color=#FF0000>Night vision is already active!</color>";
                return;
            }
            if (UpgradeBus.Instance.IndividualUpgrades[NightVision.UPGRADE_NAME])
            {
                NightVision.Instance.EnableNightVisionServerRpc();
            }
            else
            {
                UpgradeBus.Instance.UpgradeObjects[NightVision.UPGRADE_NAME].GetComponent<NightVision>().EnableOnClient();
            }
            playerHeldBy.DespawnHeldObject();
        }
    }
}
