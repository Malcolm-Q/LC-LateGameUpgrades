using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc.TerminalNodes;
using MoreShipUpgrades.Misc;
using MoreShipUpgrades.Misc.Upgrades;
using MoreShipUpgrades.Misc.Util;
using UnityEngine;

namespace MoreShipUpgrades.UpgradeComponents.TierUpgrades.AttributeUpgrades
{
    internal class RapidMotors : TierUpgrade
    {
        internal const string UPGRADE_NAME = "Rapid Motors";
        internal const string PRICES_DEFAULT = "200,300,400";

        internal override void Start()
        {
            upgradeName = UPGRADE_NAME;
            overridenUpgradeName = UpgradeBus.Instance.PluginConfiguration.RAPID_MOTORS_OVERRIDE_NAME;
            base.Start();
        }
        public override void Load()
        {
            base.Load();
            UpdateCurrentVehicleAcceleration(ComputeAdditionalAcceleration(), add: true);
        }

        public override void Increment()
        {
            base.Increment();
            UpdateCurrentVehicleAcceleration(UpgradeBus.Instance.PluginConfiguration.RAPID_MOTORS_ACCELERATION_INCREMENTAL_INCREASE, add: true);
        }

        public override void Unwind()
        {
            UpdateCurrentVehicleAcceleration(ComputeAdditionalAcceleration(), add: false);
            base.Unwind();
        }
        void UpdateCurrentVehicleAcceleration(float additionalAcceleration, bool add)
        {
            foreach (VehicleController vehicle in FindObjectsOfType<VehicleController>())
            {
                if (vehicle == null) continue;
                if (add)
                    vehicle.carAcceleration += additionalAcceleration;
                else
                    vehicle.carAcceleration -= additionalAcceleration;
            }
        }
        public static float ComputeAdditionalAcceleration()
        {
            return UpgradeBus.Instance.PluginConfiguration.RAPID_MOTORS_ACCELERATION_INITIAL_INCREASE + (GetUpgradeLevel(UPGRADE_NAME) * UpgradeBus.Instance.PluginConfiguration.RAPID_MOTORS_ACCELERATION_INCREMENTAL_INCREASE);
        }
        public static float GetAdditionalAcceleration(float defaultValue)
        {
            if (!UpgradeBus.Instance.PluginConfiguration.RAPID_MOTORS_ENABLED) return defaultValue;
            if (!GetActiveUpgrade(UPGRADE_NAME)) return defaultValue;
            float additionalValue = ComputeAdditionalAcceleration();
            return Mathf.Clamp(defaultValue + additionalValue, defaultValue, float.MaxValue);
        }
        public override bool CanInitializeOnStart
        {
            get
            {
                string[] prices = UpgradeBus.Instance.PluginConfiguration.RAPID_MOTORS_PRICES.Value.Split(',');
                return UpgradeBus.Instance.PluginConfiguration.RAPID_MOTORS_PRICE.Value <= 0 && prices.Length == 1 && (prices[0] == "" || prices[0] == "0");
            }
        }
        public override string GetDisplayInfo(int initialPrice = -1, int maxLevels = -1, int[] incrementalPrices = null)
        {
            System.Func<int, float> infoFunction = level => UpgradeBus.Instance.PluginConfiguration.RAPID_MOTORS_ACCELERATION_INITIAL_INCREASE.Value + (level * UpgradeBus.Instance.PluginConfiguration.RAPID_MOTORS_ACCELERATION_INCREMENTAL_INCREASE.Value);
            const string infoFormat = "LVL {0} - ${1} - Company Cruiser vehicle's maximum acceleration is increased by {2}.\n";
            return Tools.GenerateInfoForUpgrade(infoFormat, initialPrice, incrementalPrices, infoFunction);
        }
        public new static (string, string[]) RegisterScrapToUpgrade()
        {
            return (UPGRADE_NAME, UpgradeBus.Instance.PluginConfiguration.RAPID_MOTORS_ITEM_PROGRESSION_ITEMS.Value.Split(","));
        }
        public new static void RegisterUpgrade()
        {
            SetupGenericPerk<RapidMotors>(UPGRADE_NAME);
        }
        public new static CustomTerminalNode RegisterTerminalNode()
        {
            LategameConfiguration configuration = UpgradeBus.Instance.PluginConfiguration;

            return UpgradeBus.Instance.SetupMultiplePurchasableTerminalNode(UPGRADE_NAME,
                                                shareStatus: true,
                                                configuration.RAPID_MOTORS_ENABLED.Value,
                                                configuration.RAPID_MOTORS_PRICE.Value,
                                                UpgradeBus.ParseUpgradePrices(configuration.RAPID_MOTORS_PRICES.Value),
                                                configuration.OVERRIDE_UPGRADE_NAMES ? configuration.RAPID_MOTORS_OVERRIDE_NAME : "");
        }
    }
}
