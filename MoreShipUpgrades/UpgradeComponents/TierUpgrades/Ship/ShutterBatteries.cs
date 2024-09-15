using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc;
using MoreShipUpgrades.Misc.TerminalNodes;
using MoreShipUpgrades.Misc.Upgrades;
using MoreShipUpgrades.Misc.Util;
using System;
using UnityEngine;

namespace MoreShipUpgrades.UpgradeComponents.TierUpgrades
{
    internal class ShutterBatteries : TierUpgrade
    {
        public const string UPGRADE_NAME = "Shutter Batteries";
        public const string PRICES_DEFAULT = "200,300,400";

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
        void Awake()
        {
            upgradeName = UPGRADE_NAME;
            overridenUpgradeName = GetConfiguration().SHUTTER_BATTERIES_OVERRIDE_NAME;
        }
        public override string GetDisplayInfo(int initialPrice = -1, int maxLevels = -1, int[] incrementalPrices = null)
        {
            static float infoFunction(int level)
            {
                LategameConfiguration config = GetConfiguration();
                return config.DOOR_HYDRAULICS_BATTERY_INITIAL.Value + (level * config.DOOR_HYDRAULICS_BATTERY_INCREMENTAL.Value);
            }
            const string infoFormat = "LVL {0} - ${1} - Increases the door's hydraulic capacity to remain closed by {2} units\n";
            return Tools.GenerateInfoForUpgrade(infoFormat, initialPrice, incrementalPrices, infoFunction);
        }
        public override bool CanInitializeOnStart
        {
            get
            {
                LategameConfiguration config = GetConfiguration();
                string[] prices = config.DOOR_HYDRAULICS_BATTERY_PRICES.Value.Split(',');
                return config.DOOR_HYDRAULICS_BATTERY_PRICE.Value <= 0 && prices.Length == 1 && (prices[0].Length == 0 || prices[0] == "0");
            }
        }
        public static float GetAdditionalDoorTime(float defaultValue)
        {
            LategameConfiguration config = GetConfiguration();
            if (!config.DOOR_HYDRAULICS_BATTERY_ENABLED) return defaultValue;
            if (!GetActiveUpgrade(UPGRADE_NAME)) return defaultValue;
            float additionalValue = config.DOOR_HYDRAULICS_BATTERY_INITIAL + (GetUpgradeLevel(UPGRADE_NAME) * config.DOOR_HYDRAULICS_BATTERY_INCREMENTAL);
            return Mathf.Clamp(defaultValue + additionalValue, defaultValue, float.MaxValue);
        }

        public new static (string, string[]) RegisterScrapToUpgrade()
        {
            return (UPGRADE_NAME, GetConfiguration().SHUTTER_BATTERIES_ITEM_PROGRESSION_ITEMS.Value.Split(","));
        }
        public new static void RegisterUpgrade()
        {
            SetupGenericPerk<ShutterBatteries>(UPGRADE_NAME);
        }
        public new static CustomTerminalNode RegisterTerminalNode()
        {
            LategameConfiguration configuration = GetConfiguration();

            return UpgradeBus.Instance.SetupMultiplePurchasableTerminalNode(UPGRADE_NAME,
                                                shareStatus: true,
                                                configuration.DOOR_HYDRAULICS_BATTERY_ENABLED.Value,
                                                configuration.DOOR_HYDRAULICS_BATTERY_PRICE.Value,
                                                UpgradeBus.ParseUpgradePrices(configuration.DOOR_HYDRAULICS_BATTERY_PRICES.Value),
                                                configuration.OVERRIDE_UPGRADE_NAMES ? configuration.SHUTTER_BATTERIES_OVERRIDE_NAME : "");
        }
    }
}
