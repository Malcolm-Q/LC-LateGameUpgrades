using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc.TerminalNodes;
using MoreShipUpgrades.Misc;
using MoreShipUpgrades.Misc.Upgrades;
using System;
using System.Collections.Generic;
using System.Text;
using MoreShipUpgrades.Misc.Util;
using UnityEngine;

namespace MoreShipUpgrades.UpgradeComponents.TierUpgrades
{
    internal class ScavengerInstincts : TierUpgrade
    {
        internal const string UPGRADE_NAME = "Scavenger Instincts";
        internal const string DEFAULT_PRICES = "800,1000,1200,1400";

        void Awake()
        {
            upgradeName = UPGRADE_NAME;
            overridenUpgradeName = UpgradeBus.Instance.PluginConfiguration.SCAVENGER_INSTINCTS_OVERRIDE_NAME;
        }
        public override bool CanInitializeOnStart
        {
            get
            {
                string[] prices = UpgradeBus.Instance.PluginConfiguration.SCAVENGER_INSTINCTS_PRICES.Value.Split(',');
                bool free = UpgradeBus.Instance.PluginConfiguration.SCAVENGER_INSTINCTS_PRICE.Value <= 0 && prices.Length == 1 && (prices[0] == "" || prices[0] == "0");
                return free;
            }
        }
        public static int IncreaseScrapAmount(int defaultValue)
        {
            if (!(GetActiveUpgrade(UPGRADE_NAME))) return defaultValue;
            int additionalScrap = UpgradeBus.Instance.PluginConfiguration.SCAVENGER_INSTINCTS_INITIAL_AMOUNT_SCRAP_INCREASE.Value + (GetUpgradeLevel(UPGRADE_NAME) * UpgradeBus.Instance.PluginConfiguration.SCAVENGER_INSTINCTS_INCREMENTAL_AMOUN_SCRAP_INCREASE.Value);
            return Mathf.Clamp(defaultValue + Mathf.CeilToInt(additionalScrap/RoundManager.Instance.scrapAmountMultiplier), defaultValue, int.MaxValue);
        }
        public override string GetDisplayInfo(int initialPrice = -1, int maxLevels = -1, int[] incrementalPrices = null)
        {
            System.Func<int, float> infoFunction = level => UpgradeBus.Instance.PluginConfiguration.SCAVENGER_INSTINCTS_INITIAL_AMOUNT_SCRAP_INCREASE.Value + (level * UpgradeBus.Instance.PluginConfiguration.SCAVENGER_INSTINCTS_INCREMENTAL_AMOUN_SCRAP_INCREASE.Value);
            string infoFormat = "LVL {0} - ${1} - Increases the average amount of scrap spawns by {2} additional items.\n";
            return Tools.GenerateInfoForUpgrade(infoFormat, initialPrice, incrementalPrices, infoFunction);
        }
        public new static void RegisterUpgrade()
        {
            SetupGenericPerk<ScavengerInstincts>(UPGRADE_NAME);
        }
        public new static CustomTerminalNode RegisterTerminalNode()
        {
            LategameConfiguration configuration = UpgradeBus.Instance.PluginConfiguration;

            return UpgradeBus.Instance.SetupMultiplePurchasableTerminalNode(UPGRADE_NAME,
                                                shareStatus: true,
                                                configuration.SCAVENGER_INSTINCTS_ENABLED,
                                                configuration.SCAVENGER_INSTINCTS_PRICE,
                                                UpgradeBus.ParseUpgradePrices(configuration.SCAVENGER_INSTINCTS_PRICES),
                                                configuration.OVERRIDE_UPGRADE_NAMES ? configuration.SCAVENGER_INSTINCTS_OVERRIDE_NAME : "");
        }
    }
}
