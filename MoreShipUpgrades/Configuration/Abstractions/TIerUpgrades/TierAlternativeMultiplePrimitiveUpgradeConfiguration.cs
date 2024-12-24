using BepInEx.Configuration;
using CSync.Lib;
using MoreShipUpgrades.Configuration.Interfaces;
using System;

namespace MoreShipUpgrades.Configuration.Abstractions.TIerUpgrades
{
    public class TierAlternativeMultiplePrimitiveUpgradeConfiguration<K, T> : TierMultiplePrimitiveUpgradeConfiguration<K>, IAlternativeUpgradeConfiguration<T> where T : Enum
    {
        public TierAlternativeMultiplePrimitiveUpgradeConfiguration(ConfigFile cfg, string topSection, string enabledDescription, string defaultPrices) : base(cfg, topSection, enabledDescription, defaultPrices)
        {
        }

        public TierAlternativeMultiplePrimitiveUpgradeConfiguration(ConfigFile cfg, string topSection, string enabledDescription, string defaultPrices, SyncedEntry<T> alternativeMode) : base(cfg, topSection, enabledDescription, defaultPrices)
        {
            AlternativeMode = alternativeMode;
        }

        [field: SyncedEntryField] public SyncedEntry<T> AlternativeMode { get; set; }
    }
}
