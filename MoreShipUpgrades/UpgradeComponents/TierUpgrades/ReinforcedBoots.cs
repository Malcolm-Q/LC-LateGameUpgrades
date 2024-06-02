using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc;
using MoreShipUpgrades.Misc.TerminalNodes;
using MoreShipUpgrades.Misc.Upgrades;
using MoreShipUpgrades.Misc.Util;
using UnityEngine;

namespace MoreShipUpgrades.UpgradeComponents.TierUpgrades
{
    internal class ReinforcedBoots : TierUpgrade
    {
        internal const string UPGRADE_NAME = "Reinforced Boots";
        internal const string DEFAULT_PRICES = "200,300,400,500";
        void Awake()
        {
            upgradeName = UPGRADE_NAME;
            overridenUpgradeName = UpgradeBus.Instance.PluginConfiguration.REINFORCED_BOOTS_OVERRIDE_NAME;
        }
        public static int ReduceFallDamage(int defaultValue)
        {
            if (!(GetActiveUpgrade(UPGRADE_NAME))) return defaultValue;
            float multiplier = 1f - ((UpgradeBus.Instance.PluginConfiguration.REINFORCED_BOOTS_INITIAL_DAMAGE_REDUCTION + (GetUpgradeLevel(UPGRADE_NAME) * UpgradeBus.Instance.PluginConfiguration.REINFORCED_BOOTS_INCREMENTAL_DAMAGE_REDUCTION)) / 100f);
            return (int)Mathf.Clamp(defaultValue * multiplier, 0f, defaultValue);
        }
        public override string GetDisplayInfo(int initialPrice = -1, int maxLevels = -1, int[] incrementalPrices = null)
        {
            System.Func<int, float> infoFunction = level => UpgradeBus.Instance.PluginConfiguration.REINFORCED_BOOTS_INITIAL_DAMAGE_REDUCTION.Value + (level * UpgradeBus.Instance.PluginConfiguration.REINFORCED_BOOTS_INCREMENTAL_DAMAGE_REDUCTION.Value);
            string infoFormat = "LVL {0} - ${1} - Reduces fall damage by {2}%\n";
            return Tools.GenerateInfoForUpgrade(infoFormat, initialPrice, incrementalPrices, infoFunction);
        }

        public override bool CanInitializeOnStart
        {
            get
            {
                string[] prices = UpgradeBus.Instance.PluginConfiguration.REINFORCED_BOOTS_PRICES.Value.Split(',');
                bool free = UpgradeBus.Instance.PluginConfiguration.REINFORCED_BOOTS_PRICE.Value <= 0 && prices.Length == 1 && (prices[0] == "" || prices[0] == "0");
                return free;
            }
        }

        public new static (string, string[]) RegisterScrapToUpgrade()
        {
            return (UPGRADE_NAME, UpgradeBus.Instance.PluginConfiguration.REINFORCED_BOOTS_ITEM_PROGRESSION_ITEMS.Value.Split(","));
        }
        public new static void RegisterUpgrade()
        {
            SetupGenericPerk<ReinforcedBoots>(UPGRADE_NAME);
        }
        public new static CustomTerminalNode RegisterTerminalNode()
        {
            LategameConfiguration configuration = UpgradeBus.Instance.PluginConfiguration;

            return UpgradeBus.Instance.SetupMultiplePurchasableTerminalNode(UPGRADE_NAME,
                                                configuration.SHARED_UPGRADES || !configuration.REINFORCED_BOOTS_INDIVIDUAL,
                                                configuration.REINFORCED_BOOTS_ENABLED,
                                                configuration.REINFORCED_BOOTS_PRICE,
                                                UpgradeBus.ParseUpgradePrices(configuration.REINFORCED_BOOTS_PRICES),
                                                configuration.OVERRIDE_UPGRADE_NAMES ? configuration.REINFORCED_BOOTS_OVERRIDE_NAME : "");
        }
    }
}
