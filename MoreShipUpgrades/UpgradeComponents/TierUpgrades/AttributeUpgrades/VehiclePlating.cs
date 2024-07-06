using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc.TerminalNodes;
using MoreShipUpgrades.Misc;
using MoreShipUpgrades.Misc.Upgrades;
using MoreShipUpgrades.Misc.Util;
using UnityEngine;

namespace MoreShipUpgrades.UpgradeComponents.TierUpgrades.AttributeUpgrades
{
    internal class VehiclePlating : TierUpgrade
    {
        internal const string UPGRADE_NAME = "Vehicle Plating";
        internal const string PRICES_DEFAULT = "300,400,500,600";

        internal override void Start()
        {
            upgradeName = UPGRADE_NAME;
            overridenUpgradeName = UpgradeBus.Instance.PluginConfiguration.VEHICLE_PLATING_OVERRIDE_NAME;
            base.Start();
        }
        public override void Load()
        {
            base.Load();
            UpdateCurrentVehicleHealth(ComputeAdditionalMaximumHealth(), add: true);
        }

        public override void Increment()
        {
            base.Increment();
            UpdateCurrentVehicleHealth(UpgradeBus.Instance.PluginConfiguration.VEHICLE_PLATING_HEALTH_INCREMENTAL_INCREASE, add: true);
        }

        public override void Unwind()
        {
            UpdateCurrentVehicleHealth(ComputeAdditionalMaximumHealth(), add: false);
            base.Unwind();
        }
        public static int GetAdditionalMaximumHealth(int defaultValue)
        {
            if (!UpgradeBus.Instance.PluginConfiguration.VEHICLE_PLATING_ENABLED) return defaultValue;
            if (!GetActiveUpgrade(UPGRADE_NAME)) return defaultValue;
            int additionalValue = ComputeAdditionalMaximumHealth();
            return Mathf.Clamp(defaultValue + additionalValue, defaultValue, int.MaxValue);
        }

        void UpdateCurrentVehicleHealth(int additionalHealth, bool add)
        {
            foreach (VehicleController vehicle in FindObjectsOfType<VehicleController>())
            {
                if (vehicle == null) continue;
                if (add)
                    vehicle.carHP += additionalHealth;
                else
                    vehicle.carHP -= additionalHealth;
            }
        }

        public static int ComputeAdditionalMaximumHealth()
        {
            return UpgradeBus.Instance.PluginConfiguration.VEHICLE_PLATING_HEALTH_INITIAL_INCREASE + (GetUpgradeLevel(UPGRADE_NAME) * UpgradeBus.Instance.PluginConfiguration.VEHICLE_PLATING_HEALTH_INCREMENTAL_INCREASE);
        }
        public override bool CanInitializeOnStart
        {
            get
            {
                string[] prices = UpgradeBus.Instance.PluginConfiguration.VEHICLE_PLATING_PRICES.Value.Split(',');
                return UpgradeBus.Instance.PluginConfiguration.VEHICLE_PLATING_PRICE.Value <= 0 && prices.Length == 1 && (prices[0] == "" || prices[0] == "0");
            }
        }
        public override string GetDisplayInfo(int initialPrice = -1, int maxLevels = -1, int[] incrementalPrices = null)
        {
            System.Func<int, float> infoFunction = level => UpgradeBus.Instance.PluginConfiguration.VEHICLE_PLATING_HEALTH_INITIAL_INCREASE.Value + (level * UpgradeBus.Instance.PluginConfiguration.VEHICLE_PLATING_HEALTH_INCREMENTAL_INCREASE.Value);
            const string infoFormat = "LVL {0} - ${1} - Company Cruiser vehicle's maximum health is increased by {2}.\n";
            return Tools.GenerateInfoForUpgrade(infoFormat, initialPrice, incrementalPrices, infoFunction);
        }
        public new static (string, string[]) RegisterScrapToUpgrade()
        {
            return (UPGRADE_NAME, UpgradeBus.Instance.PluginConfiguration.VEHICLE_PLATING_ITEM_PROGRESSION_ITEMS.Value.Split(","));
        }
        public new static void RegisterUpgrade()
        {
            SetupGenericPerk<VehiclePlating>(UPGRADE_NAME);
        }
        public new static CustomTerminalNode RegisterTerminalNode()
        {
            LategameConfiguration configuration = UpgradeBus.Instance.PluginConfiguration;

            return UpgradeBus.Instance.SetupMultiplePurchasableTerminalNode(UPGRADE_NAME,
                                                shareStatus: true,
                                                configuration.VEHICLE_PLATING_ENABLED.Value,
                                                configuration.VEHICLE_PLATING_PRICE.Value,
                                                UpgradeBus.ParseUpgradePrices(configuration.VEHICLE_PLATING_PRICES.Value),
                                                configuration.OVERRIDE_UPGRADE_NAMES ? configuration.VEHICLE_PLATING_OVERRIDE_NAME : "");
        }
    }
}
