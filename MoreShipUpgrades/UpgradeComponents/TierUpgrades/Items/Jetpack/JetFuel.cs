using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc;
using MoreShipUpgrades.Misc.Upgrades;
using MoreShipUpgrades.Misc.Util;
using MoreShipUpgrades.UI.TerminalNodes;
using UnityEngine;

namespace MoreShipUpgrades.UpgradeComponents.TierUpgrades.Items.Jetpack
{
    internal class JetFuel : TierUpgrade
    {
        internal const string UPGRADE_NAME = "Jet Fuel";
        internal const string DEFAULT_PRICES = "200,400,500";

        internal override void Start()
        {
            upgradeName = UPGRADE_NAME;
            overridenUpgradeName = GetConfiguration().JET_FUEL_OVERRIDE_NAME;
            base.Start();
        }
        public override bool CanInitializeOnStart
        {
            get
            {
                LategameConfiguration config = GetConfiguration();
                string[] prices = config.JET_FUEL_PRICES.Value.Split(',');
                return config.JET_FUEL_PRICE.Value <= 0 && prices.Length == 1 && (prices[0].Length == 0 || prices[0] == "0");
            }
        }
        public override string GetDisplayInfo(int initialPrice = -1, int maxLevels = -1, int[] incrementalPrices = null)
        {
            static float infoFunction(int level)
            {
                LategameConfiguration config = GetConfiguration();
                return config.JET_FUEL_INITIAL_ACCELERATION_INCREASE.Value + (level * config.JET_FUEL_INCREMENTAL_ACCELERATION_INCREASE.Value);
            }
            const string infoFormat = "LVL {0} - ${1} - The acceleration of the jetpack during flight is increased by {2}%\n";
            return Tools.GenerateInfoForUpgrade(infoFormat, initialPrice, incrementalPrices, infoFunction);
        }
        public static float GetIncreasedAcceleration()
        {
            LategameConfiguration config = GetConfiguration();
            int percentage = config.JET_FUEL_INITIAL_ACCELERATION_INCREASE + (GetUpgradeLevel(UPGRADE_NAME) * config.JET_FUEL_INCREMENTAL_ACCELERATION_INCREASE);
            return percentage / 100f;
        }
        public static float IncreaseJetpackAcceleration(float defaultValue)
        {
            if (!GetConfiguration().JET_FUEL_ENABLED) return defaultValue;
            if (!GetActiveUpgrade(UPGRADE_NAME)) return defaultValue;
            float multiplier = GetIncreasedAcceleration();
            return Mathf.Clamp(defaultValue + (defaultValue * multiplier), defaultValue, float.MaxValue);
        }
        public new static (string, string[]) RegisterScrapToUpgrade()
        {
            return (UPGRADE_NAME, GetConfiguration().JET_FUEL_ITEM_PROGRESSION_ITEMS.Value.Split(","));
        }
        public new static void RegisterUpgrade()
        {
            GameObject prefab = LethalLib.Modules.NetworkPrefabs.CreateNetworkPrefab(UPGRADE_NAME);
            prefab.AddComponent<JetFuel>();
            LethalLib.Modules.NetworkPrefabs.RegisterNetworkPrefab(prefab);
            Plugin.networkPrefabs[UPGRADE_NAME] = prefab;
        }
        public new static CustomTerminalNode RegisterTerminalNode()
        {
            LategameConfiguration configuration = GetConfiguration();

            return UpgradeBus.Instance.SetupMultiplePurchasableTerminalNode(UPGRADE_NAME,
                                                shareStatus: configuration.SHARED_UPGRADES.Value || !configuration.JET_FUEL_INDIVIDUAL.Value,
                                                configuration.JET_FUEL_ENABLED,
                                                configuration.JET_FUEL_PRICE,
                                                UpgradeBus.ParseUpgradePrices(configuration.JET_FUEL_PRICES),
                                                configuration.OVERRIDE_UPGRADE_NAMES ? configuration.JET_FUEL_OVERRIDE_NAME : "",
                                                Plugin.networkPrefabs[UPGRADE_NAME]);
        }
    }
}
