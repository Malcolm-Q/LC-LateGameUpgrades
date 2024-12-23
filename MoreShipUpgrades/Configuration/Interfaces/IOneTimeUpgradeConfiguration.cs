using CSync.Lib;

namespace MoreShipUpgrades.Configuration.Interfaces
{
    public interface IOneTimeUpgradeConfiguration : IUpgradeConfiguration
    {
        SyncedEntry<int> Price {  get; set; }
    }
}
