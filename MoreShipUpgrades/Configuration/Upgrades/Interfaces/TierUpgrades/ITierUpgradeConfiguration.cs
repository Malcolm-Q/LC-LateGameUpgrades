using CSync.Lib;
using MoreShipUpgrades.Configuration.Upgrades.Interfaces;

namespace MoreShipUpgrades.Configuration.Upgrades.Interfaces.TierUpgrades
{
    public interface ITierUpgradeConfiguration : IUpgradeConfiguration
    {
        SyncedEntry<string> Prices { get; set; }
    }
}
