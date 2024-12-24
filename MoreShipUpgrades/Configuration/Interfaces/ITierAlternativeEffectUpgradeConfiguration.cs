using System;

namespace MoreShipUpgrades.Configuration.Interfaces
{
    public interface ITierAlternativeEffectUpgradeConfiguration<K, T> : ITierEffectUpgradeConfiguration<K>, IAlternativeUpgradeConfiguration<T> where T : Enum
    {
    }
}
