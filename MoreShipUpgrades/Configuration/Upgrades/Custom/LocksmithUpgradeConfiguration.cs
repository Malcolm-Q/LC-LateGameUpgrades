using BepInEx.Configuration;
using CSync.Extensions;
using CSync.Lib;
using MoreShipUpgrades.Configuration.Upgrades.Abstractions.OneTimeUpgrades;

namespace MoreShipUpgrades.Configuration.Upgrades.Custom
{
    public class LocksmithUpgradeConfiguration : OneTimeIndividualUpgradeConfiguration
    {
        [field: SyncedEntryField] public SyncedEntry<bool> DisableDoorCollision { get; set; }
        public LocksmithUpgradeConfiguration(ConfigFile cfg, string topSection, string enabledDescription, int defaultPrice) : base(cfg, topSection, enabledDescription, defaultPrice)
        {
            DisableDoorCollision = cfg.BindSyncedEntry(topSection, "Disable Door Collision", false, "If disabled, the locksmith minigame will not be activated when colliding with locked doors, effectively only allowing interacting to start it");
        }
    }
}
