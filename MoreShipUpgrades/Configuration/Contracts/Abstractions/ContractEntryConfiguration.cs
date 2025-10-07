using CSync.Lib;

namespace MoreShipUpgrades.Configuration.Contracts.Abstractions
{
	public abstract class ContractEntryConfiguration
	{
		[field: SyncedEntryField] public SyncedEntry<bool> Enabled { get; set; }
		[field: SyncedEntryField] public SyncedEntry<int> RewardValue { get; set; }
	}
}
