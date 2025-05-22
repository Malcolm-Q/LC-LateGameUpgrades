using MoreShipUpgrades.Configuration.Upgrades.Interfaces.TierUpgrades;
using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc.Upgrades;
using MoreShipUpgrades.Misc.Util;
using MoreShipUpgrades.UI.TerminalNodes;
using MoreShipUpgrades.UpgradeComponents.Interfaces;
using UnityEngine;

namespace MoreShipUpgrades.UpgradeComponents.TierUpgrades.Ship
{
    internal class MidasTouch : TierUpgrade, IUpgradeWorldBuilding
    {
        internal const string UPGRADE_NAME = "Midas Touch";
        internal const string DEFAULT_PRICES = "1000,1000,1500,1800,2000";
        internal const string WORLD_BUILDING_TEXT = "\n\nYour commitment to Company Standards & Values 3E-60-92-43 and adherence" +
            " to Work Code & Ethics 429-A-71-36 has appeased The Company a little bit. The memories attached to objects handled by your department are among The Company's favorites to review.\n\n";

        public string GetWorldBuildingText(bool shareStatus = false)
        {
            return WORLD_BUILDING_TEXT;
        }
        void Awake()
        {
            upgradeName = UPGRADE_NAME;
            overridenUpgradeName = GetConfiguration().MidasTouchConfiguration.OverrideName;
        }
        public override bool CanInitializeOnStart
        {
            get
            {
                ITierEffectUpgradeConfiguration<int> upgradeConfig = GetConfiguration().MidasTouchConfiguration;
                string[] prices = upgradeConfig.Prices.Value.Split(',');
                return prices.Length == 0 || (prices.Length == 1 && (prices[0].Length == 0 || prices[0] == "0"));
            }
        }
        static float GetIncreasedScrapValueMultiplier()
        {
            ITierEffectUpgradeConfiguration<int> upgradeConfig = GetConfiguration().MidasTouchConfiguration;
            return (upgradeConfig.InitialEffect + (GetUpgradeLevel(UPGRADE_NAME) * upgradeConfig.IncrementalEffect)) / 100f;
        }
        public static float IncreaseScrapValue(float defaultValue)
        {
            ITierEffectUpgradeConfiguration<int> upgradeConfig = GetConfiguration().MidasTouchConfiguration;
            if (!upgradeConfig.Enabled) return defaultValue;
            if (!GetActiveUpgrade(UPGRADE_NAME)) return defaultValue;
            float additionalScrapValueMultiplier = GetIncreasedScrapValueMultiplier();
            return Mathf.Clamp(defaultValue + (defaultValue * additionalScrapValueMultiplier), defaultValue, float.MaxValue);
        }
        public static int IncreaseScrapValueInteger(int defaultValue)
        {
            ITierEffectUpgradeConfiguration<int> upgradeConfig = GetConfiguration().MidasTouchConfiguration;
            if (!upgradeConfig.Enabled) return defaultValue;
            if (!GetActiveUpgrade(UPGRADE_NAME)) return defaultValue;
            float additionalScrapValueMultiplier = 1f + GetIncreasedScrapValueMultiplier();
            return Mathf.Clamp(Mathf.CeilToInt(defaultValue * additionalScrapValueMultiplier), defaultValue, int.MaxValue);
        }
        public override string GetDisplayInfo(int initialPrice = -1, int maxLevels = -1, int[] incrementalPrices = null)
        {
            static float infoFunction(int level)
            {
                ITierEffectUpgradeConfiguration<int> upgradeConfig = GetConfiguration().MidasTouchConfiguration;
                return upgradeConfig.InitialEffect.Value + (level * upgradeConfig.IncrementalEffect.Value);
            }
            const string infoFormat = "LVL {0} - {1} - Increases the value of the scrap found in the moons by {2}%.\n";
            return Tools.GenerateInfoForUpgrade(infoFormat, initialPrice, incrementalPrices, infoFunction, purchaseMode: GetConfiguration().MidasTouchConfiguration.PurchaseMode);
        }
        public new static (string, string[]) RegisterScrapToUpgrade()
        {
            return (UPGRADE_NAME, GetConfiguration().MidasTouchConfiguration.ItemProgressionItems.Value.Split(","));
        }
        public new static void RegisterUpgrade()
        {
            GameObject prefab = LethalLib.Modules.NetworkPrefabs.CreateNetworkPrefab(UPGRADE_NAME);
            prefab.AddComponent<MidasTouch>();
            LethalLib.Modules.NetworkPrefabs.RegisterNetworkPrefab(prefab);
            Plugin.networkPrefabs[UPGRADE_NAME] = prefab;
        }
        public new static CustomTerminalNode RegisterTerminalNode()
        {
            return UpgradeBus.Instance.SetupMultiplePurchaseableTerminalNode(UPGRADE_NAME, GetConfiguration().MidasTouchConfiguration, Plugin.networkPrefabs[UPGRADE_NAME]);
        }
    }
}
