using System;

namespace MoreShipUpgrades.Configuration.Interfaces.TierUpgrades
{
    internal interface ITierAlternativeMultipleEffectUpgradeConfiguration<K, V, T> : ITierMultipleEffectUpgradeConfiguration<K, V>, IAlternativeUpgradeConfiguration<T> where T : Enum
    {
    }
}
