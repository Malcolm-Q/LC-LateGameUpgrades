using CSync.Lib;

namespace MoreShipUpgrades.Configuration.Interfaces.TierUpgrades
{
    public interface ITierCollectionUpgradeConfiguration : ITierUpgradeConfiguration
    {
        public SyncedEntry<string> TierCollection { get; set; }
    }
}
