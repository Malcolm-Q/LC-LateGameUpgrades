using System;

namespace MoreShipUpgrades.Configuration.Interfaces
{
    internal interface ITierAlternativeMultipleEffectUpgradeConfiguration<K,V,T> : ITierMultipleEffectUpgradeConfiguration<K,V> , IAlternativeUpgradeConfiguration<T> where T : Enum
    {
    }
}
