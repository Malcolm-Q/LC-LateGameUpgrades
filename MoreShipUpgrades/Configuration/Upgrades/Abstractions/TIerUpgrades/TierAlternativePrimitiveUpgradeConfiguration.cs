using BepInEx.Configuration;
using CSync.Lib;
using MoreShipUpgrades.Configuration.Upgrades.Interfaces.TierUpgrades;
using System;

namespace MoreShipUpgrades.Configuration.Upgrades.Abstractions.TIerUpgrades
{
    public class TierAlternativePrimitiveUpgradeConfiguration<K, T> : TierPrimitiveUpgradeConfiguration<K>, ITierAlternativeEffectUpgradeConfiguration<K,T> where T : Enum
    {
        public TierAlternativePrimitiveUpgradeConfiguration(ConfigFile cfg, string topSection, string enabledDescription, string defaultPrices) : base(cfg, topSection, enabledDescription, defaultPrices)
        {
        }

        public TierAlternativePrimitiveUpgradeConfiguration(ConfigFile cfg, string topSection, string enabledDescription, string defaultPrices, SyncedEntry<T> alternativeMode) : base(cfg, topSection, enabledDescription, defaultPrices)
        {
            AlternativeMode = alternativeMode;
        }

        [field: SyncedEntryField] public SyncedEntry<T> AlternativeMode { get; set; }
    }
}
