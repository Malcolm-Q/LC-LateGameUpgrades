using MoreShipUpgrades.Configuration.Upgrades.Interfaces.TierUpgrades;
using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc.Upgrades;
using MoreShipUpgrades.Misc.Util;
using MoreShipUpgrades.UI.TerminalNodes;
using MoreShipUpgrades.UpgradeComponents.Interfaces;
using MoreShipUpgrades.UpgradeComponents.TierUpgrades.AttributeUpgrades;
using UnityEngine;

namespace MoreShipUpgrades.UpgradeComponents.TierUpgrades.Player
{
    internal class MedicalNanobots : TierUpgrade, IUpgradeWorldBuilding
    {
        internal const string UPGRADE_NAME = "Medical Nanobots";
        internal const string DEFAULT_PRICES = "300,400,500,750";
        internal const string WORLD_BUILDING_TEXT = "A weekly subscription service offered by the Company for disposable medical nanobots that are stored in a vial in your suit." +
            " When a serious injury is detected the vial is opened and they begin working to seal the wound…somewhat." +
            " They have been known to not be particularly effective and to break quickly but can save you in a pinch.";

        internal override void Start()
        {
            upgradeName = UPGRADE_NAME;
            overridenUpgradeName = GetConfiguration().MedicalNanobotsConfiguration.OverrideName;
            base.Start();
        }
        public override string GetDisplayInfo(int initialPrice = -1, int maxLevels = -1, int[] incrementalPrices = null)
        {
            static float infoFunction(int level)
            {
                ITierEffectUpgradeConfiguration<int> upgradeConfig = GetConfiguration().MedicalNanobotsConfiguration;
                return upgradeConfig.InitialEffect.Value + (level * upgradeConfig.IncrementalEffect.Value);
            }
            const string infoFormat = "LVL {0} - {1} - Increases the player's health regeneration cap by {2}%\n";
            return Tools.GenerateInfoForUpgrade(infoFormat, initialPrice, incrementalPrices, infoFunction, purchaseMode: GetConfiguration().MedicalNanobotsConfiguration.PurchaseMode);
        }
        public static int GetIncreasedHealthRegeneration(int defaultValue)
        {
            ITierEffectUpgradeConfiguration<int> upgradeConfig = GetConfiguration().MedicalNanobotsConfiguration;
            if (!upgradeConfig.Enabled) return defaultValue;
            if (!GetActiveUpgrade(UPGRADE_NAME)) return defaultValue;
            float percentage = (upgradeConfig.InitialEffect + (GetUpgradeLevel(UPGRADE_NAME) * upgradeConfig.IncrementalEffect))/100f;
            return Mathf.Clamp(defaultValue + (int)(defaultValue*percentage), defaultValue, Stimpack.CheckForAdditionalHealth(100));
        }

        public override bool CanInitializeOnStart
        {
            get
            {
                ITierUpgradeConfiguration upgradeConfig = GetConfiguration().MedicalNanobotsConfiguration;
                string[] prices = upgradeConfig.Prices.Value.Split(',');
                return prices.Length == 0 || (prices.Length == 1 && (prices[0].Length == 0 || prices[0] == "0"));
            }
        }
        public new static (string, string[]) RegisterScrapToUpgrade()
        {
            return (UPGRADE_NAME, GetConfiguration().MedicalNanobotsConfiguration.ItemProgressionItems.Value.Split(","));
        }
        public new static void RegisterUpgrade()
        {
            GameObject prefab = ItemManager.CreateNetworkPrefab(UPGRADE_NAME);
            prefab.AddComponent<MedicalNanobots>();
            ItemManager.RegisterNetworkPrefab(prefab);
            Plugin.networkPrefabs[UPGRADE_NAME] = prefab;
        }
        public new static CustomTerminalNode RegisterTerminalNode()
        {
            return UpgradeBus.Instance.SetupMultiplePurchaseableTerminalNode(UPGRADE_NAME, GetConfiguration().MedicalNanobotsConfiguration, Plugin.networkPrefabs[UPGRADE_NAME]);
        }

        public string GetWorldBuildingText(bool shareStatus = false)
        {
            return WORLD_BUILDING_TEXT;
        }
    }
}
