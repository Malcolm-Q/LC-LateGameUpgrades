using GameNetcodeStuff;
using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc;
using System;
using System.Numerics;
using UnityEngine;

namespace MoreShipUpgrades.UpgradeComponents.TierUpgrades
{
    internal class playerHealthScript : BaseUpgrade
    {
        public static string UPGRADE_NAME = "Stimpack";
        private static bool active;
        private int previousLevel;
        private static LGULogger logger;
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
            logger = new LGULogger(UPGRADE_NAME);
        }

        public override void Increment()
        {
            PlayerControllerB player = UpgradeBus.instance.GetLocalPlayer();
            player.health += UpgradeBus.instance.cfg.PLAYER_HEALTH_ADDITIONAL_HEALTH_INCREMENT;
            logger.LogDebug($"Adding {UpgradeBus.instance.cfg.PLAYER_HEALTH_ADDITIONAL_HEALTH_INCREMENT} to the player's health...");
            UpgradeBus.instance.playerHealthLevel++;
            previousLevel++;
            LGUStore.instance.PlayerHealthUpdateLevelServerRpc(player.playerSteamId, UpgradeBus.instance.playerHealthLevel);
        }

        public override void load()
        {
            PlayerControllerB player = UpgradeBus.instance.GetLocalPlayer();
            if (!active)
            {
                logger.LogDebug($"Adding {UpgradeBus.instance.cfg.PLAYER_HEALTH_ADDITIONAL_HEALTH_UNLOCK} to the player's health on unlock...");
                player.health += UpgradeBus.instance.cfg.PLAYER_HEALTH_ADDITIONAL_HEALTH_UNLOCK;
            }
            base.load();

            UpgradeBus.instance.playerHealth = true;
            active = true;

            int amountToIncrement = 0;
            for (int i = 1; i < UpgradeBus.instance.playerHealthLevel + 1; i++)
            {
                if (i <= previousLevel) continue;
                logger.LogDebug($"Adding {UpgradeBus.instance.cfg.PLAYER_HEALTH_ADDITIONAL_HEALTH_INCREMENT} to the player's health on increment...");
                amountToIncrement += UpgradeBus.instance.cfg.PLAYER_HEALTH_ADDITIONAL_HEALTH_INCREMENT;
            }

            player.health += amountToIncrement;
            previousLevel = UpgradeBus.instance.playerHealthLevel;
            LGUStore.instance.PlayerHealthUpdateLevelServerRpc(player.playerSteamId, UpgradeBus.instance.playerHealthLevel);
        }

        public override void Register()
        {
            base.Register();
        }

        public override void Unwind()
        {
            PlayerControllerB player = UpgradeBus.instance.GetLocalPlayer();
            if (active) ResetStimpackBuff(ref player);
            base.Unwind();

            UpgradeBus.instance.playerHealthLevel = 0;
            UpgradeBus.instance.playerHealth = false;
            previousLevel = 0;
            active = false;
            LGUStore.instance.PlayerHealthUpdateLevelServerRpc(player.playerSteamId, -1);
        }
        public static int CheckForAdditionalHealth(int health)
        {
            PlayerControllerB player = UpgradeBus.instance.GetLocalPlayer();
            if (!UpgradeBus.instance.playerHealthLevels.ContainsKey(player.playerSteamId)) return health;
            int currentLevel = UpgradeBus.instance.playerHealthLevels[player.playerSteamId];

            return health + UpgradeBus.instance.cfg.PLAYER_HEALTH_ADDITIONAL_HEALTH_UNLOCK + currentLevel * UpgradeBus.instance.cfg.PLAYER_HEALTH_ADDITIONAL_HEALTH_INCREMENT;
        }
        /// <summary>
        /// Returns the maximum health possible for the player with given steam identifier
        /// 
        /// Precondition: playerHealthLevels contains the steam identifier as a key
        /// </summary>
        /// <param name="health">Health before applying the Stimpack upgrade</param>
        /// <param name="steamId">Identifier of the client through steam</param>
        /// <returns>Health of the player after applying the Stimpack effects</returns>
        public static int GetHealthFromPlayer(int health, ulong steamId)
        {
            int currentLevel = UpgradeBus.instance.playerHealthLevels[steamId];
            return health + UpgradeBus.instance.cfg.PLAYER_HEALTH_ADDITIONAL_HEALTH_UNLOCK + currentLevel * UpgradeBus.instance.cfg.PLAYER_HEALTH_ADDITIONAL_HEALTH_INCREMENT;
        }
        public static void ResetStimpackBuff(ref PlayerControllerB player)
        {
            int healthRemoval = UpgradeBus.instance.cfg.PLAYER_HEALTH_ADDITIONAL_HEALTH_UNLOCK;
            for (int i = 0; i < UpgradeBus.instance.playerHealthLevel; i++)
            {
                healthRemoval += UpgradeBus.instance.cfg.PLAYER_HEALTH_ADDITIONAL_HEALTH_INCREMENT;
            }
            logger.LogDebug($"Removing {player.playerUsername}'s health boost ({player.health}) with a boost of {healthRemoval}");
            player.health -= healthRemoval;
            logger.LogDebug($"Upgrade reset on {player.playerUsername}");
            active = false;
        }
    }
}
