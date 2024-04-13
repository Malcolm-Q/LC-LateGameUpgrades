﻿using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc.Upgrades;
using MoreShipUpgrades.Misc.Util;
using UnityEngine;

namespace MoreShipUpgrades.UpgradeComponents.TierUpgrades
{
    internal class MarketInfluence : TierUpgrade
    {
        internal const string UPGRADE_NAME = "Market Influence";
        internal const string PRICES_DEFAULT = "200,350,500";

        internal override void Start()
        {
            upgradeName = UpgradeBus.Instance.PluginConfiguration.OVERRIDE_UPGRADE_NAMES ? UpgradeBus.Instance.PluginConfiguration.MARKET_INFLUENCE_OVERRIDE_NAME : UPGRADE_NAME;
            base.Start();
        }
        public static int GetGuaranteedPercentageSale(int defaultPercentage, int maxValue)
        {
            if (!GetActiveUpgrade(UPGRADE_NAME)) return defaultPercentage;
            return Mathf.Clamp(defaultPercentage + UpgradeBus.Instance.PluginConfiguration.MARKET_INFLUENCE_INITIAL_PERCENTAGE.Value + GetUpgradeLevel(UPGRADE_NAME) * UpgradeBus.Instance.PluginConfiguration.MARKET_INFLUENCE_INCREMENTAL_PERCENTAGE.Value, 0, maxValue);
        }
        public override string GetDisplayInfo(int initialPrice = -1, int maxLevels = -1, int[] incrementalPrices = null)
        {
            System.Func<int, float> infoFunction = level => UpgradeBus.Instance.PluginConfiguration.MARKET_INFLUENCE_INITIAL_PERCENTAGE.Value + (level * UpgradeBus.Instance.PluginConfiguration.MARKET_INFLUENCE_INCREMENTAL_PERCENTAGE.Value);
            string infoFormat = "LVL {0} - ${1} - Guarantees the item sales' percentage to be at least {2}%\n";
            return Tools.GenerateInfoForUpgrade(infoFormat, initialPrice, incrementalPrices, infoFunction);
        }
        internal override bool CanInitializeOnStart()
        {
            string[] prices = UpgradeBus.Instance.PluginConfiguration.MARKET_INFLUENCE_PRICES.Value.Split(',');
            bool free = UpgradeBus.Instance.PluginConfiguration.MARKET_INFLUENCE_PRICE.Value <= 0 && prices.Length == 1 && (prices[0] == "" || prices[0] == "0");
            return free;
        }
    }
}
