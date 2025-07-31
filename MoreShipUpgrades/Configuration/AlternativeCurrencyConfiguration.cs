using BepInEx.Configuration;
using CSync.Extensions;
using CSync.Lib;
using MoreShipUpgrades.Misc.Util;
using MoreShipUpgrades.UI.TerminalNodes;

namespace MoreShipUpgrades.Configuration
{
	public class AlternativeCurrencyConfiguration
	{
		[field: SyncedEntryField] public SyncedEntry<bool> Enabled { get; set; }
		[field: SyncedEntryField] public SyncedEntry<bool> EnableGlobalPurchase { get; set; }
		[field: SyncedEntryField] public SyncedEntry<PurchaseMode> GlobalPurchaseMode {  get; set; }
		[field: SyncedEntryField] public SyncedEntry<int> MaximumAmountPerPlayer { get; set; }
		[field: SyncedEntryField] public SyncedEntry<bool> IncludeSpentInMaximum {  get; set; }
		[field: SyncedEntryField] public SyncedEntry<bool> BlockGainOperations { get; set; }
		[field: SyncedEntryField] public SyncedEntry<int> CreditsToCurrencyRatio { get; set; }
		[field: SyncedEntryField] public SyncedEntry<int> QuotaToCurrencyRatio { get; set; }
		[field: SyncedEntryField] public SyncedEntry<int> CreditsToCurrencyConversionRatio { get; set; }
		[field: SyncedEntryField] public SyncedEntry<int> CurrencyToCreditsConversionRatio { get; set; }

		public AlternativeCurrencyConfiguration(ConfigFile cfg){

			string topSection = LguConstants.ALTERNATIVE_CURRENCY_TOPSECTION;
			Enabled = cfg.BindSyncedEntry(topSection, LguConstants.ALTERNATIVE_CURRENCY_ENABLED_KEY, LguConstants.ALTERNATIVE_CURRENCY_ENABLED_DEFAULT, LguConstants.ALTERNATIVE_CURRENCY_ENABLED_DESCRIPTION);
			EnableGlobalPurchase = cfg.BindSyncedEntry(topSection, "Enable Global Purchase", false, "Toggle which replaces all upgrades' configuration settings for purchase mode with this sections entry");
			GlobalPurchaseMode = cfg.BindSyncedEntry(topSection, "Global Purchase Mode", PurchaseMode.Both, "If \'Enable Global Purchase\' is enabled, all upgrades will take this configuration to determine which currency is used to purchase them");
			MaximumAmountPerPlayer = cfg.BindSyncedEntry(topSection, "Maximum Amount per Player", -1, "If greater than zero, in any moment where the player would obtain some unit of alternative currency, it will be capped at the selected value");
			IncludeSpentInMaximum = cfg.BindSyncedEntry(topSection, "Include spent amount in Maximum amount", false, "If enabled, units of alternative currency spent in upgrades will count towards the maximum amount per player");
			BlockGainOperations = cfg.BindSyncedEntry(topSection, "Block Gain Operations when Maxed", false, "If enabled, operations such as refund or conversion will be blocked (thus the player cannot willingly lose credits) if the amount obtained from the operation exceeds the selected maximum value.");
			CreditsToCurrencyRatio = cfg.BindSyncedEntry(topSection, LguConstants.ALTERNATIVE_CURRENCY_CREDITS_TO_CURRENCY_RATIO_KEY, LguConstants.ALTERNATIVE_CURRENCY_CREDITS_TO_CURRENCY_RATIO_DEFAULT, LguConstants.ALTERNATIVE_CURRENCY_CREDITS_TO_CURRENCY_RATIO_DESCRIPTION);
			QuotaToCurrencyRatio = cfg.BindSyncedEntry(topSection, LguConstants.ALTERNATIVE_CURRENCY_QUOTA_TO_CURRENCY_RATIO_KEY, LguConstants.ALTERNATIVE_CURRENCY_QUOTA_TO_CURRENCY_RATIO_DEFAULT, LguConstants.ALTERNATIVE_CURRENCY_QUOTA_TO_CURRENCY_RATIO_DESCRIPTION);
			CreditsToCurrencyConversionRatio = cfg.BindSyncedEntry(topSection, LguConstants.ALTERNATIVE_CURRENCY_CREDITS_TO_CURRENCY_CONVERSION_RATIO_KEY, LguConstants.ALTERNATIVE_CURRENCY_CREDITS_TO_CURRENCY_CONVERSION_RATIO_DEFAULT, LguConstants.ALTERNATIVE_CURRENCY_CREDITS_TO_CURRENCY_CONVERSION_RATIO_DESCRIPTION);
			CurrencyToCreditsConversionRatio = cfg.BindSyncedEntry(topSection, LguConstants.ALTERNATIVE_CURRENCY_CURRENCY_TO_CREDITS_CONVERSION_RATIO_KEY, LguConstants.ALTERNATIVE_CURRENCY_CURRENCY_TO_CREDITS_CONVERSION_RATIO_DEFAULT, LguConstants.ALTERNATIVE_CURRENCY_CURRENCY_TO_CREDITS_CONVERSION_RATIO_DESCRIPTION);

		}
	}
}
