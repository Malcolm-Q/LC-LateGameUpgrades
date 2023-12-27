using GameNetcodeStuff;
using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc;
using UnityEngine;

namespace MoreShipUpgrades.UpgradeComponents
{
    internal class biggerLungScript : BaseUpgrade
    {
        public static string UPGRADE_NAME = "Bigger Lungs";
        public static string PRICES_DEFAULT = "350,450,550";
        private static float DEFAULT_SPRINT_TIME = 11f;

        void Start()
        {
            upgradeName = UPGRADE_NAME;
            DontDestroyOnLoad(gameObject);
            Register();
        }

        public override void Increment()
        {
            UpgradeBus.instance.lungLevel++;
            GameNetworkManager.Instance.localPlayerController.sprintTime += UpgradeBus.instance.cfg.SPRINT_TIME_INCREMENT;  //17
        }

        public override void load()
        {
            base.load();
            UpgradeBus.instance.biggerLungs = true;

            GameNetworkManager.Instance.localPlayerController.sprintTime = UpgradeBus.instance.cfg.SPRINT_TIME_INCREASE; //17

            float amountToIncrement = 0;
            for(int i = 0; i < UpgradeBus.instance.lungLevel; i++)
            {
                amountToIncrement += UpgradeBus.instance.cfg.SPRINT_TIME_INCREMENT;
            }

            GameNetworkManager.Instance.localPlayerController.sprintTime += amountToIncrement;
        }

        public override void Unwind()
        {
            base.Unwind();

            UpgradeBus.instance.biggerLungs = false;
            GameNetworkManager.Instance.localPlayerController.sprintTime = DEFAULT_SPRINT_TIME;
        }

        public override void Register()
        {
            base.Register();
        }
    }
}
