﻿using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc;
using MoreShipUpgrades.Misc.Upgrades;
using MoreShipUpgrades.Misc.Util;
using MoreShipUpgrades.UpgradeComponents.Interfaces;

namespace MoreShipUpgrades.UpgradeComponents.TierUpgrades.AttributeUpgrades
{
    internal class StrongLegs : GameAttributeTierUpgrade, IUpgradeWorldBuilding, IPlayerSync
    {
        public const string UPGRADE_NAME = "Strong Legs";
        public static string PRICES_DEFAULT = "150,190,250";
        internal const string WORLD_BUILDING_TEXT = "\n\nOne-time issuance of {0}." +
            " Comes with a vague list of opt-in maintenance procedures offered by The Company, which includes such gems as 'actuation optimization'," +
            " 'weight & balance personalization', and similar nigh-meaningless corpo-tech jargon. All of it is expensive.\n\n";
        void Awake()
        {
            upgradeName = UPGRADE_NAME;
            logger = new LguLogger(UPGRADE_NAME);
            changingAttribute = GameAttribute.PLAYER_JUMP_FORCE;
            initialValue = UpgradeBus.Instance.PluginConfiguration.JUMP_FORCE_UNLOCK.Value;
            incrementalValue = UpgradeBus.Instance.PluginConfiguration.JUMP_FORCE_INCREMENT.Value;
        }
        public static int ReduceFallDamage(int defaultValue)
        {
            if (!(GetActiveUpgrade(UPGRADE_NAME) && GetUpgradeLevel(UPGRADE_NAME) == UpgradeBus.Instance.PluginConfiguration.STRONG_LEGS_UPGRADE_PRICES.Value.Split(',').Length)) return defaultValue;
            return (int)(defaultValue * (1.0f - UpgradeBus.Instance.PluginConfiguration.STRONG_LEGS_REDUCE_FALL_DAMAGE_MULTIPLIER.Value));
        }
        public string GetWorldBuildingText(bool shareStatus = false)
        {
            return string.Format(WORLD_BUILDING_TEXT, shareStatus ? "proprietary pressure-assisted kneebraces to your crew" : "a proprietary pressure-assisted kneebrace");
        }

        public override string GetDisplayInfo(int initialPrice = -1, int maxLevels = -1, int[] incrementalPrices = null)
        {
            System.Func<int, float> infoFunction = level => UpgradeBus.Instance.PluginConfiguration.JUMP_FORCE_UNLOCK.Value + level * UpgradeBus.Instance.PluginConfiguration.JUMP_FORCE_INCREMENT.Value;
            string infoFormat = AssetBundleHandler.GetInfoFromJSON(UPGRADE_NAME);
            return Tools.GenerateInfoForUpgrade(infoFormat, initialPrice, incrementalPrices, infoFunction);
        }
        internal override bool CanInitializeOnStart()
        {
            string[] prices = UpgradeBus.Instance.PluginConfiguration.STRONG_LEGS_UPGRADE_PRICES.Value.Split(',');
            bool free = UpgradeBus.Instance.PluginConfiguration.STRONG_LEGS_PRICE.Value <= 0 && prices.Length == 1 && (prices[0] == "" || prices[0] == "0");
            return free;
        }
    }
}
