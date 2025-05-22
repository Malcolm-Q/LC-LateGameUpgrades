using BepInEx.Configuration;
using CSync.Extensions;
using CSync.Lib;
using MoreShipUpgrades.Configuration.Upgrades.Abstractions.OneTimeUpgrades;
using MoreShipUpgrades.Misc.Util;

namespace MoreShipUpgrades.Configuration.Upgrades.Custom
{
    public class SickBeatsUpgradeConfiguration : OneTimeIndividualUpgradeConfiguration
    {
        [field: SyncedEntryField] public SyncedEntry<bool> EnableDamage {  get; set; }
        [field: SyncedEntryField] public SyncedEntry<int> DamageBoost { get; set; }
        [field: SyncedEntryField] public SyncedEntry<bool> EnableDefense { get; set; }
        [field: SyncedEntryField] public SyncedEntry<float> DefenseBoost { get; set; }
        [field: SyncedEntryField] public SyncedEntry<bool> EnableSpeed { get; set; }
        [field: SyncedEntryField] public SyncedEntry<float> SpeedBoost {  get; set; }
        [field: SyncedEntryField] public SyncedEntry<bool> EnableStaminaRegen {  get; set; }
        [field: SyncedEntryField] public SyncedEntry<float> StaminaRegenBoost {  get; set; }
        [field: SyncedEntryField] public SyncedEntry<bool> ApplyStaminaDuringConsumption { get; set; }

        [field: SyncedEntryField] public SyncedEntry<bool> BoomboxAttract { get; set; }
        [field: SyncedEntryField] public SyncedEntry<float> Radius {  get; set; }
        public SickBeatsUpgradeConfiguration(ConfigFile cfg, string topSection, string enabledDescription, int defaultPrice) : base(cfg, topSection, enabledDescription, defaultPrice)
        {
            EnableDamage = cfg.BindSyncedEntry(topSection, LguConstants.SICK_BEATS_DAMAGE_KEY, LguConstants.SICK_BEATS_DAMAGE_DEFAULT);
            DamageBoost = cfg.BindSyncedEntry(topSection, LguConstants.SICK_BEATS_ADDITIONAL_DAMAGE_KEY, LguConstants.SICK_BEATS_ADDITIONAL_DAMAGE_DEFAULT);
            EnableStaminaRegen = cfg.BindSyncedEntry(topSection, LguConstants.SICK_BEATS_STAMINA_KEY, LguConstants.SICK_BEATS_STAMINA_DEFAULT);
            StaminaRegenBoost = cfg.BindSyncedEntry(topSection, LguConstants.SICK_BEATS_STAMINA_MULTIPLIER_KEY, LguConstants.SICK_BEATS_STAMINA_MULTIPLIER_DEFAULT, LguConstants.SICK_BEATS_STAMINA_MULTIPLIER_DESCRIPTION);
            EnableDefense = cfg.BindSyncedEntry(topSection, LguConstants.SICK_BEATS_DEFENSE_KEY, LguConstants.SICK_BEATS_DEFENSE_DEFAULT);
            DefenseBoost = cfg.BindSyncedEntry(topSection, LguConstants.SICK_BEATS_DEFENSE_MULTIPLIER_KEY, LguConstants.SICK_BEATS_DEFENSE_MULTIPLIER_DEFAULT, LguConstants.SICK_BEATS_DEFENSE_MULTIPLIER_DESCRIPTION);
            EnableSpeed = cfg.BindSyncedEntry(topSection, LguConstants.SICK_BEATS_SPEED_KEY, LguConstants.SICK_BEATS_SPEED_DEFAULT);
            SpeedBoost = cfg.BindSyncedEntry(topSection, LguConstants.SICK_BEATS_ADDITIONAL_SPEED_KEY, LguConstants.SICK_BEATS_ADDITIONAL_SPEED_DEFAULT);
            Radius = cfg.BindSyncedEntry(topSection, LguConstants.SICK_BEATS_EFFECT_RADIUS_KEY, LguConstants.SICK_BEATS_EFFECT_RADIUS_DEFAULT, LguConstants.SICK_BEATS_EFFECT_RADIUS_DESCRIPTION);
            ApplyStaminaDuringConsumption = cfg.BindSyncedEntry(topSection, LguConstants.SICK_BEATS_APPLY_STAMINA_CONSUMPTION_KEY, LguConstants.SICK_BEATS_APPLY_STAMINA_CONSUMPTION_DEFAULT, LguConstants.SICK_BEATS_APPLY_STAMINA_CONSUMPTION_DESCRIPTION);
            BoomboxAttract = cfg.BindSyncedEntry(topSection, LguConstants.SICK_BEATS_BOOMBOX_ATTRACT_SOUND_KEY, LguConstants.SICK_BEATS_BOOMBOX_ATTRACT_SOUND_DEFAULT, LguConstants.SICK_BEATS_BOOMBOX_ATTRACT_SOUND_DESCRIPTION);
        }
    }
}
