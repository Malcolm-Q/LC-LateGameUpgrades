using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc.Upgrades;
using MoreShipUpgrades.Misc.Util;
using UnityEngine;
using MoreShipUpgrades.UI.TerminalNodes;
using MoreShipUpgrades.Configuration.Upgrades.Interfaces.TierUpgrades;
using MoreShipUpgrades.UpgradeComponents.Interfaces;

namespace MoreShipUpgrades.UpgradeComponents.TierUpgrades.Items.WeedKiller
{
    internal class WeedGeneticManipulation : TierUpgrade, IUpgradeWorldBuilding
    {
        internal const string UPGRADE_NAME = "Weed Genetic Manipulation";
        internal const string PRICES_DEFAULT = "100,100,150,200";
        internal const string WORLD_BUILDING_TEXT = "You figured out that pissing into the Weed Killer makes it more effective." +
            " The cost of this upgrade comes from extra portable water requisitions your department needs to order to make the project happen.";

        internal const string WORLD_BUILDING_TEXT_2 = "Engineered to be extra deadly to Vain Shrouds and Cadaver Growths infesting different moons," +
            " and yet they keep coming back all the same. It's been rumoured the Company is engineering both the problem and the solution," +
            " and no it does not tame the split-faced dogs! They are not your friend!";

        public string GetWorldBuildingText(bool shareStatus = false)
        {
            if (UnityEngine.Random.value < 0.5f)
            {
                return WORLD_BUILDING_TEXT;
            }
            else
            {
                return WORLD_BUILDING_TEXT_2;
            }
        }
        internal override void Start()
        {
            upgradeName = UPGRADE_NAME;
            overridenUpgradeName = GetConfiguration().WeedGeneticManipulationConfiguration.OverrideName;
            base.Start();
        }
        public static float ComputeWeedKillerEffectiveness()
        {
            ITierEffectUpgradeConfiguration<int> upgradeConfig = GetConfiguration().WeedGeneticManipulationConfiguration;
            int percentage = upgradeConfig.InitialEffect + (GetUpgradeLevel(UPGRADE_NAME) * upgradeConfig.IncrementalEffect);
            return percentage / 100f;
        }
        public static float GetWeedKillerEffectiveness(float defaultValue)
        {
            if (!GetConfiguration().WeedGeneticManipulationConfiguration.Enabled) return defaultValue;
            if (!GetActiveUpgrade(UPGRADE_NAME)) return defaultValue;
            float multiplier = ComputeWeedKillerEffectiveness();
            return Mathf.Clamp(defaultValue * multiplier, defaultValue, float.MaxValue);
        }
        public override bool CanInitializeOnStart
        {
            get
            {
                ITierEffectUpgradeConfiguration<int> upgradeConfig = GetConfiguration().WeedGeneticManipulationConfiguration;
                string[] prices = upgradeConfig.Prices.Value.Split(',');
                return prices.Length == 0 || (prices.Length == 1 && (prices[0].Length == 0 || prices[0] == "0"));
            }
        }
        public override string GetDisplayInfo(int initialPrice = -1, int maxLevels = -1, int[] incrementalPrices = null)
        {
            static float infoFunction(int level)
            {
                ITierEffectUpgradeConfiguration<int> upgradeConfig = GetConfiguration().WeedGeneticManipulationConfiguration;
                return upgradeConfig.InitialEffect.Value + (level * upgradeConfig.IncrementalEffect.Value);
            }
            const string infoFormat = "LVL {0} - {1} - Effectiveness of the Weed Killer item in eradicating plants is increased by {2}%\n";
            return Tools.GenerateInfoForUpgrade(infoFormat, initialPrice, incrementalPrices, infoFunction, purchaseMode: GetConfiguration().WeedGeneticManipulationConfiguration.PurchaseMode);
        }
        public new static (string, string[]) RegisterScrapToUpgrade()
        {
            return (UPGRADE_NAME, GetConfiguration().WeedGeneticManipulationConfiguration.ItemProgressionItems.Value.Split(","));
        }
        public new static void RegisterUpgrade()
        {
            SetupGenericPerk<WeedGeneticManipulation>(UPGRADE_NAME);
        }
        public new static CustomTerminalNode RegisterTerminalNode()
        {
            return UpgradeBus.Instance.SetupMultiplePurchaseableTerminalNode(UPGRADE_NAME, GetConfiguration().WeedGeneticManipulationConfiguration);
        }
    }
}
