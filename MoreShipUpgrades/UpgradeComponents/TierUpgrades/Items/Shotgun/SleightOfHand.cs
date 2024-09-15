using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc.TerminalNodes;
using MoreShipUpgrades.Misc;
using MoreShipUpgrades.Misc.Upgrades;
using MoreShipUpgrades.Misc.Util;
using UnityEngine;

namespace MoreShipUpgrades.UpgradeComponents.TierUpgrades.Items.Shotgun
{
    internal class SleightOfHand : TierUpgrade
    {
        internal const string UPGRADE_NAME = "Sleight of Hand";
        internal const string PRICES_DEFAULT = "150,200,250";

        internal override void Start()
        {
            upgradeName = UPGRADE_NAME;
            overridenUpgradeName = GetConfiguration().SLEIGHT_OF_HAND_OVERRIDE_NAME;
            base.Start();
        }
        public override bool CanInitializeOnStart
        {
            get
            {
                LategameConfiguration config = GetConfiguration();
                string[] prices = config.SLEIGHT_OF_HAND_PRICES.Value.Split(',');
                return config.SLEIGHT_OF_HAND_PRICE.Value <= 0 && prices.Length == 1 && (prices[0].Length == 0 || prices[0] == "0");
            }
        }
        public override string GetDisplayInfo(int initialPrice = -1, int maxLevels = -1, int[] incrementalPrices = null)
        {
            static float infoFunction(int level)
            {
                LategameConfiguration config = GetConfiguration();
                return config.SLEIGHT_OF_HAND_INITIAL_INCREASE.Value + (level * config.SLEIGHT_OF_HAND_INCREMENTAL_INCREASE.Value);
            }
            const string infoFormat = "LVL {0} - ${1} - The reload speed of weaponry is increased by {2}%\n";
            return Tools.GenerateInfoForUpgrade(infoFormat, initialPrice, incrementalPrices, infoFunction);
        }
        public static float ComputeSleightOfHandSpeedBoost()
        {
            LategameConfiguration config = GetConfiguration();
            int percentage = config.SLEIGHT_OF_HAND_INITIAL_INCREASE + (GetUpgradeLevel(UPGRADE_NAME) * config.SLEIGHT_OF_HAND_INCREMENTAL_INCREASE);
            return percentage / 100f;
        }
        public static float GetSleightOfHandSpeedBoost(float defaultValue)
        {
            if (!GetConfiguration().SLEIGHT_OF_HAND_ENABLED) return defaultValue;
            if (!GetActiveUpgrade(UPGRADE_NAME)) return defaultValue;
            float multiplier = ComputeSleightOfHandSpeedBoost();
            return Mathf.Clamp(defaultValue * (1f - multiplier), 0, defaultValue);
        }
        public new static (string, string[]) RegisterScrapToUpgrade()
        {
            return (UPGRADE_NAME, GetConfiguration().SLEIGHT_OF_HAND_ITEM_PROGRESSION_ITEMS.Value.Split(","));
        }
        public new static void RegisterUpgrade()
        {
            SetupGenericPerk<SleightOfHand>(UPGRADE_NAME);
        }
        public new static CustomTerminalNode RegisterTerminalNode()
        {
            LategameConfiguration configuration = GetConfiguration();

            return UpgradeBus.Instance.SetupMultiplePurchasableTerminalNode(UPGRADE_NAME,
                                                shareStatus: configuration.SHARED_UPGRADES.Value || !configuration.SLEIGHT_OF_HAND_INDIVIDUAL.Value,
                                                configuration.SLEIGHT_OF_HAND_ENABLED.Value,
                                                configuration.SLEIGHT_OF_HAND_PRICE.Value,
                                                UpgradeBus.ParseUpgradePrices(configuration.SLEIGHT_OF_HAND_PRICES.Value),
                                                configuration.OVERRIDE_UPGRADE_NAMES ? configuration.SLEIGHT_OF_HAND_OVERRIDE_NAME : "");
        }
    }
}
