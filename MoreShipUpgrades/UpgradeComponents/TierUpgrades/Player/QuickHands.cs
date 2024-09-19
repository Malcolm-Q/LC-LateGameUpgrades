using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc;
using MoreShipUpgrades.Misc.Upgrades;
using MoreShipUpgrades.Misc.Util;
using MoreShipUpgrades.UI.TerminalNodes;
using UnityEngine;

namespace MoreShipUpgrades.UpgradeComponents.TierUpgrades.Player
{
    internal class QuickHands : TierUpgrade
    {
        internal const string UPGRADE_NAME = "Quick Hands";
        internal const string DEFAULT_PRICES = "100,150,200,250";
        void Awake()
        {
            upgradeName = UPGRADE_NAME;
            overridenUpgradeName = GetConfiguration().QUICK_HANDS_OVERRIDE_NAME;
        }
        public static float GetIncreasedInteractionSpeedMultiplier()
        {
            LategameConfiguration config = GetConfiguration();
            return (config.QUICK_HANDS_INITIAL_INTERACTION_SPEED_INCREASE + (GetUpgradeLevel(UPGRADE_NAME) * config.QUICK_HANDS_INCREMENTAL_INTERACTION_SPEED_INCREASE)) / 100f;
        }
        public static float IncreaseInteractionSpeed(float defaultValue)
        {
            LategameConfiguration config = GetConfiguration();
            if (!config.QUICK_HANDS_ENABLED) return defaultValue;
            if (!GetActiveUpgrade(UPGRADE_NAME)) return defaultValue;
            float multiplier = GetIncreasedInteractionSpeedMultiplier();
            return (int)Mathf.Clamp(defaultValue + (defaultValue * multiplier), defaultValue, float.MaxValue);
        }
        public override string GetDisplayInfo(int initialPrice = -1, int maxLevels = -1, int[] incrementalPrices = null)
        {
            static float infoFunction(int level)
            {
                LategameConfiguration config = GetConfiguration();
                return config.QUICK_HANDS_INITIAL_INTERACTION_SPEED_INCREASE.Value + (level * config.QUICK_HANDS_INCREMENTAL_INTERACTION_SPEED_INCREASE.Value);
            }
            const string infoFormat = "LVL {0} - ${1} - Increases interaction speed of the player by {2}%\n";
            return Tools.GenerateInfoForUpgrade(infoFormat, initialPrice, incrementalPrices, infoFunction);
        }

        public override bool CanInitializeOnStart
        {
            get
            {
                LategameConfiguration config = GetConfiguration();
                string[] prices = config.QUICK_HANDS_PRICES.Value.Split(',');
                return config.QUICK_HANDS_PRICE.Value <= 0 && prices.Length == 1 && (prices[0].Length == 0 || prices[0] == "0");
            }
        }

        public new static (string, string[]) RegisterScrapToUpgrade()
        {
            return (UPGRADE_NAME, GetConfiguration().QUICK_HANDS_ITEM_PROGRESSION_ITEMS.Value.Split(","));
        }
        public new static void RegisterUpgrade()
        {
            GameObject prefab = LethalLib.Modules.NetworkPrefabs.CreateNetworkPrefab(UPGRADE_NAME);
            prefab.AddComponent<QuickHands>();
            LethalLib.Modules.NetworkPrefabs.RegisterNetworkPrefab(prefab);
            Plugin.networkPrefabs[UPGRADE_NAME] = prefab;
        }
        public new static CustomTerminalNode RegisterTerminalNode()
        {
            LategameConfiguration configuration = GetConfiguration();

            return UpgradeBus.Instance.SetupMultiplePurchasableTerminalNode(UPGRADE_NAME,
                                                configuration.SHARED_UPGRADES || !configuration.QUICK_HANDS_INDIVIDUAL,
                                                configuration.QUICK_HANDS_ENABLED,
                                                configuration.QUICK_HANDS_PRICE,
                                                UpgradeBus.ParseUpgradePrices(configuration.QUICK_HANDS_PRICES),
                                                configuration.OVERRIDE_UPGRADE_NAMES ? configuration.QUICK_HANDS_OVERRIDE_NAME : "",
                                                Plugin.networkPrefabs[UPGRADE_NAME]);
        }
    }
}
