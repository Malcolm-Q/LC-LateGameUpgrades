using BepInEx.Configuration;
using CSync.Extensions;
using CSync.Lib;
using MoreShipUpgrades.Configuration.Upgrades.Abstractions.TIerUpgrades;
using MoreShipUpgrades.Misc.Util;

namespace MoreShipUpgrades.Configuration.Upgrades.Custom
{
    public class TZPBufferUpgradeConfiguration : TierIndividualMultiplePrimitiveUpgradeConfiguration<int>
    {
        public TZPBufferUpgradeConfiguration(ConfigFile cfg, string topSection, string enabledDescription, string defaultPrices) : base(cfg, topSection, enabledDescription, defaultPrices)
        {
        }
    }
}
