using MoreShipUpgrades.Configuration;
using MoreShipUpgrades.Configuration.Interfaces.TierUpgrades;
using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc.Upgrades;
using MoreShipUpgrades.Misc.Util;
using MoreShipUpgrades.UI.TerminalNodes;
using MoreShipUpgrades.UpgradeComponents.Interfaces;

namespace MoreShipUpgrades.UpgradeComponents.TierUpgrades.Items
{
    internal class LithiumBatteries : TierUpgrade, IUpgradeWorldBuilding
    {
        internal const string UPGRADE_NAME = "Lithium Batteries";
        internal const string PRICES_DEFAULT = "100,150, 200, 250, 300";
        internal const string WORLD_BUILDING_TEXT = "\n\nHobby-grade rechargeable batteries for your crew-portable electronic equipment." +
            " There is no battery recollection program offered by the Company, so there's no need to hold onto the old batteries. Just throw them in the ocean.\n\n";

        public string GetWorldBuildingText(bool shareStatus = false)
        {
            return WORLD_BUILDING_TEXT;
        }
        internal override void Start()
        {
            upgradeName = UPGRADE_NAME;
            overridenUpgradeName = GetConfiguration().LithiumBatteriesConfiguration.OverrideName;
            base.Start();
        }
        public static float GetChargeRateMultiplier(float defaultChargeRate)
        {
            ITierEffectUpgradeConfiguration<int> config = GetConfiguration().LithiumBatteriesConfiguration;
            if (!config.Enabled) return defaultChargeRate;
            if (!GetActiveUpgrade(UPGRADE_NAME)) return defaultChargeRate;
            float appliedMultiplier = config.InitialEffect.Value;
            appliedMultiplier += GetUpgradeLevel(UPGRADE_NAME) * config.IncrementalEffect.Value;
            appliedMultiplier = (100 - appliedMultiplier) / 100f;
            return defaultChargeRate * appliedMultiplier;
        }
        public override string GetDisplayInfo(int initialPrice = -1, int maxLevels = -1, int[] incrementalPrices = null)
        {
            static float infoFunction(int level)
            {
                ITierEffectUpgradeConfiguration<int> config = GetConfiguration().LithiumBatteriesConfiguration;
                return config.InitialEffect.Value + (level * config.IncrementalEffect.Value);
            }
            const string infoFormat = "LVL {0} - ${1} - Decreases the rate of battery used on the items by {2}%\n";
            return Tools.GenerateInfoForUpgrade(infoFormat, initialPrice, incrementalPrices, infoFunction);
        }

        public override bool CanInitializeOnStart
        {
            get
            {
                ITierEffectUpgradeConfiguration<int> config = GetConfiguration().LithiumBatteriesConfiguration;
                string[] prices = config.Prices.Value.Split(',');
                return prices.Length == 0 || (prices.Length == 1 && (prices[0].Length == 0 || prices[0] == "0"));
            }
        }

        public new static (string, string[]) RegisterScrapToUpgrade()
        {
            return (UPGRADE_NAME, GetConfiguration().LithiumBatteriesConfiguration.ItemProgressionItems.Value.Split(","));
        }
        public new static void RegisterUpgrade()
        {
            SetupGenericPerk<LithiumBatteries>(UPGRADE_NAME);
        }
        public new static CustomTerminalNode RegisterTerminalNode()
        {
            return UpgradeBus.Instance.SetupMultiplePurchaseableTerminalNode(UPGRADE_NAME, GetConfiguration().LithiumBatteriesConfiguration);
        }
    }
}
