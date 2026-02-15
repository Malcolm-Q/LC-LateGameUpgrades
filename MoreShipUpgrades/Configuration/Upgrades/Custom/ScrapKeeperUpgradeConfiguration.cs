using BepInEx.Configuration;
using CSync.Extensions;
using CSync.Lib;
using MoreShipUpgrades.Configuration.Upgrades.Abstractions.TIerUpgrades;

namespace MoreShipUpgrades.Configuration.Upgrades.Custom
{
	public class ScrapKeeperUpgradeConfiguration : TierPrimitiveUpgradeConfiguration<int>
	{
		[field: SyncedEntryField] public SyncedEntry<bool> OnlyKeepItemsInShip { get; set; }
		public ScrapKeeperUpgradeConfiguration(ConfigFile cfg, string topSection, string enabledDescription, string defaultPrices) : base(cfg, topSection, enabledDescription, defaultPrices)
		{
			OnlyKeepItemsInShip = cfg.BindSyncedEntry(topSection, "Only Keep Previous Items", false, "If enabled, the upgrade will ignore the items that were stored in the same day and only apply its effect to the items kept from previous rounds.");
		}
	}
}
