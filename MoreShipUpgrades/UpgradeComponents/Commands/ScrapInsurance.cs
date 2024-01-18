using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc;
using System;
using System.Collections.Generic;
using System.Text;
using Unity.Netcode;

namespace MoreShipUpgrades.UpgradeComponents.Commands
{
    internal class ScrapInsurance : NetworkBehaviour
    {
        public static string COMMAND_NAME = "Scrap Insurance";
        public static int DEFAULT_PRICE = 400;
        void Start()
        {
            DontDestroyOnLoad(gameObject);
            UpgradeBus.instance.insurance = false;
        }
        public static bool GetScrapInsuranceStatus()
        {
            LGULogger logger = new LGULogger(COMMAND_NAME);
            logger.LogDebug("Grabbing status of insurance...");
            return UpgradeBus.instance.insurance;
        }
        public static bool ScrapInsuranceStatus()
        {
            return UpgradeBus.instance.insurance;
        }
        public static void TurnOnScrapInsurance()
        {
            LGULogger logger = new LGULogger(COMMAND_NAME);
            logger.LogDebug("Turning on...");
            ToggleInsurance(true);
        }

        public static void TurnOffScrapInsurance()
        {
            LGULogger logger = new LGULogger(COMMAND_NAME);
            logger.LogDebug("Turning off...");
            ToggleInsurance(false);
        }

        static void ToggleInsurance(bool enabled)
        {
            UpgradeBus.instance.insurance = enabled;
        }
    }
}
