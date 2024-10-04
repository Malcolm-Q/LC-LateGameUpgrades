using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc;
using MoreShipUpgrades.Misc.Upgrades;
using MoreShipUpgrades.Misc.Util;
using MoreShipUpgrades.UI.TerminalNodes;
using MoreShipUpgrades.UpgradeComponents.Interfaces;
using UnityEngine;

namespace MoreShipUpgrades.UpgradeComponents.TierUpgrades
{
    internal class LandingThrusters : TierUpgrade, IUpgradeWorldBuilding
    {
        internal const string UPGRADE_NAME = "Landing Thrusters";
        internal const string DEFAULT_PRICES = "250,450,650";
        internal const string WORLD_BUILDING_TEXT = "\n\nOptimization procedure for your Ship's in-atmosphere thrusters that makes quicker landings possible" +
            " by ordering the autopilot to commit to a longer freefall. Technically more dangerous, but it'll be fine.";

        public string GetWorldBuildingText(bool shareStatus = false)
        {
            return WORLD_BUILDING_TEXT;
        }
        void Awake()
        {
            upgradeName = UPGRADE_NAME;
            overridenUpgradeName = GetConfiguration().LANDING_THRUSTERS_OVERRIDE_NAME;
        }
        public static float GetInteractMutliplier()
        {
            LategameConfiguration config = GetConfiguration();
            if (!config.LANDING_THRUSTERS_ENABLED) return 1f;
            if (!config.LANDING_THRUSTERS_AFFECT_LANDING) return 1f;
            if (!GetActiveUpgrade(UPGRADE_NAME)) return 1f;
            return 1f + Mathf.Max(0f, (config.LANDING_THRUSTERS_INITIAL_SPEED_INCREASE + (GetUpgradeLevel(UPGRADE_NAME) * config.LANDING_THRUSTERS_INCREMENTAL_SPEED_INCREASE)) / 100f);
        }
        public static float GetLandingSpeedMultiplier()
        {
            LategameConfiguration config = GetConfiguration();
            if (!config.LANDING_THRUSTERS_ENABLED) return 1f;
            if (!GetActiveUpgrade(UPGRADE_NAME)) return 1f;
            return 1f + Mathf.Max(0f, (config.LANDING_THRUSTERS_INITIAL_SPEED_INCREASE + (GetUpgradeLevel(UPGRADE_NAME) * config.LANDING_THRUSTERS_INCREMENTAL_SPEED_INCREASE)) / 100f);
        }
        public override string GetDisplayInfo(int initialPrice = -1, int maxLevels = -1, int[] incrementalPrices = null)
        {
            static float infoFunction(int level)
            {
                LategameConfiguration config = GetConfiguration();
                return config.LANDING_THRUSTERS_INITIAL_SPEED_INCREASE.Value + (level * config.LANDING_THRUSTERS_INCREMENTAL_SPEED_INCREASE.Value);
            }
            const string infoFormat = "LVL {0} - ${1} - Increases the ship's landing speed by {2}%\n";
            return Tools.GenerateInfoForUpgrade(infoFormat, initialPrice, incrementalPrices, infoFunction);
        }

        public override bool CanInitializeOnStart
        {
            get
            {
                LategameConfiguration config = GetConfiguration();
                string[] prices = config.LANDING_THRUSTERS_PRICES.Value.Split(',');
                return config.LANDING_THRUSTERS_PRICE.Value <= 0 && prices.Length == 1 && (prices[0].Length == 0 || prices[0] == "0");
            }
        }

        public new static (string, string[]) RegisterScrapToUpgrade()
        {
            return (UPGRADE_NAME, GetConfiguration().LANDING_THRUSTERS_ITEM_PROGRESSION_ITEMS.Value.Split(","));
        }
        public new static void RegisterUpgrade()
        {
            SetupGenericPerk<LandingThrusters>(UPGRADE_NAME);
        }
        public new static CustomTerminalNode RegisterTerminalNode()
        {
            LategameConfiguration configuration = GetConfiguration();

            return UpgradeBus.Instance.SetupMultiplePurchasableTerminalNode(UPGRADE_NAME,
                                                shareStatus: true,
                                                configuration.LANDING_THRUSTERS_ENABLED,
                                                configuration.LANDING_THRUSTERS_PRICE,
                                                UpgradeBus.ParseUpgradePrices(configuration.LANDING_THRUSTERS_PRICES),
                                                configuration.OVERRIDE_UPGRADE_NAMES ? configuration.LANDING_THRUSTERS_OVERRIDE_NAME : "");
        }
    }
}
