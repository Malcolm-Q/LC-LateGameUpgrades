using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc.Upgrades;
using MoreShipUpgrades.Misc.Util;
using UnityEngine;
using MoreShipUpgrades.UI.TerminalNodes;
using MoreShipUpgrades.UpgradeComponents.Interfaces;
using MoreShipUpgrades.Configuration;
using MoreShipUpgrades.Configuration.Interfaces.TierUpgrades;

namespace MoreShipUpgrades.UpgradeComponents.TierUpgrades.Player
{
    internal class HikingBoots : TierUpgrade, IUpgradeWorldBuilding
    {
        internal const string UPGRADE_NAME = "Hiking Boots";
        internal const string PRICES_DEFAULT = "75,100,150,175";
        internal const string WORLD_BUILDING_TEXT = "\n\nIn the 'GEAR' section of the Company Catalogue, the very last ad on the thirtieth page is for this product." +
            " It's a requisition of calf-length boots made of flexible synthetic fiber. Supports your ankles while walking on inclines, allowing your gait to adjust more effortlessly.\n\n";

        public string GetWorldBuildingText(bool shareStatus = false)
        {
            return WORLD_BUILDING_TEXT;
        }
        void Awake()
        {
            upgradeName = UPGRADE_NAME;
            overridenUpgradeName = GetConfiguration().HikingBootsConfiguration.OverrideName;
        }
        static float ComputeUphillSlopeDebuffMultiplier()
        {
            ITierEffectUpgradeConfiguration<int> upgradeConfig = GetConfiguration().HikingBootsConfiguration;
            return 1f - ((upgradeConfig.InitialEffect + (GetUpgradeLevel(UPGRADE_NAME) * upgradeConfig.IncrementalEffect))/100f);
        }
        public static float ReduceUphillSlopeDebuff(float defaultValue)
        {
            if (!GetConfiguration().HikingBootsConfiguration.Enabled) return defaultValue;
            if (!GetActiveUpgrade(UPGRADE_NAME)) return defaultValue;
            float multiplier = ComputeUphillSlopeDebuffMultiplier();
            return Mathf.Clamp(defaultValue * multiplier, 0f, defaultValue);
        }
        public override string GetDisplayInfo(int initialPrice = -1, int maxLevels = -1, int[] incrementalPrices = null)
        {
            static float infoFunction(int level)
            {
                ITierEffectUpgradeConfiguration<int> upgradeConfig = GetConfiguration().HikingBootsConfiguration;
                return upgradeConfig.InitialEffect.Value + (level * upgradeConfig.IncrementalEffect.Value);
            }
            const string infoFormat = "LVL {0} - {1} - Reduces the movement speed change when going through slopes by {2}%\n";
            return Tools.GenerateInfoForUpgrade(infoFormat, initialPrice, incrementalPrices, infoFunction, purchaseMode: GetConfiguration().HikingBootsConfiguration.PurchaseMode);
        }

        public override bool CanInitializeOnStart
        {
            get
            {
                ITierUpgradeConfiguration upgradeConfig = GetConfiguration().HikingBootsConfiguration;
                string[] prices = upgradeConfig.Prices.Value.Split(',');
                return prices.Length == 0 || (prices.Length == 1 && (prices[0].Length == 0 || prices[0] == "0"));
            }
        }

        public new static (string, string[]) RegisterScrapToUpgrade()
        {
            return (UPGRADE_NAME, GetConfiguration().HikingBootsConfiguration.ItemProgressionItems.Value.Split(","));
        }
        public new static void RegisterUpgrade()
        {
            SetupGenericPerk<HikingBoots>(UPGRADE_NAME);
        }
        public new static CustomTerminalNode RegisterTerminalNode()
        {
            return UpgradeBus.Instance.SetupMultiplePurchaseableTerminalNode(UPGRADE_NAME, GetConfiguration().HikingBootsConfiguration);
        }
    }
}
