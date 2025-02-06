using MoreShipUpgrades.Configuration.Interfaces.TierUpgrades;
using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc.Upgrades;
using MoreShipUpgrades.Misc.Util;
using MoreShipUpgrades.UI.TerminalNodes;
using MoreShipUpgrades.UpgradeComponents.Interfaces;
using UnityEngine;

namespace MoreShipUpgrades.UpgradeComponents.TierUpgrades
{
    internal class EfficientEngines : TierUpgrade, IUpgradeWorldBuilding
    {
        internal const string UPGRADE_NAME = "Efficient Engines";
        internal const string DEFAULT_PRICES = "450,600, 750, 900";
        internal const string WORLD_BUILDING_TEXT = "\n\nService package for your Ship's autopiloting system that tweaks the parameters by which it routes between moons." +
            " By default, the autopiloting software is configured to only use the safest routes, but this package will authorize it to use faster but slightly more dangerous \"Liability Routes\" and certain spatial anomalies to expedite travel, conserving fuel costs.\n\n";

        public string GetWorldBuildingText(bool shareStatus = false)
        {
            return WORLD_BUILDING_TEXT;
        }
        void Awake()
        {
            upgradeName = UPGRADE_NAME;
            overridenUpgradeName = GetConfiguration().EfficientEnginesConfiguration.OverrideName;
        }
        public override string GetDisplayInfo(int initialPrice = -1, int maxLevels = -1, int[] incrementalPrices = null)
        {
            static float infoFunction(int level)
            {
                ITierEffectUpgradeConfiguration<int> config = GetConfiguration().EfficientEnginesConfiguration;
                return config.InitialEffect.Value + (level * config.IncrementalEffect.Value);
            }
            const string infoFormat = "LVL {0} - {1} - Moon routing will be {2}% cheaper\n";
            return Tools.GenerateInfoForUpgrade(infoFormat, initialPrice, incrementalPrices, infoFunction, purchaseMode: GetConfiguration().EfficientEnginesConfiguration.PurchaseMode);
        }

        public static int GetDiscountedMoonPrice(int defaultPrice)
        {
            ITierEffectUpgradeConfiguration<int> config = GetConfiguration().EfficientEnginesConfiguration;
            if (!config.Enabled.Value) return defaultPrice;
            if (!GetActiveUpgrade(UPGRADE_NAME)) return defaultPrice;
            if (defaultPrice == 0) return defaultPrice;
            float discountedPrice = defaultPrice * (1f - ((config.InitialEffect.Value + (GetUpgradeLevel(UPGRADE_NAME) * config.IncrementalEffect.Value))/100f));
            return Mathf.CeilToInt(Mathf.Clamp(discountedPrice, 0f, defaultPrice));
        }
        public override bool CanInitializeOnStart
        {
            get
            {
                string[] prices = GetConfiguration().EfficientEnginesConfiguration.Prices.Value.Split(',');
                return prices.Length == 0 || (prices.Length == 1 && (prices[0].Length == 0 || prices[0] == "0"));
            }
        }

        public new static (string, string[]) RegisterScrapToUpgrade()
        {
            return (UPGRADE_NAME, GetConfiguration().EfficientEnginesConfiguration.ItemProgressionItems.Value.Split(","));
        }
        public new static void RegisterUpgrade()
        {
            SetupGenericPerk<EfficientEngines>(UPGRADE_NAME);
        }
        public new static CustomTerminalNode RegisterTerminalNode()
        {
            return UpgradeBus.Instance.SetupMultiplePurchaseableTerminalNode(UPGRADE_NAME, GetConfiguration().EfficientEnginesConfiguration);
        }
    }
}
