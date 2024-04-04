using BepInEx.Configuration;
using System;
using MoreShipUpgrades.UpgradeComponents.OneTimeUpgrades;
using MoreShipUpgrades.UpgradeComponents.TierUpgrades;
using MoreShipUpgrades.UpgradeComponents.Commands;
using MoreShipUpgrades.Misc.Upgrades;
using MoreShipUpgrades.UpgradeComponents.TierUpgrades.AttributeUpgrades;
using CSync.Lib;
using System.Runtime.Serialization;
using CSync.Extensions;
using LethalLib.Modules;
using MoreShipUpgrades.Managers;
using UnityEngine;


namespace MoreShipUpgrades.Misc
{
    [DataContract]
    public class PluginConfig : SyncedConfig<PluginConfig>
    {
        // enabled disabled
        [field: DataMember] public SyncedEntry<bool> CHARGING_BOOSTER_ENABLED { get; set; }
        [field: DataMember] public SyncedEntry<bool> MARKET_INFLUENCE_ENABLED { get; set; }
        [field: DataMember] public SyncedEntry<bool> BARGAIN_CONNECTIONS_ENABLED { get; set; }
        [field: DataMember] public SyncedEntry<bool> LETHAL_DEALS_ENABLED { get; set; }
        [field: DataMember] public SyncedEntry<bool> QUANTUM_DISRUPTOR_ENABLED { get; set; }
        [field: DataMember] public SyncedEntry<bool> CONTRACTS_ENABLED { get; set; }
        [field: DataMember] public SyncedEntry<bool> ADVANCED_TELE_ENABLED { get; set; }
        [field: DataMember] public SyncedEntry<bool> WEAK_TELE_ENABLED { get; set; }
        [field: DataMember] public SyncedEntry<bool> BEEKEEPER_ENABLED { get; set; }
        [field: DataMember] public SyncedEntry<bool> PROTEIN_ENABLED { get; set; }
        [field: DataMember] public SyncedEntry<bool> BIGGER_LUNGS_ENABLED { get; set; }
        [field: DataMember] public SyncedEntry<bool> BACK_MUSCLES_ENABLED { get; set; }
        [field: DataMember] public SyncedEntry<bool> NIGHT_VISION_ENABLED { get; set; }
        [field: DataMember] public SyncedEntry<bool> RUNNING_SHOES_ENABLED { get; set; }
        [field: DataMember] public SyncedEntry<bool> BETTER_SCANNER_ENABLED { get; set; }
        [field: DataMember] public SyncedEntry<bool> STRONG_LEGS_ENABLED { get; set; }
        [field: DataMember] public SyncedEntry<bool> DISCOMBOBULATOR_ENABLED { get; set; }
        [field: DataMember] public SyncedEntry<bool> MALWARE_BROADCASTER_ENABLED { get; set; }
        [field: DataMember] public SyncedEntry<bool> LIGHTNING_ROD_ENABLED { get; set; }
        [field: DataMember] public SyncedEntry<bool> HUNTER_ENABLED { get; set; }
        [field: DataMember] public SyncedEntry<bool> PLAYER_HEALTH_ENABLED { get; set; }
        [field: DataMember] public SyncedEntry<bool> PEEPER_ENABLED { get; set; }
        [field: DataMember] public SyncedEntry<bool> EXTEND_DEADLINE_ENABLED { get; set; }
        [field: DataMember] public SyncedEntry<bool> WHEELBARROW_ENABLED { get; set; }
        [field: DataMember] public SyncedEntry<bool> SCRAP_WHEELBARROW_ENABLED { get; set; }
        [field: DataMember] public SyncedEntry<bool> DOOR_HYDRAULICS_BATTERY_ENABLED { get; set; }
        [field: DataMember] public SyncedEntry<bool> SCRAP_INSURANCE_ENABLED { get; set; }
        [field: DataMember] public SyncedEntry<bool> FASTER_DROP_POD_ENABLED { get; set; }
        [field: DataMember] public SyncedEntry<bool> SIGURD_ENABLED { get; set; }
        [field: DataMember] public SyncedEntry<bool> SIGURD_LAST_DAY_ENABLED { get; set; }
        [field: DataMember] public SyncedEntry<bool> EFFICIENT_ENGINES_ENABLED { get; set; }
        [field: DataMember] public SyncedEntry<bool> CLIMBING_GLOVES_ENABLED {  get; set; }
        [field: DataMember] public SyncedEntry<bool> WEATHER_PROBE_ENABLED { get; set; }
        [field: DataMember] public SyncedEntry<bool> LITHIUM_BATTERIES_ENABLED { get; set; }
        // individual or shared
        [field: DataMember] public SyncedEntry<bool> BEEKEEPER_INDIVIDUAL { get; set; }
        [field: DataMember] public SyncedEntry<bool> PROTEIN_INDIVIDUAL { get; set; }
        [field: DataMember] public SyncedEntry<bool> BIGGER_LUNGS_INDIVIDUAL { get; set; }
        [field: DataMember] public SyncedEntry<bool> BACK_MUSCLES_INDIVIDUAL { get; set; }
        [field: DataMember] public SyncedEntry<bool> NIGHT_VISION_INDIVIDUAL { get; set; }
        [field: DataMember] public SyncedEntry<bool> PLAYER_HEALTH_INDIVIDUAL { get; set; }

        [field: DataMember] public SyncedEntry<bool> RUNNING_SHOES_INDIVIDUAL { get; set; }
        [field: DataMember] public SyncedEntry<bool> BETTER_SCANNER_INDIVIDUAL { get; set; }
        [field: DataMember] public SyncedEntry<bool> STRONG_LEGS_INDIVIDUAL { get; set; }
        [field: DataMember] public SyncedEntry<bool> DISCOMBOBULATOR_INDIVIDUAL { get; set; }
        [field: DataMember] public SyncedEntry<bool> MALWARE_BROADCASTER_INDIVIDUAL { get; set; }
        [field: DataMember] public SyncedEntry<bool> INTERN_INDIVIDUAL { get; set; }
        [field: DataMember] public SyncedEntry<bool> LOCKSMITH_INDIVIDUAL { get; set; }
        [field: DataMember] public SyncedEntry<bool> CLIMBING_GLOVES_INDIVIDUAL { get; set; }
        [field: DataMember] public SyncedEntry<bool> LITHIUM_BATTERIES_INDIVIDUAL { get; set; }
        // prices
        [field: DataMember] public SyncedEntry<int> CLIMBING_GLOVES_PRICE {  get; set; }
        [field: DataMember] public SyncedEntry<int> WEATHER_PROBE_PRICE {  get; set; }
        [field: DataMember] public SyncedEntry<int> WEATHER_PROBE_PICKED_WEATHER_PRICE {  get; set; }
        [field: DataMember] public SyncedEntry<int> CHARGING_BOOSTER_PRICE { get; set; }
        [field: DataMember] public SyncedEntry<int> MARKET_INFLUENCE_PRICE { get; set; }
        [field: DataMember] public SyncedEntry<int> BARGAIN_CONNECTIONS_PRICE { get; set; }
        [field: DataMember] public SyncedEntry<int> LETHAL_DEALS_PRICE { get; set; }
        [field: DataMember] public SyncedEntry<float> QUANTUM_DISRUPTOR_INITIAL_MULTIPLIER { get; set; }
        [field: DataMember] public SyncedEntry<float> QUANTUM_DISRUPTOR_INCREMENTAL_MULTIPLIER { get; set; }
        [field: DataMember] public SyncedEntry<string> QUANTUM_DISRUPTOR_PRICES { get; set; }
        [field: DataMember] public SyncedEntry<int> QUANTUM_DISRUPTOR_PRICE { get; set; }
        [field: DataMember] public SyncedEntry<int> PEEPER_PRICE { get; set; }
        [field: DataMember] public SyncedEntry<int> HUNTER_PRICE { get; set; }
        [field: DataMember] public SyncedEntry<int> ADVANCED_TELE_PRICE { get; set; }
        [field: DataMember] public SyncedEntry<int> WEAK_TELE_PRICE { get; set; }
        [field: DataMember] public SyncedEntry<int> BEEKEEPER_PRICE { get; set; }
        [field: DataMember] public SyncedEntry<int> PROTEIN_PRICE { get; set; }
        [field: DataMember] public SyncedEntry<int> BIGGER_LUNGS_PRICE { get; set; }
        [field: DataMember] public SyncedEntry<int> BACK_MUSCLES_PRICE { get; set; }
        [field: DataMember] public SyncedEntry<int> NIGHT_VISION_PRICE { get; set; }
        [field: DataMember] public SyncedEntry<int> RUNNING_SHOES_PRICE { get; set; }
        [field: DataMember] public SyncedEntry<int> BETTER_SCANNER_PRICE { get; set; }
        [field: DataMember] public SyncedEntry<int> STRONG_LEGS_PRICE { get; set; }
        [field: DataMember] public SyncedEntry<int> DISCOMBOBULATOR_PRICE { get; set; }
        [field: DataMember] public SyncedEntry<int> MALWARE_BROADCASTER_PRICE { get; set; }
        [field: DataMember] public SyncedEntry<int> WALKIE_PRICE { get; set; }
        [field: DataMember] public SyncedEntry<int> LIGHTNING_ROD_PRICE { get; set; }
        [field: DataMember] public SyncedEntry<int> PLAYER_HEALTH_PRICE { get; set; }
        [field: DataMember] public SyncedEntry<int> EXTEND_DEADLINE_PRICE { get; set; }
        [field: DataMember] public SyncedEntry<int> CONTRACT_PRICE { get; set; }
        [field: DataMember] public SyncedEntry<int> CONTRACT_SPECIFY_PRICE { get; set; }
        [field: DataMember] public SyncedEntry<int> WHEELBARROW_PRICE { get; set; }
        [field: DataMember] public SyncedEntry<int> DOOR_HYDRAULICS_BATTERY_PRICE { get; set; }
        [field: DataMember] public SyncedEntry<int> SCRAP_INSURANCE_PRICE { get; set; }
        [field: DataMember] public SyncedEntry<int> FASTER_DROP_POD_PRICE { get; set; }
        [field: DataMember] public SyncedEntry<int> SIGURD_PRICE { get; set; }
        [field: DataMember] public SyncedEntry<int> EFFICIENT_ENGINES_PRICE { get; set; }
        [field: DataMember] public SyncedEntry<int> LITHIUM_BATTERIES_PRICE {  get; set; }

