using CSync.Lib;

namespace MoreShipUpgrades.Configuration.Interfaces.TierUpgrades
{
    public interface ITierEffectUpgradeConfiguration<T> : ITierUpgradeConfiguration
    {
        public SyncedEntry<T> InitialEffect { get; set; }
        public SyncedEntry<T> IncrementalEffect { get; set; }
    }
}
