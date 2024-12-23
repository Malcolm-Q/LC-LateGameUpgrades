using BepInEx.Configuration;
using CSync.Lib;
using MoreShipUpgrades.Configuration.Interfaces;
using System;
using System.Collections.Generic;

namespace MoreShipUpgrades.Configuration.Abstractions.TIerUpgrades
{
    public class TierMultiplePrimitiveUpgradeConfiguration<T> : TierUpgradeConfiguration, ITierMultipleEffectUpgrade<T>
    {
        public TierMultiplePrimitiveUpgradeConfiguration(ConfigFile cfg, string topSection, string enabledDescription, string defaultPrices) : base(cfg, topSection, enabledDescription, defaultPrices)
        {
            InitialEffects = new();
            IncrementalEffects = new();
        }
        public TierMultiplePrimitiveUpgradeConfiguration(ConfigFile cfg, string topSection, string enabledDescription, string defaultPrices, List<SyncedEntry<T>> initialEffects, List<SyncedEntry<T>> incrementalEffects) : base(cfg, topSection, enabledDescription, defaultPrices)
        {
            InitialEffects = initialEffects;
            IncrementalEffects = incrementalEffects;
        }
        [field: SyncedEntryField] public List<SyncedEntry<T>> InitialEffects { get; set; }
        [field: SyncedEntryField] public List<SyncedEntry<T>> IncrementalEffects { get; set; }

        public void AddEffectPair(SyncedEntry<T> initialEffect, SyncedEntry<T> incrementalEffect)
        {
            InitialEffects.Add(initialEffect);
            IncrementalEffects.Add(incrementalEffect);
        }

        public (SyncedEntry<T>, SyncedEntry<T>) GetEffectPair(int index)
        {
            return (InitialEffects[index], IncrementalEffects[index]);
        }
    }
    public class TierMultiplePrimitiveUpgradeConfiguration<K,T> : TierMultiplePrimitiveUpgradeConfiguration<K>, ITierMultipleEffectUpgrade<K,T>
    {
        public TierMultiplePrimitiveUpgradeConfiguration(ConfigFile cfg, string topSection, string enabledDescription, string defaultPrices) : base(cfg, topSection, enabledDescription, defaultPrices)
        {
            InitialSecondEffects = new();
            IncrementalSecondEffects = new();
        }
        public TierMultiplePrimitiveUpgradeConfiguration(ConfigFile cfg, string topSection, string enabledDescription, string defaultPrices, List<SyncedEntry<K>> initialEffects, List<SyncedEntry<K>> incrementalEffects) : base(cfg, topSection, enabledDescription, defaultPrices, initialEffects, incrementalEffects)
        {
            InitialSecondEffects = new();
            IncrementalSecondEffects = new();
        }
        public TierMultiplePrimitiveUpgradeConfiguration(ConfigFile cfg, string topSection, string enabledDescription, string defaultPrices, List<SyncedEntry<K>> initialEffects, List<SyncedEntry<K>> incrementalEffects, List<SyncedEntry<T>> initialSecondEffects, List<SyncedEntry<T>> incrementalSecondEffects) : base(cfg, topSection, enabledDescription, defaultPrices, initialEffects, incrementalEffects)
        {
            InitialSecondEffects = initialSecondEffects;
            IncrementalSecondEffects = incrementalSecondEffects;
        }
        [field: SyncedEntryField] public List<SyncedEntry<T>> InitialSecondEffects { get; set; }
        [field: SyncedEntryField] public List<SyncedEntry<T>> IncrementalSecondEffects { get; set; }

        public void AddSecondEffectPair(SyncedEntry<T> initialEffect, SyncedEntry<T> incrementalEffect)
        {
            InitialSecondEffects.Add(initialEffect);
            IncrementalSecondEffects.Add(incrementalEffect);
        }

        public (SyncedEntry<T>, SyncedEntry<T>) GetSecondEffectPair(int index)
        {
            return (InitialSecondEffects[index], IncrementalSecondEffects[index]);
        }
    }
}
