using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc;
using MoreShipUpgrades.Misc.Upgrades;
using MoreShipUpgrades.Misc.Util;
using MoreShipUpgrades.UI.TerminalNodes;
using MoreShipUpgrades.UpgradeComponents.Interfaces;
using UnityEngine;

namespace MoreShipUpgrades.UpgradeComponents.TierUpgrades.Items.Shotgun
{
    internal class LongBarrel : TierUpgrade, IUpgradeWorldBuilding
    {
        internal const string UPGRADE_NAME = "Long Barrel";
        internal const string DEFAULT_PRICES = "500,750";
        internal const string WORLD_BUILDING_TEXT = "\n\nThe shotguns Nutcrackers are issued with break a lot of common gunsmithing conventions. For one thing, the barrels are overly short, and sometimes one tube is longer than the other. You got some help from a friend prying a few sections of spare pipe off the walls of the Factory. The parts appear capable of baring pressure, and the kind of adhesive you need is available in the Company Catalogue, but it's expensive...\n\n";
        public string GetWorldBuildingText(bool shareStatus = false)
        {
            return WORLD_BUILDING_TEXT;
        }
        internal override void Start()
        {
            upgradeName = UPGRADE_NAME;
            overridenUpgradeName = GetConfiguration().LONG_BARREL_OVERRIDE_NAME;
            base.Start();
        }
        public override bool CanInitializeOnStart
        {
            get
            {
                LategameConfiguration config = GetConfiguration();
                string[] prices = config.LONG_BARREL_PRICES.Value.Split(',');
                return config.LONG_BARREL_PRICE.Value <= 0 && prices.Length == 1 && (prices[0].Length == 0 || prices[0] == "0");
            }
        }
        public override string GetDisplayInfo(int initialPrice = -1, int maxLevels = -1, int[] incrementalPrices = null)
        {
            static float infoFunction(int level)
            {
                LategameConfiguration config = GetConfiguration();
                return config.LONG_BARREL_INITIAL_SHOTGUN_RANGE_INCREASE.Value + (level * config.LONG_BARREL_INCREMENTAL_SHOTGUN_RANGE_INCREASE.Value);
            }
            const string infoFormat = "LVL {0} - ${1} - Shotgun's range and its effective damage ranges are increased by {2}%\n";
            return Tools.GenerateInfoForUpgrade(infoFormat, initialPrice, incrementalPrices, infoFunction);
        }
        public static float ComputeLongBarrelRangeBoost()
        {
            LategameConfiguration config = GetConfiguration();
            int percentage = config.LONG_BARREL_INITIAL_SHOTGUN_RANGE_INCREASE + (GetUpgradeLevel(UPGRADE_NAME) * config.LONG_BARREL_INCREMENTAL_SHOTGUN_RANGE_INCREASE);
            return percentage / 100f;
        }
        public static float GetLongBarrelRangeBoost(float defaultValue)
        {
            if (!GetConfiguration().LONG_BARREL_ENABLED) return defaultValue;
            if (!GetActiveUpgrade(UPGRADE_NAME)) return defaultValue;
            float multiplier = ComputeLongBarrelRangeBoost();
            return defaultValue + Mathf.Clamp(defaultValue * multiplier, 0f, float.MaxValue);
        }
        public new static (string, string[]) RegisterScrapToUpgrade()
        {
            return (UPGRADE_NAME, GetConfiguration().LONG_BARREL_ITEM_PROGRESSION_ITEMS.Value.Split(","));
        }
        public new static void RegisterUpgrade()
        {
            GameObject prefab = LethalLib.Modules.NetworkPrefabs.CreateNetworkPrefab(UPGRADE_NAME);
            prefab.AddComponent<LongBarrel>();
            LethalLib.Modules.NetworkPrefabs.RegisterNetworkPrefab(prefab);
            Plugin.networkPrefabs[UPGRADE_NAME] = prefab;
        }
        public new static CustomTerminalNode RegisterTerminalNode()
        {
            LategameConfiguration configuration = GetConfiguration();

            return UpgradeBus.Instance.SetupMultiplePurchasableTerminalNode(UPGRADE_NAME,
                                                shareStatus: configuration.SHARED_UPGRADES.Value || !configuration.LONG_BARREL_INDIVIDUAL.Value,
                                                configuration.LONG_BARREL_ENABLED,
                                                configuration.LONG_BARREL_PRICE,
                                                UpgradeBus.ParseUpgradePrices(configuration.LONG_BARREL_PRICES),
                                                configuration.OVERRIDE_UPGRADE_NAMES ? configuration.LONG_BARREL_OVERRIDE_NAME : "",
                                                Plugin.networkPrefabs[UPGRADE_NAME]);
        }
    }
}
