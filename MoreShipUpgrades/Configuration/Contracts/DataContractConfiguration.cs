using BepInEx.Configuration;
using CSync.Extensions;
using MoreShipUpgrades.Configuration.Contracts.Abstractions;
using MoreShipUpgrades.Misc.Util;

namespace MoreShipUpgrades.Configuration.Contracts
{
	public class DefusalContractConfiguration : ContractEntryConfiguration
	{
		
		public DefusalContractConfiguration(ConfigFile config, string topSection)
		{
			Enabled = config.BindSyncedEntry(topSection, LguConstants.CONTRACT_DATA_ENABLED_KEY, LguConstants.CONTRACT_DATA_ENABLED_DEFAULT, LguConstants.CONTRACT_DATA_ENABLED_DESCRIPTION);
			RewardValue = config.BindSyncedEntry(topSection, LguConstants.CONTRACT_DATA_REWARD_KEY, LguConstants.CONTRACT_DATA_REWARD_DEFAULT);
		}
	}
}
