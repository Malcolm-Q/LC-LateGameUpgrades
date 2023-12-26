using GameNetcodeStuff;
using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc;
using UnityEngine;

namespace MoreShipUpgrades.UpgradeComponents
{
    internal class biggerLungScript : BaseUpgrade
    {
        PlayerControllerB localPlayer;
        private static LGULogger logger = new LGULogger("Bigger Lungs");
        private static float DEFAULT_SPRINT_TIME = 11f;

        void Start()
        {
            DontDestroyOnLoad(gameObject);
            Register();
        }

        public override void Increment()
        {
            UpgradeBus.instance.lungLevel++;
            PlayerControllerB player = GetLocalPlayer();
            player.sprintTime += UpgradeBus.instance.cfg.SPRINT_TIME_INCREMENT;  //17
        }

        public override void load()
        {
            PlayerControllerB player = GetLocalPlayer();
            player.sprintTime = UpgradeBus.instance.cfg.SPRINT_TIME_INCREASE; //17
            UpgradeBus.instance.biggerLungs = true;
            HUDManager.Instance.chatText.text += "\n<color=#FF0000>Bigger Lungs is active!</color>";

            float amountToIncrement = 0;
            for(int i = 0; i < UpgradeBus.instance.lungLevel; i++)
            {
                amountToIncrement += UpgradeBus.instance.cfg.SPRINT_TIME_INCREMENT;
            }

            localPlayer.sprintTime += amountToIncrement;
        }

        public override void Unwind()
        {
            PlayerControllerB player = GetLocalPlayer();
            player.sprintTime = DEFAULT_SPRINT_TIME;
            UpgradeBus.instance.biggerLungs = false;
            HUDManager.Instance.chatText.text += "\n<color=#FF0000>Bigger Lungs has been disabled.</color>";
        }

        public override void Register()
        {
            if(!UpgradeBus.instance.UpgradeObjects.ContainsKey("Bigger Lungs")) { UpgradeBus.instance.UpgradeObjects.Add("Bigger Lungs", gameObject); }
        }

        private PlayerControllerB GetLocalPlayer()
        {
            if (localPlayer == null) localPlayer = GameNetworkManager.Instance.localPlayerController;

            return localPlayer;
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
