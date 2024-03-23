using MoreShipUpgrades.Misc;
using Unity.Netcode;

namespace MoreShipUpgrades.UpgradeComponents.Commands
{
    internal class ScrapInsurance : NetworkBehaviour
    {
        internal const string COMMAND_NAME = "Scrap Insurance";
        internal const int DEFAULT_PRICE = 400;
        static bool insurance = false;
        void Start()
        {
            DontDestroyOnLoad(gameObject);
            insurance = false;
        }
        public static bool GetScrapInsuranceStatus()
        {
            LguLogger logger = new LguLogger(COMMAND_NAME);
            logger.LogDebug("Grabbing status of insurance...");
            return insurance;
        }
        public static void TurnOnScrapInsurance()
        {
            LguLogger logger = new LguLogger(COMMAND_NAME);
            logger.LogDebug("Turning on...");
            ToggleInsurance(true);
        }

        public static void TurnOffScrapInsurance()
        {
            LguLogger logger = new LguLogger(COMMAND_NAME);
            logger.LogDebug("Turning off...");
            ToggleInsurance(false);
        }

        static void ToggleInsurance(bool enabled)
        {
            insurance = enabled;
        }
    }
}
