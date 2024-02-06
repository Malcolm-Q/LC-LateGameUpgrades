using GameNetcodeStuff;
using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc;
using MoreShipUpgrades.Misc.Upgrades;
using System;
using System.Numerics;
using UnityEngine;

namespace MoreShipUpgrades.UpgradeComponents.TierUpgrades
{
    internal class Stimpack : GameAttributeTierUpgrade
    {
        public static string UPGRADE_NAME = "Stimpack";
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

        internal override void Start()
        {
            upgradeName = UPGRADE_NAME;
            logger = new LGULogger(UPGRADE_NAME);
            base.Start();
            changingAttribute = GameAttribute.PLAYER_HEALTH;
            initialValue = UpgradeBus.instance.cfg.PLAYER_HEALTH_ADDITIONAL_HEALTH_UNLOCK;
            incrementalValue = UpgradeBus.instance.cfg.PLAYER_HEALTH_ADDITIONAL_HEALTH_INCREMENT;
        }

        public override void Increment()
        {
            base.Increment();
            UpgradeBus.instance.playerHealthLevel++;
            PlayerControllerB player = UpgradeBus.instance.GetLocalPlayer();
            LGUStore.instance.PlayerHealthUpdateLevelServerRpc(player.playerSteamId, UpgradeBus.instance.playerHealthLevel);
        }

        public override void Load()
        {
            LoadUpgradeAttribute(ref UpgradeBus.instance.playerHealth, UpgradeBus.instance.playerHealthLevel);
            base.Load();
        }

        public override void Unwind()
        {
            UnloadUpgradeAttribute(ref UpgradeBus.instance.playerHealth, ref UpgradeBus.instance.playerHealthLevel);
            base.Unwind();
            PlayerControllerB player = UpgradeBus.instance.GetLocalPlayer();
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
    }
}
