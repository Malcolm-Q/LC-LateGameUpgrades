using BepInEx.Configuration;
using CSync.Extensions;
using CSync.Lib;
using MoreShipUpgrades.Configuration.Abstractions.OneTimeUpgrades;
using MoreShipUpgrades.Misc.Util;

namespace MoreShipUpgrades.Configuration.Custom
{
    public class BetterScannerUpgradeConfiguration : OneTimeIndividualUpgradeConfiguration
    {
        [field: SyncedEntryField] public SyncedEntry<int> SecondPrice {  get; set; }
        [field: SyncedEntryField] public SyncedEntry<int> ThirdPrice { get; set; }
        [field: SyncedEntryField] public SyncedEntry<float> OutsideNodesRangeIncrease { get; set; }
        [field: SyncedEntryField] public SyncedEntry<float> NodeRangeIncrease {  get; set; }
        [field: SyncedEntryField] public SyncedEntry<bool> SeeEnemiesThroughWalls {  get; set; }
        [field: SyncedEntryField] public SyncedEntry<bool> VerboseEnemies {  get; set; }
        public BetterScannerUpgradeConfiguration(ConfigFile cfg, string topSection, string enabledDescription, int defaultPrice) : base(cfg, topSection, enabledDescription, defaultPrice)
        {
            OutsideNodesRangeIncrease = cfg.BindSyncedEntry(topSection, LguConstants.BETTER_SCANNER_OUTSIDE_NODE_DISTANCE_INCREASE_KEY, LguConstants.BETTER_SCANNER_OUTSIDE_NODE_DISTANCE_INCREASE_DEFAULT, LguConstants.BETTER_SCANNER_OUTSIDE_NODE_DISTANCE_INCREASE_DESCRIPTION);
            NodeRangeIncrease = cfg.BindSyncedEntry(topSection, LguConstants.BETTER_SCANNER_NODE_DISTANCE_INCREASE_KEY, LguConstants.BETTER_SCANNER_NODE_DISTANCE_INCREASE_DEFAULT, LguConstants.BETTER_SCANNER_NODE_DISTANCE_INCREASE_DESCRIPTION);
            SeeEnemiesThroughWalls = cfg.BindSyncedEntry(topSection, LguConstants.BETTER_SCANNER_ENEMIES_THROUGH_WALLS_KEY, LguConstants.BETTER_SCANNER_ENEMIES_THROUGH_WALLS_DEFAULT, LguConstants.BETTER_SCANNER_ENEMIES_THROUGH_WALLS_DESCRIPTION);
            VerboseEnemies = cfg.BindSyncedEntry(topSection, LguConstants.BETTER_SCANNER_VERBOSE_ENEMIES_KEY, LguConstants.BETTER_SCANNER_VERBOSE_ENEMIES_DEFAULT, LguConstants.BETTER_SCANNER_VERBOSE_ENEMIES_DESCRIPTION);
            SecondPrice = cfg.BindSyncedEntry(topSection, LguConstants.BETTER_SCANNER_SECOND_TIER_PRICE_KEY, LguConstants.BETTER_SCANNER_SECOND_TIER_PRICE_DEFAULT, LguConstants.BETTER_SCANNER_SECOND_TIER_PRICE_DESCRIPTION);
            ThirdPrice = cfg.BindSyncedEntry(topSection, LguConstants.BETTER_SCANNER_THIRD_TIER_PRICE_KEY, LguConstants.BETTER_SCANNER_THIRD_TIER_PRICE_DEFAULT, LguConstants.BETTER_SCANNER_THIRD_TIER_PRICE_DESCRIPTION);
        }
    }
}
