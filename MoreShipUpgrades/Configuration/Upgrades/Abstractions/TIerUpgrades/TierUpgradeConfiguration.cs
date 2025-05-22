using BepInEx.Configuration;
using CSync.Extensions;
using CSync.Lib;
using MoreShipUpgrades.Configuration.Upgrades.Abstractions;
using MoreShipUpgrades.Configuration.Upgrades.Interfaces.TierUpgrades;
using MoreShipUpgrades.Misc.Upgrades;

namespace MoreShipUpgrades.Configuration.Upgrades.Abstractions.TIerUpgrades
{
    public class TierUpgradeConfiguration : UpgradeConfiguration, ITierUpgradeConfiguration
    {
        [field: SyncedEntryField] public SyncedEntry<string> Prices { get; set; }
        public TierUpgradeConfiguration(ConfigFile cfg, string topSection, string enabledDescription, string defaultPrices) : base(cfg, topSection, enabledDescription)
        {
            Prices = cfg.BindSyncedEntry(topSection, BaseUpgrade.PRICES_SECTION, defaultPrices, BaseUpgrade.PRICES_DESCRIPTION);
        }
    }
}
