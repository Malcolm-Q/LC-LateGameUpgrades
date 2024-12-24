using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc.Upgrades;
using MoreShipUpgrades.Misc.Util;
using UnityEngine;
using LCVR;
using MoreShipUpgrades.UI.TerminalNodes;
using MoreShipUpgrades.UpgradeComponents.Interfaces;
using MoreShipUpgrades.Configuration;
using MoreShipUpgrades.Configuration.Interfaces;

namespace MoreShipUpgrades.UpgradeComponents.TierUpgrades.Ship
{
    internal class ScavengerInstincts : TierUpgrade, IUpgradeWorldBuilding
    {
        internal const string UPGRADE_NAME = "Scavenger Instincts";
        internal const string DEFAULT_PRICES = "800,800,1000,1200,1400";
        internal const string WORLD_BUILDING_TEXT = "\n\nAfter spending a lot of time in the facility, you begin to notice certain patterns in the location of valuable objects. Your department's output increases, but the implications are disturbing.\n\n";

        public string GetWorldBuildingText(bool shareStatus = false)
        {
            return WORLD_BUILDING_TEXT;
        }
        void Awake()
        {
            upgradeName = UPGRADE_NAME;
            overridenUpgradeName = GetConfiguration().ScavengerInstictsConfiguration.OverrideName;
        }
        public override bool CanInitializeOnStart
        {
            get
            {
                ITierEffectUpgradeConfiguration<int> upgradeConfig = GetConfiguration().ScavengerInstictsConfiguration;
                string[] prices = upgradeConfig.Prices.Value.Split(',');
                return prices.Length == 0 || (prices.Length == 1 && (prices[0].Length == 0 || prices[0] == "0"));
            }
        }
        public static int IncreaseScrapAmount(int defaultValue)
        {
            ITierEffectUpgradeConfiguration<int> upgradeConfig = GetConfiguration().ScavengerInstictsConfiguration;
            if (!upgradeConfig.Enabled) return defaultValue;
            if (!GetActiveUpgrade(UPGRADE_NAME)) return defaultValue;
            int additionalScrap = upgradeConfig.InitialEffect.Value + (GetUpgradeLevel(UPGRADE_NAME) * upgradeConfig.IncrementalEffect.Value);
            return Mathf.Clamp(defaultValue + Mathf.CeilToInt(additionalScrap / RoundManager.Instance.scrapAmountMultiplier), defaultValue, int.MaxValue);
        }
        public override string GetDisplayInfo(int initialPrice = -1, int maxLevels = -1, int[] incrementalPrices = null)
        {
            static float infoFunction(int level)
            {
                ITierEffectUpgradeConfiguration<int> upgradeConfig = GetConfiguration().ScavengerInstictsConfiguration;
                return upgradeConfig.InitialEffect.Value + (level * upgradeConfig.IncrementalEffect.Value);
            }
            const string infoFormat = "LVL {0} - ${1} - Increases the average amount of scrap spawns by {2} additional items.\n";
            return Tools.GenerateInfoForUpgrade(infoFormat, initialPrice, incrementalPrices, infoFunction);
        }
        public new static (string, string[]) RegisterScrapToUpgrade()
        {
            return (UPGRADE_NAME, GetConfiguration().ScavengerInstictsConfiguration.ItemProgressionItems.Value.Split(","));
        }
        public new static void RegisterUpgrade()
        {
            SetupGenericPerk<ScavengerInstincts>(UPGRADE_NAME);
        }
        public new static CustomTerminalNode RegisterTerminalNode()
        {
            return UpgradeBus.Instance.SetupMultiplePurchaseableTerminalNode(UPGRADE_NAME, GetConfiguration().ScavengerInstictsConfiguration);
        }
    }
}
