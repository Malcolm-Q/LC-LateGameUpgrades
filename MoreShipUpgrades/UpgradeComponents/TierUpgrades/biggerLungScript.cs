using GameNetcodeStuff;
using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc;
using MoreShipUpgrades.UpgradeComponents.Interfaces;
using UnityEngine;

namespace MoreShipUpgrades.UpgradeComponents.TierUpgrades
{
    internal class biggerLungScript : BaseUpgrade, IUpgradeWorldBuilding, ITierUpgradeDisplayInfo
    {
        private static LGULogger logger;
        public const string UPGRADE_NAME = "Bigger Lungs";
        public static string PRICES_DEFAULT = "350,450,550";
        private int currentLevel = 0; // For "Load LGU" issues
        private static bool active = false;
        internal const string WORLD_BUILDING_TEXT = "\n\nService package for {0}." +
            " Opting into every maintenance procedure will arrange for your suit's pipes to be cleaned and repaired, filters re-issued," +
            " and DRM removed from the integrated air conditioning system.\n\n";

        void Start()
        {
            upgradeName = UPGRADE_NAME;
            logger = new LGULogger(UPGRADE_NAME);
            DontDestroyOnLoad(gameObject);
            Register();
        }

        public override void Increment()
        {
            PlayerControllerB player = UpgradeBus.instance.GetLocalPlayer();
            player.sprintTime += UpgradeBus.instance.cfg.SPRINT_TIME_INCREMENT;
            logger.LogDebug($"Adding {UpgradeBus.instance.cfg.SPRINT_TIME_INCREMENT} to the player's sprint time...");
            UpgradeBus.instance.lungLevel++;
            currentLevel++;
        }

        public override void load()
        {
            PlayerControllerB player = UpgradeBus.instance.GetLocalPlayer();
            if (!active)
            {
                logger.LogDebug($"Adding {UpgradeBus.instance.cfg.SPRINT_TIME_INCREASE_UNLOCK} to the player's sprint time...");
                player.sprintTime += UpgradeBus.instance.cfg.SPRINT_TIME_INCREASE_UNLOCK;
            }
            UpgradeBus.instance.biggerLungs = true;
            active = true;
            base.load();

            float amountToIncrement = 0;
            for (int i = 1; i < UpgradeBus.instance.lungLevel + 1; i++)
            {
                if (i <= currentLevel) continue;
                logger.LogDebug($"Adding {UpgradeBus.instance.cfg.SPRINT_TIME_INCREMENT} to the player's sprint time...");
                amountToIncrement += UpgradeBus.instance.cfg.SPRINT_TIME_INCREMENT;
            }

            player.sprintTime += amountToIncrement;
            currentLevel = UpgradeBus.instance.lungLevel;
        }

        public override void Unwind()
        {
            PlayerControllerB player = UpgradeBus.instance.GetLocalPlayer();
            if (active) ResetBiggerLungsBuff(ref player);
            base.Unwind();

            UpgradeBus.instance.biggerLungs = false;
            UpgradeBus.instance.lungLevel = 0;
            active = false;
            currentLevel = 0;
        }
        public override void Register()
        {
            base.Register();
        }
        public static void ResetBiggerLungsBuff(ref PlayerControllerB player)
        {
            float sprintTimeRemoval = UpgradeBus.instance.cfg.SPRINT_TIME_INCREASE_UNLOCK;
            for (int i = 0; i < UpgradeBus.instance.lungLevel; i++)
            {
                sprintTimeRemoval += UpgradeBus.instance.cfg.SPRINT_TIME_INCREMENT;
            }
            logger.LogDebug($"Removing {player.playerUsername}'s sprint time boost ({player.sprintTime}) with a boost of {sprintTimeRemoval}");
            player.sprintTime -= sprintTimeRemoval;
            logger.LogDebug($"Upgrade reset on {player.playerUsername}");
            active = false;
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

        public string GetDisplayInfo(int initialPrice = -1, int maxLevels = -1, int[] incrementalPrices = null)
        {
            System.Func<int, float> infoFunction = level => UpgradeBus.instance.cfg.SPRINT_TIME_INCREASE_UNLOCK + (level * UpgradeBus.instance.cfg.SPRINT_TIME_INCREMENT);
            string infoFormat = AssetBundleHandler.GetInfoFromJSON(UPGRADE_NAME);
            return Tools.GenerateInfoForUpgrade(infoFormat, initialPrice, incrementalPrices, infoFunction);
        }
    }
}
