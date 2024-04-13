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
using MoreShipUpgrades.Misc.Util;
using MoreShipUpgrades.UpgradeComponents.Items;
using MoreShipUpgrades.UpgradeComponents.Items.PortableTeleporter;


namespace MoreShipUpgrades.Misc
{
    [DataContract]
    public class PluginConfig : SyncedConfig<PluginConfig>
    {
        #region Enabled

        [field: DataMember] public SyncedEntry<bool> ALUMINIUM_COILS_ENABLED {  get; set; }
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

        #endregion

        #region Individual
        [field: DataMember] public SyncedEntry<bool> ALUMINIUM_COILS_INDIVIDUAL { get; set; }
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
        [field: DataMember] public SyncedEntry<bool> LOCKSMITH_INDIVIDUAL { get; set; }
        [field: DataMember] public SyncedEntry<bool> CLIMBING_GLOVES_INDIVIDUAL { get; set; }
        [field: DataMember] public SyncedEntry<bool> LITHIUM_BATTERIES_INDIVIDUAL { get; set; }

        #endregion

        #region Initial Prices
        [field: DataMember] public SyncedEntry<int> ALUMINIUM_COILS_PRICE { get; set; }
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

        #endregion

        #region Attributes
        [field: DataMember] public SyncedEntry<string> ALUMINIUM_COILS_PRICES {  get; set; }
        [field: DataMember] public SyncedEntry<int> ALUMINIUM_COILS_INITIAL_COOLDOWN_DECREASE {  get; set; }
        [field: DataMember] public SyncedEntry<int> ALUMINIUM_COILS_INCREMENTAL_COOLDOWN_DECREASE { get; set; }
        [field: DataMember] public SyncedEntry<int> ALUMINIUM_COILS_INITIAL_DIFFICULTY_DECREASE {  get; set; }
        [field: DataMember] public SyncedEntry<float> ALUMINIUM_COILS_INITIAL_STUN_TIMER_INCREASE {  get; set; }
        [field: DataMember] public SyncedEntry<float> ALUMINIUM_COILS_INCREMENTAL_STUN_TIMER_INCREASE { get; set; }
        [field: DataMember] public SyncedEntry<int> ALUMINIUM_COILS_INCREMENTAL_DIFFICULTY_DECREASE { get; set; }
        [field: DataMember] public SyncedEntry<float> ALUMINIUM_COILS_INITIAL_RANGE_INCREASE { get; set; }
        [field: DataMember] public SyncedEntry<float> ALUMINIUM_COILS_INCREMENTAL_RANGE_INCREASE { get; set; }
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

        #endregion

