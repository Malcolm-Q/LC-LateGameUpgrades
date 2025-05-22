using MoreShipUpgrades.Configuration.Upgrades.Interfaces;
using System;

namespace MoreShipUpgrades.Configuration.Upgrades.Interfaces.TierUpgrades
{
    internal interface ITierAlternativeMultipleEffectUpgradeConfiguration<K, V, T> : ITierMultipleEffectUpgradeConfiguration<K, V>, IAlternativeUpgradeConfiguration<T> where T : Enum
    {
    }
}
