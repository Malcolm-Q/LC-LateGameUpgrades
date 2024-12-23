using CSync.Lib;

namespace MoreShipUpgrades.Configuration.Interfaces
{
    public interface IIndividualUpgradeConfiguration : IUpgradeConfiguration
    {
        public SyncedEntry<bool> Individual { get; set; }
    }
}
