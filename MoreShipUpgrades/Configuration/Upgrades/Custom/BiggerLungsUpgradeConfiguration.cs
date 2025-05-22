using BepInEx.Configuration;
using CSync.Lib;
using MoreShipUpgrades.Configuration.Upgrades.Abstractions.TIerUpgrades;

namespace MoreShipUpgrades.Configuration.Upgrades.Custom
{
    public class BiggerLungsUpgradeConfiguration : TierIndividualMultiplePrimitiveUpgradeConfiguration<float>
    {
        public BiggerLungsUpgradeConfiguration(ConfigFile cfg, string topSection, string enabledDescription, string defaultPrices) : base(cfg, topSection, enabledDescription, defaultPrices)
        {
        }

        [field: SyncedEntryField] public SyncedEntry<int> JumpReductionLevel { get; set; }
        [field: SyncedEntryField] public SyncedEntry<int> StaminaRegenerationLevel {  get; set; }

    }
}
