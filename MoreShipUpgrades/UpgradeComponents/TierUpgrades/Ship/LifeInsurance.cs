using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc;
using MoreShipUpgrades.Misc.Upgrades;
using MoreShipUpgrades.Misc.Util;
using UnityEngine;
using MoreShipUpgrades.UI.TerminalNodes;
using MoreShipUpgrades.UpgradeComponents.Interfaces;

namespace MoreShipUpgrades.UpgradeComponents.TierUpgrades.Ship
{
    public class LifeInsurance : TierUpgrade, IUpgradeWorldBuilding
    {
        internal const string UPGRADE_NAME = "Life Insurance";
        internal const string DEFAULT_PRICES = "200,200,250,300";
        internal const string WORLD_BUILDING_TEXT = "\n\nHigh-grossing departments that operate in sectors of severe risk can optimize body retrieval costs by essentially" +
            " hiring the cleanup crew on a retainer. These arrangements are made Ship-to-Ship between individual Company Departments and require a lot of phonecalls and paperwork.\n\n";
        public string GetWorldBuildingText(bool shareStatus = false)
        {
            return WORLD_BUILDING_TEXT;
        }
        void Awake()
        {
            upgradeName = UPGRADE_NAME;
            overridenUpgradeName = GetConfiguration().LifeInsuranceConfiguration.OverrideName;
        }
        public static float CalculateDecreaseMultiplier()
        {
            TierPrimitiveUpgradeConfiguration<int> upgradeConfig = GetConfiguration().LifeInsuranceConfiguration;
            if (!upgradeConfig.Enabled || !GetActiveUpgrade(UPGRADE_NAME)) return 0f;
            return (upgradeConfig.InitialEffect + (upgradeConfig.IncrementalEffect * GetUpgradeLevel(UPGRADE_NAME))) / 100f;
        }
        public static float ReduceCreditCostPercentage(float defaultValue)
        {
            float decreaseMultiplier = CalculateDecreaseMultiplier();
            return Mathf.Clamp(1f - decreaseMultiplier, 0f, 1f) * defaultValue;
        }
        public override string GetDisplayInfo(int initialPrice = -1, int maxLevels = -1, int[] incrementalPrices = null)
        {
            static float infoFunction(int level)
            {
                TierPrimitiveUpgradeConfiguration<int> upgradeConfig = GetConfiguration().LifeInsuranceConfiguration;
                return upgradeConfig.InitialEffect.Value + (level * upgradeConfig.IncrementalEffect.Value);
            }
            const string infoFormat = "LVL {0} - ${1} - Reduces the credit loss when leaving a body behind when exiting a moon by {2}%\n";
            return Tools.GenerateInfoForUpgrade(infoFormat, initialPrice, incrementalPrices, infoFunction);
        }

        public override bool CanInitializeOnStart
        {
            get
            {
                TierPrimitiveUpgradeConfiguration<int> upgradeConfig = GetConfiguration().LifeInsuranceConfiguration;
                string[] prices = upgradeConfig.Prices.Value.Split(',');
                return prices.Length == 0 || (prices.Length == 1 && (prices[0].Length == 0 || prices[0] == "0"));
            }
        }

        public new static (string, string[]) RegisterScrapToUpgrade()
        {
            return (UPGRADE_NAME, GetConfiguration().LifeInsuranceConfiguration.ItemProgressionItems.Value.Split(","));
        }
        public new static void RegisterUpgrade()
        {
            GameObject prefab = LethalLib.Modules.NetworkPrefabs.CreateNetworkPrefab(UPGRADE_NAME);
            prefab.AddComponent<LifeInsurance>();
            LethalLib.Modules.NetworkPrefabs.RegisterNetworkPrefab(prefab);
            Plugin.networkPrefabs[UPGRADE_NAME] = prefab;
        }
        public new static CustomTerminalNode RegisterTerminalNode()
        {
            return UpgradeBus.Instance.SetupMultiplePurchaseableTerminalNode(UPGRADE_NAME, GetConfiguration().LifeInsuranceConfiguration, Plugin.networkPrefabs[UPGRADE_NAME]);
        }
    }
}
