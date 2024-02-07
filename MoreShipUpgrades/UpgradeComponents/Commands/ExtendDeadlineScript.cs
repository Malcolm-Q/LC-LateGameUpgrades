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
        internal static ExtendDeadlineScript instance;
        void Start()
        {
            logger = new LGULogger(NAME);
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        [ClientRpc]
        public void ExtendDeadlineClientRpc(int days)
        {
            float before = TimeOfDay.Instance.timeUntilDeadline;
            TimeOfDay.Instance.timeUntilDeadline += TimeOfDay.Instance.totalTime * days;
            TimeOfDay.Instance.UpdateProfitQuotaCurrentTime();
            TimeOfDay.Instance.SyncTimeClientRpc(TimeOfDay.Instance.globalTime, (int)TimeOfDay.Instance.timeUntilDeadline);
            logger.LogDebug($"Previous time: {before}, new time: {TimeOfDay.Instance.timeUntilDeadline}");
        }

        [ServerRpc(RequireOwnership = false)]
        public void ExtendDeadlineServerRpc(int days)
        {
            ExtendDeadlineClientRpc(days);
        }
    }
}
