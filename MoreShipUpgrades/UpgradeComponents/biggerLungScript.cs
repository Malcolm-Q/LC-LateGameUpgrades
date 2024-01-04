using GameNetcodeStuff;
using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc;
using UnityEngine;

namespace MoreShipUpgrades.UpgradeComponents
{
    internal class biggerLungScript : BaseUpgrade
    {
        PlayerControllerB localPlayer;
        private static LGULogger logger = new LGULogger(UPGRADE_NAME);
        public static string UPGRADE_NAME = "Bigger Lungs";
        public static string PRICES_DEFAULT = "350,450,550";

        void Start()
        {
            upgradeName = UPGRADE_NAME;
            DontDestroyOnLoad(gameObject);
            Register();
        }

        public override void Increment()
        {
            UpgradeBus.instance.lungLevel++;
            PlayerControllerB player = GetLocalPlayer();
            player.sprintTime += UpgradeBus.instance.cfg.SPRINT_TIME_INCREMENT;
        }

        public override void load()
        {
            PlayerControllerB player = GetLocalPlayer();
            player.sprintTime += UpgradeBus.instance.cfg.SPRINT_TIME_INCREASE_UNLOCK;
            UpgradeBus.instance.biggerLungs = true;
            base.load();

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
            player.sprintTime -= (UpgradeBus.instance.cfg.SPRINT_TIME_INCREASE_UNLOCK + (UpgradeBus.instance.lungLevel * UpgradeBus.instance.cfg.SPRINT_TIME_INCREMENT));
            base.Unwind();

            UpgradeBus.instance.biggerLungs = false;
        }

        public override void Register()
        {
            base.Register();
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
