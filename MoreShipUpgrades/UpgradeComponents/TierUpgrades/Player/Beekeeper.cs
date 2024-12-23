using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc;
using MoreShipUpgrades.Misc.Upgrades;
using MoreShipUpgrades.Misc.Util;
using MoreShipUpgrades.UI.TerminalNodes;
using MoreShipUpgrades.UpgradeComponents.Interfaces;
using Unity.Netcode;
using UnityEngine;

namespace MoreShipUpgrades.UpgradeComponents.TierUpgrades
{
    class Beekeeper : TierUpgrade, IUpgradeWorldBuilding
    {
        internal static Beekeeper Instance;

        public const string UPGRADE_NAME = "Beekeeper";
        public const string PRICES_DEFAULT = "225,280,340";
        internal const string WORLD_BUILDING_TEXT = "\n\nOn-the-job training package that instructs {0} how to more safely and efficiently handle Circuit Bee Nests." +
            " Departments with a LVL {1} Certification in Circuit Bee Nest Handling earn an extra commission for every Nest they sell.\n\n";

        protected bool CanIncreaseHivePrice => GetUpgradeLevel(UPGRADE_NAME) == GetConfiguration().BEEKEEPER_UPGRADE_PRICES.Value.Split(',').Length;

        void Awake()
        {
            upgradeName = UPGRADE_NAME;
            overridenUpgradeName = GetConfiguration().BEEKEEPER_OVERRIDE_NAME;
            Instance = this;
        }

        public static int CalculateBeeDamage(int damageNumber)
        {
            LategameConfiguration config = GetConfiguration();
            if (!config.BEEKEEPER_ENABLED) return damageNumber;
            if (!GetActiveUpgrade(UPGRADE_NAME)) return damageNumber;
            return Mathf.Clamp((int)(damageNumber * (config.BEEKEEPER_DAMAGE_MULTIPLIER.Value - (GetUpgradeLevel(UPGRADE_NAME) * config.BEEKEEPER_DAMAGE_MULTIPLIER_INCREMENT.Value))), 0, damageNumber);
        }

        public static int GetHiveScrapValue(int originalValue)
        {
            LategameConfiguration config = GetConfiguration();
            if (!config.BEEKEEPER_ENABLED.Value || !Instance.CanIncreaseHivePrice) return originalValue;
            return (int)(originalValue * config.BEEKEEPER_HIVE_VALUE_INCREASE.Value);
        }

        public string GetWorldBuildingText(bool shareStatus = false)
        {
            return string.Format(WORLD_BUILDING_TEXT, shareStatus ? "your crew" : "you", GetConfiguration().BEEKEEPER_UPGRADE_PRICES.Value.Split(',').Length);
        }

        public override string GetDisplayInfo(int initialPrice = -1, int maxLevels = -1, int[] incrementalPrices = null)
        {
            static float infoFunction(int level)
            {
                LategameConfiguration config = GetConfiguration();
                return 100 * (config.BEEKEEPER_DAMAGE_MULTIPLIER.Value - (level * config.BEEKEEPER_DAMAGE_MULTIPLIER_INCREMENT.Value));
            }
            string infoFormat = AssetBundleHandler.GetInfoFromJSON(UPGRADE_NAME);
            return Tools.GenerateInfoForUpgrade(infoFormat, initialPrice, incrementalPrices, infoFunction) + $"\nOn maximum level, applies a {(GetConfiguration().BEEKEEPER_HIVE_VALUE_INCREASE - 1f)*100f:F0}% scrap value increase on beehives.";
        }

        public override bool CanInitializeOnStart
        {
            get
            {
                LategameConfiguration config = GetConfiguration();
                string[] prices = config.BEEKEEPER_UPGRADE_PRICES.Value.Split(',');
                return config.BEEKEEPER_PRICE.Value <= 0 && prices.Length == 1 && (prices[0].Length == 0 || prices[0] == "0");
            }
        }
        public new static (string, string[]) RegisterScrapToUpgrade()
        {
            return (UPGRADE_NAME, GetConfiguration().BEEKEEPER_ITEM_PROGRESSION_ITEMS.Value.Split(","));
        }
        public new static void RegisterUpgrade()
        {
            SetupGenericPerk<Beekeeper>(UPGRADE_NAME);
        }
        public new static CustomTerminalNode RegisterTerminalNode()
        {
            LategameConfiguration configuration = GetConfiguration();

            return UpgradeBus.Instance.SetupMultiplePurchasableTerminalNode(
                             UPGRADE_NAME,
                            configuration.SHARED_UPGRADES.Value || !configuration.BEEKEEPER_INDIVIDUAL.Value,
                            configuration.BEEKEEPER_ENABLED.Value,
                            configuration.BEEKEEPER_PRICE.Value,
                            UpgradeBus.ParseUpgradePrices(configuration.BEEKEEPER_UPGRADE_PRICES.Value),
                            configuration.OVERRIDE_UPGRADE_NAMES ? configuration.BEEKEEPER_OVERRIDE_NAME : "");
        }
    }
}
