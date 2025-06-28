using BepInEx.Configuration;
using CSync.Extensions;
using CSync.Lib;
using MoreShipUpgrades.Configuration.Upgrades.Interfaces;
using MoreShipUpgrades.Misc.Util;
using MoreShipUpgrades.UI.TerminalNodes;

namespace MoreShipUpgrades.Configuration.Upgrades.Abstractions
{
    public abstract class UpgradeConfiguration(ConfigFile cfg, string topSection, string enabledDescription) : IUpgradeConfiguration
    {
        [field: SyncedEntryField] public SyncedEntry<bool> Enabled { get; set; } = cfg.BindSyncedEntry(topSection, string.Format(LguConstants.ENABLED_FORMAT, topSection), true, enabledDescription);
        [field: SyncedEntryField] public SyncedEntry<int> MinimumSalePercentage { get; set; } = cfg.BindSyncedEntry(topSection, "Minimum Sale Percentage", 60, "Minimum percentage achieved when the upgrade goes on sale");
        [field: SyncedEntryField] public SyncedEntry<int> MaximumSalePercentage { get; set; } = cfg.BindSyncedEntry(topSection, "Maximum Sale Percentage", 90, "Maximum percentage achieved when the upgrade goes on sale");
        [field: SyncedEntryField] public SyncedEntry<string> OverrideName { get; set; } = cfg.BindSyncedEntry(topSection, string.Format(LguConstants.OVERRIDE_NAME_KEY_FORMAT, topSection), topSection);
        [field: SyncedEntryField] public SyncedEntry<string> ItemProgressionItems { get; set; } = cfg.BindSyncedEntry(topSection, LguConstants.ITEM_PROGRESSION_ITEMS_KEY, LguConstants.ITEM_PROGRESSION_ITEMS_DEFAULT, LguConstants.ITEM_PROGRESSION_ITEMS_DESCRIPTION);
        [field: SyncedEntryField] public SyncedEntry<PurchaseMode> PurchaseMode { get; set; } = cfg.BindSyncedEntry(topSection, "Purchase Mode", UI.TerminalNodes.PurchaseMode.Both, "Method of purchase allowed for the upgrade");
        [field: SyncedEntryField] public SyncedEntry<bool> Refundable { get; set; } = cfg.BindSyncedEntry(topSection, "Allow refund", false, "If enabled, you are able to refund a level from the selected upgrade.");
        [field: SyncedEntryField] public SyncedEntry<int> RefundPercentage { get; set; } = cfg.BindSyncedEntry(topSection, "Refund percentage", 100, "Amount of credits in percentage (%) given back from the refunded level");
    }
}
