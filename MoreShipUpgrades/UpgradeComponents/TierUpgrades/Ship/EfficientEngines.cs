using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc;
using MoreShipUpgrades.Misc.Upgrades;
using MoreShipUpgrades.Misc.Util;
using MoreShipUpgrades.UI.TerminalNodes;
using UnityEngine;

namespace MoreShipUpgrades.UpgradeComponents.TierUpgrades
{
    internal class EfficientEngines : TierUpgrade
    {
        internal const string UPGRADE_NAME = "Efficient Engines";
        internal const string DEFAULT_PRICES = "600, 750, 900";
        void Awake()
        {
            upgradeName = UPGRADE_NAME;
            overridenUpgradeName = GetConfiguration().EFFICIENT_ENGINES_OVERRIDE_NAME;
        }
        public override string GetDisplayInfo(int initialPrice = -1, int maxLevels = -1, int[] incrementalPrices = null)
        {
            static float infoFunction(int level)
            {
                LategameConfiguration config = GetConfiguration();
                return config.EFFICIENT_ENGINES_INITIAL_DISCOUNT.Value + (level * config.EFFICIENT_ENGINES_INCREMENTAL_DISCOUNT.Value);
            }
            const string infoFormat = "LVL {0} - ${1} - Moon routing will be {2}% cheaper\n";
            return Tools.GenerateInfoForUpgrade(infoFormat, initialPrice, incrementalPrices, infoFunction);
        }

        public static int GetDiscountedMoonPrice(int defaultPrice)
        {
            LategameConfiguration config = GetConfiguration();
            if (!config.EFFICIENT_ENGINES_ENABLED.Value) return defaultPrice;
            if (!GetActiveUpgrade(UPGRADE_NAME)) return defaultPrice;
            if (defaultPrice == 0) return defaultPrice;
            float discountedPrice = defaultPrice * (1f - ((config.EFFICIENT_ENGINES_INITIAL_DISCOUNT.Value + (GetUpgradeLevel(UPGRADE_NAME) * config.EFFICIENT_ENGINES_INCREMENTAL_DISCOUNT.Value))/100f));
            return Mathf.CeilToInt(Mathf.Clamp(discountedPrice, 0f, defaultPrice));
        }
        public override bool CanInitializeOnStart
        {
            get
            {
                LategameConfiguration config = GetConfiguration();
                string[] prices = config.EFFICIENT_ENGINES_PRICES.Value.Split(',');
                return config.EFFICIENT_ENGINES_PRICE.Value <= 0 && prices.Length == 1 && (prices[0].Length == 0 || prices[0] == "0");
            }
        }

        public new static (string, string[]) RegisterScrapToUpgrade()
        {
            return (UPGRADE_NAME, GetConfiguration().EFFICIENT_ENGINES_ITEM_PROGRESSION_ITEMS.Value.Split(","));
        }
        public new static void RegisterUpgrade()
        {
            SetupGenericPerk<EfficientEngines>(UPGRADE_NAME);
        }
        public new static CustomTerminalNode RegisterTerminalNode()
        {
            LategameConfiguration configuration = GetConfiguration();

            return UpgradeBus.Instance.SetupMultiplePurchasableTerminalNode(UPGRADE_NAME,
                                                shareStatus: true,
                                                configuration.EFFICIENT_ENGINES_ENABLED.Value,
                                                configuration.EFFICIENT_ENGINES_PRICE.Value,
                                                UpgradeBus.ParseUpgradePrices(configuration.EFFICIENT_ENGINES_PRICES.Value),
                                                configuration.OVERRIDE_UPGRADE_NAMES ? configuration.EFFICIENT_ENGINES_OVERRIDE_NAME : "");
        }
    }
}
