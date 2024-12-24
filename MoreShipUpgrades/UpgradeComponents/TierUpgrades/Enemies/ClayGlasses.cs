using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc.Upgrades;
using MoreShipUpgrades.Misc.Util;
using UnityEngine;
using MoreShipUpgrades.UI.TerminalNodes;
using MoreShipUpgrades.UpgradeComponents.Interfaces;
using MoreShipUpgrades.Configuration;
using MoreShipUpgrades.Configuration.Interfaces;

namespace MoreShipUpgrades.UpgradeComponents.TierUpgrades.Enemies
{
    public class ClayGlasses : TierUpgrade, IUpgradeWorldBuilding
    {
        public const string UPGRADE_NAME = "Clay Glasses";
        internal const string WORLD_BUILDING_TEXT = "\n\nSubscription to 'Prestidigitation Press' magazine. Comes every week." +
            " The magazines are full of supposed-eyewitness testimony of supernatural occurences." +
            " Most of the content of the magazine appears blatantly fake to you, but the fact that the Clay Surgeon is mentioned and described in exacting detail sends a shiver down your spine.\n\n";
        internal const string PRICES_DEFAULT = "200,300,400,500,600";
        public string GetWorldBuildingText(bool shareStatus = false)
        {
            return WORLD_BUILDING_TEXT;
        }
        internal override void Start()
        {
            upgradeName = UPGRADE_NAME;
            overridenUpgradeName = GetConfiguration().ClayGlassesConfiguration.OverrideName;
            base.Start();
        }
        public override bool CanInitializeOnStart
        {
            get
            {
                ITierEffectUpgradeConfiguration<float> upgradeConfig = GetConfiguration().ClayGlassesConfiguration;
                string[] prices = upgradeConfig.Prices.Value.Split(',');
                return prices.Length == 0 || (prices.Length == 1 && (prices[0].Length == 0 || prices[0] == "0"));
            }
        }
        public static float GetAdditionalMaximumDistance(float defaultValue)
        {
            ITierEffectUpgradeConfiguration<float> upgradeConfig = GetConfiguration().ClayGlassesConfiguration;
            if (!upgradeConfig.Enabled) return defaultValue;
            if (!GetActiveUpgrade(UPGRADE_NAME)) return defaultValue;
            float additionalValue = upgradeConfig.InitialEffect + (GetUpgradeLevel(UPGRADE_NAME) * upgradeConfig.IncrementalEffect);
            return Mathf.Clamp(defaultValue + additionalValue, defaultValue, float.MaxValue);
        }

        public override string GetDisplayInfo(int initialPrice = -1, int maxLevels = -1, int[] incrementalPrices = null)
        {
            static float infoFunction(int level)
            {
                ITierEffectUpgradeConfiguration<float> upgradeConfig = GetConfiguration().ClayGlassesConfiguration;
                return upgradeConfig.InitialEffect.Value + (level * upgradeConfig.IncrementalEffect.Value);
            }
            const string infoFormat = "LVL {0} - ${1} - The maximum distance to spot a \"Clay Surgeon\" entity is increased by {2} additional units.\n";
            return Tools.GenerateInfoForUpgrade(infoFormat, initialPrice, incrementalPrices, infoFunction);
        }

        public new static (string, string[]) RegisterScrapToUpgrade()
        {
            return (UPGRADE_NAME, GetConfiguration().ClayGlassesConfiguration.ItemProgressionItems.Value.Split(","));
        }
        public new static void RegisterUpgrade()
        {
            SetupGenericPerk<ClayGlasses>(UPGRADE_NAME);
        }
        public new static CustomTerminalNode RegisterTerminalNode()
        {
            return UpgradeBus.Instance.SetupMultiplePurchaseableTerminalNode(UPGRADE_NAME, GetConfiguration().ClayGlassesConfiguration);
        }
    }
}
