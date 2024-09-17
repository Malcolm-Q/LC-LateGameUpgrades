using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc;
using MoreShipUpgrades.Misc.Upgrades;
using MoreShipUpgrades.UI.TerminalNodes;
using MoreShipUpgrades.UpgradeComponents.Interfaces;
using UnityEngine;

namespace MoreShipUpgrades.UpgradeComponents.OneTimeUpgrades.Store
{
    class Sigurd : OneTimeUpgrade, IUpgradeWorldBuilding
    {
        public const string UPGRADE_NAME = "Sigurd Access";
        internal const string WORLD_BUILDING_TEXT = "\n\nSigurd always laughed at Desmond when he remembered the stories about The Company paying 120% of the value of the" +
            " scrap. Before disappearing, Sigurd found a module for the ship's terminal. After installing this mysterious module, the last scrap sale that Sigurd made" +
            " was recorded at a value above normal.";
        public static Sigurd Instance;

        internal override void Start()
        {
            upgradeName = UPGRADE_NAME;
            overridenUpgradeName = GetConfiguration().SIGURD_ACCESS_OVERRIDE_NAME;
            base.Start();
        }

        void Awake()
        {
            Instance = this;
        }
        public static float GetBuyingRateLastDay(float defaultValue)
        {
            LategameConfiguration config = GetConfiguration();
            if (!config.SIGURD_LAST_DAY_ENABLED.Value) return defaultValue;
            if (!GetActiveUpgrade(UPGRADE_NAME)) return defaultValue;
            System.Random random = new(StartOfRound.Instance.randomMapSeed);
            if (random.Next(0, 100) < Mathf.Clamp(config.SIGURD_LAST_DAY_CHANCE.Value, 0, 100))
                return defaultValue + (config.SIGURD_LAST_DAY_PERCENT.Value / 100f);
            return defaultValue;
        }

        public static float GetBuyingRate(float defaultValue)
        {
            LategameConfiguration config = GetConfiguration();
            if (!config.SIGURD_ENABLED.Value) return defaultValue;
            if (!GetActiveUpgrade(UPGRADE_NAME)) return defaultValue;
            if (TimeOfDay.Instance.daysUntilDeadline == 0) return defaultValue;

            System.Random random = new(StartOfRound.Instance.randomMapSeed);
            if (random.Next(0, 100) < Mathf.Clamp(config.SIGURD_CHANCE.Value, 0, 100))
                return defaultValue + (config.SIGURD_PERCENT.Value / 100f);
            return defaultValue;
        }

        public override string GetDisplayInfo(int price = -1)
        {
            return $"${price} - There's a chance that the company will pay more.";
        }

        public string GetWorldBuildingText(bool shareStatus = false)
        {
            return WORLD_BUILDING_TEXT;
        }

        public override bool CanInitializeOnStart => (GetConfiguration().SIGURD_ENABLED.Value || GetConfiguration().SIGURD_LAST_DAY_ENABLED.Value) && GetConfiguration().SIGURD_PRICE.Value <= 0;
        public new static (string, string[]) RegisterScrapToUpgrade()
        {
            return (UPGRADE_NAME, GetConfiguration().SIGURD_ACCESS_ITEM_PROGRESSION_ITEMS.Value.Split(","));
        }
        public new static void RegisterUpgrade()
        {
            SetupGenericPerk<Sigurd>(UPGRADE_NAME);
        }
        public new static CustomTerminalNode RegisterTerminalNode()
        {
            LategameConfiguration configuration = GetConfiguration();

            return UpgradeBus.Instance.SetupOneTimeTerminalNode(UPGRADE_NAME,
                                    shareStatus: true,
                                    configuration.SIGURD_ENABLED.Value || configuration.SIGURD_LAST_DAY_ENABLED.Value,
                                    configuration.SIGURD_PRICE.Value,
                                    configuration.OVERRIDE_UPGRADE_NAMES ? configuration.SIGURD_ACCESS_OVERRIDE_NAME : "");
        }
    }
}
