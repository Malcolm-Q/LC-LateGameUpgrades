using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc.TerminalNodes;
using MoreShipUpgrades.Misc;
using MoreShipUpgrades.Misc.Upgrades;
using MoreShipUpgrades.Misc.Util;
using UnityEngine;

namespace MoreShipUpgrades.UpgradeComponents.TierUpgrades.Vehicle
{
    internal class TurboTank : TierUpgrade
    {
        internal const string UPGRADE_NAME = "Turbo Tank";
        internal const string PRICES_DEFAULT = "100,200,300";
        internal override void Start()
        {
            upgradeName = UPGRADE_NAME;
            overridenUpgradeName = UpgradeBus.Instance.PluginConfiguration.TURBO_TANK_OVERRIDE_NAME;
            base.Start();
        }
        public static int ComputeAdditionalTurboCapacity()
        {
            return UpgradeBus.Instance.PluginConfiguration.TURBO_TANK_CAPACITY_INITIAL_INCREASE + (GetUpgradeLevel(UPGRADE_NAME) * UpgradeBus.Instance.PluginConfiguration.TURBO_TANK_CAPACITY_INCREMENTAL_INCREASE);
        }
        public static int GetAdditionalTurboCapacity(int defaultValue)
        {
            if (!UpgradeBus.Instance.PluginConfiguration.TURBO_TANK_ENABLED) return defaultValue;
            if (!GetActiveUpgrade(UPGRADE_NAME)) return defaultValue;
            int additionalValue = ComputeAdditionalTurboCapacity();
            return Mathf.Clamp(defaultValue + additionalValue, defaultValue, int.MaxValue);
        }
        public static float GetAdditionalTurboCapacity(float defaultValue)
        {
            if (!UpgradeBus.Instance.PluginConfiguration.TURBO_TANK_ENABLED) return defaultValue;
            if (!GetActiveUpgrade(UPGRADE_NAME)) return defaultValue;
            float additionalValue = ComputeAdditionalTurboCapacity();
            return Mathf.Clamp(defaultValue + additionalValue, defaultValue, float.MaxValue);
        }
        public override bool CanInitializeOnStart
        {
            get
            {
                string[] prices = UpgradeBus.Instance.PluginConfiguration.TURBO_TANK_PRICES.Value.Split(',');
                return UpgradeBus.Instance.PluginConfiguration.TURBO_TANK_PRICE.Value <= 0 && prices.Length == 1 && (prices[0] == "" || prices[0] == "0");
            }
        }
        public override string GetDisplayInfo(int initialPrice = -1, int maxLevels = -1, int[] incrementalPrices = null)
        {
            System.Func<int, float> infoFunction = level => UpgradeBus.Instance.PluginConfiguration.TURBO_TANK_CAPACITY_INITIAL_INCREASE.Value + (level * UpgradeBus.Instance.PluginConfiguration.TURBO_TANK_CAPACITY_INCREMENTAL_INCREASE.Value);
            const string infoFormat = "LVL {0} - ${1} - Company Cruiser vehicle's maximum turbo capacity is increased by {2}.\n";
            return Tools.GenerateInfoForUpgrade(infoFormat, initialPrice, incrementalPrices, infoFunction);
        }
        public new static (string, string[]) RegisterScrapToUpgrade()
        {
            return (UPGRADE_NAME, UpgradeBus.Instance.PluginConfiguration.TURBO_TANK_ITEM_PROGRESSION_ITEMS.Value.Split(","));
        }
        public new static void RegisterUpgrade()
        {
            SetupGenericPerk<TurboTank>(UPGRADE_NAME);
        }
        public new static CustomTerminalNode RegisterTerminalNode()
        {
            LategameConfiguration configuration = UpgradeBus.Instance.PluginConfiguration;

            return UpgradeBus.Instance.SetupMultiplePurchasableTerminalNode(UPGRADE_NAME,
                                                shareStatus: true,
                                                configuration.TURBO_TANK_ENABLED.Value,
                                                configuration.TURBO_TANK_PRICE.Value,
                                                UpgradeBus.ParseUpgradePrices(configuration.TURBO_TANK_PRICES.Value),
                                                configuration.OVERRIDE_UPGRADE_NAMES ? configuration.TURBO_TANK_OVERRIDE_NAME : "");
        }
    }
}
