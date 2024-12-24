using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc.Upgrades;
using MoreShipUpgrades.Misc.Util;
using UnityEngine;
using MoreShipUpgrades.UI.TerminalNodes;
using MoreShipUpgrades.UpgradeComponents.Interfaces;
using MoreShipUpgrades.Configuration;
using MoreShipUpgrades.Configuration.Interfaces;

namespace MoreShipUpgrades.UpgradeComponents.TierUpgrades.Items.Shotgun
{
    internal class SleightOfHand : TierUpgrade, IUpgradeWorldBuilding
    {
        internal const string UPGRADE_NAME = "Sleight of Hand";
        internal const string PRICES_DEFAULT = "100,150,200,250";
        internal const string WORLD_BUILDING_TEXT = "\n\nOn-the-job training package detailing the safe & efficient handling and application of firearms.\n\n";

        public string GetWorldBuildingText(bool shareStatus = false)
        {
            return WORLD_BUILDING_TEXT;
        }
        internal override void Start()
        {
            upgradeName = UPGRADE_NAME;
            overridenUpgradeName = GetConfiguration().SleightOfHandConfiguration.OverrideName;
            base.Start();
        }
        public override bool CanInitializeOnStart
        {
            get
            {
                ITierEffectUpgradeConfiguration<int> upgradeConfig = GetConfiguration().SleightOfHandConfiguration;
                string[] prices = upgradeConfig.Prices.Value.Split(',');
                return prices.Length == 0 || (prices.Length == 1 && (prices[0].Length == 0 || prices[0] == "0"));
            }
        }
        public override string GetDisplayInfo(int initialPrice = -1, int maxLevels = -1, int[] incrementalPrices = null)
        {
            static float infoFunction(int level)
            {
                ITierEffectUpgradeConfiguration<int> upgradeConfig = GetConfiguration().SleightOfHandConfiguration;
                return upgradeConfig.InitialEffect.Value + (level * upgradeConfig.IncrementalEffect.Value);
            }
            const string infoFormat = "LVL {0} - ${1} - The reload speed of weaponry is increased by {2}%\n";
            return Tools.GenerateInfoForUpgrade(infoFormat, initialPrice, incrementalPrices, infoFunction);
        }
        public static float ComputeSleightOfHandSpeedBoost()
        {
            ITierEffectUpgradeConfiguration<int> upgradeConfig = GetConfiguration().SleightOfHandConfiguration;
            int percentage = upgradeConfig.InitialEffect + (GetUpgradeLevel(UPGRADE_NAME) * upgradeConfig.IncrementalEffect);
            return percentage / 100f;
        }
        public static float GetSleightOfHandSpeedBoost(float defaultValue)
        {
            if (!GetConfiguration().SleightOfHandConfiguration.Enabled) return defaultValue;
            if (!GetActiveUpgrade(UPGRADE_NAME)) return defaultValue;
            float multiplier = ComputeSleightOfHandSpeedBoost();
            return Mathf.Clamp(defaultValue * (1f - multiplier), 0, defaultValue);
        }
        public new static (string, string[]) RegisterScrapToUpgrade()
        {
            return (UPGRADE_NAME, GetConfiguration().SleightOfHandConfiguration.ItemProgressionItems.Value.Split(","));
        }
        public new static void RegisterUpgrade()
        {
            SetupGenericPerk<SleightOfHand>(UPGRADE_NAME);
        }
        public new static CustomTerminalNode RegisterTerminalNode()
        {
            return UpgradeBus.Instance.SetupMultiplePurchaseableTerminalNode(UPGRADE_NAME, GetConfiguration().SleightOfHandConfiguration);
        }
    }
}
