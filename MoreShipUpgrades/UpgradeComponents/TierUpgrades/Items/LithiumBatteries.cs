using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc;
using MoreShipUpgrades.Misc.Upgrades;
using MoreShipUpgrades.Misc.Util;
using MoreShipUpgrades.UI.TerminalNodes;

namespace MoreShipUpgrades.UpgradeComponents.TierUpgrades.Items
{
    internal class LithiumBatteries : TierUpgrade
    {
        internal const string UPGRADE_NAME = "Lithium Batteries";
        internal const string PRICES_DEFAULT = "150, 200, 250, 300";

        internal override void Start()
        {
            upgradeName = UPGRADE_NAME;
            overridenUpgradeName = GetConfiguration().LITHIUM_BATTERIES_OVERRIDE_NAME;
            base.Start();
        }
        public static float GetChargeRateMultiplier(float defaultChargeRate)
        {
            LategameConfiguration config = GetConfiguration();
            if (!config.LITHIUM_BATTERIES_ENABLED) return defaultChargeRate;
            if (!GetActiveUpgrade(UPGRADE_NAME)) return defaultChargeRate;
            float appliedMultiplier = config.LITHIUM_BATTERIES_INITIAL_MULTIPLIER.Value;
            appliedMultiplier += GetUpgradeLevel(UPGRADE_NAME) * config.LITHIUM_BATTERIES_INCREMENTAL_MULTIPLIER.Value;
            appliedMultiplier = (100 - appliedMultiplier) / 100f;
            return defaultChargeRate * appliedMultiplier;
        }
        public override string GetDisplayInfo(int initialPrice = -1, int maxLevels = -1, int[] incrementalPrices = null)
        {
            static float infoFunction(int level)
            {
                LategameConfiguration config = GetConfiguration();
                return config.LITHIUM_BATTERIES_INITIAL_MULTIPLIER.Value + (level * config.LITHIUM_BATTERIES_INCREMENTAL_MULTIPLIER.Value);
            }
            const string infoFormat = "LVL {0} - ${1} - Decreases the rate of battery used on the items by {2}%\n";
            return Tools.GenerateInfoForUpgrade(infoFormat, initialPrice, incrementalPrices, infoFunction);
        }

        public override bool CanInitializeOnStart
        {
            get
            {
                LategameConfiguration config = GetConfiguration();
                string[] prices = config.LITHIUM_BATTERIES_PRICES.Value.Split(',');
                return config.LITHIUM_BATTERIES_PRICE.Value <= 0 && prices.Length == 1 && (prices[0].Length == 0 || prices[0] == "0");
            }
        }

        public new static (string, string[]) RegisterScrapToUpgrade()
        {
            return (UPGRADE_NAME, GetConfiguration().LITHIUM_BATTERIES_ITEM_PROGRESSION_ITEMS.Value.Split(","));
        }
        public new static void RegisterUpgrade()
        {
            SetupGenericPerk<LithiumBatteries>(UPGRADE_NAME);
        }
        public new static CustomTerminalNode RegisterTerminalNode()
        {
            LategameConfiguration configuration = GetConfiguration();

            return UpgradeBus.Instance.SetupMultiplePurchasableTerminalNode(UPGRADE_NAME,
                                                configuration.SHARED_UPGRADES.Value || !configuration.LITHIUM_BATTERIES_INDIVIDUAL.Value,
                                                configuration.LITHIUM_BATTERIES_ENABLED.Value,
                                                configuration.LITHIUM_BATTERIES_PRICE.Value,
                                                UpgradeBus.ParseUpgradePrices(configuration.LITHIUM_BATTERIES_PRICES.Value),
                                                configuration.OVERRIDE_UPGRADE_NAMES ? configuration.LITHIUM_BATTERIES_OVERRIDE_NAME : "");
        }
    }
}
