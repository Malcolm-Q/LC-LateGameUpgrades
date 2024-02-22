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
            upgradeName = UPGRADE_NAME;
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
    }
}
