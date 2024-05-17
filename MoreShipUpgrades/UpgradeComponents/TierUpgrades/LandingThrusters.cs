using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc;
using MoreShipUpgrades.Misc.Upgrades;
using MoreShipUpgrades.Misc.Util;
using MoreShipUpgrades.UpgradeComponents.Interfaces;
using UnityEngine;

namespace MoreShipUpgrades.UpgradeComponents.TierUpgrades
{
    internal class LandingThrusters : TierUpgrade, IServerSync
    {
        internal const string UPGRADE_NAME = "Landing Thrusters";
        internal const string DEFAULT_PRICES = "250,450,650";

        void Awake()
        {
            upgradeName = UPGRADE_NAME;
            overridenUpgradeName = UpgradeBus.Instance.PluginConfiguration.LANDING_THRUSTERS_OVERRIDE_NAME;
        }
        public static float GetInteractMutliplier()
        {
            if (!UpgradeBus.Instance.PluginConfiguration.LANDING_THRUSTERS_ENABLED) return 1f;
            if (!UpgradeBus.Instance.PluginConfiguration.LANDING_THRUSTERS_AFFECT_LANDING) return 1f;
            if (!GetActiveUpgrade(UPGRADE_NAME)) return 1f;
            return 1f + Mathf.Max(0f, (UpgradeBus.Instance.PluginConfiguration.LANDING_THRUSTERS_INITIAL_SPEED_INCREASE + (GetUpgradeLevel(UPGRADE_NAME) * UpgradeBus.Instance.PluginConfiguration.LANDING_THRUSTERS_INCREMENTAL_SPEED_INCREASE)) / 100f);
        }
        public static float GetLandingSpeedMultiplier()
        {
            if (!UpgradeBus.Instance.PluginConfiguration.LANDING_THRUSTERS_ENABLED) return 1f;
            if (!GetActiveUpgrade(UPGRADE_NAME)) return 1f;
            return 1f + Mathf.Max(0f, (UpgradeBus.Instance.PluginConfiguration.LANDING_THRUSTERS_INITIAL_SPEED_INCREASE + (GetUpgradeLevel(UPGRADE_NAME) * UpgradeBus.Instance.PluginConfiguration.LANDING_THRUSTERS_INCREMENTAL_SPEED_INCREASE)) / 100f);
        }
        public override string GetDisplayInfo(int initialPrice = -1, int maxLevels = -1, int[] incrementalPrices = null)
        {
            System.Func<int, float> infoFunction = level => UpgradeBus.Instance.PluginConfiguration.LANDING_THRUSTERS_INITIAL_SPEED_INCREASE.Value + (level * UpgradeBus.Instance.PluginConfiguration.LANDING_THRUSTERS_INCREMENTAL_SPEED_INCREASE.Value);
            string infoFormat = "LVL {0} - ${1} - Increases the ship's landing speed by {2}%\n";
            return Tools.GenerateInfoForUpgrade(infoFormat, initialPrice, incrementalPrices, infoFunction);
        }

        public override bool CanInitializeOnStart
        {
            get
            {
                string[] prices = UpgradeBus.Instance.PluginConfiguration.LANDING_THRUSTERS_PRICES.Value.Split(',');
                bool free = UpgradeBus.Instance.PluginConfiguration.LANDING_THRUSTERS_PRICE.Value <= 0 && prices.Length == 1 && (prices[0] == "" || prices[0] == "0");
                return free;
            }
        }

        public new static void RegisterUpgrade()
        {
            SetupGenericPerk<LandingThrusters>(UPGRADE_NAME);
        }
        public new static void RegisterTerminalNode()
        {
            LategameConfiguration configuration = UpgradeBus.Instance.PluginConfiguration;

            UpgradeBus.Instance.SetupMultiplePurchasableTerminalNode(UPGRADE_NAME,
                                                shareStatus: true,
                                                configuration.LANDING_THRUSTERS_ENABLED,
                                                configuration.LANDING_THRUSTERS_PRICE,
                                                UpgradeBus.ParseUpgradePrices(configuration.LANDING_THRUSTERS_PRICES),
                                                configuration.OVERRIDE_UPGRADE_NAMES ? configuration.LANDING_THRUSTERS_OVERRIDE_NAME : "");
        }
    }
}
