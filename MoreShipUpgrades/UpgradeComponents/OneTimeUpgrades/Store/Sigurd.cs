using MoreShipUpgrades.Configuration;
using MoreShipUpgrades.Configuration.Upgrades.Custom;
using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc.Upgrades;
using MoreShipUpgrades.UI.TerminalNodes;
using MoreShipUpgrades.UpgradeComponents.Interfaces;
using UnityEngine;

namespace MoreShipUpgrades.UpgradeComponents.OneTimeUpgrades.Store
{
    public class Sigurd : OneTimeUpgrade, IUpgradeWorldBuilding
    {
        public const string UPGRADE_NAME = "Sigurd Access";
        internal const string WORLD_BUILDING_TEXT = "\n\nSigurd always laughed at Desmond when he remembered the stories about The Company paying 120% of the value of the" +
            " scrap. Before disappearing, Sigurd found a module for the ship's terminal. After installing this mysterious module, the last scrap sale that Sigurd made" +
            " was recorded at a value above normal.";
        public static Sigurd Instance;

        internal override void Start()
        {
            upgradeName = UPGRADE_NAME;
            overridenUpgradeName = GetConfiguration().SigurdAccessConfiguration.OverrideName;
            base.Start();
        }

        void Awake()
        {
            Instance = this;
        }

        public enum FunctionModes
        {
            LastDay,
            AllDays,
        }

        public static float GetBuyingRate(float defaultValue)
        {
            SigurdAccessUpgradeConfiguration config = GetConfiguration().SigurdAccessConfiguration;
            if (!config.Enabled.Value) return defaultValue;
            if (!GetActiveUpgrade(UPGRADE_NAME)) return defaultValue;
            switch(config.AlternativeMode.Value)
            {
                case FunctionModes.LastDay:
                    {
                        if (TimeOfDay.Instance.daysUntilDeadline != 0) return defaultValue;
                        break;
                    }
                case FunctionModes.AllDays:
                    {
                        break;
                    }
            }

            System.Random random = new(StartOfRound.Instance.randomMapSeed);
            if (random.Next(0, 100) < Mathf.Clamp(config.Chance.Value, 0, 100))
                return defaultValue + (config.Effect.Value / 100f);
            return defaultValue;
        }

        public override string GetDisplayInfo(int price = -1)
        {
            return $"{GetUpgradePrice(price, GetConfiguration().SigurdAccessConfiguration.PurchaseMode)} - There's a chance that the company will pay more.";
        }

        public string GetWorldBuildingText(bool shareStatus = false)
        {
            return WORLD_BUILDING_TEXT;
        }

        public override bool CanInitializeOnStart => (GetConfiguration().SigurdAccessConfiguration.Enabled.Value) && GetConfiguration().SigurdAccessConfiguration.Price.Value <= 0;
        public new static (string, string[]) RegisterScrapToUpgrade()
        {
            return (UPGRADE_NAME, GetConfiguration().SigurdAccessConfiguration.ItemProgressionItems.Value.Split(","));
        }
        public new static void RegisterUpgrade()
        {
            SetupGenericPerk<Sigurd>(UPGRADE_NAME);
        }
        public new static CustomTerminalNode RegisterTerminalNode()
        {
            return UpgradeBus.Instance.SetupOneTimeTerminalNode(UPGRADE_NAME, GetConfiguration().SigurdAccessConfiguration);
        }
    }
}
