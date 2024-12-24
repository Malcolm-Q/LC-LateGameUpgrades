using CSync.Lib;

namespace MoreShipUpgrades.Configuration.Interfaces
{
    public interface ITierCollectionUpgradeConfiguration : ITierUpgradeConfiguration
    {
        public SyncedEntry<string> TierCollection { get; set; }
    }
}
