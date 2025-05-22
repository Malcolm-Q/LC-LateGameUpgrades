using CSync.Lib;
using MoreShipUpgrades.Configuration;
using MoreShipUpgrades.Configuration.Upgrades.Interfaces.TierUpgrades;
using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc;
using MoreShipUpgrades.Misc.Upgrades;
using MoreShipUpgrades.Misc.Util;
using MoreShipUpgrades.UI.TerminalNodes;
using MoreShipUpgrades.UpgradeComponents.Interfaces;
using UnityEngine;

namespace MoreShipUpgrades.UpgradeComponents.TierUpgrades
{
    class Beekeeper : TierUpgrade, IUpgradeWorldBuilding
    {
        internal static Beekeeper Instance;

        public const string UPGRADE_NAME = "Beekeeper";
        public const string PRICES_DEFAULT = "450,225,280,340";
        internal const string WORLD_BUILDING_TEXT = "\n\nOn-the-job training package that instructs {0} how to more safely and efficiently handle Circuit Bee Nests." +
            " Departments with a LVL {1} Certification in Circuit Bee Nest Handling earn an extra commission for every Nest they sell.\n\n";

        protected bool CanIncreaseHivePrice => GetUpgradeLevel(UPGRADE_NAME) == GetConfiguration().BeekeeperConfiguration.Prices.Value.Split(',').Length;

        void Awake()
        {
            upgradeName = UPGRADE_NAME;
            overridenUpgradeName = GetConfiguration().BeekeeperConfiguration.OverrideName;
            Instance = this;
        }

        public static int CalculateBeeDamage(int damageNumber)
        {
            ITierMultipleEffectUpgradeConfiguration<float> config = GetConfiguration().BeekeeperConfiguration;
            if (!config.Enabled) return damageNumber;
            if (!GetActiveUpgrade(UPGRADE_NAME)) return damageNumber;
            (SyncedEntry<float>, SyncedEntry<float>) damageReductionPair = config.GetEffectPair(0);
            return Mathf.Clamp((int)(damageNumber * (damageReductionPair.Item1.Value - (GetUpgradeLevel(UPGRADE_NAME) * damageReductionPair.Item2.Value))), 0, damageNumber);
        }

        public static int GetHiveScrapValue(int originalValue)
        {
            ITierMultipleEffectUpgradeConfiguration<float> config = GetConfiguration().BeekeeperConfiguration;
            if (!config.Enabled.Value || !Instance.CanIncreaseHivePrice) return originalValue;
            (SyncedEntry<float>, SyncedEntry<float>) hiveIncreasePair = config.GetEffectPair(1);
            return (int)(originalValue * hiveIncreasePair.Item1.Value);
        }

        public string GetWorldBuildingText(bool shareStatus = false)
        {
            return string.Format(WORLD_BUILDING_TEXT, shareStatus ? "your crew" : "you", GetConfiguration().BeekeeperConfiguration.Prices.Value.Split(',').Length);
        }

        public override string GetDisplayInfo(int initialPrice = -1, int maxLevels = -1, int[] incrementalPrices = null)
        {
            static float infoFunction(int level)
            {
                ITierMultipleEffectUpgradeConfiguration<float> config = GetConfiguration().BeekeeperConfiguration;
                (SyncedEntry<float>, SyncedEntry<float>) damageReductionPair = config.GetEffectPair(0);
                return 100 * (damageReductionPair.Item1.Value - (level * damageReductionPair.Item2.Value));
            }
            string infoFormat = AssetBundleHandler.GetInfoFromJSON(UPGRADE_NAME);
            return Tools.GenerateInfoForUpgrade(infoFormat, initialPrice, incrementalPrices, infoFunction, purchaseMode: GetConfiguration().BeekeeperConfiguration.PurchaseMode) + $"\nOn maximum level, applies a {(GetConfiguration().BeekeeperConfiguration.GetEffectPair(1).Item1 - 1f)*100f:F0}% scrap value increase on beehives.";
        }

        public override bool CanInitializeOnStart
        {
            get
            {
                ITierMultipleEffectUpgradeConfiguration<float> upgradeConfig = GetConfiguration().BeekeeperConfiguration;
                string[] prices = upgradeConfig.Prices.Value.Split(',');
                return prices.Length == 0 || (prices.Length == 1 && (prices[0].Length == 0 || prices[0] == "0"));
            }
        }
        public new static (string, string[]) RegisterScrapToUpgrade()
        {
            return (UPGRADE_NAME, GetConfiguration().BeekeeperConfiguration.ItemProgressionItems.Value.Split(","));
        }
        public new static void RegisterUpgrade()
        {
            SetupGenericPerk<Beekeeper>(UPGRADE_NAME);
        }
        public new static CustomTerminalNode RegisterTerminalNode()
        {
            return UpgradeBus.Instance.SetupMultiplePurchaseableTerminalNode(UPGRADE_NAME, GetConfiguration().BeekeeperConfiguration);
        }
    }
}
