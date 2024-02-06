using BepInEx.Logging;
using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc;
using MoreShipUpgrades.Misc.Upgrades;
using Unity.Netcode;

namespace MoreShipUpgrades.UpgradeComponents.Commands
{
    /// <summary>
    /// Currently this isn't working, I will have to figure out why
    /// </summary>
    public class ExtendDeadlineScript : NetworkBehaviour
    {
        public static string NAME = "Extend Deadline";
        public static string ENABLED_SECTION = $"Enable {NAME}";
        private static LGULogger logger;
        void Start()
        {
            logger = new LGULogger(NAME);
            DontDestroyOnLoad(gameObject);
        }
    }
}
