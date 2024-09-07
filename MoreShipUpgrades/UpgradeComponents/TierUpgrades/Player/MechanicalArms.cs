using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc.TerminalNodes;
using MoreShipUpgrades.Misc;
using MoreShipUpgrades.Misc.Upgrades;
using MoreShipUpgrades.Misc.Util;
using UnityEngine;

namespace MoreShipUpgrades.UpgradeComponents.TierUpgrades.Player
{
    internal class MechanicalArms : TierUpgrade
    {
        internal const string UPGRADE_NAME = "Mechanical Arms";
        internal const string PRICES_DEFAULT = "400,600,800";

        internal override void Start()
        {
            upgradeName = UPGRADE_NAME;
            overridenUpgradeName = UpgradeBus.Instance.PluginConfiguration.MECHANICAL_ARMS_OVERRIDE_NAME;
            base.Start();
        }
        public static float GetIncreasedGrabDistance(float defaultValue)
        {
            if (!UpgradeBus.Instance.PluginConfiguration.MECHANICAL_ARMS_ENABLED) return defaultValue;
            if (!GetActiveUpgrade(UPGRADE_NAME)) return defaultValue;
            float increasedRange = UpgradeBus.Instance.PluginConfiguration.MECHANICAL_ARMS_INITIAL_RANGE_INCREASE + (GetUpgradeLevel(UPGRADE_NAME) * UpgradeBus.Instance.PluginConfiguration.MECHANICAL_ARMS_INCREMENTAL_RANGE_INCREASE);
            return Mathf.Clamp(defaultValue + increasedRange, defaultValue, float.MaxValue);
        }
        public override string GetDisplayInfo(int initialPrice = -1, int maxLevels = -1, int[] incrementalPrices = null)
        {
            static float infoFunction(int level) => UpgradeBus.Instance.PluginConfiguration.MECHANICAL_ARMS_INITIAL_RANGE_INCREASE.Value + (level * UpgradeBus.Instance.PluginConfiguration.MECHANICAL_ARMS_INCREMENTAL_RANGE_INCREASE.Value);
            const string infoFormat = "LVL {0} - ${1} - Increases the player's interaction range by {2} units.\n";
            return Tools.GenerateInfoForUpgrade(infoFormat, initialPrice, incrementalPrices, infoFunction);
        }
        public override bool CanInitializeOnStart
        {
            get
            {
                string[] prices = UpgradeBus.Instance.PluginConfiguration.MECHANICAL_ARMS_PRICES.Value.Split(',');
                return UpgradeBus.Instance.PluginConfiguration.MECHANICAL_ARMS_PRICE.Value <= 0 && prices.Length == 1 && (prices[0].Length == 0 || prices[0] == "0");
            }
        }

        public new static (string, string[]) RegisterScrapToUpgrade()
        {
            return (UPGRADE_NAME, UpgradeBus.Instance.PluginConfiguration.MECHANICAL_ARMS_ITEM_PROGRESSION_ITEMS.Value.Split(","));
        }
        public new static void RegisterUpgrade()
        {
            SetupGenericPerk<MechanicalArms>(UPGRADE_NAME);
        }
        public new static CustomTerminalNode RegisterTerminalNode()
        {
            LategameConfiguration configuration = UpgradeBus.Instance.PluginConfiguration;

            return UpgradeBus.Instance.SetupMultiplePurchasableTerminalNode(UPGRADE_NAME,
                                                shareStatus: configuration.SHARED_UPGRADES || !configuration.MECHANICAL_ARMS_INDIVIDUAL,
                                                configuration.MECHANICAL_ARMS_ENABLED.Value,
                                                configuration.MECHANICAL_ARMS_PRICE.Value,
                                                UpgradeBus.ParseUpgradePrices(configuration.MECHANICAL_ARMS_PRICES.Value),
                                                configuration.OVERRIDE_UPGRADE_NAMES ? configuration.MECHANICAL_ARMS_OVERRIDE_NAME : "");
        }
    }
}
