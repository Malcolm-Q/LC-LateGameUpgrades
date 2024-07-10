using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc;
using MoreShipUpgrades.Misc.TerminalNodes;
using MoreShipUpgrades.Misc.Upgrades;
using MoreShipUpgrades.Misc.Util;
using MoreShipUpgrades.UpgradeComponents.Interfaces;
using UnityEngine;

namespace MoreShipUpgrades.UpgradeComponents.TierUpgrades.Store
{
    internal class MarketInfluence : TierUpgrade
    {
        internal const string UPGRADE_NAME = "Market Influence";
        internal const string PRICES_DEFAULT = "200,350,500";

        internal override void Start()
        {
            upgradeName = UPGRADE_NAME;
            overridenUpgradeName = UpgradeBus.Instance.PluginConfiguration.MARKET_INFLUENCE_OVERRIDE_NAME;
            base.Start();
        }
        public static int GetGuaranteedPercentageSale(int defaultPercentage, int maxValue)
        {
            if (!GetActiveUpgrade(UPGRADE_NAME)) return defaultPercentage;
            return Mathf.Clamp(defaultPercentage + UpgradeBus.Instance.PluginConfiguration.MARKET_INFLUENCE_INITIAL_PERCENTAGE.Value + GetUpgradeLevel(UPGRADE_NAME) * UpgradeBus.Instance.PluginConfiguration.MARKET_INFLUENCE_INCREMENTAL_PERCENTAGE.Value, 0, maxValue);
        }
        public override string GetDisplayInfo(int initialPrice = -1, int maxLevels = -1, int[] incrementalPrices = null)
        {
            System.Func<int, float> infoFunction = level => UpgradeBus.Instance.PluginConfiguration.MARKET_INFLUENCE_INITIAL_PERCENTAGE.Value + level * UpgradeBus.Instance.PluginConfiguration.MARKET_INFLUENCE_INCREMENTAL_PERCENTAGE.Value;
            string infoFormat = "LVL {0} - ${1} - Guarantees the item sales' percentage to be at least {2}%\n";
            return Tools.GenerateInfoForUpgrade(infoFormat, initialPrice, incrementalPrices, infoFunction);
        }
        public override bool CanInitializeOnStart
        {
            get
            {
                string[] prices = UpgradeBus.Instance.PluginConfiguration.MARKET_INFLUENCE_PRICES.Value.Split(',');
                bool free = UpgradeBus.Instance.PluginConfiguration.MARKET_INFLUENCE_PRICE.Value <= 0 && prices.Length == 1 && (prices[0] == "" || prices[0] == "0");
                return free;
            }
        }

        public new static (string, string[]) RegisterScrapToUpgrade()
        {
            return (UPGRADE_NAME, UpgradeBus.Instance.PluginConfiguration.MARKET_INFLUENCE_ITEM_PROGRESSION_ITEMS.Value.Split(","));
        }
        public new static void RegisterUpgrade()
        {
            SetupGenericPerk<MarketInfluence>(UPGRADE_NAME);
        }
        public new static CustomTerminalNode RegisterTerminalNode()
        {
            LategameConfiguration configuration = UpgradeBus.Instance.PluginConfiguration;

            return UpgradeBus.Instance.SetupMultiplePurchasableTerminalNode(UPGRADE_NAME,
                                                shareStatus: true,
                                                configuration.MARKET_INFLUENCE_ENABLED.Value,
                                                configuration.MARKET_INFLUENCE_PRICE.Value,
                                                UpgradeBus.ParseUpgradePrices(configuration.MARKET_INFLUENCE_PRICES.Value),
                                                configuration.OVERRIDE_UPGRADE_NAMES ? configuration.MARKET_INFLUENCE_OVERRIDE_NAME : "");
        }
    }
}
