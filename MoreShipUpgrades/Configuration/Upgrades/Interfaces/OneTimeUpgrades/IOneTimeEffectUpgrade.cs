using CSync.Lib;

namespace MoreShipUpgrades.Configuration.Upgrades.Interfaces.OneTimeUpgrades
{
    public interface IOneTimeEffectUpgrade<T> : IOneTimeUpgradeConfiguration
    {
        SyncedEntry<T> Effect { get; set; }
    }
}
