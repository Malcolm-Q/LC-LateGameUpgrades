using GameNetcodeStuff;
using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc;
using UnityEngine;

namespace MoreShipUpgrades.UpgradeComponents
{
    internal class biggerLungScript : BaseUpgrade
    {
        private static LGULogger logger;
        public static string UPGRADE_NAME = "Bigger Lungs";
        public static string PRICES_DEFAULT = "350,450,550";
        private int currentLevel = 0; // For "Load LGU" issues
        private static bool active = false;

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
            for(int i = 1; i < UpgradeBus.instance.lungLevel+1; i++)
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
            if (!UpgradeBus.instance.biggerLungs || UpgradeBus.instance.lungLevel < 0) return regenValue;
            return regenValue*UpgradeBus.instance.cfg.BIGGER_LUNGS_STAMINA_REGEN_INCREASE;
        }

        public static float ApplyPossibleReducedJumpStaminaCost(float jumpCost)
        {
            if (!UpgradeBus.instance.biggerLungs || UpgradeBus.instance.lungLevel < 1) return jumpCost;
            return jumpCost * UpgradeBus.instance.cfg.BIGGER_LUNGS_JUMP_STAMINA_COST_DECREASE;
        }
    }
}
