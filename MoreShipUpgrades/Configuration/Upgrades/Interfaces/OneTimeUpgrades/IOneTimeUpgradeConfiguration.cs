using CSync.Lib;
using MoreShipUpgrades.Configuration.Upgrades.Interfaces;

namespace MoreShipUpgrades.Configuration.Upgrades.Interfaces.OneTimeUpgrades
{
    public interface IOneTimeUpgradeConfiguration : IUpgradeConfiguration
    {
        SyncedEntry<int> Price { get; set; }
    }
}
