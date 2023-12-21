using GameNetcodeStuff;
using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc;
using System;
using System.Collections.Generic;
using System.Text;

namespace MoreShipUpgrades.UpgradeComponents
{
    internal class playerHealthScript : BaseUpgrade
    {
        public static string UPGRADE_NAME = "Stimpack";

        private static int DEFAULT_HEALTH = 100;

        // Configuration
        public static string ENABLED_SECTION = string.Format("Enable {0} Upgrade", UPGRADE_NAME);
        public static bool ENABLED_DEFAULT = true;
        public static string ENABLED_DESCRIPTION = "Increases player's health.";

        public static string PRICE_SECTION = string.Format("{0} Price", UPGRADE_NAME);
        public static int PRICE_DEFAULT = 600;

        public static string INDIVIDUAL_SECTION = "Individual Purchase";
        public static bool INDIVIDUAL_DEFAULT = false;
        public static string INDIVIDUAL_DESCRIPTION = "If true: upgrade will apply only to the client that purchased it.";

        // Chat Messages
        private static string LOAD_COLOUR = "#FF0000";
        private static string LOAD_MESSAGE = string.Format("\n<color={0}>{1} is active!</color>", LOAD_COLOUR, UPGRADE_NAME);

        private static string UNLOAD_COLOUR = LOAD_COLOUR;
        private static string UNLOAD_MESSAGE = string.Format("\n<color={0}>{1} has been disabled</color>", UNLOAD_COLOUR, UPGRADE_NAME);

        public static string UPGRADE_PRICES_SECTION = "Price of each additional upgrade";
        public static string UPGRADE_PRICES_DEFAULT = "300, 450, 600";

        public static string ADDITIONAL_HEALTH_UNLOCK_SECTION = "Initial health boost";
        public static int ADDITIONAL_HEALTH_UNLOCK_DEFAULT = 20;
        public static string ADDITIONAL_HEALTH_UNLOCK_DESCRIPTION = "Amount of health gained when unlocking the upgrade";

        public static string ADDITIONAL_HEALTH_INCREMENT_SECTION = "Additional health boost";
        public static int ADDITIONAL_HEALTH_INCREMENT_DEFAULT = 20;
        public static string ADDITIONAL_HEALTH_INCREMENT_DESCRIPTION = string.Format("Every time {0} is upgraded this value will be added to the value above.", UPGRADE_NAME);

        void Start()
        {
            DontDestroyOnLoad(gameObject);
            UpgradeBus.instance.UpgradeObjects.Add(UPGRADE_NAME, gameObject);
        }

        public override void Increment()
        {
            UpgradeBus.instance.playerHealthLevel++;
            LGUStore.instance.UpdatePlayerNewHealthsServerRpc(GameNetworkManager.Instance.localPlayerController.playerSteamId, DEFAULT_HEALTH + UpgradeBus.instance.cfg.PLAYER_HEALTH_ADDITIONAL_HEALTH_UNLOCK + (UpgradeBus.instance.playerHealthLevel * UpgradeBus.instance.cfg.PLAYER_HEALTH_ADDITIONAL_HEALTH_INCREMENT));
        }

        public override void load()
        {
            UpgradeBus.instance.playerHealth = true;
            HUDManager.Instance.chatText.text += LOAD_MESSAGE;
            LGUStore.instance.UpdatePlayerNewHealthsServerRpc(GameNetworkManager.Instance.localPlayerController.playerSteamId, DEFAULT_HEALTH + UpgradeBus.instance.cfg.PLAYER_HEALTH_ADDITIONAL_HEALTH_UNLOCK + (UpgradeBus.instance.playerHealthLevel * UpgradeBus.instance.cfg.PLAYER_HEALTH_ADDITIONAL_HEALTH_INCREMENT));
        }

        public override void Register()
        {
            if (!UpgradeBus.instance.UpgradeObjects.ContainsKey(UPGRADE_NAME)) { UpgradeBus.instance.UpgradeObjects.Add(UPGRADE_NAME, gameObject); }
        }

        public override void Unwind()
        {
            UpgradeBus.instance.playerHealthLevel = 0;
            UpgradeBus.instance.playerHealth = false;
            HUDManager.Instance.chatText.text += UNLOAD_MESSAGE;
            LGUStore.instance.UpdatePlayerNewHealthsServerRpc(GameNetworkManager.Instance.localPlayerController.playerSteamId, 0);
        }

        public static void CheckAdditionalHealth(StartOfRound __instance)
        {
            PlayerControllerB[] players = __instance.allPlayerScripts;
            foreach (PlayerControllerB player in players)
            {
                CheckForAdditionalHealth(player);
            }
        }

        public static void CheckForAdditionalHealth(PlayerControllerB player)
        {
            if (UpgradeBus.instance.playerHPs.ContainsKey(player.playerSteamId))
                player.health = UpgradeBus.instance.playerHPs[player.playerSteamId];
            if (player.playerSteamId == GameNetworkManager.Instance.localPlayerController.playerSteamId)
                HUDManager.Instance.UpdateHealthUI(player.health, hurtPlayer: false);
        }
    }
}
