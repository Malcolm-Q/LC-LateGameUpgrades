using BepInEx.Configuration;
using CSync.Extensions;
using CSync.Lib;
using MoreShipUpgrades.Configuration.Upgrades.Interfaces;
using MoreShipUpgrades.Misc.Upgrades;

namespace MoreShipUpgrades.Configuration.Upgrades.Abstractions.OneTimeUpgrades
{
    public class OneTimeIndividualPrimitiveUpgradeConfiguration<T> : OneTimePrimitiveUpgradeConfiguration<T> , IIndividualUpgradeConfiguration
    {
        [field: SyncedEntryField] public SyncedEntry<bool> Individual { get; set; }
        public OneTimeIndividualPrimitiveUpgradeConfiguration(ConfigFile cfg, string topSection, string enabledDescription, int defaultPrice) : base(cfg, topSection, enabledDescription, defaultPrice)
        {
            Individual = cfg.BindSyncedEntry(topSection,
            BaseUpgrade.INDIVIDUAL_SECTION,
            BaseUpgrade.INDIVIDUAL_DEFAULT,
            BaseUpgrade.INDIVIDUAL_DESCRIPTION);
        }
    }
}
