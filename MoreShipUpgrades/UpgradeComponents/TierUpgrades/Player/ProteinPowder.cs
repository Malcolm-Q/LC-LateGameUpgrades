using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc;
using MoreShipUpgrades.Misc.TerminalNodes;
using MoreShipUpgrades.Misc.Upgrades;
using MoreShipUpgrades.Misc.Util;
using MoreShipUpgrades.UpgradeComponents.Interfaces;

namespace MoreShipUpgrades.UpgradeComponents.TierUpgrades
{
    internal class ProteinPowder : TierUpgrade, IUpgradeWorldBuilding
    {
        public const string UPGRADE_NAME = "Protein Powder";
        internal const string WORLD_BUILDING_TEXT = "\n\nMultivitamins, creatine, and military surplus stimulants blended together and repackaged," +
            " then offered on subscription. Known to be habit-forming. The label includes a Company Surgeon General's warning about increased aggression.\n\n";

        const int CRIT_DAMAGE_VALUE = 100;

        // Configuration
        public const string ENABLED_SECTION = $"Enable {UPGRADE_NAME} Upgrade";
        public const bool ENABLED_DEFAULT = true;
        public const string ENABLED_DESCRIPTION = "Do more damage with shovels";

        public const string PRICE_SECTION = $"Price of {UPGRADE_NAME} Upgrade";
        public const int PRICE_DEFAULT = 1000;

        public const string UNLOCK_FORCE_SECTION = "Initial additional hit force";
        public const int UNLOCK_FORCE_DEFAULT = 1;
        public const string UNLOCK_FORCE_DESCRIPTION = "The value added to hit force on initial unlock.";

        public const string INCREMENT_FORCE_SECTION = "Additional hit force per level";
        public const int INCREMENT_FORCE_DEFAULT = 1;
        public const string INCREMENT_FORCE_DESCRIPTION = $"Every time {UPGRADE_NAME} is upgraded this value will be added to the value above.";

        public const string PRICES_DEFAULT = "700";

        public const string CRIT_CHANCE_SECTION = "Chance of dealing a crit which will instakill the enemy.";
        public const float CRIT_CHANCE_DEFAULT = 0.01f;
        public const string CRIT_CHANCE_DESCRIPTION = $"This value is only valid when maxed out {UPGRADE_NAME}. Any previous levels will not apply crit.";

        internal override void Start()
        {
            upgradeName = UPGRADE_NAME;
            overridenUpgradeName = UpgradeBus.Instance.PluginConfiguration.PROTEIN_POWDER_OVERRIDE_NAME;
            base.Start();
        }

        public static int GetShovelHitForce(int force)
        {
            if (!UpgradeBus.Instance.PluginConfiguration.PROTEIN_ENABLED.Value) return force;
            if (!GetActiveUpgrade(UPGRADE_NAME)) return force;
            int proteinForce = TryToCritEnemy() ? CRIT_DAMAGE_VALUE : UpgradeBus.Instance.PluginConfiguration.PROTEIN_UNLOCK_FORCE.Value + (UpgradeBus.Instance.PluginConfiguration.PROTEIN_INCREMENT.Value * GetUpgradeLevel(UPGRADE_NAME));
            return force + proteinForce;
        }

        private static bool TryToCritEnemy()
        {
            string[] prices = UpgradeBus.Instance.PluginConfiguration.PROTEIN_UPGRADE_PRICES.Value.Split(',');
            int maximumLevel = prices.Length;
            int currentLevel = GetUpgradeLevel(UPGRADE_NAME);

            if (currentLevel != maximumLevel && !(prices.Length == 1 && (prices[0].Length == 0 || prices[0] == "0"))) return false;

            return UnityEngine.Random.value < UpgradeBus.Instance.PluginConfiguration.PROTEIN_CRIT_CHANCE.Value;
        }

        public string GetWorldBuildingText(bool shareStatus = false)
        {
            return WORLD_BUILDING_TEXT;
        }

        public override string GetDisplayInfo(int initialPrice = -1, int maxLevels = -1, int[] incrementalPrices = null)
        {
            static float infoFunction(int level) => UpgradeBus.Instance.PluginConfiguration.PROTEIN_UNLOCK_FORCE.Value + (UpgradeBus.Instance.PluginConfiguration.PROTEIN_INCREMENT.Value * level);
            string infoFormat = AssetBundleHandler.GetInfoFromJSON(UPGRADE_NAME);
            return Tools.GenerateInfoForUpgrade(infoFormat, initialPrice, incrementalPrices, infoFunction);
        }
        public override bool CanInitializeOnStart
        {
            get
            {
                string[] prices = UpgradeBus.Instance.PluginConfiguration.PROTEIN_UPGRADE_PRICES.Value.Split(',');
                return UpgradeBus.Instance.PluginConfiguration.PROTEIN_PRICE.Value <= 0 && prices.Length == 1 && (prices[0].Length == 0 || prices[0] == "0");
            }
        }

        public new static (string, string[]) RegisterScrapToUpgrade()
        {
            return (UPGRADE_NAME, UpgradeBus.Instance.PluginConfiguration.PROTEIN_POWDER_ITEM_PROGRESSION_ITEMS.Value.Split(","));
        }
        public new static void RegisterUpgrade()
        {
            SetupGenericPerk<ProteinPowder>(UPGRADE_NAME);
        }
        public new static CustomTerminalNode RegisterTerminalNode()
        {
            LategameConfiguration configuration = UpgradeBus.Instance.PluginConfiguration;

            return UpgradeBus.Instance.SetupMultiplePurchasableTerminalNode(UPGRADE_NAME,
                                                configuration.SHARED_UPGRADES.Value || !configuration.PROTEIN_INDIVIDUAL.Value,
                                                configuration.PROTEIN_ENABLED.Value,
                                                configuration.PROTEIN_PRICE.Value,
                                                UpgradeBus.ParseUpgradePrices(configuration.PROTEIN_UPGRADE_PRICES.Value),
                                                configuration.OVERRIDE_UPGRADE_NAMES ? configuration.PROTEIN_POWDER_OVERRIDE_NAME : "");
        }
    }
}
