using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc;
using Unity.Netcode;

namespace MoreShipUpgrades.UpgradeComponents
{
    public class ExtendDeadlineScript : BaseUpgrade
    {
        public static string UPGRADE_NAME = "Extend Deadline";
        public static string ENABLED_SECTION = $"Enable {UPGRADE_NAME}";
        private static LGULogger logger = new LGULogger(nameof(ExtendDeadlineScript));
        void Start()
        {
            upgradeName = UPGRADE_NAME;
            DontDestroyOnLoad(gameObject);
            Register();
            UpgradeBus.instance.extendScript = this;
        }

        public override void Register()
        {
            base.Register();
        }

        [ClientRpc]
        public void ExtendDeadlineClientRpc(int days)
        {
            float before = TimeOfDay.Instance.timeUntilDeadline;
            TimeOfDay.Instance.timeUntilDeadline += TimeOfDay.Instance.totalTime * days;
            TimeOfDay.Instance.UpdateProfitQuotaCurrentTime();
            TimeOfDay.Instance.SyncTimeClientRpc(TimeOfDay.Instance.globalTime, (int)TimeOfDay.Instance.timeUntilDeadline);
            logger.LogInfo($"Previous time: {before}, new time: {TimeOfDay.Instance.timeUntilDeadline}");
        }

        [ServerRpc(RequireOwnership = false)]
        public void ExtendDeadlineServerRpc(int days)
        {
            ExtendDeadlineClientRpc(days);
        }
    }
}
