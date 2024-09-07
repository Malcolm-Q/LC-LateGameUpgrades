using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc.TerminalNodes;
using MoreShipUpgrades.Misc;
using MoreShipUpgrades.Misc.Upgrades;
using MoreShipUpgrades.Misc.Util;
using UnityEngine;

namespace MoreShipUpgrades.UpgradeComponents.TierUpgrades.Player
{
    internal class CarbonKneejoints : TierUpgrade
    {
        internal const string UPGRADE_NAME = "Carbon Kneejoints";
        internal const string DEFAULT_PRICES = "50,100,150";
        void Awake()
        {
            upgradeName = UPGRADE_NAME;
            overridenUpgradeName = UpgradeBus.Instance.PluginConfiguration.CARBON_KNEEJOINTS_OVERRIDE_NAME;
        }
        public static float CalculateDecreaseMultiplier()
        {
            if (!UpgradeBus.Instance.PluginConfiguration.CARBON_KNEEJOINTS_ENABLED || !GetActiveUpgrade(UPGRADE_NAME)) return 0f;
            return (UpgradeBus.Instance.PluginConfiguration.CARBON_KNEEJOINTS_INITIAL_CROUCH_DEBUFF_DECREASE + (UpgradeBus.Instance.PluginConfiguration.CARBON_KNEEJOINTS_INCREMENTAL_CROUCH_DEBUFF_DECREASE * GetUpgradeLevel(UPGRADE_NAME))) / 100f;
        }
        public static float ReduceCrouchMovementSpeedDebuff(float defaultValue)
        {
            float decreaseMultiplier = CalculateDecreaseMultiplier();
            float multipliedValue = 1f - defaultValue; // Being less than 1 means it makes the player faster while crouching
            return defaultValue - (Mathf.Clamp(1f - decreaseMultiplier, 0f, 1f) * multipliedValue);
        }
        public override string GetDisplayInfo(int initialPrice = -1, int maxLevels = -1, int[] incrementalPrices = null)
        {
            static float infoFunction(int level) => UpgradeBus.Instance.PluginConfiguration.CARBON_KNEEJOINTS_INITIAL_CROUCH_DEBUFF_DECREASE.Value + (level * UpgradeBus.Instance.PluginConfiguration.CARBON_KNEEJOINTS_INCREMENTAL_CROUCH_DEBUFF_DECREASE.Value);
            const string infoFormat = "LVL {0} - ${1} - Reduces the movement speed loss while crouching by {2}%\n";
            return Tools.GenerateInfoForUpgrade(infoFormat, initialPrice, incrementalPrices, infoFunction);
        }

        public override bool CanInitializeOnStart
        {
            get
            {
                string[] prices = UpgradeBus.Instance.PluginConfiguration.CARBON_KNEEJOINTS_PRICES.Value.Split(',');
                return UpgradeBus.Instance.PluginConfiguration.CARBON_KNEEJOINTS_PRICE.Value <= 0 && prices.Length == 1 && (prices[0].Length == 0 || prices[0] == "0");
            }
        }

        public new static (string, string[]) RegisterScrapToUpgrade()
        {
            return (UPGRADE_NAME, UpgradeBus.Instance.PluginConfiguration.CARBON_KNEEJOINTS_ITEM_PROGRESSION_ITEMS.Value.Split(","));
        }
        public new static void RegisterUpgrade()
        {
            GameObject prefab = LethalLib.Modules.NetworkPrefabs.CreateNetworkPrefab(UPGRADE_NAME);
            prefab.AddComponent<CarbonKneejoints>();
            LethalLib.Modules.NetworkPrefabs.RegisterNetworkPrefab(prefab);
            Plugin.networkPrefabs[UPGRADE_NAME] = prefab;
        }
        public new static CustomTerminalNode RegisterTerminalNode()
        {
            LategameConfiguration configuration = UpgradeBus.Instance.PluginConfiguration;

            return UpgradeBus.Instance.SetupMultiplePurchasableTerminalNode(UPGRADE_NAME,
                                                configuration.SHARED_UPGRADES || !configuration.CARBON_KNEEJOINTS_INDIVIDUAL,
                                                configuration.CARBON_KNEEJOINTS_ENABLED,
                                                configuration.CARBON_KNEEJOINTS_PRICE,
                                                UpgradeBus.ParseUpgradePrices(configuration.CARBON_KNEEJOINTS_PRICES),
                                                configuration.OVERRIDE_UPGRADE_NAMES ? configuration.CARBON_KNEEJOINTS_OVERRIDE_NAME : "",
                                                Plugin.networkPrefabs[UPGRADE_NAME]);
        }
    }
}
