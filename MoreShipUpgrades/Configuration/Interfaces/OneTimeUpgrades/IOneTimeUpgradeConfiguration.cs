using CSync.Lib;

namespace MoreShipUpgrades.Configuration.Interfaces.OneTimeUpgrades
{
    public interface IOneTimeUpgradeConfiguration : IUpgradeConfiguration
    {
        SyncedEntry<int> Price { get; set; }
    }
}
