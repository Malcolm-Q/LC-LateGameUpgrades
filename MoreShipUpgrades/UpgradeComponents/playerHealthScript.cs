using GameNetcodeStuff;
using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc;
using System;
using System.Numerics;
using UnityEngine;

namespace MoreShipUpgrades.UpgradeComponents
{
    internal class playerHealthScript : BaseUpgrade
    {
        public static string UPGRADE_NAME = "Stimpack";

        private static int DEFAULT_HEALTH = 100;

        // Configuration
        public static string ENABLED_SECTION = $"Enable {UPGRADE_NAME} Upgrade";
        public static bool ENABLED_DEFAULT = true;
        public static string ENABLED_DESCRIPTION = "Increases player's health.";

        public static string PRICE_SECTION = $"{UPGRADE_NAME} Price";
        public static int PRICE_DEFAULT = 600;

        public static string PRICES_DEFAULT = "300, 450, 600";

        public static string ADDITIONAL_HEALTH_UNLOCK_SECTION = "Initial health boost";
        public static int ADDITIONAL_HEALTH_UNLOCK_DEFAULT = 20;
        public static string ADDITIONAL_HEALTH_UNLOCK_DESCRIPTION = "Amount of health gained when unlocking the upgrade";

        public static string ADDITIONAL_HEALTH_INCREMENT_SECTION = "Additional health boost";
        public static int ADDITIONAL_HEALTH_INCREMENT_DEFAULT = 20;
        public static string ADDITIONAL_HEALTH_INCREMENT_DESCRIPTION = $"Every time {UPGRADE_NAME} is upgraded this value will be added to the value above.";

        void Start()
        {
            upgradeName = UPGRADE_NAME;
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
            base.load();

            UpgradeBus.instance.playerHealth = true;
            LGUStore.instance.UpdatePlayerNewHealthsServerRpc(GameNetworkManager.Instance.localPlayerController.playerSteamId, DEFAULT_HEALTH + UpgradeBus.instance.cfg.PLAYER_HEALTH_ADDITIONAL_HEALTH_UNLOCK + (UpgradeBus.instance.playerHealthLevel * UpgradeBus.instance.cfg.PLAYER_HEALTH_ADDITIONAL_HEALTH_INCREMENT));
        }

        public override void Register()
        {
            base.Register();
        }

        public override void Unwind()
        {
            base.Unwind();

            UpgradeBus.instance.playerHealthLevel = 0;
            UpgradeBus.instance.playerHealth = false;
            LGUStore.instance.UpdatePlayerNewHealthsServerRpc(GameNetworkManager.Instance.localPlayerController.playerSteamId, DEFAULT_HEALTH);
        }

        public static void CheckAdditionalHealth(StartOfRound __instance)
        {
            PlayerControllerB[] players = __instance.allPlayerScripts;
            foreach (PlayerControllerB player in players)
            {
                UpdateMaxHealth(player);
            }
        }
        public static int CheckForAdditionalHealth()
        {
            if (UpgradeBus.instance.playerHPs.ContainsKey(GameNetworkManager.Instance.localPlayerController.playerSteamId))
               return UpgradeBus.instance.playerHPs[GameNetworkManager.Instance.localPlayerController.playerSteamId];
            return DEFAULT_HEALTH;
        }

        public static void UpdateMaxHealth(PlayerControllerB player)
        {
            if (UpgradeBus.instance.playerHPs.ContainsKey(player.playerSteamId))
                player.health = UpgradeBus.instance.playerHPs[player.playerSteamId];
        }
    }
}
