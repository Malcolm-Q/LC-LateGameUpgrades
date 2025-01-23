using CSync.Lib;
using System;
using System.Collections.Generic;
using System.Text;

namespace MoreShipUpgrades.Configuration.Interfaces
{
    public interface IAlternativeUpgradeConfiguration<T> : IUpgradeConfiguration where T : Enum
    {
        SyncedEntry<T> AlternativeMode { get; set; }
    }
}
