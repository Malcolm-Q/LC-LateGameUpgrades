using BepInEx.Configuration;
using CSync.Extensions;
using CSync.Lib;
using MoreShipUpgrades.Configuration.Abstractions.OneTimeUpgrades;
using MoreShipUpgrades.Misc.Util;

namespace MoreShipUpgrades.Configuration.Custom
{
    public class DropPodThrustersUpgradeConfiguration : OneTimeUpgradeConfiguration
    {
        [field: SyncedEntryField] public SyncedEntry<float> InitialTimer { get; set; }
        [field: SyncedEntryField] public SyncedEntry<float> Timer { get; set; }
        [field: SyncedEntryField] public SyncedEntry<float> ExitTimer { get; set; }
        public DropPodThrustersUpgradeConfiguration(ConfigFile cfg, string topSection, string enabledDescription, int defaultPrice) : base(cfg, topSection, enabledDescription, defaultPrice)
        {
            Timer = cfg.BindSyncedEntry(topSection, LguConstants.DROP_POD_THRUSTERS_TIME_DECREASE_KEY, LguConstants.DROP_POD_THRUSTERS_TIME_DECREASE_DEFAULT);
            InitialTimer = cfg.BindSyncedEntry(topSection, LguConstants.DROP_POD_THRUSTERS_FIRST_TIME_DECREASE_KEY, LguConstants.DROP_POD_THRUSTERS_FIRST_TIME_DECREASE_DEFAULT);
            ExitTimer = cfg.BindSyncedEntry(topSection, LguConstants.DROP_POD_THRUSTERS_LEAVE_TIMER_KEY, LguConstants.DROP_POD_THRUSTERS_LEAVE_TIMER_DEFAULT, LguConstants.DROP_POD_THRUSTERS_LEAVE_TIMER_DESCRIPTION);
        }
    }
}
