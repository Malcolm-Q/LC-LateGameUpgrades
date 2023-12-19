using GameNetcodeStuff;
using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc;
using UnityEngine;

namespace MoreShipUpgrades.UpgradeComponents
{
    internal class biggerLungScript : BaseUpgrade
    {

        void Start()
        {
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
            GameNetworkManager.Instance.localPlayerController.sprintTime = UpgradeBus.instance.cfg.SPRINT_TIME_INCREASE; //17
            UpgradeBus.instance.biggerLungs = true;
            HUDManager.Instance.chatText.text += "\n<color=#FF0000>Bigger Lungs is active!</color>";

            float amountToIncrement = 0;
            for(int i = 0; i < UpgradeBus.instance.lungLevel; i++)
            {
                amountToIncrement += UpgradeBus.instance.cfg.SPRINT_TIME_INCREMENT;
            }

            GameNetworkManager.Instance.localPlayerController.sprintTime += amountToIncrement;
        }

        public override void Unwind()
        {
            UpgradeBus.instance.biggerLungs = false;
            GameNetworkManager.Instance.localPlayerController.sprintTime = 11f;
            HUDManager.Instance.chatText.text += "\n<color=#FF0000>Bigger Lungs has been disabled.</color>";
        }

        public override void Register()
        {
            if(!UpgradeBus.instance.UpgradeObjects.ContainsKey("Bigger Lungs")) { UpgradeBus.instance.UpgradeObjects.Add("Bigger Lungs", gameObject); }
        }
    }
}
