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
        private bool active = false;
        void Start()
        {
            upgradeName = UPGRADE_NAME;
            logger = new LGULogger(UPGRADE_NAME);
            DontDestroyOnLoad(gameObject);
            Register();
        }

        public override void Increment()
        {
            GameNetworkManager.Instance.localPlayerController.movementSpeed += UpgradeBus.instance.cfg.MOVEMENT_INCREMENT;
            UpgradeBus.instance.runningLevel++;
            currentLevel++;
        }

        public override void Unwind()
        {
            base.Unwind();

            GameNetworkManager.Instance.localPlayerController.movementSpeed -= (UpgradeBus.instance.cfg.MOVEMENT_SPEED_UNLOCK + (UpgradeBus.instance.runningLevel * UpgradeBus.instance.cfg.MOVEMENT_INCREMENT));
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
            if (!active) GameNetworkManager.Instance.localPlayerController.movementSpeed += UpgradeBus.instance.cfg.MOVEMENT_SPEED_UNLOCK;

            float amountToIncrement = 0;
            for(int i = 1; i < UpgradeBus.instance.runningLevel+1; i++)
            {
                if (i <= currentLevel) continue;

                logger.LogDebug($"Adding {UpgradeBus.instance.cfg.MOVEMENT_INCREMENT} to the player's movement speed...");
                amountToIncrement += UpgradeBus.instance.cfg.MOVEMENT_INCREMENT;
            }

            GameNetworkManager.Instance.localPlayerController.movementSpeed += amountToIncrement;
            active = true;
            currentLevel = UpgradeBus.instance.runningLevel;
        }

        public static float ApplyPossibleReducedNoiseRange(float defaultValue)
        {
            if (!(UpgradeBus.instance.runningShoes && UpgradeBus.instance.runningLevel == UpgradeBus.instance.cfg.RUNNING_SHOES_UPGRADE_PRICES.Split(',').Length)) return defaultValue;
            return Mathf.Clamp(defaultValue - UpgradeBus.instance.cfg.NOISE_REDUCTION, 0f, defaultValue);
        }
    }
}
