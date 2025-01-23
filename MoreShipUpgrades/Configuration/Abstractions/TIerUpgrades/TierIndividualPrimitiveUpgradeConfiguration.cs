using BepInEx.Configuration;
using CSync.Extensions;
using CSync.Lib;
using MoreShipUpgrades.Configuration.Interfaces;
using MoreShipUpgrades.Misc.Upgrades;
using System;

namespace MoreShipUpgrades.Configuration.Abstractions.TIerUpgrades
{
    public class TierIndividualAlternativePrimitiveUpgradeConfiguration<K,T> : TierAlternativePrimitiveUpgradeConfiguration<K,T>, IIndividualUpgradeConfiguration where T : Enum
    {
        [field: SyncedEntryField] public SyncedEntry<bool> Individual { get; set; }
        public TierIndividualAlternativePrimitiveUpgradeConfiguration(ConfigFile cfg, string topSection, string enabledDescription, string defaultPrices) : base(cfg, topSection, enabledDescription, defaultPrices)
        {
            Individual = cfg.BindSyncedEntry(topSection,
            BaseUpgrade.INDIVIDUAL_SECTION,
            BaseUpgrade.INDIVIDUAL_DEFAULT,
            BaseUpgrade.INDIVIDUAL_DESCRIPTION);
        }
    }
}
