using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc;
using MoreShipUpgrades.Misc.Upgrades;
using MoreShipUpgrades.Misc.Util;
using MoreShipUpgrades.UI.TerminalNodes;
using UnityEngine;

namespace MoreShipUpgrades.UpgradeComponents.TierUpgrades.Items.Shotgun
{
    public class HollowPoint: TierUpgrade
    {
        internal const string UPGRADE_NAME = "Hollow Point";
        internal const string DEFAULT_PRICES = "800,1000";
        internal override void Start()
        {
            upgradeName = UPGRADE_NAME;
            overridenUpgradeName = GetConfiguration().HOLLOW_POINT_OVERRIDE_NAME;
            base.Start();
        }
        public override bool CanInitializeOnStart
        {
            get
            {
                LategameConfiguration config = GetConfiguration();
                string[] prices = config.HOLLOW_POINT_PRICES.Value.Split(',');
                return config.HOLLOW_POINT_PRICE.Value <= 0 && prices.Length == 1 && (prices[0].Length == 0 || prices[0] == "0");
            }
        }
        public override string GetDisplayInfo(int initialPrice = -1, int maxLevels = -1, int[] incrementalPrices = null)
        {
            static float infoFunction(int level)
            {
                LategameConfiguration config = GetConfiguration();
                return config.HOLLOW_POINT_INITIAL_SHOTGUN_DAMAGE_INCREASE.Value + (level * config.HOLLOW_POINT_INCREMENTAL_SHOTGUN_DAMAGE_INCREASE.Value);
            }
            const string infoFormat = "LVL {0} - ${1} - Damage dealt by the shotgun is increased by {2}\n";
            return Tools.GenerateInfoForUpgrade(infoFormat, initialPrice, incrementalPrices, infoFunction);
        }
        public static int ComputeHollowPointDamageBoost()
        {
            LategameConfiguration config = GetConfiguration();
            return config.HOLLOW_POINT_INITIAL_SHOTGUN_DAMAGE_INCREASE + (GetUpgradeLevel(UPGRADE_NAME) * config.HOLLOW_POINT_INCREMENTAL_SHOTGUN_DAMAGE_INCREASE);
        }
        public static int GetHollowPointDamageBoost(int defaultValue)
        {
            if (!GetConfiguration().HOLLOW_POINT_ENABLED) return defaultValue;
            if (!GetActiveUpgrade(UPGRADE_NAME)) return defaultValue;
            int additionalDamage = ComputeHollowPointDamageBoost();
            return defaultValue + additionalDamage;
        }
        public new static (string, string[]) RegisterScrapToUpgrade()
        {
            return (UPGRADE_NAME, GetConfiguration().HOLLOW_POINT_ITEM_PROGRESSION_ITEMS.Value.Split(","));
        }
        public new static void RegisterUpgrade()
        {
            GameObject prefab = LethalLib.Modules.NetworkPrefabs.CreateNetworkPrefab(UPGRADE_NAME);
            prefab.AddComponent<HollowPoint>();
            LethalLib.Modules.NetworkPrefabs.RegisterNetworkPrefab(prefab);
            Plugin.networkPrefabs[UPGRADE_NAME] = prefab;
        }
        public new static CustomTerminalNode RegisterTerminalNode()
        {
            LategameConfiguration configuration = GetConfiguration();

            return UpgradeBus.Instance.SetupMultiplePurchasableTerminalNode(UPGRADE_NAME,
                                                shareStatus: configuration.SHARED_UPGRADES.Value || !configuration.HOLLOW_POINT_INDIVIDUAL.Value,
                                                configuration.HOLLOW_POINT_ENABLED,
                                                configuration.HOLLOW_POINT_PRICE,
                                                UpgradeBus.ParseUpgradePrices(configuration.HOLLOW_POINT_PRICES),
                                                configuration.OVERRIDE_UPGRADE_NAMES ? configuration.HOLLOW_POINT_OVERRIDE_NAME : "",
                                                Plugin.networkPrefabs[UPGRADE_NAME]);
        }
    }
}
