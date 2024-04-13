using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc;
using MoreShipUpgrades.Misc.Upgrades;

namespace MoreShipUpgrades.UpgradeComponents.OneTimeUpgrades
{
    class FastEncryption : OneTimeUpgrade
    {
        public const string UPGRADE_NAME = "Fast Encryption";
        public static FastEncryption instance;
        private static LguLogger logger = new LguLogger(UPGRADE_NAME);

        const float TRANSMIT_MULTIPLIER = 0.2f;
        void Awake()
        {
            upgradeName = UpgradeBus.Instance.PluginConfiguration.OVERRIDE_UPGRADE_NAMES ? UpgradeBus.Instance.PluginConfiguration.FAST_ENCRYPTION_OVERRIDE_NAME : UPGRADE_NAME;
            instance = this;
        }
        public static int GetLimitOfCharactersTransmit(int defaultLimit, string message)
        {
            if (!GetActiveUpgrade(UPGRADE_NAME)) return defaultLimit;
            return message.Length;
        }
        public static float GetMultiplierOnSignalTextTimer(float defaultMultiplier)
        {
            if (!GetActiveUpgrade(UPGRADE_NAME)) return defaultMultiplier;
            return defaultMultiplier* TRANSMIT_MULTIPLIER;
        }

        public override string GetDisplayInfo(int price = -1)
        {
            return $"${price} - The transmitter will write the letters faster and the restriction of characters will be lifted.";
        }

        internal override bool CanInitializeOnStart()
        {
            return UpgradeBus.Instance.PluginConfiguration.PAGER_PRICE.Value <= 0;
        }
    }
}
