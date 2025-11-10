using BepInEx.Configuration;
using CSync.Extensions;
using CSync.Lib;
using MoreShipUpgrades.Misc.Util;

namespace MoreShipUpgrades.Configuration.Contracts
{
	public class ContractsConfiguration
	{
		[field: SyncedEntryField] public SyncedEntry<bool> Enabled { get; set; }
		[field: SyncedEntryField] public SyncedEntry<int> RandomPrice { get; set; }
		[field: SyncedEntryField] public SyncedEntry<int> SpecifyPrice { get; set; }
		[field: SyncedEntryField] public SyncedEntry<bool> RandomOnly { get; set; }
		[field: SyncedEntryField] public SyncedEntry<bool> FreeMoonsOnly {  get; set; }
		[field: SyncedEntryField] public SyncedEntry<bool> FurthestSpawnPossible { get; set; }
		[field: SyncedEntryField] public SyncedEntry<int> RewardQuotaMultiplier { get; set; }
		[field: SyncedEntryField] public SyncedEntry<string> BlacklistedMoons { get; set; }
		
		public DataContractConfiguration DataConfiguration { get; set; }
		public DefusalContractConfiguration DefusalConfiguration { get; set; }
		public ExorcismContractConfiguration ExorcismConfiguration { get; set; }
		public ExterminationContractConfiguration ExterminationConfiguration { get; set; }
		public ExtractionContractConfiguration ExtractionConfiguration { get; set; }

		public ContractsConfiguration(ConfigFile cfg, string topSection)
		{
			Enabled = cfg.BindSyncedEntry(topSection, LguConstants.ENABLE_CONTRACTS_KEY, LguConstants.ENABLE_CONTRACTS_DEFAULT);
			RandomOnly = cfg.BindSyncedEntry(topSection, LguConstants.CONTRACT_PROVIDE_RANDOM_ONLY_KEY, LguConstants.CONTRACT_PROVIDE_RANDOM_ONLY_DEFAULT, LguConstants.CONTRACT_PROVIDE_RANDOM_ONLY_DESCRIPTION);
			FreeMoonsOnly = cfg.BindSyncedEntry(topSection, LguConstants.CONTRACT_FREE_MOONS_ONLY_KEY, LguConstants.CONTRACT_FREE_MOONS_ONLY_DEFAULT, LguConstants.CONTRACT_FREE_MOONS_ONLY_DESCRIPTION);
			RandomPrice = cfg.BindSyncedEntry(topSection, LguConstants.CONTRACT_PRICE_KEY, LguConstants.CONTRACT_PRICE_DEFAULT);
			SpecifyPrice = cfg.BindSyncedEntry(topSection, LguConstants.CONTRACT_SPECIFY_PRICE_KEY, LguConstants.CONTRACT_SPECIFY_PRICE_DEFAULT);
			FurthestSpawnPossible = cfg.BindSyncedEntry(topSection, LguConstants.CONTRACT_FAR_FROM_MAIN_KEY, LguConstants.CONTRACT_FAR_FROM_MAIN_DEFAULT, LguConstants.CONTRACT_FAR_FROM_MAIN_DESCRIPTION);
			RewardQuotaMultiplier = cfg.BindSyncedEntry(topSection, LguConstants.CONTRACT_QUOTA_MULTIPLIER_KEY, LguConstants.CONTRACT_QUOTA_MULTIPLIER_DEFAULT, LguConstants.CONTRACT_QUOTA_MULTIPLIER_DESCRIPTION);
			BlacklistedMoons = cfg.BindSyncedEntry(topSection, "Blacklisted Moons", "", "Collection of moons' names separated by a comma (,) where you do not wish to obtain a contract on.\n This should be used when using custom moons where you do not wish to obtain a contract on.\n");

			ExtractionConfiguration = new ExtractionContractConfiguration(cfg, topSection);
			DataConfiguration = new DataContractConfiguration(cfg, topSection);
			DefusalConfiguration = new DefusalContractConfiguration(cfg, topSection);
			ExorcismConfiguration = new ExorcismContractConfiguration(cfg, topSection);
			ExterminationConfiguration = new ExterminationContractConfiguration(cfg, topSection);
		}
	}
}
