using MoreShipUpgrades.Configuration.Upgrades.Interfaces.TierUpgrades;
using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc.Upgrades;
using MoreShipUpgrades.Misc.Util;
using MoreShipUpgrades.UI.TerminalNodes;
using MoreShipUpgrades.UpgradeComponents.Interfaces;
using UnityEngine;

namespace MoreShipUpgrades.UpgradeComponents.TierUpgrades
{
    internal class ClimbingGloves : TierUpgrade, IUpgradeWorldBuilding
    {
        internal const string UPGRADE_NAME = "Climbing Gloves";
        internal const string DEFAULT_PRICES = "150,200,250,300";
        internal const string WORLD_BUILDING_TEXT = "\n\nIn the 'GEAR' section of the Company Catalogue, at the bottom of the last page sandwiched between two ads, you find an entry for this product." +
            " They are Premium Synthetic Proprietary Ladder Gripping Gloves. The sales screed promises a 2% increase in your department's efficiency at outdoor traversal if you buy this.\n\n";

        public string GetWorldBuildingText(bool shareStatus = false)
        {
            return WORLD_BUILDING_TEXT;
        }
        void Awake()
        {
            upgradeName = UPGRADE_NAME;
            overridenUpgradeName = UpgradeBus.Instance.PluginConfiguration.ClimblingGlovesConfiguration.OverrideName;
        }

        public override string GetDisplayInfo(int initialPrice = -1, int maxLevels = -1, int[] incrementalPrices = null)
        {
            static float infoFunction(int level)
            {
                ITierEffectUpgradeConfiguration<float> config = GetConfiguration().ClimblingGlovesConfiguration;
                return config.InitialEffect.Value + (level * config.IncrementalEffect.Value);
            }
            const string infoFormat = "LVL {0} - {1} - Increases the speed of climbing ladders by {2} units.\n";
            return Tools.GenerateInfoForUpgrade(infoFormat, initialPrice, incrementalPrices, infoFunction, purchaseMode: GetConfiguration().ClimblingGlovesConfiguration.PurchaseMode);
        }
        public override bool CanInitializeOnStart
        {
            get
            {
                ITierEffectUpgradeConfiguration<float> config = GetConfiguration().ClimblingGlovesConfiguration;
                string[] prices = config.Prices.Value.Split(',');
                return prices.Length == 0 || (prices.Length == 1 && (prices[0].Length == 0 || prices[0] == "0"));
            }
        }
        public static float GetAdditionalClimbingSpeed(float defaultValue)
        {
            ITierEffectUpgradeConfiguration<float> config = GetConfiguration().ClimblingGlovesConfiguration;
            if (!config.Enabled) return defaultValue;
            if (!GetActiveUpgrade(UPGRADE_NAME)) return defaultValue;
            float additionalValue = config.InitialEffect + (GetUpgradeLevel(UPGRADE_NAME) * config.IncrementalEffect);
            return Mathf.Clamp(defaultValue + additionalValue, defaultValue, float.MaxValue);
        }

        public new static (string, string[]) RegisterScrapToUpgrade()
        {
            return (UPGRADE_NAME, GetConfiguration().ClimblingGlovesConfiguration.ItemProgressionItems.Value.Split(","));
        }
        public new static void RegisterUpgrade()
        {
            SetupGenericPerk<ClimbingGloves>(UPGRADE_NAME);
        }
        public new static CustomTerminalNode RegisterTerminalNode()
        {
            return UpgradeBus.Instance.SetupMultiplePurchaseableTerminalNode(UPGRADE_NAME, GetConfiguration().ClimblingGlovesConfiguration);
        }
    }
}
