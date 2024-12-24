using MoreShipUpgrades.Configuration;
using MoreShipUpgrades.Configuration.Interfaces;
using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc.Upgrades;
using MoreShipUpgrades.Misc.Util;
using MoreShipUpgrades.UI.TerminalNodes;
using MoreShipUpgrades.UpgradeComponents.Interfaces;

namespace MoreShipUpgrades.UpgradeComponents.TierUpgrades.Store
{
    class BargainConnections : TierUpgrade, IUpgradeWorldBuilding
    {
        internal const string UPGRADE_NAME = "Bargain Connections";
        internal const string PRICES_DEFAULT = "200,225,300,375";
        internal const string WORLD_BUILDING_TEXT = "\n\nSubscription to 'Coupon Cutters' magazine. Every once in a while the magazine comes with coupons already cut out from it. Strange.\n\n";
        internal override void Start()
        {
            upgradeName = UPGRADE_NAME;
            overridenUpgradeName = GetConfiguration().BargainConnectionsConfiguration.OverrideName;
            base.Start();
        }
        public static int GetBargainConnectionsAdditionalItems(int defaultAmountItems)
        {
            ITierEffectUpgradeConfiguration<int> config = GetConfiguration().BargainConnectionsConfiguration;
            if (!GetActiveUpgrade(UPGRADE_NAME)) return defaultAmountItems;
            return defaultAmountItems + config.InitialEffect.Value + (GetUpgradeLevel(UPGRADE_NAME) * config.IncrementalEffect.Value);
        }
        public override string GetDisplayInfo(int initialPrice = -1, int maxLevels = -1, int[] incrementalPrices = null)
        {
            static float infoFunction(int level)
            {
                ITierEffectUpgradeConfiguration<int> config = GetConfiguration().BargainConnectionsConfiguration;
                return config.InitialEffect.Value + (level * config.IncrementalEffect.Value);
            }
            const string infoFormat = "LVL {0} - ${1} - Increases the amount of items that can be on sale by {2}\n";
            return Tools.GenerateInfoForUpgrade(infoFormat, initialPrice, incrementalPrices, infoFunction);
        }
        public override bool CanInitializeOnStart
        {
            get
            {
                ITierEffectUpgradeConfiguration<int> upgradeConfig = GetConfiguration().BargainConnectionsConfiguration;
                string[] prices = upgradeConfig.Prices.Value.Split(',');
                return prices.Length == 0 || (prices.Length == 1 && (prices[0].Length == 0 || prices[0] == "0"));
            }
        }
        public new static (string, string[]) RegisterScrapToUpgrade()
        {
            return (UPGRADE_NAME, GetConfiguration().BargainConnectionsConfiguration.ItemProgressionItems.Value.Split(","));
        }
        public new static void RegisterUpgrade()
        {
            SetupGenericPerk<BargainConnections>(UPGRADE_NAME);
        }
        public new static CustomTerminalNode RegisterTerminalNode()
        {
            return UpgradeBus.Instance.SetupMultiplePurchaseableTerminalNode(UPGRADE_NAME,GetConfiguration().BargainConnectionsConfiguration);
        }

        public string GetWorldBuildingText(bool shareStatus = false)
        {
            return WORLD_BUILDING_TEXT;
        }
    }
}
