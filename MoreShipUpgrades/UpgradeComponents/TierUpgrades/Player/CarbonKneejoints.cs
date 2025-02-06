using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc.Upgrades;
using MoreShipUpgrades.Misc.Util;
using UnityEngine;
using MoreShipUpgrades.UI.TerminalNodes;
using MoreShipUpgrades.UpgradeComponents.Interfaces;
using MoreShipUpgrades.Configuration.Abstractions.TIerUpgrades;
using MoreShipUpgrades.Configuration.Interfaces.TierUpgrades;

namespace MoreShipUpgrades.UpgradeComponents.TierUpgrades.Player
{
    internal class CarbonKneejoints : TierUpgrade, IUpgradeWorldBuilding
    {
        internal const string UPGRADE_NAME = "Carbon Kneejoints";
        internal const string DEFAULT_PRICES = "100,50,100,150";
        internal const string WORLD_BUILDING_TEXT = "\n\nSpecial kneebrace & lower body harness system that supports your joints. Makes you feel prepared to sneak around. Also makes your butt look amazing.\n\n";

        
        void Awake()
        {
            upgradeName = UPGRADE_NAME;
            overridenUpgradeName = GetConfiguration().CarbonKneejointsConfiguration.OverrideName;
        }
        public static float CalculateDecreaseMultiplier()
        {
            ITierEffectUpgradeConfiguration<int> upgradeConfig = GetConfiguration().CarbonKneejointsConfiguration;
            if (!upgradeConfig.Enabled || !GetActiveUpgrade(UPGRADE_NAME)) return 0f;
            return (upgradeConfig.InitialEffect + (upgradeConfig.IncrementalEffect * GetUpgradeLevel(UPGRADE_NAME))) / 100f;
        }
        public static float ReduceCrouchMovementSpeedDebuff(float defaultValue)
        {
            float decreaseMultiplier = CalculateDecreaseMultiplier();
            float multipliedValue = 1f - defaultValue; // Being less than 1 means it makes the player faster while crouching
            return defaultValue - (Mathf.Clamp(1f - decreaseMultiplier, 0f, 1f) * multipliedValue);
        }
        public override string GetDisplayInfo(int initialPrice = -1, int maxLevels = -1, int[] incrementalPrices = null)
        {
            static float infoFunction(int level)
            {
                ITierEffectUpgradeConfiguration<int> upgradeConfig = GetConfiguration().CarbonKneejointsConfiguration;
                return upgradeConfig.InitialEffect.Value + (level * upgradeConfig.IncrementalEffect.Value);
            }
            const string infoFormat = "LVL {0} - {1} - Reduces the movement speed loss while crouching by {2}%\n";
            return Tools.GenerateInfoForUpgrade(infoFormat, initialPrice, incrementalPrices, infoFunction, purchaseMode: GetConfiguration().CarbonKneejointsConfiguration.PurchaseMode);
        }

        public override bool CanInitializeOnStart
        {
            get
            {
                ITierUpgradeConfiguration upgradeConfig = GetConfiguration().CarbonKneejointsConfiguration;
                string[] prices = upgradeConfig.Prices.Value.Split(',');
                return prices.Length == 0 || (prices.Length == 1 && (prices[0].Length == 0 || prices[0] == "0"));
            }
        }

        public new static (string, string[]) RegisterScrapToUpgrade()
        {
            return (UPGRADE_NAME, GetConfiguration().CarbonKneejointsConfiguration.ItemProgressionItems.Value.Split(","));
        }
        public new static void RegisterUpgrade()
        {
            GameObject prefab = LethalLib.Modules.NetworkPrefabs.CreateNetworkPrefab(UPGRADE_NAME);
            prefab.AddComponent<CarbonKneejoints>();
            LethalLib.Modules.NetworkPrefabs.RegisterNetworkPrefab(prefab);
            Plugin.networkPrefabs[UPGRADE_NAME] = prefab;
        }
        public new static CustomTerminalNode RegisterTerminalNode()
        {
            return UpgradeBus.Instance.SetupMultiplePurchaseableTerminalNode(UPGRADE_NAME, GetConfiguration().CarbonKneejointsConfiguration, Plugin.networkPrefabs[UPGRADE_NAME]);
        }

        public string GetWorldBuildingText(bool shareStatus = false)
        {
            return WORLD_BUILDING_TEXT;
        }
    }
}
