using CSync.Lib;

namespace MoreShipUpgrades.Configuration.Interfaces
{
    public interface ITierCollectionUpgrade : ITierUpgradeConfiguration
    {
        public SyncedEntry<string> TierCollection { get; set; }
    }
}
