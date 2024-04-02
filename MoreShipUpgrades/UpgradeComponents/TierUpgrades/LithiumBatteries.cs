using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc.Upgrades;
using MoreShipUpgrades.Misc.Util;

namespace MoreShipUpgrades.UpgradeComponents.TierUpgrades
{
    internal class LithiumBatteries : TierUpgrade
    {
        internal const string UPGRADE_NAME = "Lithium Batteries";
        internal const string PRICES_DEFAULT = "150, 200, 250, 300";

        internal override void Start()
        {
            upgradeName = UPGRADE_NAME;
            base.Start();
        }
        public static float GetChargeRateMultiplier(float defaultChargeRate)
        {
            if (!GetActiveUpgrade(UPGRADE_NAME)) return defaultChargeRate;
            float appliedMultiplier = UpgradeBus.Instance.PluginConfiguration.LITHIUM_BATTERIES_INITIAL_MULTIPLIER.Value;
            appliedMultiplier += GetUpgradeLevel(UPGRADE_NAME) * UpgradeBus.Instance.PluginConfiguration.LITHIUM_BATTERIES_INCREMENTAL_MULTIPLIER.Value;
            appliedMultiplier = (100 - appliedMultiplier) / 100f;
            return defaultChargeRate * appliedMultiplier;
        }
        public override string GetDisplayInfo(int initialPrice = -1, int maxLevels = -1, int[] incrementalPrices = null)
        {
            System.Func<int, float> infoFunction = level => UpgradeBus.Instance.PluginConfiguration.LITHIUM_BATTERIES_INITIAL_MULTIPLIER.Value + (level * UpgradeBus.Instance.PluginConfiguration.LITHIUM_BATTERIES_INCREMENTAL_MULTIPLIER.Value);
            string infoFormat = "LVL {0} - ${1} - Decreases the rate of battery used on the items by {2}%\n";
            return Tools.GenerateInfoForUpgrade(infoFormat, initialPrice, incrementalPrices, infoFunction);
        }

        internal override bool CanInitializeOnStart()
        {
            string[] prices = UpgradeBus.Instance.PluginConfiguration.LITHIUM_BATTERIES_PRICES.Value.Split(',');
            bool free = UpgradeBus.Instance.PluginConfiguration.LITHIUM_BATTERIES_PRICE.Value <= 0 && prices.Length == 1 && (prices[0] == "" || prices[0] == "0");
            return free;
        }
    }
}