        // attributes
        [field: DataMember] public SyncedEntry<string> LITHIUM_BATTERIES_PRICES { get; set; }
        [field: DataMember] public SyncedEntry<int> LITHIUM_BATTERIES_INITIAL_MULTIPLIER { get; set; }
        [field: DataMember] public SyncedEntry<int> LITHIUM_BATTERIES_INCREMENTAL_MULTIPLIER {  get; set; }
        [field: DataMember] public SyncedEntry<string> EFFICIENT_ENGINES_PRICES {  get; set; }
        [field: DataMember] public SyncedEntry<int> EFFICIENT_ENGINES_INITIAL_DISCOUNT {  get; set; }
        [field: DataMember] public SyncedEntry<int> EFFICIENT_ENGINES_INCREMENTAL_DISCOUNT { get; set; }
        [field: DataMember] public SyncedEntry<string> CLIMBING_GLOVES_PRICES {  get; set; }
        [field: DataMember] public SyncedEntry<float> INITIAL_CLIMBING_SPEED_BOOST {  get; set; }
        [field: DataMember] public SyncedEntry<float> INCREMENTAL_CLIMBING_SPEED_BOOST { get; set; }
        [field: DataMember] public SyncedEntry<string> CHARGING_BOOSTER_PRICES { get; set; }
        [field: DataMember] public SyncedEntry<float> CHARGING_BOOSTER_COOLDOWN { get; set; }
        [field: DataMember] public SyncedEntry<float> CHARGING_BOOSTER_INCREMENTAL_COOLDOWN_DECREASE { get; set; }
        [field: DataMember] public SyncedEntry<int> CHARGING_BOOSTER_CHARGE_PERCENTAGE { get; set; }
        [field: DataMember] public SyncedEntry<float> BIGGER_LUNGS_STAMINA_REGEN_INCREASE { get; set; }
        [field: DataMember] public SyncedEntry<float> BIGGER_LUNGS_STAMINA_REGEN_INCREMENTAL_INCREASE { get; set; }
        [field: DataMember] public SyncedEntry<int> BIGGER_LUNGS_STAMINA_REGEN_APPLY_LEVEL { get; set; }
        [field: DataMember] public SyncedEntry<int> BIGGER_LUNGS_JUMP_STAMINA_APPLY_LEVEL { get; set; }
        [field: DataMember] public SyncedEntry<float> BIGGER_LUNGS_JUMP_STAMINA_COST_DECREASE { get; set; }
        [field: DataMember] public SyncedEntry<float> BIGGER_LUNGS_JUMP_STAMINA_COST_INCREMENTAL_DECREASE { get; set; }
        [field: DataMember] public SyncedEntry<int> PROTEIN_INCREMENT { get; set; }
        [field: DataMember] public SyncedEntry<bool> KEEP_ITEMS_ON_TELE { get; set; }
        [field: DataMember] public SyncedEntry<float> SPRINT_TIME_INCREASE_UNLOCK { get; set; }
        [field: DataMember] public SyncedEntry<float> MOVEMENT_SPEED_UNLOCK { get; set; }
        [field: DataMember] public SyncedEntry<float> JUMP_FORCE_UNLOCK { get; set; }
        [field: DataMember] public SyncedEntry<bool> DESTROY_TRAP { get; set; }
        [field: DataMember] public SyncedEntry<float> DISARM_TIME { get; set; }
        [field: DataMember] public SyncedEntry<bool> EXPLODE_TRAP { get; set; }
        [field: DataMember] public SyncedEntry<float> CARRY_WEIGHT_REDUCTION { get; set; }
        [field: DataMember] public SyncedEntry<float> NODE_DISTANCE_INCREASE { get; set; }
        [field: DataMember] public SyncedEntry<float> SHIP_AND_ENTRANCE_DISTANCE_INCREASE { get; set; }
        [field: DataMember] public SyncedEntry<float> NOISE_REDUCTION { get; set; }
        [field: DataMember] public SyncedEntry<float> DISCOMBOBULATOR_COOLDOWN { get; set; }
        [field: DataMember] public SyncedEntry<float> ADV_CHANCE_TO_BREAK { get; set; }
        [field: DataMember] public SyncedEntry<bool> ADV_KEEP_ITEMS_ON_TELE { get; set; }
        [field: DataMember] public SyncedEntry<float> CHANCE_TO_BREAK { get; set; }
        [field: DataMember] public SyncedEntry<float> BEEKEEPER_DAMAGE_MULTIPLIER { get; set; }
        [field: DataMember] public SyncedEntry<float> BEEKEEPER_DAMAGE_MULTIPLIER_INCREMENT { get; set; }
        [field: DataMember] public SyncedEntry<float> DISCOMBOBULATOR_RADIUS { get; set; }
        [field: DataMember] public SyncedEntry<float> DISCOMBOBULATOR_STUN_DURATION { get; set; }
        [field: DataMember] public SyncedEntry<bool> DISCOMBOBULATOR_NOTIFY_CHAT { get; set; }
        [field: DataMember] public SyncedEntry<string> NIGHT_VIS_COLOR { get; set; }
        [field: DataMember] public SyncedEntry<string> NIGHT_VIS_UI_TEXT_COLOR {  get; set; }
        [field: DataMember] public SyncedEntry<string> NIGHT_VIS_UI_BAR_COLOR { get; set; }
        [field: DataMember] public SyncedEntry<float> NIGHT_VIS_DRAIN_SPEED { get; set; }
        [field: DataMember] public SyncedEntry<float> NIGHT_VIS_REGEN_SPEED { get; set; }
        [field: DataMember] public SyncedEntry<float> NIGHT_BATTERY_MAX { get; set; }
        [field: DataMember] public SyncedEntry<float> NIGHT_VIS_RANGE { get; set; }
        [field: DataMember] public SyncedEntry<float> NIGHT_VIS_RANGE_INCREMENT { get; set; }
        [field: DataMember] public SyncedEntry<float> NIGHT_VIS_INTENSITY { get; set; }
        [field: DataMember] public SyncedEntry<float> NIGHT_VIS_INTENSITY_INCREMENT { get; set; }
        [field: DataMember] public SyncedEntry<float> NIGHT_VIS_STARTUP { get; set; }
        [field: DataMember] public SyncedEntry<float> NIGHT_VIS_EXHAUST { get; set; }
        [field: DataMember] public SyncedEntry<float> NIGHT_VIS_DRAIN_INCREMENT { get; set; }
        [field: DataMember] public SyncedEntry<float> NIGHT_VIS_REGEN_INCREMENT { get; set; }
        [field: DataMember] public SyncedEntry<float> NIGHT_VIS_BATTERY_INCREMENT { get; set; }
        [field: DataMember] public SyncedEntry<float> CARRY_WEIGHT_INCREMENT { get; set; }
        [field: DataMember] public SyncedEntry<float> MOVEMENT_INCREMENT { get; set; }
        [field: DataMember] public SyncedEntry<float> SPRINT_TIME_INCREMENT { get; set; }
        [field: DataMember] public SyncedEntry<float> JUMP_FORCE_INCREMENT { get; set; }
        [field: DataMember] public SyncedEntry<float> DISCOMBOBULATOR_INCREMENT { get; set; }
        [field: DataMember] public SyncedEntry<int> INTERN_PRICE { get; set; }
        [field: DataMember] public SyncedEntry<int> LOCKSMITH_PRICE { get; set; }
        [field: DataMember] public SyncedEntry<bool> INTERN_ENABLED { get; set; }
        [field: DataMember] public SyncedEntry<bool> LOCKSMITH_ENABLED { get; set; }
        [field: DataMember] public SyncedEntry<float> SALE_PERC { get; set; }
        [field: DataMember] public SyncedEntry<bool> LOSE_NIGHT_VIS_ON_DEATH { get; set; }
        [field: DataMember] public SyncedEntry<bool> NIGHT_VISION_DROP_ON_DEATH { get; set; }
        [field: DataMember] public SyncedEntry<string> BEEKEEPER_UPGRADE_PRICES { get; set; }
        [field: DataMember] public SyncedEntry<float> BEEKEEPER_HIVE_VALUE_INCREASE { get; set; }
        [field: DataMember] public SyncedEntry<string> BACK_MUSCLES_UPGRADE_PRICES { get; set; }
        [field: DataMember] public SyncedEntry<string> BIGGER_LUNGS_UPGRADE_PRICES { get; set; }
        [field: DataMember] public SyncedEntry<string> NIGHT_VISION_UPGRADE_PRICES { get; set; }
        [field: DataMember] public SyncedEntry<string> RUNNING_SHOES_UPGRADE_PRICES { get; set; }
        [field: DataMember] public SyncedEntry<string> STRONG_LEGS_UPGRADE_PRICES { get; set; }
        [field: DataMember] public SyncedEntry<string> DISCO_UPGRADE_PRICES { get; set; }
        [field: DataMember] public SyncedEntry<string> PROTEIN_UPGRADE_PRICES { get; set; }
        [field: DataMember] public SyncedEntry<string> PLAYER_HEALTH_UPGRADE_PRICES { get; set; }
        [field: DataMember] public SyncedEntry<string> HUNTER_UPGRADE_PRICES { get; set; }
        [field: DataMember] public SyncedEntry<string> HUNTER_SAMPLE_TIERS { get; set; }
        [field: DataMember] public SyncedEntry<bool> SHARED_UPGRADES { get; set; }
        [field: DataMember] public SyncedEntry<bool> WALKIE_ENABLED { get; set; }
        [field: DataMember] public SyncedEntry<bool> WALKIE_INDIVIDUAL { get; set; }
        [field: DataMember] public SyncedEntry<int> PROTEIN_UNLOCK_FORCE { get; set; }
        [field: DataMember] public SyncedEntry<float> PROTEIN_CRIT_CHANCE { get; set; }
        [field: DataMember] public SyncedEntry<int> BETTER_SCANNER_PRICE2 { get; set; }
        [field: DataMember] public SyncedEntry<int> BETTER_SCANNER_PRICE3 { get; set; }
        [field: DataMember] public SyncedEntry<bool> BETTER_SCANNER_ENEMIES { get; set; }
        [field: DataMember] public SyncedEntry<bool> INTRO_ENABLED { get; set; }
        [field: DataMember] public SyncedEntry<bool> LIGHTNING_ROD_ACTIVE { get; set; }
        [field: DataMember] public SyncedEntry<float> LIGHTNING_ROD_DIST { get; set; }
        [field: DataMember] public SyncedEntry<bool> PAGER_ENABLED { get; set; }
        [field: DataMember] public SyncedEntry<int> PAGER_PRICE { get; set; }
        [field: DataMember] public SyncedEntry<bool> VERBOSE_ENEMIES { get; set; }
        [field: DataMember] public SyncedEntry<int> PLAYER_HEALTH_ADDITIONAL_HEALTH_UNLOCK { get; set; }
        [field: DataMember] public SyncedEntry<int> PLAYER_HEALTH_ADDITIONAL_HEALTH_INCREMENT { get; set; }
        [field: DataMember] public SyncedEntry<bool> MEDKIT_ENABLED { get; set; }
        [field: DataMember] public SyncedEntry<int> MEDKIT_PRICE { get; set; }
        [field: DataMember] public SyncedEntry<int> MEDKIT_HEAL_VALUE { get; set; }
        [field: DataMember] public SyncedEntry<int> MEDKIT_USES { get; set; }
        [field: DataMember] public SyncedEntry<int> DIVEKIT_PRICE { get; set; }
        [field: DataMember] public SyncedEntry<bool> DIVEKIT_ENABLED { get; set; }
        [field: DataMember] public SyncedEntry<float> DIVEKIT_WEIGHT { get; set; }
        [field: DataMember] public SyncedEntry<bool> DIVEKIT_TWO_HANDED { get; set; }
        [field: DataMember] public SyncedEntry<int> DISCOMBOBULATOR_DAMAGE_LEVEL { get; set; }
        [field: DataMember] public SyncedEntry<int> DISCOMBOBULATOR_INITIAL_DAMAGE { get; set; }
        [field: DataMember] public SyncedEntry<int> DISCOMBOBULATOR_DAMAGE_INCREASE { get; set; }
        [field: DataMember] public SyncedEntry<float> STRONG_LEGS_REDUCE_FALL_DAMAGE_MULTIPLIER { get; set; }
        [field: DataMember] public SyncedEntry<bool> KEEP_UPGRADES_AFTER_FIRED_CUTSCENE { get; set; }
        [field: DataMember] public SyncedEntry<int> CONTRACT_BUG_REWARD { get; set; }
        [field: DataMember] public SyncedEntry<int> CONTRACT_EXOR_REWARD { get; set; }
        [field: DataMember] public SyncedEntry<int> CONTRACT_DEFUSE_REWARD { get; set; }
        [field: DataMember] public SyncedEntry<int> CONTRACT_BUG_SPAWNS { get; set; }
        [field: DataMember] public SyncedEntry<int> CONTRACT_EXTRACT_REWARD { get; set; }
        [field: DataMember] public SyncedEntry<float> CONTRACT_EXTRACT_WEIGHT { get; set; }
        [field: DataMember] public SyncedEntry<int> CONTRACT_DATA_REWARD { get; set; }
        [field: DataMember] public SyncedEntry<bool> BEATS_INDIVIDUAL { get; set; }
        [field: DataMember] public SyncedEntry<bool> BEATS_ENABLED { get; set; }
        [field: DataMember] public SyncedEntry<int> BEATS_PRICE { get; set; }
        [field: DataMember] public SyncedEntry<bool> BEATS_SPEED { get; set; }
        [field: DataMember] public SyncedEntry<bool> BEATS_DMG { get; set; }
        [field: DataMember] public SyncedEntry<bool> BEATS_STAMINA { get; set; }
        [field: DataMember] public SyncedEntry<float> BEATS_STAMINA_CO { get; set; }
        [field: DataMember] public SyncedEntry<bool> BEATS_DEF { get; set; }
        [field: DataMember] public SyncedEntry<float> BEATS_DEF_CO { get; set; }
        [field: DataMember] public SyncedEntry<float> BEATS_SPEED_INC { get; set; }
        [field: DataMember] public SyncedEntry<float> BEATS_RADIUS { get; set; }
        [field: DataMember] public SyncedEntry<int> BEATS_DMG_INC { get; set; }
        [field: DataMember] public SyncedEntry<bool> HELMET_ENABLED { get; set; }
        [field: DataMember] public SyncedEntry<int> HELMET_PRICE { get; set; }
        [field: DataMember] public SyncedEntry<int> HELMET_HITS_BLOCKED { get; set; }
        [field: DataMember] public SyncedEntry<int> SNARE_FLEA_SAMPLE_MINIMUM_VALUE { get; set; }
        [field: DataMember] public SyncedEntry<int> SNARE_FLEA_SAMPLE_MAXIMUM_VALUE { get; set; }
        [field: DataMember] public SyncedEntry<int> BUNKER_SPIDER_SAMPLE_MINIMUM_VALUE { get; set; }
        [field: DataMember] public SyncedEntry<int> BUNKER_SPIDER_SAMPLE_MAXIMUM_VALUE { get; set; }
        [field: DataMember] public SyncedEntry<int> HOARDING_BUG_SAMPLE_MINIMUM_VALUE { get; set; }
        [field: DataMember] public SyncedEntry<int> HOARDING_BUG_SAMPLE_MAXIMUM_VALUE { get; set; }
        [field: DataMember] public SyncedEntry<int> BRACKEN_SAMPLE_MINIMUM_VALUE { get; set; }
        [field: DataMember] public SyncedEntry<int> BRACKEN_SAMPLE_MAXIMUM_VALUE { get; set; }
        [field: DataMember] public SyncedEntry<int> EYELESS_DOG_SAMPLE_MINIMUM_VALUE { get; set; }
        [field: DataMember] public SyncedEntry<int> EYELESS_DOG_SAMPLE_MAXIMUM_VALUE { get; set; }
        [field: DataMember] public SyncedEntry<int> BABOON_HAWK_SAMPLE_MINIMUM_VALUE { get; set; }
        [field: DataMember] public SyncedEntry<int> BABOON_HAWK_SAMPLE_MAXIMUM_VALUE { get; set; }
        [field: DataMember] public SyncedEntry<int> THUMPER_SAMPLE_MINIMUM_VALUE { get; set; }
        [field: DataMember] public SyncedEntry<int> THUMPER_SAMPLE_MAXIMUM_VALUE { get; set; }
        [field: DataMember] public SyncedEntry<int> CONTRACT_GHOST_SPAWN { get; set; }
        [field: DataMember] public SyncedEntry<string> WHEELBARROW_RESTRICTION_MODE { get; set; }
        [field: DataMember] public SyncedEntry<int> WHEELBARROW_MAXIMUM_AMOUNT_ITEMS { get; set; }
        [field: DataMember] public SyncedEntry<float> WHEELBARROW_MAXIMUM_WEIGHT_ALLOWED { get; set; }
        [field: DataMember] public SyncedEntry<float> WHEELBARROW_WEIGHT_REDUCTION_MULTIPLIER { get; set; }
        [field: DataMember] public SyncedEntry<float> WHEELBARROW_WEIGHT { get; set; }
        [field: DataMember] public SyncedEntry<float> WHEELBARROW_LOOK_SENSITIVITY_DRAWBACK { get; set; }
        [field: DataMember] public SyncedEntry<float> WHEELBARROW_MOVEMENT_SLOPPY { get; set; }
        [field: DataMember] public SyncedEntry<float> WHEELBARROW_NOISE_RANGE { get; set; }
        [field: DataMember] public SyncedEntry<bool> WHEELBARROW_PLAY_NOISE { get; set; }
        [field: DataMember] public SyncedEntry<string> SCRAP_WHEELBARROW_RESTRICTION_MODE { get; set; }
        [field: DataMember] public SyncedEntry<int> SCRAP_WHEELBARROW_MAXIMUM_AMOUNT_ITEMS { get; set; }
        [field: DataMember] public SyncedEntry<float> SCRAP_WHEELBARROW_MAXIMUM_WEIGHT_ALLOWED { get; set; }
        [field: DataMember] public SyncedEntry<float> SCRAP_WHEELBARROW_WEIGHT_REDUCTION_MULTIPLIER { get; set; }
        [field: DataMember] public SyncedEntry<float> SCRAP_WHEELBARROW_WEIGHT { get; set; }
        [field: DataMember] public SyncedEntry<float> SCRAP_WHEELBARROW_LOOK_SENSITIVITY_DRAWBACK { get; set; }
        [field: DataMember] public SyncedEntry<float> SCRAP_WHEELBARROW_NOISE_RANGE { get; set; }
        [field: DataMember] public SyncedEntry<int> SCRAP_WHEELBARROW_MINIMUM_VALUE { get; set; }
        [field: DataMember] public SyncedEntry<int> SCRAP_WHEELBARROW_MAXIMUM_VALUE { get; set; }
        [field: DataMember] public SyncedEntry<float> SCRAP_WHEELBARROW_MOVEMENT_SLOPPY { get; set; }
        [field: DataMember] public SyncedEntry<float> SCRAP_WHEELBARROW_RARITY { get; set; }
        [field: DataMember] public SyncedEntry<bool> SCRAP_WHEELBARROW_PLAY_NOISE { get; set; }
        [field: DataMember] public SyncedEntry<float> SCAV_VOLUME { get; set; }
        [field: DataMember] public SyncedEntry<bool> CONTRACT_FREE_MOONS_ONLY { get; set; }
        [field: DataMember] public SyncedEntry<string> DOOR_HYDRAULICS_BATTERY_PRICES { get; set; }
        [field: DataMember] public SyncedEntry<float> DOOR_HYDRAULICS_BATTERY_INITIAL { get; set; }
        [field: DataMember] public SyncedEntry<float> DOOR_HYDRAULICS_BATTERY_INCREMENTAL { get; set; }
        [field: DataMember] public SyncedEntry<bool> DATA_CONTRACT { get; set; }
        [field: DataMember] public SyncedEntry<bool> EXTERMINATOR_CONTRACT { get; set; }
        [field: DataMember] public SyncedEntry<bool> EXORCISM_CONTRACT { get; set; }
        [field: DataMember] public SyncedEntry<bool> EXTRACTION_CONTRACT { get; set; }
        [field: DataMember] public SyncedEntry<bool> DEFUSAL_CONTRACT { get; set; }
        [field: DataMember] public SyncedEntry<bool> MAIN_OBJECT_FURTHEST { get; set; }
        [field: DataMember] public SyncedEntry<string> MARKET_INFLUENCE_PRICES { get; set; }
        [field: DataMember] public SyncedEntry<int> MARKET_INFLUENCE_INITIAL_PERCENTAGE { get; set; }
        [field: DataMember] public SyncedEntry<int> MARKET_INFLUENCE_INCREMENTAL_PERCENTAGE { get; set; }
        [field: DataMember] public SyncedEntry<int> BARGAIN_CONNECTIONS_INITIAL_ITEM_AMOUNT { get; set; }
        [field: DataMember] public SyncedEntry<int> BARGAIN_CONNECTIONS_INCREMENTAL_ITEM_AMOUNT { get; set; }
        [field: DataMember] public SyncedEntry<string> BARGAIN_CONNECTIONS_PRICES { get; set; }
        [field: DataMember] public SyncedEntry<float> FASTER_DROP_POD_TIMER { get; set; }
        [field: DataMember] public SyncedEntry<float> FASTER_DROP_POD_INITIAL_TIMER { get; set; }
        [field: DataMember] public SyncedEntry<int> EXTRACTION_CONTRACT_AMOUNT_MEDKITS { get; set; }
        [field: DataMember] public SyncedEntry<int> CONTRACT_REWARD_QUOTA_MULTIPLIER { get; set; }
        [field: DataMember] public SyncedEntry<bool> SHOW_UPGRADES_CHAT { get; set; }
        [field: DataMember] public SyncedEntry<float> SIGURD_CHANCE { get; set; }
        [field: DataMember] public SyncedEntry<float> SIGURD_LAST_DAY_CHANCE { get; set; }
        [field: DataMember] public SyncedEntry<float> SIGURD_PERCENT { get; set; }
        [field: DataMember] public SyncedEntry<float> SIGURD_LAST_DAY_PERCENT { get; set; }
        [field: DataMember] public SyncedEntry<bool> SALE_APPLY_ONCE { get; set; }
        [field: DataMember] public SyncedEntry<bool> WEATHER_PROBE_ALWAYS_CLEAR {  get; set; }

