using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc.Upgrades;
using MoreShipUpgrades.Misc.Util;

namespace MoreShipUpgrades.UpgradeComponents.TierUpgrades
{
    class BargainConnections : TierUpgrade
    {
        internal const string UPGRADE_NAME = "Bargain Connections";
        internal const string PRICES_DEFAULT = "225,300,375";
        internal override void Start()
        {
            upgradeName = UpgradeBus.Instance.PluginConfiguration.OVERRIDE_UPGRADE_NAMES ? UpgradeBus.Instance.PluginConfiguration.BARGAIN_CONNECTIONS_OVERRIDE_NAME : UPGRADE_NAME;
            base.Start();
        }
        public static int GetBargainConnectionsAdditionalItems(int defaultAmountItems)
        {
            if (!GetActiveUpgrade(UPGRADE_NAME)) return defaultAmountItems;
            return defaultAmountItems + UpgradeBus.Instance.PluginConfiguration.BARGAIN_CONNECTIONS_INITIAL_ITEM_AMOUNT.Value + (GetUpgradeLevel(UPGRADE_NAME) * UpgradeBus.Instance.PluginConfiguration.BARGAIN_CONNECTIONS_INCREMENTAL_ITEM_AMOUNT.Value);
        }
        public override string GetDisplayInfo(int initialPrice = -1, int maxLevels = -1, int[] incrementalPrices = null)
        {
            System.Func<int, float> infoFunction = level => UpgradeBus.Instance.PluginConfiguration.BARGAIN_CONNECTIONS_INITIAL_ITEM_AMOUNT.Value + (level * UpgradeBus.Instance.PluginConfiguration.BARGAIN_CONNECTIONS_INCREMENTAL_ITEM_AMOUNT.Value);
            string infoFormat = "LVL {0} - ${1} - Increases the amount of items that can be on sale by {2}\n";
            return Tools.GenerateInfoForUpgrade(infoFormat, initialPrice, incrementalPrices, infoFunction);
        }
        internal override bool CanInitializeOnStart()
        {
            string[] prices = UpgradeBus.Instance.PluginConfiguration.BARGAIN_CONNECTIONS_PRICES.Value.Split(',');
            bool free = UpgradeBus.Instance.PluginConfiguration.BARGAIN_CONNECTIONS_PRICE.Value <= 0 && prices.Length == 1 && (prices[0] == "" || prices[0] == "0");
            return free;
        }
    }
}
