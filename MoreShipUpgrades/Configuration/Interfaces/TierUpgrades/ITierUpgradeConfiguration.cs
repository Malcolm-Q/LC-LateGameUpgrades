using CSync.Lib;

namespace MoreShipUpgrades.Configuration.Interfaces.TierUpgrades
{
    public interface ITierUpgradeConfiguration : IUpgradeConfiguration
    {
        SyncedEntry<string> Prices { get; set; }
    }
}
