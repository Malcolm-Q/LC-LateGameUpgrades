using GameNetcodeStuff;
using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc;
using MoreShipUpgrades.Misc.Upgrades;
using MoreShipUpgrades.UpgradeComponents.Interfaces;
using System;

namespace MoreShipUpgrades.UpgradeComponents.TierUpgrades.AttributeUpgrades
{
    internal class Stimpack : GameAttributeTierUpgrade, IUpgradeWorldBuilding, IPlayerSync
    {
        public const string UPGRADE_NAME = "Stimpack";
        internal const string WORLD_BUILDING_TEXT = "\n\nAn experimental Company-offered 'health treatment' program advertised only on old, peeling Ship posters," +
            " which are themselves only present in about 40% of all Company-issued Ships. Some Ships even have multiple. Nothing is known from the outside about how it works," +
            " and in order to be eligible for the program, {0} must sign an NDA.\n\n";
        // Configuration
        public const string ENABLED_SECTION = $"Enable {UPGRADE_NAME} Upgrade";
        public const bool ENABLED_DEFAULT = true;
        public const string ENABLED_DESCRIPTION = "Increases player's health.";

        public const string PRICE_SECTION = $"{UPGRADE_NAME} Price";
        public const int PRICE_DEFAULT = 600;

        public const string PRICES_DEFAULT = "300, 450, 600";

        public const string ADDITIONAL_HEALTH_UNLOCK_SECTION = "Initial health boost";
        public const int ADDITIONAL_HEALTH_UNLOCK_DEFAULT = 20;
        public const string ADDITIONAL_HEALTH_UNLOCK_DESCRIPTION = "Amount of health gained when unlocking the upgrade";

        public const string ADDITIONAL_HEALTH_INCREMENT_SECTION = "Additional health boost";
        public const int ADDITIONAL_HEALTH_INCREMENT_DEFAULT = 20;
        public const string ADDITIONAL_HEALTH_INCREMENT_DESCRIPTION = $"Every time {UPGRADE_NAME} is upgraded this value will be added to the value above.";

        internal override void Start()
        {
            upgradeName = UPGRADE_NAME;
            logger = new LguLogger(UPGRADE_NAME);
            base.Start();
            changingAttribute = GameAttribute.PLAYER_HEALTH;
            initialValue = UpgradeBus.Instance.PluginConfiguration.PLAYER_HEALTH_ADDITIONAL_HEALTH_UNLOCK.Value;
            incrementalValue = UpgradeBus.Instance.PluginConfiguration.PLAYER_HEALTH_ADDITIONAL_HEALTH_INCREMENT.Value;
        }

        public override void Increment()
        {
            base.Increment();
            PlayerControllerB player = UpgradeBus.Instance.GetLocalPlayer();
            LguStore.Instance.PlayerHealthUpdateLevelServerRpc(player.playerSteamId, GetUpgradeLevel(UPGRADE_NAME));
        }

        public override void Unwind()
        {
            base.Unwind();
            PlayerControllerB player = UpgradeBus.Instance.GetLocalPlayer();
            LguStore.Instance.PlayerHealthUpdateLevelServerRpc(player.playerSteamId, -1);
        }
        public static int CheckForAdditionalHealth(int health)
        {
            PlayerControllerB player = UpgradeBus.Instance.GetLocalPlayer();
            if (!UpgradeBus.Instance.playerHealthLevels.ContainsKey(player.playerSteamId)) return health;
            int currentLevel = UpgradeBus.Instance.playerHealthLevels[player.playerSteamId];

            return health + UpgradeBus.Instance.PluginConfiguration.PLAYER_HEALTH_ADDITIONAL_HEALTH_UNLOCK.Value + currentLevel * UpgradeBus.Instance.PluginConfiguration.PLAYER_HEALTH_ADDITIONAL_HEALTH_INCREMENT.Value;
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
            int currentLevel = UpgradeBus.Instance.playerHealthLevels[steamId];
            return health + UpgradeBus.Instance.PluginConfiguration.PLAYER_HEALTH_ADDITIONAL_HEALTH_UNLOCK.Value + currentLevel * UpgradeBus.Instance.PluginConfiguration.PLAYER_HEALTH_ADDITIONAL_HEALTH_INCREMENT.Value;
        }
        public string GetWorldBuildingText(bool shareStatus = false)
        {
            return string.Format(WORLD_BUILDING_TEXT, shareStatus ? "your crew" : "you");
        }

        public override string GetDisplayInfo(int initialPrice = -1, int maxLevels = -1, int[] incrementalPrices = null)
        {
            Func<int, float> infoFunction = level => UpgradeBus.Instance.PluginConfiguration.PLAYER_HEALTH_ADDITIONAL_HEALTH_UNLOCK.Value + level * UpgradeBus.Instance.PluginConfiguration.PLAYER_HEALTH_ADDITIONAL_HEALTH_INCREMENT.Value;
            string infoFormat = AssetBundleHandler.GetInfoFromJSON(UPGRADE_NAME);
            return Tools.GenerateInfoForUpgrade(infoFormat, initialPrice, incrementalPrices, infoFunction);
        }
    }
}
