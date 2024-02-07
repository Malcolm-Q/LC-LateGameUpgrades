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

        public override void Increment()
        {
            UpgradeBus.instance.marketInfluenceLevel++;
        }

        public override void Load()
        {
            base.Load();
            UpgradeBus.instance.marketInfluence = true;
        }

        public override void Unwind()
        {
            base.Unwind();

            UpgradeBus.instance.marketInfluenceLevel = 0;
            UpgradeBus.instance.marketInfluence = false;
        }
        public static int GetGuaranteedPercentageSale(int defaultPercentage, int maxValue)
        {
            if (!UpgradeBus.instance.marketInfluence) return defaultPercentage;
            return Mathf.Clamp(defaultPercentage + UpgradeBus.instance.cfg.MARKET_INFLUENCE_INITIAL_PERCENTAGE + UpgradeBus.instance.marketInfluenceLevel * UpgradeBus.instance.cfg.MARKET_INFLUENCE_INCREMENTAL_PERCENTAGE, 0, maxValue);
        }
        public override string GetDisplayInfo(int initialPrice = -1, int maxLevels = -1, int[] incrementalPrices = null)
        {
            System.Func<int, float> infoFunction = level => UpgradeBus.instance.cfg.MARKET_INFLUENCE_INITIAL_PERCENTAGE + (level * UpgradeBus.instance.cfg.MARKET_INFLUENCE_INCREMENTAL_PERCENTAGE);
            string infoFormat = "LVL {0} - ${1} - Guarantees the item sales' percentage to be at least {2}%\n";
            return Tools.GenerateInfoForUpgrade(infoFormat, initialPrice, incrementalPrices, infoFunction);
        }
    }
}
