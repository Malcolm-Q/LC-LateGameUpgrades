﻿using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc;
using MoreShipUpgrades.Misc.Upgrades;
using MoreShipUpgrades.Misc.Util;
using MoreShipUpgrades.UI.TerminalNodes;
using MoreShipUpgrades.UpgradeComponents.Interfaces;
using UnityEngine;

namespace MoreShipUpgrades.UpgradeComponents.TierUpgrades.Player
{
    internal class RunningShoes : TierUpgrade, IUpgradeWorldBuilding
    {
        public const string UPGRADE_NAME = "Running Shoes";
        public const string PRICES_DEFAULT = "500,750,1000";
        internal const string WORLD_BUILDING_TEXT = "\n\nA new pair of boots {0} a whole new lease on life. In this instance," +
            " it might also result in fewer wet sock incidents and consequent trenchfoot. After all, who knows how many people have walked in {1} shoes?\n\n";
        void Awake()
        {
            upgradeName = UPGRADE_NAME;
            overridenUpgradeName = GetConfiguration().RUNNING_SHOES_OVERRIDE_NAME;
        }
        public static float ApplyPossibleReducedNoiseRange(float defaultValue)
        {
            LategameConfiguration config = GetConfiguration();
            if (!(config.RUNNING_SHOES_ENABLED && GetActiveUpgrade(UPGRADE_NAME) && GetUpgradeLevel(UPGRADE_NAME) == config.RUNNING_SHOES_UPGRADE_PRICES.Value.Split(',').Length)) return defaultValue;
            return Mathf.Clamp(defaultValue - config.NOISE_REDUCTION.Value, 0f, defaultValue);
        }

        public string GetWorldBuildingText(bool shareStatus = false)
        {
            return string.Format(WORLD_BUILDING_TEXT, shareStatus ? "could give your crew" : "can give you", shareStatus ? "y'all's" : "your");
        }

        public override string GetDisplayInfo(int initialPrice = -1, int maxLevels = -1, int[] incrementalPrices = null)
        {
            static float infoFunction(int level)
            {
                LategameConfiguration config = GetConfiguration();
                return config.MOVEMENT_SPEED_UNLOCK.Value + (level * config.MOVEMENT_INCREMENT.Value);
            }
            string infoFormat = AssetBundleHandler.GetInfoFromJSON(UPGRADE_NAME);
            return Tools.GenerateInfoForUpgrade(infoFormat, initialPrice, incrementalPrices, infoFunction);
        }

        public static float GetAdditionalMovementSpeed(float defaultValue)
        {
            LategameConfiguration config = GetConfiguration();
            if (!config.RUNNING_SHOES_ENABLED) return defaultValue;
            if (!GetActiveUpgrade(UPGRADE_NAME)) return defaultValue;
            float additionalValue = config.MOVEMENT_SPEED_UNLOCK + (GetUpgradeLevel(UPGRADE_NAME) * config.MOVEMENT_INCREMENT);
            return Mathf.Clamp(defaultValue + additionalValue, defaultValue, float.MaxValue);
        }

        public override bool CanInitializeOnStart
        {
            get
            {
                LategameConfiguration config = GetConfiguration();
                string[] prices = config.RUNNING_SHOES_UPGRADE_PRICES.Value.Split(',');
                return config.RUNNING_SHOES_PRICE.Value <= 0 && prices.Length == 1 && (prices[0].Length == 0 || prices[0] == "0");
            }
        }

        public new static (string, string[]) RegisterScrapToUpgrade()
        {
            return (UPGRADE_NAME, GetConfiguration().RUNNING_SHOES_ITEM_PROGRESSION_ITEMS.Value.Split(","));
        }
        public new static void RegisterUpgrade()
        {
            SetupGenericPerk<RunningShoes>(UPGRADE_NAME);
        }
        public new static CustomTerminalNode RegisterTerminalNode()
        {
            LategameConfiguration configuration = GetConfiguration();

            return UpgradeBus.Instance.SetupMultiplePurchasableTerminalNode(UPGRADE_NAME,
                                                configuration.SHARED_UPGRADES.Value || !configuration.RUNNING_SHOES_INDIVIDUAL.Value,
                                                configuration.RUNNING_SHOES_ENABLED.Value,
                                                configuration.RUNNING_SHOES_PRICE.Value,
                                                UpgradeBus.ParseUpgradePrices(configuration.RUNNING_SHOES_UPGRADE_PRICES.Value),
                                                configuration.OVERRIDE_UPGRADE_NAMES ? configuration.RUNNING_SHOES_OVERRIDE_NAME : "");
        }
    }
}
