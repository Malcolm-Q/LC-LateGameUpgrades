using BepInEx.Configuration;
using CSync.Extensions;
using CSync.Lib;
using MoreShipUpgrades.Configuration.Upgrades.Abstractions.TIerUpgrades;
using MoreShipUpgrades.Misc.Util;

namespace MoreShipUpgrades.Configuration.Upgrades.Custom
{
    public class LandingThrusterUpgradeConfiguration : TierPrimitiveUpgradeConfiguration<int>
    {
        [field: SyncedEntryField] public SyncedEntry<bool> AffectLanding {  get; set; }
        [field: SyncedEntryField] public SyncedEntry<bool> AffectDeparture { get; set; }
        public LandingThrusterUpgradeConfiguration(ConfigFile cfg, string topSection, string enabledDescription, string defaultPrices) : base(cfg, topSection, enabledDescription, defaultPrices)
        {
            AffectLanding = cfg.BindSyncedEntry(topSection, LguConstants.LANDING_THRUSTERS_AFFECT_LANDING_KEY, LguConstants.LANDING_THRUSTERS_AFFECT_LANDING_DEFAULT, LguConstants.LANDING_THRUSTERS_AFFECT_LANDING_DESCRIPTION);
            AffectDeparture = cfg.BindSyncedEntry(topSection, LguConstants.LANDING_THRUSTERS_AFFECT_DEPARTING_KEY, LguConstants.LANDING_THRUSTERS_AFFECT_DEPARTING_DEFAULT, LguConstants.LANDING_THRUSTERS_AFFECT_DEPARTING_DESCRIPTION);
        }
    }
}
