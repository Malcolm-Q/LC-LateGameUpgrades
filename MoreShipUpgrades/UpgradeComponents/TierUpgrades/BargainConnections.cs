using BepInEx.Logging;
using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc;
using MoreShipUpgrades.Misc.Upgrades;

namespace MoreShipUpgrades.UpgradeComponents.TierUpgrades
{
    class BargainConnections : TierUpgrade
    {
        public static string UPGRADE_NAME = "Bargain Connections";
        public static string PRICES_DEFAULT = "225,300,375";
        internal override void Start()
        {
            upgradeName = UPGRADE_NAME;
            base.Start();
        }
        public static int GetBargainConnectionsAdditionalItems(int defaultAmountItems)
        {
            if (!GetActiveUpgrade(UPGRADE_NAME)) return defaultAmountItems;
            return defaultAmountItems + UpgradeBus.instance.cfg.BARGAIN_CONNECTIONS_INITIAL_ITEM_AMOUNT.Value + (UpgradeBus.instance.upgradeLevels[UPGRADE_NAME] * UpgradeBus.instance.cfg.BARGAIN_CONNECTIONS_INCREMENTAL_ITEM_AMOUNT.Value);
        }
        public override string GetDisplayInfo(int initialPrice = -1, int maxLevels = -1, int[] incrementalPrices = null)
        {
            System.Func<int, float> infoFunction = level => UpgradeBus.instance.cfg.BARGAIN_CONNECTIONS_INITIAL_ITEM_AMOUNT.Value + (level * UpgradeBus.instance.cfg.BARGAIN_CONNECTIONS_INCREMENTAL_ITEM_AMOUNT.Value);
            string infoFormat = "LVL {0} - ${1} - Increases the amount of items that can be on sale by {2}\n";
            return Tools.GenerateInfoForUpgrade(infoFormat, initialPrice, incrementalPrices, infoFunction);
        }
    }
}
