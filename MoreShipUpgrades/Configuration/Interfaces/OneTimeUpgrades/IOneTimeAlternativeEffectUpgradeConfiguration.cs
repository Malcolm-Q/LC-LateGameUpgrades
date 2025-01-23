using System;

namespace MoreShipUpgrades.Configuration.Interfaces.OneTimeUpgrades
{
    public interface IOneTimeAlternativeEffectUpgradeConfiguration<K,T> : IOneTimeEffectUpgrade<K>, IAlternativeUpgradeConfiguration<T> where T : Enum
    {
    }
}
