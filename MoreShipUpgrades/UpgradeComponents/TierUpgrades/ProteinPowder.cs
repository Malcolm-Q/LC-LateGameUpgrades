using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc;
using MoreShipUpgrades.Misc.Upgrades;
using MoreShipUpgrades.UpgradeComponents.Interfaces;
using System;

namespace MoreShipUpgrades.UpgradeComponents.TierUpgrades
{
    internal class ProteinPowder : TierUpgrade, IUpgradeWorldBuilding
    {
        public const string UPGRADE_NAME = "Protein Powder";
        internal const string WORLD_BUILDING_TEXT = "\n\nMultivitamins, creatine, and military surplus stimulants blended together and repackaged," +
            " then offered on subscription. Known to be habit-forming. The label includes a Company Surgeon General's warning about increased aggression.\n\n";

        private static int CRIT_DAMAGE_VALUE = 100;

        // Configuration
        public static string ENABLED_SECTION = $"Enable {UPGRADE_NAME} Upgrade";
        public static bool ENABLED_DEFAULT = true;
        public static string ENABLED_DESCRIPTION = "Do more damage with shovels";

        public static string PRICE_SECTION = $"Price of {UPGRADE_NAME} Upgrade";
        public static int PRICE_DEFAULT = 1000;

        public static string UNLOCK_FORCE_SECTION = "Initial additional hit force";
        public static int UNLOCK_FORCE_DEFAULT = 1;
        public static string UNLOCK_FORCE_DESCRIPTION = "The value added to hit force on initial unlock.";

        public static string INCREMENT_FORCE_SECTION = "Additional hit force per level";
        public static int INCREMENT_FORCE_DEFAULT = 1;
        public static string INCREMENT_FORCE_DESCRIPTION = $"Every time {UPGRADE_NAME} is upgraded this value will be added to the value above.";

        public static string PRICES_DEFAULT = "700";

        public static string CRIT_CHANCE_SECTION = "Chance of dealing a crit which will instakill the enemy.";
        public static float CRIT_CHANCE_DEFAULT = 0.01f;
        public static string CRIT_CHANCE_DESCRIPTION = $"This value is only valid when maxed out {UPGRADE_NAME}. Any previous levels will not apply crit.";

        internal override void Start()
        {
            upgradeName = UPGRADE_NAME;
            base.Start();
        }

        public static int GetShovelHitForce(int force)
        {
            // Truly one of THE ternary operators
            return (GetActiveUpgrade(UPGRADE_NAME) ? TryToCritEnemy() ? CRIT_DAMAGE_VALUE : UpgradeBus.instance.cfg.PROTEIN_INCREMENT.Value * GetUpgradeLevel(UPGRADE_NAME) + UpgradeBus.instance.cfg.PROTEIN_UNLOCK_FORCE.Value + force : force) + UpgradeBus.instance.damageBoost;
            // .damageBoost is tied to boombox upgrade, it will always be 0 when inactive or x when active.
        }

        private static bool TryToCritEnemy()
        {
            int maximumLevel = UpgradeBus.instance.cfg.PROTEIN_UPGRADE_PRICES.Value.Split(',').Length;
            int currentLevel = GetUpgradeLevel(UPGRADE_NAME);

            if (currentLevel != maximumLevel) return false;

            return UnityEngine.Random.value < UpgradeBus.instance.cfg.PROTEIN_CRIT_CHANCE.Value;
        }

        public string GetWorldBuildingText(bool shareStatus = false)
        {
            return WORLD_BUILDING_TEXT;
        }

        public override string GetDisplayInfo(int initialPrice = -1, int maxLevels = -1, int[] incrementalPrices = null)
        {
            Func<int, float> infoFunction = level => UpgradeBus.instance.cfg.PROTEIN_UNLOCK_FORCE.Value + 1 + (UpgradeBus.instance.cfg.PROTEIN_INCREMENT.Value * level);
            string infoFormat = AssetBundleHandler.GetInfoFromJSON(UPGRADE_NAME);
            return Tools.GenerateInfoForUpgrade(infoFormat, initialPrice, incrementalPrices, infoFunction);
        }
    }
}
