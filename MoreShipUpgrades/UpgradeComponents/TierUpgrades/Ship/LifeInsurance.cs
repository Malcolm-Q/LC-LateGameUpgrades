using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc.TerminalNodes;
using MoreShipUpgrades.Misc;
using MoreShipUpgrades.Misc.Upgrades;
using MoreShipUpgrades.Misc.Util;
using MoreShipUpgrades.UpgradeComponents.TierUpgrades.Player;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace MoreShipUpgrades.UpgradeComponents.TierUpgrades.Ship
{
    public class LifeInsurance : TierUpgrade
    {
        internal const string UPGRADE_NAME = "Life Insurance";
        internal const string DEFAULT_PRICES = "200,250,300";
        void Awake()
        {
            upgradeName = UPGRADE_NAME;
            overridenUpgradeName = UpgradeBus.Instance.PluginConfiguration.LIFE_INSURANCE_OVERRIDE_NAME;
        }
        public static float CalculateDecreaseMultiplier()
        {
            if (!UpgradeBus.Instance.PluginConfiguration.LIFE_INSURANCE_ENABLED || !GetActiveUpgrade(UPGRADE_NAME)) return 0f;
            return (UpgradeBus.Instance.PluginConfiguration.LIFE_INSURANCE_INITIAL_COST_PERCENTAGE_DECREASE + (UpgradeBus.Instance.PluginConfiguration.LIFE_INSURANCE_INCREMENTAL_COST_PERCENTAGE_DECREASE * GetUpgradeLevel(UPGRADE_NAME))) / 100f;
        }
        public static float ReduceCreditCostPercentage(float defaultValue)
        {
            float decreaseMultiplier = CalculateDecreaseMultiplier();
            return Mathf.Clamp(1f - decreaseMultiplier, 0f, 1f) * defaultValue;
        }
        public override string GetDisplayInfo(int initialPrice = -1, int maxLevels = -1, int[] incrementalPrices = null)
        {
            static float infoFunction(int level) => UpgradeBus.Instance.PluginConfiguration.LIFE_INSURANCE_INITIAL_COST_PERCENTAGE_DECREASE.Value + (level * UpgradeBus.Instance.PluginConfiguration.LIFE_INSURANCE_INCREMENTAL_COST_PERCENTAGE_DECREASE.Value);
            const string infoFormat = "LVL {0} - ${1} - Reduces the credit loss when leaving a body behind when exiting a moon by {2}%\n";
            return Tools.GenerateInfoForUpgrade(infoFormat, initialPrice, incrementalPrices, infoFunction);
        }

        public override bool CanInitializeOnStart
        {
            get
            {
                string[] prices = UpgradeBus.Instance.PluginConfiguration.LIFE_INSURANCE_PRICES.Value.Split(',');
                return UpgradeBus.Instance.PluginConfiguration.LIFE_INSURANCE_PRICE.Value <= 0 && prices.Length == 1 && (prices[0].Length == 0 || prices[0] == "0");
            }
        }

        public new static (string, string[]) RegisterScrapToUpgrade()
        {
            return (UPGRADE_NAME, UpgradeBus.Instance.PluginConfiguration.LIFE_INSURANCE_ITEM_PROGRESSION_ITEMS.Value.Split(","));
        }
        public new static void RegisterUpgrade()
        {
            GameObject prefab = LethalLib.Modules.NetworkPrefabs.CreateNetworkPrefab(UPGRADE_NAME);
            prefab.AddComponent<LifeInsurance>();
            LethalLib.Modules.NetworkPrefabs.RegisterNetworkPrefab(prefab);
            Plugin.networkPrefabs[UPGRADE_NAME] = prefab;
        }
        public new static CustomTerminalNode RegisterTerminalNode()
        {
            LategameConfiguration configuration = UpgradeBus.Instance.PluginConfiguration;

            return UpgradeBus.Instance.SetupMultiplePurchasableTerminalNode(UPGRADE_NAME,
                                                shareStatus: true,
                                                configuration.LIFE_INSURANCE_ENABLED,
                                                configuration.LIFE_INSURANCE_PRICE,
                                                UpgradeBus.ParseUpgradePrices(configuration.LIFE_INSURANCE_PRICES),
                                                configuration.OVERRIDE_UPGRADE_NAMES ? configuration.LIFE_INSURANCE_OVERRIDE_NAME : "",
                                                Plugin.networkPrefabs[UPGRADE_NAME]);
        }
    }
}
