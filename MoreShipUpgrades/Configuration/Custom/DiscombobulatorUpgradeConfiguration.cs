using BepInEx.Configuration;
using CSync.Extensions;
using CSync.Lib;
using MoreShipUpgrades.Configuration.Abstractions.TIerUpgrades;
using MoreShipUpgrades.Misc.Util;

namespace MoreShipUpgrades.Configuration.Custom
{
    public class DiscombobulatorUpgradeConfiguration : TierPrimitiveUpgradeConfiguration<float>
    {
        [field: SyncedEntryField] public SyncedEntry<int> DamageLevel {  get; set; }
        [field :SyncedEntryField] public SyncedEntry<int> InitialDamage {  get; set; }
        [field: SyncedEntryField] public SyncedEntry<int> IncrementalDamage {  get; set; }
        [field: SyncedEntryField] public SyncedEntry<string> BlacklistEnemies {  get; set; }
        [field: SyncedEntryField] public SyncedEntry<bool> NotifyChat {  get; set; }
        [field: SyncedEntryField] public SyncedEntry<float> Radius { get; set; }
        [field: SyncedEntryField] public SyncedEntry<float> Cooldown {  get; set; }
        [field: SyncedEntryField] public SyncedEntry<bool> Absolute {  get; set; }
        public DiscombobulatorUpgradeConfiguration(ConfigFile cfg, string topSection, string enabledDescription, string defaultPrices) : base(cfg, topSection, enabledDescription, defaultPrices)
        {
            NotifyChat = cfg.BindSyncedEntry(topSection, LguConstants.DISCOMBOBULATOR_NOTIFY_CHAT_KEY, LguConstants.DISCOMBOBULATOR_NOTIFY_CHAT_DEFAULT);
            DamageLevel = cfg.BindSyncedEntry(topSection, LguConstants.DISCOMBOBULATOR_APPLY_DAMAGE_LEVEL_KEY, LguConstants.DISCOMBOBULATOR_APPLY_DAMAGE_LEVEL_DEFAULT, LguConstants.DISCOMBOBULATOR_APPLY_DAMAGE_LEVEL_DESCRIPTION);
            InitialDamage = cfg.BindSyncedEntry(topSection, LguConstants.DISCOMBOBULATOR_INITIAL_DAMAGE_KEY, LguConstants.DISCOMBOBULATOR_INITIAL_DAMAGE_DEFAULT, LguConstants.DISCOMBOBULATOR_INITIAL_DAMAGE_DESCRIPTION);
            IncrementalDamage = cfg.BindSyncedEntry(topSection, LguConstants.DISCOMBOBULATOR_INCREMENTAL_DAMAGE_KEY, LguConstants.DISCOMBOBULATOR_INCREMENTAL_DAMAGE_DEFAULT, LguConstants.DISCOMBOBULATOR_INCREMENTAL_DAMAGE_DESCRIPTION);
            BlacklistEnemies = cfg.BindSyncedEntry(topSection, "Blacklisted Enemies", "", "Enemies that aren't affected by Discombobulator. Either the internal name (EnemyType.enemyName) or the name shown in the scan node if it has one. Each enemy is separated by a comma (',')");
            Radius = cfg.BindSyncedEntry(topSection, LguConstants.DISCOMBOBULATOR_EFFECT_RADIUS_KEY, LguConstants.DISCOMBOBULATOR_EFFECT_RADIUS_DEFAULT);
            Cooldown = cfg.BindSyncedEntry(topSection, LguConstants.DISCOMBOBULATOR_COOLDOWN_KEY, LguConstants.DISCOMBOBULATOR_COOLDOWN_DEFAULT);
            Absolute = cfg.BindSyncedEntry(topSection, "Absolute Stun Duration", true, "If enabled, the stun time used during trigger is absolute and it is not influenced by the enemy stun resistance.");
        }
    }
}