        public PluginConfig(ConfigFile cfg) : base(Metadata.GUID)
        {
            ConfigManager.Register(this);

            string topSection = "Contracts";
            CONTRACTS_ENABLED = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, "Enable the ability to purchase contracts / missions", true, "");
            CONTRACT_FREE_MOONS_ONLY = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, "Random contracts on free moons only", true, "If true, \"contract\" command will only generate contracts on free moons.");
            CONTRACT_PRICE = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, "Price of a random contract", 500, "");
            CONTRACT_SPECIFY_PRICE = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, "Price of a specified moon contract", 750, "");
            CONTRACT_BUG_REWARD = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, "Value of an exterminator contract reward", 500, "");
            CONTRACT_EXOR_REWARD = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, "Value of an exorcism contract reward", 500, "");
            CONTRACT_DEFUSE_REWARD = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, "Value of an defusal contract reward", 500, "");
            CONTRACT_EXTRACT_REWARD = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, "Value of an extraction contract reward", 500, "");
            CONTRACT_DATA_REWARD = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, "Value of a data contract reward", 500, "");
            CONTRACT_BUG_SPAWNS = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, "Hoarder Bug Spawn Number", 20, "How many bugs to spawn during exterminator contracts.");
            CONTRACT_GHOST_SPAWN = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, "Dress Girl / Thumper Spawn Number", 3, "How many ghosts/thumpers to spawn when failing exorcism contracts");
            CONTRACT_EXTRACT_WEIGHT = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, "Weight of an extraction human", 2.5f, "Subtract 1 and multiply by 100 (2.5 = 150lbs).");
            SCAV_VOLUME = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, "Volume of the scavenger voice clips", 0.25f, "0.0 - 1.0");
            MAIN_OBJECT_FURTHEST = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, "Spawn main object far away", true, "If true the main object for contracts will try spawn as far away from the main entrance as possible. If false it will spawn at a random location.");
            EXTRACTION_CONTRACT_AMOUNT_MEDKITS = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, "Amount of medkits that can spawn in the Extraction contract", 3, "");

            // this is kind of dumb and I'd like to just use a comma seperated cfg.BindSyncedEntry<string> but this is much more foolproof
            DATA_CONTRACT = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, "Enable the data contract", true, "Make this false if you don't want the data contract");
            EXTRACTION_CONTRACT = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, "Enable the extraction contract", true, "Make this false if you don't want the extraction contract");
            EXORCISM_CONTRACT = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, "Enable the exorcism contract", true, "Make this false if you don't want the exorcism contract");
            DEFUSAL_CONTRACT = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, "Enable the defusal contract", true, "Make this false if you don't want the defusal contract");
            EXTERMINATOR_CONTRACT = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, "Enable the exterminator contract", true, "Make this false if you don't want the exterminator contract");

            CONTRACT_REWARD_QUOTA_MULTIPLIER = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, "Multiplier applied to the contract loot items dependant on the current applied quota", 25, "0 = None of the quota value will influence the loot's value.\n100 = The quota value will be added fully to the loot's value.");

            topSection = ChargingBooster.UPGRADE_NAME;
            CHARGING_BOOSTER_ENABLED = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, "Enable Charging Booster Upgrade", true, "Tier upgrade which allows charging items in a radar booster");
            CHARGING_BOOSTER_PRICE = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, "Price of Charging Booster Upgrade", 300, "");
            CHARGING_BOOSTER_PRICES = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, BaseUpgrade.PRICES_SECTION, "250,300,400", BaseUpgrade.PRICES_DESCRIPTION);
            CHARGING_BOOSTER_COOLDOWN = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, "Cooldown of the charging station in radar boosters when used", 90f, "");
            CHARGING_BOOSTER_INCREMENTAL_COOLDOWN_DECREASE = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, "Incremental cooldown decrease whenever the level of the upgrade is incremented", 10f, "");
            CHARGING_BOOSTER_CHARGE_PERCENTAGE = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, "Percentage of charge that is added to the held item when using the radar booster charge station", 50, "");

            topSection = "_Misc_";
            SHARED_UPGRADES = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, "Convert all upgrades to be shared.", true, "If true this will ignore the individual shared upgrade option for all other upgrades and set all upgrades to be shared.");
            SALE_PERC = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, "Chance of upgrades going on sale", 0.85f, "0.85 = 15% chance of an upgrade going on sale.");
            INTRO_ENABLED = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, "Intro Enabled", true, "If true shows a splashscreen with some info once per update of LGU.");
            KEEP_UPGRADES_AFTER_FIRED_CUTSCENE = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, "Keep upgrades after quota failure", false, "If true, you will keep your upgrades after being fired by The Company.");
            SHOW_UPGRADES_CHAT = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, "Show upgrades being loaded in chat", true, "If enabled, chat messages will be displayed when loading an upgrade for the first time.");
            SALE_APPLY_ONCE = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, "Apply upgrade sale on one purchase", false, "When an upgrade is on sale, apply the sale only on the first ever purchase of it while on sale. Consecutive purchases will not have the sale applied");

            topSection = "Helmet";
            HELMET_ENABLED = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, "Enable the helmet for purchase", true, "");
            HELMET_PRICE = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, "Price of the helmet", 750, "");
            HELMET_HITS_BLOCKED = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, "Amount of hits blocked by helmet", 2, "");

            topSection = "Sick Beats";
            BEATS_ENABLED = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, "Enable Sick Beats Upgrade", true, "Get buffs from nearby active boomboxes.");
            BEATS_INDIVIDUAL = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, BaseUpgrade.INDIVIDUAL_SECTION, BaseUpgrade.INDIVIDUAL_DEFAULT, BaseUpgrade.INDIVIDUAL_DESCRIPTION);
            BEATS_PRICE = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, "Default unlock price", 500, "");
            BEATS_SPEED = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, "Enable Speed Boost Effect", true, "");
            BEATS_DMG = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, "Enable Damage Boost Effect", true, "");
            BEATS_STAMINA = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, "Enable Stamina Boost Effect", false, "");
            BEATS_DEF = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, "Enable Defense Boost Effect", false, "");
            BEATS_DEF_CO = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, "Defense Boost Coefficient", 0.5f, "Multiplied to incoming damage.");
            BEATS_STAMINA_CO = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, "Stamina Regen Coefficient", 1.25f, "Multiplied to stamina regen.");
            BEATS_DMG_INC = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, "Additional Damage Dealt", 1, "");
            BEATS_SPEED_INC = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, "Speed Boost Addition", 1.5f, "");
            BEATS_RADIUS = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, "Effect Radius", 15f, "Radius in unity units players will be effected by an active boombox.");

            topSection = "Advanced Portable Teleporter";
            ADVANCED_TELE_ENABLED = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, "Enable Advanced Portable Teleporter", true, "");
            ADVANCED_TELE_PRICE = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, "Price of Advanced Portable Teleporter", 1750, "");
            ADV_CHANCE_TO_BREAK = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, "Chance to break on use", 0.1f, "value should be 0.00 - 1.00");
            ADV_KEEP_ITEMS_ON_TELE = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, "Keep Items When Using Advanced Portable Teleporters", true, "If set to false you will drop your items like when using the vanilla TP.");

            topSection = "Portable Teleporter";
            WEAK_TELE_ENABLED = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, "Enable Portable Teleporter", true, "");
            WEAK_TELE_PRICE = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, "Price of Portable Teleporter", 300, "");
            CHANCE_TO_BREAK = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, "Chance to break on use", 0.9f, "value should be 0.00 - 1.00");
            KEEP_ITEMS_ON_TELE = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, "Keep Items When Using Weak Portable Teleporters", true, "If set to false you will drop your items like when using the vanilla TP.");

            topSection = Beekeeper.UPGRADE_NAME;
            BEEKEEPER_ENABLED = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, "Enable Beekeeper Upgrade", true, "Take less damage from bees");
            BEEKEEPER_PRICE = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, "Price of Beekeeper Upgrade", 450, "");
            BEEKEEPER_DAMAGE_MULTIPLIER = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, "Multiplied to incoming damage (rounded to cfg.BindSyncedEntry<int>)", 0.64f, "Incoming damage from bees is 10.");
            BEEKEEPER_UPGRADE_PRICES = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, BaseUpgrade.PRICES_SECTION, Beekeeper.PRICES_DEFAULT, BaseUpgrade.PRICES_DESCRIPTION);
            BEEKEEPER_INDIVIDUAL = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, BaseUpgrade.INDIVIDUAL_SECTION, BaseUpgrade.INDIVIDUAL_DEFAULT, BaseUpgrade.INDIVIDUAL_DESCRIPTION);
            BEEKEEPER_DAMAGE_MULTIPLIER_INCREMENT = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, "Additional % Reduced per level", 0.15f, "Every time beekeeper is upgraded this value will be subtracted to the base multiplier above.");
            BEEKEEPER_HIVE_VALUE_INCREASE = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, "Hive value increase multiplier", 1.5f, "Multiplier applied to the value of beehive when reached max level");

            topSection = ProteinPowder.UPGRADE_NAME;
            PROTEIN_ENABLED = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, ProteinPowder.ENABLED_SECTION, ProteinPowder.ENABLED_DEFAULT, ProteinPowder.ENABLED_DESCRIPTION);
            PROTEIN_PRICE = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, ProteinPowder.PRICE_SECTION, ProteinPowder.PRICE_DEFAULT, "");
            PROTEIN_UNLOCK_FORCE = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, ProteinPowder.UNLOCK_FORCE_SECTION, ProteinPowder.UNLOCK_FORCE_DEFAULT, ProteinPowder.UNLOCK_FORCE_DESCRIPTION);
            PROTEIN_INCREMENT = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, ProteinPowder.INCREMENT_FORCE_SECTION, ProteinPowder.INCREMENT_FORCE_DEFAULT, ProteinPowder.INCREMENT_FORCE_DESCRIPTION);
            PROTEIN_INDIVIDUAL = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, BaseUpgrade.INDIVIDUAL_SECTION, BaseUpgrade.INDIVIDUAL_DEFAULT, BaseUpgrade.INDIVIDUAL_DESCRIPTION);
            PROTEIN_UPGRADE_PRICES = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, BaseUpgrade.PRICES_SECTION, ProteinPowder.PRICES_DEFAULT, BaseUpgrade.PRICES_DESCRIPTION);
            PROTEIN_CRIT_CHANCE = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, ProteinPowder.CRIT_CHANCE_SECTION, ProteinPowder.CRIT_CHANCE_DEFAULT, ProteinPowder.CRIT_CHANCE_DESCRIPTION);

            topSection = BiggerLungs.UPGRADE_NAME;
            BIGGER_LUNGS_ENABLED = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, "Enable Bigger Lungs Upgrade", true, "More Stamina");
            BIGGER_LUNGS_PRICE = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, "Price of Bigger Lungs Upgrade", 600, "");
            SPRINT_TIME_INCREASE_UNLOCK = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, "Sprint Time Unlock", 6f, "Amount of sprint time gained when unlocking the upgrade.\nDefault vanilla value is 11f.");
            SPRINT_TIME_INCREMENT = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, "Sprint Time Increment", 1.25f, "Amount of sprint time gained when increasing the level of upgrade.");
            BIGGER_LUNGS_UPGRADE_PRICES = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, BaseUpgrade.PRICES_SECTION, BiggerLungs.PRICES_DEFAULT, BaseUpgrade.PRICES_DESCRIPTION);
            BIGGER_LUNGS_INDIVIDUAL = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, BaseUpgrade.INDIVIDUAL_SECTION, BaseUpgrade.INDIVIDUAL_DEFAULT, BaseUpgrade.INDIVIDUAL_DESCRIPTION);
            BIGGER_LUNGS_STAMINA_REGEN_APPLY_LEVEL = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, "Level of Bigger Lungs to apply stamina regeneration", 1, "When reached provided level, the effect will start applying.");
            BIGGER_LUNGS_STAMINA_REGEN_INCREASE = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, "Stamina Regeneration Increase", 1.05f, "Affects the rate of the player regaining stamina");
            BIGGER_LUNGS_STAMINA_REGEN_INCREMENTAL_INCREASE = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, "Incremental stamina regeneration increase", 0.05f, "Added to initial multiplier");
            BIGGER_LUNGS_JUMP_STAMINA_APPLY_LEVEL = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, "Level of Bigger Lungs to apply stamina cost decrease on jumps", 2, "When reached provided level, the effect will start applying.");
            BIGGER_LUNGS_JUMP_STAMINA_COST_DECREASE = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, "Stamina cost decrease on jumps", 0.90f, "Multiplied with the vanilla cost of jumping");
            BIGGER_LUNGS_JUMP_STAMINA_COST_INCREMENTAL_DECREASE = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, "Incremental stamina cost decrease on jumps", 0.05f, "Added to initial multiplier (as in reduce the factor)");

            topSection = "Extend Deadline";
            EXTEND_DEADLINE_ENABLED = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, "Enable Extend Deadline Purchase", true, "Increments the amount of days before deadline is reached.");
            EXTEND_DEADLINE_PRICE = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, "Extend Deadline Price", 800, "Price of each day extension requested in the terminal.");

            topSection = RunningShoes.UPGRADE_NAME;
            RUNNING_SHOES_ENABLED = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, "Enable Running Shoes Upgrade", true, "Run Faster");
            RUNNING_SHOES_PRICE = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, "Price of Running Shoes Upgrade", 650, "");
            MOVEMENT_SPEED_UNLOCK = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, "Movement Speed Unlock", 1.4f, "Value added to player's movement speed when first purchased.\nDefault vanilla value is 4.6f.");
            MOVEMENT_INCREMENT = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, "Movement Speed Increment", 0.5f, "How much the above value is increased on upgrade.");
            RUNNING_SHOES_UPGRADE_PRICES = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, BaseUpgrade.PRICES_SECTION, BiggerLungs.PRICES_DEFAULT, BaseUpgrade.PRICES_DESCRIPTION);
            RUNNING_SHOES_INDIVIDUAL = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, BaseUpgrade.INDIVIDUAL_SECTION, BaseUpgrade.INDIVIDUAL_DEFAULT, BaseUpgrade.INDIVIDUAL_DESCRIPTION);
            NOISE_REDUCTION = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, "Noise Reduction", 10f, "Distance units to subtract from footstep noise when reached final level.");

            topSection = StrongLegs.UPGRADE_NAME;
            STRONG_LEGS_ENABLED = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, "Enable Strong Legs Upgrade", true, "Jump Higher");
            STRONG_LEGS_PRICE = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, "Price of Strong Legs Upgrade", 300, "");
            JUMP_FORCE_UNLOCK = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, "Jump Force Unlock", 3f, "Amount of jump force added when unlocking the upgrade.\nDefault vanilla value is 13f.");
            JUMP_FORCE_INCREMENT = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, "Jump Force Increment", 0.75f, "How much the above value is increased on upgrade.");
            STRONG_LEGS_UPGRADE_PRICES = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, BaseUpgrade.PRICES_SECTION, StrongLegs.PRICES_DEFAULT, BaseUpgrade.PRICES_DESCRIPTION);
            STRONG_LEGS_INDIVIDUAL = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, BaseUpgrade.INDIVIDUAL_SECTION, BaseUpgrade.INDIVIDUAL_DEFAULT, BaseUpgrade.INDIVIDUAL_DESCRIPTION);
            STRONG_LEGS_REDUCE_FALL_DAMAGE_MULTIPLIER = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, "Damage mitigation when falling", 0.5f, "Multiplier applied on fall damage that you wish to ignore when reached max level");

            topSection = MalwareBroadcaster.UPGRADE_NAME;
            MALWARE_BROADCASTER_ENABLED = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, "Enable Malware Broadcaster Upgrade", true, "Explode Map Hazards");
            MALWARE_BROADCASTER_PRICE = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, "Price of Malware Broadcaster Upgrade", 550, "");
            DESTROY_TRAP = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, "Destroy Trap", true, "If false Malware Broadcaster will disable the trap for a long time instead of destroying.");
            DISARM_TIME = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, "Disarm Time", 7f, "If `Destroy Trap` is false this is the duration traps will be disabled.");
            EXPLODE_TRAP = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, "Explode Trap", true, "Destroy Trap must be true! If this is true when destroying a trap it will also explode.");
            MALWARE_BROADCASTER_INDIVIDUAL = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, BaseUpgrade.INDIVIDUAL_SECTION, BaseUpgrade.INDIVIDUAL_DEFAULT, BaseUpgrade.INDIVIDUAL_DESCRIPTION);

            topSection = "Night Vision";
            NIGHT_VISION_ENABLED = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, "Enable Night Vision Upgrade", true, "Toggleable night vision.");
            NIGHT_VISION_PRICE = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, "Price of Night Vision Upgrade", 380, "");
            NIGHT_BATTERY_MAX = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, "The max charge for your night vision battery", 10f, "Default settings this will be the unupgraded time in seconds the battery will drain and regen in. Increase to increase battery life.");
            NIGHT_VIS_DRAIN_SPEED = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, "Multiplier for night vis battery drain", 1f, "Multiplied by timedelta, lower to increase battery life.");
            NIGHT_VIS_REGEN_SPEED = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, "Multiplier for night vis battery regen", 1f, "Multiplied by timedelta, raise to speed up battery regen time.");
            NIGHT_VIS_COLOR = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, "Night Vision Color", "#00FF00FF", "The color your night vision light emits.");
            NIGHT_VIS_UI_TEXT_COLOR = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, "Night Vision UI Text Color", "#FFFFFFFF", "The color used for the night vision's UI text.");
            NIGHT_VIS_UI_BAR_COLOR = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, "Night Vision UI Bar Color", "#00FF00FF", "The color used for the night vision's UI battery bar.");
            NIGHT_VIS_RANGE = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, "Night Vision Range", 2000f, "Kind of like the distance your night vision travels.");
            NIGHT_VIS_RANGE_INCREMENT = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, "Night Vision Range Increment", 0f, "Increases your range by this value each upgrade.");
            NIGHT_VIS_INTENSITY = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, "Night Vision Intensity", 1000f, "Kind of like the brightness of your Night Vision.");
            NIGHT_VIS_INTENSITY_INCREMENT = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, "Night Vision Intensity Increment", 0f, "Increases your intensity by this value each upgrade.");
            NIGHT_VIS_STARTUP = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, "Night Vision StartUp Cost", 0.1f, "The percent battery drained when turned on (0.1 = 10%).");
            NIGHT_VIS_EXHAUST = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, "Night Vision Exhaustion", 2f, "How many seconds night vision stays fully depleted.");
            NIGHT_VIS_DRAIN_INCREMENT = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, "Decrease for night vis battery drain", 0.15f, "Applied to drain speed on each upgrade.");
            NIGHT_VIS_REGEN_INCREMENT = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, "Increase for night vis battery regen", 0.40f, "Applied to regen speed on each upgrade.");
            NIGHT_VIS_BATTERY_INCREMENT = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, "Increase for night vis battery life", 2f, "Applied to the max charge for night vis battery on each upgrade.");
            LOSE_NIGHT_VIS_ON_DEATH = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, "Lose Night Vision On Death", true, "If true when you die the night vision will disable and will need a new pair of goggles.");
            NIGHT_VISION_DROP_ON_DEATH = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, "Drop Night Vision item on Death", true, "If true, when you die and lose night vision upon death, you will drop the night vision goggles on your body.");
            NIGHT_VISION_UPGRADE_PRICES = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, BaseUpgrade.PRICES_SECTION, NightVision.PRICES_DEFAULT, BaseUpgrade.PRICES_DESCRIPTION);
            NIGHT_VISION_INDIVIDUAL = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, BaseUpgrade.INDIVIDUAL_SECTION, BaseUpgrade.INDIVIDUAL_DEFAULT, BaseUpgrade.INDIVIDUAL_DESCRIPTION);

            topSection = Discombobulator.UPGRADE_NAME;
            DISCOMBOBULATOR_ENABLED = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, "Enable Discombobulator Upgrade", true, "Stun enemies around the ship.");
            DISCOMBOBULATOR_PRICE = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, "Price of Discombobulator Upgrade", 450, "");
            DISCOMBOBULATOR_COOLDOWN = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, "Discombobulator Cooldown", 120f, "");
            DISCOMBOBULATOR_RADIUS = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, "Discombobulator Effect Radius", 40f, "");
            DISCOMBOBULATOR_STUN_DURATION = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, "Discombobulator Stun Duration", 7.5f, "");
            DISCOMBOBULATOR_NOTIFY_CHAT = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, "Notify Local Chat of Enemy Stun Duration", true, "");
            DISCOMBOBULATOR_INCREMENT = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, "Discombobulator Increment", 1f, "The amount added to stun duration on upgrade.");
            DISCO_UPGRADE_PRICES = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, BaseUpgrade.PRICES_SECTION, Discombobulator.PRICES_DEFAULT, BaseUpgrade.PRICES_DESCRIPTION);
            DISCOMBOBULATOR_INDIVIDUAL = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, BaseUpgrade.INDIVIDUAL_SECTION, BaseUpgrade.INDIVIDUAL_DEFAULT, BaseUpgrade.INDIVIDUAL_DESCRIPTION);
            DISCOMBOBULATOR_DAMAGE_LEVEL = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, "Initial level of dealing damage", 2, "Level of Discombobulator in which it starts dealing damage on enemies it stuns");
            DISCOMBOBULATOR_INITIAL_DAMAGE = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, "Initial amount of damage", 2, "Amount of damage when reaching the first level that unlocks damage on stun\nTo give an idea of what's too much, Eyeless Dogs have 12 health.");
            DISCOMBOBULATOR_DAMAGE_INCREASE = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, "Damage Increase on Purchase", 1, "Damage increase when purchasing later levels");

            topSection = BetterScanner.UPGRADE_NAME;
            BETTER_SCANNER_ENABLED = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, "Enable Better Scanner Upgrade", true, "Further scan distance, no LOS needed.");
            BETTER_SCANNER_PRICE = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, "Price of Better Scanner Upgrade", 650, "");
            SHIP_AND_ENTRANCE_DISTANCE_INCREASE = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, "Ship and Entrance node distance boost", 150f, "How much further away you can scan the ship and entrance.");
            NODE_DISTANCE_INCREASE = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, "Node distance boost", 20f, "How much further away you can scan other nodes.");
            BETTER_SCANNER_INDIVIDUAL = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, BaseUpgrade.INDIVIDUAL_SECTION, BaseUpgrade.INDIVIDUAL_DEFAULT, BaseUpgrade.INDIVIDUAL_DESCRIPTION);
            BETTER_SCANNER_PRICE2 = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, "Price of first Better Scanner tier", 500, "This tier unlocks ship scan commands.");
            BETTER_SCANNER_PRICE3 = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, "Price of second Better Scanner tier", 800, "This tier unlocks scanning through walls.");
            BETTER_SCANNER_ENEMIES = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, "Scan enemies through walls on final upgrade", false, "If true the final upgrade will scan scrap AND enemies through walls.");
            VERBOSE_ENEMIES = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, "Verbose `scan enemies` command", true, "If false `scan enemies` only returns a count of outside and inside enemies, else it returns the count for each enemy type.");

            topSection = BackMuscles.UPGRADE_NAME;
            BACK_MUSCLES_ENABLED = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, "Enable Back Muscles Upgrade", true, "Reduce carry weight");
            BACK_MUSCLES_PRICE = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, "Price of Back Muscles Upgrade", 715, "");
            CARRY_WEIGHT_REDUCTION = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, "Carry Weight Multiplier", 0.5f, "Your carry weight is multiplied by this.");
            CARRY_WEIGHT_INCREMENT = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, "Carry Weight Increment", 0.1f, "Each upgrade subtracts this from the above coefficient.");
            BACK_MUSCLES_UPGRADE_PRICES = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, BaseUpgrade.PRICES_SECTION, BackMuscles.PRICES_DEFAULT, BaseUpgrade.PRICES_DESCRIPTION);
            BACK_MUSCLES_INDIVIDUAL = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, BaseUpgrade.INDIVIDUAL_SECTION, BaseUpgrade.INDIVIDUAL_DEFAULT, BaseUpgrade.INDIVIDUAL_DESCRIPTION);

            topSection = "Interns";
            INTERN_ENABLED = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, "Enable hiring of interns", true, "Pay x amount of credits to revive a player.");
            INTERN_PRICE = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, "Intern Price", 1000, "Default price to hire an intern.");
            INTERN_INDIVIDUAL = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, BaseUpgrade.INDIVIDUAL_SECTION, BaseUpgrade.INDIVIDUAL_DEFAULT, BaseUpgrade.INDIVIDUAL_DESCRIPTION);

            topSection = FastEncryption.UPGRADE_NAME;
            PAGER_ENABLED = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, "Enable Fast Encryption", true, "Upgrades the transmitter.");
            PAGER_PRICE = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, "Fast Encryption Price", 300, "");

            topSection = LockSmith.UPGRADE_NAME;
            LOCKSMITH_ENABLED = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, "Enable Locksmith upgrade", true, "Allows you to pick locked doors by completing a minigame.");
            LOCKSMITH_PRICE = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, "Locksmith Price", 640, "Default price of Locksmith upgrade.");
            LOCKSMITH_INDIVIDUAL = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, BaseUpgrade.INDIVIDUAL_SECTION, BaseUpgrade.INDIVIDUAL_DEFAULT, BaseUpgrade.INDIVIDUAL_DESCRIPTION);

            topSection = "Peeper";
            PEEPER_ENABLED = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, "Enable Peeper item", true, "An item that will stare at coilheads for you.");
            PEEPER_PRICE = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, "Peeper Price", 500, "Default price to purchase a Peeper.");

            topSection = LightningRod.UPGRADE_NAME;
            LIGHTNING_ROD_ENABLED = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, LightningRod.ENABLED_SECTION, LightningRod.ENABLED_DEFAULT, LightningRod.ENABLED_DESCRIPTION);
            LIGHTNING_ROD_PRICE = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, LightningRod.PRICE_SECTION, LightningRod.PRICE_DEFAULT, "");
            LIGHTNING_ROD_ACTIVE = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, LightningRod.ACTIVE_SECTION, LightningRod.ACTIVE_DEFAULT, LightningRod.ACTIVE_DESCRIPTION);
            LIGHTNING_ROD_DIST = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, LightningRod.DIST_SECTION, LightningRod.DIST_DEFAULT, LightningRod.DIST_DESCRIPTION);

            topSection = "Walkie";
            WALKIE_ENABLED = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, "Enable the walkie talkie gps upgrade", true, "Holding a walkie talkie displays location.");
            WALKIE_PRICE = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, "Walkie GPS Price", 450, "Default price for upgrade.");
            WALKIE_INDIVIDUAL = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, BaseUpgrade.INDIVIDUAL_SECTION, BaseUpgrade.INDIVIDUAL_DEFAULT, BaseUpgrade.INDIVIDUAL_DESCRIPTION);

            topSection = FasterDropPod.UPGRADE_NAME;
            FASTER_DROP_POD_ENABLED = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, "Enable the Drop Pod Thrusters upgrade", true, "Make the Drop Pod, the ship that deliver items bought on the terminal, land faster.");
            FASTER_DROP_POD_PRICE = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, "Drop Pod Thrusters Price", 300, "Default price for upgrade. If set to 0 it will enable the Drop Pod without buying the upgrade.");
            FASTER_DROP_POD_TIMER = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, "Time decrement on the timer used for subsequent item deliveries", 20f, "");
            FASTER_DROP_POD_INITIAL_TIMER = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, "Time decrement on the timer used for the first ever item delivery", 10f, "");

            topSection = Sigurd.UPGRADE_NAME;
            SIGURD_ENABLED = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, "Enable the Sigurd Access upgrade", true, "There's a chance that The Company will pay more for the scrap.");
            SIGURD_LAST_DAY_ENABLED = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, "Enable the Sigurd Access upgrade for the last day", true, "There's a chance that the last day scrap you go over 100% value.");
            SIGURD_PRICE = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, "Sigurd Access Price", 500, "Default price for upgrade. If set to 0 it will enable the Sigurd Access without buying the upgrade.");
            SIGURD_CHANCE = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, "Chance for the upgrade to activate", 20f, "");
            SIGURD_LAST_DAY_CHANCE = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, "Chance for the upgrade to activate on the last day", 20f, "");
            SIGURD_PERCENT = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, "How much the percentage will go up", 20f, "");
            SIGURD_LAST_DAY_PERCENT = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, "How much the percentage will go up on the last day", 20f, "");

            topSection = Hunter.UPGRADE_NAME;
            HUNTER_ENABLED = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, "Enable the Hunter upgrade", true, "Collect and sell samples from dead enemies");
            HUNTER_PRICE = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, "Hunter price", 700, "Default price for upgrade.");
            HUNTER_UPGRADE_PRICES = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, BaseUpgrade.PRICES_SECTION, "500,600", BaseUpgrade.PRICES_DESCRIPTION);
            HUNTER_SAMPLE_TIERS = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection,
                                            "Samples dropping at each tier",
                                            "Hoarding Bug, Centipede-Bunker Spider, Baboon hawk-Flowerman, MouthDog, Crawler",
                                            "Specifies at which tier of Hunter do each sample start dropping from. Each tier is separated with a dash ('-') and each list of monsters will be separated with a comma (',')\nSupported Enemies: Hoarding Bug, Centipede (Snare Flea),Bunker Spider, Baboon Hawk, Crawler (Half/Thumper), Flowerman (Bracken) and MouthDog (Eyeless Dog)");
            SNARE_FLEA_SAMPLE_MINIMUM_VALUE = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, "Minimum scrap value of a Snare Flea sample", 35, "");
            SNARE_FLEA_SAMPLE_MAXIMUM_VALUE = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, "Maximum scrap value of a Snare Flea sample", 60, "");
            BUNKER_SPIDER_SAMPLE_MINIMUM_VALUE = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, "Minimum scrap value of a Bunker Spider sample", 65, "");
            BUNKER_SPIDER_SAMPLE_MAXIMUM_VALUE = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, "Maximum scrap value of a Bunker Spider sample", 95, "");
            HOARDING_BUG_SAMPLE_MINIMUM_VALUE = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, "Minimum scrap value of a Hoarding Bug sample", 45, "");
            HOARDING_BUG_SAMPLE_MAXIMUM_VALUE = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, "Maximum scrap value of a Hoarding Bug sample", 75, "");
            BRACKEN_SAMPLE_MINIMUM_VALUE = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, "Minimum scrap value of a Bracken sample", 80, "");
            BRACKEN_SAMPLE_MAXIMUM_VALUE = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, "Maximum scrap value of a Bracken sample", 125, "");
            EYELESS_DOG_SAMPLE_MINIMUM_VALUE = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, "Minimum scrap value of a Eyeless Dog sample", 100, "");
            EYELESS_DOG_SAMPLE_MAXIMUM_VALUE = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, "Maximum scrap value of a Eyeless Dog sample", 150, "");
            BABOON_HAWK_SAMPLE_MINIMUM_VALUE = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, "Minimum scrap value of a Baboon Hawk sample", 75, "");
            BABOON_HAWK_SAMPLE_MAXIMUM_VALUE = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, "Maximum scrap value of a Baboon Hawk sample", 115, "");
            THUMPER_SAMPLE_MINIMUM_VALUE = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, "Minimum scrap value of a Half sample", 80, "");
            THUMPER_SAMPLE_MAXIMUM_VALUE = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, "Maximum scrap value of a Half sample", 125, "");

            topSection = Stimpack.UPGRADE_NAME;
            PLAYER_HEALTH_ENABLED = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, Stimpack.ENABLED_SECTION, Stimpack.ENABLED_DEFAULT, Stimpack.ENABLED_DESCRIPTION);
            PLAYER_HEALTH_PRICE = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, Stimpack.PRICE_SECTION, Stimpack.PRICE_DEFAULT, "");
            PLAYER_HEALTH_INDIVIDUAL = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, BaseUpgrade.INDIVIDUAL_SECTION, BaseUpgrade.INDIVIDUAL_DEFAULT, BaseUpgrade.INDIVIDUAL_DESCRIPTION);
            PLAYER_HEALTH_UPGRADE_PRICES = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, BaseUpgrade.PRICES_SECTION, Stimpack.PRICES_DEFAULT, BaseUpgrade.PRICES_DESCRIPTION);
            PLAYER_HEALTH_ADDITIONAL_HEALTH_UNLOCK = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, Stimpack.ADDITIONAL_HEALTH_UNLOCK_SECTION, Stimpack.ADDITIONAL_HEALTH_UNLOCK_DEFAULT, Stimpack.ADDITIONAL_HEALTH_UNLOCK_DESCRIPTION);
            PLAYER_HEALTH_ADDITIONAL_HEALTH_INCREMENT = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, Stimpack.ADDITIONAL_HEALTH_INCREMENT_SECTION, Stimpack.ADDITIONAL_HEALTH_INCREMENT_DEFAULT, Stimpack.ADDITIONAL_HEALTH_INCREMENT_DESCRIPTION);

            topSection = "Medkit";
            MEDKIT_ENABLED = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, "Enable the medkit item", true, "Allows you to buy a medkit to heal yourself.");
            MEDKIT_PRICE = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, "price", 300, "Default price for Medkit.");
            MEDKIT_HEAL_VALUE = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, "Heal Amount", 20, "The amount the medkit heals you.");
            MEDKIT_USES = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, "Uses", 3, "The amount of times the medkit can heal you.");

            topSection = "Diving Kit";
            DIVEKIT_ENABLED = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, "Enable the Diving Kit Item", true, "Allows you to buy a diving kit to breathe underwater.");
            DIVEKIT_PRICE = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, "price", 650, "Price for Diving Kit.");
            DIVEKIT_WEIGHT = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, "Item weight", 1.65f, "-1 and multiply by 100 (1.65 = 65 lbs)");
            DIVEKIT_TWO_HANDED = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, "Two Handed Item", true, "One or two handed item.");

            topSection = DoorsHydraulicsBattery.UPGRADE_NAME;
            DOOR_HYDRAULICS_BATTERY_ENABLED = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, DoorsHydraulicsBattery.ENABLED_SECTION, true, DoorsHydraulicsBattery.ENABLED_DESCRIPTION);
            DOOR_HYDRAULICS_BATTERY_PRICE = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, DoorsHydraulicsBattery.PRICE_SECTION, DoorsHydraulicsBattery.PRICE_DEFAULT, "");
            DOOR_HYDRAULICS_BATTERY_PRICES = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, BaseUpgrade.PRICES_SECTION, DoorsHydraulicsBattery.PRICES_DEFAULT, BaseUpgrade.PRICES_DESCRIPTION);
            DOOR_HYDRAULICS_BATTERY_INITIAL = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, DoorsHydraulicsBattery.INITIAL_SECTION, DoorsHydraulicsBattery.INITIAL_DEFAULT, DoorsHydraulicsBattery.INITIAL_DESCRIPTION);
            DOOR_HYDRAULICS_BATTERY_INCREMENTAL = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, DoorsHydraulicsBattery.INCREMENTAL_SECTION, DoorsHydraulicsBattery.INCREMENTAL_DEFAULT, DoorsHydraulicsBattery.INCREMENTAL_DESCRIPTION);

            topSection = ScrapInsurance.COMMAND_NAME;
            SCRAP_INSURANCE_ENABLED = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, "Enable Scrap Insurance Command", true, "One time purchase which allows you to keep all your scrap upon a team wipe on a moon trip");
            SCRAP_INSURANCE_PRICE = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, "Price of Scrap Insurance", ScrapInsurance.DEFAULT_PRICE, "");

            topSection = MarketInfluence.UPGRADE_NAME;
            MARKET_INFLUENCE_ENABLED = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, "Enable Market Influence Upgrade", true, "Tier upgrade which guarantees a minimum percentage sale on the selected item in the store.");
            MARKET_INFLUENCE_PRICE = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, "Price of Market Influence", 250, "");
            MARKET_INFLUENCE_PRICES = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, BaseUpgrade.PRICES_SECTION, MarketInfluence.PRICES_DEFAULT, BaseUpgrade.PRICES_DESCRIPTION);
            MARKET_INFLUENCE_INITIAL_PERCENTAGE = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, "Initial percentage guarantee on the items on sale", 10, "");
            MARKET_INFLUENCE_INCREMENTAL_PERCENTAGE = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, "Incremental percentage guarantee on the items on sale", 5, "");

            topSection = BargainConnections.UPGRADE_NAME;
            BARGAIN_CONNECTIONS_ENABLED = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, "Enable Bargain Connections Upgrade", true, "Tier upgrade which increases the amount of items that can be on sale in the store");
            BARGAIN_CONNECTIONS_PRICE = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, "Price of Bargain Connections", 200, "");
            BARGAIN_CONNECTIONS_INITIAL_ITEM_AMOUNT = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, "Initial additional amount of items that can go on sale", 3, "");
            BARGAIN_CONNECTIONS_INCREMENTAL_ITEM_AMOUNT = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, "Incremental additional amount of items that can go on sale", 2, "");
            BARGAIN_CONNECTIONS_PRICES = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, BaseUpgrade.PRICES_SECTION, BargainConnections.PRICES_DEFAULT, BaseUpgrade.PRICES_DESCRIPTION);

            topSection = LethalDeals.UPGRADE_NAME;
            LETHAL_DEALS_ENABLED = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, "Enable Lethal Deals Upgrade", true, "One time upgrade which guarantees at least one item will be on sale in the store.");
            LETHAL_DEALS_PRICE = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, "Price of Lethal Deals", 300, "");

            topSection = QuantumDisruptor.UPGRADE_NAME;
            QUANTUM_DISRUPTOR_ENABLED = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, "Enable Quantum Disruptor Upgrade", true, "Tier upgrade which increases the time you can stay in a moon landing");
            QUANTUM_DISRUPTOR_PRICE = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, "Price of Quantum Disruptor Upgrade", 1000, "");
            QUANTUM_DISRUPTOR_INITIAL_MULTIPLIER = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, "How slower time will go by when unlocking the Quantum Disruptor upgrade", 0.2f, "");
            QUANTUM_DISRUPTOR_INCREMENTAL_MULTIPLIER = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, "How slower time will go by when incrementing the Quantum Disruptor level", 0.1f, "");
            QUANTUM_DISRUPTOR_PRICES = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, BaseUpgrade.PRICES_SECTION, QuantumDisruptor.PRICES_DEFAULT, BaseUpgrade.PRICES_DESCRIPTION);

            topSection = EfficientEngines.UPGRADE_NAME;
            EFFICIENT_ENGINES_ENABLED = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, "Enable Efficient Engines Upgrade", true, "Tier upgrade which applies a discount on moon routing prices for cheaper travels");
            EFFICIENT_ENGINES_PRICE = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, "Price of Efficient Engines", 450, "");
            EFFICIENT_ENGINES_PRICES = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, BaseUpgrade.PRICES_SECTION, "600, 750, 900", BaseUpgrade.PRICES_DESCRIPTION);
            EFFICIENT_ENGINES_INITIAL_DISCOUNT = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, "Initial discount applied to moon routing (%)", 15, "");
            EFFICIENT_ENGINES_INCREMENTAL_DISCOUNT = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, "Incremental discount applied to moon routing (%)", 5, "");
            
            topSection = ClimbingGloves.UPGRADE_NAME;
            CLIMBING_GLOVES_ENABLED = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, "Enable Climbing Gloves Upgrade", true, "Tier upgrade which increases the speed of climbing ladders");
            CLIMBING_GLOVES_INDIVIDUAL = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, BaseUpgrade.INDIVIDUAL_SECTION, BaseUpgrade.INDIVIDUAL_DEFAULT, BaseUpgrade.INDIVIDUAL_DESCRIPTION);
            CLIMBING_GLOVES_PRICE = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, "Price of Climbing Gloves Upgrade", 150, "");
            CLIMBING_GLOVES_PRICES = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, BaseUpgrade.PRICES_SECTION, "200,250,300", BaseUpgrade.PRICES_DESCRIPTION);
            INITIAL_CLIMBING_SPEED_BOOST = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, "Initial value added to the players climbing speed", 1f, "Vanilla climb speed is 4 units");
            INCREMENTAL_CLIMBING_SPEED_BOOST = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, "Incremental value added to the players climbing speed", 0.5f, "");

            topSection = LithiumBatteries.UPGRADE_NAME;
            LITHIUM_BATTERIES_ENABLED = SyncedBindingExtensions.BindSyncedEntry(cfg, topSection, "Enable Lithium Batteries Upgrade", true, "Tier upgrade which decreases the rate of battery consumed when using the item");
            LITHIUM_BATTERIES_INDIVIDUAL = SyncedBindingExtensions.BindSyncedEntry(cfg, topSection, BaseUpgrade.INDIVIDUAL_SECTION, BaseUpgrade.INDIVIDUAL_DEFAULT, BaseUpgrade.INDIVIDUAL_DESCRIPTION);
            LITHIUM_BATTERIES_PRICE = SyncedBindingExtensions.BindSyncedEntry(cfg, topSection, "Price of Lithium Batteries upgrade", 100);
            LITHIUM_BATTERIES_PRICES = SyncedBindingExtensions.BindSyncedEntry(cfg, topSection, BaseUpgrade.PRICES_SECTION, LithiumBatteries.PRICES_DEFAULT, BaseUpgrade.PRICES_DESCRIPTION);
            LITHIUM_BATTERIES_INITIAL_MULTIPLIER = SyncedBindingExtensions.BindSyncedEntry(cfg, topSection, "Initial multiplier applied to the use rate of the battery (%)", 10, "Past 100%, it won't consume any battery charge from the item");
            LITHIUM_BATTERIES_INCREMENTAL_MULTIPLIER = SyncedBindingExtensions.BindSyncedEntry(cfg, topSection, "Incremental multiplier applied to the use rate of the battery (%)", 10, "Past 100%, it won't consume any battery charge from the item");

            topSection = "Weather Probe";
            WEATHER_PROBE_ENABLED = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, "Enable Weather Probe Command", true, "Allows changing weather of a level through cost of credits");
            WEATHER_PROBE_PRICE = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, "Price of Weather Probe", 300, "Price of the weather probe when a weather is not selected for the level");
            WEATHER_PROBE_ALWAYS_CLEAR = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, "Always pick clear weather", false, "When enabled, randomized weather probe will always clear out the weather present in the selected level");
            WEATHER_PROBE_PICKED_WEATHER_PRICE = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, "Price of Weather Probe with selected weather", 500, "This price is used when using the probe command with a weather");

            topSection = "Wheelbarrow";
            WHEELBARROW_ENABLED = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, "Enable the Wheelbarrow Item", true, "Allows you to buy a wheelbarrow to carry items outside of your inventory");
            WHEELBARROW_PRICE = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, "Price of the Wheelbarrow Item", 400, "Price of the Wheelbarrow in the store");
            WHEELBARROW_WEIGHT = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, "Weight of the Wheelbarrow Item", 30f, "Weight of the wheelbarrow without any items in lbs");
            WHEELBARROW_RESTRICTION_MODE = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, "Restrictions on the Wheelbarrow Item", "ItemCount", "Restriction applied when trying to insert an item on the wheelbarrow.\nSupported values: None, ItemCount, TotalWeight, All");
            WHEELBARROW_MAXIMUM_WEIGHT_ALLOWED = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, "Maximum amount of weight", 100f, "How much weight (in lbs and after weight reduction multiplier is applied on the stored items) a wheelbarrow can carry in items before it is considered full.");
            WHEELBARROW_MAXIMUM_AMOUNT_ITEMS = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, "Maximum amount of items", 4, "Amount of items allowed before the wheelbarrow is considered full");
            WHEELBARROW_WEIGHT_REDUCTION_MULTIPLIER = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, "Weight reduction multiplier", 0.7f, "How much an item's weight will be ignored to the wheelbarrow's total weight");
            WHEELBARROW_NOISE_RANGE = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, "Noise range of the Wheelbarrow Item", 14f, "How far the wheelbarrow sound propagates to nearby enemies when in movement");
            WHEELBARROW_LOOK_SENSITIVITY_DRAWBACK = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, "Look sensitivity drawback of the Wheelbarrow Item", 0.4f, "Value multiplied on the player's look sensitivity when moving with the wheelbarrow Item");
            WHEELBARROW_MOVEMENT_SLOPPY = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, "Sloppiness of the Wheelbarrow Item", 5f, "Value multiplied on the player's movement to give the feeling of drifting while carrying the Wheelbarrow Item");
            WHEELBARROW_PLAY_NOISE = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, "Plays noises for players with Wheelbarrow Item", true, "If false, it will just not play the sounds, it will still attract monsters to noise");
            SCRAP_WHEELBARROW_ENABLED = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, "Enable the Shopping Cart Item", true, "Allows you to scavenge a wheelbarrow in which you can store items on");
            SCRAP_WHEELBARROW_RARITY = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, "Spawn chance of Shopping Cart Item", 0.1f, "How likely it is to a scrap wheelbarrow item to spawn when landing on a moon. (0.1 = 10%)");
            SCRAP_WHEELBARROW_WEIGHT = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, "Weight of the Shopping Cart Item", 25f, "Weight of the scrap wheelbarrow's without any items in lbs");
            SCRAP_WHEELBARROW_MAXIMUM_AMOUNT_ITEMS = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, "Maximum amount of items for Shopping Cart", 6, "Amount of items allowed before the scrap wheelbarrow is considered full");
            SCRAP_WHEELBARROW_WEIGHT_REDUCTION_MULTIPLIER = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, "Weight reduction multiplier for Shopping Cart", 0.5f, "How much an item's weight will be ignored to the scrap wheelbarrow's total weight");
            SCRAP_WHEELBARROW_MINIMUM_VALUE = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, "Minimum scrap value of Shopping Cart", 50, "Lower boundary of the scrap's possible value");
            SCRAP_WHEELBARROW_MAXIMUM_VALUE = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, "Maximum scrap value of Shopping Cart", 100, "Higher boundary of the scrap's possible value");
            SCRAP_WHEELBARROW_RESTRICTION_MODE = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, "Restrictions on the Shopping Cart Item", "ItemCount", "Restriction applied when trying to insert an item on the scrap wheelbarrow.\nSupported values: None, ItemCount, TotalWeight, All");
            SCRAP_WHEELBARROW_MAXIMUM_WEIGHT_ALLOWED = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, "Maximum amount of weight for Shopping Cart", 100f, "How much weight (in lbs and after weight reduction multiplier is applied on the stored items) a scrap wheelbarrow can carry in items before it is considered full.");
            SCRAP_WHEELBARROW_NOISE_RANGE = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, "Noise range of the Shopping Cart Item", 18f, "How far the scrap wheelbarrow sound propagates to nearby enemies when in movement");
            SCRAP_WHEELBARROW_LOOK_SENSITIVITY_DRAWBACK = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, "Look sensitivity drawback of the Shopping Cart Item", 0.8f, "Value multiplied on the player's look sensitivity when moving with the Scrap wheelbarrow Item");
            SCRAP_WHEELBARROW_MOVEMENT_SLOPPY = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, "Sloppiness of the Shopping Cart Item", 2f, "Value multiplied on the player's movement to give the feeling of drifting while carrying the Scrap Wheelbarrow Item");
            SCRAP_WHEELBARROW_PLAY_NOISE = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, "Plays noises for players with Shopping Cart Item", true, "If false, it will just not play the sounds, it will still attract monsters to noise");
            InitialSyncCompleted += PluginConfig_InitialSyncCompleted;
        }

        private void PluginConfig_InitialSyncCompleted(object sender, EventArgs e)
        {
            UpgradeBus.Instance.PluginConfiguration = Instance;
            CheckMedkit();
            UpgradeBus.Instance.Reconstruct();
        }

        void CheckMedkit()
        {
            int amount = UpgradeBus.Instance.spawnableMapObjectsAmount["MedkitMapItem"];
            if (amount == UpgradeBus.Instance.PluginConfiguration.EXTRACTION_CONTRACT_AMOUNT_MEDKITS.Value) return;
            MapObjects.RemoveMapObject(UpgradeBus.Instance.spawnableMapObjects["MedkitMapItem"], Levels.LevelTypes.All);
            AnimationCurve curve = new(new Keyframe(0f, UpgradeBus.Instance.PluginConfiguration.EXTRACTION_CONTRACT_AMOUNT_MEDKITS.Value), new Keyframe(1f, UpgradeBus.Instance.PluginConfiguration.EXTRACTION_CONTRACT_AMOUNT_MEDKITS.Value));
            MapObjects.RegisterMapObject(mapObject: UpgradeBus.Instance.spawnableMapObjects["MedkitMapItem"], levels: Levels.LevelTypes.All, spawnRateFunction: (level) => curve);
        }

    }
}
