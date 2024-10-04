using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc;
using MoreShipUpgrades.Misc.Upgrades;
using MoreShipUpgrades.Misc.Util;
using MoreShipUpgrades.UI.TerminalNodes;
using MoreShipUpgrades.UpgradeComponents.Interfaces;
using UnityEngine;

namespace MoreShipUpgrades.UpgradeComponents.TierUpgrades.Player
{
    internal class ReinforcedBoots : TierUpgrade, IUpgradeWorldBuilding
    {
        internal const string UPGRADE_NAME = "Reinforced Boots";
        internal const string DEFAULT_PRICES = "200,300,400,500";
        internal const string WORLD_BUILDING_TEXT = "\n\nIn the 'GEAR' section of the Company Catalogue, the third ad on the first page is for this product." +
            " They're Premium High-Impact Shock Absorbent Heel & Toe Pads for your crew's boots." +
            " The rhetoric of the advertisement promised a 19% increase in your departent's overall safety when traversing outdoors.\n\n";

        public string GetWorldBuildingText(bool shareStatus = false)
        {
            return WORLD_BUILDING_TEXT;
        }
        void Awake()
        {
            upgradeName = UPGRADE_NAME;
            overridenUpgradeName = GetConfiguration().REINFORCED_BOOTS_OVERRIDE_NAME;
        }
        public static int ReduceFallDamage(int defaultValue)
        {
            LategameConfiguration config = GetConfiguration();
            if (!config.REINFORCED_BOOTS_ENABLED) return defaultValue;
            if (!GetActiveUpgrade(UPGRADE_NAME)) return defaultValue;
            float multiplier = 1f - ((config.REINFORCED_BOOTS_INITIAL_DAMAGE_REDUCTION + (GetUpgradeLevel(UPGRADE_NAME) * config.REINFORCED_BOOTS_INCREMENTAL_DAMAGE_REDUCTION)) / 100f);
            return (int)Mathf.Clamp(defaultValue * multiplier, 0f, defaultValue);
        }
        public override string GetDisplayInfo(int initialPrice = -1, int maxLevels = -1, int[] incrementalPrices = null)
        {
            static float infoFunction(int level)
            {
                LategameConfiguration config = GetConfiguration();
                return config.REINFORCED_BOOTS_INITIAL_DAMAGE_REDUCTION.Value + (level * config.REINFORCED_BOOTS_INCREMENTAL_DAMAGE_REDUCTION.Value);
            }
            const string infoFormat = "LVL {0} - ${1} - Reduces fall damage by {2}%\n";
            return Tools.GenerateInfoForUpgrade(infoFormat, initialPrice, incrementalPrices, infoFunction);
        }

        public override bool CanInitializeOnStart
        {
            get
            {
                LategameConfiguration config = GetConfiguration();
                string[] prices = config.REINFORCED_BOOTS_PRICES.Value.Split(',');
                return config.REINFORCED_BOOTS_PRICE.Value <= 0 && prices.Length == 1 && (prices[0].Length == 0 || prices[0] == "0");
            }
        }

        public new static (string, string[]) RegisterScrapToUpgrade()
        {
            return (UPGRADE_NAME, GetConfiguration().REINFORCED_BOOTS_ITEM_PROGRESSION_ITEMS.Value.Split(","));
        }
        public new static void RegisterUpgrade()
        {
            SetupGenericPerk<ReinforcedBoots>(UPGRADE_NAME);
        }
        public new static CustomTerminalNode RegisterTerminalNode()
        {
            LategameConfiguration configuration = GetConfiguration();

            return UpgradeBus.Instance.SetupMultiplePurchasableTerminalNode(UPGRADE_NAME,
                                                configuration.SHARED_UPGRADES || !configuration.REINFORCED_BOOTS_INDIVIDUAL,
                                                configuration.REINFORCED_BOOTS_ENABLED,
                                                configuration.REINFORCED_BOOTS_PRICE,
                                                UpgradeBus.ParseUpgradePrices(configuration.REINFORCED_BOOTS_PRICES),
                                                configuration.OVERRIDE_UPGRADE_NAMES ? configuration.REINFORCED_BOOTS_OVERRIDE_NAME : "");
        }
    }
}
