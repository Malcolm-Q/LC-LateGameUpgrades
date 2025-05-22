using CSync.Lib;

namespace MoreShipUpgrades.Configuration.Upgrades.Interfaces
{
    public interface IIndividualUpgradeConfiguration : IUpgradeConfiguration
    {
        public SyncedEntry<bool> Individual { get; set; }
    }
}
