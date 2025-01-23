using BepInEx.Configuration;
using CSync.Extensions;
using CSync.Lib;
using MoreShipUpgrades.Configuration.Interfaces;
using MoreShipUpgrades.Misc.Upgrades;
using System.Collections.Generic;

namespace MoreShipUpgrades.Configuration.Abstractions.TIerUpgrades
{
    public class TierIndividualMultiplePrimitiveUpgradeConfiguration<T> : TierMultiplePrimitiveUpgradeConfiguration<T>, IIndividualUpgradeConfiguration
    {
        [field: SyncedEntryField] public SyncedEntry<bool> Individual { get; set; }
        public TierIndividualMultiplePrimitiveUpgradeConfiguration(ConfigFile cfg, string topSection, string enabledDescription, string defaultPrices) : base(cfg, topSection, enabledDescription, defaultPrices)
        {
            Individual = cfg.BindSyncedEntry(topSection,
            BaseUpgrade.INDIVIDUAL_SECTION,
            BaseUpgrade.INDIVIDUAL_DEFAULT,
            BaseUpgrade.INDIVIDUAL_DESCRIPTION);
        }
        public TierIndividualMultiplePrimitiveUpgradeConfiguration(ConfigFile cfg, string topSection, string enabledDescription, string defaultPrices, List<SyncedEntry<T>> initialEffects, List<SyncedEntry<T>> incrementalEffects) : base(cfg, topSection, enabledDescription, defaultPrices)
        {
            Individual = cfg.BindSyncedEntry(topSection,
            BaseUpgrade.INDIVIDUAL_SECTION,
            BaseUpgrade.INDIVIDUAL_DEFAULT,
            BaseUpgrade.INDIVIDUAL_DESCRIPTION);
        }
    }
    public class TierIndividualMultiplePrimitiveUpgradeConfiguration<K,T> : TierMultiplePrimitiveUpgradeConfiguration<K,T>, IIndividualUpgradeConfiguration
    {
        [field: SyncedEntryField] public SyncedEntry<bool> Individual { get; set; }
        public TierIndividualMultiplePrimitiveUpgradeConfiguration(ConfigFile cfg, string topSection, string enabledDescription, string defaultPrices) : base(cfg, topSection, enabledDescription, defaultPrices)
        {
            Individual = cfg.BindSyncedEntry(topSection,
            BaseUpgrade.INDIVIDUAL_SECTION,
            BaseUpgrade.INDIVIDUAL_DEFAULT,
            BaseUpgrade.INDIVIDUAL_DESCRIPTION);
        }
        public TierIndividualMultiplePrimitiveUpgradeConfiguration(ConfigFile cfg, string topSection, string enabledDescription, string defaultPrices, List<SyncedEntry<K>> initialEffects, List<SyncedEntry<K>> incrementalEffects) : base(cfg, topSection, enabledDescription, defaultPrices, initialEffects, incrementalEffects)
        {
            Individual = cfg.BindSyncedEntry(topSection,
            BaseUpgrade.INDIVIDUAL_SECTION,
            BaseUpgrade.INDIVIDUAL_DEFAULT,
            BaseUpgrade.INDIVIDUAL_DESCRIPTION);
        }
        public TierIndividualMultiplePrimitiveUpgradeConfiguration(ConfigFile cfg, string topSection, string enabledDescription, string defaultPrices, List<SyncedEntry<K>> initialEffects, List<SyncedEntry<K>> incrementalEffects, List<SyncedEntry<T>> initialSecondEffects, List<SyncedEntry<T>> incrementalSecondEffects) : base(cfg, topSection, enabledDescription, defaultPrices, initialEffects, incrementalEffects, initialSecondEffects, incrementalSecondEffects)
        {
            Individual = cfg.BindSyncedEntry(topSection,
            BaseUpgrade.INDIVIDUAL_SECTION,
            BaseUpgrade.INDIVIDUAL_DEFAULT,
            BaseUpgrade.INDIVIDUAL_DESCRIPTION);
        }
    }
}
