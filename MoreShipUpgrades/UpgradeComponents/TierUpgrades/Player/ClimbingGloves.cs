using LCVR;
using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc;
using MoreShipUpgrades.Misc.TerminalNodes;
using MoreShipUpgrades.Misc.Upgrades;
using MoreShipUpgrades.Misc.Util;
using UnityEngine;

namespace MoreShipUpgrades.UpgradeComponents.TierUpgrades
{
    internal class ClimbingGloves : TierUpgrade
    {
        internal const string UPGRADE_NAME = "Climbing Gloves";
        internal const string DEFAULT_PRICES = "200,250,300";
        void Awake()
        {
            upgradeName = UPGRADE_NAME;
            overridenUpgradeName = UpgradeBus.Instance.PluginConfiguration.CLIMBING_GLOVES_OVERRIDE_NAME;
        }

        public override string GetDisplayInfo(int initialPrice = -1, int maxLevels = -1, int[] incrementalPrices = null)
        {
            static float infoFunction(int level)
            {
                LategameConfiguration config = GetConfiguration();
                return config.INITIAL_CLIMBING_SPEED_BOOST.Value + (level * config.INCREMENTAL_CLIMBING_SPEED_BOOST.Value);
            }
            const string infoFormat = "LVL {0} - ${1} - Increases the speed of climbing ladders by {2} units.\n";
            return Tools.GenerateInfoForUpgrade(infoFormat, initialPrice, incrementalPrices, infoFunction);
        }
        public override bool CanInitializeOnStart
        {
            get
            {
                LategameConfiguration config = GetConfiguration();
                string[] prices = config.CLIMBING_GLOVES_PRICES.Value.Split(',');
                return config.CLIMBING_GLOVES_PRICE.Value <= 0 && prices.Length == 1 && (prices[0].Length == 0 || prices[0] == "0");
            }
        }
        public static float GetAdditionalClimbingSpeed(float defaultValue)
        {
            LategameConfiguration config = GetConfiguration();
            if (!config.CLIMBING_GLOVES_ENABLED) return defaultValue;
            if (!GetActiveUpgrade(UPGRADE_NAME)) return defaultValue;
            float additionalValue = config.INITIAL_CLIMBING_SPEED_BOOST + (GetUpgradeLevel(UPGRADE_NAME) * config.INCREMENTAL_CLIMBING_SPEED_BOOST);
            return Mathf.Clamp(defaultValue + additionalValue, defaultValue, float.MaxValue);
        }

        public new static (string, string[]) RegisterScrapToUpgrade()
        {
            return (UPGRADE_NAME, GetConfiguration().CLIMBING_GLOVES_ITEM_PROGRESSION_ITEMS.Value.Split(","));
        }
        public new static void RegisterUpgrade()
        {
            SetupGenericPerk<ClimbingGloves>(UPGRADE_NAME);
        }
        public new static CustomTerminalNode RegisterTerminalNode()
        {
            LategameConfiguration configuration = GetConfiguration();

            return UpgradeBus.Instance.SetupMultiplePurchasableTerminalNode(UPGRADE_NAME,
                                                configuration.SHARED_UPGRADES.Value || !configuration.CLIMBING_GLOVES_INDIVIDUAL.Value,
                                                configuration.CLIMBING_GLOVES_ENABLED.Value,
                                                configuration.CLIMBING_GLOVES_PRICE.Value,
                                                UpgradeBus.ParseUpgradePrices(configuration.CLIMBING_GLOVES_PRICES.Value),
                                                configuration.OVERRIDE_UPGRADE_NAMES ? configuration.CLIMBING_GLOVES_OVERRIDE_NAME : "");
        }
    }
}
