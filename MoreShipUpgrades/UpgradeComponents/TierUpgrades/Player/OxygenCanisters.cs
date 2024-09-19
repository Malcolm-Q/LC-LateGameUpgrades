using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc;
using MoreShipUpgrades.Misc.Upgrades;
using MoreShipUpgrades.Misc.Util;
using UnityEngine;
using MoreShipUpgrades.UI.TerminalNodes;

namespace MoreShipUpgrades.UpgradeComponents.TierUpgrades.Player
{
    public class OxygenCanisters : TierUpgrade
    {
        internal const string UPGRADE_NAME = "Oxygen Canisters";
        internal const string DEFAULT_PRICES = "100,200,400";
        void Awake()
        {
            upgradeName = UPGRADE_NAME;
            overridenUpgradeName = GetConfiguration().OXYGEN_CANISTERS_OVERRIDE_NAME;
        }
        public static float CalculateDecreaseMultiplier()
        {
            LategameConfiguration config = GetConfiguration();
            if (!config.OXYGEN_CANISTERS_ENABLED || !GetActiveUpgrade(UPGRADE_NAME)) return 0f;
            return (config.OXYGEN_CANISTERS_INITIAL_OXYGEN_CONSUMPTION_DECREASE + (config.OXYGEN_CANISTERS_INCREMENTAL_OXYGEN_CONSUMPTION_DECREASE * GetUpgradeLevel(UPGRADE_NAME)))/100f;
        }
        public static float ReduceOxygenConsumption(float defaultValue)
        {
            float decreaseMultiplier = CalculateDecreaseMultiplier();
            return Mathf.Clamp(1f -  decreaseMultiplier, 0f, 1f) * defaultValue;
        }
        public override string GetDisplayInfo(int initialPrice = -1, int maxLevels = -1, int[] incrementalPrices = null)
        {
            static float infoFunction(int level)
            {
                LategameConfiguration config = GetConfiguration();
                return config.OXYGEN_CANISTERS_INITIAL_OXYGEN_CONSUMPTION_DECREASE.Value + (level * config.OXYGEN_CANISTERS_INCREMENTAL_OXYGEN_CONSUMPTION_DECREASE.Value);
            }
            const string infoFormat = "LVL {0} - ${1} - Reduces oxygen consumption rate by {2}%\n";
            return Tools.GenerateInfoForUpgrade(infoFormat, initialPrice, incrementalPrices, infoFunction);
        }

        public override bool CanInitializeOnStart
        {
            get
            {
                LategameConfiguration config = GetConfiguration();
                string[] prices = config.OXYGEN_CANISTERS_PRICES.Value.Split(',');
                return config.OXYGEN_CANISTERS_PRICE.Value <= 0 && prices.Length == 1 && (prices[0].Length == 0 || prices[0] == "0");
            }
        }

        public new static (string, string[]) RegisterScrapToUpgrade()
        {
            return (UPGRADE_NAME, GetConfiguration().OXYGEN_CANISTERS_ITEM_PROGRESSION_ITEMS.Value.Split(","));
        }
        public new static void RegisterUpgrade()
        {
            GameObject prefab = LethalLib.Modules.NetworkPrefabs.CreateNetworkPrefab(UPGRADE_NAME);
            prefab.AddComponent<OxygenCanisters>();
            LethalLib.Modules.NetworkPrefabs.RegisterNetworkPrefab(prefab);
            Plugin.networkPrefabs[UPGRADE_NAME] = prefab;
        }
        public new static CustomTerminalNode RegisterTerminalNode()
        {
            LategameConfiguration configuration = GetConfiguration();

            return UpgradeBus.Instance.SetupMultiplePurchasableTerminalNode(UPGRADE_NAME,
                                                configuration.SHARED_UPGRADES || !configuration.OXYGEN_CANISTERS_INDIVIDUAL,
                                                configuration.OXYGEN_CANISTERS_ENABLED,
                                                configuration.OXYGEN_CANISTERS_PRICE,
                                                UpgradeBus.ParseUpgradePrices(configuration.OXYGEN_CANISTERS_PRICES),
                                                configuration.OVERRIDE_UPGRADE_NAMES ? configuration.OXYGEN_CANISTERS_OVERRIDE_NAME : "",
                                                Plugin.networkPrefabs[UPGRADE_NAME]);
        }
    }
}
