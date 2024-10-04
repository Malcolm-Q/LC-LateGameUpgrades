using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc;
using MoreShipUpgrades.Misc.Upgrades;
using MoreShipUpgrades.Misc.Util;
using UnityEngine;
using LCVR;
using MoreShipUpgrades.UI.TerminalNodes;
using MoreShipUpgrades.UpgradeComponents.Interfaces;

namespace MoreShipUpgrades.UpgradeComponents.TierUpgrades.Ship
{
    internal class ScavengerInstincts : TierUpgrade, IUpgradeWorldBuilding
    {
        internal const string UPGRADE_NAME = "Scavenger Instincts";
        internal const string DEFAULT_PRICES = "800,1000,1200,1400";
        internal const string WORLD_BUILDING_TEXT = "\n\nAfter spending a lot of time in the facility, you begin to notice certain patterns in the location of valuable objects. Your department's output increases, but the implications are disturbing.\n\n";

        public string GetWorldBuildingText(bool shareStatus = false)
        {
            return WORLD_BUILDING_TEXT;
        }
        void Awake()
        {
            upgradeName = UPGRADE_NAME;
            overridenUpgradeName = GetConfiguration().SCAVENGER_INSTINCTS_OVERRIDE_NAME;
        }
        public override bool CanInitializeOnStart
        {
            get
            {
                LategameConfiguration config = GetConfiguration();
                string[] prices = config.SCAVENGER_INSTINCTS_PRICES.Value.Split(',');
                return config.SCAVENGER_INSTINCTS_PRICE.Value <= 0 && prices.Length == 1 && (prices[0].Length == 0 || prices[0] == "0");
            }
        }
        public static int IncreaseScrapAmount(int defaultValue)
        {
            LategameConfiguration config = GetConfiguration();
            if (!config.SCAVENGER_INSTINCTS_ENABLED) return defaultValue;
            if (!GetActiveUpgrade(UPGRADE_NAME)) return defaultValue;
            int additionalScrap = config.SCAVENGER_INSTINCTS_INITIAL_AMOUNT_SCRAP_INCREASE.Value + (GetUpgradeLevel(UPGRADE_NAME) * config.SCAVENGER_INSTINCTS_INCREMENTAL_AMOUN_SCRAP_INCREASE.Value);
            return Mathf.Clamp(defaultValue + Mathf.CeilToInt(additionalScrap / RoundManager.Instance.scrapAmountMultiplier), defaultValue, int.MaxValue);
        }
        public override string GetDisplayInfo(int initialPrice = -1, int maxLevels = -1, int[] incrementalPrices = null)
        {
            static float infoFunction(int level)
            {
                LategameConfiguration config = GetConfiguration();
                return config.SCAVENGER_INSTINCTS_INITIAL_AMOUNT_SCRAP_INCREASE.Value + (level * config.SCAVENGER_INSTINCTS_INCREMENTAL_AMOUN_SCRAP_INCREASE.Value);
            }
            const string infoFormat = "LVL {0} - ${1} - Increases the average amount of scrap spawns by {2} additional items.\n";
            return Tools.GenerateInfoForUpgrade(infoFormat, initialPrice, incrementalPrices, infoFunction);
        }
        public new static (string, string[]) RegisterScrapToUpgrade()
        {
            return (UPGRADE_NAME, GetConfiguration().SCAVENGER_INSTINCTS_ITEM_PROGRESSION_ITEMS.Value.Split(","));
        }
        public new static void RegisterUpgrade()
        {
            SetupGenericPerk<ScavengerInstincts>(UPGRADE_NAME);
        }
        public new static CustomTerminalNode RegisterTerminalNode()
        {
            LategameConfiguration configuration = GetConfiguration();

            return UpgradeBus.Instance.SetupMultiplePurchasableTerminalNode(UPGRADE_NAME,
                                                shareStatus: true,
                                                configuration.SCAVENGER_INSTINCTS_ENABLED,
                                                configuration.SCAVENGER_INSTINCTS_PRICE,
                                                UpgradeBus.ParseUpgradePrices(configuration.SCAVENGER_INSTINCTS_PRICES),
                                                configuration.OVERRIDE_UPGRADE_NAMES ? configuration.SCAVENGER_INSTINCTS_OVERRIDE_NAME : "");
        }
    }
}
