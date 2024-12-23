using BepInEx.Configuration;
using CSync.Extensions;
using CSync.Lib;
using MoreShipUpgrades.Configuration.Interfaces;
using MoreShipUpgrades.Misc.Util;

namespace MoreShipUpgrades.Configuration.Abstractions.OneTimeUpgrades
{
    public class OneTimeUpgradeConfiguration : UpgradeConfiguration, IOneTimeUpgradeConfiguration
    {
        public OneTimeUpgradeConfiguration(ConfigFile cfg, string topSection, string enabledDescription, int defaultPrice) : base(cfg, topSection, enabledDescription)
        {
            Price = cfg.BindSyncedEntry(topSection, string.Format(LguConstants.PRICE_FORMAT, topSection), defaultPrice);
        }

        [field: SyncedEntryField] public SyncedEntry<int> Price { get; set; }
    }
}
