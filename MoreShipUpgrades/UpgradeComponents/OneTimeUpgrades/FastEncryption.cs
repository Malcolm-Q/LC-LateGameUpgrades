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

            UpgradeBus.instance.pager = true;
            instance = this;
        }

        public override void Unwind()
        {
            base.Unwind();
            UpgradeBus.instance.pager = false;
        }
        public static int GetLimitOfCharactersTransmit(int defaultLimit, string message)
        {
            logger.LogDebug("Being called");
            if (!UpgradeBus.instance.pager) return defaultLimit;
            return message.Length;
        }
        public static float GetMultiplierOnSignalTextTimer(float defaultMultiplier)
        {
            if (!UpgradeBus.instance.pager) return defaultMultiplier;
            return defaultMultiplier* TRANSMIT_MULTIPLIER;
        }

        public override string GetDisplayInfo(int price = -1)
        {
            return $"${price} - The transmitter will write the letters faster and the restriction of characters will be lifted.";
        }
    }
}
