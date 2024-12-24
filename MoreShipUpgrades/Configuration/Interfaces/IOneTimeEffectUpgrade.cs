using CSync.Lib;

namespace MoreShipUpgrades.Configuration.Interfaces
{
    public interface IOneTimeEffectUpgrade<T> : IOneTimeUpgradeConfiguration
    {
        SyncedEntry<T> Effect { get; set; }
    }
}
