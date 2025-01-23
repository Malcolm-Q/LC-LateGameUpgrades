using BepInEx.Configuration;
using CSync.Extensions;
using CSync.Lib;
using MoreShipUpgrades.Configuration.Interfaces;
using MoreShipUpgrades.Misc.Upgrades;

namespace MoreShipUpgrades.Configuration.Abstractions.OneTimeUpgrades
{
    public class OneTimeIndividualUpgradeConfiguration : OneTimeUpgradeConfiguration, IIndividualUpgradeConfiguration
    {
        [field: SyncedEntryField] public SyncedEntry<bool> Individual { get; set; }
        public OneTimeIndividualUpgradeConfiguration(ConfigFile cfg, string topSection, string enabledDescription, int defaultPrice) : base(cfg, topSection, enabledDescription, defaultPrice)
        {
            Individual = cfg.BindSyncedEntry(topSection,
            BaseUpgrade.INDIVIDUAL_SECTION,
            BaseUpgrade.INDIVIDUAL_DEFAULT,
            BaseUpgrade.INDIVIDUAL_DESCRIPTION);
        }
    }
}
