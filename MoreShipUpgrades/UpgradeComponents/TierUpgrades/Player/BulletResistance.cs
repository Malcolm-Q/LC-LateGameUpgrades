using MoreShipUpgrades.Configuration.Upgrades.Interfaces.TierUpgrades;
using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc.Upgrades;
using MoreShipUpgrades.Misc.Util;
using MoreShipUpgrades.UI.TerminalNodes;
using UnityEngine;

namespace MoreShipUpgrades.UpgradeComponents.TierUpgrades.Player
{
    internal class BulletResistance : TierUpgrade
    {
        internal const string UPGRADE_NAME = "Bullet Resistance";
        internal const string DEFAULT_PRICES = "200, 250, 350";

        internal override void Start()
        {
            upgradeName = UPGRADE_NAME;
            overridenUpgradeName = GetConfiguration().BulletResistanceConfiguration.OverrideName;
            base.Start();
        }
        public override string GetDisplayInfo(int initialPrice = -1, int maxLevels = -1, int[] incrementalPrices = null)
        {
            float infoFunction(int level)
            {
                ITierEffectUpgradeConfiguration<int> upgradeConfig = GetConfiguration().BulletResistanceConfiguration;
                return upgradeConfig.InitialEffect.Value + (level * upgradeConfig.IncrementalEffect.Value);
            }
            const string infoFormat = "LVL {0} - {1} - Increases the damage resistance to bullets by {2}\n";
            return Tools.GenerateInfoForUpgrade(infoFormat, initialPrice, incrementalPrices, infoFunction, purchaseMode: GetConfiguration().BulletResistanceConfiguration.PurchaseMode);
        }
        public static int GetBulletDamageResistance(int defaultValue)
        {
            ITierEffectUpgradeConfiguration<int> upgradeConfig = GetConfiguration().BulletResistanceConfiguration;
            if (!upgradeConfig.Enabled) return defaultValue;
            if (!GetActiveUpgrade(UPGRADE_NAME)) return defaultValue;
            int bulletDamageResistance = upgradeConfig.InitialEffect + (GetUpgradeLevel(UPGRADE_NAME) * upgradeConfig.IncrementalEffect);
            return defaultValue - (Mathf.RoundToInt(defaultValue * (bulletDamageResistance / 100f)));
        }

        public override bool CanInitializeOnStart
        {
            get
            {
                ITierUpgradeConfiguration upgradeConfig = GetConfiguration().BulletResistanceConfiguration;
                string[] prices = upgradeConfig.Prices.Value.Split(',');
                return prices.Length == 0 || (prices.Length == 1 && (prices[0].Length == 0 || prices[0] == "0"));
            }
        }
        public new static (string, string[]) RegisterScrapToUpgrade()
        {
            return (UPGRADE_NAME, GetConfiguration().BulletResistanceConfiguration.ItemProgressionItems.Value.Split(","));
        }
        public new static void RegisterUpgrade()
        {
            GameObject prefab = ItemManager.CreateNetworkPrefab(UPGRADE_NAME);
            prefab.AddComponent<BulletResistance>();
            ItemManager.RegisterNetworkPrefab(prefab);
            Plugin.networkPrefabs[UPGRADE_NAME] = prefab;
        }
        public new static CustomTerminalNode RegisterTerminalNode()
        {
            return UpgradeBus.Instance.SetupMultiplePurchaseableTerminalNode(UPGRADE_NAME, GetConfiguration().BulletResistanceConfiguration, Plugin.networkPrefabs[UPGRADE_NAME]);
        }
    }
}
