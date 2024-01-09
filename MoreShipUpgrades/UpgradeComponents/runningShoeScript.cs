using GameNetcodeStuff;
using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc;
using UnityEngine;

namespace MoreShipUpgrades.UpgradeComponents
{
    internal class runningShoeScript : BaseUpgrade
    {
        public static string UPGRADE_NAME = "Running Shoes";
        private static LGULogger logger;
        public static string PRICES_DEFAULT = "500,750,1000";
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
            player.movementSpeed += UpgradeBus.instance.cfg.MOVEMENT_INCREMENT;
            logger.LogDebug($"Adding {UpgradeBus.instance.cfg.MOVEMENT_INCREMENT} to the player's movement speed...");
            UpgradeBus.instance.runningLevel++;
            currentLevel++;
        }

        public override void Unwind()
        {
            base.Unwind();
            PlayerControllerB player = UpgradeBus.instance.GetLocalPlayer();
            if (UpgradeBus.instance.runningShoes) ResetRunningShoesBuff(ref player);
            UpgradeBus.instance.runningShoes = false;
            UpgradeBus.instance.runningLevel = 0;
            active = false;
            currentLevel = 0;
        }
        public override void Register()
        {
            base.Register();
        }

        public override void load()
        {
            base.load();

            UpgradeBus.instance.runningShoes = true;
            PlayerControllerB player = UpgradeBus.instance.GetLocalPlayer();
            if (!active)
            {
                logger.LogDebug($"Adding {UpgradeBus.instance.cfg.MOVEMENT_SPEED_UNLOCK} to the player's movement speed...");
                player.movementSpeed += UpgradeBus.instance.cfg.MOVEMENT_SPEED_UNLOCK;
            }

            float amountToIncrement = 0;
            for(int i = 1; i < UpgradeBus.instance.runningLevel+1; i++)
            {
                if (i <= currentLevel) continue;

                logger.LogDebug($"Adding {UpgradeBus.instance.cfg.MOVEMENT_INCREMENT} to the player's movement speed...");
                amountToIncrement += UpgradeBus.instance.cfg.MOVEMENT_INCREMENT;
            }
            logger.LogDebug($"Adding player's movement speed ({player.movementSpeed}) with {amountToIncrement}");
            player.movementSpeed += amountToIncrement;
            active = true;
            currentLevel = UpgradeBus.instance.runningLevel;
        }
        public static void ResetRunningShoesBuff(ref PlayerControllerB player)
        {
            float movementSpeedRemoval = UpgradeBus.instance.cfg.MOVEMENT_SPEED_UNLOCK;
            for (int i = 0; i < UpgradeBus.instance.runningLevel; i++)
            {
                movementSpeedRemoval += UpgradeBus.instance.cfg.MOVEMENT_INCREMENT;
            }
            logger.LogDebug($"Removing {player.playerUsername}'s movement speed boost ({player.movementSpeed}) with a boost of {movementSpeedRemoval}");
            player.movementSpeed -= movementSpeedRemoval;
            logger.LogDebug($"Upgrade reset on {player.playerUsername}");
            active = false;
        }
        public static float ApplyPossibleReducedNoiseRange(float defaultValue)
        {
            if (!(UpgradeBus.instance.runningShoes && UpgradeBus.instance.runningLevel == UpgradeBus.instance.cfg.RUNNING_SHOES_UPGRADE_PRICES.Split(',').Length)) return defaultValue;
            return Mathf.Clamp(defaultValue - UpgradeBus.instance.cfg.NOISE_REDUCTION, 0f, defaultValue);
        }
    }
}
