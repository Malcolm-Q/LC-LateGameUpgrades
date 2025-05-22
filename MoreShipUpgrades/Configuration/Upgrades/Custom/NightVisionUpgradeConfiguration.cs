using BepInEx.Configuration;
using CSync.Extensions;
using CSync.Lib;
using MoreShipUpgrades.Configuration.Upgrades.Abstractions.TIerUpgrades;
using MoreShipUpgrades.Misc.Util;

namespace MoreShipUpgrades.Configuration.Upgrades.Custom
{
    public class NightVisionUpgradeConfiguration : TierIndividualMultiplePrimitiveUpgradeConfiguration<float>
    {
        [field: SyncedEntryField] public SyncedEntry<int> ItemPrice {  get; set; }
        [field: SyncedEntryField] public SyncedEntry<string> LightColour { get; set; }
        [field: SyncedEntryField] public SyncedEntry<string> TextColour { get; set; }
        [field: SyncedEntryField] public SyncedEntry<string> BarColour {  get; set; }
        [field: SyncedEntryField] public SyncedEntry<float> StartupPercentage {  get; set; }
        [field: SyncedEntryField] public SyncedEntry<float> ExhaustTime { get; set; }
        [field: SyncedEntryField] public SyncedEntry<bool> LoseOnDeath {  get; set; }
        [field: SyncedEntryField] public SyncedEntry<bool> DropOnDeath { get; set; }
        public NightVisionUpgradeConfiguration(ConfigFile cfg, string topSection, string enabledDescription, string defaultPrices) : base(cfg, topSection, enabledDescription, defaultPrices)
        {
            ItemPrice = cfg.BindSyncedEntry(topSection, LguConstants.NIGHT_VISION_PRICE_KEY, LguConstants.NIGHT_VISION_PRICE_DEFAULT);
            LightColour = cfg.BindSyncedEntry(topSection, LguConstants.NIGHT_VISION_COLOR_KEY, LguConstants.NIGHT_VISION_COLOR_DEFAULT, LguConstants.NIGHT_VISION_COLOR_DESCRIPTION);
            TextColour = cfg.BindSyncedEntry(topSection, LguConstants.NIGHT_VISION_UI_TEXT_COLOR_KEY, LguConstants.NIGHT_VISION_UI_TEXT_COLOR_DEFAULT, LguConstants.NIGHT_VISION_UI_TEXT_COLOR_DESCRIPTION);
            BarColour = cfg.BindSyncedEntry(topSection, LguConstants.NIGHT_VISION_UI_BAR_COLOR_KEY, LguConstants.NIGHT_VISION_UI_BAR_COLOR_DEFAULT, LguConstants.NIGHT_VISION_UI_BAR_COLOR_DESCRIPTION);
            StartupPercentage = cfg.BindSyncedEntry(topSection, LguConstants.NIGHT_VISION_STARTUP_KEY, LguConstants.NIGHT_VISION_STARTUP_DEFAULT, LguConstants.NIGHT_VISION_STARTUP_DESCRIPTION);
            ExhaustTime = cfg.BindSyncedEntry(topSection, LguConstants.NIGHT_VISION_EXHAUST_KEY, LguConstants.NIGHT_VISION_EXHAUST_DEFAULT, LguConstants.NIGHT_VISION_EXHAUST_DESCRIPTION);
            LoseOnDeath = cfg.BindSyncedEntry(topSection, LguConstants.LOSE_NIGHT_VISION_ON_DEATH_KEY, LguConstants.LOSE_NIGHT_VISION_ON_DEATH_DEFAULT, LguConstants.LOSE_NIGHT_VISION_ON_DEATH_DESCRIPTION);
            DropOnDeath = cfg.BindSyncedEntry(topSection, LguConstants.NIGHT_VISION_DROP_ON_DEATH_KEY, LguConstants.NIGHT_VISION_DROP_ON_DEATH_DEFAULT, LguConstants.NIGHT_VISION_DROP_ON_DEATH_DESCRIPTION);
        }
    }
}
