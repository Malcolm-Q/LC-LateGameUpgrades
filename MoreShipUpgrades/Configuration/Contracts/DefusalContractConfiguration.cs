using BepInEx.Configuration;
using CSync.Extensions;
using MoreShipUpgrades.Configuration.Contracts.Abstractions;
using MoreShipUpgrades.Misc.Util;

namespace MoreShipUpgrades.Configuration.Contracts
{
	public class DataContractConfiguration : ContractEntryConfiguration
	{
		
		public DataContractConfiguration(ConfigFile config, string topSection)
		{
			Enabled = config.BindSyncedEntry(topSection, LguConstants.CONTRACT_DEFUSAL_ENABLED_KEY, LguConstants.CONTRACT_DEFUSAL_ENABLED_DEFAULT, LguConstants.CONTRACT_DEFUSAL_ENABLED_DESCRIPTION);
			RewardValue = config.BindSyncedEntry(topSection, LguConstants.CONTRACT_DEFUSAL_REWARD_KEY, LguConstants.CONTRACT_DEFUSAL_REWARD_DEFAULT);
		}
	}
}
