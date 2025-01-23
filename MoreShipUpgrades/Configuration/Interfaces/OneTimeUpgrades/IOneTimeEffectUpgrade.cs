using CSync.Lib;

namespace MoreShipUpgrades.Configuration.Interfaces.OneTimeUpgrades
{
    public interface IOneTimeEffectUpgrade<T> : IOneTimeUpgradeConfiguration
    {
        SyncedEntry<T> Effect { get; set; }
    }
}
