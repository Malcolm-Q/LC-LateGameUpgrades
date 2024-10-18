using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc;
using MoreShipUpgrades.Misc.Upgrades;
using MoreShipUpgrades.Misc.Util;
using MoreShipUpgrades.UI.TerminalNodes;
using Unity.Netcode;
using UnityEngine;

namespace MoreShipUpgrades.UpgradeComponents.TierUpgrades.Items
{
    public class ScrapKeeper : TierUpgrade
    {
        internal const string UPGRADE_NAME = "Scrap Keeper";
        internal const string PRICES_DEFAULT = "1000,1500,3000";
        internal override void Start()
        {
            upgradeName = UPGRADE_NAME;
            overridenUpgradeName = GetConfiguration().SCRAP_KEEPER_OVERRIDE_NAME;
            base.Start();
        }
        public override bool CanInitializeOnStart
        {
            get
            {
                LategameConfiguration config = GetConfiguration();
                string[] prices = config.SCRAP_KEEPER_PRICES.Value.Split(',');
                return config.SCRAP_KEEPER_PRICE.Value <= 0 && prices.Length == 1 && (prices[0].Length == 0 || prices[0] == "0");
            }
        }
        public override string GetDisplayInfo(int initialPrice = -1, int maxLevels = -1, int[] incrementalPrices = null)
        {
            static float infoFunction(int level)
            {
                LategameConfiguration config = GetConfiguration();
                return config.SCRAP_KEEPER_INITIAL_KEEP_SCRAP_CHANCE_INCREASE.Value + (level * config.SCRAP_KEEPER_INCREMENTAL_KEEP_SCRAP_CHANCE_INCREASE.Value);
            }
            const string infoFormat = "LVL {0} - ${1} - In case of a full team wipe, each scrap present in the ship has a {2}% chance of not being discarded.\n";
            return Tools.GenerateInfoForUpgrade(infoFormat, initialPrice, incrementalPrices, infoFunction);
        }
        public static float ComputeScrapKeeperKeepScrapChance()
        {
            LategameConfiguration config = GetConfiguration();
            int percentage = config.SCRAP_KEEPER_INITIAL_KEEP_SCRAP_CHANCE_INCREASE + (GetUpgradeLevel(UPGRADE_NAME) * config.SCRAP_KEEPER_INCREMENTAL_KEEP_SCRAP_CHANCE_INCREASE);
            return percentage / 100f;
        }
        public static bool CanKeepScrapBasedOnChance()
        {
            if (!GetConfiguration().SCRAP_KEEPER_ENABLED) return false;
            if (!GetActiveUpgrade(UPGRADE_NAME)) return false;
            float scrapChance = Mathf.Clamp(ComputeScrapKeeperKeepScrapChance(), 0f, 1f);
            return Random.Range(0f, 1f) <= scrapChance;
        }

        public static bool CheckIfKeptScrap()
        {
            if (!GetConfiguration().SCRAP_KEEPER_ENABLED) return false;
            if (!GetActiveUpgrade(UPGRADE_NAME)) return false;
            return true;
        }
        public new static (string, string[]) RegisterScrapToUpgrade()
        {
            return (UPGRADE_NAME, GetConfiguration().SCRAP_KEEPER_ITEM_PROGRESSION_ITEMS.Value.Split(","));
        }
        public new static void RegisterUpgrade()
        {
            GameObject prefab = LethalLib.Modules.NetworkPrefabs.CreateNetworkPrefab(UPGRADE_NAME);
            prefab.AddComponent<ScrapKeeper>();
            LethalLib.Modules.NetworkPrefabs.RegisterNetworkPrefab(prefab);
            Plugin.networkPrefabs[UPGRADE_NAME] = prefab;
        }
        public new static CustomTerminalNode RegisterTerminalNode()
        {
            LategameConfiguration configuration = GetConfiguration();

            return UpgradeBus.Instance.SetupMultiplePurchasableTerminalNode(UPGRADE_NAME,
                                                shareStatus: true,
                                                configuration.SCRAP_KEEPER_ENABLED.Value,
                                                configuration.SCRAP_KEEPER_PRICE.Value,
                                                UpgradeBus.ParseUpgradePrices(configuration.SCRAP_KEEPER_PRICES.Value),
                                                configuration.OVERRIDE_UPGRADE_NAMES ? configuration.SCRAP_KEEPER_OVERRIDE_NAME : "",
                                                Plugin.networkPrefabs[UPGRADE_NAME]);
        }
    }
}
