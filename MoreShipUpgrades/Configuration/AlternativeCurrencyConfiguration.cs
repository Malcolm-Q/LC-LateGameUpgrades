using BepInEx.Configuration;
using CSync.Extensions;
using CSync.Lib;
using MoreShipUpgrades.Misc.Util;

namespace MoreShipUpgrades.Configuration
{
	public class AlternativeCurrencyConfiguration
	{
		[field: SyncedEntryField] public SyncedEntry<bool> Enabled { get; set; }
		[field: SyncedEntryField] public SyncedEntry<int> CreditsToCurrencyRatio { get; set; }
		[field: SyncedEntryField] public SyncedEntry<int> QuotaToCurrencyRatio { get; set; }
		[field: SyncedEntryField] public SyncedEntry<int> CreditsToCurrencyConversionRatio { get; set; }
		[field: SyncedEntryField] public SyncedEntry<int> CurrencyToCreditsConversionRatio { get; set; }

		public AlternativeCurrencyConfiguration(ConfigFile cfg){

			string topSection = LguConstants.ALTERNATIVE_CURRENCY_TOPSECTION;
			Enabled = cfg.BindSyncedEntry(topSection, LguConstants.ALTERNATIVE_CURRENCY_ENABLED_KEY, LguConstants.ALTERNATIVE_CURRENCY_ENABLED_DEFAULT, LguConstants.ALTERNATIVE_CURRENCY_ENABLED_DESCRIPTION);
			CreditsToCurrencyRatio = cfg.BindSyncedEntry(topSection, LguConstants.ALTERNATIVE_CURRENCY_CREDITS_TO_CURRENCY_RATIO_KEY, LguConstants.ALTERNATIVE_CURRENCY_CREDITS_TO_CURRENCY_RATIO_DEFAULT, LguConstants.ALTERNATIVE_CURRENCY_CREDITS_TO_CURRENCY_RATIO_DESCRIPTION);
			QuotaToCurrencyRatio = cfg.BindSyncedEntry(topSection, LguConstants.ALTERNATIVE_CURRENCY_QUOTA_TO_CURRENCY_RATIO_KEY, LguConstants.ALTERNATIVE_CURRENCY_QUOTA_TO_CURRENCY_RATIO_DEFAULT, LguConstants.ALTERNATIVE_CURRENCY_QUOTA_TO_CURRENCY_RATIO_DESCRIPTION);
			CreditsToCurrencyConversionRatio = cfg.BindSyncedEntry(topSection, LguConstants.ALTERNATIVE_CURRENCY_CREDITS_TO_CURRENCY_CONVERSION_RATIO_KEY, LguConstants.ALTERNATIVE_CURRENCY_CREDITS_TO_CURRENCY_CONVERSION_RATIO_DEFAULT, LguConstants.ALTERNATIVE_CURRENCY_CREDITS_TO_CURRENCY_CONVERSION_RATIO_DESCRIPTION);
			CurrencyToCreditsConversionRatio = cfg.BindSyncedEntry(topSection, LguConstants.ALTERNATIVE_CURRENCY_CURRENCY_TO_CREDITS_CONVERSION_RATIO_KEY, LguConstants.ALTERNATIVE_CURRENCY_CURRENCY_TO_CREDITS_CONVERSION_RATIO_DEFAULT, LguConstants.ALTERNATIVE_CURRENCY_CURRENCY_TO_CREDITS_CONVERSION_RATIO_DESCRIPTION);

		}
	}
}
