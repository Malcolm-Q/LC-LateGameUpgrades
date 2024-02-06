using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc;
using MoreShipUpgrades.Misc.Upgrades;
using MoreShipUpgrades.UpgradeComponents.Interfaces;

namespace MoreShipUpgrades.UpgradeComponents.TierUpgrades.AttributeUpgrades
{
    class BiggerLungs : GameAttributeTierUpgrade, IUpgradeWorldBuilding
    {
        public const string UPGRADE_NAME = "Bigger Lungs";
        public static string PRICES_DEFAULT = "350,450,550";
        internal const string WORLD_BUILDING_TEXT = "\n\nService package for {0}." +
            " Opting into every maintenance procedure will arrange for your suit's pipes to be cleaned and repaired, filters re-issued," +
            " and DRM removed from the integrated air conditioning system.\n\n";

        internal override void Start()
        {
            upgradeName = UPGRADE_NAME;
            logger = new LGULogger(UPGRADE_NAME);
            base.Start();
            changingAttribute = GameAttribute.PLAYER_SPRINT_TIME;
            initialValue = UpgradeBus.instance.cfg.SPRINT_TIME_INCREASE_UNLOCK;
            incrementalValue = UpgradeBus.instance.cfg.SPRINT_TIME_INCREMENT;
        }

        public override void Increment()
        {
            base.Increment();
            UpgradeBus.instance.lungLevel++;
        }

        public override void Load()
        {
            LoadUpgradeAttribute(ref UpgradeBus.instance.biggerLungs, UpgradeBus.instance.lungLevel);
            base.Load();
        }

        public override void Unwind()
        {
            UnloadUpgradeAttribute(ref UpgradeBus.instance.biggerLungs, ref UpgradeBus.instance.lungLevel);
            base.Unwind();
        }
        public static float ApplyPossibleIncreasedStaminaRegen(float regenValue)
        {
            if (!UpgradeBus.instance.biggerLungs || UpgradeBus.instance.lungLevel < 0) return regenValue * UpgradeBus.instance.staminaDrainCoefficient;
            return regenValue * UpgradeBus.instance.cfg.BIGGER_LUNGS_STAMINA_REGEN_INCREASE * UpgradeBus.instance.staminaDrainCoefficient;
        }

        public static float ApplyPossibleReducedJumpStaminaCost(float jumpCost)
        {
            if (!UpgradeBus.instance.biggerLungs || UpgradeBus.instance.lungLevel < 1) return jumpCost;
            return jumpCost * UpgradeBus.instance.cfg.BIGGER_LUNGS_JUMP_STAMINA_COST_DECREASE;
        }
        public string GetWorldBuildingText(bool shareStatus = false)
        {
            return string.Format(WORLD_BUILDING_TEXT, shareStatus ? "your crew's suit oxigen delivery systems" : "your suit's oxygen delivery system");
        }

        public override string GetDisplayInfo(int initialPrice = -1, int maxLevels = -1, int[] incrementalPrices = null)
        {
            System.Func<int, float> infoFunction = level => UpgradeBus.instance.cfg.SPRINT_TIME_INCREASE_UNLOCK + level * UpgradeBus.instance.cfg.SPRINT_TIME_INCREMENT;
            string infoFormat = AssetBundleHandler.GetInfoFromJSON(UPGRADE_NAME);
            return Tools.GenerateInfoForUpgrade(infoFormat, initialPrice, incrementalPrices, infoFunction);
        }
    }
}
