using BepInEx.Configuration;
using CSync.Extensions;
using CSync.Lib;
using MoreShipUpgrades.Configuration.Upgrades.Abstractions.TIerUpgrades;
using MoreShipUpgrades.Misc.Util;

namespace MoreShipUpgrades.Configuration.Upgrades.Custom
{
    public class DeeperPocketsUpgradeConfiguration : TierIndividualPrimitiveUpgradeConfiguration<int>
    {
        [field: SyncedEntryField] public SyncedEntry<bool> AllowWheelbarrows { get; set; }
        public DeeperPocketsUpgradeConfiguration(ConfigFile cfg, string topSection, string enabledDescription, string defaultPrices) : base(cfg, topSection, enabledDescription, defaultPrices)
        {
            AllowWheelbarrows = cfg.BindSyncedEntry(topSection, LguConstants.DEEPER_POCKETS_ALLOW_WHEELBARROWS_KEY, LguConstants.DEEPER_POCKETS_ALLOW_WHEELBARROWS_DEFAULT, LguConstants.DEEPER_POCKETS_ALLOW_WHEELBARROWS_DESCRIPTION);
        }
    }
}
