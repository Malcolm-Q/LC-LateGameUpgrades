using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc;
using MoreShipUpgrades.Misc.Upgrades;
using MoreShipUpgrades.UpgradeComponents.Interfaces;
using System;
using UnityEngine;

namespace MoreShipUpgrades.UpgradeComponents.TierUpgrades.AttributeUpgrades
{
    internal class RunningShoes : GameAttributeTierUpgrade, IUpgradeWorldBuilding, IPlayerSync
    {
        public const string UPGRADE_NAME = "Running Shoes";
        public static string PRICES_DEFAULT = "500,750,1000";
        internal const string WORLD_BUILDING_TEXT = "\n\nA new pair of boots {0} a whole new lease on life. In this instance," +
            " it might also result in fewer wet sock incidents and consequent trenchfoot. After all, who knows how many people have walked in {1} shoes?\n\n";
        void Awake()
        {
            upgradeName = UPGRADE_NAME;
            logger = new LguLogger(UPGRADE_NAME);
            changingAttribute = GameAttribute.PLAYER_MOVEMENT_SPEED;
            initialValue = UpgradeBus.Instance.PluginConfiguration.MOVEMENT_SPEED_UNLOCK.Value;
            incrementalValue = UpgradeBus.Instance.PluginConfiguration.MOVEMENT_INCREMENT.Value;
        }
        public static float ApplyPossibleReducedNoiseRange(float defaultValue)
        {
            if (!(GetActiveUpgrade(UPGRADE_NAME) && GetUpgradeLevel(UPGRADE_NAME) == UpgradeBus.Instance.PluginConfiguration.RUNNING_SHOES_UPGRADE_PRICES.Value.Split(',').Length)) return defaultValue;
            return Mathf.Clamp(defaultValue - UpgradeBus.Instance.PluginConfiguration.NOISE_REDUCTION.Value, 0f, defaultValue);
        }

        public string GetWorldBuildingText(bool shareStatus = false)
        {
            return string.Format(WORLD_BUILDING_TEXT, shareStatus ? "could give your crew" : "can give you", shareStatus ? "y'all's" : "your");
        }

        public override string GetDisplayInfo(int initialPrice = -1, int maxLevels = -1, int[] incrementalPrices = null)
        {
            Func<int, float> infoFunction = level => UpgradeBus.Instance.PluginConfiguration.MOVEMENT_SPEED_UNLOCK.Value + level * UpgradeBus.Instance.PluginConfiguration.MOVEMENT_INCREMENT.Value;
            string infoFormat = AssetBundleHandler.GetInfoFromJSON(UPGRADE_NAME);
            return Tools.GenerateInfoForUpgrade(infoFormat, initialPrice, incrementalPrices, infoFunction);
        }
    }
}
