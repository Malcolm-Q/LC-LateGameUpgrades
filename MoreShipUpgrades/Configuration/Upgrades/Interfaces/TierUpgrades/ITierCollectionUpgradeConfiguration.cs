using CSync.Lib;

namespace MoreShipUpgrades.Configuration.Upgrades.Interfaces.TierUpgrades
{
    public interface ITierCollectionUpgradeConfiguration : ITierUpgradeConfiguration
    {
        public SyncedEntry<string> TierCollection { get; set; }
    }
}
