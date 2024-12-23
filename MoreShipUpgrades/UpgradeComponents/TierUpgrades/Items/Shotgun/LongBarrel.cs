using MoreShipUpgrades.Configuration.Abstractions.TIerUpgrades;
using MoreShipUpgrades.Configuration.Interfaces;
using MoreShipUpgrades.Managers;
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
        internal const string DEFAULT_PRICES = "500,500,750";
        internal const string WORLD_BUILDING_TEXT = "\n\nThe shotguns Nutcrackers are issued with break a lot of common gunsmithing conventions. For one thing, the barrels are overly short, and sometimes one tube is longer than the other. You got some help from a friend prying a few sections of spare pipe off the walls of the Factory. The parts appear capable of baring pressure, and the kind of adhesive you need is available in the Company Catalogue, but it's expensive...\n\n";
        public string GetWorldBuildingText(bool shareStatus = false)
        {
            return WORLD_BUILDING_TEXT;
        }
        internal override void Start()
        {
            upgradeName = UPGRADE_NAME;
            overridenUpgradeName = GetConfiguration().LongBarrelConfiguration.OverrideName;
            base.Start();
        }
        public override bool CanInitializeOnStart
        {
            get
            {
                ITierUpgradeConfiguration upgradeConfig = GetConfiguration().LongBarrelConfiguration;
                string[] prices = upgradeConfig.Prices.Value.Split(',');
                return prices.Length == 0 || (prices.Length == 1 && (prices[0].Length == 0 || prices[0] == "0"));
            }
        }
        public override string GetDisplayInfo(int initialPrice = -1, int maxLevels = -1, int[] incrementalPrices = null)
        {
            static float infoFunction(int level)
            {
                ITierEffectUpgrade<int> config = GetConfiguration().LongBarrelConfiguration;
                return config.InitialEffect.Value + (level * config.IncrementalEffect.Value);
            }
            const string infoFormat = "LVL {0} - ${1} - Shotgun's range and its effective damage ranges are increased by {2}%\n";
            return Tools.GenerateInfoForUpgrade(infoFormat, initialPrice, incrementalPrices, infoFunction);
        }
        public static float ComputeLongBarrelRangeBoost()
        {
            ITierEffectUpgrade<int> config = GetConfiguration().LongBarrelConfiguration;
            int percentage = config.InitialEffect + (GetUpgradeLevel(UPGRADE_NAME) * config.IncrementalEffect);
            return percentage / 100f;
        }
        public static float GetLongBarrelRangeBoost(float defaultValue)
        {
            if (!GetConfiguration().LongBarrelConfiguration.Enabled) return defaultValue;
            if (!GetActiveUpgrade(UPGRADE_NAME)) return defaultValue;
            float multiplier = ComputeLongBarrelRangeBoost();
            return defaultValue + Mathf.Clamp(defaultValue * multiplier, 0f, float.MaxValue);
        }
        public new static (string, string[]) RegisterScrapToUpgrade()
        {
            return (UPGRADE_NAME, GetConfiguration().LongBarrelConfiguration.ItemProgressionItems.Value.Split(","));
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
            return UpgradeBus.Instance.SetupMultiplePurchaseableTerminalNode(UPGRADE_NAME, GetConfiguration().LongBarrelConfiguration, Plugin.networkPrefabs[UPGRADE_NAME]);
        }
    }
}
