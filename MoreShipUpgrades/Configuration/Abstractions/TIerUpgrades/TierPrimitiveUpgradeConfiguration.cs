using BepInEx.Configuration;
using CSync.Lib;
using MoreShipUpgrades.Configuration.Interfaces.TierUpgrades;

namespace MoreShipUpgrades.Configuration.Abstractions.TIerUpgrades
{
    public class TierPrimitiveUpgradeConfiguration<T> : TierUpgradeConfiguration, ITierEffectUpgradeConfiguration<T>
    {
        public TierPrimitiveUpgradeConfiguration(ConfigFile cfg, string topSection, string enabledDescription, string defaultPrices) : base(cfg, topSection, enabledDescription, defaultPrices)
        {
        }

        [field: SyncedEntryField] public SyncedEntry<T> InitialEffect { get; set; }
        [field: SyncedEntryField] public SyncedEntry<T> IncrementalEffect { get; set; }
    }
}
