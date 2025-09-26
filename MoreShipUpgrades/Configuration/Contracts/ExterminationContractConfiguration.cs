using BepInEx.Configuration;
using CSync.Extensions;
using CSync.Lib;
using MoreShipUpgrades.Configuration.Contracts.Abstractions;
using MoreShipUpgrades.Misc.Util;

namespace MoreShipUpgrades.Configuration.Contracts
{
	public class ExterminationContractConfiguration : ContractEntryConfiguration
	{
		[field:SyncedEntryField] public SyncedEntry<int> AmountSpawns { get; set; }
		public ExterminationContractConfiguration(ConfigFile config, string topSection)
		{
			Enabled = config.BindSyncedEntry(topSection, LguConstants.CONTRACT_EXTERMINATION_ENABLED_KEY, LguConstants.CONTRACT_EXTERMINATION_ENABLED_DEFAULT, LguConstants.CONTRACT_EXTERMINATION_ENABLED_DESCRIPTION);
			RewardValue = config.BindSyncedEntry(topSection, LguConstants.CONTRACT_BUG_REWARD_KEY, LguConstants.CONTRACT_BUG_REWARD_DEFAULT);
			AmountSpawns = config.BindSyncedEntry(topSection, LguConstants.EXTERMINATION_BUG_SPAWNS_KEY, LguConstants.EXTERMINATION_BUG_SPAWNS_DEFAULT, LguConstants.EXTERMINATION_BUG_SPAWNS_DESCRIPTION);
		}
	}
}
