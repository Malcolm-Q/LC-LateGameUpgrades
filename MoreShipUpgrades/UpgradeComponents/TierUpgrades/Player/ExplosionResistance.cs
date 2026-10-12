using MoreShipUpgrades.Configuration.Upgrades.Interfaces.TierUpgrades;
using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc.Upgrades;
using MoreShipUpgrades.Misc.Util;
using MoreShipUpgrades.UI.TerminalNodes;
using MoreShipUpgrades.UpgradeComponents.Interfaces;
using UnityEngine;

namespace MoreShipUpgrades.UpgradeComponents.TierUpgrades.Player
{
    internal class ExplosionResistance : TierUpgrade, IUpgradeWorldBuilding
    {
        internal const string UPGRADE_NAME = "Explosion Resistance";
        internal const string DEFAULT_PRICES = "150, 200, 250";
        internal const string WORLD_BUILDING_TEXT = "A set of proprietary spine reinforcement braces for your suit along with some specialized padded kevlar work boots." +
            " You may only have a slightly higher chance of surviving an explosion with these, but you will look awesome doing it!";

        internal override void Start()
        {
            upgradeName = UPGRADE_NAME;
            overridenUpgradeName = GetConfiguration().ExplosionReistanceConfiguration.OverrideName;
            base.Start();
        }
        public override string GetDisplayInfo(int initialPrice = -1, int maxLevels = -1, int[] incrementalPrices = null)
        {
            float infoFunction(int level)
            {
                ITierEffectUpgradeConfiguration<int> upgradeConfig = GetConfiguration().ExplosionReistanceConfiguration;
                return upgradeConfig.InitialEffect.Value + (level * upgradeConfig.IncrementalEffect.Value);
            }
            const string infoFormat = "LVL {0} - {1} - Increases the damage resistance to explosions by {2}%\n";
            return Tools.GenerateInfoForUpgrade(infoFormat, initialPrice, incrementalPrices, infoFunction, purchaseMode: GetConfiguration().ExplosionReistanceConfiguration.PurchaseMode);
        }
        public static int GetExplosionDamageResistance(int defaultValue)
        {
            ITierEffectUpgradeConfiguration<int> upgradeConfig = GetConfiguration().ExplosionReistanceConfiguration;
            if (!upgradeConfig.Enabled) return defaultValue;
            if (!GetActiveUpgrade(UPGRADE_NAME)) return defaultValue;
            int explosionDamageResistance = upgradeConfig.InitialEffect + (GetUpgradeLevel(UPGRADE_NAME) * upgradeConfig.IncrementalEffect);
            return defaultValue - (Mathf.RoundToInt(defaultValue * (explosionDamageResistance / 100f)));
        }

        public override bool CanInitializeOnStart
        {
            get
            {
                ITierUpgradeConfiguration upgradeConfig = GetConfiguration().ExplosionReistanceConfiguration;
                string[] prices = upgradeConfig.Prices.Value.Split(',');
                return prices.Length == 0 || (prices.Length == 1 && (prices[0].Length == 0 || prices[0] == "0"));
            }
        }
        public new static (string, string[]) RegisterScrapToUpgrade()
        {
            return (UPGRADE_NAME, GetConfiguration().ExplosionReistanceConfiguration.ItemProgressionItems.Value.Split(","));
        }
        public new static void RegisterUpgrade()
        {
            GameObject prefab = ItemManager.CreateNetworkPrefab(UPGRADE_NAME);
            prefab.AddComponent<ExplosionResistance>();
            ItemManager.RegisterNetworkPrefab(prefab);
            Plugin.networkPrefabs[UPGRADE_NAME] = prefab;
        }
        public new static CustomTerminalNode RegisterTerminalNode()
        {
            return UpgradeBus.Instance.SetupMultiplePurchaseableTerminalNode(UPGRADE_NAME, GetConfiguration().ExplosionReistanceConfiguration, Plugin.networkPrefabs[UPGRADE_NAME]);
        }

        public string GetWorldBuildingText(bool shareStatus = false)
        {
            return WORLD_BUILDING_TEXT;
        }
    }
}
