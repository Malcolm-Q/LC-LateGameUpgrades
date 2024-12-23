using CSync.Lib;

namespace MoreShipUpgrades.Configuration.Interfaces
{
    public interface ITierUpgradeConfiguration : IUpgradeConfiguration
    {
        SyncedEntry<string> Prices {  get; set; }
    }
}
