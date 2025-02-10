using CSync.Lib;
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
    internal class RunningShoes : TierUpgrade, IUpgradeWorldBuilding
    {
        public const string UPGRADE_NAME = "Running Shoes";
        public const string PRICES_DEFAULT = "650,500,750,1000";
        internal const string WORLD_BUILDING_TEXT = "\n\nA new pair of boots {0} a whole new lease on life. In this instance," +
            " it might also result in fewer wet sock incidents and consequent trenchfoot. After all, who knows how many people have walked in {1} shoes?\n\n";
        void Awake()
        {
            upgradeName = UPGRADE_NAME;
            overridenUpgradeName = GetConfiguration().RunningShoesConfiguration.OverrideName;
        }
        public static float ApplyPossibleReducedNoiseRange(float defaultValue)
        {
            ITierMultipleEffectUpgradeConfiguration<float> config = GetConfiguration().RunningShoesConfiguration;
            if (!(config.Enabled && GetActiveUpgrade(UPGRADE_NAME) && GetUpgradeLevel(UPGRADE_NAME) == config.Prices.Value.Split(',').Length)) return defaultValue;
            (SyncedEntry<float>, SyncedEntry<float>) noiseReductionPair = config.GetEffectPair(1);
            return Mathf.Clamp(defaultValue - noiseReductionPair.Item1.Value, 0f, defaultValue);
        }

        public string GetWorldBuildingText(bool shareStatus = false)
        {
            return string.Format(WORLD_BUILDING_TEXT, shareStatus ? "could give your crew" : "can give you", shareStatus ? "y'all's" : "your");
        }

        public override string GetDisplayInfo(int initialPrice = -1, int maxLevels = -1, int[] incrementalPrices = null)
        {
            static float infoFunction(int level)
            {
                ITierMultipleEffectUpgradeConfiguration<float> config = GetConfiguration().RunningShoesConfiguration;
                (SyncedEntry<float>, SyncedEntry<float>) movementSpeedPair = config.GetEffectPair(0);
                return movementSpeedPair.Item1.Value + (level * movementSpeedPair.Item2.Value);
            }
            string infoFormat = AssetBundleHandler.GetInfoFromJSON(UPGRADE_NAME);
            return Tools.GenerateInfoForUpgrade(infoFormat, initialPrice, incrementalPrices, infoFunction, purchaseMode: GetConfiguration().RunningShoesConfiguration.PurchaseMode);
        }

        public static float GetAdditionalMovementSpeed(float defaultValue)
        {
            ITierMultipleEffectUpgradeConfiguration<float> config = GetConfiguration().RunningShoesConfiguration;
            if (!config.Enabled) return defaultValue;
            if (!GetActiveUpgrade(UPGRADE_NAME)) return defaultValue;
            (SyncedEntry<float>, SyncedEntry<float>) movementSpeedPair = config.GetEffectPair(0);
            float additionalValue = movementSpeedPair.Item1 + (GetUpgradeLevel(UPGRADE_NAME) * movementSpeedPair.Item2);
            return Mathf.Clamp(defaultValue + additionalValue, defaultValue, float.MaxValue);
        }

        public override bool CanInitializeOnStart
        {
            get
            {
                ITierMultipleEffectUpgradeConfiguration<float> config = GetConfiguration().RunningShoesConfiguration;
                string[] prices = config.Prices.Value.Split(',');
                return prices.Length == 0 || (prices.Length == 1 && (prices[0].Length == 0 || prices[0] == "0"));
            }
        }

        public new static (string, string[]) RegisterScrapToUpgrade()
        {
            return (UPGRADE_NAME, GetConfiguration().RunningShoesConfiguration.ItemProgressionItems.Value.Split(","));
        }
        public new static void RegisterUpgrade()
        {
            SetupGenericPerk<RunningShoes>(UPGRADE_NAME);
        }
        public new static CustomTerminalNode RegisterTerminalNode()
        {
            return UpgradeBus.Instance.SetupMultiplePurchaseableTerminalNode(UPGRADE_NAME, GetConfiguration().RunningShoesConfiguration);
        }
    }
}
