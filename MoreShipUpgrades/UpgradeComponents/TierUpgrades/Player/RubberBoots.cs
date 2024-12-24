using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc.Upgrades;
using MoreShipUpgrades.Misc.Util;
using UnityEngine;
using MoreShipUpgrades.UI.TerminalNodes;
using MoreShipUpgrades.UpgradeComponents.Interfaces;
using MoreShipUpgrades.Configuration.Abstractions.TIerUpgrades;
using MoreShipUpgrades.Configuration.Interfaces;

namespace MoreShipUpgrades.UpgradeComponents.TierUpgrades.Player
{
    public class RubberBoots : TierUpgrade, IUpgradeWorldBuilding
    {
        internal const string UPGRADE_NAME = "Rubber Boots";
        internal const string DEFAULT_PRICES = "50,50,100,200";
        internal const string WORLD_BUILDING_TEXT = "\n\nIn the 'GEAR' section of the Company Catalogue, the twelfth ad on the fourth page is for this product." +
            " It's a knee-high boot of rubbery synthetic material that fits over your standard footwear. The boots are flexible enough to be folded for compact storage in a pouch or hip bag.\n\n";

        public string GetWorldBuildingText(bool shareStatus = false)
        {
            return WORLD_BUILDING_TEXT;
        }
        void Awake()
        {
            upgradeName = UPGRADE_NAME;
            overridenUpgradeName = GetConfiguration().RubberBootsConfiguration.OverrideName;
        }
        public static float CalculateDecreaseMultiplier()
        {
            ITierEffectUpgradeConfiguration<int> config = GetConfiguration().RubberBootsConfiguration;
            if (!config.Enabled || !GetActiveUpgrade(UPGRADE_NAME)) return 0f;
            return (config.InitialEffect + (config.IncrementalEffect * GetUpgradeLevel(UPGRADE_NAME))) / 100f;
        }
        public static int ClearMovementHinderance(int defaultValue)
        {
            if (CalculateDecreaseMultiplier() >= 1f)
                return 0;
            else return defaultValue;
        }
        public static float ReduceMovementHinderance(float defaultValue)
        {
            float decreaseMultiplier = CalculateDecreaseMultiplier();
            return Mathf.Clamp(Mathf.Clamp(1f - decreaseMultiplier, 0f, 1f) * defaultValue, 1f, 100f);
        }
        public override string GetDisplayInfo(int initialPrice = -1, int maxLevels = -1, int[] incrementalPrices = null)
        {
            static float infoFunction(int level)
            {
                ITierEffectUpgradeConfiguration<int> config = GetConfiguration().RubberBootsConfiguration;
                return config.InitialEffect.Value + (level * config.IncrementalEffect.Value);
            }
            const string infoFormat = "LVL {0} - ${1} - Reduces the movement debuff when walking on water surfaces by {2}%\n";
            return Tools.GenerateInfoForUpgrade(infoFormat, initialPrice, incrementalPrices, infoFunction);
        }

        public override bool CanInitializeOnStart
        {
            get
            {
                ITierEffectUpgradeConfiguration<int> config = GetConfiguration().RubberBootsConfiguration;
                string[] prices = config.Prices.Value.Split(',');
                return prices.Length == 0 || (prices.Length == 1 && (prices[0].Length == 0 || prices[0] == "0"));
            }
        }

        public new static (string, string[]) RegisterScrapToUpgrade()
        {
            return (UPGRADE_NAME, GetConfiguration().RubberBootsConfiguration.ItemProgressionItems.Value.Split(","));
        }
        public new static void RegisterUpgrade()
        {
            GameObject prefab = LethalLib.Modules.NetworkPrefabs.CreateNetworkPrefab(UPGRADE_NAME);
            prefab.AddComponent<RubberBoots>();
            LethalLib.Modules.NetworkPrefabs.RegisterNetworkPrefab(prefab);
            Plugin.networkPrefabs[UPGRADE_NAME] = prefab;
        }
        public new static CustomTerminalNode RegisterTerminalNode()
        {
            return UpgradeBus.Instance.SetupMultiplePurchaseableTerminalNode(UPGRADE_NAME, GetConfiguration().RubberBootsConfiguration, Plugin.networkPrefabs[UPGRADE_NAME]);
        }
    }
}
