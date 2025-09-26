using BepInEx.Configuration;
using CSync.Extensions;
using CSync.Lib;
using MoreShipUpgrades.Configuration.Contracts.Abstractions;
using MoreShipUpgrades.Misc.Util;

namespace MoreShipUpgrades.Configuration.Contracts
{
	public class ExorcismContractConfiguration : ContractEntryConfiguration
	{
		[field:SyncedEntryField] public SyncedEntry<int> AmountSpawnsOnFail { get; set; }
		public ExorcismContractConfiguration(ConfigFile config, string topSection)
		{
			Enabled = config.BindSyncedEntry(topSection, LguConstants.CONTRACT_EXORCISM_ENABLED_KEY, LguConstants.CONTRACT_EXORCISM_ENABLED_DEFAULT, LguConstants.CONTRACT_EXORCISM_ENABLED_DESCRIPTION);
			RewardValue = config.BindSyncedEntry(topSection, LguConstants.CONTRACT_EXORCISM_REWARD_KEY, LguConstants.CONTRACT_EXORCISM_REWARD_DEFAULT);
			AmountSpawnsOnFail = config.BindSyncedEntry(topSection, LguConstants.EXORCISM_GHOST_SPAWN_KEY, LguConstants.EXORCISM_GHOST_SPAWN_DEFAULT, LguConstants.EXORCISM_GHOST_SPAWN_DESCRIPTION);
		}
	}
}
