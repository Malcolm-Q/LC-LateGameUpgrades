using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc.Upgrades;
using MoreShipUpgrades.Misc.Util;
using UnityEngine;
using MoreShipUpgrades.UI.TerminalNodes;
using MoreShipUpgrades.UpgradeComponents.Interfaces;
using MoreShipUpgrades.Configuration;
using MoreShipUpgrades.Configuration.Upgrades.Interfaces.TierUpgrades;

namespace MoreShipUpgrades.UpgradeComponents.TierUpgrades.Player
{
    public class OxygenCanisters : TierUpgrade, IUpgradeWorldBuilding
    {
        internal const string UPGRADE_NAME = "Oxygen Canisters";
        internal const string DEFAULT_PRICES = "100,100,200,400";
        internal const string WORLD_BUILDING_TEXT = "\n\nPremium Condensed Air Tanks from OxyCo that come in a variety of flavors.\n\n";
        public string GetWorldBuildingText(bool shareStatus = false)
        {
            return WORLD_BUILDING_TEXT;
        }
        void Awake()
        {
            upgradeName = UPGRADE_NAME;
            overridenUpgradeName = GetConfiguration().OxygenCanistersConfiguration.OverrideName;
        }
        public static float CalculateDecreaseMultiplier()
        {
            ITierEffectUpgradeConfiguration<int> config = GetConfiguration().OxygenCanistersConfiguration;
            if (!config.Enabled || !GetActiveUpgrade(UPGRADE_NAME)) return 0f;
            return (config.InitialEffect + (config.IncrementalEffect * GetUpgradeLevel(UPGRADE_NAME)))/100f;
        }
        public static float ReduceOxygenConsumption(float defaultValue)
        {
            float decreaseMultiplier = CalculateDecreaseMultiplier();
            return Mathf.Clamp(1f -  decreaseMultiplier, 0f, 1f) * defaultValue;
        }
        public override string GetDisplayInfo(int initialPrice = -1, int maxLevels = -1, int[] incrementalPrices = null)
        {
            static float infoFunction(int level)
            {
                ITierEffectUpgradeConfiguration<int> config = GetConfiguration().OxygenCanistersConfiguration;
                return config.InitialEffect.Value + (level * config.IncrementalEffect.Value);
            }
            const string infoFormat = "LVL {0} - {1} - Reduces oxygen consumption rate by {2}%\n";
            return Tools.GenerateInfoForUpgrade(infoFormat, initialPrice, incrementalPrices, infoFunction, purchaseMode: GetConfiguration().OxygenCanistersConfiguration.PurchaseMode);
        }

        public override bool CanInitializeOnStart
        {
            get
            {
                ITierEffectUpgradeConfiguration<int> config = GetConfiguration().OxygenCanistersConfiguration;
                string[] prices = config.Prices.Value.Split(',');
                return prices.Length == 0 || (prices.Length == 1 && (prices[0].Length == 0 || prices[0] == "0"));
            }
        }

        public new static (string, string[]) RegisterScrapToUpgrade()
        {
            return (UPGRADE_NAME, GetConfiguration().OxygenCanistersConfiguration.ItemProgressionItems.Value.Split(","));
        }
        public new static void RegisterUpgrade()
        {
            GameObject prefab = ItemManager.CreateNetworkPrefab(UPGRADE_NAME);
            prefab.AddComponent<OxygenCanisters>();
            ItemManager.RegisterNetworkPrefab(prefab);
            Plugin.networkPrefabs[UPGRADE_NAME] = prefab;
        }
        public new static CustomTerminalNode RegisterTerminalNode()
        { 
            return UpgradeBus.Instance.SetupMultiplePurchaseableTerminalNode(UPGRADE_NAME, GetConfiguration().OxygenCanistersConfiguration, Plugin.networkPrefabs[UPGRADE_NAME]);
        }
    }
}
