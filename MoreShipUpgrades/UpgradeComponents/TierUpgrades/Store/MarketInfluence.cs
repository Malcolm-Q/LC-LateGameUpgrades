using MoreShipUpgrades.Configuration;
using MoreShipUpgrades.Configuration.Upgrades.Interfaces.TierUpgrades;
using MoreShipUpgrades.Managers;
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
            overridenUpgradeName = GetConfiguration().MarketInfluenceConfiguration.OverrideName;
            base.Start();
        }
        public static int GetGuaranteedPercentageSale(int defaultPercentage, int maxValue)
        {
            ITierEffectUpgradeConfiguration<int> config = GetConfiguration().MarketInfluenceConfiguration;
            if (!config.Enabled) return defaultPercentage;
            if (!GetActiveUpgrade(UPGRADE_NAME)) return defaultPercentage;
            return Mathf.Clamp(defaultPercentage + config.InitialEffect.Value + (GetUpgradeLevel(UPGRADE_NAME) * config.IncrementalEffect.Value), 0, maxValue);
        }
        public override string GetDisplayInfo(int initialPrice = -1, int maxLevels = -1, int[] incrementalPrices = null)
        {
            static float infoFunction(int level)
            {
                ITierEffectUpgradeConfiguration<int> config = GetConfiguration().MarketInfluenceConfiguration;
                return config.InitialEffect.Value + (level * config.IncrementalEffect.Value);
            }
            const string infoFormat = "LVL {0} - {1} - Guarantees the item sales' percentage to be at least {2}%\n";
            return Tools.GenerateInfoForUpgrade(infoFormat, initialPrice, incrementalPrices, infoFunction, purchaseMode: GetConfiguration().MarketInfluenceConfiguration.PurchaseMode);
        }
        public override bool CanInitializeOnStart
        {
            get
            {
                ITierEffectUpgradeConfiguration<int> upgradeConfig = GetConfiguration().MarketInfluenceConfiguration;
                string[] prices = upgradeConfig.Prices.Value.Split(',');
                return prices.Length == 0 || (prices.Length == 1 && (prices[0].Length == 0 || prices[0] == "0"));
            }
        }

        public new static (string, string[]) RegisterScrapToUpgrade()
        {
            return (UPGRADE_NAME, GetConfiguration().MarketInfluenceConfiguration.ItemProgressionItems.Value.Split(","));
        }
        public new static void RegisterUpgrade()
        {
            SetupGenericPerk<MarketInfluence>(UPGRADE_NAME);
        }
        public new static CustomTerminalNode RegisterTerminalNode()
        {
            return UpgradeBus.Instance.SetupMultiplePurchaseableTerminalNode(UPGRADE_NAME, GetConfiguration().MarketInfluenceConfiguration);
        }
    }
}
