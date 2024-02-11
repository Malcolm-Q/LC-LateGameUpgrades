using GameNetcodeStuff;
using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc;
using MoreShipUpgrades.Misc.Upgrades;
using MoreShipUpgrades.UpgradeComponents.Interfaces;
using System;
using System.Numerics;
using UnityEngine;

namespace MoreShipUpgrades.UpgradeComponents.TierUpgrades.AttributeUpgrades
{
    internal class Stimpack : GameAttributeTierUpgrade, IUpgradeWorldBuilding, IPlayerSync
    {
        public const string UPGRADE_NAME = "Stimpack";
        internal const string WORLD_BUILDING_TEXT = "\n\nAn experimental Company-offered 'health treatment' program advertised only on old, peeling Ship posters," +
            " which are themselves only present in about 40% of all Company-issued Ships. Some Ships even have multiple. Nothing is known from the outside about how it works," +
            " and in order to be eligible for the program, {0} must sign an NDA.\n\n";
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
            initialValue = UpgradeBus.instance.cfg.PLAYER_HEALTH_ADDITIONAL_HEALTH_UNLOCK.Value;
            incrementalValue = UpgradeBus.instance.cfg.PLAYER_HEALTH_ADDITIONAL_HEALTH_INCREMENT.Value;
        }

        public override void Increment()
        {
            base.Increment();
            PlayerControllerB player = UpgradeBus.instance.GetLocalPlayer();
            LGUStore.instance.PlayerHealthUpdateLevelServerRpc(player.playerSteamId, GetUpgradeLevel(UPGRADE_NAME));
        }

        public override void Unwind()
        {
            base.Unwind();
            PlayerControllerB player = UpgradeBus.instance.GetLocalPlayer();
            LGUStore.instance.PlayerHealthUpdateLevelServerRpc(player.playerSteamId, -1);
        }
        public static int CheckForAdditionalHealth(int health)
        {
            PlayerControllerB player = UpgradeBus.instance.GetLocalPlayer();
            if (!UpgradeBus.instance.playerHealthLevels.ContainsKey(player.playerSteamId)) return health;
            int currentLevel = UpgradeBus.instance.playerHealthLevels[player.playerSteamId];

            return health + UpgradeBus.instance.cfg.PLAYER_HEALTH_ADDITIONAL_HEALTH_UNLOCK.Value + currentLevel * UpgradeBus.instance.cfg.PLAYER_HEALTH_ADDITIONAL_HEALTH_INCREMENT.Value;
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
            return health + UpgradeBus.instance.cfg.PLAYER_HEALTH_ADDITIONAL_HEALTH_UNLOCK.Value + currentLevel * UpgradeBus.instance.cfg.PLAYER_HEALTH_ADDITIONAL_HEALTH_INCREMENT.Value;
        }
        public string GetWorldBuildingText(bool shareStatus = false)
        {
            return string.Format(WORLD_BUILDING_TEXT, shareStatus ? "your crew" : "you");
        }

        public override string GetDisplayInfo(int initialPrice = -1, int maxLevels = -1, int[] incrementalPrices = null)
        {
            Func<int, float> infoFunction = level => UpgradeBus.instance.cfg.PLAYER_HEALTH_ADDITIONAL_HEALTH_UNLOCK.Value + level * UpgradeBus.instance.cfg.PLAYER_HEALTH_ADDITIONAL_HEALTH_INCREMENT.Value;
            string infoFormat = AssetBundleHandler.GetInfoFromJSON(UPGRADE_NAME);
            return Tools.GenerateInfoForUpgrade(infoFormat, initialPrice, incrementalPrices, infoFunction);
        }
    }
}
