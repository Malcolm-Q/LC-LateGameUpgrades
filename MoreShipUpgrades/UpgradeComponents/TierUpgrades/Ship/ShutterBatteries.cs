using MoreShipUpgrades.Configuration;
using MoreShipUpgrades.Configuration.Interfaces.TierUpgrades;
using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc.Upgrades;
using MoreShipUpgrades.Misc.Util;
using MoreShipUpgrades.UI.TerminalNodes;
using MoreShipUpgrades.UpgradeComponents.Interfaces;
using System;
using UnityEngine;

namespace MoreShipUpgrades.UpgradeComponents.TierUpgrades
{
    internal class ShutterBatteries : TierUpgrade, IUpgradeWorldBuilding
    {
        public const string UPGRADE_NAME = "Shutter Batteries";
        public const string PRICES_DEFAULT = "300,200,300,400";

        public const string ENABLED_SECTION = $"Enable {UPGRADE_NAME}";
        public const string ENABLED_DESCRIPTION = "Increases the amount of time the doors can remain shut";

        public const string PRICE_SECTION = $"Price of {UPGRADE_NAME}";
        public const int PRICE_DEFAULT = 300;

        public const string INITIAL_SECTION = "Initial battery boost";
        public const float INITIAL_DEFAULT = 5f;
        public const string INITIAL_DESCRIPTION = "Initial battery boost for the doors' lock on first purchase";

        public const string INCREMENTAL_SECTION = "Incremental battery boost";
        public const float INCREMENTAL_DEFAULT = 5f;
        public const string INCREMENTAL_DESCRIPTION = "Incremental battery boost for the doors' lock after purchase";

        internal const string WORLD_BUILDING_TEXT = "\n\nService package of maintenance procedures for your Ship's Proprietary Emergency Lockout Door System, the two-button monitor you control the Ship's airlock from while moonside. Opting into all of the procedures will improve the uptime of the lockout function, which may improve your department's Fatality Record Over Time by up to 11%.\n\n";

        public string GetWorldBuildingText(bool shareStatus = false)
        {
            return WORLD_BUILDING_TEXT;
        }
        void Awake()
        {
            upgradeName = UPGRADE_NAME;
            overridenUpgradeName = GetConfiguration().ShutterBatteriesConfiguration.OverrideName;
        }
        public override string GetDisplayInfo(int initialPrice = -1, int maxLevels = -1, int[] incrementalPrices = null)
        {
            static float infoFunction(int level)
            {
                ITierEffectUpgradeConfiguration<float> config = GetConfiguration().ShutterBatteriesConfiguration;
                return config.InitialEffect.Value + (level * config.IncrementalEffect.Value);
            }
            const string infoFormat = "LVL {0} - ${1} - Increases the door's hydraulic capacity to remain closed by {2} units\n";
            return Tools.GenerateInfoForUpgrade(infoFormat, initialPrice, incrementalPrices, infoFunction);
        }
        public override bool CanInitializeOnStart
        {
            get
            {
                ITierEffectUpgradeConfiguration<float> upgradeConfig = GetConfiguration().ShutterBatteriesConfiguration;
                string[] prices = upgradeConfig.Prices.Value.Split(',');
                return prices.Length == 0 || (prices.Length == 1 && (prices[0].Length == 0 || prices[0] == "0"));
            }
        }
        public static float GetAdditionalDoorTime(float defaultValue)
        {
            ITierEffectUpgradeConfiguration<float> config = GetConfiguration().ShutterBatteriesConfiguration;
            if (!config.Enabled) return defaultValue;
            if (!GetActiveUpgrade(UPGRADE_NAME)) return defaultValue;
            float additionalValue = config.InitialEffect + (GetUpgradeLevel(UPGRADE_NAME) * config.IncrementalEffect);
            return Mathf.Clamp(defaultValue + additionalValue, defaultValue, float.MaxValue);
        }

        public new static (string, string[]) RegisterScrapToUpgrade()
        {
            return (UPGRADE_NAME, GetConfiguration().ShutterBatteriesConfiguration.ItemProgressionItems.Value.Split(","));
        }
        public new static void RegisterUpgrade()
        {
            SetupGenericPerk<ShutterBatteries>(UPGRADE_NAME);
        }
        public new static CustomTerminalNode RegisterTerminalNode()
        {
            return UpgradeBus.Instance.SetupMultiplePurchaseableTerminalNode(UPGRADE_NAME, GetConfiguration().ShutterBatteriesConfiguration);
        }
    }
}
