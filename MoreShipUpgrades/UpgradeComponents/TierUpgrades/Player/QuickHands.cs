using MoreShipUpgrades.Configuration.Upgrades.Interfaces.TierUpgrades;
using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc.Upgrades;
using MoreShipUpgrades.Misc.Util;
using MoreShipUpgrades.UI.TerminalNodes;
using MoreShipUpgrades.UpgradeComponents.Interfaces;
using UnityEngine;

namespace MoreShipUpgrades.UpgradeComponents.TierUpgrades.Player
{
    internal class QuickHands : TierUpgrade, IUpgradeWorldBuilding
    {
        internal const string UPGRADE_NAME = "Quick Hands";
        internal const string DEFAULT_PRICES = "100,100,150,200,250";
        internal const string WORLD_BUILDING_TEXT = "\n\nThe 'Intern Tremble' is a known reoccuring phenomena among contractors in this field. Employees get nervous while exploring the Ruins" +
            " and it manifests as a constant shaking in the wrists. In the back of a magazine you found an advert for 'Stress Management Lozenges' and decided to take it up. Your wrists don't shake anymore.\n\n";

        public string GetWorldBuildingText(bool shareStatus = false)
        {
            return WORLD_BUILDING_TEXT;
        }
        void Awake()
        {
            upgradeName = UPGRADE_NAME;
            overridenUpgradeName = GetConfiguration().QuickHandsConfiguration.OverrideName;
        }
        public static float GetIncreasedInteractionSpeedMultiplier()
        {
            ITierEffectUpgradeConfiguration<int> upgradeConfig = GetConfiguration().QuickHandsConfiguration;
            return (upgradeConfig.InitialEffect + (GetUpgradeLevel(UPGRADE_NAME) * upgradeConfig.IncrementalEffect)) / 100f;
        }
        public static float IncreaseInteractionSpeed(float defaultValue)
        {
            if (!GetConfiguration().QuickHandsConfiguration.Enabled) return defaultValue;
            if (!GetActiveUpgrade(UPGRADE_NAME)) return defaultValue;
            float multiplier = GetIncreasedInteractionSpeedMultiplier();
            return (int)Mathf.Clamp(defaultValue + (defaultValue * multiplier), defaultValue, float.MaxValue);
        }
        public override string GetDisplayInfo(int initialPrice = -1, int maxLevels = -1, int[] incrementalPrices = null)
        {
            static float infoFunction(int level)
            {
                ITierEffectUpgradeConfiguration<int> upgradeConfig = GetConfiguration().QuickHandsConfiguration;
                return upgradeConfig.InitialEffect.Value + (level * upgradeConfig.IncrementalEffect.Value);
            }
            const string infoFormat = "LVL {0} - {1} - Increases interaction speed of the player by {2}%\n";
            return Tools.GenerateInfoForUpgrade(infoFormat, initialPrice, incrementalPrices, infoFunction, purchaseMode: GetConfiguration().QuickHandsConfiguration.PurchaseMode);
        }

        public override bool CanInitializeOnStart
        {
            get
            {
                ITierEffectUpgradeConfiguration<int> upgradeConfig = GetConfiguration().QuickHandsConfiguration;
                string[] prices = upgradeConfig.Prices.Value.Split(',');
                return prices.Length == 0 || (prices.Length == 1 && (prices[0].Length == 0 || prices[0] == "0"));
            }
        }

        public new static (string, string[]) RegisterScrapToUpgrade()
        {
            return (UPGRADE_NAME, GetConfiguration().QuickHandsConfiguration.ItemProgressionItems.Value.Split(","));
        }
        public new static void RegisterUpgrade()
        {
            GameObject prefab = ItemManager.CreateNetworkPrefab(UPGRADE_NAME);
            prefab.AddComponent<QuickHands>();
            ItemManager.RegisterNetworkPrefab(prefab);
            Plugin.networkPrefabs[UPGRADE_NAME] = prefab;
        }
        public new static CustomTerminalNode RegisterTerminalNode()
        {
            return UpgradeBus.Instance.SetupMultiplePurchaseableTerminalNode(UPGRADE_NAME, GetConfiguration().QuickHandsConfiguration, Plugin.networkPrefabs[UPGRADE_NAME]);
        }
    }
}
