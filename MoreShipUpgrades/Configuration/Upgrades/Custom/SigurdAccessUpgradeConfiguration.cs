using BepInEx.Configuration;
using CSync.Lib;
using MoreShipUpgrades.Configuration.Upgrades.Abstractions.OneTimeUpgrades;
using MoreShipUpgrades.UpgradeComponents.OneTimeUpgrades.Store;
using System;
using System.Collections.Generic;
using System.Text;

namespace MoreShipUpgrades.Configuration.Upgrades.Custom
{
    public class SigurdAccessUpgradeConfiguration : OneTimeAlternativePrimitiveUpgradeConfiguration<float, Sigurd.FunctionModes>
    {
        [field: SyncedEntryField] public SyncedEntry<float> Chance { get; set; }
        public SigurdAccessUpgradeConfiguration(ConfigFile cfg, string topSection, string enabledDescription, int defaultPrice) : base(cfg, topSection, enabledDescription, defaultPrice)
        {
        }
    }
}
