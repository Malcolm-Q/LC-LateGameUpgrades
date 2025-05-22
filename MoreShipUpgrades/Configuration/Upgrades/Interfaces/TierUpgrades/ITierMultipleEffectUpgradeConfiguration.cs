using CSync.Lib;
using System.Collections.Generic;

namespace MoreShipUpgrades.Configuration.Upgrades.Interfaces.TierUpgrades
{
    public interface ITierMultipleEffectUpgradeConfiguration<T> : ITierUpgradeConfiguration
    {
        List<SyncedEntry<T>> InitialEffects { get; set; }
        List<SyncedEntry<T>> IncrementalEffects { get; set; }
        void AddEffectPair(SyncedEntry<T> initialEffect, SyncedEntry<T> incrementalEffect);
        (SyncedEntry<T>, SyncedEntry<T>) GetEffectPair(int index);
    }
    public interface ITierMultipleEffectUpgradeConfiguration<K, T> : ITierMultipleEffectUpgradeConfiguration<K>
    {
        List<SyncedEntry<T>> InitialSecondEffects { get; set; }
        List<SyncedEntry<T>> IncrementalSecondEffects { get; set; }
        void AddSecondEffectPair(SyncedEntry<T> initialEffect, SyncedEntry<T> incrementalEffect);
        (SyncedEntry<T>, SyncedEntry<T>) GetSecondEffectPair(int index);
    }
}
