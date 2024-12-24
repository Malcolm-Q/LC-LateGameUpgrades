using MoreShipUpgrades.Configuration.Interfaces;
using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc.Upgrades;
using MoreShipUpgrades.Misc.Util;
using MoreShipUpgrades.UI.TerminalNodes;
using MoreShipUpgrades.UpgradeComponents.Interfaces;
using UnityEngine;

namespace MoreShipUpgrades.UpgradeComponents.TierUpgrades.Items
{
    public class ScrapKeeper : TierUpgrade, IUpgradeWorldBuilding
    {
        internal const string UPGRADE_NAME = "Scrap Keeper";
        internal const string PRICES_DEFAULT = "1000,1000,1500,3000";
        internal const string WORLD_BUILDING_TEXT = "\n\nIn the race to the bottom of all possible operating costs, The Company has made many compromises. Some of these compromises end up being pretty questionable, and it's up to individual departments to handle these problems on their own. There is a design flaw in the standard-issue Company Ship that causes it to empty its contents when it's forced to leave a moon and there is no-one onboard to ensure the doors close at the right time. You'd think this would be a simple fix, but for so many reasons it'd be a waste of both our time to describe, no. No, it very much isn't.\n\n";
        public string GetWorldBuildingText(bool shareStatus = false)
        {
            return WORLD_BUILDING_TEXT;
        }
        internal override void Start()
        {
            upgradeName = UPGRADE_NAME;
            overridenUpgradeName = GetConfiguration().ScrapKeeperConfiguration.OverrideName;
            base.Start();
        }
        public override bool CanInitializeOnStart
        {
            get
            {
                ITierUpgradeConfiguration upgradeConfig = GetConfiguration().ScrapKeeperConfiguration;
                string[] prices = upgradeConfig.Prices.Value.Split(',');
                return prices.Length == 0 || (prices.Length == 1 && (prices[0].Length == 0 || prices[0] == "0"));
            }
        }
        public override string GetDisplayInfo(int initialPrice = -1, int maxLevels = -1, int[] incrementalPrices = null)
        {
            static float infoFunction(int level)
            {
                ITierEffectUpgradeConfiguration<int> upgradeConfig = GetConfiguration().ScrapKeeperConfiguration;
                return upgradeConfig.InitialEffect.Value + (level * upgradeConfig.IncrementalEffect.Value);
            }
            const string infoFormat = "LVL {0} - ${1} - In case of a full team wipe, each scrap present in the ship has a {2}% chance of not being discarded.\n";
            return Tools.GenerateInfoForUpgrade(infoFormat, initialPrice, incrementalPrices, infoFunction);
        }
        public static float ComputeScrapKeeperKeepScrapChance()
        {
            ITierEffectUpgradeConfiguration<int> upgradeConfig = GetConfiguration().ScrapKeeperConfiguration;
            int percentage = upgradeConfig.InitialEffect + (GetUpgradeLevel(UPGRADE_NAME) * upgradeConfig.IncrementalEffect);
            return percentage / 100f;
        }
        public static bool CanKeepScrapBasedOnChance()
        {
            IUpgradeConfiguration upgradeConfig = GetConfiguration().EffectiveBandaidsConfiguration;
            if (!upgradeConfig.Enabled) return false;
            if (!GetActiveUpgrade(UPGRADE_NAME)) return false;
            float scrapChance = Mathf.Clamp(ComputeScrapKeeperKeepScrapChance(), 0f, 1f);
            return Random.Range(0f, 1f) <= scrapChance;
        }

        public static bool CheckIfKeptScrap()
        {
            if (!GetConfiguration().ScrapKeeperConfiguration.Enabled) return false;
            if (!GetActiveUpgrade(UPGRADE_NAME)) return false;
            return true;
        }
        public new static (string, string[]) RegisterScrapToUpgrade()
        {
            return (UPGRADE_NAME, GetConfiguration().ScrapKeeperConfiguration.ItemProgressionItems.Value.Split(","));
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
            return UpgradeBus.Instance.SetupMultiplePurchaseableTerminalNode(UPGRADE_NAME, GetConfiguration().ScrapKeeperConfiguration, Plugin.networkPrefabs[UPGRADE_NAME]);
        }
    }
}
