using MoreShipUpgrades.Configuration.Upgrades.Interfaces;
using System;

namespace MoreShipUpgrades.Configuration.Upgrades.Interfaces.TierUpgrades
{
    public interface ITierAlternativeEffectUpgradeConfiguration<K, T> : ITierEffectUpgradeConfiguration<K>, IAlternativeUpgradeConfiguration<T> where T : Enum
    {
    }
}
