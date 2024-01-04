using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc;
using UnityEngine;

namespace MoreShipUpgrades.UpgradeComponents
{
    internal class runningShoeScript : BaseUpgrade
    {
        public static string UPGRADE_NAME = "Running Shoes";
        public static string PRICES_DEFAULT = "500,750,1000";
        void Start()
        {
            upgradeName = UPGRADE_NAME;
            DontDestroyOnLoad(gameObject);
            Register();
        }

        public override void Increment()
        {
            GameNetworkManager.Instance.localPlayerController.movementSpeed += UpgradeBus.instance.cfg.MOVEMENT_INCREMENT;
            UpgradeBus.instance.runningLevel++;
        }

        public override void Unwind()
        {
            base.Unwind();

            GameNetworkManager.Instance.localPlayerController.movementSpeed -= (UpgradeBus.instance.cfg.MOVEMENT_SPEED_UNLOCK + (UpgradeBus.instance.runningLevel * UpgradeBus.instance.cfg.MOVEMENT_INCREMENT));
            UpgradeBus.instance.runningShoes = false;
            UpgradeBus.instance.runningLevel = 0;
        }
        public override void Register()
        {
            base.Register();
        }

        public override void load()
        {
            base.load();

            UpgradeBus.instance.runningShoes = true;
            GameNetworkManager.Instance.localPlayerController.movementSpeed += UpgradeBus.instance.cfg.MOVEMENT_SPEED_UNLOCK;

            float amountToIncrement = 0;
            for(int i = 0; i < UpgradeBus.instance.runningLevel; i++)
            {
                amountToIncrement += UpgradeBus.instance.cfg.MOVEMENT_INCREMENT;
            }

            GameNetworkManager.Instance.localPlayerController.movementSpeed += amountToIncrement;
        }

        public static float ApplyPossibleReducedNoiseRange(float defaultValue)
        {
            if (!(UpgradeBus.instance.runningShoes && UpgradeBus.instance.runningLevel == UpgradeBus.instance.cfg.RUNNING_SHOES_UPGRADE_PRICES.Split(',').Length)) return defaultValue;
            return Mathf.Clamp(defaultValue - UpgradeBus.instance.cfg.NOISE_REDUCTION, 0f, defaultValue);
        }
    }
}
