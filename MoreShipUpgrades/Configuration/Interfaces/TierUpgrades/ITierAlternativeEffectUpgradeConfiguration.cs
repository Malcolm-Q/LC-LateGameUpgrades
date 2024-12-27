using System;

namespace MoreShipUpgrades.Configuration.Interfaces.TierUpgrades
{
    public interface ITierAlternativeEffectUpgradeConfiguration<K, T> : ITierEffectUpgradeConfiguration<K>, IAlternativeUpgradeConfiguration<T> where T : Enum
    {
    }
}
