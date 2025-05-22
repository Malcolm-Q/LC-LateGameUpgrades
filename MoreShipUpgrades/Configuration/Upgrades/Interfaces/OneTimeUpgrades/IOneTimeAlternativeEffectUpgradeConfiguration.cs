using MoreShipUpgrades.Configuration.Upgrades.Interfaces;
using System;

namespace MoreShipUpgrades.Configuration.Upgrades.Interfaces.OneTimeUpgrades
{
    public interface IOneTimeAlternativeEffectUpgradeConfiguration<K,T> : IOneTimeEffectUpgrade<K>, IAlternativeUpgradeConfiguration<T> where T : Enum
    {
    }
}
