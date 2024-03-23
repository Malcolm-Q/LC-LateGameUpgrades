using MoreShipUpgrades.Misc;
using Unity.Netcode;

namespace MoreShipUpgrades.UpgradeComponents.Commands
{
    public class ExtendDeadlineScript : NetworkBehaviour
    {
        internal const string NAME = "Extend Deadline";
        internal const string ENABLED_SECTION = $"Enable {NAME}";
        private static LguLogger logger;
        internal static ExtendDeadlineScript instance;
        void Start()
        {
            logger = new LguLogger(NAME);
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
