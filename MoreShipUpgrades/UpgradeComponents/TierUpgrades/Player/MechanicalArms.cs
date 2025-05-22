using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc.Upgrades;
using MoreShipUpgrades.Misc.Util;
using UnityEngine;
using MoreShipUpgrades.UI.TerminalNodes;
using MoreShipUpgrades.UpgradeComponents.Interfaces;
using MoreShipUpgrades.Configuration.Upgrades.Interfaces.TierUpgrades;

namespace MoreShipUpgrades.UpgradeComponents.TierUpgrades.Player
{
    internal class MechanicalArms : TierUpgrade, IUpgradeWorldBuilding
    {
        internal const string UPGRADE_NAME = "Mechanical Arms";
        internal const string PRICES_DEFAULT = "300,400,600,800";
        internal const string WORLD_BUILDING_TEXT = "\n\nYou were pricked by a weirdly sharp metal fragment in the facility one day. You fell ill and collapsed for twenty minutes" +
            " and awoke feeling bizarre. Ever since then, for some reason, you've been able to grab items and open doors from further away.\n\n";

        public string GetWorldBuildingText(bool shareStatus = false)
        {
            return WORLD_BUILDING_TEXT;
        }
        internal override void Start()
        {
            upgradeName = UPGRADE_NAME;
            overridenUpgradeName = GetConfiguration().MechanicalArmsConfiguration.OverrideName;
            base.Start();
        }
        public static float GetIncreasedGrabDistance(float defaultValue)
        {
            ITierEffectUpgradeConfiguration<float> upgradeConfig = GetConfiguration().MechanicalArmsConfiguration;
            if (!upgradeConfig.Enabled) return defaultValue;
            if (!GetActiveUpgrade(UPGRADE_NAME)) return defaultValue;
            float increasedRange = upgradeConfig.InitialEffect + (GetUpgradeLevel(UPGRADE_NAME) * upgradeConfig.IncrementalEffect);
            return Mathf.Clamp(defaultValue + increasedRange, defaultValue, float.MaxValue);
        }
        public override string GetDisplayInfo(int initialPrice = -1, int maxLevels = -1, int[] incrementalPrices = null)
        {
            static float infoFunction(int level)
            {
                ITierEffectUpgradeConfiguration<float> upgradeConfig = GetConfiguration().MechanicalArmsConfiguration;
                return upgradeConfig.InitialEffect.Value + (level * upgradeConfig.IncrementalEffect.Value);
            }
            const string infoFormat = "LVL {0} - {1} - Increases the player's interaction range by {2} units.\n";
            return Tools.GenerateInfoForUpgrade(infoFormat, initialPrice, incrementalPrices, infoFunction, purchaseMode: GetConfiguration().MechanicalArmsConfiguration.PurchaseMode);
        }
        public override bool CanInitializeOnStart
        {
            get
            {
                ITierEffectUpgradeConfiguration<float> upgradeConfig = GetConfiguration().MechanicalArmsConfiguration;
                string[] prices = upgradeConfig.Prices.Value.Split(',');
                return prices.Length == 0 || (prices.Length == 1 && (prices[0].Length == 0 || prices[0] == "0"));
            }
        }

        public new static (string, string[]) RegisterScrapToUpgrade()
        {
            return (UPGRADE_NAME, GetConfiguration().MechanicalArmsConfiguration.ItemProgressionItems.Value.Split(","));
        }
        public new static void RegisterUpgrade()
        {
            SetupGenericPerk<MechanicalArms>(UPGRADE_NAME);
        }
        public new static CustomTerminalNode RegisterTerminalNode()
        {
            return UpgradeBus.Instance.SetupMultiplePurchaseableTerminalNode(UPGRADE_NAME, GetConfiguration().MechanicalArmsConfiguration);
        }
    }
}
