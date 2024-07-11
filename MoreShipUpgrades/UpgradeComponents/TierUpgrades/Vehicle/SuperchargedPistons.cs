using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc.TerminalNodes;
using MoreShipUpgrades.Misc;
using MoreShipUpgrades.Misc.Upgrades;
using MoreShipUpgrades.Misc.Util;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace MoreShipUpgrades.UpgradeComponents.TierUpgrades.Vehicle
{
    internal class SuperchargedPistons : TierUpgrade
    {
        internal const string UPGRADE_NAME = "Supercharged Pistons";
        internal const string PRICES_DEFAULT = "200,350";

        internal override void Start()
        {
            upgradeName = UPGRADE_NAME;
            overridenUpgradeName = UpgradeBus.Instance.PluginConfiguration.SUPERCHARGED_PISTONS_OVERRIDE_NAME;
            base.Start();
        }
        public override void Load()
        {
            base.Load();
            UpdateCurrentVehicleEngineTorque(ComputeAdditionalEngineTorque(), add: true);
        }

        public override void Increment()
        {
            base.Increment();
            UpdateCurrentVehicleEngineTorque(UpgradeBus.Instance.PluginConfiguration.SUPERCHARGED_PISTONS_ENGINE_TORQUE_INCREMENTAL_INCREASE, add: true);
        }

        public override void Unwind()
        {
            UpdateCurrentVehicleEngineTorque(ComputeAdditionalEngineTorque(), add: false);
            base.Unwind();
        }
        void UpdateCurrentVehicleEngineTorque(float additionalEngineTorque, bool add)
        {
            foreach (VehicleController vehicle in FindObjectsOfType<VehicleController>())
            {
                if (vehicle == null) continue;
                if (add)
                    vehicle.EngineTorque += additionalEngineTorque;
                else
                    vehicle.EngineTorque -= additionalEngineTorque;
            }
        }

        public static float ComputeAdditionalEngineTorque()
        {
            return UpgradeBus.Instance.PluginConfiguration.SUPERCHARGED_PISTONS_ENGINE_TORQUE_INITIAL_INCREASE + (GetUpgradeLevel(UPGRADE_NAME) * UpgradeBus.Instance.PluginConfiguration.SUPERCHARGED_PISTONS_ENGINE_TORQUE_INCREMENTAL_INCREASE);
        }
        public static float GetAdditionalEngineTorque(float defaultValue)
        {
            if (!UpgradeBus.Instance.PluginConfiguration.SUPERCHARGED_PISTONS_ENABLED) return defaultValue;
            if (!GetActiveUpgrade(UPGRADE_NAME)) return defaultValue;
            float additionalValue = ComputeAdditionalEngineTorque();
            return Mathf.Clamp(defaultValue + additionalValue, defaultValue, float.MaxValue);
        }
        public override bool CanInitializeOnStart
        {
            get
            {
                string[] prices = UpgradeBus.Instance.PluginConfiguration.SUPERCHARGED_PISTONS_PRICES.Value.Split(',');
                return UpgradeBus.Instance.PluginConfiguration.SUPERCHARGED_PISTONS_PRICE.Value <= 0 && prices.Length == 1 && (prices[0] == "" || prices[0] == "0");
            }
        }
        public override string GetDisplayInfo(int initialPrice = -1, int maxLevels = -1, int[] incrementalPrices = null)
        {
            Func<int, float> infoFunction = level => UpgradeBus.Instance.PluginConfiguration.SUPERCHARGED_PISTONS_ENGINE_TORQUE_INITIAL_INCREASE.Value + level * UpgradeBus.Instance.PluginConfiguration.SUPERCHARGED_PISTONS_ENGINE_TORQUE_INCREMENTAL_INCREASE.Value;
            const string infoFormat = "LVL {0} - ${1} - Company Cruiser vehicle's maximum speed is increased by {2}.\n";
            return Tools.GenerateInfoForUpgrade(infoFormat, initialPrice, incrementalPrices, infoFunction);
        }
        public new static (string, string[]) RegisterScrapToUpgrade()
        {
            return (UPGRADE_NAME, UpgradeBus.Instance.PluginConfiguration.SUPERCHARGED_PISTONS_ITEM_PROGRESSION_ITEMS.Value.Split(","));
        }
        public new static void RegisterUpgrade()
        {
            SetupGenericPerk<SuperchargedPistons>(UPGRADE_NAME);
        }
        public new static CustomTerminalNode RegisterTerminalNode()
        {
            LategameConfiguration configuration = UpgradeBus.Instance.PluginConfiguration;

            return UpgradeBus.Instance.SetupMultiplePurchasableTerminalNode(UPGRADE_NAME,
                                                shareStatus: true,
                                                configuration.SUPERCHARGED_PISTONS_ENABLED.Value,
                                                configuration.SUPERCHARGED_PISTONS_PRICE.Value,
                                                UpgradeBus.ParseUpgradePrices(configuration.SUPERCHARGED_PISTONS_PRICES.Value),
                                                configuration.OVERRIDE_UPGRADE_NAMES ? configuration.SUPERCHARGED_PISTONS_OVERRIDE_NAME : "");
        }
    }
}
