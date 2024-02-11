using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc;
using MoreShipUpgrades.Misc.Upgrades;
using Unity.Netcode;

namespace MoreShipUpgrades.UpgradeComponents.OneTimeUpgrades
{
    class FastEncryption : OneTimeUpgrade
    {
        public static string UPGRADE_NAME = "Fast Encryption";
        public static FastEncryption instance;
        private static LGULogger logger;

        const float TRANSMIT_MULTIPLIER = 0.2f;
        internal override void Start()
        {
            upgradeName = UPGRADE_NAME;
            logger = new LGULogger(upgradeName);
            base.Start();
        }
        public override void Load()
        {
            base.Load();
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
