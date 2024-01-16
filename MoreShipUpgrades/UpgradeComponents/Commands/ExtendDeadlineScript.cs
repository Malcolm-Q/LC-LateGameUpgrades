using BepInEx.Logging;
using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc;
using Unity.Netcode;

namespace MoreShipUpgrades.UpgradeComponents.Commands
{
    /// <summary>
    /// Currently this isn't working, I will have to figure out why
    /// </summary>
    public class ExtendDeadlineScript : BaseUpgrade
    {
        public static string UPGRADE_NAME = "Extend Deadline";
        public static string ENABLED_SECTION = $"Enable {UPGRADE_NAME}";
        private static LGULogger logger;
        void Start()
        {
            upgradeName = UPGRADE_NAME;
            logger = new LGULogger(upgradeName);
            DontDestroyOnLoad(gameObject);
            Register();
            logger.LogDebug(upgradeName);
        }
        public override void Register()
        {
            base.Register();
        }
    }
}
