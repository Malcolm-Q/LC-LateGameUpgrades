using MoreShipUpgrades.Configuration;
using MoreShipUpgrades.Configuration.Interfaces.TierUpgrades;
using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc;
using MoreShipUpgrades.Misc.Upgrades;
using MoreShipUpgrades.Misc.Util;
using MoreShipUpgrades.UI.TerminalNodes;
using MoreShipUpgrades.UpgradeComponents.Interfaces;
using UnityEngine;

namespace MoreShipUpgrades.UpgradeComponents.TierUpgrades.Player
{
    internal class StrongLegs : TierUpgrade, IUpgradeWorldBuilding
    {
        public const string UPGRADE_NAME = "Strong Legs";
        public const string PRICES_DEFAULT = "300,150,190,250";
        internal const string WORLD_BUILDING_TEXT = "\n\nOne-time issuance of {0}." +
            " Comes with a vague list of opt-in maintenance procedures offered by The Company, which includes such gems as 'actuation optimization'," +
            " 'weight & balance personalization', and similar nigh-meaningless corpo-tech jargon. All of it is expensive.\n\n";
        void Awake()
        {
            upgradeName = UPGRADE_NAME;
            overridenUpgradeName = GetConfiguration().StrongLegsConfiguration.OverrideName;
        }
        public string GetWorldBuildingText(bool shareStatus = false)
        {
            return string.Format(WORLD_BUILDING_TEXT, shareStatus ? "proprietary pressure-assisted kneebraces to your crew" : "a proprietary pressure-assisted kneebrace");
        }

        public override string GetDisplayInfo(int initialPrice = -1, int maxLevels = -1, int[] incrementalPrices = null)
        {
            static float infoFunction(int level)
            {
                ITierEffectUpgradeConfiguration<float> config = GetConfiguration().StrongLegsConfiguration;
                return config.InitialEffect.Value + (level * config.IncrementalEffect.Value);
            }
            string infoFormat = AssetBundleHandler.GetInfoFromJSON(UPGRADE_NAME);
            return Tools.GenerateInfoForUpgrade(infoFormat, initialPrice, incrementalPrices, infoFunction);
        }
        public override bool CanInitializeOnStart
        {
            get
            {
                ITierEffectUpgradeConfiguration<float> upgradeConfig = GetConfiguration().StrongLegsConfiguration;
                string[] prices = upgradeConfig.Prices.Value.Split(',');
                return prices.Length == 0 || (prices.Length == 1 && (prices[0].Length == 0 || prices[0] == "0"));
            }
        }
        public static float GetAdditionalJumpForce(float defaultValue)
        {
            ITierEffectUpgradeConfiguration<float> upgradeConfig = GetConfiguration().StrongLegsConfiguration;
            if (!upgradeConfig.Enabled) return defaultValue;
            if (!GetActiveUpgrade(UPGRADE_NAME)) return defaultValue;
            float additionalValue = upgradeConfig.InitialEffect + (GetUpgradeLevel(UPGRADE_NAME) * upgradeConfig.IncrementalEffect);
            return Mathf.Clamp(defaultValue + additionalValue, defaultValue, float.MaxValue);
        }

        public new static (string, string[]) RegisterScrapToUpgrade()
        {
            return (UPGRADE_NAME, GetConfiguration().StrongLegsConfiguration.ItemProgressionItems.Value.Split(","));
        }
        public new static void RegisterUpgrade()
        {
            SetupGenericPerk<StrongLegs>(UPGRADE_NAME);
        }
        public new static CustomTerminalNode RegisterTerminalNode()
        {
            return UpgradeBus.Instance.SetupMultiplePurchaseableTerminalNode(UPGRADE_NAME, GetConfiguration().StrongLegsConfiguration);
        }
    }
}
