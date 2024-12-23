using CSync.Lib;

namespace MoreShipUpgrades.Configuration.Interfaces
{
    public interface ITierEffectUpgrade<T> : ITierUpgradeConfiguration
    {
        public SyncedEntry<T> InitialEffect { get; set; }
        public SyncedEntry<T> IncrementalEffect { get; set; }
    }
}