        #region Configuration Bindings
        public PluginConfig(ConfigFile cfg) : base(Metadata.GUID)
        {
            ConfigManager.Register(this);
            string topSection;

            #region Miscellaneous

            topSection = LGUConstants.MISCELLANEOUS_SECTION;
            SHARED_UPGRADES = SyncedBindingExtensions.BindSyncedEntry(cfg, topSection, LGUConstants.SHARE_ALL_UPGRADES_KEY, LGUConstants.SHARE_ALL_UPGRADES_DEFAULT, LGUConstants.SHARE_ALL_UPGRADES_DESCRIPTION);
            SALE_PERC = SyncedBindingExtensions.BindSyncedEntry(cfg, topSection, LGUConstants.SALE_PERCENT_KEY, LGUConstants.SALE_PERCENT_DEFAULT, LGUConstants.SALE_PERCENT_DESCRIPTION);
            KEEP_UPGRADES_AFTER_FIRED_CUTSCENE = SyncedBindingExtensions.BindSyncedEntry(cfg, topSection, LGUConstants.KEEP_UPGRADES_AFTER_FIRED_KEY, LGUConstants.KEEP_UPGRADES_AFTER_FIRED_DEFAULT, LGUConstants.KEEP_UPGRADES_AFTER_FIRED_DESCRIPTION);
            SHOW_UPGRADES_CHAT = SyncedBindingExtensions.BindSyncedEntry(cfg, topSection, LGUConstants.SHOW_UPGRADES_CHAT_KEY, LGUConstants.SHOW_UPGRADES_CHAT_DEFAULT, LGUConstants.SHOW_UPGRADES_CHAT_DESCRIPTION);
            SALE_APPLY_ONCE = SyncedBindingExtensions.BindSyncedEntry(cfg, topSection, LGUConstants.SALE_APPLY_ONCE_KEY, LGUConstants.SALE_APPLY_ONCE_DEFAULT, LGUConstants.SALE_APPLY_ONCE_DESCRIPTION);

            #endregion

            #region Contracts

            topSection = LGUConstants.CONTRACTS_SECTION;
            CONTRACTS_ENABLED                   = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, LGUConstants.ENABLE_CONTRACTS_KEY                 , LGUConstants.ENABLE_CONTRACTS_DEFAULT);
            CONTRACT_FREE_MOONS_ONLY            = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, LGUConstants.CONTRACT_FREE_MOONS_ONLY_KEY         , LGUConstants.CONTRACT_FREE_MOONS_ONLY_DEFAULT, LGUConstants.CONTRACT_FREE_MOONS_ONLY_DESCRIPTION);
            CONTRACT_PRICE                      = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, LGUConstants.CONTRACT_PRICE_KEY                   , LGUConstants.CONTRACT_PRICE_DEFAULT);
            CONTRACT_SPECIFY_PRICE              = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, LGUConstants.CONTRACT_SPECIFY_PRICE_KEY           , LGUConstants.CONTRACT_SPECIFY_PRICE_DEFAULT);
            CONTRACT_BUG_REWARD                 = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, LGUConstants.CONTRACT_BUG_REWARD_KEY              , LGUConstants.CONTRACT_BUG_REWARD_DEFAULT);
            CONTRACT_EXOR_REWARD                = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, LGUConstants.CONTRACT_EXORCISM_REWARD_KEY         , LGUConstants.CONTRACT_EXORCISM_REWARD_DEFAULT);
            CONTRACT_DEFUSE_REWARD              = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, LGUConstants.CONTRACT_DEFUSAL_REWARD_KEY          , LGUConstants.CONTRACT_DEFUSAL_REWARD_DEFAULT);
            CONTRACT_EXTRACT_REWARD             = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, LGUConstants.CONTRACT_EXTRACTION_REWARD_KEY       , LGUConstants.CONTRACT_EXTRACTION_REWARD_DEFAULT);
            CONTRACT_DATA_REWARD                = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, LGUConstants.CONTRACT_DATA_REWARD_KEY             , LGUConstants.CONTRACT_DATA_REWARD_DEFAULT);
            CONTRACT_BUG_SPAWNS                 = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, LGUConstants.EXTERMINATION_BUG_SPAWNS_KEY         , LGUConstants.EXTERMINATION_BUG_SPAWNS_DEFAULT, LGUConstants.EXTERMINATION_BUG_SPAWNS_DESCRIPTION);
            CONTRACT_GHOST_SPAWN                = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, LGUConstants.EXORCISM_GHOST_SPAWN_KEY             , LGUConstants.EXORCISM_GHOST_SPAWN_DEFAULT, LGUConstants.EXORCISM_GHOST_SPAWN_DESCRIPTION);
            CONTRACT_EXTRACT_WEIGHT             = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, LGUConstants.EXTRACTION_SCAVENGER_WEIGHT_KEY      , LGUConstants.EXTRACTION_SCAVENGER_WEIGHT_DEFAULT, LGUConstants.EXTRACTION_SCAVENGER_WEIGHT_DESCRIPTION);
            SCAV_VOLUME                         = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, LGUConstants.EXTRACTION_SCAVENGER_SOUND_VOLUME_KEY, LGUConstants.EXTRACTION_SCAVENGER_SOUND_VOLUME_DEFAULT, LGUConstants.EXTRACTION_SCAVENGER_SOUND_VOLUME_DESCRIPTION);
            MAIN_OBJECT_FURTHEST                = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, LGUConstants.CONTRACT_FAR_FROM_MAIN_KEY           , LGUConstants.CONTRACT_FAR_FROM_MAIN_DEFAULT, LGUConstants.CONTRACT_FAR_FROM_MAIN_DESCRIPTION);
            EXTRACTION_CONTRACT_AMOUNT_MEDKITS  = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, LGUConstants.EXTRACTION_MEDKIT_AMOUNT_KEY         , LGUConstants.EXTRACTION_MEDKIT_AMOUNT_DEFAULT);

            // this is kind of dumb and I'd like to just use a comma seperated cfg.BindSyncedEntry<string> but this is much more foolproof
            DATA_CONTRACT                       = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, LGUConstants.CONTRACT_DATA_ENABLED_KEY            , LGUConstants.CONTRACT_DATA_ENABLED_DEFAULT, LGUConstants.CONTRACT_DATA_ENABLED_DESCRIPTION);
            EXTRACTION_CONTRACT                 = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, LGUConstants.CONTRACT_EXTRACTION_ENABLED_KEY      , LGUConstants.CONTRACT_EXTRACTION_ENABLED_DEFAULT, LGUConstants.CONTRACT_EXTRACTION_ENABLED_DESCRIPTION);
            EXORCISM_CONTRACT                   = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, LGUConstants.CONTRACT_EXORCISM_ENABLED_KEY        , LGUConstants.CONTRACT_EXORCISM_ENABLED_DEFAULT, LGUConstants.CONTRACT_EXORCISM_ENABLED_DESCRIPTION);
            DEFUSAL_CONTRACT                    = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, LGUConstants.CONTRACT_DEFUSAL_ENABLED_KEY         , LGUConstants.CONTRACT_DEFUSAL_ENABLED_DEFAULT, LGUConstants.CONTRACT_DEFUSAL_ENABLED_DESCRIPTION);
            EXTERMINATOR_CONTRACT               = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, LGUConstants.CONTRACT_EXTERMINATION_ENABLED_KEY   , LGUConstants.CONTRACT_EXTERMINATION_ENABLED_DEFAULT, LGUConstants.CONTRACT_EXTERMINATION_ENABLED_DESCRIPTION);
            CONTRACT_REWARD_QUOTA_MULTIPLIER    = SyncedBindingExtensions.BindSyncedEntry(cfg,topSection, LGUConstants.CONTRACT_QUOTA_MULTIPLIER_KEY        , LGUConstants.CONTRACT_QUOTA_MULTIPLIER_DEFAULT, LGUConstants.CONTRACT_QUOTA_MULTIPLIER_DESCRIPTION);

            #endregion

            #region Items

            #region Weak Portable Teleporter

            topSection = RegularPortableTeleporter.ITEM_NAME;
            WEAK_TELE_ENABLED = SyncedBindingExtensions.BindSyncedEntry(cfg, topSection, LGUConstants.PORTABLE_TELEPORTER_ENABLED_KEY, LGUConstants.PORTABLE_TELEPORTER_ENABLED_DEFAULT);
            WEAK_TELE_PRICE = SyncedBindingExtensions.BindSyncedEntry(cfg, topSection, LGUConstants.PORTABLE_TELEPORTER_PRICE_KEY, LGUConstants.PORTABLE_TELEPORTER_PRICE_DEFAULT);
            CHANCE_TO_BREAK = SyncedBindingExtensions.BindSyncedEntry(cfg, topSection, LGUConstants.PORTABLE_TELEPORTER_BREAK_CHANCE_KEY, LGUConstants.PORTABLE_TELEPORTER_BREAK_CHANCE_DEFAULT, LGUConstants.PORTABLE_TELEPORTER_BREAK_CHANCE_DESCRIPTION);
            KEEP_ITEMS_ON_TELE = SyncedBindingExtensions.BindSyncedEntry(cfg, topSection, LGUConstants.PORTABLE_TELEPORTER_KEEP_ITEMS_KEY, LGUConstants.PORTABLE_TELEPORTER_KEEP_ITEMS_DEFAULT, LGUConstants.PORTABLE_TELEPORTER_KEEP_ITEMS_DESCRIPTION);

            #endregion

            #region Advanced Portable Teleporter

            topSection = AdvancedPortableTeleporter.ITEM_NAME;
            ADVANCED_TELE_ENABLED = SyncedBindingExtensions.BindSyncedEntry(cfg, topSection, LGUConstants.ADVANCED_PORTABLE_TELEPORTER_ENABLED_KEY, LGUConstants.ADVANCED_PORTABLE_TELEPORTER_ENABLED_DEFAULT);
            ADVANCED_TELE_PRICE = SyncedBindingExtensions.BindSyncedEntry(cfg, topSection, LGUConstants.ADVANCED_PORTABLE_TELEPORTER_PRICE_KEY, LGUConstants.ADVANCED_PORTABLE_TELEPORTER_PRICE_DEFAULT);
            ADV_CHANCE_TO_BREAK = SyncedBindingExtensions.BindSyncedEntry(cfg, topSection, LGUConstants.ADVANCED_PORTABLE_TELEPORTER_BREAK_CHANCE_KEY, LGUConstants.ADVANCED_PORTABLE_TELEPORTER_BREAK_CHANCE_DEFAULT, LGUConstants.ADVANCED_PORTABLE_TELEPORTER_BREAK_CHANCE_DESCRIPTION);
            ADV_KEEP_ITEMS_ON_TELE = SyncedBindingExtensions.BindSyncedEntry(cfg, topSection, LGUConstants.ADVANCED_PORTABLE_TELEPORTER_KEEP_ITEMS_KEY, LGUConstants.ADVANCED_PORTABLE_TELEPORTER_KEEP_ITEMS_DEFAULT, LGUConstants.ADVANCED_PORTABLE_TELEPORTER_KEEP_ITEMS_DESCRIPTION);

            #endregion

            #region Helmet

            topSection = Helmet.ITEM_NAME;
            HELMET_ENABLED = SyncedBindingExtensions.BindSyncedEntry(cfg, topSection, LGUConstants.HELMET_ENABLED_KEY, LGUConstants.HELMET_ENABLED_DEFAULT);
            HELMET_PRICE = SyncedBindingExtensions.BindSyncedEntry(cfg, topSection, LGUConstants.HELMET_PRICE_KEY, LGUConstants.HELMET_PRICE_DEFAULT);
            HELMET_HITS_BLOCKED = SyncedBindingExtensions.BindSyncedEntry(cfg, topSection, LGUConstants.HELMET_AMOUNT_OF_HITS_KEY, LGUConstants.HELMET_AMOUNT_OF_HITS_DEFAULT);

            #endregion

            #region Peeper

            topSection = Peeper.ITEM_NAME;
            PEEPER_ENABLED = SyncedBindingExtensions.BindSyncedEntry(cfg, topSection, LGUConstants.PEEPER_ENABLED_KEY, LGUConstants.PEEPER_ENABLED_DEFAULT, LGUConstants.PEEPER_ENABLED_DESCRIPTION);
            PEEPER_PRICE = SyncedBindingExtensions.BindSyncedEntry(cfg, topSection, LGUConstants.PEEPER_PRICE_KEY, LGUConstants.PEEPER_PRICE_DEFAULT, LGUConstants.PEEPER_PRICE_DESCRIPTION);

            #endregion

            #region Medkit

            topSection = Medkit.ITEM_NAME;
            MEDKIT_ENABLED = SyncedBindingExtensions.BindSyncedEntry(cfg, topSection, LGUConstants.MEDKIT_ENABLED_KEY, LGUConstants.MEDKIT_ENABLED_DEFAULT, LGUConstants.MEDKIT_ENABLED_DESCRIPTION);
            MEDKIT_PRICE = SyncedBindingExtensions.BindSyncedEntry(cfg, topSection, LGUConstants.MEDKIT_PRICE_KEY, LGUConstants.MEDKIT_PRICE_DEFAULT, LGUConstants.MEDKIT_PRICE_DESCRIPTION);
            MEDKIT_HEAL_VALUE = SyncedBindingExtensions.BindSyncedEntry(cfg, topSection, LGUConstants.MEDKIT_HEAL_AMOUNT_KEY, LGUConstants.MEDKIT_HEAL_AMOUNT_DEFAULT, LGUConstants.MEDKIT_HEAL_AMOUNT_DESCRIPTION);
            MEDKIT_USES = SyncedBindingExtensions.BindSyncedEntry(cfg, topSection, LGUConstants.MEDKIT_USES_KEY, LGUConstants.MEDKIT_USES_DEFAULT, LGUConstants.MEDKIT_USES_DESCRIPTION);

            #endregion

            #region Diving Kit

            topSection = DivingKit.ITEM_NAME;
            DIVEKIT_ENABLED = SyncedBindingExtensions.BindSyncedEntry(cfg, topSection, LGUConstants.DIVING_KIT_ENABLED_KEY, LGUConstants.DIVING_KIT_ENABLED_DEFAULT, LGUConstants.DIVING_KIT_ENABLED_DESCRIPTION);
            DIVEKIT_PRICE = SyncedBindingExtensions.BindSyncedEntry(cfg, topSection, LGUConstants.DIVING_KIT_PRICE_KEY, LGUConstants.DIVING_KIT_PRICE_DEFAULT, LGUConstants.DIVING_KIT_PRICE_DESCRIPTION);
            DIVEKIT_WEIGHT = SyncedBindingExtensions.BindSyncedEntry(cfg, topSection, LGUConstants.DIVING_KIT_WEIGHT_KEY, LGUConstants.DIVING_KIT_WEIGHT_DEFAULT, LGUConstants.DIVING_KIT_WEIGHT_DESCRIPTION);
            DIVEKIT_TWO_HANDED = SyncedBindingExtensions.BindSyncedEntry(cfg, topSection, LGUConstants.DIVING_KIT_TWO_HANDED_KEY, LGUConstants.DIVING_KIT_TWO_HANDED_DEFAULT, LGUConstants.DIVING_KIT_TWO_HANDED_DESCRIPTION);

            #endregion

            #endregion

            #region Upgrades

            #region Aluminium Coils

            topSection = AluminiumCoils.UPGRADE_NAME;
            ALUMINIUM_COILS_ENABLED = SyncedBindingExtensions.BindSyncedEntry(cfg, topSection, LGUConstants.ALUMINIUM_COILS_ENABLED_KEY, LGUConstants.ALUMINIUM_COILS_ENABLED_DEFAULT, LGUConstants.ALUMINIUM_COILS_ENABLED_DESCRIPTION);
            ALUMINIUM_COILS_INDIVIDUAL = SyncedBindingExtensions.BindSyncedEntry(cfg, topSection, BaseUpgrade.INDIVIDUAL_SECTION, BaseUpgrade.INDIVIDUAL_DEFAULT, BaseUpgrade.INDIVIDUAL_DESCRIPTION);
            ALUMINIUM_COILS_PRICE = SyncedBindingExtensions.BindSyncedEntry(cfg, topSection, LGUConstants.ALUMINIUM_COILS_PRICE_KEY, LGUConstants.ALUMINIUM_COILS_PRICE_DEFAULT);
            ALUMINIUM_COILS_PRICES = SyncedBindingExtensions.BindSyncedEntry(cfg, topSection, BaseUpgrade.PRICES_SECTION, AluminiumCoils.DEFAULT_PRICES, BaseUpgrade.PRICES_DESCRIPTION);
            ALUMINIUM_COILS_INITIAL_DIFFICULTY_DECREASE = SyncedBindingExtensions.BindSyncedEntry(cfg, topSection, LGUConstants.ALUMINIUM_COILS_INITIAL_DIFFICULTY_MULTIPLIER_KEY, LGUConstants.ALUMINIUM_COILS_INITIAL_DIFFICULTY_MULTIPLIER_DEFAULT, LGUConstants.ALUMINIUM_COILS_INITIAL_DIFFICULTY_MULTIPLIER_DESCRIPTION);
            ALUMINIUM_COILS_INCREMENTAL_DIFFICULTY_DECREASE = SyncedBindingExtensions.BindSyncedEntry(cfg, topSection, LGUConstants.ALUMINIUM_COILS_INCREMENTAL_DIFFICULTY_MULTIPLIER_KEY, LGUConstants.ALUMINIUM_COILS_INCREMENTAL_DIFFICULTY_MULTIPLIER_DEFAULT, LGUConstants.ALUMINIUM_COILS_INCREMENTAL_DIFFICULTY_MULTIPLIER_DESCRIPTION);
            ALUMINIUM_COILS_INITIAL_STUN_TIMER_INCREASE = SyncedBindingExtensions.BindSyncedEntry(cfg, topSection, LGUConstants.ALUMINIUM_COILS_INITIAL_STUN_TIMER_KEY, LGUConstants.ALUMINIUM_COILS_INITIAL_STUN_TIMER_DEFAULT);
            ALUMINIUM_COILS_INCREMENTAL_STUN_TIMER_INCREASE = SyncedBindingExtensions.BindSyncedEntry(cfg, topSection, LGUConstants.ALUMINIUM_COILS_INCREMENTAL_STUN_TIMER_KEY, LGUConstants.ALUMINIUM_COILS_INCREMENTAL_STUN_TIMER_DEFAULT);
            ALUMINIUM_COILS_INITIAL_RANGE_INCREASE = SyncedBindingExtensions.BindSyncedEntry(cfg, topSection, LGUConstants.ALUMINIUM_COILS_INITIAL_RANGE_KEY, LGUConstants.ALUMINIUM_COILS_INITIAL_RANGE_DEFAULT);
            ALUMINIUM_COILS_INCREMENTAL_RANGE_INCREASE = SyncedBindingExtensions.BindSyncedEntry(cfg, topSection, LGUConstants.ALUMINIUM_COILS_INCREMENTAL_RANGE_KEY, LGUConstants.ALUMINIUM_COILS_INCREMENTAL_RANGE_DEFAULT);
            ALUMINIUM_COILS_INITIAL_COOLDOWN_DECREASE = SyncedBindingExtensions.BindSyncedEntry(cfg, topSection, LGUConstants.ALUMINIUM_COILS_INITIAL_COOLDOWN_KEY, LGUConstants.ALUMINIUM_COILS_INITIAL_COOLDOWN_DEFAULT, LGUConstants.ALUMINIUM_COILS_INITIAL_COOLDOWN_DESCRIPTION);
            ALUMINIUM_COILS_INCREMENTAL_COOLDOWN_DECREASE = SyncedBindingExtensions.BindSyncedEntry(cfg, topSection, LGUConstants.ALUMINIUM_COILS_INCREMENTAL_COOLDOWN_KEY, LGUConstants.ALUMINIUM_COILS_INCREMENTAL_COOLDOWN_DEFAULT, LGUConstants.ALUMINIUM_COILS_INCREMENTAL_COOLDOWN_DESCRIPTION);

            #endregion

            #region Back Muscles

            topSection = BackMuscles.UPGRADE_NAME;
            BACK_MUSCLES_ENABLED = SyncedBindingExtensions.BindSyncedEntry(cfg, topSection, LGUConstants.BACK_MUSCLES_ENABLED_KEY, LGUConstants.BACK_MUSCLES_ENABLED_DEFAULT, LGUConstants.BACK_MUSCLES_ENABLED_DESCRIPTION);
            BACK_MUSCLES_PRICE = SyncedBindingExtensions.BindSyncedEntry(cfg, topSection, LGUConstants.BACK_MUSCLES_PRICE_KEY, LGUConstants.BACK_MUSCLES_PRICE_DEFAULT);
            CARRY_WEIGHT_REDUCTION = SyncedBindingExtensions.BindSyncedEntry(cfg, topSection, LGUConstants.BACK_MUSCLES_INITIAL_WEIGHT_MULTIPLIER_KEY, LGUConstants.BACK_MUSCLES_INITIAL_WEIGHT_MULTIPLIER_DEFAULT, LGUConstants.BACK_MUSCLES_INITIAL_WEIGHT_MULTIPLIER_DESCRIPTION);
            CARRY_WEIGHT_INCREMENT = SyncedBindingExtensions.BindSyncedEntry(cfg, topSection, LGUConstants.BACK_MUSCLES_INCREMENTAL_WEIGHT_MULTIPLIER_KEY, LGUConstants.BACK_MUSCLES_INCREMENTAL_WEIGHT_MULTIPLIER_DEFAULT, LGUConstants.BACK_MUSCLES_INCREMENTAL_WEIGHT_MULTIPLIER_DESCRIPTION);
            BACK_MUSCLES_UPGRADE_PRICES = SyncedBindingExtensions.BindSyncedEntry(cfg, topSection, BaseUpgrade.PRICES_SECTION, BackMuscles.PRICES_DEFAULT, BaseUpgrade.PRICES_DESCRIPTION);
            BACK_MUSCLES_INDIVIDUAL = SyncedBindingExtensions.BindSyncedEntry(cfg, topSection, BaseUpgrade.INDIVIDUAL_SECTION, BaseUpgrade.INDIVIDUAL_DEFAULT, BaseUpgrade.INDIVIDUAL_DESCRIPTION);

            #endregion

            #region Bargain Connections

            topSection = BargainConnections.UPGRADE_NAME;
            BARGAIN_CONNECTIONS_ENABLED = SyncedBindingExtensions.BindSyncedEntry(cfg, topSection, LGUConstants.BARGAIN_CONNECTIONS_ENABLED_KEY, LGUConstants.BARGAIN_CONNECTIONS_ENABLED_DEFAULT, LGUConstants.BARGAIN_CONNECTIONS_ENABLED_DESCRIPTION);
            BARGAIN_CONNECTIONS_PRICE = SyncedBindingExtensions.BindSyncedEntry(cfg, topSection, LGUConstants.BARGAIN_CONNECTIONS_PRICE_KEY, LGUConstants.BARGAIN_CONNECTIONS_PRICE_DEFAULT);
            BARGAIN_CONNECTIONS_INITIAL_ITEM_AMOUNT = SyncedBindingExtensions.BindSyncedEntry(cfg, topSection, LGUConstants.BARGAIN_CONNECTIONS_INITIAL_AMOUNT_KEY, LGUConstants.BARGAIN_CONNECTIONS_INITIAL_AMOUNT_DEFAULT);
            BARGAIN_CONNECTIONS_INCREMENTAL_ITEM_AMOUNT = SyncedBindingExtensions.BindSyncedEntry(cfg, topSection, LGUConstants.BARGAIN_CONNECTIONS_INCREMENTAL_AMOUNT_KEY, LGUConstants.BARGAIN_CONNECTIONS_INCREMENTAL_AMOUNT_DEFAULT);
            BARGAIN_CONNECTIONS_PRICES = SyncedBindingExtensions.BindSyncedEntry(cfg, topSection, BaseUpgrade.PRICES_SECTION, BargainConnections.PRICES_DEFAULT, BaseUpgrade.PRICES_DESCRIPTION);

            #endregion

            #region Beekeeper

            topSection = Beekeeper.UPGRADE_NAME;
            BEEKEEPER_ENABLED = SyncedBindingExtensions.BindSyncedEntry(cfg, topSection, LGUConstants.BEEKEEPER_ENABLED_KEY, LGUConstants.BEEKEEPER_ENABLED_DEFAULT, LGUConstants.BEEKEEPER_ENABLED_DESCRIPTION);
            BEEKEEPER_PRICE = SyncedBindingExtensions.BindSyncedEntry(cfg, topSection, LGUConstants.BEEKEEPER_PRICE_KEY, LGUConstants.BEEKEEPER_PRICE_DEFAULT);
            BEEKEEPER_UPGRADE_PRICES = SyncedBindingExtensions.BindSyncedEntry(cfg, topSection, BaseUpgrade.PRICES_SECTION, Beekeeper.PRICES_DEFAULT, BaseUpgrade.PRICES_DESCRIPTION);
            BEEKEEPER_INDIVIDUAL = SyncedBindingExtensions.BindSyncedEntry(cfg, topSection, BaseUpgrade.INDIVIDUAL_SECTION, BaseUpgrade.INDIVIDUAL_DEFAULT, BaseUpgrade.INDIVIDUAL_DESCRIPTION);
            BEEKEEPER_DAMAGE_MULTIPLIER = SyncedBindingExtensions.BindSyncedEntry(cfg, topSection, LGUConstants.BEEKEEPER_DAMAGE_MULTIPLIER_KEY, LGUConstants.BEEKEEPER_DAMAGE_MULTIPLIER_DEFAULT, LGUConstants.BEEKEEPER_DAMAGE_MULTIPLIER_DESCRIPTION);
            BEEKEEPER_DAMAGE_MULTIPLIER_INCREMENT = SyncedBindingExtensions.BindSyncedEntry(cfg, topSection, LGUConstants.BEEKEEPER_DAMAGE_INCREMENTAL_MULTIPLIER_KEY, LGUConstants.BEEKEEPER_DAMAGE_INCREMENTAL_MULTIPLIER_DEFAULT, LGUConstants.BEEKEEPER_DAMAGE_INCREMENTAL_MULTIPLIER_DESCRIPTION);
            BEEKEEPER_HIVE_VALUE_INCREASE = SyncedBindingExtensions.BindSyncedEntry(cfg, topSection, LGUConstants.BEEKEEPER_HIVE_MULTIPLIER_KEY, LGUConstants.BEEKEEPER_HIVE_MULTIPLIER_DEFAULT, LGUConstants.BEEKEEPER_HIVE_MULTIPLIER_DESCRIPTION);

            #endregion

            #region Better Scanner

            topSection = BetterScanner.UPGRADE_NAME;
            BETTER_SCANNER_ENABLED = SyncedBindingExtensions.BindSyncedEntry(cfg, topSection, LGUConstants.BETTER_SCANNER_ENABLED_KEY, LGUConstants.BETTER_SCANNER_ENABLED_DEFAULT, LGUConstants.BETTER_SCANNER_ENABLED_DESCRIPTION);
            BETTER_SCANNER_PRICE = SyncedBindingExtensions.BindSyncedEntry(cfg, topSection, LGUConstants.BETTER_SCANNER_PRICE_KEY, LGUConstants.BETTER_SCANNER_PRICE_DEFAULT);
            SHIP_AND_ENTRANCE_DISTANCE_INCREASE = SyncedBindingExtensions.BindSyncedEntry(cfg, topSection, LGUConstants.BETTER_SCANNER_OUTSIDE_NODE_DISTANCE_INCREASE_KEY, LGUConstants.BETTER_SCANNER_OUTSIDE_NODE_DISTANCE_INCREASE_DEFAULT, LGUConstants.BETTER_SCANNER_OUTSIDE_NODE_DISTANCE_INCREASE_DESCRIPTION);
            NODE_DISTANCE_INCREASE = SyncedBindingExtensions.BindSyncedEntry(cfg, topSection, LGUConstants.BETTER_SCANNER_NODE_DISTANCE_INCREASE_KEY, LGUConstants.BETTER_SCANNER_NODE_DISTANCE_INCREASE_DEFAULT, LGUConstants.BETTER_SCANNER_NODE_DISTANCE_INCREASE_DESCRIPTION);
            BETTER_SCANNER_INDIVIDUAL = SyncedBindingExtensions.BindSyncedEntry(cfg, topSection, BaseUpgrade.INDIVIDUAL_SECTION, BaseUpgrade.INDIVIDUAL_DEFAULT, BaseUpgrade.INDIVIDUAL_DESCRIPTION);
            BETTER_SCANNER_PRICE2 = SyncedBindingExtensions.BindSyncedEntry(cfg, topSection, LGUConstants.BETTER_SCANNER_SECOND_TIER_PRICE_KEY, LGUConstants.BETTER_SCANNER_SECOND_TIER_PRICE_DEFAULT, LGUConstants.BETTER_SCANNER_SECOND_TIER_PRICE_DESCRIPTION);
            BETTER_SCANNER_PRICE3 = SyncedBindingExtensions.BindSyncedEntry(cfg, topSection, LGUConstants.BETTER_SCANNER_THIRD_TIER_PRICE_KEY, LGUConstants.BETTER_SCANNER_THIRD_TIER_PRICE_DEFAULT, LGUConstants.BETTER_SCANNER_THIRD_TIER_PRICE_DESCRIPTION);
            BETTER_SCANNER_ENEMIES = SyncedBindingExtensions.BindSyncedEntry(cfg, topSection, LGUConstants.BETTER_SCANNER_ENEMIES_THROUGH_WALLS_KEY, LGUConstants.BETTER_SCANNER_ENEMIES_THROUGH_WALLS_DEFAULT, LGUConstants.BETTER_SCANNER_ENEMIES_THROUGH_WALLS_DESCRIPTION);
            VERBOSE_ENEMIES = SyncedBindingExtensions.BindSyncedEntry(cfg, topSection, LGUConstants.BETTER_SCANNER_VERBOSE_ENEMIES_KEY, LGUConstants.BETTER_SCANNER_VERBOSE_ENEMIES_DEFAULT, LGUConstants.BETTER_SCANNER_VERBOSE_ENEMIES_DESCRIPTION);

            #endregion

            #region Bigger Lungs

            topSection = BiggerLungs.UPGRADE_NAME;
            BIGGER_LUNGS_ENABLED = SyncedBindingExtensions.BindSyncedEntry(cfg, topSection, LGUConstants.BIGGER_LUNGS_ENABLED_KEY, LGUConstants.BIGGER_LUNGS_ENABLED_DEFAULT, LGUConstants.BIGGER_LUNGS_ENABLED_DESCRIPTION);
            BIGGER_LUNGS_PRICE = SyncedBindingExtensions.BindSyncedEntry(cfg, topSection, LGUConstants.BIGGER_LUNGS_PRICE_KEY, LGUConstants.BIGGER_LUNGS_PRICE_DEFAULT);
            SPRINT_TIME_INCREASE_UNLOCK = SyncedBindingExtensions.BindSyncedEntry(cfg, topSection, LGUConstants.BIGGER_LUNGS_INITIAL_SPRINT_TIME_KEY, LGUConstants.BIGGER_LUNGS_INITIAL_SPRINT_TIME_DEFAULT, LGUConstants.BIGGER_LUNGS_INITIAL_SPRINT_TIME_DESCRIPTION);
            SPRINT_TIME_INCREMENT = SyncedBindingExtensions.BindSyncedEntry(cfg, topSection, LGUConstants.BIGGER_LUNGS_INCREMENTAL_SPRINT_TIME_KEY, LGUConstants.BIGGER_LUNGS_INCREMENTAL_SPRINT_TIME_DEFAULT, LGUConstants.BIGGER_LUNGS_INCREMENTAL_SPRINT_TIME_DESCRIPTION);
            BIGGER_LUNGS_UPGRADE_PRICES = SyncedBindingExtensions.BindSyncedEntry(cfg, topSection, BaseUpgrade.PRICES_SECTION, BiggerLungs.PRICES_DEFAULT, BaseUpgrade.PRICES_DESCRIPTION);
            BIGGER_LUNGS_INDIVIDUAL = SyncedBindingExtensions.BindSyncedEntry(cfg, topSection, BaseUpgrade.INDIVIDUAL_SECTION, BaseUpgrade.INDIVIDUAL_DEFAULT, BaseUpgrade.INDIVIDUAL_DESCRIPTION);
            BIGGER_LUNGS_STAMINA_REGEN_APPLY_LEVEL = SyncedBindingExtensions.BindSyncedEntry(cfg, topSection, LGUConstants.BIGGER_LUNGS_STAMINA_APPLY_LEVEL_KEY, LGUConstants.BIGGER_LUNGS_STAMINA_APPLY_LEVEL_DEFAULT, LGUConstants.BIGGER_LUNGS_STAMINA_APPLY_LEVEL_DESCRIPTION);
            BIGGER_LUNGS_STAMINA_REGEN_INCREASE = SyncedBindingExtensions.BindSyncedEntry(cfg, topSection, LGUConstants.BIGGER_LUNGS_INITIAL_STAMINA_REGEN_KEY, LGUConstants.BIGGER_LUNGS_INITIAL_STAMINA_REGEN_DEFAULT, LGUConstants.BIGGER_LUNGS_INITIAL_STAMINA_REGEN_DESCRIPTION);
            BIGGER_LUNGS_STAMINA_REGEN_INCREMENTAL_INCREASE = SyncedBindingExtensions.BindSyncedEntry(cfg, topSection, LGUConstants.BIGGER_LUNGS_INCREMENTAL_STAMINA_REGEN_KEY, LGUConstants.BIGGER_LUNGS_INCREMENTAL_STAMINA_REGEN_DEFAULT, LGUConstants.BIGGER_LUNGS_INCREMENTAL_STAMINA_REGEN_DESCRIPTION);
            BIGGER_LUNGS_JUMP_STAMINA_APPLY_LEVEL = SyncedBindingExtensions.BindSyncedEntry(cfg, topSection, LGUConstants.BIGGER_LUNGS_JUMP_STAMINA_DECREASE_APPLY_LEVEL_KEY, LGUConstants.BIGGER_LUNGS_JUMP_STAMINA_DECREASE_APPLY_LEVEL_DEFAULT, LGUConstants.BIGGER_LUNGS_JUMP_STAMINA_DECREASE_APPLY_LEVEL_DESCRIPTION);
            BIGGER_LUNGS_JUMP_STAMINA_COST_DECREASE = SyncedBindingExtensions.BindSyncedEntry(cfg, topSection, LGUConstants.BIGGER_LUNGS_INITIAL_JUMP_STAMINA_DECREASE_KEY, LGUConstants.BIGGER_LUNGS_INITIAL_JUMP_STAMINA_DECREASE_DEFAULT, LGUConstants.BIGGER_LUNGS_INITIAL_JUMP_STAMINA_DECREASE_DESCRIPTION);
            BIGGER_LUNGS_JUMP_STAMINA_COST_INCREMENTAL_DECREASE = SyncedBindingExtensions.BindSyncedEntry(cfg, topSection, LGUConstants.BIGGER_LUNGS_INCREMENTAL_JUMP_STAMINA_DECREASE_KEY, LGUConstants.BIGGER_LUNGS_INCREMENTAL_JUMP_STAMINA_DECREASE_DEFAULT, LGUConstants.BIGGER_LUNGS_INCREMENTAL_JUMP_STAMINA_DECREASE_DESCRIPTION);

            #endregion

            #region Charging Booster

            topSection = ChargingBooster.UPGRADE_NAME;
            CHARGING_BOOSTER_ENABLED = SyncedBindingExtensions.BindSyncedEntry(cfg, topSection, LGUConstants.CHARGING_BOOSTER_ENABLED_KEY, LGUConstants.CHARGING_BOOSTER_ENABLED_DEFAULT, LGUConstants.CHARGING_BOOSTER_ENABLED_DESCRIPTION);
            CHARGING_BOOSTER_PRICE = SyncedBindingExtensions.BindSyncedEntry(cfg, topSection, LGUConstants.CHARGING_BOOSTER_PRICE_KEY, LGUConstants.CHARGING_BOOSTER_PRICE_DEFAULT);
            CHARGING_BOOSTER_PRICES = SyncedBindingExtensions.BindSyncedEntry(cfg, topSection, BaseUpgrade.PRICES_SECTION, LGUConstants.CHARGING_BOOSTER_PRICES_DEFAULT, BaseUpgrade.PRICES_DESCRIPTION);
            CHARGING_BOOSTER_COOLDOWN = SyncedBindingExtensions.BindSyncedEntry(cfg, topSection, LGUConstants.CHARGING_BOOSTER_COOLDOWN_KEY, LGUConstants.CHARGING_BOOSTER_COOLDOWN_DEFAULT);
            CHARGING_BOOSTER_INCREMENTAL_COOLDOWN_DECREASE = SyncedBindingExtensions.BindSyncedEntry(cfg, topSection, LGUConstants.CHARGING_BOOSTER_INCREMENTAL_COOLDOWN_DECREASE_KEY, LGUConstants.CHARGING_BOOSTER_INCREMENTAL_COOLDOWN_DECREASE_DEFAULT);
            CHARGING_BOOSTER_CHARGE_PERCENTAGE = SyncedBindingExtensions.BindSyncedEntry(cfg, topSection, LGUConstants.CHARGING_BOOSTER_CHARGE_PERCENTAGE_KEY, LGUConstants.CHARGING_BOOSTER_CHARGE_PERCENTAGE_DEFAULT);

            #endregion
                
            #region Climbing Gloves

            topSection = ClimbingGloves.UPGRADE_NAME;
            CLIMBING_GLOVES_ENABLED             = SyncedBindingExtensions.BindSyncedEntry(cfg, topSection, LGUConstants.CLIMBING_GLOVES_ENABLED_KEY, LGUConstants.CLIMBING_GLOVES_ENABLED_DEFAULT, LGUConstants.CLIMBING_GLOVES_ENABLED_DESCRIPTION);
            CLIMBING_GLOVES_INDIVIDUAL          = SyncedBindingExtensions.BindSyncedEntry(cfg, topSection, BaseUpgrade.INDIVIDUAL_SECTION, BaseUpgrade.INDIVIDUAL_DEFAULT, BaseUpgrade.INDIVIDUAL_DESCRIPTION);
            CLIMBING_GLOVES_PRICE               = SyncedBindingExtensions.BindSyncedEntry(cfg, topSection, LGUConstants.CLIMBING_GLOVES_PRICE_KEY, LGUConstants.CLIMBING_GLOVES_PRICE_DEFAULT);
            CLIMBING_GLOVES_PRICES              = SyncedBindingExtensions.BindSyncedEntry(cfg, topSection, BaseUpgrade.PRICES_SECTION, ClimbingGloves.DEFAULT_PRICES, BaseUpgrade.PRICES_DESCRIPTION);
            INITIAL_CLIMBING_SPEED_BOOST        = SyncedBindingExtensions.BindSyncedEntry(cfg, topSection, LGUConstants.CLIMBING_GLOVES_INITIAL_MULTIPLIER_KEY, LGUConstants.CLIMBING_GLOVES_INITIAL_MULTIPLIER_DEFAULT, LGUConstants.CLIMBING_GLOVES_INITIAL_MULTIPLIER_DESCRIPTION);
            INCREMENTAL_CLIMBING_SPEED_BOOST    = SyncedBindingExtensions.BindSyncedEntry(cfg, topSection, LGUConstants.CLIMBING_GLOVES_INCREMENTAL_MULTIPLIER_KEY, LGUConstants.CLIMBING_GLOVES_INCREMENTAL_MULTIPLIER_DEFAULT);

            #endregion

            #region Discombobulator

            topSection = Discombobulator.UPGRADE_NAME;
            DISCOMBOBULATOR_ENABLED = SyncedBindingExtensions.BindSyncedEntry(cfg, topSection, LGUConstants.DISCOMBOBULATOR_ENABLED_KEY, LGUConstants.DISCOMBOBULATOR_ENABLED_DEFAULT, LGUConstants.DISCOMBOBULATOR_ENABLED_DESCRIPTION);
            DISCOMBOBULATOR_PRICE = SyncedBindingExtensions.BindSyncedEntry(cfg, topSection, LGUConstants.DISCOMBOBULATOR_PRICE_KEY, LGUConstants.DISCOMBOBULATOR_PRICE_DEFAULT);
            DISCOMBOBULATOR_COOLDOWN = SyncedBindingExtensions.BindSyncedEntry(cfg, topSection, LGUConstants.DISCOMBOBULATOR_COOLDOWN_KEY, LGUConstants.DISCOMBOBULATOR_COOLDOWN_DEFAULT);
            DISCOMBOBULATOR_RADIUS = SyncedBindingExtensions.BindSyncedEntry(cfg, topSection, LGUConstants.DISCOMBOBULATOR_EFFECT_RADIUS_KEY, LGUConstants.DISCOMBOBULATOR_EFFECT_RADIUS_DEFAULT);
            DISCOMBOBULATOR_STUN_DURATION = SyncedBindingExtensions.BindSyncedEntry(cfg, topSection, LGUConstants.DISCOMBOBULATOR_STUN_DURATION_KEY, LGUConstants.DISCOMBOBULATOR_STUN_DURATION_DEFAULT);
            DISCOMBOBULATOR_NOTIFY_CHAT = SyncedBindingExtensions.BindSyncedEntry(cfg, topSection, LGUConstants.DISCOMBOBULATOR_NOTIFY_CHAT_KEY, LGUConstants.DISCOMBOBULATOR_NOTIFY_CHAT_DEFAULT);
            DISCOMBOBULATOR_INCREMENT = SyncedBindingExtensions.BindSyncedEntry(cfg, topSection, LGUConstants.DISCOMBOBULATOR_INCREMENTAL_STUN_TIME_KEY, LGUConstants.DISCOMBOBULATOR_INCREMENTAL_STUN_TIME_DEFAULT, LGUConstants.DISCOMBOBULATOR_INCREMENTAL_STUN_TIME_DESCRIPTION);
            DISCO_UPGRADE_PRICES = SyncedBindingExtensions.BindSyncedEntry(cfg, topSection, BaseUpgrade.PRICES_SECTION, Discombobulator.PRICES_DEFAULT, BaseUpgrade.PRICES_DESCRIPTION);
            DISCOMBOBULATOR_INDIVIDUAL = SyncedBindingExtensions.BindSyncedEntry(cfg, topSection, BaseUpgrade.INDIVIDUAL_SECTION, BaseUpgrade.INDIVIDUAL_DEFAULT, BaseUpgrade.INDIVIDUAL_DESCRIPTION);
            DISCOMBOBULATOR_DAMAGE_LEVEL = SyncedBindingExtensions.BindSyncedEntry(cfg, topSection, LGUConstants.DISCOMBOBULATOR_APPLY_DAMAGE_LEVEL_KEY, LGUConstants.DISCOMBOBULATOR_APPLY_DAMAGE_LEVEL_DEFAULT, LGUConstants.DISCOMBOBULATOR_APPLY_DAMAGE_LEVEL_DESCRIPTION);
            DISCOMBOBULATOR_INITIAL_DAMAGE = SyncedBindingExtensions.BindSyncedEntry(cfg, topSection, LGUConstants.DISCOMBOBULATOR_INITIAL_DAMAGE_KEY, LGUConstants.DISCOMBOBULATOR_INITIAL_DAMAGE_DEFAULT, LGUConstants.DISCOMBOBULATOR_INITIAL_DAMAGE_DESCRIPTION);
            DISCOMBOBULATOR_DAMAGE_INCREASE = SyncedBindingExtensions.BindSyncedEntry(cfg, topSection, LGUConstants.DISCOMBOBULATOR_INCREMENTAL_DAMAGE_KEY, LGUConstants.DISCOMBOBULATOR_INCREMENTAL_DAMAGE_DEFAULT, LGUConstants.DISCOMBOBULATOR_INCREMENTAL_DAMAGE_DESCRIPTION);

            #endregion

            #region Drop Pod Thrusters

            topSection = FasterDropPod.UPGRADE_NAME;
            FASTER_DROP_POD_ENABLED = SyncedBindingExtensions.BindSyncedEntry(cfg, topSection, LGUConstants.DROP_POD_THRUSTERS_ENABLED_KEY, LGUConstants.DROP_POD_THRUSTERS_ENABLED_DEFAULT, LGUConstants.DROP_POD_THRUSTERS_ENABLED_DESCRIPTION);
            FASTER_DROP_POD_PRICE = SyncedBindingExtensions.BindSyncedEntry(cfg, topSection, LGUConstants.DROP_POD_THRUSTERS_PRICE_KEY, LGUConstants.DROP_POD_THRUSTERS_PRICE_DEFAULT, LGUConstants.DROP_POD_THRUSTERS_PRICE_DESCRIPTION);
            FASTER_DROP_POD_TIMER = SyncedBindingExtensions.BindSyncedEntry(cfg, topSection, LGUConstants.DROP_POD_THRUSTERS_TIME_DECREASE_KEY, LGUConstants.DROP_POD_THRUSTERS_TIME_DECREASE_DEFAULT);
            FASTER_DROP_POD_INITIAL_TIMER = SyncedBindingExtensions.BindSyncedEntry(cfg, topSection, LGUConstants.DROP_POD_THRUSTERS_FIRST_TIME_DECREASE_KEY, LGUConstants.DROP_POD_THRUSTERS_FIRST_TIME_DECREASE_DEFAULT);

            #endregion

            #region Efficient Engines

            topSection = EfficientEngines.UPGRADE_NAME;
            EFFICIENT_ENGINES_ENABLED = SyncedBindingExtensions.BindSyncedEntry(cfg, topSection, LGUConstants.EFFICIENT_ENGINES_ENABLED_KEY, LGUConstants.EFFICIENT_ENGINES_ENABLED_DEFAULT, LGUConstants.EFFICIENT_ENGINES_ENABLED_DESCRIPTION);
            EFFICIENT_ENGINES_PRICE = SyncedBindingExtensions.BindSyncedEntry(cfg, topSection, LGUConstants.EFFICIENT_ENGINES_PRICE_KEY, LGUConstants.EFFICIENT_ENGINES_PRICE_DEFAULT);
            EFFICIENT_ENGINES_PRICES = SyncedBindingExtensions.BindSyncedEntry(cfg, topSection, BaseUpgrade.PRICES_SECTION, EfficientEngines.DEFAULT_PRICES, BaseUpgrade.PRICES_DESCRIPTION);
            EFFICIENT_ENGINES_INITIAL_DISCOUNT = SyncedBindingExtensions.BindSyncedEntry(cfg, topSection, LGUConstants.EFFICIENT_ENGINES_INITIAL_MULTIPLIER_KEY, LGUConstants.EFFICIENT_ENGINES_INITIAL_MULTIPLIER_DEFAULT);
            EFFICIENT_ENGINES_INCREMENTAL_DISCOUNT = SyncedBindingExtensions.BindSyncedEntry(cfg, topSection, LGUConstants.EFFICIENT_ENGINES_INCREMENTAL_MULTIPLIER_KEY, LGUConstants.EFFICIENT_ENGINES_INCREMENTAL_MULTIPLIER_DEFAULT);

            #endregion

            #region Fast Encryption

            topSection = FastEncryption.UPGRADE_NAME;
            PAGER_ENABLED = SyncedBindingExtensions.BindSyncedEntry(cfg, topSection, LGUConstants.FAST_ENCRYPTION_ENABLED_KEY, LGUConstants.FAST_ENCRYPTION_ENABLED_DEFAULT, LGUConstants.FAST_ENCRYPTION_ENABLED_DESCRIPTION);
            PAGER_PRICE = SyncedBindingExtensions.BindSyncedEntry(cfg, topSection, LGUConstants.FAST_ENCRYPTION_PRICE_KEY, LGUConstants.FAST_ENCRYPTION_PRICE_DEFAULT);

            #endregion

            #region Lethal Deals

            topSection = LethalDeals.UPGRADE_NAME;
            LETHAL_DEALS_ENABLED = SyncedBindingExtensions.BindSyncedEntry(cfg, topSection, LGUConstants.LETHAL_DEALS_ENABLED_KEY, LGUConstants.LETHAL_DEALS_ENABLED_DEFAULT, LGUConstants.LETHAL_DEALS_ENABLED_DESCRIPTION);
            LETHAL_DEALS_PRICE = SyncedBindingExtensions.BindSyncedEntry(cfg, topSection, LGUConstants.LETHAL_DEALS_PRICE_KEY, LGUConstants.LETHAL_DEALS_PRICE_DEFAULT);

            #endregion

            #region Lightning Rod

            topSection = LightningRod.UPGRADE_NAME;
            LIGHTNING_ROD_ENABLED = SyncedBindingExtensions.BindSyncedEntry(cfg, topSection, LightningRod.ENABLED_SECTION, LightningRod.ENABLED_DEFAULT, LightningRod.ENABLED_DESCRIPTION);
            LIGHTNING_ROD_PRICE = SyncedBindingExtensions.BindSyncedEntry(cfg, topSection, LightningRod.PRICE_SECTION, LightningRod.PRICE_DEFAULT);
            LIGHTNING_ROD_ACTIVE = SyncedBindingExtensions.BindSyncedEntry(cfg, topSection, LightningRod.ACTIVE_SECTION, LightningRod.ACTIVE_DEFAULT, LightningRod.ACTIVE_DESCRIPTION);
            LIGHTNING_ROD_DIST = SyncedBindingExtensions.BindSyncedEntry(cfg, topSection, LightningRod.DIST_SECTION, LightningRod.DIST_DEFAULT, LightningRod.DIST_DESCRIPTION);

            #endregion

            #region Lithium Batteries

            topSection = LithiumBatteries.UPGRADE_NAME;
            LITHIUM_BATTERIES_ENABLED = SyncedBindingExtensions.BindSyncedEntry(cfg, topSection, LGUConstants.LITHIUM_BATTERIES_ENABLED_KEY, LGUConstants.LITHIUM_BATTERIES_ENABLED_DEFAULT, LGUConstants.LITHIUM_BATTERIES_ENABLED_DESCRIPTION);
            LITHIUM_BATTERIES_INDIVIDUAL = SyncedBindingExtensions.BindSyncedEntry(cfg, topSection, BaseUpgrade.INDIVIDUAL_SECTION, BaseUpgrade.INDIVIDUAL_DEFAULT, BaseUpgrade.INDIVIDUAL_DESCRIPTION);
            LITHIUM_BATTERIES_PRICE = SyncedBindingExtensions.BindSyncedEntry(cfg, topSection, LGUConstants.LITHIUM_BATTERIES_PRICE_KEY, LGUConstants.LITHIUM_BATTERIES_PRICE_DEFAULT);
            LITHIUM_BATTERIES_PRICES = SyncedBindingExtensions.BindSyncedEntry(cfg, topSection, BaseUpgrade.PRICES_SECTION, LithiumBatteries.PRICES_DEFAULT, BaseUpgrade.PRICES_DESCRIPTION);
            LITHIUM_BATTERIES_INITIAL_MULTIPLIER = SyncedBindingExtensions.BindSyncedEntry(cfg, topSection, LGUConstants.LITHIUM_BATTERIES_INITIAL_MULTIPLIER_KEY, LGUConstants.LITHIUM_BATTERIES_INITIAL_MULTIPLIER_DEFAULT, LGUConstants.LITHIUM_BATTERIES_INITIAL_MULTIPLIER_DESCRIPTION);
            LITHIUM_BATTERIES_INCREMENTAL_MULTIPLIER = SyncedBindingExtensions.BindSyncedEntry(cfg, topSection, LGUConstants.LITHIUM_BATTERIES_INCREMENTAL_MULTIPLIER_KEY, LGUConstants.LITHIUM_BATTERIES_INCREMENTAL_MULTIPLIER_DEFAULT, LGUConstants.LITHIUM_BATTERIES_INCREMENTAL_MULTIPLIER_DESCRIPTION);

            #endregion

            #region Locksmith

            topSection = LockSmith.UPGRADE_NAME;
            LOCKSMITH_ENABLED = SyncedBindingExtensions.BindSyncedEntry(cfg, topSection, LGUConstants.LOCKSMITH_ENABLED_KEY, LGUConstants.LOCKSMITH_ENABLED_DEFAULT, LGUConstants.LOCKSMITH_ENABLED_DESCRIPTION);
            LOCKSMITH_PRICE = SyncedBindingExtensions.BindSyncedEntry(cfg, topSection, LGUConstants.LOCKSMITH_PRICE_KEY, LGUConstants.LOCKSMITH_PRICE_DEFAULT, LGUConstants.LOCKSMITH_PRICE_DESCRIPTION);
            LOCKSMITH_INDIVIDUAL = SyncedBindingExtensions.BindSyncedEntry(cfg, topSection, BaseUpgrade.INDIVIDUAL_SECTION, BaseUpgrade.INDIVIDUAL_DEFAULT, BaseUpgrade.INDIVIDUAL_DESCRIPTION);

            #endregion

            #region Malware Broadcaster

            topSection = MalwareBroadcaster.UPGRADE_NAME;
            MALWARE_BROADCASTER_ENABLED = SyncedBindingExtensions.BindSyncedEntry(cfg, topSection, LGUConstants.MALWARE_BROADCASTER_ENABLED_KEY, LGUConstants.MALWARE_BROADCASTER_ENABLED_DEFAULT, LGUConstants.MALWARE_BROADCASTER_ENABLED_DESCRIPTION);
            MALWARE_BROADCASTER_PRICE = SyncedBindingExtensions.BindSyncedEntry(cfg, topSection, LGUConstants.MALWARE_BROADCASTER_PRICE_KEY, LGUConstants.MALWARE_BROADCASTER_PRICE_DEFAULT);
            DESTROY_TRAP = SyncedBindingExtensions.BindSyncedEntry(cfg, topSection, LGUConstants.MALWARE_BROADCASTER_DESTROY_TRAPS_KEY, LGUConstants.MALWARE_BROADCASTER_DESTROY_TRAPS_DEFAULT, LGUConstants.MALWARE_BROADCASTER_DESTROY_TRAPS_DESCRIPTION);
            DISARM_TIME = SyncedBindingExtensions.BindSyncedEntry(cfg, topSection, LGUConstants.MALWARE_BROADCASTER_DISARM_TIME_KEY, LGUConstants.MALWARE_BROADCASTER_DISARM_TIME_DEFAULT, LGUConstants.MALWARE_BROADCASTER_DISARM_TIME_DESCRIPTION);
            EXPLODE_TRAP = SyncedBindingExtensions.BindSyncedEntry(cfg, topSection, LGUConstants.MALWARE_BROADCASTER_EXPLODE_TRAPS_KEY, LGUConstants.MALWARE_BROADCASTER_EXPLODE_TRAPS_DEFAULT, LGUConstants.MALWARE_BROADCASTER_EXPLODE_TRAPS_DESCRIPTION);
            MALWARE_BROADCASTER_INDIVIDUAL = SyncedBindingExtensions.BindSyncedEntry(cfg, topSection, BaseUpgrade.INDIVIDUAL_SECTION, BaseUpgrade.INDIVIDUAL_DEFAULT, BaseUpgrade.INDIVIDUAL_DESCRIPTION);

            #endregion

            #region Market Influence

            topSection = MarketInfluence.UPGRADE_NAME;
            MARKET_INFLUENCE_ENABLED = SyncedBindingExtensions.BindSyncedEntry(cfg, topSection, LGUConstants.MARKET_INFLUENCE_ENABLED_KEY, LGUConstants.MARKET_INFLUENCE_ENABLED_DEFAULT, LGUConstants.MARKET_INFLUENCE_ENABLED_DESCRIPTION);
            MARKET_INFLUENCE_PRICE = SyncedBindingExtensions.BindSyncedEntry(cfg, topSection, LGUConstants.MARKET_INFLUENCE_PRICE_KEY, LGUConstants.MARKET_INFLUENCE_PRICE_DEFAULT);
            MARKET_INFLUENCE_PRICES = SyncedBindingExtensions.BindSyncedEntry(cfg, topSection, BaseUpgrade.PRICES_SECTION, MarketInfluence.PRICES_DEFAULT, BaseUpgrade.PRICES_DESCRIPTION);
            MARKET_INFLUENCE_INITIAL_PERCENTAGE = SyncedBindingExtensions.BindSyncedEntry(cfg, topSection, LGUConstants.MARKET_INFLUENCE_INITIAL_PERCENTAGE_KEY, LGUConstants.MARKET_INFLUENCE_INITIAL_PERCENTAGE_DEFAULT);
            MARKET_INFLUENCE_INCREMENTAL_PERCENTAGE = SyncedBindingExtensions.BindSyncedEntry(cfg, topSection, LGUConstants.MARKET_INFLUENCE_INCREMENTAL_PERCENTAGE_KEY, LGUConstants.MARKET_INFLUENCE_INCREMENTAL_PERCENTAGE_DEFAULT);

            #endregion

            #region Protein Powder

            topSection = ProteinPowder.UPGRADE_NAME;
            PROTEIN_ENABLED = SyncedBindingExtensions.BindSyncedEntry(cfg, topSection, ProteinPowder.ENABLED_SECTION, ProteinPowder.ENABLED_DEFAULT, ProteinPowder.ENABLED_DESCRIPTION);
            PROTEIN_PRICE = SyncedBindingExtensions.BindSyncedEntry(cfg, topSection, ProteinPowder.PRICE_SECTION, ProteinPowder.PRICE_DEFAULT);
            PROTEIN_UNLOCK_FORCE = SyncedBindingExtensions.BindSyncedEntry(cfg, topSection, ProteinPowder.UNLOCK_FORCE_SECTION, ProteinPowder.UNLOCK_FORCE_DEFAULT, ProteinPowder.UNLOCK_FORCE_DESCRIPTION);
            PROTEIN_INCREMENT = SyncedBindingExtensions.BindSyncedEntry(cfg, topSection, ProteinPowder.INCREMENT_FORCE_SECTION, ProteinPowder.INCREMENT_FORCE_DEFAULT, ProteinPowder.INCREMENT_FORCE_DESCRIPTION);
            PROTEIN_INDIVIDUAL = SyncedBindingExtensions.BindSyncedEntry(cfg, topSection, BaseUpgrade.INDIVIDUAL_SECTION, BaseUpgrade.INDIVIDUAL_DEFAULT, BaseUpgrade.INDIVIDUAL_DESCRIPTION);
            PROTEIN_UPGRADE_PRICES = SyncedBindingExtensions.BindSyncedEntry(cfg, topSection, BaseUpgrade.PRICES_SECTION, ProteinPowder.PRICES_DEFAULT, BaseUpgrade.PRICES_DESCRIPTION);
            PROTEIN_CRIT_CHANCE = SyncedBindingExtensions.BindSyncedEntry(cfg, topSection, ProteinPowder.CRIT_CHANCE_SECTION, ProteinPowder.CRIT_CHANCE_DEFAULT, ProteinPowder.CRIT_CHANCE_DESCRIPTION);

            #endregion

            #region Quantum Disruptor

            topSection = QuantumDisruptor.UPGRADE_NAME;
            QUANTUM_DISRUPTOR_ENABLED = SyncedBindingExtensions.BindSyncedEntry(cfg, topSection, LGUConstants.QUANTUM_DISRUPTOR_ENABLED_KEY, LGUConstants.QUANTUM_DISRUPTOR_ENABLED_DEFAULT, LGUConstants.QUANTUM_DISRUPTOR_ENABLED_DESCRIPTION);
            QUANTUM_DISRUPTOR_PRICE = SyncedBindingExtensions.BindSyncedEntry(cfg, topSection, LGUConstants.QUANTUM_DISRUPTOR_PRICE_KEY, LGUConstants.QUANTUM_DISRUPTOR_PRICE_DEFAULT);
            QUANTUM_DISRUPTOR_INITIAL_MULTIPLIER = SyncedBindingExtensions.BindSyncedEntry(cfg, topSection, LGUConstants.QUANTUM_DISRUPTOR_INITIAL_MULTIPLIER_KEY, LGUConstants.QUANTUM_DISRUPTOR_INITIAL_MULTIPLIER_DEFAULT);
            QUANTUM_DISRUPTOR_INCREMENTAL_MULTIPLIER = SyncedBindingExtensions.BindSyncedEntry(cfg, topSection, LGUConstants.QUANTUM_DISRUPTOR_INCREMENTAL_MULTIPLIER_KEY, LGUConstants.QUANTUM_DISRUPTOR_INCREMENTAL_MULTIPLIER_DEFAULT);
            QUANTUM_DISRUPTOR_PRICES = SyncedBindingExtensions.BindSyncedEntry(cfg, topSection, BaseUpgrade.PRICES_SECTION, QuantumDisruptor.PRICES_DEFAULT, BaseUpgrade.PRICES_DESCRIPTION);

            #endregion

            #region Running Shoes

            topSection = RunningShoes.UPGRADE_NAME;
            RUNNING_SHOES_ENABLED = SyncedBindingExtensions.BindSyncedEntry(cfg, topSection, LGUConstants.RUNNING_SHOES_ENABLED_KEY, LGUConstants.RUNNING_SHOES_ENABLED_DEFAULT, LGUConstants.RUNNING_SHOES_ENABLED_DESCRIPTION);
            RUNNING_SHOES_PRICE = SyncedBindingExtensions.BindSyncedEntry(cfg, topSection, LGUConstants.RUNNING_SHOES_PRICE_KEY, LGUConstants.RUNNING_SHOES_PRICE_DEFAULT);
            MOVEMENT_SPEED_UNLOCK = SyncedBindingExtensions.BindSyncedEntry(cfg, topSection, LGUConstants.RUNNING_SHOES_INITIAL_MOVEMENT_BOOST_KEY, LGUConstants.RUNNING_SHOES_INITIAL_MOVEMENT_BOOST_DEFAULT, LGUConstants.RUNNING_SHOES_INITIAL_MOVEMENT_BOOST_DESCRIPTION);
            MOVEMENT_INCREMENT = SyncedBindingExtensions.BindSyncedEntry(cfg, topSection, LGUConstants.RUNNING_SHOES_INCREMENTAL_MOVEMENT_BOOST_KEY, LGUConstants.RUNNING_SHOES_INCREMENTAL_MOVEMENT_BOOST_DEFAULT, LGUConstants.RUNNING_SHOES_INCREMENTAL_MOVEMENT_BOOST_DESCRIPTION);
            RUNNING_SHOES_UPGRADE_PRICES = SyncedBindingExtensions.BindSyncedEntry(cfg, topSection, BaseUpgrade.PRICES_SECTION, BiggerLungs.PRICES_DEFAULT, BaseUpgrade.PRICES_DESCRIPTION);
            RUNNING_SHOES_INDIVIDUAL = SyncedBindingExtensions.BindSyncedEntry(cfg, topSection, BaseUpgrade.INDIVIDUAL_SECTION, BaseUpgrade.INDIVIDUAL_DEFAULT, BaseUpgrade.INDIVIDUAL_DESCRIPTION);
            NOISE_REDUCTION = SyncedBindingExtensions.BindSyncedEntry(cfg, topSection, LGUConstants.RUNNING_SHOES_NOISE_REDUCTION_KEY, LGUConstants.RUNNING_SHOES_NOISE_REDUCTION_DEFAULT, LGUConstants.RUNNING_SHOES_NOISE_REDUCTION_DESCRIPTION);

            #endregion

            #region Shutter Batteries

            topSection = DoorsHydraulicsBattery.UPGRADE_NAME;
            DOOR_HYDRAULICS_BATTERY_ENABLED = SyncedBindingExtensions.BindSyncedEntry(cfg, topSection, DoorsHydraulicsBattery.ENABLED_SECTION, true, DoorsHydraulicsBattery.ENABLED_DESCRIPTION);
            DOOR_HYDRAULICS_BATTERY_PRICE = SyncedBindingExtensions.BindSyncedEntry(cfg, topSection, DoorsHydraulicsBattery.PRICE_SECTION, DoorsHydraulicsBattery.PRICE_DEFAULT, "");
            DOOR_HYDRAULICS_BATTERY_PRICES = SyncedBindingExtensions.BindSyncedEntry(cfg, topSection, BaseUpgrade.PRICES_SECTION, DoorsHydraulicsBattery.PRICES_DEFAULT, BaseUpgrade.PRICES_DESCRIPTION);
            DOOR_HYDRAULICS_BATTERY_INITIAL = SyncedBindingExtensions.BindSyncedEntry(cfg, topSection, DoorsHydraulicsBattery.INITIAL_SECTION, DoorsHydraulicsBattery.INITIAL_DEFAULT, DoorsHydraulicsBattery.INITIAL_DESCRIPTION);
            DOOR_HYDRAULICS_BATTERY_INCREMENTAL = SyncedBindingExtensions.BindSyncedEntry(cfg, topSection, DoorsHydraulicsBattery.INCREMENTAL_SECTION, DoorsHydraulicsBattery.INCREMENTAL_DEFAULT, DoorsHydraulicsBattery.INCREMENTAL_DESCRIPTION);

            #endregion

            #region Sick Beats

            topSection = SickBeats.UPGRADE_NAME;
            BEATS_ENABLED = SyncedBindingExtensions.BindSyncedEntry(cfg, topSection, LGUConstants.SICK_BEATS_ENABLED_KEY, LGUConstants.SICK_BEATS_ENABLED_DEFAULT, LGUConstants.SICK_BEATS_ENABLED_DESCRIPTION);
            BEATS_INDIVIDUAL = SyncedBindingExtensions.BindSyncedEntry(cfg, topSection, BaseUpgrade.INDIVIDUAL_SECTION, BaseUpgrade.INDIVIDUAL_DEFAULT, BaseUpgrade.INDIVIDUAL_DESCRIPTION);
            BEATS_PRICE = SyncedBindingExtensions.BindSyncedEntry(cfg, topSection, LGUConstants.SICK_BEATS_PRICE_KEY, LGUConstants.SICK_BEATS_PRICE_DEFAULT);
            BEATS_SPEED = SyncedBindingExtensions.BindSyncedEntry(cfg, topSection, LGUConstants.SICK_BEATS_SPEED_KEY, LGUConstants.SICK_BEATS_SPEED_DEFAULT);
            BEATS_DMG = SyncedBindingExtensions.BindSyncedEntry(cfg, topSection, LGUConstants.SICK_BEATS_DAMAGE_KEY, LGUConstants.SICK_BEATS_DAMAGE_DEFAULT);
            BEATS_STAMINA = SyncedBindingExtensions.BindSyncedEntry(cfg, topSection, LGUConstants.SICK_BEATS_STAMINA_KEY, LGUConstants.SICK_BEATS_STAMINA_DEFAULT);
            BEATS_DEF = SyncedBindingExtensions.BindSyncedEntry(cfg, topSection, LGUConstants.SICK_BEATS_DEFENSE_KEY, LGUConstants.SICK_BEATS_DEFENSE_DEFAULT);
            BEATS_DEF_CO = SyncedBindingExtensions.BindSyncedEntry(cfg, topSection, LGUConstants.SICK_BEATS_DEFENSE_MULTIPLIER_KEY, LGUConstants.SICK_BEATS_DEFENSE_MULTIPLIER_DEFAULT, LGUConstants.SICK_BEATS_DEFENSE_MULTIPLIER_DESCRIPTION);
            BEATS_STAMINA_CO = SyncedBindingExtensions.BindSyncedEntry(cfg, topSection, LGUConstants.SICK_BEATS_STAMINA_MULTIPLIER_KEY, LGUConstants.SICK_BEATS_STAMINA_MULTIPLIER_DEFAULT, LGUConstants.SICK_BEATS_STAMINA_MULTIPLIER_DESCRIPTION);
            BEATS_DMG_INC = SyncedBindingExtensions.BindSyncedEntry(cfg, topSection, LGUConstants.SICK_BEATS_ADDITIONAL_DAMAGE_KEY, LGUConstants.SICK_BEATS_ADDITIONAL_DAMAGE_DEFAULT);
            BEATS_SPEED_INC = SyncedBindingExtensions.BindSyncedEntry(cfg, topSection, LGUConstants.SICK_BEATS_ADDITIONAL_SPEED_KEY, LGUConstants.SICK_BEATS_ADDITIONAL_SPEED_DEFAULT);
            BEATS_RADIUS = SyncedBindingExtensions.BindSyncedEntry(cfg, topSection, LGUConstants.SICK_BEATS_EFFECT_RADIUS_KEY, LGUConstants.SICK_BEATS_EFFECT_RADIUS_DEFAULT, LGUConstants.SICK_BEATS_EFFECT_RADIUS_DESCRIPTION);

            #endregion

            #region Sigurd Access

            topSection = Sigurd.UPGRADE_NAME;
            SIGURD_ENABLED = SyncedBindingExtensions.BindSyncedEntry(cfg, topSection, LGUConstants.SIGURD_ACCESS_ENABLED_KEY, LGUConstants.SIGURD_ACCESS_ENABLED_DEFAULT, LGUConstants.SIGURD_ACCESS_ENABLED_DESCRIPTION);
            SIGURD_LAST_DAY_ENABLED = SyncedBindingExtensions.BindSyncedEntry(cfg, topSection, LGUConstants.SIGURD_ACCESS_ENABLED_LAST_DAY_KEY, LGUConstants.SIGURD_ACCESS_ENABLED_LAST_DAY_DEFAULT, LGUConstants.SIGURD_ACCESS_ENABLED_LAST_DAY_DESCRIPTION);
            SIGURD_PRICE = SyncedBindingExtensions.BindSyncedEntry(cfg, topSection, LGUConstants.SIGURD_ACCESS_PRICE_KEY, LGUConstants.SIGURD_ACCESS_PRICE_DEFAULT, LGUConstants.SIGURD_ACCESS_PRICE_DESCRIPTION);
            SIGURD_CHANCE = SyncedBindingExtensions.BindSyncedEntry(cfg, topSection, LGUConstants.SIGURD_ACCESS_CHANCE_KEY, LGUConstants.SIGURD_ACCESS_CHANCE_DEFAULT);
            SIGURD_LAST_DAY_CHANCE = SyncedBindingExtensions.BindSyncedEntry(cfg, topSection, LGUConstants.SIGURD_ACCESS_CHANCE_LAST_DAY_KEY, LGUConstants.SIGURD_ACCESS_CHANCE_LAST_DAY_DEFAULT);
            SIGURD_PERCENT = SyncedBindingExtensions.BindSyncedEntry(cfg, topSection, LGUConstants.SIGURD_ACCESS_ADDITIONAL_PERCENT_KEY, LGUConstants.SIGURD_ACCESS_ADDITIONAL_PERCENT_DEFAULT);
            SIGURD_LAST_DAY_PERCENT = SyncedBindingExtensions.BindSyncedEntry(cfg, topSection, LGUConstants.SIGURD_ACCESS_ADDITIONAL_PERCENT_LAST_DAY_KEY, LGUConstants.SIGURD_ACCESS_ADDITIONAL_PERCENT_LAST_DAY_DEFAULT);

            #endregion

            #region Stimpack

            topSection = Stimpack.UPGRADE_NAME;
            PLAYER_HEALTH_ENABLED = SyncedBindingExtensions.BindSyncedEntry(cfg, topSection, Stimpack.ENABLED_SECTION, Stimpack.ENABLED_DEFAULT, Stimpack.ENABLED_DESCRIPTION);
            PLAYER_HEALTH_PRICE = SyncedBindingExtensions.BindSyncedEntry(cfg, topSection, Stimpack.PRICE_SECTION, Stimpack.PRICE_DEFAULT);
            PLAYER_HEALTH_INDIVIDUAL = SyncedBindingExtensions.BindSyncedEntry(cfg, topSection, BaseUpgrade.INDIVIDUAL_SECTION, BaseUpgrade.INDIVIDUAL_DEFAULT, BaseUpgrade.INDIVIDUAL_DESCRIPTION);
            PLAYER_HEALTH_UPGRADE_PRICES = SyncedBindingExtensions.BindSyncedEntry(cfg, topSection, BaseUpgrade.PRICES_SECTION, Stimpack.PRICES_DEFAULT, BaseUpgrade.PRICES_DESCRIPTION);
            PLAYER_HEALTH_ADDITIONAL_HEALTH_UNLOCK = SyncedBindingExtensions.BindSyncedEntry(cfg, topSection, Stimpack.ADDITIONAL_HEALTH_UNLOCK_SECTION, Stimpack.ADDITIONAL_HEALTH_UNLOCK_DEFAULT, Stimpack.ADDITIONAL_HEALTH_UNLOCK_DESCRIPTION);
            PLAYER_HEALTH_ADDITIONAL_HEALTH_INCREMENT = SyncedBindingExtensions.BindSyncedEntry(cfg, topSection, Stimpack.ADDITIONAL_HEALTH_INCREMENT_SECTION, Stimpack.ADDITIONAL_HEALTH_INCREMENT_DEFAULT, Stimpack.ADDITIONAL_HEALTH_INCREMENT_DESCRIPTION);

            #endregion

            #region Strong Legs

            topSection = StrongLegs.UPGRADE_NAME;
            STRONG_LEGS_ENABLED = SyncedBindingExtensions.BindSyncedEntry(cfg, topSection, LGUConstants.STRONG_LEGS_ENABLED_KEY, LGUConstants.STRONG_LEGS_ENABLED_DEFAULT, LGUConstants.STRONG_LEGS_ENABLED_DESCRIPTION);
            STRONG_LEGS_PRICE = SyncedBindingExtensions.BindSyncedEntry(cfg, topSection, LGUConstants.STRONG_LEGS_PRICE_KEY, LGUConstants.STRONG_LEGS_PRICE_DEFAULT);
            JUMP_FORCE_UNLOCK = SyncedBindingExtensions.BindSyncedEntry(cfg, topSection, LGUConstants.STRONG_LEGS_INITIAL_JUMP_FORCE_KEY, LGUConstants.STRONG_LEGS_INITIAL_JUMP_FORCE_DEFAULT, LGUConstants.STRONG_LEGS_INITIAL_JUMP_FORCE_DESCRIPTION);
            JUMP_FORCE_INCREMENT = SyncedBindingExtensions.BindSyncedEntry(cfg, topSection, LGUConstants.STRONG_LEGS_INCREMENTAL_JUMP_FORCE_KEY, LGUConstants.STRONG_LEGS_INCREMENTAL_JUMP_FORCE_DEFAULT, LGUConstants.STRONG_LEGS_INCREMENTAL_JUMP_FORCE_DESCRIPTION);
            STRONG_LEGS_UPGRADE_PRICES = SyncedBindingExtensions.BindSyncedEntry(cfg, topSection, BaseUpgrade.PRICES_SECTION, StrongLegs.PRICES_DEFAULT, BaseUpgrade.PRICES_DESCRIPTION);
            STRONG_LEGS_INDIVIDUAL = SyncedBindingExtensions.BindSyncedEntry(cfg, topSection, BaseUpgrade.INDIVIDUAL_SECTION, BaseUpgrade.INDIVIDUAL_DEFAULT, BaseUpgrade.INDIVIDUAL_DESCRIPTION);
            STRONG_LEGS_REDUCE_FALL_DAMAGE_MULTIPLIER = SyncedBindingExtensions.BindSyncedEntry(cfg, topSection, LGUConstants.STRONG_LEGS_FALL_DAMAGE_MITIGATION_KEY, LGUConstants.STRONG_LEGS_FALL_DAMAGE_MITIGATION_DEFAULT, LGUConstants.STRONG_LEGS_FALL_DAMAGE_MITIGATION_DESCRIPTION);

            #endregion

            #region Walkie GPS

            topSection = WalkieGPS.UPGRADE_NAME;
            WALKIE_ENABLED = SyncedBindingExtensions.BindSyncedEntry(cfg, topSection, LGUConstants.WALKIE_GPS_ENABLED_KEY, LGUConstants.WALKIE_GPS_ENABLED_DEFAULT, LGUConstants.WALKIE_GPS_ENABLED_DESCRIPTION);
            WALKIE_PRICE = SyncedBindingExtensions.BindSyncedEntry(cfg, topSection, LGUConstants.WALKIE_GPS_PRICE_KEY, LGUConstants.WALKIE_GPS_PRICE_DEFAULT, LGUConstants.WALKIE_GPS_PRICE_DESCRIPTION);
            WALKIE_INDIVIDUAL = SyncedBindingExtensions.BindSyncedEntry(cfg, topSection, BaseUpgrade.INDIVIDUAL_SECTION, BaseUpgrade.INDIVIDUAL_DEFAULT, BaseUpgrade.INDIVIDUAL_DESCRIPTION);

            #endregion

            #endregion

            #region Commands

            #region Extend Deadline

            topSection = ExtendDeadlineScript.NAME;
            EXTEND_DEADLINE_ENABLED = SyncedBindingExtensions.BindSyncedEntry(cfg, topSection, LGUConstants.EXTEND_DEADLINE_ENABLED_KEY, LGUConstants.EXTEND_DEADLINE_ENABLED_DEFAULT, LGUConstants.EXTEND_DEADLINE_ENABLED_DESCRIPTION);
            EXTEND_DEADLINE_PRICE = SyncedBindingExtensions.BindSyncedEntry(cfg, topSection, LGUConstants.EXTEND_DEADLINE_PRICE_KEY, LGUConstants.EXTEND_DEADLINE_PRICE_DEFAULT, LGUConstants.EXTEND_DEADLINE_PRICE_DESCRIPTION);

            #endregion

            #region Interns

            topSection = Interns.NAME;
            INTERN_ENABLED = SyncedBindingExtensions.BindSyncedEntry(cfg, topSection, LGUConstants.INTERNS_ENABLED_KEY, LGUConstants.INTERNS_ENABLED_DEFAULT, LGUConstants.INTERNS_ENABLED_DESCRIPTION);
            INTERN_PRICE = SyncedBindingExtensions.BindSyncedEntry(cfg, topSection, LGUConstants.INTERNS_PRICE_KEY, LGUConstants.INTERNS_PRICE_DEFAULT, LGUConstants.INTERNS_PRICE_DESCRIPTION);

            #endregion

            #region Scrap Insurance

            topSection = ScrapInsurance.COMMAND_NAME;
            SCRAP_INSURANCE_ENABLED = SyncedBindingExtensions.BindSyncedEntry(cfg, topSection, LGUConstants.SCRAP_INSURANCE_ENABLED_KEY, LGUConstants.SCRAP_INSURANCE_ENABLED_DEFAULT, LGUConstants.SCRAP_INSURANCE_ENABLED_DESCRIPTION);
            SCRAP_INSURANCE_PRICE = SyncedBindingExtensions.BindSyncedEntry(cfg, topSection, LGUConstants.SCRAP_INSURANCE_PRICE_KEY, ScrapInsurance.DEFAULT_PRICE);

            #endregion

            #region Weather Probe

            topSection = WeatherManager.WEATHER_PROBE_COMMAND;
            WEATHER_PROBE_ENABLED = SyncedBindingExtensions.BindSyncedEntry(cfg, topSection, LGUConstants.WEATHER_PROBE_ENABLED_KEY, LGUConstants.WEATHER_PROBE_ENABLED_DEFAULT, LGUConstants.WEATHER_PROBE_ENABLED_DESCRIPTION);
            WEATHER_PROBE_PRICE = SyncedBindingExtensions.BindSyncedEntry(cfg, topSection, LGUConstants.WEATHER_PROBE_PRICE_KEY, LGUConstants.WEATHER_PROBE_PRICE_DEFAULT, LGUConstants.WEATHER_PROBE_PRICE_DESCRIPTION);
            WEATHER_PROBE_ALWAYS_CLEAR = SyncedBindingExtensions.BindSyncedEntry(cfg, topSection, LGUConstants.WEATHER_PROBE_ALWAYS_CLEAR_KEY, LGUConstants.WEATHER_PROBE_ALWAYS_CLEAR_DEFAULT, LGUConstants.WEATHER_PROBE_ALWAYS_CLEAR_DESCRIPTION);
            WEATHER_PROBE_PICKED_WEATHER_PRICE = SyncedBindingExtensions.BindSyncedEntry(cfg, topSection, LGUConstants.WEATHER_PROBE_PICKED_WEATHER_PRICE_KEY, LGUConstants.WEATHER_PROBE_PICKED_WEATHER_PRICE_DEFAULT, LGUConstants.WEATHER_PROBE_PICKED_WEATHER_PRICE_DESCRIPTION);

            #endregion

            #endregion

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

        #endregion

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
