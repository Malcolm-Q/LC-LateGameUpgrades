using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc;
using MoreShipUpgrades.Misc.Upgrades;
using MoreShipUpgrades.Misc.Util;
using MoreShipUpgrades.UI.TerminalNodes;
using MoreShipUpgrades.UpgradeComponents.Interfaces;
using UnityEngine;

namespace MoreShipUpgrades.UpgradeComponents.TierUpgrades.Store
{
    internal class MarketInfluence : TierUpgrade, IUpgradeWorldBuilding
    {
        internal const string UPGRADE_NAME = "Market Influence";
        internal const string PRICES_DEFAULT = "200,350,500";
        internal const string WORLD_BUILDING_TEXT = "\n\nBy investing scrip into the Company's subsidiaries that produce your equipment instead of buying the equipment itself," +
            " you are sometimes awarded coupons in the mail for various Company Store offerings.\n\n";

        public string GetWorldBuildingText(bool shareStatus = false)
        {
            return WORLD_BUILDING_TEXT;
        }
        internal override void Start()
        {
            upgradeName = UPGRADE_NAME;
            overridenUpgradeName = GetConfiguration().MARKET_INFLUENCE_OVERRIDE_NAME;
            base.Start();
        }
        public static int GetGuaranteedPercentageSale(int defaultPercentage, int maxValue)
        {
            LategameConfiguration config = GetConfiguration();
            if (!config.MARKET_INFLUENCE_ENABLED) return defaultPercentage;
            if (!GetActiveUpgrade(UPGRADE_NAME)) return defaultPercentage;
            return Mathf.Clamp(defaultPercentage + config.MARKET_INFLUENCE_INITIAL_PERCENTAGE.Value + (GetUpgradeLevel(UPGRADE_NAME) * config.MARKET_INFLUENCE_INCREMENTAL_PERCENTAGE.Value), 0, maxValue);
        }
        public override string GetDisplayInfo(int initialPrice = -1, int maxLevels = -1, int[] incrementalPrices = null)
        {
            static float infoFunction(int level)
            {
                LategameConfiguration config = GetConfiguration();
                return config.MARKET_INFLUENCE_INITIAL_PERCENTAGE.Value + (level * config.MARKET_INFLUENCE_INCREMENTAL_PERCENTAGE.Value);
            }
            const string infoFormat = "LVL {0} - ${1} - Guarantees the item sales' percentage to be at least {2}%\n";
            return Tools.GenerateInfoForUpgrade(infoFormat, initialPrice, incrementalPrices, infoFunction);
        }
        public override bool CanInitializeOnStart
        {
            get
            {
                LategameConfiguration config = GetConfiguration();
                string[] prices = config.MARKET_INFLUENCE_PRICES.Value.Split(',');
                return config.MARKET_INFLUENCE_PRICE.Value <= 0 && prices.Length == 1 && (prices[0].Length == 0 || prices[0] == "0");
            }
        }

        public new static (string, string[]) RegisterScrapToUpgrade()
        {
            return (UPGRADE_NAME, GetConfiguration().MARKET_INFLUENCE_ITEM_PROGRESSION_ITEMS.Value.Split(","));
        }
        public new static void RegisterUpgrade()
        {
            SetupGenericPerk<MarketInfluence>(UPGRADE_NAME);
        }
        public new static CustomTerminalNode RegisterTerminalNode()
        {
            LategameConfiguration configuration = GetConfiguration();

            return UpgradeBus.Instance.SetupMultiplePurchasableTerminalNode(UPGRADE_NAME,
                                                shareStatus: true,
                                                configuration.MARKET_INFLUENCE_ENABLED.Value,
                                                configuration.MARKET_INFLUENCE_PRICE.Value,
                                                UpgradeBus.ParseUpgradePrices(configuration.MARKET_INFLUENCE_PRICES.Value),
                                                configuration.OVERRIDE_UPGRADE_NAMES ? configuration.MARKET_INFLUENCE_OVERRIDE_NAME : "");
        }
    }
}
