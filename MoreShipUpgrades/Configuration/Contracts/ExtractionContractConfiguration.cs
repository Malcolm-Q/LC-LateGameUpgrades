using BepInEx.Configuration;
using CSync.Extensions;
using CSync.Lib;
using MoreShipUpgrades.Configuration.Contracts.Abstractions;
using MoreShipUpgrades.Misc.Util;

namespace MoreShipUpgrades.Configuration.Contracts
{
	public class ExtractionContractConfiguration : ContractEntryConfiguration
	{
		[field:SyncedEntryField] public SyncedEntry<int> AmountMedkits { get; set; }
		[field:SyncedEntryField] public SyncedEntry<float> ScavengerVolume { get; set; }
		[field:SyncedEntryField] public SyncedEntry<float> ScavengerWeight { get; set; }
		public ExtractionContractConfiguration(ConfigFile config, string topSection)
		{
			Enabled = config.BindSyncedEntry(topSection, LguConstants.CONTRACT_EXTRACTION_ENABLED_KEY, LguConstants.CONTRACT_EXTRACTION_ENABLED_DEFAULT, LguConstants.CONTRACT_EXTRACTION_ENABLED_DESCRIPTION);
			RewardValue = config.BindSyncedEntry(topSection, LguConstants.CONTRACT_EXTRACTION_REWARD_KEY, LguConstants.CONTRACT_EXTRACTION_REWARD_DEFAULT);
			AmountMedkits = config.BindSyncedEntry(topSection, LguConstants.EXTRACTION_MEDKIT_AMOUNT_KEY, LguConstants.EXTRACTION_MEDKIT_AMOUNT_DEFAULT);
			ScavengerVolume = config.BindSyncedEntry(topSection, LguConstants.EXTRACTION_SCAVENGER_SOUND_VOLUME_KEY, LguConstants.EXTRACTION_SCAVENGER_SOUND_VOLUME_DEFAULT, LguConstants.EXTRACTION_SCAVENGER_SOUND_VOLUME_DESCRIPTION);
			ScavengerWeight = config.BindSyncedEntry(topSection, LguConstants.EXTRACTION_SCAVENGER_WEIGHT_KEY, LguConstants.EXTRACTION_SCAVENGER_WEIGHT_DEFAULT, LguConstants.EXTRACTION_SCAVENGER_WEIGHT_DESCRIPTION);
		}
	}
}
