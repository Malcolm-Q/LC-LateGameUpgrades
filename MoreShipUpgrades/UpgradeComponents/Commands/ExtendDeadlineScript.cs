using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc;
using MoreShipUpgrades.Misc.Commands;
using Unity.Netcode;

namespace MoreShipUpgrades.UpgradeComponents.Commands
{
    public class ExtendDeadlineScript : BaseCommand
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
            SetDaysExtended(GetDaysExtended() + days);
            if (IsHost || IsServer)
            {
                LguStore.Instance.UpdateServerSave();
            }
            logger.LogDebug($"Previous time: {before}, new time: {TimeOfDay.Instance.timeUntilDeadline}");
        }

        [ServerRpc(RequireOwnership = false)]
        public void ExtendDeadlineServerRpc(int days)
        {
            ExtendDeadlineClientRpc(days);
        }

        internal static int GetTotalCost()
        {
            return UpgradeBus.Instance.PluginConfiguration.EXTEND_DEADLINE_PRICE + UpgradeBus.Instance.PluginConfiguration.EXTEND_DEADLINE_ADDITIONAL_PRICE_PER_QUOTA * TimeOfDay.Instance.timesFulfilledQuota;
        }

        internal int GetTotalCostPerDay(int days)
        {
            int daysExtended = GetDaysExtended();
            int totalCost = 0;
            for(int i = 0; i < days; i++)
            {
                totalCost += GetTotalCost() + daysExtended * UpgradeBus.Instance.PluginConfiguration.EXTEND_DEADLINE_ADDITIONAL_PRICE_PER_DAY;
                daysExtended++;
            }
            return totalCost;
        }

        public static int GetDaysExtended()
        {
            return UpgradeBus.Instance.daysExtended;
        }

        public static void SetDaysExtended(int daysExtended)
        {
            UpgradeBus.Instance.daysExtended = daysExtended;
        }

        internal static new void RegisterCommand()
        {
            SetupGenericCommand<ExtendDeadlineScript>(NAME);
        }
    }
}
