using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc.TerminalNodes;
using MoreShipUpgrades.Misc.Upgrades;
using MoreShipUpgrades.Misc.Util;
using UnityEngine;
using MoreShipUpgrades.Misc;

namespace MoreShipUpgrades.UpgradeComponents.TierUpgrades.Player
{
    internal class HikingBoots : TierUpgrade
    {
        internal const string UPGRADE_NAME = "Hiking Boots";
        internal const string PRICES_DEFAULT = "100,150,175";
        void Awake()
        {
            upgradeName = UPGRADE_NAME;
            overridenUpgradeName = GetConfiguration().HIKING_BOOTS_OVERRIDE_NAME;
        }
        static float ComputeUphillSlopeDebuffMultiplier()
        {
            LategameConfiguration config = GetConfiguration();
            return 1f - ((config.HIKING_BOOTS_INITIAL_DECREASE + (GetUpgradeLevel(UPGRADE_NAME) * config.HIKING_BOOTS_INCREMENTAL_DECREASE))/100f);
        }
        public static float ReduceUphillSlopeDebuff(float defaultValue)
        {
            if (!GetConfiguration().HIKING_BOOTS_ENABLED) return defaultValue;
            if (!GetActiveUpgrade(UPGRADE_NAME)) return defaultValue;
            float multiplier = ComputeUphillSlopeDebuffMultiplier();
            return Mathf.Clamp(defaultValue * multiplier, 0f, defaultValue);
        }
        public override string GetDisplayInfo(int initialPrice = -1, int maxLevels = -1, int[] incrementalPrices = null)
        {
            static float infoFunction(int level)
            {
                LategameConfiguration config = GetConfiguration();
                return config.HIKING_BOOTS_INITIAL_DECREASE.Value + (level * config.HIKING_BOOTS_INCREMENTAL_DECREASE.Value);
            }
            const string infoFormat = "LVL {0} - ${1} - Reduces the movement speed change when going through slopes by {2}%\n";
            return Tools.GenerateInfoForUpgrade(infoFormat, initialPrice, incrementalPrices, infoFunction);
        }

        public override bool CanInitializeOnStart
        {
            get
            {
                LategameConfiguration config = GetConfiguration();
                string[] prices = config.HIKING_BOOTS_PRICES.Value.Split(',');
                return config.HIKING_BOOTS_PRICE.Value <= 0 && prices.Length == 1 && (prices[0].Length == 0 || prices[0] == "0");
            }
        }

        public new static (string, string[]) RegisterScrapToUpgrade()
        {
            return (UPGRADE_NAME, GetConfiguration().HIKING_BOOTS_ITEM_PROGRESSION_ITEMS.Value.Split(","));
        }
        public new static void RegisterUpgrade()
        {
            SetupGenericPerk<HikingBoots>(UPGRADE_NAME);
        }
        public new static CustomTerminalNode RegisterTerminalNode()
        {
            LategameConfiguration configuration = GetConfiguration();

            return UpgradeBus.Instance.SetupMultiplePurchasableTerminalNode(UPGRADE_NAME,
                                                configuration.SHARED_UPGRADES || !configuration.HIKING_BOOTS_INDIVIDUAL,
                                                configuration.HIKING_BOOTS_ENABLED,
                                                configuration.HIKING_BOOTS_PRICE,
                                                UpgradeBus.ParseUpgradePrices(configuration.HIKING_BOOTS_PRICES),
                                                configuration.OVERRIDE_UPGRADE_NAMES ? configuration.HIKING_BOOTS_OVERRIDE_NAME : "");
        }
    }
}
