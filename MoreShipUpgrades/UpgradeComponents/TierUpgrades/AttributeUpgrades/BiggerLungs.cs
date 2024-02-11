using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc;
using MoreShipUpgrades.Misc.Upgrades;
using MoreShipUpgrades.UpgradeComponents.Interfaces;
using System.Collections.Generic;

namespace MoreShipUpgrades.UpgradeComponents.TierUpgrades.AttributeUpgrades
{
    class BiggerLungs : GameAttributeTierUpgrade, IUpgradeWorldBuilding, IPlayerSync
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
            initialValue = UpgradeBus.instance.cfg.SPRINT_TIME_INCREASE_UNLOCK.Value;
            incrementalValue = UpgradeBus.instance.cfg.SPRINT_TIME_INCREMENT.Value;
        }
        public static float ApplyPossibleIncreasedStaminaRegen(float regenValue)
        {
            if (!GetActiveUpgrade(UPGRADE_NAME) || GetUpgradeLevel(UPGRADE_NAME) < 0) return regenValue * UpgradeBus.instance.staminaDrainCoefficient;
            return regenValue * UpgradeBus.instance.cfg.BIGGER_LUNGS_STAMINA_REGEN_INCREASE.Value * UpgradeBus.instance.staminaDrainCoefficient;
        }

        public static float ApplyPossibleReducedJumpStaminaCost(float jumpCost)
        {
            if (!GetActiveUpgrade(UPGRADE_NAME) || GetUpgradeLevel(UPGRADE_NAME) < 1) return jumpCost;
            return jumpCost * UpgradeBus.instance.cfg.BIGGER_LUNGS_JUMP_STAMINA_COST_DECREASE.Value;
        }
        public string GetWorldBuildingText(bool shareStatus = false)
        {
            return string.Format(WORLD_BUILDING_TEXT, shareStatus ? "your crew's suit oxigen delivery systems" : "your suit's oxygen delivery system");
        }

        public override string GetDisplayInfo(int initialPrice = -1, int maxLevels = -1, int[] incrementalPrices = null)
        {
            System.Func<int, float> infoFunction = level => UpgradeBus.instance.cfg.SPRINT_TIME_INCREASE_UNLOCK.Value + level * UpgradeBus.instance.cfg.SPRINT_TIME_INCREMENT.Value;
            string infoFormat = AssetBundleHandler.GetInfoFromJSON(UPGRADE_NAME);
            return Tools.GenerateInfoForUpgrade(infoFormat, initialPrice, incrementalPrices, infoFunction);
        }
    }
}
