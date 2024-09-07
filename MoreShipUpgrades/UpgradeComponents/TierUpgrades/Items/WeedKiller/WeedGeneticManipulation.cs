using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc.TerminalNodes;
using MoreShipUpgrades.Misc;
using MoreShipUpgrades.Misc.Upgrades;
using MoreShipUpgrades.Misc.Util;
using UnityEngine;

namespace MoreShipUpgrades.UpgradeComponents.TierUpgrades.Items.WeedKiller
{
    internal class WeedGeneticManipulation : TierUpgrade
    {
        internal const string UPGRADE_NAME = "Weed Genetic Manipulation";
        internal const string PRICES_DEFAULT = "100,150,200";

        internal override void Start()
        {
            upgradeName = UPGRADE_NAME;
            overridenUpgradeName = UpgradeBus.Instance.PluginConfiguration.WEED_GENETIC_MANIPULATION_OVERRIDE_NAME;
            base.Start();
        }
        public static float ComputeWeedKillerEffectiveness()
        {
            int percentage = UpgradeBus.Instance.PluginConfiguration.WEED_GENETIC_MANIPULATION_INITIAL_EFFECTIVENESS_INCREASE + (GetUpgradeLevel(UPGRADE_NAME) * UpgradeBus.Instance.PluginConfiguration.WEED_GENETIC_MANIPULATION_INCREMENTAL_EFFECTIVENESS_INCREASE);
            return percentage / 100f;
        }
        public static float GetWeedKillerEffectiveness(float defaultValue)
        {
            if (!UpgradeBus.Instance.PluginConfiguration.WEED_GENETIC_MANIPULATION_ENABLED) return defaultValue;
            if (!GetActiveUpgrade(UPGRADE_NAME)) return defaultValue;
            float multiplier = ComputeWeedKillerEffectiveness();
            return Mathf.Clamp(defaultValue * multiplier, defaultValue, float.MaxValue);
        }
        public override bool CanInitializeOnStart
        {
            get
            {
                string[] prices = UpgradeBus.Instance.PluginConfiguration.WEED_GENETIC_MANIPULATION_PRICES.Value.Split(',');
                return UpgradeBus.Instance.PluginConfiguration.WEED_GENETIC_MANIPULATION_PRICE.Value <= 0 && prices.Length == 1 && (prices[0].Length == 0 || prices[0] == "0");
            }
        }
        public override string GetDisplayInfo(int initialPrice = -1, int maxLevels = -1, int[] incrementalPrices = null)
        {
            static float infoFunction(int level) => UpgradeBus.Instance.PluginConfiguration.WEED_GENETIC_MANIPULATION_INITIAL_EFFECTIVENESS_INCREASE.Value + (level * UpgradeBus.Instance.PluginConfiguration.WEED_GENETIC_MANIPULATION_INCREMENTAL_EFFECTIVENESS_INCREASE.Value);
            const string infoFormat = "LVL {0} - ${1} - Effectiveness of the Weed Killer item in eradicating plants is increased by {2}%\n";
            return Tools.GenerateInfoForUpgrade(infoFormat, initialPrice, incrementalPrices, infoFunction);
        }
        public new static (string, string[]) RegisterScrapToUpgrade()
        {
            return (UPGRADE_NAME, UpgradeBus.Instance.PluginConfiguration.WEED_GENETIC_MANIPULATION_ITEM_PROGRESSION_ITEMS.Value.Split(","));
        }
        public new static void RegisterUpgrade()
        {
            SetupGenericPerk<WeedGeneticManipulation>(UPGRADE_NAME);
        }
        public new static CustomTerminalNode RegisterTerminalNode()
        {
            LategameConfiguration configuration = UpgradeBus.Instance.PluginConfiguration;

            return UpgradeBus.Instance.SetupMultiplePurchasableTerminalNode(UPGRADE_NAME,
                                                shareStatus: true,
                                                configuration.WEED_GENETIC_MANIPULATION_ENABLED.Value,
                                                configuration.WEED_GENETIC_MANIPULATION_PRICE.Value,
                                                UpgradeBus.ParseUpgradePrices(configuration.WEED_GENETIC_MANIPULATION_PRICES.Value),
                                                configuration.OVERRIDE_UPGRADE_NAMES ? configuration.WEED_GENETIC_MANIPULATION_OVERRIDE_NAME : "");
        }
    }
}
