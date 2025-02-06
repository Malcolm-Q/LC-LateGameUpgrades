using CSync.Lib;
using MoreShipUpgrades.UI.TerminalNodes;

namespace MoreShipUpgrades.Configuration.Interfaces
{
    public interface IUpgradeConfiguration
    {
        SyncedEntry<bool> Enabled { get; set; }
        SyncedEntry<int> MinimumSalePercentage { get; set; }
        SyncedEntry<int> MaximumSalePercentage { get; set; }
        SyncedEntry<string> OverrideName { get; set; }
        SyncedEntry<PurchaseMode> PurchaseMode {  get; set; }
        SyncedEntry<string> ItemProgressionItems { get; set; }
    }
}
