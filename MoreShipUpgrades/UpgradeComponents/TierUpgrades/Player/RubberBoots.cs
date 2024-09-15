using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc.TerminalNodes;
using MoreShipUpgrades.Misc;
using MoreShipUpgrades.Misc.Upgrades;
using MoreShipUpgrades.Misc.Util;
using UnityEngine;

namespace MoreShipUpgrades.UpgradeComponents.TierUpgrades.Player
{
    public class RubberBoots : TierUpgrade
    {
        internal const string UPGRADE_NAME = "Rubber Boots";
        internal const string DEFAULT_PRICES = "50,100,200";
        void Awake()
        {
            upgradeName = UPGRADE_NAME;
            overridenUpgradeName = GetConfiguration().RUBBER_BOOTS_OVERRIDE_NAME;
        }
        public static float CalculateDecreaseMultiplier()
        {
            LategameConfiguration config = GetConfiguration();
            if (!config.RUBBER_BOOTS_ENABLED || !GetActiveUpgrade(UPGRADE_NAME)) return 0f;
            return (config.RUBBER_BOOTS_INITIAL_MOVEMENT_HINDERANCE_DECREASE + (config.RUBBER_BOOTS_INCREMENTAL_MOVEMENT_HINDERANCE_DECREASE * GetUpgradeLevel(UPGRADE_NAME))) / 100f;
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
                LategameConfiguration config = GetConfiguration();
                return config.RUBBER_BOOTS_INITIAL_MOVEMENT_HINDERANCE_DECREASE.Value + (level * config.RUBBER_BOOTS_INCREMENTAL_MOVEMENT_HINDERANCE_DECREASE.Value);
            }
            const string infoFormat = "LVL {0} - ${1} - Reduces the movement debuff when walking on water surfaces by {2}%\n";
            return Tools.GenerateInfoForUpgrade(infoFormat, initialPrice, incrementalPrices, infoFunction);
        }

        public override bool CanInitializeOnStart
        {
            get
            {
                LategameConfiguration config = GetConfiguration();
                string[] prices = config.RUBBER_BOOTS_PRICES.Value.Split(',');
                return config.RUBBER_BOOTS_PRICE.Value <= 0 && prices.Length == 1 && (prices[0].Length == 0 || prices[0] == "0");
            }
        }

        public new static (string, string[]) RegisterScrapToUpgrade()
        {
            return (UPGRADE_NAME, GetConfiguration().RUBBER_BOOTS_ITEM_PROGRESSION_ITEMS.Value.Split(","));
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
            LategameConfiguration configuration = GetConfiguration();

            return UpgradeBus.Instance.SetupMultiplePurchasableTerminalNode(UPGRADE_NAME,
                                                configuration.SHARED_UPGRADES || !configuration.RUBBER_BOOTS_INDIVIDUAL,
                                                configuration.RUBBER_BOOTS_ENABLED,
                                                configuration.RUBBER_BOOTS_PRICE,
                                                UpgradeBus.ParseUpgradePrices(configuration.RUBBER_BOOTS_PRICES),
                                                configuration.OVERRIDE_UPGRADE_NAMES ? configuration.RUBBER_BOOTS_OVERRIDE_NAME : "",
                                                Plugin.networkPrefabs[UPGRADE_NAME]);
        }
    }
}
