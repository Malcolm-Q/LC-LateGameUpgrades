using BepInEx.Configuration;
using CSync.Lib;
using MoreShipUpgrades.Configuration.Interfaces.OneTimeUpgrades;
using System;

namespace MoreShipUpgrades.Configuration.Abstractions.OneTimeUpgrades
{
    public class OneTimeAlternativePrimitiveUpgradeConfiguration<K, V> : OneTimePrimitiveUpgradeConfiguration<K>, IOneTimeAlternativeEffectUpgradeConfiguration<K, V> where V : Enum
    {
        public OneTimeAlternativePrimitiveUpgradeConfiguration(ConfigFile cfg, string topSection, string enabledDescription, int defaultPrice) : base(cfg, topSection, enabledDescription, defaultPrice)
        {
        }

        [field: SyncedEntryField] public SyncedEntry<V> AlternativeMode { get; set; }
    }
}
