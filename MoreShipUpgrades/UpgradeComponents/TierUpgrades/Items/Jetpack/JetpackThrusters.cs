using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc;
using MoreShipUpgrades.Misc.Upgrades;
using MoreShipUpgrades.Misc.Util;
using MoreShipUpgrades.UI.TerminalNodes;
using UnityEngine;

namespace MoreShipUpgrades.UpgradeComponents.TierUpgrades.Items.Jetpack
{
    internal class JetpackThrusters : TierUpgrade
    {
        internal const string UPGRADE_NAME = "Jetpack Thrusters";
        internal const string DEFAULT_PRICES = "150,300,400";

        internal override void Start()
        {
            upgradeName = UPGRADE_NAME;
            overridenUpgradeName = GetConfiguration().JETPACK_THRUSTERS_OVERRIDE_NAME;
            base.Start();
        }
        public override bool CanInitializeOnStart
        {
            get
            {
                LategameConfiguration config = GetConfiguration();
                string[] prices = config.JETPACK_THRUSTERS_PRICES.Value.Split(',');
                return config.JETPACK_THRUSTERS_PRICE.Value <= 0 && prices.Length == 1 && (prices[0].Length == 0 || prices[0] == "0");
            }
        }
        public override string GetDisplayInfo(int initialPrice = -1, int maxLevels = -1, int[] incrementalPrices = null)
        {
            static float infoFunction(int level)
            {
                LategameConfiguration config = GetConfiguration();
                return config.JETPACK_THRUSTERS_INITIAL_MAXIMUM_POWER_INCREASE.Value + (level * config.JETPACK_THRUSTERS_INCREMENTAL_MAXIMUM_POWER_INCREASE.Value);
            }
            const string infoFormat = "LVL {0} - ${1} - The acceleration of the jetpack during flight is increased by {2}%\n";
            return Tools.GenerateInfoForUpgrade(infoFormat, initialPrice, incrementalPrices, infoFunction);
        }
        public static float GetIncreasedMaximumPower()
        {
            LategameConfiguration config = GetConfiguration();
            int percentage = config.JETPACK_THRUSTERS_INITIAL_MAXIMUM_POWER_INCREASE + (GetUpgradeLevel(UPGRADE_NAME) * config.JETPACK_THRUSTERS_INCREMENTAL_MAXIMUM_POWER_INCREASE);
            return percentage / 100f;
        }
        public static float IncreaseJetpackMaximumPower(float defaultValue)
        {
            if (!GetConfiguration().JET_FUEL_ENABLED) return defaultValue;
            if (!GetActiveUpgrade(UPGRADE_NAME)) return defaultValue;
            float multiplier = GetIncreasedMaximumPower();
            return Mathf.Clamp(defaultValue + (defaultValue * multiplier), defaultValue, float.MaxValue);
        }
        public new static (string, string[]) RegisterScrapToUpgrade()
        {
            return (UPGRADE_NAME, GetConfiguration().JET_FUEL_ITEM_PROGRESSION_ITEMS.Value.Split(","));
        }
        public new static void RegisterUpgrade()
        {
            GameObject prefab = LethalLib.Modules.NetworkPrefabs.CreateNetworkPrefab(UPGRADE_NAME);
            prefab.AddComponent<JetpackThrusters>();
            LethalLib.Modules.NetworkPrefabs.RegisterNetworkPrefab(prefab);
            Plugin.networkPrefabs[UPGRADE_NAME] = prefab;
        }
        public new static CustomTerminalNode RegisterTerminalNode()
        {
            LategameConfiguration configuration = GetConfiguration();

            return UpgradeBus.Instance.SetupMultiplePurchasableTerminalNode(UPGRADE_NAME,
                                                shareStatus: configuration.SHARED_UPGRADES.Value || !configuration.JETPACK_THURSTERS_INDIVIDUAL.Value,
                                                configuration.JETPACK_THRUSTERS_ENABLED,
                                                configuration.JETPACK_THRUSTERS_PRICE,
                                                UpgradeBus.ParseUpgradePrices(configuration.JETPACK_THRUSTERS_PRICES),
                                                configuration.OVERRIDE_UPGRADE_NAMES ? configuration.JETPACK_THRUSTERS_OVERRIDE_NAME : "",
                                                Plugin.networkPrefabs[UPGRADE_NAME]);
        }
    }
}
