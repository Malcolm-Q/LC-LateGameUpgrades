using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc.Upgrades;
using MoreShipUpgrades.Misc.Util;
using UnityEngine;

namespace MoreShipUpgrades.UpgradeComponents.TierUpgrades
{
    internal class EfficientEngines : TierUpgrade
    {
        internal const string UPGRADE_NAME = "Efficient Engines";
        internal const string DEFAULT_PRICES = "600, 750, 900";
        void Awake()
        {
            upgradeName = UpgradeBus.Instance.PluginConfiguration.OVERRIDE_UPGRADE_NAMES ? UpgradeBus.Instance.PluginConfiguration.EFFICIENT_ENGINES_OVERRIDE_NAME : UPGRADE_NAME;
        }
        public override string GetDisplayInfo(int initialPrice = -1, int maxLevels = -1, int[] incrementalPrices = null)
        {
            System.Func<int, float> infoFunction = level => UpgradeBus.Instance.PluginConfiguration.EFFICIENT_ENGINES_INITIAL_DISCOUNT.Value + (level * UpgradeBus.Instance.PluginConfiguration.EFFICIENT_ENGINES_INCREMENTAL_DISCOUNT.Value);
            string infoFormat = "LVL {0} - ${1} - Moon routing will be {2}% cheaper\n";
            return Tools.GenerateInfoForUpgrade(infoFormat, initialPrice, incrementalPrices, infoFunction);
        }

        public static int GetDiscountedMoonPrice(int defaultPrice)
        {
            if (!UpgradeBus.Instance.PluginConfiguration.EFFICIENT_ENGINES_ENABLED.Value) return defaultPrice;
            if (!GetActiveUpgrade(UPGRADE_NAME)) return defaultPrice;
            if (defaultPrice == 0) return defaultPrice;
            float discountedPrice = defaultPrice * (1f - ((UpgradeBus.Instance.PluginConfiguration.EFFICIENT_ENGINES_INITIAL_DISCOUNT.Value + (GetUpgradeLevel(UPGRADE_NAME) * UpgradeBus.Instance.PluginConfiguration.EFFICIENT_ENGINES_INCREMENTAL_DISCOUNT.Value))/100f));
            return Mathf.CeilToInt(Mathf.Clamp(discountedPrice, 0f, defaultPrice));
        }
        internal override bool CanInitializeOnStart()
        {
            string[] prices = UpgradeBus.Instance.PluginConfiguration.EFFICIENT_ENGINES_PRICES.Value.Split(',');
            bool free = UpgradeBus.Instance.PluginConfiguration.EFFICIENT_ENGINES_PRICE.Value <= 0 && prices.Length == 1 && (prices[0] == "" || prices[0] == "0");
            return free;
        }
    }
}
