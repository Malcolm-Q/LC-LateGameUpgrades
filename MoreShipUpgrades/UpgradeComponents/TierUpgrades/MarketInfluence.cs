using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc;
using MoreShipUpgrades.Misc.Upgrades;
using UnityEngine;

namespace MoreShipUpgrades.UpgradeComponents.TierUpgrades
{
    internal class MarketInfluence : TierUpgrade
    {
        public static string UPGRADE_NAME = "Market Influence";
        public static string PRICES_DEFAULT = "200,350,500";

        internal override void Start()
        {
            upgradeName = UPGRADE_NAME;
            base.Start();
        }
        public static int GetGuaranteedPercentageSale(int defaultPercentage, int maxValue)
        {
            if (!GetActiveUpgrade(UPGRADE_NAME)) return defaultPercentage;
            return Mathf.Clamp(defaultPercentage + UpgradeBus.instance.cfg.MARKET_INFLUENCE_INITIAL_PERCENTAGE.Value + GetUpgradeLevel(UPGRADE_NAME) * UpgradeBus.instance.cfg.MARKET_INFLUENCE_INCREMENTAL_PERCENTAGE.Value, 0, maxValue);
        }
        public override string GetDisplayInfo(int initialPrice = -1, int maxLevels = -1, int[] incrementalPrices = null)
        {
            System.Func<int, float> infoFunction = level => UpgradeBus.instance.cfg.MARKET_INFLUENCE_INITIAL_PERCENTAGE.Value + (level * UpgradeBus.instance.cfg.MARKET_INFLUENCE_INCREMENTAL_PERCENTAGE.Value);
            string infoFormat = "LVL {0} - ${1} - Guarantees the item sales' percentage to be at least {2}%\n";
            return Tools.GenerateInfoForUpgrade(infoFormat, initialPrice, incrementalPrices, infoFunction);
        }
    }
}
