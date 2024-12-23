using BepInEx.Configuration;
using CSync.Lib;

namespace MoreShipUpgrades.Configuration.Abstractions.OneTimeUpgrades
{
    public class OneTimePrimitiveUpgradeConfiguration<T> : OneTimeUpgradeConfiguration
    {
        public OneTimePrimitiveUpgradeConfiguration(ConfigFile cfg, string topSection, string enabledDescription, int defaultPrice) : base(cfg, topSection, enabledDescription, defaultPrice)
        {
        }

        [field: SyncedEntryField] public SyncedEntry<T> Effect { get; set; }
    }
}
