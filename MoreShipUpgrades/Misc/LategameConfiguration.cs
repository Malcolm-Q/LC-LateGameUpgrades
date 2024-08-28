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
using MoreShipUpgrades.UpgradeComponents.TierUpgrades.Items;
using MoreShipUpgrades.UpgradeComponents.TierUpgrades.Store;
using MoreShipUpgrades.UpgradeComponents.TierUpgrades.Player;
using MoreShipUpgrades.UpgradeComponents.TierUpgrades.Enemies;
using MoreShipUpgrades.UpgradeComponents.TierUpgrades.Ship;
using MoreShipUpgrades.UpgradeComponents.TierUpgrades.Items.WeedKiller;
using MoreShipUpgrades.UpgradeComponents.TierUpgrades.Items.Zapgun;
using MoreShipUpgrades.UpgradeComponents.TierUpgrades.Items.RadarBooster;
using MoreShipUpgrades.UpgradeComponents.OneTimeUpgrades.Ship;
using MoreShipUpgrades.UpgradeComponents.OneTimeUpgrades.Store;
using MoreShipUpgrades.UpgradeComponents.OneTimeUpgrades.Items;
using MoreShipUpgrades.UpgradeComponents.OneTimeUpgrades.Player;
using MoreShipUpgrades.UpgradeComponents.OneTimeUpgrades.Enemies;
using MoreShipUpgrades.UpgradeComponents.TierUpgrades.Items.Shotgun;

namespace MoreShipUpgrades.Misc
{
    [DataContract]
    public class LategameConfiguration : SyncedConfig2<LategameConfiguration>
    {
        #region Enabled
        [field: SyncedEntryField] public SyncedEntry<bool> RUBBER_BOOTS_ENABLED { get; set; }
        [field: SyncedEntryField] public SyncedEntry<bool> OXYGEN_CANISTERS_ENABLED {  get; set; }
        [field: SyncedEntryField] public SyncedEntry<bool> SLEIGHT_OF_HAND_ENABLED {  get; set; }
        [field: SyncedEntryField] public SyncedEntry<bool> HIKING_BOOTS_ENABLED { get; set; }
        [field: SyncedEntryField] public SyncedEntry<bool> TRACTION_BOOTS_ENABLED {  get; set; }
        [field: SyncedEntryField] public SyncedEntry<bool> FEDORA_SUIT_ENABLED {  get; set; }
        [field: SyncedEntryField] public SyncedEntry<bool> WEED_GENETIC_MANIPULATION_ENABLED {  get; set; }
        [field: SyncedEntryField] public SyncedEntry<bool> CLAY_GLASSES_ENABLED {  get; set; }
        [field: SyncedEntryField] public SyncedEntry<bool> MECHANICAL_ARMS_ENABLED {  get; set; }
        [field: SyncedEntryField] public SyncedEntry<bool> SCAVENGER_INSTINCTS_ENABLED {  get; set; }
        [field: SyncedEntryField] public SyncedEntry<bool> LANDING_THRUSTERS_ENABLED {  get; set; }
        [field: SyncedEntryField] public SyncedEntry<bool> REINFORCED_BOOTS_ENABLED {  get; set; }
        [field: SyncedEntryField] public SyncedEntry<bool> DEEPER_POCKETS_ENABLED {  get; set; }
        [field: SyncedEntryField] public SyncedEntry<bool> ALUMINIUM_COILS_ENABLED {  get; set; }
        [field: SyncedEntryField] public SyncedEntry<bool> CHARGING_BOOSTER_ENABLED { get; set; }
        [field: SyncedEntryField] public SyncedEntry<bool> MARKET_INFLUENCE_ENABLED { get; set; }
        [field: SyncedEntryField] public SyncedEntry<bool> BARGAIN_CONNECTIONS_ENABLED { get; set; }
        [field: SyncedEntryField] public SyncedEntry<bool> LETHAL_DEALS_ENABLED { get; set; }
        [field: SyncedEntryField] public SyncedEntry<bool> QUANTUM_DISRUPTOR_ENABLED { get; set; }
        [field: SyncedEntryField] public SyncedEntry<bool> CONTRACTS_ENABLED { get; set; }
        [field: SyncedEntryField] public SyncedEntry<bool> BEEKEEPER_ENABLED { get; set; }
        [field: SyncedEntryField] public SyncedEntry<bool> PROTEIN_ENABLED { get; set; }
        [field: SyncedEntryField] public SyncedEntry<bool> BIGGER_LUNGS_ENABLED { get; set; }
        [field: SyncedEntryField] public SyncedEntry<bool> BACK_MUSCLES_ENABLED { get; set; }
        [field: SyncedEntryField] public SyncedEntry<bool> NIGHT_VISION_ENABLED { get; set; }
        [field: SyncedEntryField] public SyncedEntry<bool> RUNNING_SHOES_ENABLED { get; set; }
        [field: SyncedEntryField] public SyncedEntry<bool> BETTER_SCANNER_ENABLED { get; set; }
        [field: SyncedEntryField] public SyncedEntry<bool> STRONG_LEGS_ENABLED { get; set; }
        [field: SyncedEntryField] public SyncedEntry<bool> DISCOMBOBULATOR_ENABLED { get; set; }
        [field: SyncedEntryField] public SyncedEntry<bool> MALWARE_BROADCASTER_ENABLED { get; set; }
        [field: SyncedEntryField] public SyncedEntry<bool> LIGHTNING_ROD_ENABLED { get; set; }
        [field: SyncedEntryField] public SyncedEntry<bool> HUNTER_ENABLED { get; set; }
        [field: SyncedEntryField] public SyncedEntry<bool> PLAYER_HEALTH_ENABLED { get; set; }
        [field: SyncedEntryField] public SyncedEntry<bool> DOOR_HYDRAULICS_BATTERY_ENABLED { get; set; }
        [field: SyncedEntryField] public SyncedEntry<bool> FASTER_DROP_POD_ENABLED { get; set; }
        [field: SyncedEntryField] public SyncedEntry<bool> SIGURD_ENABLED { get; set; }
        [field: SyncedEntryField] public SyncedEntry<bool> SIGURD_LAST_DAY_ENABLED { get; set; }
        [field: SyncedEntryField] public SyncedEntry<bool> EFFICIENT_ENGINES_ENABLED { get; set; }
        [field: SyncedEntryField] public SyncedEntry<bool> CLIMBING_GLOVES_ENABLED {  get; set; }
        [field: SyncedEntryField] public SyncedEntry<bool> LITHIUM_BATTERIES_ENABLED { get; set; }

        #endregion

        #region Individual
        [field: SyncedEntryField] public SyncedEntry<bool> RUBBER_BOOTS_INDIVIDUAL {  get; set; }
        [field: SyncedEntryField] public SyncedEntry<bool> OXYGEN_CANISTERS_INDIVIDUAL { get; set; }
        [field: SyncedEntryField] public SyncedEntry<bool> SLEIGHT_OF_HAND_INDIVIDUAL {  get; set; }
        [field: SyncedEntryField] public SyncedEntry<bool> HIKING_BOOTS_INDIVIDUAL {  get; set; }
        [field: SyncedEntryField] public SyncedEntry<bool> TRACTION_BOOTS_INDIVIDUAL {  get; set; }
        [field: SyncedEntryField] public SyncedEntry<bool> FEDORA_SUIT_INDIVIDUAL {  get; set; }
        [field: SyncedEntryField] public SyncedEntry<bool> CLAY_GLASSES_INDIVIDUAL {  get; set; }
        [field: SyncedEntryField] public SyncedEntry<bool> MECHANICAL_ARMS_INDIVIDUAL {  get; set; }
        [field: SyncedEntryField] public SyncedEntry<bool> REINFORCED_BOOTS_INDIVIDUAL {  get; set; }
        [field: SyncedEntryField] public SyncedEntry<bool> DEEPER_POCKETS_INDIVIDUAL {  get; set; }
        [field: SyncedEntryField] public SyncedEntry<bool> ALUMINIUM_COILS_INDIVIDUAL { get; set; }
        [field: SyncedEntryField] public SyncedEntry<bool> BEEKEEPER_INDIVIDUAL { get; set; }
        [field: SyncedEntryField] public SyncedEntry<bool> PROTEIN_INDIVIDUAL { get; set; }
        [field: SyncedEntryField] public SyncedEntry<bool> BIGGER_LUNGS_INDIVIDUAL { get; set; }
        [field: SyncedEntryField] public SyncedEntry<bool> BACK_MUSCLES_INDIVIDUAL { get; set; }
        [field: SyncedEntryField] public SyncedEntry<bool> NIGHT_VISION_INDIVIDUAL { get; set; }
        [field: SyncedEntryField] public SyncedEntry<bool> PLAYER_HEALTH_INDIVIDUAL { get; set; }

        [field: SyncedEntryField] public SyncedEntry<bool> RUNNING_SHOES_INDIVIDUAL { get; set; }
        [field: SyncedEntryField] public SyncedEntry<bool> BETTER_SCANNER_INDIVIDUAL { get; set; }
        [field: SyncedEntryField] public SyncedEntry<bool> STRONG_LEGS_INDIVIDUAL { get; set; }
        [field: SyncedEntryField] public SyncedEntry<bool> DISCOMBOBULATOR_INDIVIDUAL { get; set; }
        [field: SyncedEntryField] public SyncedEntry<bool> MALWARE_BROADCASTER_INDIVIDUAL { get; set; }
        [field: SyncedEntryField] public SyncedEntry<bool> LOCKSMITH_INDIVIDUAL { get; set; }
        [field: SyncedEntryField] public SyncedEntry<bool> CLIMBING_GLOVES_INDIVIDUAL { get; set; }
        [field: SyncedEntryField] public SyncedEntry<bool> LITHIUM_BATTERIES_INDIVIDUAL { get; set; }

        #endregion

        #region Initial Prices
        [field: SyncedEntryField] public SyncedEntry<int> RUBBER_BOOTS_PRICE {  get; set; }
        [field: SyncedEntryField] public SyncedEntry<int> OXYGEN_CANISTERS_PRICE { get; set; }
        [field: SyncedEntryField] public SyncedEntry<int> SLEIGHT_OF_HAND_PRICE {  get; set; }
        [field: SyncedEntryField] public SyncedEntry<int> HIKING_BOOTS_PRICE {  get; set; }
        [field: SyncedEntryField] public SyncedEntry<int> TRACTION_BOOTS_PRICE {  get; set; }
        [field: SyncedEntryField] public SyncedEntry<int> FEDORA_SUIT_PRICE {  get; set; }
        [field: SyncedEntryField] public SyncedEntry<int> WEED_GENETIC_MANIPULATION_PRICE {  get; set; }
        [field: SyncedEntryField] public SyncedEntry<int> CLAY_GLASSES_PRICE {  get; set; }
        [field: SyncedEntryField] public SyncedEntry<int> MECHANICAL_ARMS_PRICE {  get; set; }
        [field: SyncedEntryField] public SyncedEntry<int> SCAVENGER_INSTINCTS_PRICE {  get; set; }
        [field: SyncedEntryField] public SyncedEntry<int> LANDING_THRUSTERS_PRICE {  get; set; }
        [field: SyncedEntryField] public SyncedEntry<int> REINFORCED_BOOTS_PRICE {  get; set; }
        [field: SyncedEntryField] public SyncedEntry<int> DEEPER_POCKETS_PRICE {  get; set; }
        [field: SyncedEntryField] public SyncedEntry<int> ALUMINIUM_COILS_PRICE { get; set; }
        [field: SyncedEntryField] public SyncedEntry<int> CLIMBING_GLOVES_PRICE {  get; set; }
        [field: SyncedEntryField] public SyncedEntry<int> CHARGING_BOOSTER_PRICE { get; set; }
        [field: SyncedEntryField] public SyncedEntry<int> MARKET_INFLUENCE_PRICE { get; set; }
        [field: SyncedEntryField] public SyncedEntry<int> BARGAIN_CONNECTIONS_PRICE { get; set; }
        [field: SyncedEntryField] public SyncedEntry<int> LETHAL_DEALS_PRICE { get; set; }
        [field: SyncedEntryField] public SyncedEntry<float> QUANTUM_DISRUPTOR_INITIAL_MULTIPLIER { get; set; }
        [field: SyncedEntryField] public SyncedEntry<float> QUANTUM_DISRUPTOR_INCREMENTAL_MULTIPLIER { get; set; }
        [field: SyncedEntryField] public SyncedEntry<string> QUANTUM_DISRUPTOR_PRICES { get; set; }
        [field: SyncedEntryField] public SyncedEntry<int> QUANTUM_DISRUPTOR_PRICE { get; set; }
        [field: SyncedEntryField] public SyncedEntry<int> HUNTER_PRICE { get; set; }
        [field: SyncedEntryField] public SyncedEntry<int> BEEKEEPER_PRICE { get; set; }
        [field: SyncedEntryField] public SyncedEntry<int> PROTEIN_PRICE { get; set; }
        [field: SyncedEntryField] public SyncedEntry<int> BIGGER_LUNGS_PRICE { get; set; }
        [field: SyncedEntryField] public SyncedEntry<int> BACK_MUSCLES_PRICE { get; set; }
        [field: SyncedEntryField] public SyncedEntry<int> NIGHT_VISION_PRICE { get; set; }
        [field: SyncedEntryField] public SyncedEntry<int> RUNNING_SHOES_PRICE { get; set; }
        [field: SyncedEntryField] public SyncedEntry<int> BETTER_SCANNER_PRICE { get; set; }
        [field: SyncedEntryField] public SyncedEntry<int> STRONG_LEGS_PRICE { get; set; }
        [field: SyncedEntryField] public SyncedEntry<int> DISCOMBOBULATOR_PRICE { get; set; }
        [field: SyncedEntryField] public SyncedEntry<int> MALWARE_BROADCASTER_PRICE { get; set; }
        [field: SyncedEntryField] public SyncedEntry<int> WALKIE_PRICE { get; set; }
        [field: SyncedEntryField] public SyncedEntry<int> LIGHTNING_ROD_PRICE { get; set; }
        [field: SyncedEntryField] public SyncedEntry<int> PLAYER_HEALTH_PRICE { get; set; }
        [field: SyncedEntryField] public SyncedEntry<int> CONTRACT_PRICE { get; set; }
        [field: SyncedEntryField] public SyncedEntry<int> CONTRACT_SPECIFY_PRICE { get; set; }
        [field: SyncedEntryField] public SyncedEntry<int> DOOR_HYDRAULICS_BATTERY_PRICE { get; set; }
        [field: SyncedEntryField] public SyncedEntry<int> FASTER_DROP_POD_PRICE { get; set; }
        [field: SyncedEntryField] public SyncedEntry<int> SIGURD_PRICE { get; set; }
        [field: SyncedEntryField] public SyncedEntry<int> EFFICIENT_ENGINES_PRICE { get; set; }
        [field: SyncedEntryField] public SyncedEntry<int> LITHIUM_BATTERIES_PRICE {  get; set; }

        #endregion

        #region Attributes
        [field: SyncedEntryField] public SyncedEntry<string> RUBBER_BOOTS_PRICES {  get; set; }
        [field: SyncedEntryField] public SyncedEntry<string> RUBBER_BOOTS_OVERRIDE_NAME {  get; set; }
        [field: SyncedEntryField] public SyncedEntry<int> RUBBER_BOOTS_INITIAL_MOVEMENT_HINDERANCE_DECREASE {  get; set; }
        [field: SyncedEntryField] public SyncedEntry<int> RUBBER_BOOTS_INCREMENTAL_MOVEMENT_HINDERANCE_DECREASE { get; set; }
        [field: SyncedEntryField] public SyncedEntry<string> OXYGEN_CANISTERS_PRICES {  get; set; }
        [field: SyncedEntryField] public SyncedEntry<string> OXYGEN_CANISTERS_OVERRIDE_NAME {  get; set; }
        [field: SyncedEntryField] public SyncedEntry<int> OXYGEN_CANISTERS_INITIAL_OXYGEN_CONSUMPTION_DECREASE {  get; set; }
        [field: SyncedEntryField] public SyncedEntry<int> OXYGEN_CANISTERS_INCREMENTAL_OXYGEN_CONSUMPTION_DECREASE { get; set; }
        [field: SyncedEntryField] public SyncedEntry<string> SLEIGHT_OF_HAND_PRICES {  get; set; }
        [field: SyncedEntryField] public SyncedEntry<string> SLEIGHT_OF_HAND_OVERRIDE_NAME {  get; set; }
        [field: SyncedEntryField] public SyncedEntry<int> SLEIGHT_OF_HAND_INITIAL_INCREASE {  get; set; }
        [field: SyncedEntryField] public SyncedEntry<int> SLEIGHT_OF_HAND_INCREMENTAL_INCREASE { get; set; }
        [field: SyncedEntryField] public SyncedEntry<bool> CONTRACT_PROVIDE_RANDOM_ONLY {  get; set; }
        [field: SyncedEntryField] public SyncedEntry<bool> SHOW_WORLD_BUILDING_TEXT {  get; set; }
        [field: SyncedEntryField] public SyncedEntry<string> HIKING_BOOTS_PRICES {  get; set; }
        [field: SyncedEntryField] public SyncedEntry<string> HIKING_BOOTS_OVERRIDE_NAME { get; set; }
        [field: SyncedEntryField] public SyncedEntry<int> HIKING_BOOTS_INITIAL_DECREASE {  get; set; }
        [field: SyncedEntryField] public SyncedEntry<int> HIKING_BOOTS_INCREMENTAL_DECREASE { get; set; }
        [field: SyncedEntryField] public SyncedEntry<string> TRACTION_BOOTS_PRICES {  get; set; }
        [field: SyncedEntryField] public SyncedEntry<string> TRACTION_BOOTS_OVERRIDE_NAME {  get; set; }
        [field: SyncedEntryField] public SyncedEntry<int> TRACTION_BOOTS_INITIAL_INCREASE {  get; set; }
        [field: SyncedEntryField] public SyncedEntry<int> TRACTION_BOOTS_INCREMENTAL_INCREASE { get; set; }
        [field: SyncedEntryField] public SyncedEntry<string> FEDORA_SUIT_OVERRIDE_NAME {  get; set; }
        [field: SyncedEntryField] public SyncedEntry<string> WEED_GENETIC_MANIPULATION_PRICES { get; set; }
        [field: SyncedEntryField] public SyncedEntry<string> WEED_GENETIC_MANIPULATION_OVERRIDE_NAME {  get; set; }
        [field: SyncedEntryField] public SyncedEntry<int> WEED_GENETIC_MANIPULATION_INITIAL_EFFECTIVENESS_INCREASE {  get; set; }
        [field: SyncedEntryField] public SyncedEntry<int> WEED_GENETIC_MANIPULATION_INCREMENTAL_EFFECTIVENESS_INCREASE { get; set; }
        [field: SyncedEntryField] public SyncedEntry<string> CLAY_GLASSES_OVERRIDE_NAME { get; set; }
        [field: SyncedEntryField] public SyncedEntry<string> CLAY_GLASSES_PRICES {  get; set; }
        [field: SyncedEntryField] public SyncedEntry<float> CLAY_GLASSES_DISTANCE_INITIAL_INCREASE {  get; set; }
        [field: SyncedEntryField] public SyncedEntry<float> CLAY_GLASSES_DISTANCE_INCREMENTAL_INCREASE { get; set; }
        [field: SyncedEntryField] public SyncedEntry<LightningRod.UpgradeMode> LIGHTNING_ROD_UPGRADE_MODE { get; set; }
        [field: SyncedEntryField] public SyncedEntry<BackMuscles.UpgradeMode> BACK_MUSCLES_UPGRADE_MODE { get; set; }
        [field: SyncedEntryField] public SyncedEntry<string> MECHANICAL_ARMS_PRICES {  get; set; }
        [field: SyncedEntryField] public SyncedEntry<float> MECHANICAL_ARMS_INITIAL_RANGE_INCREASE {  get; set; }
        [field: SyncedEntryField] public SyncedEntry<float> MECHANICAL_ARMS_INCREMENTAL_RANGE_INCREASE { get; set; }
        [field: SyncedEntryField] public SyncedEntry<string> MECHANICAL_ARMS_OVERRIDE_NAME {  get; set; }
        [field: SyncedEntryField] public SyncedEntry<string> SCAVENGER_INSTINCTS_OVERRIDE_NAME { get; set; }
        [field: SyncedEntryField] public SyncedEntry<string> SCAVENGER_INSTINCTS_PRICES {  get; set; }
        [field: SyncedEntryField] public SyncedEntry<int> SCAVENGER_INSTINCTS_INITIAL_AMOUNT_SCRAP_INCREASE { get; set; }
        [field: SyncedEntryField] public SyncedEntry<int> SCAVENGER_INSTINCTS_INCREMENTAL_AMOUN_SCRAP_INCREASE { get; set; }
        [field: SyncedEntryField] public SyncedEntry<bool> MEDKIT_SCAN_NODE { get; set; }

        [field: SyncedEntryField] public SyncedEntry<QuantumDisruptor.ResetModes> QUANTUM_DISRUPTOR_RESET_MODE {  get; set; }
        [field: SyncedEntryField] public SyncedEntry<QuantumDisruptor.UpgradeModes> QUANTUM_DISRUPTOR_UPGRADE_MODE { get; set; }
        [field: SyncedEntryField] public SyncedEntry<int> QUANTUM_DISRUPTOR_INITIAL_HOURS_REVERT_ON_USE { get; set; }
        [field: SyncedEntryField] public SyncedEntry<int> QUANTUM_DISRUPTOR_INCREMENTAL_HOURS_REVERT_ON_USE { get; set; }
        [field: SyncedEntryField] public SyncedEntry<int> QUANTUM_DISRUPTOR_INITIAL_USES { get; set; }
        [field: SyncedEntryField] public SyncedEntry<int> QUANTUM_DISRUPTOR_INCREMENTAL_USES {  get; set; }
        [field: SyncedEntryField] public SyncedEntry<string> LANDING_THRUSTERS_OVERRIDE_NAME {  get; set; }
        [field: SyncedEntryField] public SyncedEntry<string> LANDING_THRUSTERS_PRICES {  get; set; }
        [field: SyncedEntryField] public SyncedEntry<int> LANDING_THRUSTERS_INITIAL_SPEED_INCREASE {  get; set; }
        [field: SyncedEntryField] public SyncedEntry<int> LANDING_THRUSTERS_INCREMENTAL_SPEED_INCREASE { get; set; }
        [field: SyncedEntryField] public SyncedEntry<bool> LANDING_THRUSTERS_AFFECT_LANDING {  get; set; }
        [field: SyncedEntryField] public SyncedEntry<bool> LANDING_THRUSTERS_AFFECT_DEPARTING { get; set; }
        [field: SyncedEntryField] public SyncedEntry<string> REINFORCED_BOOTS_OVERRIDE_NAME {  get; set; }
        [field: SyncedEntryField] public SyncedEntry<string> REINFORCED_BOOTS_PRICES {  get; set; }
        [field: SyncedEntryField] public SyncedEntry<int> REINFORCED_BOOTS_INITIAL_DAMAGE_REDUCTION {  get; set; }
        [field: SyncedEntryField] public SyncedEntry<int> REINFORCED_BOOTS_INCREMENTAL_DAMAGE_REDUCTION { get; set; }
        [field: SyncedEntryField] public SyncedEntry<string> DEEPER_POCKETS_OVERRIDE_NAME { get; set; }
        [field: SyncedEntryField] public SyncedEntry<string> DEEPER_POCKETS_PRICES { get; set; }
        [field: SyncedEntryField] public SyncedEntry<int> DEEPER_POCKETS_INITIAL_TWO_HANDED_ITEMS {  get; set; }
        [field: SyncedEntryField] public SyncedEntry<int> DEEPER_POCKETS_INCREMENTAL_TWO_HANDED_ITEMS { get; set; }
        [field: SyncedEntryField] public SyncedEntry<bool> DEEPER_POCKETS_ALLOW_WHEELBARROWS { get; set; }
        [field: SyncedEntryField] public SyncedEntry<string> ALUMINIUM_COILS_OVERRIDE_NAME { get; set; }
        [field: SyncedEntryField] public SyncedEntry<string> BACK_MUSCLES_OVERRIDE_NAME { get; set; }
        [field: SyncedEntryField] public SyncedEntry<string> BARGAIN_CONNECTIONS_OVERRIDE_NAME { get; set; }
        [field: SyncedEntryField] public SyncedEntry<string> BEEKEEPER_OVERRIDE_NAME { get; set; }
        [field: SyncedEntryField] public SyncedEntry<string> BETTER_SCANNER_OVERRIDE_NAME { get; set; }
        [field: SyncedEntryField] public SyncedEntry<string> CHARGING_BOOSTER_OVERRIDE_NAME { get; set; }
        [field: SyncedEntryField] public SyncedEntry<string> DISCOMBOBULATOR_OVERRIDE_NAME { get; set; }
        [field: SyncedEntryField] public SyncedEntry<string> EFFICIENT_ENGINES_OVERRIDE_NAME { get; set; }
        [field: SyncedEntryField] public SyncedEntry<string> HUNTER_OVERRIDE_NAME { get; set; }
        [field: SyncedEntryField] public SyncedEntry<string> LITHIUM_BATTERIES_OVERRIDE_NAME { get; set; }
        [field: SyncedEntryField] public SyncedEntry<string> MARKET_INFLUENCE_OVERRIDE_NAME { get; set; }
        [field: SyncedEntryField] public SyncedEntry<string> NIGHT_VISION_OVERRIDE_NAME { get; set; }
        [field: SyncedEntryField] public SyncedEntry<string> PROTEIN_POWDER_OVERRIDE_NAME { get; set; }
        [field: SyncedEntryField] public SyncedEntry<string> BIGGER_LUNGS_OVERRIDE_NAME { get; set; }
        [field: SyncedEntryField] public SyncedEntry<string> CLIMBING_GLOVES_OVERRIDE_NAME { get; set; }
        [field: SyncedEntryField] public SyncedEntry<string> SHUTTER_BATTERIES_OVERRIDE_NAME { get; set; }
        [field: SyncedEntryField] public SyncedEntry<string> QUANTUM_DISRUPTOR_OVERRIDE_NAME { get; set; }
        [field: SyncedEntryField] public SyncedEntry<string> RUNNING_SHOES_OVERRIDE_NAME { get; set; }
        [field: SyncedEntryField] public SyncedEntry<string> STIMPACK_OVERRIDE_NAME { get; set; }
        [field: SyncedEntryField] public SyncedEntry<string> STRONG_LEGS_OVERRIDE_NAME { get; set; }
        [field: SyncedEntryField] public SyncedEntry<string> FAST_ENCRYPTION_OVERRIDE_NAME { get; set; }
        [field: SyncedEntryField] public SyncedEntry<string> DROP_POD_THRUSTERS_OVERRIDE_NAME { get; set; }
        [field: SyncedEntryField] public SyncedEntry<string> LETHAL_DEALS_OVERRIDE_NAME { get; set; }
        [field: SyncedEntryField] public SyncedEntry<string> LIGHTNING_ROD_OVERRIDE_NAME { get; set; }
        [field: SyncedEntryField] public SyncedEntry<string> LOCKSMITH_OVERRIDE_NAME { get; set; }
        [field: SyncedEntryField] public SyncedEntry<string> MALWARE_BROADCASTER_OVERRIDE_NAME { get; set; }
        [field: SyncedEntryField] public SyncedEntry<string> SICK_BEATS_OVERRIDE_NAME { get; set; }
        [field: SyncedEntryField] public SyncedEntry<string> SIGURD_ACCESS_OVERRIDE_NAME { get; set; }
        [field: SyncedEntryField] public SyncedEntry<string> WALKIE_GPS_OVERRIDE_NAME { get; set; }
        [field: SyncedEntryField] public SyncedEntry<bool> OVERRIDE_UPGRADE_NAMES {  get; set; }
        [field: SyncedEntryField] public SyncedEntry<string> ALUMINIUM_COILS_PRICES {  get; set; }
        [field: SyncedEntryField] public SyncedEntry<int> ALUMINIUM_COILS_INITIAL_COOLDOWN_DECREASE {  get; set; }
        [field: SyncedEntryField] public SyncedEntry<int> ALUMINIUM_COILS_INCREMENTAL_COOLDOWN_DECREASE { get; set; }
        [field: SyncedEntryField] public SyncedEntry<int> ALUMINIUM_COILS_INITIAL_DIFFICULTY_DECREASE {  get; set; }
        [field: SyncedEntryField] public SyncedEntry<float> ALUMINIUM_COILS_INITIAL_STUN_TIMER_INCREASE {  get; set; }
        [field: SyncedEntryField] public SyncedEntry<float> ALUMINIUM_COILS_INCREMENTAL_STUN_TIMER_INCREASE { get; set; }
        [field: SyncedEntryField] public SyncedEntry<int> ALUMINIUM_COILS_INCREMENTAL_DIFFICULTY_DECREASE { get; set; }
        [field: SyncedEntryField] public SyncedEntry<float> ALUMINIUM_COILS_INITIAL_RANGE_INCREASE { get; set; }
        [field: SyncedEntryField] public SyncedEntry<float> ALUMINIUM_COILS_INCREMENTAL_RANGE_INCREASE { get; set; }
        [field: SyncedEntryField] public SyncedEntry<string> LITHIUM_BATTERIES_PRICES { get; set; }
        [field: SyncedEntryField] public SyncedEntry<int> LITHIUM_BATTERIES_INITIAL_MULTIPLIER { get; set; }
        [field: SyncedEntryField] public SyncedEntry<int> LITHIUM_BATTERIES_INCREMENTAL_MULTIPLIER {  get; set; }
        [field: SyncedEntryField] public SyncedEntry<string> EFFICIENT_ENGINES_PRICES {  get; set; }
        [field: SyncedEntryField] public SyncedEntry<int> EFFICIENT_ENGINES_INITIAL_DISCOUNT {  get; set; }
        [field: SyncedEntryField] public SyncedEntry<int> EFFICIENT_ENGINES_INCREMENTAL_DISCOUNT { get; set; }
        [field: SyncedEntryField] public SyncedEntry<string> CLIMBING_GLOVES_PRICES {  get; set; }
        [field: SyncedEntryField] public SyncedEntry<float> INITIAL_CLIMBING_SPEED_BOOST {  get; set; }
        [field: SyncedEntryField] public SyncedEntry<float> INCREMENTAL_CLIMBING_SPEED_BOOST { get; set; }
        [field: SyncedEntryField] public SyncedEntry<string> CHARGING_BOOSTER_PRICES { get; set; }
        [field: SyncedEntryField] public SyncedEntry<float> CHARGING_BOOSTER_COOLDOWN { get; set; }
        [field: SyncedEntryField] public SyncedEntry<float> CHARGING_BOOSTER_INCREMENTAL_COOLDOWN_DECREASE { get; set; }
        [field: SyncedEntryField] public SyncedEntry<int> CHARGING_BOOSTER_CHARGE_PERCENTAGE { get; set; }
        [field: SyncedEntryField] public SyncedEntry<float> BIGGER_LUNGS_STAMINA_REGEN_INCREASE { get; set; }
        [field: SyncedEntryField] public SyncedEntry<float> BIGGER_LUNGS_STAMINA_REGEN_INCREMENTAL_INCREASE { get; set; }
        [field: SyncedEntryField] public SyncedEntry<int> BIGGER_LUNGS_STAMINA_REGEN_APPLY_LEVEL { get; set; }
        [field: SyncedEntryField] public SyncedEntry<int> BIGGER_LUNGS_JUMP_STAMINA_APPLY_LEVEL { get; set; }
        [field: SyncedEntryField] public SyncedEntry<float> BIGGER_LUNGS_JUMP_STAMINA_COST_DECREASE { get; set; }
        [field: SyncedEntryField] public SyncedEntry<float> BIGGER_LUNGS_JUMP_STAMINA_COST_INCREMENTAL_DECREASE { get; set; }
        [field: SyncedEntryField] public SyncedEntry<int> PROTEIN_INCREMENT { get; set; }
        [field: SyncedEntryField] public SyncedEntry<float> SPRINT_TIME_INCREASE_UNLOCK { get; set; }
        [field: SyncedEntryField] public SyncedEntry<float> MOVEMENT_SPEED_UNLOCK { get; set; }
        [field: SyncedEntryField] public SyncedEntry<float> JUMP_FORCE_UNLOCK { get; set; }
        [field: SyncedEntryField] public SyncedEntry<bool> DESTROY_TRAP { get; set; }
        [field: SyncedEntryField] public SyncedEntry<float> DISARM_TIME { get; set; }
        [field: SyncedEntryField] public SyncedEntry<bool> EXPLODE_TRAP { get; set; }
        [field: SyncedEntryField] public SyncedEntry<float> CARRY_WEIGHT_REDUCTION { get; set; }
        [field: SyncedEntryField] public SyncedEntry<float> NODE_DISTANCE_INCREASE { get; set; }
        [field: SyncedEntryField] public SyncedEntry<float> SHIP_AND_ENTRANCE_DISTANCE_INCREASE { get; set; }
        [field: SyncedEntryField] public SyncedEntry<float> NOISE_REDUCTION { get; set; }
        [field: SyncedEntryField] public SyncedEntry<float> DISCOMBOBULATOR_COOLDOWN { get; set; }
        [field: SyncedEntryField] public SyncedEntry<float> BEEKEEPER_DAMAGE_MULTIPLIER { get; set; }
        [field: SyncedEntryField] public SyncedEntry<float> BEEKEEPER_DAMAGE_MULTIPLIER_INCREMENT { get; set; }
        [field: SyncedEntryField] public SyncedEntry<float> DISCOMBOBULATOR_RADIUS { get; set; }
        [field: SyncedEntryField] public SyncedEntry<float> DISCOMBOBULATOR_STUN_DURATION { get; set; }
        [field: SyncedEntryField] public SyncedEntry<bool> DISCOMBOBULATOR_NOTIFY_CHAT { get; set; }
        [field: SyncedEntryField] public SyncedEntry<string> NIGHT_VIS_COLOR { get; set; }
        [field: SyncedEntryField] public SyncedEntry<string> NIGHT_VIS_UI_TEXT_COLOR {  get; set; }
        [field: SyncedEntryField] public SyncedEntry<string> NIGHT_VIS_UI_BAR_COLOR { get; set; }
        [field: SyncedEntryField] public SyncedEntry<float> NIGHT_VIS_DRAIN_SPEED { get; set; }
        [field: SyncedEntryField] public SyncedEntry<float> NIGHT_VIS_REGEN_SPEED { get; set; }
        [field: SyncedEntryField] public SyncedEntry<float> NIGHT_BATTERY_MAX { get; set; }
        [field: SyncedEntryField] public SyncedEntry<float> NIGHT_VIS_RANGE { get; set; }
        [field: SyncedEntryField] public SyncedEntry<float> NIGHT_VIS_RANGE_INCREMENT { get; set; }
        [field: SyncedEntryField] public SyncedEntry<float> NIGHT_VIS_INTENSITY { get; set; }
        [field: SyncedEntryField] public SyncedEntry<float> NIGHT_VIS_INTENSITY_INCREMENT { get; set; }
        [field: SyncedEntryField] public SyncedEntry<float> NIGHT_VIS_STARTUP { get; set; }
        [field: SyncedEntryField] public SyncedEntry<float> NIGHT_VIS_EXHAUST { get; set; }
        [field: SyncedEntryField] public SyncedEntry<float> NIGHT_VIS_DRAIN_INCREMENT { get; set; }
        [field: SyncedEntryField] public SyncedEntry<float> NIGHT_VIS_REGEN_INCREMENT { get; set; }
        [field: SyncedEntryField] public SyncedEntry<float> NIGHT_VIS_BATTERY_INCREMENT { get; set; }
        [field: SyncedEntryField] public SyncedEntry<float> CARRY_WEIGHT_INCREMENT { get; set; }
        [field: SyncedEntryField] public SyncedEntry<float> MOVEMENT_INCREMENT { get; set; }
        [field: SyncedEntryField] public SyncedEntry<float> SPRINT_TIME_INCREMENT { get; set; }
        [field: SyncedEntryField] public SyncedEntry<float> JUMP_FORCE_INCREMENT { get; set; }
        [field: SyncedEntryField] public SyncedEntry<float> DISCOMBOBULATOR_INCREMENT { get; set; }
        [field: SyncedEntryField] public SyncedEntry<int> INTERN_PRICE { get; set; }
        [field: SyncedEntryField] public SyncedEntry<int> LOCKSMITH_PRICE { get; set; }
        [field: SyncedEntryField] public SyncedEntry<bool> INTERN_ENABLED { get; set; }
        [field: SyncedEntryField] public SyncedEntry<bool> LOCKSMITH_ENABLED { get; set; }
        [field: SyncedEntryField] public SyncedEntry<float> SALE_PERC { get; set; }
        [field: SyncedEntryField] public SyncedEntry<bool> LOSE_NIGHT_VIS_ON_DEATH { get; set; }
        [field: SyncedEntryField] public SyncedEntry<bool> NIGHT_VISION_DROP_ON_DEATH { get; set; }
        [field: SyncedEntryField] public SyncedEntry<string> BEEKEEPER_UPGRADE_PRICES { get; set; }
        [field: SyncedEntryField] public SyncedEntry<float> BEEKEEPER_HIVE_VALUE_INCREASE { get; set; }
        [field: SyncedEntryField] public SyncedEntry<string> BACK_MUSCLES_UPGRADE_PRICES { get; set; }
        [field: SyncedEntryField] public SyncedEntry<string> BIGGER_LUNGS_UPGRADE_PRICES { get; set; }
        [field: SyncedEntryField] public SyncedEntry<string> NIGHT_VISION_UPGRADE_PRICES { get; set; }
        [field: SyncedEntryField] public SyncedEntry<string> RUNNING_SHOES_UPGRADE_PRICES { get; set; }
        [field: SyncedEntryField] public SyncedEntry<string> STRONG_LEGS_UPGRADE_PRICES { get; set; }
        [field: SyncedEntryField] public SyncedEntry<string> DISCO_UPGRADE_PRICES { get; set; }
        [field: SyncedEntryField] public SyncedEntry<string> PROTEIN_UPGRADE_PRICES { get; set; }
        [field: SyncedEntryField] public SyncedEntry<string> PLAYER_HEALTH_UPGRADE_PRICES { get; set; }
        [field: SyncedEntryField] public SyncedEntry<string> HUNTER_UPGRADE_PRICES { get; set; }
        [field: SyncedEntryField] public SyncedEntry<string> HUNTER_SAMPLE_TIERS { get; set; }
        [field: SyncedEntryField] public SyncedEntry<bool> SHARED_UPGRADES { get; set; }
        [field: SyncedEntryField] public SyncedEntry<bool> WALKIE_ENABLED { get; set; }
        [field: SyncedEntryField] public SyncedEntry<bool> WALKIE_INDIVIDUAL { get; set; }
        [field: SyncedEntryField] public SyncedEntry<int> PROTEIN_UNLOCK_FORCE { get; set; }
        [field: SyncedEntryField] public SyncedEntry<float> PROTEIN_CRIT_CHANCE { get; set; }
        [field: SyncedEntryField] public SyncedEntry<int> BETTER_SCANNER_PRICE2 { get; set; }
        [field: SyncedEntryField] public SyncedEntry<int> BETTER_SCANNER_PRICE3 { get; set; }
        [field: SyncedEntryField] public SyncedEntry<bool> BETTER_SCANNER_ENEMIES { get; set; }
        [field: SyncedEntryField] public SyncedEntry<bool> LIGHTNING_ROD_ACTIVE { get; set; }
        [field: SyncedEntryField] public SyncedEntry<float> LIGHTNING_ROD_DIST { get; set; }
        [field: SyncedEntryField] public SyncedEntry<bool> PAGER_ENABLED { get; set; }
        [field: SyncedEntryField] public SyncedEntry<int> PAGER_PRICE { get; set; }
        [field: SyncedEntryField] public SyncedEntry<bool> VERBOSE_ENEMIES { get; set; }
        [field: SyncedEntryField] public SyncedEntry<int> PLAYER_HEALTH_ADDITIONAL_HEALTH_UNLOCK { get; set; }
        [field: SyncedEntryField] public SyncedEntry<int> PLAYER_HEALTH_ADDITIONAL_HEALTH_INCREMENT { get; set; }
        [field: SyncedEntryField] public SyncedEntry<bool> MEDKIT_ENABLED { get; set; }
        [field: SyncedEntryField] public SyncedEntry<int> MEDKIT_PRICE { get; set; }
        [field: SyncedEntryField] public SyncedEntry<int> MEDKIT_HEAL_VALUE { get; set; }
        [field: SyncedEntryField] public SyncedEntry<int> MEDKIT_USES { get; set; }
        [field: SyncedEntryField] public SyncedEntry<int> DISCOMBOBULATOR_DAMAGE_LEVEL { get; set; }
        [field: SyncedEntryField] public SyncedEntry<int> DISCOMBOBULATOR_INITIAL_DAMAGE { get; set; }
        [field: SyncedEntryField] public SyncedEntry<int> DISCOMBOBULATOR_DAMAGE_INCREASE { get; set; }
        [field: SyncedEntryField] public SyncedEntry<bool> KEEP_UPGRADES_AFTER_FIRED_CUTSCENE { get; set; }
        [field: SyncedEntryField] public SyncedEntry<int> CONTRACT_BUG_REWARD { get; set; }
        [field: SyncedEntryField] public SyncedEntry<int> CONTRACT_EXOR_REWARD { get; set; }
        [field: SyncedEntryField] public SyncedEntry<int> CONTRACT_DEFUSE_REWARD { get; set; }
        [field: SyncedEntryField] public SyncedEntry<int> CONTRACT_BUG_SPAWNS { get; set; }
        [field: SyncedEntryField] public SyncedEntry<int> CONTRACT_EXTRACT_REWARD { get; set; }
        [field: SyncedEntryField] public SyncedEntry<float> CONTRACT_EXTRACT_WEIGHT { get; set; }
        [field: SyncedEntryField] public SyncedEntry<int> CONTRACT_DATA_REWARD { get; set; }
        [field: SyncedEntryField] public SyncedEntry<bool> BEATS_INDIVIDUAL { get; set; }
        [field: SyncedEntryField] public SyncedEntry<bool> BEATS_ENABLED { get; set; }
        [field: SyncedEntryField] public SyncedEntry<int> BEATS_PRICE { get; set; }
        [field: SyncedEntryField] public SyncedEntry<bool> BEATS_SPEED { get; set; }
        [field: SyncedEntryField] public SyncedEntry<bool> BEATS_DMG { get; set; }
        [field: SyncedEntryField] public SyncedEntry<bool> BEATS_STAMINA { get; set; }
        [field: SyncedEntryField] public SyncedEntry<float> BEATS_STAMINA_CO { get; set; }
        [field: SyncedEntryField] public SyncedEntry<bool> BEATS_DEF { get; set; }
        [field: SyncedEntryField] public SyncedEntry<float> BEATS_DEF_CO { get; set; }
        [field: SyncedEntryField] public SyncedEntry<float> BEATS_SPEED_INC { get; set; }
        [field: SyncedEntryField] public SyncedEntry<float> BEATS_RADIUS { get; set; }
        [field: SyncedEntryField] public SyncedEntry<int> BEATS_DMG_INC { get; set; }
        [field: SyncedEntryField] public SyncedEntry<int> SNARE_FLEA_SAMPLE_MINIMUM_VALUE { get; set; }
        [field: SyncedEntryField] public SyncedEntry<int> SNARE_FLEA_SAMPLE_MAXIMUM_VALUE { get; set; }
        [field: SyncedEntryField] public SyncedEntry<int> BUNKER_SPIDER_SAMPLE_MINIMUM_VALUE { get; set; }
        [field: SyncedEntryField] public SyncedEntry<int> BUNKER_SPIDER_SAMPLE_MAXIMUM_VALUE { get; set; }
        [field: SyncedEntryField] public SyncedEntry<int> HOARDING_BUG_SAMPLE_MINIMUM_VALUE { get; set; }
        [field: SyncedEntryField] public SyncedEntry<int> HOARDING_BUG_SAMPLE_MAXIMUM_VALUE { get; set; }
        [field: SyncedEntryField] public SyncedEntry<int> BRACKEN_SAMPLE_MINIMUM_VALUE { get; set; }
        [field: SyncedEntryField] public SyncedEntry<int> BRACKEN_SAMPLE_MAXIMUM_VALUE { get; set; }
        [field: SyncedEntryField] public SyncedEntry<int> EYELESS_DOG_SAMPLE_MINIMUM_VALUE { get; set; }
        [field: SyncedEntryField] public SyncedEntry<int> EYELESS_DOG_SAMPLE_MAXIMUM_VALUE { get; set; }
        [field: SyncedEntryField] public SyncedEntry<int> BABOON_HAWK_SAMPLE_MINIMUM_VALUE { get; set; }
        [field: SyncedEntryField] public SyncedEntry<int> BABOON_HAWK_SAMPLE_MAXIMUM_VALUE { get; set; }
        [field: SyncedEntryField] public SyncedEntry<int> THUMPER_SAMPLE_MINIMUM_VALUE { get; set; }
        [field: SyncedEntryField] public SyncedEntry<int> THUMPER_SAMPLE_MAXIMUM_VALUE { get; set; }
        [field: SyncedEntryField] public SyncedEntry<int> FOREST_KEEPER_SAMPLE_MINIMUM_VALUE { get; set; }
        [field: SyncedEntryField] public SyncedEntry<int> FOREST_KEEPER_SAMPLE_MAXIMUM_VALUE { get; set; }
        [field: SyncedEntryField] public SyncedEntry<int> MANTICOIL_SAMPLE_MINIMUM_VALUE { get; set; }
        [field: SyncedEntryField] public SyncedEntry<int> MANTICOIL_SAMPLE_MAXIMUM_VALUE { get; set; }
        [field: SyncedEntryField] public SyncedEntry<int> TULIP_SNAKE_SAMPLE_MINIMUM_VALUE { get; set; }
        [field: SyncedEntryField] public SyncedEntry<int> TULIP_SNAKE_SAMPLE_MAXIMUM_VALUE { get; set; }
        [field: SyncedEntryField] public SyncedEntry<int> KIDNAPPER_FOX_SAMPLE_MINIMUM_VALUE { get; set; }
        [field: SyncedEntryField] public SyncedEntry<int> KIDNAPPER_FOX_SAMPLE_MAXIMUM_VALUE { get; set; }
        [field: SyncedEntryField] public SyncedEntry<int> SPORE_LIZARD_SAMPLE_MINIMUM_VALUE { get; set; }
        [field: SyncedEntryField] public SyncedEntry<int> SPORE_LIZARD_SAMPLE_MAXIMUM_VALUE { get; set; }
        [field: SyncedEntryField] public SyncedEntry<int> MANEATER_SAMPLE_MINIMUM_VALUE { get; set; }
        [field: SyncedEntryField] public SyncedEntry<int> MANEATER_SAMPLE_MAXIMUM_VALUE { get; set; }
        [field: SyncedEntryField] public SyncedEntry<int> CONTRACT_GHOST_SPAWN { get; set; }
        [field: SyncedEntryField] public SyncedEntry<float> SCAV_VOLUME { get; set; }
        [field: SyncedEntryField] public SyncedEntry<bool> CONTRACT_FREE_MOONS_ONLY { get; set; }
        [field: SyncedEntryField] public SyncedEntry<string> DOOR_HYDRAULICS_BATTERY_PRICES { get; set; }
        [field: SyncedEntryField] public SyncedEntry<float> DOOR_HYDRAULICS_BATTERY_INITIAL { get; set; }
        [field: SyncedEntryField] public SyncedEntry<float> DOOR_HYDRAULICS_BATTERY_INCREMENTAL { get; set; }
        [field: SyncedEntryField] public SyncedEntry<bool> DATA_CONTRACT { get; set; }
        [field: SyncedEntryField] public SyncedEntry<bool> EXTERMINATOR_CONTRACT { get; set; }
        [field: SyncedEntryField] public SyncedEntry<bool> EXORCISM_CONTRACT { get; set; }
        [field: SyncedEntryField] public SyncedEntry<bool> EXTRACTION_CONTRACT { get; set; }
        [field: SyncedEntryField] public SyncedEntry<bool> DEFUSAL_CONTRACT { get; set; }
        [field: SyncedEntryField] public SyncedEntry<bool> MAIN_OBJECT_FURTHEST { get; set; }
        [field: SyncedEntryField] public SyncedEntry<string> MARKET_INFLUENCE_PRICES { get; set; }
        [field: SyncedEntryField] public SyncedEntry<int> MARKET_INFLUENCE_INITIAL_PERCENTAGE { get; set; }
        [field: SyncedEntryField] public SyncedEntry<int> MARKET_INFLUENCE_INCREMENTAL_PERCENTAGE { get; set; }
        [field: SyncedEntryField] public SyncedEntry<int> BARGAIN_CONNECTIONS_INITIAL_ITEM_AMOUNT { get; set; }
        [field: SyncedEntryField] public SyncedEntry<int> BARGAIN_CONNECTIONS_INCREMENTAL_ITEM_AMOUNT { get; set; }
        [field: SyncedEntryField] public SyncedEntry<string> BARGAIN_CONNECTIONS_PRICES { get; set; }
        [field: SyncedEntryField] public SyncedEntry<float> FASTER_DROP_POD_TIMER { get; set; }
        [field: SyncedEntryField] public SyncedEntry<float> FASTER_DROP_POD_INITIAL_TIMER { get; set; }
        [field: SyncedEntryField] public SyncedEntry<float> FASTER_DROP_POD_LEAVE_TIMER {  get; set; }
        [field: SyncedEntryField] public SyncedEntry<int> EXTRACTION_CONTRACT_AMOUNT_MEDKITS { get; set; }
        [field: SyncedEntryField] public SyncedEntry<int> CONTRACT_REWARD_QUOTA_MULTIPLIER { get; set; }
        [field: SyncedEntryField] public SyncedEntry<bool> SHOW_UPGRADES_CHAT { get; set; }
        [field: SyncedEntryField] public SyncedEntry<float> SIGURD_CHANCE { get; set; }
        [field: SyncedEntryField] public SyncedEntry<float> SIGURD_LAST_DAY_CHANCE { get; set; }
        [field: SyncedEntryField] public SyncedEntry<float> SIGURD_PERCENT { get; set; }
        [field: SyncedEntryField] public SyncedEntry<float> SIGURD_LAST_DAY_PERCENT { get; set; }
        [field: SyncedEntryField] public SyncedEntry<bool> SALE_APPLY_ONCE { get; set; }

        #endregion

        #region Item Progression
        [field: SyncedEntryField] public SyncedEntry<bool> ALTERNATIVE_ITEM_PROGRESSION { get; set; }
        [field: SyncedEntryField] public SyncedEntry<ItemProgressionManager.CollectionModes> ITEM_PROGRESSION_MODE { get; set; }
        [field: SyncedEntryField] public SyncedEntry<float> ITEM_PROGRESSION_CONTRIBUTION_MULTIPLIER { get; set; }
        [field: SyncedEntryField] public SyncedEntry<float> SCRAP_UPGRADE_CHANCE { get; set; }
        [field: SyncedEntryField] public SyncedEntry<ItemProgressionManager.ChancePerScrapModes> SCRAP_UPGRADE_CHANCE_MODE { get; set; }
        [field: SyncedEntryField] public SyncedEntry<string> ITEM_PROGRESSION_BLACKLISTED_ITEMS { get; set; }
        [field: SyncedEntryField] public SyncedEntry<string> ITEM_PROGRESSION_APPARATICE_ITEMS {  get; set; }
        [field: SyncedEntryField] public SyncedEntry<string> RUBBER_BOOTS_ITEM_PROGRESSION_ITEMS {  get; set; }
        [field: SyncedEntryField] public SyncedEntry<string> OXYGEN_CANISTERS_ITEM_PROGRESSION_ITEMS {  get; set; }
        [field: SyncedEntryField] public SyncedEntry<string> SLEIGHT_OF_HAND_ITEM_PROGRESSION_ITEMS {  get; set; }
        [field: SyncedEntryField] public SyncedEntry<string> HIKING_BOOTS_ITEM_PROGRESSION_ITEMS {  get; set; }
        [field: SyncedEntryField] public SyncedEntry<string> TRACTION_BOOTS_ITEM_PROGRESSION_ITEMS {  get; set; }
        [field: SyncedEntryField] public SyncedEntry<string> FEDORA_SUIT_ITEM_PROGRESSION_ITEMS { get; set; }
        [field: SyncedEntryField] public SyncedEntry<string> WEED_GENETIC_MANIPULATION_ITEM_PROGRESSION_ITEMS {  get; set; }
        [field: SyncedEntryField] public SyncedEntry<string> CLAY_GLASSES_ITEM_PROGRESSION_ITEMS {  get; set; }
        [field: SyncedEntryField] public SyncedEntry<string> ALUMINIUM_COILS_ITEM_PROGRESSION_ITEMS {  get; set; }
        [field: SyncedEntryField] public SyncedEntry<string> BACK_MUSCLES_ITEM_PROGRESSION_ITEMS { get; set; }
        [field: SyncedEntryField] public SyncedEntry<string> BARGAIN_CONNECTIONS_ITEM_PROGRESSION_ITEMS { get; set; }
        [field: SyncedEntryField] public SyncedEntry<string> BEEKEEPER_ITEM_PROGRESSION_ITEMS { get; set; }
        [field: SyncedEntryField] public SyncedEntry<string> BETTER_SCANNER_ITEM_PROGRESSION_ITEMS { get; set; }
        [field: SyncedEntryField] public SyncedEntry<string> CHARGING_BOOSTER_ITEM_PROGRESSION_ITEMS { get; set; }
        [field: SyncedEntryField] public SyncedEntry<string> DEEPER_POCKETS_ITEM_PROGRESSION_ITEMS { get; set; }
        [field: SyncedEntryField] public SyncedEntry<string> DISCOMBOBULATOR_ITEM_PROGRESSION_ITEMS { get; set; }
        [field: SyncedEntryField] public SyncedEntry<string> EFFICIENT_ENGINES_ITEM_PROGRESSION_ITEMS { get; set; }
        [field: SyncedEntryField] public SyncedEntry<string> HUNTER_ITEM_PROGRESSION_ITEMS { get; set; }
        [field: SyncedEntryField] public SyncedEntry<string> LANDING_THRUSTERS_ITEM_PROGRESSION_ITEMS { get; set; }
        [field: SyncedEntryField] public SyncedEntry<string> LITHIUM_BATTERIES_ITEM_PROGRESSION_ITEMS { get; set; }
        [field: SyncedEntryField] public SyncedEntry<string> MARKET_INFLUENCE_ITEM_PROGRESSION_ITEMS { get; set; }
        [field: SyncedEntryField] public SyncedEntry<string> MECHANICAL_ARMS_ITEM_PROGRESSION_ITEMS { get; set; }
        [field: SyncedEntryField] public SyncedEntry<string> NIGHT_VISION_ITEM_PROGRESSION_ITEMS { get; set; }
        [field: SyncedEntryField] public SyncedEntry<string> PROTEIN_POWDER_ITEM_PROGRESSION_ITEMS { get; set; }
        [field: SyncedEntryField] public SyncedEntry<string> REINFORCED_BOOTS_ITEM_PROGRESSION_ITEMS { get; set; }
        [field: SyncedEntryField] public SyncedEntry<string> SCAVENGER_INSTINCTS_ITEM_PROGRESSION_ITEMS { get; set; }
        [field: SyncedEntryField] public SyncedEntry<string> BIGGER_LUNGS_ITEM_PROGRESSION_ITEMS { get; set; }
        [field: SyncedEntryField] public SyncedEntry<string> CLIMBING_GLOVES_ITEM_PROGRESSION_ITEMS { get; set; }
        [field: SyncedEntryField] public SyncedEntry<string> SHUTTER_BATTERIES_ITEM_PROGRESSION_ITEMS { get; set; }
        [field: SyncedEntryField] public SyncedEntry<string> QUANTUM_DISRUPTOR_ITEM_PROGRESSION_ITEMS { get; set; }
        [field: SyncedEntryField] public SyncedEntry<string> RUNNING_SHOES_ITEM_PROGRESSION_ITEMS { get; set; }
        [field: SyncedEntryField] public SyncedEntry<string> STIMPACK_ITEM_PROGRESSION_ITEMS { get; set; }
        [field: SyncedEntryField] public SyncedEntry<string> STRONG_LEGS_ITEM_PROGRESSION_ITEMS { get; set; }
        [field: SyncedEntryField] public SyncedEntry<string> FAST_ENCRYPTION_ITEM_PROGRESSION_ITEMS { get; set; }
        [field: SyncedEntryField] public SyncedEntry<string> DROP_POD_THRUSTERS_ITEM_PROGRESSION_ITEMS { get; set; }
        [field: SyncedEntryField] public SyncedEntry<string> LETHAL_DEALS_ITEM_PROGRESSION_ITEMS { get; set; }
        [field: SyncedEntryField] public SyncedEntry<string> LIGHTNING_ROD_ITEM_PROGRESSION_ITEMS { get; set; }
        [field: SyncedEntryField] public SyncedEntry<string> LOCKSMITH_ITEM_PROGRESSION_ITEMS { get; set; }
        [field: SyncedEntryField] public SyncedEntry<string> MALWARE_BROADCASTER_ITEM_PROGRESSION_ITEMS { get; set; }
        [field: SyncedEntryField] public SyncedEntry<string> SICK_BEATS_ITEM_PROGRESSION_ITEMS { get; set; }
        [field: SyncedEntryField] public SyncedEntry<string> SIGURD_ACCESS_ITEM_PROGRESSION_ITEMS { get; set; }
        [field: SyncedEntryField] public SyncedEntry<string> WALKIE_GPS_ITEM_PROGRESSION_ITEMS { get; set; }

        #endregion

        #region Randomize Upgrades
        [field: SyncedEntryField] public SyncedEntry<bool> RANDOMIZE_UPGRADES_ENABLED { get; set; }
        [field: SyncedEntryField] public SyncedEntry<int> RANDOMIZE_UPGRADES_AMOUNT { get; set; }
        [field: SyncedEntryField] public SyncedEntry<bool> RANDOMIZE_UPGRADES_ALWAYS_SHOW_PURCHASED {  get; set; }
        [field: SyncedEntryField] public SyncedEntry<RandomizeUpgradeManager.RandomizeUpgradeEvents> RANDOMIZE_UPGRADES_CHANGE_UPGRADES_EVENT {  get; set; }
        #endregion

        #region Configuration Bindings
        public LategameConfiguration(ConfigFile cfg) : base(Metadata.GUID)
        {
            string topSection;

            #region Randomize Upgrades

            topSection = LguConstants.RANDOMIZE_UPGRADES_SECTION;
            RANDOMIZE_UPGRADES_ENABLED = cfg.BindSyncedEntry(topSection, LguConstants.RANDOMIZE_UPGRADES_ENABLED_KEY, LguConstants.RANDOMIZE_UPGRADES_ENABLED_DEFAULT, LguConstants.RANDOMIZE_UPGRADES_ENABLED_DESCRIPTION);
            RANDOMIZE_UPGRADES_AMOUNT = cfg.BindSyncedEntry(topSection, LguConstants.RANDOMIZE_UPGRADES_AMOUNT_KEY, LguConstants.RANDOMIZE_UPGRADES_AMOUNT_DEFAULT, LguConstants.RANDOMIZE_UPGRADES_AMOUNT_DESCRIPTION);
            RANDOMIZE_UPGRADES_ALWAYS_SHOW_PURCHASED = cfg.BindSyncedEntry(topSection, LguConstants.RANDOMIZE_UPGRADES_ALWAYS_SHOW_PURCHASED_KEY, LguConstants.RANDOMIZE_UPGRADES_ALWAYS_SHOW_PURCHASED_DEFAULT, LguConstants.RANDOMIZE_UPGRADES_ALWAYS_SHOW_PURCHASED_DESCRIPTION);
            RANDOMIZE_UPGRADES_CHANGE_UPGRADES_EVENT = cfg.BindSyncedEntry(topSection, LguConstants.RANDOMIZE_UPGRADES_CHANGE_UPGRADES_EVENT_KEY, LguConstants.RANDOMIZE_UPGRADES_CHANGE_UPGRADES_EVENT_DEFAULT, LguConstants.RANDOMIZE_UPGRADES_CHANGE_UPGRADES_EVENT_DESCRIPTION);

            #endregion

            #region Item Progression

            topSection = LguConstants.ITEM_PROGRESSION_SECTION;
            ALTERNATIVE_ITEM_PROGRESSION = cfg.BindSyncedEntry(topSection, LguConstants.ALTERNATIVE_ITEM_PROGRESSION_KEY, LguConstants.ALTERNATIVE_ITEM_PROGRESSION_DEFAULT, LguConstants.ALTERNATIVE_ITEM_PROGRESSION_DESCRIPTION);
            ITEM_PROGRESSION_MODE = cfg.BindSyncedEntry(topSection, LguConstants.ITEM_PROGRESSION_MODE_KEY, LguConstants.ITEM_PROGRESSION_MODE_DEFAULT, LguConstants.ITEM_PROGRESSION_MODE_DESCRIPTION);
            ITEM_PROGRESSION_CONTRIBUTION_MULTIPLIER = cfg.BindSyncedEntry(topSection, LguConstants.ITEM_PROGRESSION_CONTRIBUTION_MULTIPLIER_KEY, LguConstants.ITEM_PROGRESSION_CONTRIBUTION_MULTIPLIER_DEFAULT, LguConstants.ITEM_PROGRESSION_CONTRIBUTION_MULTIPLIER_DESCRIPTION);
            SCRAP_UPGRADE_CHANCE = cfg.BindSyncedEntry(topSection, LguConstants.SCRAP_UPGRADE_CHANCE_KEY, LguConstants.SCRAP_UPGRADE_CHANCE_DEFAULT, LguConstants.SCRAP_UPGRADE_CHANCE_DESCRIPTION);
            SCRAP_UPGRADE_CHANCE_MODE = cfg.BindSyncedEntry(topSection, LguConstants.SCRAP_UPGRADE_MODE_KEY, LguConstants.SCRAP_UPGRADE_MODE_DEFAULT, LguConstants.SCRAP_UPGRADE_MODE_DESCRIPTION);
            ITEM_PROGRESSION_BLACKLISTED_ITEMS = cfg.BindSyncedEntry(topSection, LguConstants.ITEM_PROGRESSION_BLACKLISTED_ITEMS_KEY, LguConstants.ITEM_PROGRESSION_BLACKLISTED_ITEMS_DEFAULT, LguConstants.ITEM_PROGRESSION_BLACKLISTED_ITEMS_DESCRIPTION);
            ITEM_PROGRESSION_APPARATICE_ITEMS = cfg.BindSyncedEntry(topSection, LguConstants.ITEM_PROGRESSION_APPARATICE_ITEMS_KEY, LguConstants.ITEM_PROGRESSION_APPARATICE_ITEMS_DEFAULT, LguConstants.ITEM_PROGRESSION_APPARATICE_ITEMS_DESCRIPTION);

            #endregion 

            #region Miscellaneous

            topSection = LguConstants.MISCELLANEOUS_SECTION;
            SHARED_UPGRADES = cfg.BindSyncedEntry(topSection, LguConstants.SHARE_ALL_UPGRADES_KEY, LguConstants.SHARE_ALL_UPGRADES_DEFAULT, LguConstants.SHARE_ALL_UPGRADES_DESCRIPTION);
            SALE_PERC = cfg.BindSyncedEntry(topSection, LguConstants.SALE_PERCENT_KEY, LguConstants.SALE_PERCENT_DEFAULT, LguConstants.SALE_PERCENT_DESCRIPTION);
            KEEP_UPGRADES_AFTER_FIRED_CUTSCENE = cfg.BindSyncedEntry(topSection, LguConstants.KEEP_UPGRADES_AFTER_FIRED_KEY, LguConstants.KEEP_UPGRADES_AFTER_FIRED_DEFAULT, LguConstants.KEEP_UPGRADES_AFTER_FIRED_DESCRIPTION);
            SHOW_UPGRADES_CHAT = cfg.BindSyncedEntry(topSection, LguConstants.SHOW_UPGRADES_CHAT_KEY, LguConstants.SHOW_UPGRADES_CHAT_DEFAULT, LguConstants.SHOW_UPGRADES_CHAT_DESCRIPTION);
            SALE_APPLY_ONCE = cfg.BindSyncedEntry(topSection, LguConstants.SALE_APPLY_ONCE_KEY, LguConstants.SALE_APPLY_ONCE_DEFAULT, LguConstants.SALE_APPLY_ONCE_DESCRIPTION);
            SHOW_WORLD_BUILDING_TEXT = cfg.BindSyncedEntry(topSection, LguConstants.SHOW_WORLD_BUILDING_TEXT_KEY, LguConstants.SHOW_WORLD_BUILDING_TEXT_DEFAULT, LguConstants.SHOW_WORLD_BUILDING_TEXT_DESCRIPTION);

            #endregion

            #region Override Names

            topSection = LguConstants.OVERRIDE_NAMES_SECTION;
            OVERRIDE_UPGRADE_NAMES              = cfg.BindSyncedEntry(topSection, LguConstants.OVERRIDE_NAMES_ENABLED_KEY, LguConstants.OVERRIDE_NAMES_ENABLED_DEFAULT, LguConstants.OVERRIDE_NAMES_ENABLED_DESCRIPTION);
            RUBBER_BOOTS_OVERRIDE_NAME          = cfg.BindSyncedEntry(topSection, LguConstants.RUBBER_BOOTS_OVERRIDE_NAME_KEY, RubberBoots.UPGRADE_NAME);
            OXYGEN_CANISTERS_OVERRIDE_NAME      = cfg.BindSyncedEntry(topSection, LguConstants.OXYGEN_CANISTERS_OVERRIDE_NAME_KEY, OxygenCanisters.UPGRADE_NAME);
            SLEIGHT_OF_HAND_OVERRIDE_NAME       = cfg.BindSyncedEntry(topSection, LguConstants.SLEIGHT_OF_HAND_OVERRIDE_NAME_KEY, SleightOfHand.UPGRADE_NAME);
            HIKING_BOOTS_OVERRIDE_NAME          = cfg.BindSyncedEntry(topSection, LguConstants.HIKING_BOOTS_OVERRIDE_NAME_KEY, HikingBoots.UPGRADE_NAME);
            TRACTION_BOOTS_OVERRIDE_NAME        = cfg.BindSyncedEntry(topSection, LguConstants.TRACTION_BOOTS_OVERRIDE_NAME_KEY, TractionBoots.UPGRADE_NAME);
            FEDORA_SUIT_OVERRIDE_NAME           = cfg.BindSyncedEntry(topSection, LguConstants.FEDORA_SUIT_OVERRIDE_NAME_KEY, FedoraSuit.UPGRADE_NAME);
            WEED_GENETIC_MANIPULATION_OVERRIDE_NAME = cfg.BindSyncedEntry(topSection, LguConstants.WEED_GENETIC_MANIPULATION_OVERRIDE_NAME_KEY, WeedGeneticManipulation.UPGRADE_NAME);
            CLAY_GLASSES_OVERRIDE_NAME          = cfg.BindSyncedEntry(topSection, LguConstants.CLAY_GLASSES_OVERRIDE_NAME_KEY, ClayGlasses.UPGRADE_NAME);
            MECHANICAL_ARMS_OVERRIDE_NAME       = cfg.BindSyncedEntry(topSection, LguConstants.MECHANICAL_ARMS_OVERRIDE_NAME_KEY, MechanicalArms.UPGRADE_NAME);
            SCAVENGER_INSTINCTS_OVERRIDE_NAME   = cfg.BindSyncedEntry(topSection, LguConstants.SCAVENGER_INSTINCTS_OVERRIDE_NAME_KEY, ScavengerInstincts.UPGRADE_NAME);
            LANDING_THRUSTERS_OVERRIDE_NAME     = cfg.BindSyncedEntry(topSection, LguConstants.LANDING_THRUSTERS_OVERRIDE_NAME_KEY, LandingThrusters.UPGRADE_NAME);
            REINFORCED_BOOTS_OVERRIDE_NAME      = cfg.BindSyncedEntry(topSection, LguConstants.REINFORCED_BOOTS_OVERRIDE_NAME_KEY, ReinforcedBoots.UPGRADE_NAME);
            DEEPER_POCKETS_OVERRIDE_NAME        = cfg.BindSyncedEntry(topSection, LguConstants.DEEPER_POCKETS_OVERRIDE_NAME_KEY, DeepPockets.UPGRADE_NAME);
            ALUMINIUM_COILS_OVERRIDE_NAME       = cfg.BindSyncedEntry(topSection, LguConstants.ALUMINIUM_COILS_OVERRIDE_NAME_KEY, AluminiumCoils.UPGRADE_NAME);
            BACK_MUSCLES_OVERRIDE_NAME          = cfg.BindSyncedEntry(topSection, LguConstants.BACK_MUSCLES_OVERRIDE_NAME_KEY, BackMuscles.UPGRADE_NAME);
            BARGAIN_CONNECTIONS_OVERRIDE_NAME   = cfg.BindSyncedEntry(topSection, LguConstants.BARGAIN_CONNECTIONS_OVERRIDE_NAME_KEY, BargainConnections.UPGRADE_NAME);
            BEEKEEPER_OVERRIDE_NAME             = cfg.BindSyncedEntry(topSection, LguConstants.BEEKEEPER_OVERRIDE_NAME_KEY, Beekeeper.UPGRADE_NAME);
            BETTER_SCANNER_OVERRIDE_NAME        = cfg.BindSyncedEntry(topSection, LguConstants.BETTER_SCANNER_OVERRIDE_NAME_KEY, BetterScanner.UPGRADE_NAME);
            CHARGING_BOOSTER_OVERRIDE_NAME      = cfg.BindSyncedEntry(topSection, LguConstants.CHARGING_BOOSTER_OVERRIDE_NAME_KEY, ChargingBooster.UPGRADE_NAME);
            DISCOMBOBULATOR_OVERRIDE_NAME       = cfg.BindSyncedEntry(topSection, LguConstants.DISCOMBOBULATOR_OVERRIDE_NAME_KEY, Discombobulator.UPGRADE_NAME);
            EFFICIENT_ENGINES_OVERRIDE_NAME     = cfg.BindSyncedEntry(topSection, LguConstants.EFFICIENT_ENGINES_OVERRIDE_NAME_KEY, EfficientEngines.UPGRADE_NAME);
            HUNTER_OVERRIDE_NAME                = cfg.BindSyncedEntry(topSection, LguConstants.HUNTER_OVERRIDE_NAME_KEY, Hunter.UPGRADE_NAME);
            LITHIUM_BATTERIES_OVERRIDE_NAME     = cfg.BindSyncedEntry(topSection, LguConstants.LITHIUM_BATTERIES_OVERRIDE_NAME_KEY, LithiumBatteries.UPGRADE_NAME);
            MARKET_INFLUENCE_OVERRIDE_NAME      = cfg.BindSyncedEntry(topSection, LguConstants.MARKET_INFLUENCE_OVERRIDE_NAME_KEY, MarketInfluence.UPGRADE_NAME);
            NIGHT_VISION_OVERRIDE_NAME          = cfg.BindSyncedEntry(topSection, LguConstants.NIGHT_VISION_OVERRIDE_NAME_KEY, NightVision.UPGRADE_NAME);
            PROTEIN_POWDER_OVERRIDE_NAME        = cfg.BindSyncedEntry(topSection, LguConstants.PROTEIN_POWDER_OVERRIDE_NAME_KEY, ProteinPowder.UPGRADE_NAME);
            BIGGER_LUNGS_OVERRIDE_NAME          = cfg.BindSyncedEntry(topSection, LguConstants.BIGGER_LUNGS_OVERRIDE_NAME_KEY, BiggerLungs.UPGRADE_NAME);
            CLIMBING_GLOVES_OVERRIDE_NAME       = cfg.BindSyncedEntry(topSection, LguConstants.CLIMBING_GLOVES_OVERRIDE_NAME_KEY, ClimbingGloves.UPGRADE_NAME);
            SHUTTER_BATTERIES_OVERRIDE_NAME     = cfg.BindSyncedEntry(topSection, LguConstants.SHUTTER_BATTERIES_OVERRIDE_NAME_KEY, ShutterBatteries.UPGRADE_NAME);
            QUANTUM_DISRUPTOR_OVERRIDE_NAME     = cfg.BindSyncedEntry(topSection, LguConstants.QUANTUM_DISRUPTOR_OVERRIDE_NAME_KEY, QuantumDisruptor.UPGRADE_NAME);
            RUNNING_SHOES_OVERRIDE_NAME         = cfg.BindSyncedEntry(topSection, LguConstants.RUNNING_SHOES_OVERRIDE_NAME_KEY, RunningShoes.UPGRADE_NAME);
            STIMPACK_OVERRIDE_NAME              = cfg.BindSyncedEntry(topSection, LguConstants.STIMPACK_OVERRIDE_NAME_KEY, Stimpack.UPGRADE_NAME);
            STRONG_LEGS_OVERRIDE_NAME           = cfg.BindSyncedEntry(topSection, LguConstants.STRONG_LEGS_OVERRIDE_NAME_KEY, StrongLegs.UPGRADE_NAME);
            FAST_ENCRYPTION_OVERRIDE_NAME       = cfg.BindSyncedEntry(topSection, LguConstants.FAST_ENCRYPTION_OVERRIDE_NAME_KEY, FastEncryption.UPGRADE_NAME);
            DROP_POD_THRUSTERS_OVERRIDE_NAME    = cfg.BindSyncedEntry(topSection, LguConstants.DROP_POD_THRUSTERS_OVERRIDE_NAME_KEY, FasterDropPod.UPGRADE_NAME);
            LETHAL_DEALS_OVERRIDE_NAME          = cfg.BindSyncedEntry(topSection, LguConstants.LETHAL_DEALS_OVERRIDE_NAME_KEY, LethalDeals.UPGRADE_NAME);
            LIGHTNING_ROD_OVERRIDE_NAME         = cfg.BindSyncedEntry(topSection, LguConstants.LIGHTNING_ROD_OVERRIDE_NAME_KEY, LightningRod.UPGRADE_NAME);
            LOCKSMITH_OVERRIDE_NAME             = cfg.BindSyncedEntry(topSection, LguConstants.LOCKSMITH_OVERRIDE_NAME_KEY, LockSmith.UPGRADE_NAME);
            MALWARE_BROADCASTER_OVERRIDE_NAME   = cfg.BindSyncedEntry(topSection, LguConstants.MALWARE_BROADCASTER_OVERRIDE_NAME_KEY, MalwareBroadcaster.UPGRADE_NAME);
            SICK_BEATS_OVERRIDE_NAME            = cfg.BindSyncedEntry(topSection, LguConstants.SICK_BEATS_OVERRIDE_NAME_KEY, SickBeats.UPGRADE_NAME);
            SIGURD_ACCESS_OVERRIDE_NAME         = cfg.BindSyncedEntry(topSection, LguConstants.SIGURD_ACCESS_OVERRIDE_NAME_KEY, Sigurd.UPGRADE_NAME);
            WALKIE_GPS_OVERRIDE_NAME            = cfg.BindSyncedEntry(topSection, LguConstants.WALKIE_GPS_OVERRIDE_NAME_KEY, WalkieGPS.UPGRADE_NAME);

            #endregion

            #region Contracts

            topSection = LguConstants.CONTRACTS_SECTION;
            CONTRACTS_ENABLED                   = cfg.BindSyncedEntry(topSection, LguConstants.ENABLE_CONTRACTS_KEY                 , LguConstants.ENABLE_CONTRACTS_DEFAULT);
            CONTRACT_PROVIDE_RANDOM_ONLY        = cfg.BindSyncedEntry(topSection, LguConstants.CONTRACT_PROVIDE_RANDOM_ONLY_KEY     , LguConstants.CONTRACT_PROVIDE_RANDOM_ONLY_DEFAULT, LguConstants.CONTRACT_PROVIDE_RANDOM_ONLY_DESCRIPTION);
            CONTRACT_FREE_MOONS_ONLY            = cfg.BindSyncedEntry(topSection, LguConstants.CONTRACT_FREE_MOONS_ONLY_KEY         , LguConstants.CONTRACT_FREE_MOONS_ONLY_DEFAULT, LguConstants.CONTRACT_FREE_MOONS_ONLY_DESCRIPTION);
            CONTRACT_PRICE                      = cfg.BindSyncedEntry(topSection, LguConstants.CONTRACT_PRICE_KEY                   , LguConstants.CONTRACT_PRICE_DEFAULT);
            CONTRACT_SPECIFY_PRICE              = cfg.BindSyncedEntry(topSection, LguConstants.CONTRACT_SPECIFY_PRICE_KEY           , LguConstants.CONTRACT_SPECIFY_PRICE_DEFAULT);
            CONTRACT_BUG_REWARD                 = cfg.BindSyncedEntry(topSection, LguConstants.CONTRACT_BUG_REWARD_KEY              , LguConstants.CONTRACT_BUG_REWARD_DEFAULT);
            CONTRACT_EXOR_REWARD                = cfg.BindSyncedEntry(topSection, LguConstants.CONTRACT_EXORCISM_REWARD_KEY         , LguConstants.CONTRACT_EXORCISM_REWARD_DEFAULT);
            CONTRACT_DEFUSE_REWARD              = cfg.BindSyncedEntry(topSection, LguConstants.CONTRACT_DEFUSAL_REWARD_KEY          , LguConstants.CONTRACT_DEFUSAL_REWARD_DEFAULT);
            CONTRACT_EXTRACT_REWARD             = cfg.BindSyncedEntry(topSection, LguConstants.CONTRACT_EXTRACTION_REWARD_KEY       , LguConstants.CONTRACT_EXTRACTION_REWARD_DEFAULT);
            CONTRACT_DATA_REWARD                = cfg.BindSyncedEntry(topSection, LguConstants.CONTRACT_DATA_REWARD_KEY             , LguConstants.CONTRACT_DATA_REWARD_DEFAULT);
            CONTRACT_BUG_SPAWNS                 = cfg.BindSyncedEntry(topSection, LguConstants.EXTERMINATION_BUG_SPAWNS_KEY         , LguConstants.EXTERMINATION_BUG_SPAWNS_DEFAULT, LguConstants.EXTERMINATION_BUG_SPAWNS_DESCRIPTION);
            CONTRACT_GHOST_SPAWN                = cfg.BindSyncedEntry(topSection, LguConstants.EXORCISM_GHOST_SPAWN_KEY             , LguConstants.EXORCISM_GHOST_SPAWN_DEFAULT, LguConstants.EXORCISM_GHOST_SPAWN_DESCRIPTION);
            CONTRACT_EXTRACT_WEIGHT             = cfg.BindSyncedEntry(topSection, LguConstants.EXTRACTION_SCAVENGER_WEIGHT_KEY      , LguConstants.EXTRACTION_SCAVENGER_WEIGHT_DEFAULT, LguConstants.EXTRACTION_SCAVENGER_WEIGHT_DESCRIPTION);
            SCAV_VOLUME                         = cfg.BindSyncedEntry(topSection, LguConstants.EXTRACTION_SCAVENGER_SOUND_VOLUME_KEY, LguConstants.EXTRACTION_SCAVENGER_SOUND_VOLUME_DEFAULT, LguConstants.EXTRACTION_SCAVENGER_SOUND_VOLUME_DESCRIPTION);
            MAIN_OBJECT_FURTHEST                = cfg.BindSyncedEntry(topSection, LguConstants.CONTRACT_FAR_FROM_MAIN_KEY           , LguConstants.CONTRACT_FAR_FROM_MAIN_DEFAULT, LguConstants.CONTRACT_FAR_FROM_MAIN_DESCRIPTION);
            EXTRACTION_CONTRACT_AMOUNT_MEDKITS  = cfg.BindSyncedEntry(topSection, LguConstants.EXTRACTION_MEDKIT_AMOUNT_KEY         , LguConstants.EXTRACTION_MEDKIT_AMOUNT_DEFAULT);

            // this is kind of dumb and I'd like to just use a comma seperated cfg.BindSyncedEntry<string> but this is much more foolproof
            DATA_CONTRACT                       = cfg.BindSyncedEntry(topSection, LguConstants.CONTRACT_DATA_ENABLED_KEY            , LguConstants.CONTRACT_DATA_ENABLED_DEFAULT, LguConstants.CONTRACT_DATA_ENABLED_DESCRIPTION);
            EXTRACTION_CONTRACT                 = cfg.BindSyncedEntry(topSection, LguConstants.CONTRACT_EXTRACTION_ENABLED_KEY      , LguConstants.CONTRACT_EXTRACTION_ENABLED_DEFAULT, LguConstants.CONTRACT_EXTRACTION_ENABLED_DESCRIPTION);
            EXORCISM_CONTRACT                   = cfg.BindSyncedEntry(topSection, LguConstants.CONTRACT_EXORCISM_ENABLED_KEY        , LguConstants.CONTRACT_EXORCISM_ENABLED_DEFAULT, LguConstants.CONTRACT_EXORCISM_ENABLED_DESCRIPTION);
            DEFUSAL_CONTRACT                    = cfg.BindSyncedEntry(topSection, LguConstants.CONTRACT_DEFUSAL_ENABLED_KEY         , LguConstants.CONTRACT_DEFUSAL_ENABLED_DEFAULT, LguConstants.CONTRACT_DEFUSAL_ENABLED_DESCRIPTION);
            EXTERMINATOR_CONTRACT               = cfg.BindSyncedEntry(topSection, LguConstants.CONTRACT_EXTERMINATION_ENABLED_KEY   , LguConstants.CONTRACT_EXTERMINATION_ENABLED_DEFAULT, LguConstants.CONTRACT_EXTERMINATION_ENABLED_DESCRIPTION);
            CONTRACT_REWARD_QUOTA_MULTIPLIER    = cfg.BindSyncedEntry(topSection, LguConstants.CONTRACT_QUOTA_MULTIPLIER_KEY        , LguConstants.CONTRACT_QUOTA_MULTIPLIER_DEFAULT, LguConstants.CONTRACT_QUOTA_MULTIPLIER_DESCRIPTION);

            #endregion

            #region Items

            #region Medkit

            topSection = Medkit.ITEM_NAME;
            MEDKIT_ENABLED = cfg.BindSyncedEntry(topSection, LguConstants.MEDKIT_ENABLED_KEY, LguConstants.MEDKIT_ENABLED_DEFAULT, LguConstants.MEDKIT_ENABLED_DESCRIPTION);
            MEDKIT_PRICE = cfg.BindSyncedEntry(topSection, LguConstants.MEDKIT_PRICE_KEY, LguConstants.MEDKIT_PRICE_DEFAULT, LguConstants.MEDKIT_PRICE_DESCRIPTION);
            MEDKIT_HEAL_VALUE = cfg.BindSyncedEntry(topSection, LguConstants.MEDKIT_HEAL_AMOUNT_KEY, LguConstants.MEDKIT_HEAL_AMOUNT_DEFAULT, LguConstants.MEDKIT_HEAL_AMOUNT_DESCRIPTION);
            MEDKIT_USES = cfg.BindSyncedEntry(topSection, LguConstants.MEDKIT_USES_KEY, LguConstants.MEDKIT_USES_DEFAULT, LguConstants.MEDKIT_USES_DESCRIPTION);
            MEDKIT_SCAN_NODE = cfg.BindSyncedEntry(topSection, LguConstants.MEDKIT_SCAN_NODE_KEY, LguConstants.ITEM_SCAN_NODE_DEFAULT, LguConstants.ITEM_SCAN_NODE_DESCRIPTION);

            #endregion

            #endregion

            #region Upgrades

            #region Rubber Boots

            topSection = RubberBoots.UPGRADE_NAME;
            RUBBER_BOOTS_ENABLED = cfg.BindSyncedEntry(topSection, LguConstants.RUBBER_BOOTS_ENABLED_KEY, LguConstants.RUBBER_BOOTS_ENABLED_DEFAULT, LguConstants.RUBBER_BOOTS_ENABLED_DESCRIPTION);
            RUBBER_BOOTS_INDIVIDUAL = cfg.BindSyncedEntry(topSection, BaseUpgrade.INDIVIDUAL_SECTION, BaseUpgrade.INDIVIDUAL_DEFAULT, BaseUpgrade.INDIVIDUAL_DESCRIPTION);
            RUBBER_BOOTS_PRICE = cfg.BindSyncedEntry(topSection, LguConstants.RUBBER_BOOTS_PRICE_KEY, LguConstants.RUBBER_BOOTS_PRICE_DEFAULT);
            RUBBER_BOOTS_PRICES = cfg.BindSyncedEntry(topSection, BaseUpgrade.PRICES_SECTION, RubberBoots.DEFAULT_PRICES, BaseUpgrade.PRICES_DESCRIPTION);
            RUBBER_BOOTS_INITIAL_MOVEMENT_HINDERANCE_DECREASE = cfg.BindSyncedEntry(topSection, LguConstants.RUBBER_BOOTS_INITIAL_MOVEMENT_HINDERANCE_DECREASE_KEY, LguConstants.RUBBER_BOOTS_INITIAL_MOVEMENT_HINDERANCE_DECREASE_DEFAULT, LguConstants.RUBBER_BOOTS_INITIAL_MOVEMENT_HINDERANCE_DECREASE_DESCRIPTION);
            RUBBER_BOOTS_INCREMENTAL_MOVEMENT_HINDERANCE_DECREASE = cfg.BindSyncedEntry(topSection, LguConstants.RUBBER_BOOTS_INCREMENTAL_MOVEMENT_HINDERANCE_DECREASE_KEY, LguConstants.RUBBER_BOOTS_INCREMENTAL_MOVEMENT_HINDERANCE_DECREASE_DEFAULT, LguConstants.RUBBER_BOOTS_INCREMENTAL_MOVEMENT_HINDERANCE_DECREASE_DESCRIPTION);
            RUBBER_BOOTS_ITEM_PROGRESSION_ITEMS = cfg.BindSyncedEntry(topSection, LguConstants.ITEM_PROGRESSION_ITEMS_KEY, LguConstants.ITEM_PROGRESSION_ITEMS_DEFAULT, LguConstants.ITEM_PROGRESSION_ITEMS_DESCRIPTION);

            #endregion

            #region Oxygen Canisters

            topSection = OxygenCanisters.UPGRADE_NAME;
            OXYGEN_CANISTERS_ENABLED = cfg.BindSyncedEntry(topSection, LguConstants.OXYGEN_CANISTERS_ENABLED_KEY, LguConstants.OXYGEN_CANISTERS_ENABLED_DEFAULT, LguConstants.OXYGEN_CANISTERS_ENABLED_DESCRIPTION);
            OXYGEN_CANISTERS_INDIVIDUAL = cfg.BindSyncedEntry(topSection, BaseUpgrade.INDIVIDUAL_SECTION, BaseUpgrade.INDIVIDUAL_DEFAULT, BaseUpgrade.INDIVIDUAL_DESCRIPTION);
            OXYGEN_CANISTERS_PRICE = cfg.BindSyncedEntry(topSection, LguConstants.OXYGEN_CANISTERS_PRICE_KEY, LguConstants.OXYGEN_CANISTERS_PRICE_DEFAULT);
            OXYGEN_CANISTERS_PRICES = cfg.BindSyncedEntry(topSection, BaseUpgrade.PRICES_SECTION, OxygenCanisters.DEFAULT_PRICES, BaseUpgrade.PRICES_DESCRIPTION);
            OXYGEN_CANISTERS_INITIAL_OXYGEN_CONSUMPTION_DECREASE = cfg.BindSyncedEntry(topSection, LguConstants.OXYGEN_CANISTERS_INITIAL_OXYGEN_CONSUMPTION_DECREASE_KEY, LguConstants.OXYGEN_CANISTERS_INITIAL_OXYGEN_CONSUMPTION_DECREASE_DEFAULT, LguConstants.OXYGEN_CANISTERS_INITIAL_OXYGEN_CONSUMPTION_DECREASE_DESCRIPTION);
            OXYGEN_CANISTERS_INCREMENTAL_OXYGEN_CONSUMPTION_DECREASE = cfg.BindSyncedEntry(topSection, LguConstants.OXYGEN_CANISTERS_INCREMENTAL_OXYGEN_CONSUMPTION_DECREASE_KEY, LguConstants.OXYGEN_CANISTERS_INCREMENTAL_OXYGEN_CONSUMPTION_DECREASE_DEFAULT, LguConstants.OXYGEN_CANISTERS_INCREMENTAL_OXYGEN_CONSUMPTION_DECREASE_DESCRIPTION);
            OXYGEN_CANISTERS_ITEM_PROGRESSION_ITEMS = cfg.BindSyncedEntry(topSection, LguConstants.ITEM_PROGRESSION_ITEMS_KEY, LguConstants.ITEM_PROGRESSION_ITEMS_DEFAULT, LguConstants.ITEM_PROGRESSION_ITEMS_DESCRIPTION);

            #endregion

            #region Sleight Of Hand

            topSection = SleightOfHand.UPGRADE_NAME;
            SLEIGHT_OF_HAND_ENABLED = cfg.BindSyncedEntry(topSection, LguConstants.SLEIGHT_OF_HAND_ENABLED_KEY, LguConstants.SLEIGHT_OF_HAND_ENABLED_DEFAULT, LguConstants.SLEIGHT_OF_HAND_ENABLED_DESCRIPTION);
            SLEIGHT_OF_HAND_INDIVIDUAL = cfg.BindSyncedEntry(topSection, BaseUpgrade.INDIVIDUAL_SECTION, BaseUpgrade.INDIVIDUAL_DEFAULT, BaseUpgrade.INDIVIDUAL_DESCRIPTION);
            SLEIGHT_OF_HAND_PRICE = cfg.BindSyncedEntry(topSection, LguConstants.SLEIGHT_OF_HAND_PRICE_KEY, LguConstants.SLEIGHT_OF_HAND_PRICE_DEFAULT);
            SLEIGHT_OF_HAND_PRICES = cfg.BindSyncedEntry(topSection, BaseUpgrade.PRICES_SECTION, SleightOfHand.PRICES_DEFAULT, BaseUpgrade.PRICES_DESCRIPTION);
            SLEIGHT_OF_HAND_INITIAL_INCREASE = cfg.BindSyncedEntry(topSection, LguConstants.SLEIGHT_OF_HAND_INITIAL_INCREASE_KEY, LguConstants.SLEIGHT_OF_HAND_INITIAL_INCREASE_DEFAULT, LguConstants.SLEIGHT_OF_HAND_INITIAL_INCREASE_DESCRIPTION);
            SLEIGHT_OF_HAND_INCREMENTAL_INCREASE = cfg.BindSyncedEntry(topSection, LguConstants.SLEIGHT_OF_HAND_INCREMENTAL_INCREASE_KEY, LguConstants.SLEIGHT_OF_HAND_INCREMENTAL_INCREASE_DEFAULT, LguConstants.SLEIGHT_OF_HAND_INCREMENTAL_INCREASE_DESCRIPTION);
            SLEIGHT_OF_HAND_ITEM_PROGRESSION_ITEMS = cfg.BindSyncedEntry(topSection, LguConstants.ITEM_PROGRESSION_ITEMS_KEY, LguConstants.ITEM_PROGRESSION_ITEMS_DEFAULT, LguConstants.ITEM_PROGRESSION_ITEMS_DESCRIPTION);

            #endregion

            #region Hiking Boots

            topSection = HikingBoots.UPGRADE_NAME;
            HIKING_BOOTS_ENABLED = cfg.BindSyncedEntry(topSection, LguConstants.HIKING_BOOTS_ENABLED_KEY, LguConstants.HIKING_BOOTS_ENABLED_DEFAULT, LguConstants.HIKING_BOOTS_ENABLED_DESCRIPTION);
            HIKING_BOOTS_INDIVIDUAL = cfg.BindSyncedEntry(topSection, BaseUpgrade.INDIVIDUAL_SECTION, BaseUpgrade.INDIVIDUAL_DEFAULT, BaseUpgrade.INDIVIDUAL_DESCRIPTION);
            HIKING_BOOTS_PRICE = cfg.BindSyncedEntry(topSection, LguConstants.HIKING_BOOTS_PRICE_KEY, LguConstants.HIKING_BOOTS_PRICE_DEFAULT);
            HIKING_BOOTS_PRICES = cfg.BindSyncedEntry(topSection, BaseUpgrade.PRICES_SECTION, HikingBoots.PRICES_DEFAULT, BaseUpgrade.PRICES_DESCRIPTION);
            HIKING_BOOTS_INITIAL_DECREASE = cfg.BindSyncedEntry(topSection, LguConstants.HIKING_BOOTS_INITIAL_DECREASE_KEY, LguConstants.HIKING_BOOTS_INITIAL_DECREASE_DEFAULT, LguConstants.HIKING_BOOTS_INITIAL_DECREASE_DESCRIPTION);
            HIKING_BOOTS_INCREMENTAL_DECREASE = cfg.BindSyncedEntry(topSection, LguConstants.HIKING_BOOTS_INCREMENTAL_DECREASE_KEY, LguConstants.HIKING_BOOTS_INCREMENTAL_DECREASE_DEFAULT, LguConstants.HIKING_BOOTS_INCREMENTAL_DECREASE_DESCRIPTION);
            HIKING_BOOTS_ITEM_PROGRESSION_ITEMS = cfg.BindSyncedEntry(topSection, LguConstants.ITEM_PROGRESSION_ITEMS_KEY, LguConstants.ITEM_PROGRESSION_ITEMS_DEFAULT, LguConstants.ITEM_PROGRESSION_ITEMS_DESCRIPTION);

            #endregion

            #region Traction Boots
            topSection = TractionBoots.UPGRADE_NAME;
            TRACTION_BOOTS_ENABLED = cfg.BindSyncedEntry(topSection, LguConstants.TRACTION_BOOTS_ENABLED_KEY, LguConstants.TRACTION_BOOTS_ENABLED_DEFAULT, LguConstants.TRACTION_BOOTS_ENABLED_DESCRIPTION);
            TRACTION_BOOTS_INDIVIDUAL = cfg.BindSyncedEntry(topSection, BaseUpgrade.INDIVIDUAL_SECTION, BaseUpgrade.INDIVIDUAL_DEFAULT, BaseUpgrade.INDIVIDUAL_DESCRIPTION);
            TRACTION_BOOTS_PRICE = cfg.BindSyncedEntry(topSection, LguConstants.TRACTION_BOOTS_PRICE_KEY, LguConstants.TRACTION_BOOTS_PRICE_DEFAULT);
            TRACTION_BOOTS_PRICES = cfg.BindSyncedEntry(topSection, BaseUpgrade.PRICES_SECTION, TractionBoots.PRICES_DEFAULT, BaseUpgrade.PRICES_DESCRIPTION);
            TRACTION_BOOTS_INITIAL_INCREASE = cfg.BindSyncedEntry(topSection, LguConstants.TRACTION_BOOTS_INITIAL_INCREASE_KEY, LguConstants.TRACTION_BOOTS_INITIAL_INCREASE_DEFAULT, LguConstants.TRACTION_BOOTS_INITIAL_INCREASE_DESCRIPTION);
            TRACTION_BOOTS_INCREMENTAL_INCREASE = cfg.BindSyncedEntry(topSection, LguConstants.TRACTION_BOOTS_INCREMENTAL_INCREASE_KEY, LguConstants.TRACTION_BOOTS_INCREMENTAL_INCREASE_DEFAULT, LguConstants.TRACTION_BOOTS_INCREMENTAL_INCREASE_DESCRIPTION);
            TRACTION_BOOTS_ITEM_PROGRESSION_ITEMS = cfg.BindSyncedEntry(topSection, LguConstants.ITEM_PROGRESSION_ITEMS_KEY, LguConstants.ITEM_PROGRESSION_ITEMS_DEFAULT, LguConstants.ITEM_PROGRESSION_ITEMS_DESCRIPTION);
            #endregion

            #region Fedora Suit

            topSection = FedoraSuit.UPGRADE_NAME;
            FEDORA_SUIT_ENABLED = cfg.BindSyncedEntry(topSection, LguConstants.FEDORA_SUIT_ENABLED_KEY, LguConstants.FEDORA_SUIT_ENABLED_DEFAULT, LguConstants.FEDORA_SUIT_ENABLED_DESCRIPTION);
            FEDORA_SUIT_INDIVIDUAL = cfg.BindSyncedEntry(topSection, BaseUpgrade.INDIVIDUAL_SECTION, BaseUpgrade.INDIVIDUAL_DEFAULT, BaseUpgrade.INDIVIDUAL_DESCRIPTION);
            FEDORA_SUIT_PRICE = cfg.BindSyncedEntry(topSection, LguConstants.FEDORA_SUIT_PRICE_KEY, LguConstants.FEDORA_SUIT_PRICE_DEFAULT);
            FEDORA_SUIT_ITEM_PROGRESSION_ITEMS = cfg.BindSyncedEntry(topSection, LguConstants.ITEM_PROGRESSION_ITEMS_KEY, LguConstants.ITEM_PROGRESSION_ITEMS_DEFAULT, LguConstants.ITEM_PROGRESSION_ITEMS_DESCRIPTION);

            #endregion

            #region Weed Genetic Manipulation

            topSection = WeedGeneticManipulation.UPGRADE_NAME;
            WEED_GENETIC_MANIPULATION_ENABLED = cfg.BindSyncedEntry(topSection, LguConstants.WEED_GENETIC_MANIPULATION_ENABLED_KEY, LguConstants.WEED_GENETIC_MANIPULATION_ENABLED_DEFAULT, LguConstants.WEED_GENETIC_MANIPULATION_ENABLED_DESCRIPTION);
            WEED_GENETIC_MANIPULATION_PRICE = cfg.BindSyncedEntry(topSection, LguConstants.WEED_GENETIC_MANIPULATION_PRICE_KEY, LguConstants.WEED_GENETIC_MANIPULATION_PRICE_DEFAULT);
            WEED_GENETIC_MANIPULATION_PRICES = cfg.BindSyncedEntry(topSection, BaseUpgrade.PRICES_SECTION, WeedGeneticManipulation.PRICES_DEFAULT, BaseUpgrade.PRICES_DESCRIPTION);
            WEED_GENETIC_MANIPULATION_INITIAL_EFFECTIVENESS_INCREASE = cfg.BindSyncedEntry(topSection, LguConstants.WEED_GENETIC_INITIAL_EFFECTIVENESS_INCREASE_KEY, LguConstants.WEED_GENETIC_INITIAL_EFFECTIVENESS_INCREASE_DEFAULT, LguConstants.WEED_GENETIC_INITIAL_EFFECTIVENESS_INCREASE_DESCRIPTION);
            WEED_GENETIC_MANIPULATION_INCREMENTAL_EFFECTIVENESS_INCREASE = cfg.BindSyncedEntry(topSection, LguConstants.WEED_GENETIC_INCREMENTAL_EFFECTIVENESS_INCREASE_KEY, LguConstants.WEED_GENETIC_INCREMENTAL_EFFECTIVENESS_INCREASE_DEFAULT, LguConstants.WEED_GENETIC_INCREMENTAL_EFFECTIVENESS_INCREASE_DESCRIPTION);
            WEED_GENETIC_MANIPULATION_ITEM_PROGRESSION_ITEMS = cfg.BindSyncedEntry(topSection, LguConstants.ITEM_PROGRESSION_ITEMS_KEY, LguConstants.ITEM_PROGRESSION_ITEMS_DEFAULT, LguConstants.ITEM_PROGRESSION_ITEMS_DESCRIPTION);

            #endregion

            #region Clay Glasses

            topSection = ClayGlasses.UPGRADE_NAME;
            CLAY_GLASSES_ENABLED = cfg.BindSyncedEntry(topSection, LguConstants.CLAY_GLASSES_ENABLED_KEY, LguConstants.CLAY_GLASSES_ENABLED_DEFAULT, LguConstants.CLAY_GLASSES_ENABLED_DESCRIPTION);
            CLAY_GLASSES_INDIVIDUAL = cfg.BindSyncedEntry(topSection, BaseUpgrade.INDIVIDUAL_SECTION, BaseUpgrade.INDIVIDUAL_DEFAULT, BaseUpgrade.INDIVIDUAL_DESCRIPTION);
            CLAY_GLASSES_PRICE = cfg.BindSyncedEntry(topSection, LguConstants.CLAY_GLASSES_PRICE_KEY, LguConstants.CLAY_GLASSES_PRICE_DEFAULT);
            CLAY_GLASSES_PRICES = cfg.BindSyncedEntry(topSection, BaseUpgrade.PRICES_SECTION, ClayGlasses.PRICES_DEFAULT, BaseUpgrade.PRICES_DESCRIPTION);
            CLAY_GLASSES_DISTANCE_INITIAL_INCREASE = cfg.BindSyncedEntry(topSection, LguConstants.CLAY_GLASSES_DISTANCE_INITIAL_INCREASE_KEY, LguConstants.CLAY_GLASSES_DISTANCE_INITIAL_INCREASE_DEFAULT, LguConstants.CLAY_GLASSES_DISTANCE_INITIAL_INCREASE_DESCRIPTION);
            CLAY_GLASSES_DISTANCE_INCREMENTAL_INCREASE = cfg.BindSyncedEntry(topSection, LguConstants.CLAY_GLASSES_DISTANCE_INCREMENTAL_INCREASE_KEY, LguConstants.CLAY_GLASSES_DISTANCE_INCREMENTAL_INCREASE_DEFAULT, LguConstants.CLAY_GLASSES_DISTANCE_INCREMENTAL_INCREASE_DESCRIPTION);
            CLAY_GLASSES_ITEM_PROGRESSION_ITEMS = cfg.BindSyncedEntry(topSection, LguConstants.ITEM_PROGRESSION_ITEMS_KEY, LguConstants.ITEM_PROGRESSION_ITEMS_DEFAULT, LguConstants.ITEM_PROGRESSION_ITEMS_DESCRIPTION);

            #endregion

            #region Mechanical Arms

            topSection = MechanicalArms.UPGRADE_NAME;
            MECHANICAL_ARMS_ENABLED = cfg.BindSyncedEntry(topSection, LguConstants.MECHANICAL_ARMS_ENABLED_KEY, LguConstants.MECHANICAL_ARMS_ENABLED_DEFAULT, LguConstants.MECHANICAL_ARMS_ENABLED_DESCRIPTION);
            MECHANICAL_ARMS_INDIVIDUAL = cfg.BindSyncedEntry(topSection, BaseUpgrade.INDIVIDUAL_SECTION, BaseUpgrade.INDIVIDUAL_DEFAULT, BaseUpgrade.INDIVIDUAL_DESCRIPTION);
            MECHANICAL_ARMS_PRICE = cfg.BindSyncedEntry(topSection, LguConstants.MECHANICAL_ARMS_PRICE_KEY, LguConstants.MECHANICAL_ARMS_PRICE_DEFAULT);
            MECHANICAL_ARMS_PRICES = cfg.BindSyncedEntry(topSection, BaseUpgrade.PRICES_SECTION, MechanicalArms.PRICES_DEFAULT, BaseUpgrade.PRICES_DESCRIPTION);
            MECHANICAL_ARMS_INITIAL_RANGE_INCREASE = cfg.BindSyncedEntry(topSection, LguConstants.MECHANICAL_ARMS_INITIAL_RANGE_INCREASE_KEY, LguConstants.MECHANICAL_ARMS_INITIAL_RANGE_INCREASE_DEFAULT, LguConstants.MECHANICAL_ARMS_INITIAL_RANGE_INCREASE_DESCRIPTION);
            MECHANICAL_ARMS_INCREMENTAL_RANGE_INCREASE = cfg.BindSyncedEntry(topSection, LguConstants.MECHANICAL_ARMS_INCREMENTAL_RANGE_INCREASE_KEY, LguConstants.MECHANICAL_ARMS_INCREMENTAL_RANGE_INCREASE_DEFAULT, LguConstants.MECHANICAL_ARMS_INCREMENTAL_RANGE_INCREASE_DESCRIPTION);
            MECHANICAL_ARMS_ITEM_PROGRESSION_ITEMS = cfg.BindSyncedEntry(topSection, LguConstants.ITEM_PROGRESSION_ITEMS_KEY, LguConstants.ITEM_PROGRESSION_ITEMS_DEFAULT, LguConstants.ITEM_PROGRESSION_ITEMS_DESCRIPTION);
            #endregion

            #region Scavenger Instincts
            topSection = ScavengerInstincts.UPGRADE_NAME;
            SCAVENGER_INSTINCTS_ENABLED = cfg.BindSyncedEntry(topSection, LguConstants.SCAVENGER_INSTINCTS_ENABLED_KEY, LguConstants.SCAVENGER_INSTINCTS_ENABLED_DEFAULT, LguConstants.SCAVENGER_INSTINCTS_ENABLED_DESCRIPTION);
            SCAVENGER_INSTINCTS_PRICE = cfg.BindSyncedEntry(topSection, LguConstants.SCAVENGER_INSTINCTS_PRICE_KEY, LguConstants.SCAVENGER_INSTINCTS_PRICE_DEFAULT);
            SCAVENGER_INSTINCTS_PRICES = cfg.BindSyncedEntry(topSection, BaseUpgrade.PRICES_SECTION, ScavengerInstincts.DEFAULT_PRICES, BaseUpgrade.PRICES_DESCRIPTION);
            SCAVENGER_INSTINCTS_INITIAL_AMOUNT_SCRAP_INCREASE = cfg.BindSyncedEntry(topSection, LguConstants.SCAVENGER_INSTINCTS_INITIAL_AMOUNT_SCRAP_INCREASE_KEY, LguConstants.SCAVENGER_INSTINCTS_INITIAL_AMOUNT_SCRAP_INCREASE_DEFAULT, LguConstants.SCAVENGER_INSTINCTS_INITIAL_AMOUNT_SCRAP_INCREASE_DESCRIPTION);
            SCAVENGER_INSTINCTS_INCREMENTAL_AMOUN_SCRAP_INCREASE = cfg.BindSyncedEntry(topSection, LguConstants.SCAVENGER_INSTINCTS_INCREMENTAL_AMOUNT_SCRAP_INCREASE_KEY, LguConstants.SCAVENGER_INSTINCTS_INCREMENTAL_AMOUNT_SCRAP_INCREASE_DEFAULT, LguConstants.SCAVENGER_INSTINCTS_INCREMENTAL_AMOUNT_SCRAP_INCREASE_DESCRIPTION);
            SCAVENGER_INSTINCTS_ITEM_PROGRESSION_ITEMS = cfg.BindSyncedEntry(topSection, LguConstants.ITEM_PROGRESSION_ITEMS_KEY, LguConstants.ITEM_PROGRESSION_ITEMS_DEFAULT, LguConstants.ITEM_PROGRESSION_ITEMS_DESCRIPTION);

            #endregion

            #region Landing Thrusters

            topSection = LandingThrusters.UPGRADE_NAME;
            LANDING_THRUSTERS_ENABLED = cfg.BindSyncedEntry(topSection, LguConstants.LANDING_THRUSTERS_ENABLED_KEY, LguConstants.LANDING_THRUSTERS_ENABLED_DEFAULT, LguConstants.LANDING_THRUSTERS_ENABLED_DESCRIPTION);
            LANDING_THRUSTERS_PRICE = cfg.BindSyncedEntry(topSection, LguConstants.LANDING_THURSTERS_PRICE_KEY, LguConstants.LANDING_THRUSTERS_PRICE_DEFAULT);
            LANDING_THRUSTERS_PRICES = cfg.BindSyncedEntry(topSection, BaseUpgrade.PRICES_SECTION, LandingThrusters.DEFAULT_PRICES, BaseUpgrade.PRICES_DESCRIPTION);
            LANDING_THRUSTERS_INITIAL_SPEED_INCREASE = cfg.BindSyncedEntry(topSection, LguConstants.LANDING_THRUSTERS_INITIAL_SPEED_INCREASE_KEY, LguConstants.LANDING_THRUSTERS_INITIAL_SPEED_INCREASE_DEFAULT, LguConstants.LANDING_THRUSTERS_INITIAL_SPEED_INCREASE_DESCRIPTION);
            LANDING_THRUSTERS_INCREMENTAL_SPEED_INCREASE = cfg.BindSyncedEntry(topSection, LguConstants.LANDING_THRUSTERS_INCREMENTAL_SPEED_INCREASE_KEY, LguConstants.LANDING_THRUSTERS_INCREMENTAL_SPEED_INCREASE_DEFAULT, LguConstants.LANDING_THRUSTERS_INCREMENTAL_SPEED_INCREASE_DESCRIPTION);
            LANDING_THRUSTERS_AFFECT_LANDING = cfg.BindSyncedEntry(topSection, LguConstants.LANDING_THRUSTERS_AFFECT_LANDING_KEY, LguConstants.LANDING_THRUSTERS_AFFECT_LANDING_DEFAULT, LguConstants.LANDING_THRUSTERS_AFFECT_LANDING_DESCRIPTION);
            LANDING_THRUSTERS_AFFECT_DEPARTING = cfg.BindSyncedEntry(topSection, LguConstants.LANDING_THRUSTERS_AFFECT_DEPARTING_KEY, LguConstants.LANDING_THRUSTERS_AFFECT_DEPARTING_DEFAULT, LguConstants.LANDING_THRUSTERS_AFFECT_DEPARTING_DESCRIPTION);
            LANDING_THRUSTERS_ITEM_PROGRESSION_ITEMS = cfg.BindSyncedEntry(topSection, LguConstants.ITEM_PROGRESSION_ITEMS_KEY, LguConstants.ITEM_PROGRESSION_ITEMS_DEFAULT, LguConstants.ITEM_PROGRESSION_ITEMS_DESCRIPTION);

            #endregion

            #region Reinforced Boots

            topSection = ReinforcedBoots.UPGRADE_NAME;
            REINFORCED_BOOTS_ENABLED = cfg.BindSyncedEntry(topSection, LguConstants.REINFORCED_BOOTS_ENABLED_KEY, LguConstants.REINFORCED_BOOTS_ENABLED_DEFAULT, LguConstants.REINFORCED_BOOTS_ENABLED_DESCRIPTION);
            REINFORCED_BOOTS_INDIVIDUAL = cfg.BindSyncedEntry(topSection, BaseUpgrade.INDIVIDUAL_SECTION, BaseUpgrade.INDIVIDUAL_DEFAULT, BaseUpgrade.INDIVIDUAL_DESCRIPTION);
            REINFORCED_BOOTS_PRICE = cfg.BindSyncedEntry(topSection, LguConstants.REINFORCED_BOOTS_PRICE_KEY, LguConstants.REINFORCED_BOOTS_PRICE_DEFAULT);
            REINFORCED_BOOTS_PRICES = cfg.BindSyncedEntry(topSection, BaseUpgrade.PRICES_SECTION, ReinforcedBoots.DEFAULT_PRICES, BaseUpgrade.PRICES_DESCRIPTION);
            REINFORCED_BOOTS_INITIAL_DAMAGE_REDUCTION = cfg.BindSyncedEntry(topSection, LguConstants.REINFORCED_BOOTS_INITIAL_DAMAGE_REDUCTION_KEY, LguConstants.REINFORCED_BOOTS_INITIAL_DAMAGE_REDUCTION_DEFAULT, LguConstants.REINFORCED_BOOTS_INITIAL_DAMAGE_REDUCTION_DESCRIPTION);
            REINFORCED_BOOTS_INCREMENTAL_DAMAGE_REDUCTION = cfg.BindSyncedEntry(topSection, LguConstants.REINFORCED_BOOTS_INCREMENTAL_DAMAGE_REDUCTION_KEY, LguConstants.REINFORCED_BOOTS_INCREMENTAL_DAMAGE_REDUCTION_DEFAULT, LguConstants.REINFORCED_BOOTS_INCREMENTAL_DAMAGE_REDUCTION_DESCRIPTION);
            REINFORCED_BOOTS_ITEM_PROGRESSION_ITEMS = cfg.BindSyncedEntry(topSection, LguConstants.ITEM_PROGRESSION_ITEMS_KEY, LguConstants.ITEM_PROGRESSION_ITEMS_DEFAULT, LguConstants.ITEM_PROGRESSION_ITEMS_DESCRIPTION);

            #endregion

            #region Deeper Pockets

            topSection = DeepPockets.UPGRADE_NAME;
            DEEPER_POCKETS_ENABLED = cfg.BindSyncedEntry(topSection, LguConstants.DEEPER_POCKETS_ENABLED_KEY, LguConstants.DEEPER_POCKETS_ENABLED_DEFAULT, LguConstants.DEEPER_POCKETS_ENABLED_DESCRIPTION);
            DEEPER_POCKETS_INDIVIDUAL = cfg.BindSyncedEntry(topSection, BaseUpgrade.INDIVIDUAL_SECTION, BaseUpgrade.INDIVIDUAL_DEFAULT, BaseUpgrade.INDIVIDUAL_DESCRIPTION);
            DEEPER_POCKETS_PRICE = cfg.BindSyncedEntry(topSection, LguConstants.DEEPER_POCKETS_PRICE_KEY, LguConstants.DEEPER_POCKETS_PRICE_DEFAULT);
            DEEPER_POCKETS_PRICES = cfg.BindSyncedEntry(topSection, BaseUpgrade.PRICES_SECTION, DeepPockets.DEFAULT_PRICES, BaseUpgrade.PRICES_DESCRIPTION);
            DEEPER_POCKETS_INITIAL_TWO_HANDED_ITEMS = cfg.BindSyncedEntry(topSection, LguConstants.DEEPER_POCKETS_INITIAL_TWO_HANDED_AMOUNT_KEY, LguConstants.DEEPER_POCKETS_INITIAL_TWO_HANDED_AMOUNT_DEFAULT, LguConstants.DEEPER_POCKETS_INITIAL_TWO_HANDED_AMOUNT_DESCRIPTION);
            DEEPER_POCKETS_INCREMENTAL_TWO_HANDED_ITEMS = cfg.BindSyncedEntry(topSection, LguConstants.DEEPER_POCKETS_INCREMENTAL_TWO_HANDED_AMOUNT_KEY, LguConstants.DEEPER_POCKETS_INCREMENTAL_TWO_HANDED_AMOUNT_DEFAULT, LguConstants.DEEPER_POCKETS_INCREMENTAL_TWO_HANDED_AMOUNT_DESCRIPTION);
            DEEPER_POCKETS_ALLOW_WHEELBARROWS = cfg.BindSyncedEntry(topSection, LguConstants.DEEPER_POCKETS_ALLOW_WHEELBARROWS_KEY, LguConstants.DEEPER_POCKETS_ALLOW_WHEELBARROWS_DEFAULT, LguConstants.DEEPER_POCKETS_ALLOW_WHEELBARROWS_DESCRIPTION);
            DEEPER_POCKETS_ITEM_PROGRESSION_ITEMS = cfg.BindSyncedEntry(topSection, LguConstants.ITEM_PROGRESSION_ITEMS_KEY, LguConstants.ITEM_PROGRESSION_ITEMS_DEFAULT, LguConstants.ITEM_PROGRESSION_ITEMS_DESCRIPTION);

            #endregion

            #region Aluminium Coils

            topSection = AluminiumCoils.UPGRADE_NAME;
            ALUMINIUM_COILS_ENABLED = cfg.BindSyncedEntry(topSection, LguConstants.ALUMINIUM_COILS_ENABLED_KEY, LguConstants.ALUMINIUM_COILS_ENABLED_DEFAULT, LguConstants.ALUMINIUM_COILS_ENABLED_DESCRIPTION);
            ALUMINIUM_COILS_INDIVIDUAL = cfg.BindSyncedEntry(topSection, BaseUpgrade.INDIVIDUAL_SECTION, BaseUpgrade.INDIVIDUAL_DEFAULT, BaseUpgrade.INDIVIDUAL_DESCRIPTION);
            ALUMINIUM_COILS_PRICE = cfg.BindSyncedEntry(topSection, LguConstants.ALUMINIUM_COILS_PRICE_KEY, LguConstants.ALUMINIUM_COILS_PRICE_DEFAULT);
            ALUMINIUM_COILS_PRICES = cfg.BindSyncedEntry(topSection, BaseUpgrade.PRICES_SECTION, AluminiumCoils.DEFAULT_PRICES, BaseUpgrade.PRICES_DESCRIPTION);
            ALUMINIUM_COILS_INITIAL_DIFFICULTY_DECREASE = cfg.BindSyncedEntry(topSection, LguConstants.ALUMINIUM_COILS_INITIAL_DIFFICULTY_MULTIPLIER_KEY, LguConstants.ALUMINIUM_COILS_INITIAL_DIFFICULTY_MULTIPLIER_DEFAULT, LguConstants.ALUMINIUM_COILS_INITIAL_DIFFICULTY_MULTIPLIER_DESCRIPTION);
            ALUMINIUM_COILS_INCREMENTAL_DIFFICULTY_DECREASE = cfg.BindSyncedEntry(topSection, LguConstants.ALUMINIUM_COILS_INCREMENTAL_DIFFICULTY_MULTIPLIER_KEY, LguConstants.ALUMINIUM_COILS_INCREMENTAL_DIFFICULTY_MULTIPLIER_DEFAULT, LguConstants.ALUMINIUM_COILS_INCREMENTAL_DIFFICULTY_MULTIPLIER_DESCRIPTION);
            ALUMINIUM_COILS_INITIAL_STUN_TIMER_INCREASE = cfg.BindSyncedEntry(topSection, LguConstants.ALUMINIUM_COILS_INITIAL_STUN_TIMER_KEY, LguConstants.ALUMINIUM_COILS_INITIAL_STUN_TIMER_DEFAULT);
            ALUMINIUM_COILS_INCREMENTAL_STUN_TIMER_INCREASE = cfg.BindSyncedEntry(topSection, LguConstants.ALUMINIUM_COILS_INCREMENTAL_STUN_TIMER_KEY, LguConstants.ALUMINIUM_COILS_INCREMENTAL_STUN_TIMER_DEFAULT);
            ALUMINIUM_COILS_INITIAL_RANGE_INCREASE = cfg.BindSyncedEntry(topSection, LguConstants.ALUMINIUM_COILS_INITIAL_RANGE_KEY, LguConstants.ALUMINIUM_COILS_INITIAL_RANGE_DEFAULT);
            ALUMINIUM_COILS_INCREMENTAL_RANGE_INCREASE = cfg.BindSyncedEntry(topSection, LguConstants.ALUMINIUM_COILS_INCREMENTAL_RANGE_KEY, LguConstants.ALUMINIUM_COILS_INCREMENTAL_RANGE_DEFAULT);
            ALUMINIUM_COILS_INITIAL_COOLDOWN_DECREASE = cfg.BindSyncedEntry(topSection, LguConstants.ALUMINIUM_COILS_INITIAL_COOLDOWN_KEY, LguConstants.ALUMINIUM_COILS_INITIAL_COOLDOWN_DEFAULT, LguConstants.ALUMINIUM_COILS_INITIAL_COOLDOWN_DESCRIPTION);
            ALUMINIUM_COILS_INCREMENTAL_COOLDOWN_DECREASE = cfg.BindSyncedEntry(topSection, LguConstants.ALUMINIUM_COILS_INCREMENTAL_COOLDOWN_KEY, LguConstants.ALUMINIUM_COILS_INCREMENTAL_COOLDOWN_DEFAULT, LguConstants.ALUMINIUM_COILS_INCREMENTAL_COOLDOWN_DESCRIPTION);
            ALUMINIUM_COILS_ITEM_PROGRESSION_ITEMS = cfg.BindSyncedEntry(topSection, LguConstants.ITEM_PROGRESSION_ITEMS_KEY, LguConstants.ITEM_PROGRESSION_ITEMS_DEFAULT, LguConstants.ITEM_PROGRESSION_ITEMS_DESCRIPTION);

            #endregion

            #region Back Muscles

            topSection = BackMuscles.UPGRADE_NAME;
            BACK_MUSCLES_ENABLED = cfg.BindSyncedEntry(topSection, LguConstants.BACK_MUSCLES_ENABLED_KEY, LguConstants.BACK_MUSCLES_ENABLED_DEFAULT, LguConstants.BACK_MUSCLES_ENABLED_DESCRIPTION);
            BACK_MUSCLES_PRICE = cfg.BindSyncedEntry(topSection, LguConstants.BACK_MUSCLES_PRICE_KEY, LguConstants.BACK_MUSCLES_PRICE_DEFAULT);
            CARRY_WEIGHT_REDUCTION = cfg.BindSyncedEntry(topSection, LguConstants.BACK_MUSCLES_INITIAL_WEIGHT_MULTIPLIER_KEY, LguConstants.BACK_MUSCLES_INITIAL_WEIGHT_MULTIPLIER_DEFAULT, LguConstants.BACK_MUSCLES_INITIAL_WEIGHT_MULTIPLIER_DESCRIPTION);
            CARRY_WEIGHT_INCREMENT = cfg.BindSyncedEntry(topSection, LguConstants.BACK_MUSCLES_INCREMENTAL_WEIGHT_MULTIPLIER_KEY, LguConstants.BACK_MUSCLES_INCREMENTAL_WEIGHT_MULTIPLIER_DEFAULT, LguConstants.BACK_MUSCLES_INCREMENTAL_WEIGHT_MULTIPLIER_DESCRIPTION);
            BACK_MUSCLES_UPGRADE_PRICES = cfg.BindSyncedEntry(topSection, BaseUpgrade.PRICES_SECTION, BackMuscles.PRICES_DEFAULT, BaseUpgrade.PRICES_DESCRIPTION);
            BACK_MUSCLES_INDIVIDUAL = cfg.BindSyncedEntry(topSection, BaseUpgrade.INDIVIDUAL_SECTION, BaseUpgrade.INDIVIDUAL_DEFAULT, BaseUpgrade.INDIVIDUAL_DESCRIPTION);
            BACK_MUSCLES_ITEM_PROGRESSION_ITEMS = cfg.BindSyncedEntry(topSection, LguConstants.ITEM_PROGRESSION_ITEMS_KEY, LguConstants.ITEM_PROGRESSION_ITEMS_DEFAULT, LguConstants.ITEM_PROGRESSION_ITEMS_DESCRIPTION);
            BACK_MUSCLES_UPGRADE_MODE = cfg.BindSyncedEntry(topSection, LguConstants.BACK_MUSCLES_UPGRADE_MODE_KEY , LguConstants.BACK_MUSCLES_UPGRADE_MODE_DEFAULT, LguConstants.BACK_MUSCLES_UPGRADE_MODE_DESCRIPTION);

            #endregion

            #region Bargain Connections

            topSection = BargainConnections.UPGRADE_NAME;
            BARGAIN_CONNECTIONS_ENABLED = cfg.BindSyncedEntry(topSection, LguConstants.BARGAIN_CONNECTIONS_ENABLED_KEY, LguConstants.BARGAIN_CONNECTIONS_ENABLED_DEFAULT, LguConstants.BARGAIN_CONNECTIONS_ENABLED_DESCRIPTION);
            BARGAIN_CONNECTIONS_PRICE = cfg.BindSyncedEntry(topSection, LguConstants.BARGAIN_CONNECTIONS_PRICE_KEY, LguConstants.BARGAIN_CONNECTIONS_PRICE_DEFAULT);
            BARGAIN_CONNECTIONS_INITIAL_ITEM_AMOUNT = cfg.BindSyncedEntry(topSection, LguConstants.BARGAIN_CONNECTIONS_INITIAL_AMOUNT_KEY, LguConstants.BARGAIN_CONNECTIONS_INITIAL_AMOUNT_DEFAULT);
            BARGAIN_CONNECTIONS_INCREMENTAL_ITEM_AMOUNT = cfg.BindSyncedEntry(topSection, LguConstants.BARGAIN_CONNECTIONS_INCREMENTAL_AMOUNT_KEY, LguConstants.BARGAIN_CONNECTIONS_INCREMENTAL_AMOUNT_DEFAULT);
            BARGAIN_CONNECTIONS_PRICES = cfg.BindSyncedEntry(topSection, BaseUpgrade.PRICES_SECTION, BargainConnections.PRICES_DEFAULT, BaseUpgrade.PRICES_DESCRIPTION);
            BARGAIN_CONNECTIONS_ITEM_PROGRESSION_ITEMS = cfg.BindSyncedEntry(topSection, LguConstants.ITEM_PROGRESSION_ITEMS_KEY, LguConstants.ITEM_PROGRESSION_ITEMS_DEFAULT, LguConstants.ITEM_PROGRESSION_ITEMS_DESCRIPTION);

            #endregion

            #region Beekeeper

            topSection = Beekeeper.UPGRADE_NAME;
            BEEKEEPER_ENABLED = cfg.BindSyncedEntry(topSection, LguConstants.BEEKEEPER_ENABLED_KEY, LguConstants.BEEKEEPER_ENABLED_DEFAULT, LguConstants.BEEKEEPER_ENABLED_DESCRIPTION);
            BEEKEEPER_PRICE = cfg.BindSyncedEntry(topSection, LguConstants.BEEKEEPER_PRICE_KEY, LguConstants.BEEKEEPER_PRICE_DEFAULT);
            BEEKEEPER_UPGRADE_PRICES = cfg.BindSyncedEntry(topSection, BaseUpgrade.PRICES_SECTION, Beekeeper.PRICES_DEFAULT, BaseUpgrade.PRICES_DESCRIPTION);
            BEEKEEPER_INDIVIDUAL = cfg.BindSyncedEntry(topSection, BaseUpgrade.INDIVIDUAL_SECTION, BaseUpgrade.INDIVIDUAL_DEFAULT, BaseUpgrade.INDIVIDUAL_DESCRIPTION);
            BEEKEEPER_DAMAGE_MULTIPLIER = cfg.BindSyncedEntry(topSection, LguConstants.BEEKEEPER_DAMAGE_MULTIPLIER_KEY, LguConstants.BEEKEEPER_DAMAGE_MULTIPLIER_DEFAULT, LguConstants.BEEKEEPER_DAMAGE_MULTIPLIER_DESCRIPTION);
            BEEKEEPER_DAMAGE_MULTIPLIER_INCREMENT = cfg.BindSyncedEntry(topSection, LguConstants.BEEKEEPER_DAMAGE_INCREMENTAL_MULTIPLIER_KEY, LguConstants.BEEKEEPER_DAMAGE_INCREMENTAL_MULTIPLIER_DEFAULT, LguConstants.BEEKEEPER_DAMAGE_INCREMENTAL_MULTIPLIER_DESCRIPTION);
            BEEKEEPER_HIVE_VALUE_INCREASE = cfg.BindSyncedEntry(topSection, LguConstants.BEEKEEPER_HIVE_MULTIPLIER_KEY, LguConstants.BEEKEEPER_HIVE_MULTIPLIER_DEFAULT, LguConstants.BEEKEEPER_HIVE_MULTIPLIER_DESCRIPTION);
            BEEKEEPER_ITEM_PROGRESSION_ITEMS = cfg.BindSyncedEntry(topSection, LguConstants.ITEM_PROGRESSION_ITEMS_KEY, LguConstants.ITEM_PROGRESSION_ITEMS_DEFAULT, LguConstants.ITEM_PROGRESSION_ITEMS_DESCRIPTION);

            #endregion

            #region Better Scanner

            topSection = BetterScanner.UPGRADE_NAME;
            BETTER_SCANNER_ENABLED = cfg.BindSyncedEntry(topSection, LguConstants.BETTER_SCANNER_ENABLED_KEY, LguConstants.BETTER_SCANNER_ENABLED_DEFAULT, LguConstants.BETTER_SCANNER_ENABLED_DESCRIPTION);
            BETTER_SCANNER_PRICE = cfg.BindSyncedEntry(topSection, LguConstants.BETTER_SCANNER_PRICE_KEY, LguConstants.BETTER_SCANNER_PRICE_DEFAULT);
            SHIP_AND_ENTRANCE_DISTANCE_INCREASE = cfg.BindSyncedEntry(topSection, LguConstants.BETTER_SCANNER_OUTSIDE_NODE_DISTANCE_INCREASE_KEY, LguConstants.BETTER_SCANNER_OUTSIDE_NODE_DISTANCE_INCREASE_DEFAULT, LguConstants.BETTER_SCANNER_OUTSIDE_NODE_DISTANCE_INCREASE_DESCRIPTION);
            NODE_DISTANCE_INCREASE = cfg.BindSyncedEntry(topSection, LguConstants.BETTER_SCANNER_NODE_DISTANCE_INCREASE_KEY, LguConstants.BETTER_SCANNER_NODE_DISTANCE_INCREASE_DEFAULT, LguConstants.BETTER_SCANNER_NODE_DISTANCE_INCREASE_DESCRIPTION);
            BETTER_SCANNER_INDIVIDUAL = cfg.BindSyncedEntry(topSection, BaseUpgrade.INDIVIDUAL_SECTION, BaseUpgrade.INDIVIDUAL_DEFAULT, BaseUpgrade.INDIVIDUAL_DESCRIPTION);
            BETTER_SCANNER_PRICE2 = cfg.BindSyncedEntry(topSection, LguConstants.BETTER_SCANNER_SECOND_TIER_PRICE_KEY, LguConstants.BETTER_SCANNER_SECOND_TIER_PRICE_DEFAULT, LguConstants.BETTER_SCANNER_SECOND_TIER_PRICE_DESCRIPTION);
            BETTER_SCANNER_PRICE3 = cfg.BindSyncedEntry(topSection, LguConstants.BETTER_SCANNER_THIRD_TIER_PRICE_KEY, LguConstants.BETTER_SCANNER_THIRD_TIER_PRICE_DEFAULT, LguConstants.BETTER_SCANNER_THIRD_TIER_PRICE_DESCRIPTION);
            BETTER_SCANNER_ENEMIES = cfg.BindSyncedEntry(topSection, LguConstants.BETTER_SCANNER_ENEMIES_THROUGH_WALLS_KEY, LguConstants.BETTER_SCANNER_ENEMIES_THROUGH_WALLS_DEFAULT, LguConstants.BETTER_SCANNER_ENEMIES_THROUGH_WALLS_DESCRIPTION);
            VERBOSE_ENEMIES = cfg.BindSyncedEntry(topSection, LguConstants.BETTER_SCANNER_VERBOSE_ENEMIES_KEY, LguConstants.BETTER_SCANNER_VERBOSE_ENEMIES_DEFAULT, LguConstants.BETTER_SCANNER_VERBOSE_ENEMIES_DESCRIPTION);
            BETTER_SCANNER_ITEM_PROGRESSION_ITEMS = cfg.BindSyncedEntry(topSection, LguConstants.ITEM_PROGRESSION_ITEMS_KEY, LguConstants.ITEM_PROGRESSION_ITEMS_DEFAULT, LguConstants.ITEM_PROGRESSION_ITEMS_DESCRIPTION);

            #endregion

            #region Bigger Lungs

            topSection = BiggerLungs.UPGRADE_NAME;
            BIGGER_LUNGS_ENABLED = cfg.BindSyncedEntry(topSection, LguConstants.BIGGER_LUNGS_ENABLED_KEY, LguConstants.BIGGER_LUNGS_ENABLED_DEFAULT, LguConstants.BIGGER_LUNGS_ENABLED_DESCRIPTION);
            BIGGER_LUNGS_PRICE = cfg.BindSyncedEntry(topSection, LguConstants.BIGGER_LUNGS_PRICE_KEY, LguConstants.BIGGER_LUNGS_PRICE_DEFAULT);
            SPRINT_TIME_INCREASE_UNLOCK = cfg.BindSyncedEntry(topSection, LguConstants.BIGGER_LUNGS_INITIAL_SPRINT_TIME_KEY, LguConstants.BIGGER_LUNGS_INITIAL_SPRINT_TIME_DEFAULT, LguConstants.BIGGER_LUNGS_INITIAL_SPRINT_TIME_DESCRIPTION);
            SPRINT_TIME_INCREMENT = cfg.BindSyncedEntry(topSection, LguConstants.BIGGER_LUNGS_INCREMENTAL_SPRINT_TIME_KEY, LguConstants.BIGGER_LUNGS_INCREMENTAL_SPRINT_TIME_DEFAULT, LguConstants.BIGGER_LUNGS_INCREMENTAL_SPRINT_TIME_DESCRIPTION);
            BIGGER_LUNGS_UPGRADE_PRICES = cfg.BindSyncedEntry(topSection, BaseUpgrade.PRICES_SECTION, BiggerLungs.PRICES_DEFAULT, BaseUpgrade.PRICES_DESCRIPTION);
            BIGGER_LUNGS_INDIVIDUAL = cfg.BindSyncedEntry(topSection, BaseUpgrade.INDIVIDUAL_SECTION, BaseUpgrade.INDIVIDUAL_DEFAULT, BaseUpgrade.INDIVIDUAL_DESCRIPTION);
            BIGGER_LUNGS_STAMINA_REGEN_APPLY_LEVEL = cfg.BindSyncedEntry(topSection, LguConstants.BIGGER_LUNGS_STAMINA_APPLY_LEVEL_KEY, LguConstants.BIGGER_LUNGS_STAMINA_APPLY_LEVEL_DEFAULT, LguConstants.BIGGER_LUNGS_STAMINA_APPLY_LEVEL_DESCRIPTION);
            BIGGER_LUNGS_STAMINA_REGEN_INCREASE = cfg.BindSyncedEntry(topSection, LguConstants.BIGGER_LUNGS_INITIAL_STAMINA_REGEN_KEY, LguConstants.BIGGER_LUNGS_INITIAL_STAMINA_REGEN_DEFAULT, LguConstants.BIGGER_LUNGS_INITIAL_STAMINA_REGEN_DESCRIPTION);
            BIGGER_LUNGS_STAMINA_REGEN_INCREMENTAL_INCREASE = cfg.BindSyncedEntry(topSection, LguConstants.BIGGER_LUNGS_INCREMENTAL_STAMINA_REGEN_KEY, LguConstants.BIGGER_LUNGS_INCREMENTAL_STAMINA_REGEN_DEFAULT, LguConstants.BIGGER_LUNGS_INCREMENTAL_STAMINA_REGEN_DESCRIPTION);
            BIGGER_LUNGS_JUMP_STAMINA_APPLY_LEVEL = cfg.BindSyncedEntry(topSection, LguConstants.BIGGER_LUNGS_JUMP_STAMINA_DECREASE_APPLY_LEVEL_KEY, LguConstants.BIGGER_LUNGS_JUMP_STAMINA_DECREASE_APPLY_LEVEL_DEFAULT, LguConstants.BIGGER_LUNGS_JUMP_STAMINA_DECREASE_APPLY_LEVEL_DESCRIPTION);
            BIGGER_LUNGS_JUMP_STAMINA_COST_DECREASE = cfg.BindSyncedEntry(topSection, LguConstants.BIGGER_LUNGS_INITIAL_JUMP_STAMINA_DECREASE_KEY, LguConstants.BIGGER_LUNGS_INITIAL_JUMP_STAMINA_DECREASE_DEFAULT, LguConstants.BIGGER_LUNGS_INITIAL_JUMP_STAMINA_DECREASE_DESCRIPTION);
            BIGGER_LUNGS_JUMP_STAMINA_COST_INCREMENTAL_DECREASE = cfg.BindSyncedEntry(topSection, LguConstants.BIGGER_LUNGS_INCREMENTAL_JUMP_STAMINA_DECREASE_KEY, LguConstants.BIGGER_LUNGS_INCREMENTAL_JUMP_STAMINA_DECREASE_DEFAULT, LguConstants.BIGGER_LUNGS_INCREMENTAL_JUMP_STAMINA_DECREASE_DESCRIPTION);
            BIGGER_LUNGS_ITEM_PROGRESSION_ITEMS = cfg.BindSyncedEntry(topSection, LguConstants.ITEM_PROGRESSION_ITEMS_KEY, LguConstants.ITEM_PROGRESSION_ITEMS_DEFAULT, LguConstants.ITEM_PROGRESSION_ITEMS_DESCRIPTION);

            #endregion

            #region Charging Booster

            topSection = ChargingBooster.UPGRADE_NAME;
            CHARGING_BOOSTER_ENABLED = cfg.BindSyncedEntry(topSection, LguConstants.CHARGING_BOOSTER_ENABLED_KEY, LguConstants.CHARGING_BOOSTER_ENABLED_DEFAULT, LguConstants.CHARGING_BOOSTER_ENABLED_DESCRIPTION);
            CHARGING_BOOSTER_PRICE = cfg.BindSyncedEntry(topSection, LguConstants.CHARGING_BOOSTER_PRICE_KEY, LguConstants.CHARGING_BOOSTER_PRICE_DEFAULT);
            CHARGING_BOOSTER_PRICES = cfg.BindSyncedEntry(topSection, BaseUpgrade.PRICES_SECTION, LguConstants.CHARGING_BOOSTER_PRICES_DEFAULT, BaseUpgrade.PRICES_DESCRIPTION);
            CHARGING_BOOSTER_COOLDOWN = cfg.BindSyncedEntry(topSection, LguConstants.CHARGING_BOOSTER_COOLDOWN_KEY, LguConstants.CHARGING_BOOSTER_COOLDOWN_DEFAULT);
            CHARGING_BOOSTER_INCREMENTAL_COOLDOWN_DECREASE = cfg.BindSyncedEntry(topSection, LguConstants.CHARGING_BOOSTER_INCREMENTAL_COOLDOWN_DECREASE_KEY, LguConstants.CHARGING_BOOSTER_INCREMENTAL_COOLDOWN_DECREASE_DEFAULT);
            CHARGING_BOOSTER_CHARGE_PERCENTAGE = cfg.BindSyncedEntry(topSection, LguConstants.CHARGING_BOOSTER_CHARGE_PERCENTAGE_KEY, LguConstants.CHARGING_BOOSTER_CHARGE_PERCENTAGE_DEFAULT);
            CHARGING_BOOSTER_ITEM_PROGRESSION_ITEMS = cfg.BindSyncedEntry(topSection, LguConstants.ITEM_PROGRESSION_ITEMS_KEY, LguConstants.ITEM_PROGRESSION_ITEMS_DEFAULT, LguConstants.ITEM_PROGRESSION_ITEMS_DESCRIPTION);

            #endregion

            #region Climbing Gloves

            topSection = ClimbingGloves.UPGRADE_NAME;
            CLIMBING_GLOVES_ENABLED             = cfg.BindSyncedEntry(topSection, LguConstants.CLIMBING_GLOVES_ENABLED_KEY, LguConstants.CLIMBING_GLOVES_ENABLED_DEFAULT, LguConstants.CLIMBING_GLOVES_ENABLED_DESCRIPTION);
            CLIMBING_GLOVES_INDIVIDUAL          = cfg.BindSyncedEntry(topSection, BaseUpgrade.INDIVIDUAL_SECTION, BaseUpgrade.INDIVIDUAL_DEFAULT, BaseUpgrade.INDIVIDUAL_DESCRIPTION);
            CLIMBING_GLOVES_PRICE               = cfg.BindSyncedEntry(topSection, LguConstants.CLIMBING_GLOVES_PRICE_KEY, LguConstants.CLIMBING_GLOVES_PRICE_DEFAULT);
            CLIMBING_GLOVES_PRICES              = cfg.BindSyncedEntry(topSection, BaseUpgrade.PRICES_SECTION, ClimbingGloves.DEFAULT_PRICES, BaseUpgrade.PRICES_DESCRIPTION);
            INITIAL_CLIMBING_SPEED_BOOST        = cfg.BindSyncedEntry(topSection, LguConstants.CLIMBING_GLOVES_INITIAL_MULTIPLIER_KEY, LguConstants.CLIMBING_GLOVES_INITIAL_MULTIPLIER_DEFAULT, LguConstants.CLIMBING_GLOVES_INITIAL_MULTIPLIER_DESCRIPTION);
            INCREMENTAL_CLIMBING_SPEED_BOOST    = cfg.BindSyncedEntry(topSection, LguConstants.CLIMBING_GLOVES_INCREMENTAL_MULTIPLIER_KEY, LguConstants.CLIMBING_GLOVES_INCREMENTAL_MULTIPLIER_DEFAULT);
            CLIMBING_GLOVES_ITEM_PROGRESSION_ITEMS = cfg.BindSyncedEntry(topSection, LguConstants.ITEM_PROGRESSION_ITEMS_KEY, LguConstants.ITEM_PROGRESSION_ITEMS_DEFAULT, LguConstants.ITEM_PROGRESSION_ITEMS_DESCRIPTION);

            #endregion

            #region Discombobulator

            topSection = Discombobulator.UPGRADE_NAME;
            DISCOMBOBULATOR_ENABLED = cfg.BindSyncedEntry(topSection, LguConstants.DISCOMBOBULATOR_ENABLED_KEY, LguConstants.DISCOMBOBULATOR_ENABLED_DEFAULT, LguConstants.DISCOMBOBULATOR_ENABLED_DESCRIPTION);
            DISCOMBOBULATOR_PRICE = cfg.BindSyncedEntry(topSection, LguConstants.DISCOMBOBULATOR_PRICE_KEY, LguConstants.DISCOMBOBULATOR_PRICE_DEFAULT);
            DISCOMBOBULATOR_COOLDOWN = cfg.BindSyncedEntry(topSection, LguConstants.DISCOMBOBULATOR_COOLDOWN_KEY, LguConstants.DISCOMBOBULATOR_COOLDOWN_DEFAULT);
            DISCOMBOBULATOR_RADIUS = cfg.BindSyncedEntry(topSection, LguConstants.DISCOMBOBULATOR_EFFECT_RADIUS_KEY, LguConstants.DISCOMBOBULATOR_EFFECT_RADIUS_DEFAULT);
            DISCOMBOBULATOR_STUN_DURATION = cfg.BindSyncedEntry(topSection, LguConstants.DISCOMBOBULATOR_STUN_DURATION_KEY, LguConstants.DISCOMBOBULATOR_STUN_DURATION_DEFAULT);
            DISCOMBOBULATOR_NOTIFY_CHAT = cfg.BindSyncedEntry(topSection, LguConstants.DISCOMBOBULATOR_NOTIFY_CHAT_KEY, LguConstants.DISCOMBOBULATOR_NOTIFY_CHAT_DEFAULT);
            DISCOMBOBULATOR_INCREMENT = cfg.BindSyncedEntry(topSection, LguConstants.DISCOMBOBULATOR_INCREMENTAL_STUN_TIME_KEY, LguConstants.DISCOMBOBULATOR_INCREMENTAL_STUN_TIME_DEFAULT, LguConstants.DISCOMBOBULATOR_INCREMENTAL_STUN_TIME_DESCRIPTION);
            DISCO_UPGRADE_PRICES = cfg.BindSyncedEntry(topSection, BaseUpgrade.PRICES_SECTION, Discombobulator.PRICES_DEFAULT, BaseUpgrade.PRICES_DESCRIPTION);
            DISCOMBOBULATOR_INDIVIDUAL = cfg.BindSyncedEntry(topSection, BaseUpgrade.INDIVIDUAL_SECTION, BaseUpgrade.INDIVIDUAL_DEFAULT, BaseUpgrade.INDIVIDUAL_DESCRIPTION);
            DISCOMBOBULATOR_DAMAGE_LEVEL = cfg.BindSyncedEntry(topSection, LguConstants.DISCOMBOBULATOR_APPLY_DAMAGE_LEVEL_KEY, LguConstants.DISCOMBOBULATOR_APPLY_DAMAGE_LEVEL_DEFAULT, LguConstants.DISCOMBOBULATOR_APPLY_DAMAGE_LEVEL_DESCRIPTION);
            DISCOMBOBULATOR_INITIAL_DAMAGE = cfg.BindSyncedEntry(topSection, LguConstants.DISCOMBOBULATOR_INITIAL_DAMAGE_KEY, LguConstants.DISCOMBOBULATOR_INITIAL_DAMAGE_DEFAULT, LguConstants.DISCOMBOBULATOR_INITIAL_DAMAGE_DESCRIPTION);
            DISCOMBOBULATOR_DAMAGE_INCREASE = cfg.BindSyncedEntry(topSection, LguConstants.DISCOMBOBULATOR_INCREMENTAL_DAMAGE_KEY, LguConstants.DISCOMBOBULATOR_INCREMENTAL_DAMAGE_DEFAULT, LguConstants.DISCOMBOBULATOR_INCREMENTAL_DAMAGE_DESCRIPTION);
            DISCOMBOBULATOR_ITEM_PROGRESSION_ITEMS = cfg.BindSyncedEntry(topSection, LguConstants.ITEM_PROGRESSION_ITEMS_KEY, LguConstants.ITEM_PROGRESSION_ITEMS_DEFAULT, LguConstants.ITEM_PROGRESSION_ITEMS_DESCRIPTION);

            #endregion

            #region Drop Pod Thrusters

            topSection = FasterDropPod.UPGRADE_NAME;
            FASTER_DROP_POD_ENABLED = cfg.BindSyncedEntry(topSection, LguConstants.DROP_POD_THRUSTERS_ENABLED_KEY, LguConstants.DROP_POD_THRUSTERS_ENABLED_DEFAULT, LguConstants.DROP_POD_THRUSTERS_ENABLED_DESCRIPTION);
            FASTER_DROP_POD_PRICE = cfg.BindSyncedEntry(topSection, LguConstants.DROP_POD_THRUSTERS_PRICE_KEY, LguConstants.DROP_POD_THRUSTERS_PRICE_DEFAULT, LguConstants.DROP_POD_THRUSTERS_PRICE_DESCRIPTION);
            FASTER_DROP_POD_TIMER = cfg.BindSyncedEntry(topSection, LguConstants.DROP_POD_THRUSTERS_TIME_DECREASE_KEY, LguConstants.DROP_POD_THRUSTERS_TIME_DECREASE_DEFAULT);
            FASTER_DROP_POD_INITIAL_TIMER = cfg.BindSyncedEntry(topSection, LguConstants.DROP_POD_THRUSTERS_FIRST_TIME_DECREASE_KEY, LguConstants.DROP_POD_THRUSTERS_FIRST_TIME_DECREASE_DEFAULT);
            FASTER_DROP_POD_LEAVE_TIMER = cfg.BindSyncedEntry(topSection, LguConstants.DROP_POD_THRUSTERS_LEAVE_TIMER_KEY, LguConstants.DROP_POD_THRUSTERS_LEAVE_TIMER_DEFAULT, LguConstants.DROP_POD_THRUSTERS_LEAVE_TIMER_DESCRIPTION);
            DROP_POD_THRUSTERS_ITEM_PROGRESSION_ITEMS = cfg.BindSyncedEntry(topSection, LguConstants.ITEM_PROGRESSION_ITEMS_KEY, LguConstants.ITEM_PROGRESSION_ITEMS_DEFAULT, LguConstants.ITEM_PROGRESSION_ITEMS_DESCRIPTION);

            #endregion

            #region Efficient Engines

            topSection = EfficientEngines.UPGRADE_NAME;
            EFFICIENT_ENGINES_ENABLED = cfg.BindSyncedEntry(topSection, LguConstants.EFFICIENT_ENGINES_ENABLED_KEY, LguConstants.EFFICIENT_ENGINES_ENABLED_DEFAULT, LguConstants.EFFICIENT_ENGINES_ENABLED_DESCRIPTION);
            EFFICIENT_ENGINES_PRICE = cfg.BindSyncedEntry(topSection, LguConstants.EFFICIENT_ENGINES_PRICE_KEY, LguConstants.EFFICIENT_ENGINES_PRICE_DEFAULT);
            EFFICIENT_ENGINES_PRICES = cfg.BindSyncedEntry(topSection, BaseUpgrade.PRICES_SECTION, EfficientEngines.DEFAULT_PRICES, BaseUpgrade.PRICES_DESCRIPTION);
            EFFICIENT_ENGINES_INITIAL_DISCOUNT = cfg.BindSyncedEntry(topSection, LguConstants.EFFICIENT_ENGINES_INITIAL_MULTIPLIER_KEY, LguConstants.EFFICIENT_ENGINES_INITIAL_MULTIPLIER_DEFAULT);
            EFFICIENT_ENGINES_INCREMENTAL_DISCOUNT = cfg.BindSyncedEntry(topSection, LguConstants.EFFICIENT_ENGINES_INCREMENTAL_MULTIPLIER_KEY, LguConstants.EFFICIENT_ENGINES_INCREMENTAL_MULTIPLIER_DEFAULT);
            EFFICIENT_ENGINES_ITEM_PROGRESSION_ITEMS = cfg.BindSyncedEntry(topSection, LguConstants.ITEM_PROGRESSION_ITEMS_KEY, LguConstants.ITEM_PROGRESSION_ITEMS_DEFAULT, LguConstants.ITEM_PROGRESSION_ITEMS_DESCRIPTION);

            #endregion

            #region Fast Encryption

            topSection = FastEncryption.UPGRADE_NAME;
            PAGER_ENABLED = cfg.BindSyncedEntry(topSection, LguConstants.FAST_ENCRYPTION_ENABLED_KEY, LguConstants.FAST_ENCRYPTION_ENABLED_DEFAULT, LguConstants.FAST_ENCRYPTION_ENABLED_DESCRIPTION);
            PAGER_PRICE = cfg.BindSyncedEntry(topSection, LguConstants.FAST_ENCRYPTION_PRICE_KEY, LguConstants.FAST_ENCRYPTION_PRICE_DEFAULT);
            FAST_ENCRYPTION_ITEM_PROGRESSION_ITEMS = cfg.BindSyncedEntry(topSection, LguConstants.ITEM_PROGRESSION_ITEMS_KEY, LguConstants.ITEM_PROGRESSION_ITEMS_DEFAULT, LguConstants.ITEM_PROGRESSION_ITEMS_DESCRIPTION);

            #endregion

            #region Hunter

            topSection = Hunter.UPGRADE_NAME;
            HUNTER_ENABLED = cfg.BindSyncedEntry(topSection, LguConstants.HUNTER_ENABLED_KEY, LguConstants.HUNTER_ENABLED_DEFAULT, LguConstants.HUNTER_ENABLED_DESCRIPTION);
            HUNTER_PRICE = cfg.BindSyncedEntry(topSection, LguConstants.HUNTER_PRICE_KEY, LguConstants.HUNTER_PRICE_DEFAULT);
            HUNTER_UPGRADE_PRICES = cfg.BindSyncedEntry(topSection, BaseUpgrade.PRICES_SECTION, Hunter.PRICES_DEFAULT, BaseUpgrade.PRICES_DESCRIPTION);
            HUNTER_SAMPLE_TIERS = cfg.BindSyncedEntry(topSection,
                                            LguConstants.HUNTER_SAMPLE_TIERS_KEY,
                                            LguConstants.HUNTER_SAMPLE_TIERS_DEFAULT,
                                            LguConstants.HUNTER_SAMPLE_TIERS_DESCRIPTION);
            SNARE_FLEA_SAMPLE_MINIMUM_VALUE = cfg.BindSyncedEntry(topSection, LguConstants.SNARE_FLEA_SAMPLE_MINIMUM_VALUE_KEY, LguConstants.SNARE_FLEA_SAMPLE_MINIMUM_VALUE_DEFAULT);
            SNARE_FLEA_SAMPLE_MAXIMUM_VALUE = cfg.BindSyncedEntry(topSection, LguConstants.SNARE_FLEA_SAMPLE_MAXIMUM_VALUE_KEY, LguConstants.SNARE_FLEA_SAMPLE_MAXIMUM_VALUE_DEFAULT);
            BUNKER_SPIDER_SAMPLE_MINIMUM_VALUE = cfg.BindSyncedEntry(topSection, LguConstants.BUNKER_SPIDER_SAMPLE_MINIMUM_VALUE_KEY, LguConstants.BUNKER_SPIDER_SAMPLE_MINIMUM_VALUE_DEFAULT);
            BUNKER_SPIDER_SAMPLE_MAXIMUM_VALUE = cfg.BindSyncedEntry(topSection, LguConstants.BUNKER_SPIDER_SAMPLE_MAXIMUM_VALUE_KEY, LguConstants.BUNKER_SPIDER_SAMPLE_MAXIMUM_VALUE_DEFAULT);
            HOARDING_BUG_SAMPLE_MINIMUM_VALUE = cfg.BindSyncedEntry(topSection, LguConstants.HOARDING_BUG_SAMPLE_MINIMUM_VALUE_KEY, LguConstants.HOARDING_BUG_SAMPLE_MINIMUM_VALUE_DEFAULT);
            HOARDING_BUG_SAMPLE_MAXIMUM_VALUE = cfg.BindSyncedEntry(topSection, LguConstants.HOARDING_BUG_SAMPLE_MAXIMUM_VALUE_KEY, LguConstants.HOARDING_BUG_SAMPLE_MAXIMUM_VALUE_DEFAULT);
            BRACKEN_SAMPLE_MINIMUM_VALUE = cfg.BindSyncedEntry(topSection, LguConstants.BRACKEN_SAMPLE_MINIMUM_VALUE_KEY, LguConstants.BRACKEN_SAMPLE_MINIMUM_VALUE_DEFAULT);
            BRACKEN_SAMPLE_MAXIMUM_VALUE = cfg.BindSyncedEntry(topSection, LguConstants.BRACKEN_SAMPLE_MAXIMUM_VALUE_KEY, LguConstants.BRACKEN_SAMPLE_MAXIMUM_VALUE_DEFAULT);
            EYELESS_DOG_SAMPLE_MINIMUM_VALUE = cfg.BindSyncedEntry(topSection, LguConstants.EYELESS_DOG_SAMPLE_MINIMUM_VALUE_KEY, LguConstants.EYELESS_DOG_SAMPLE_MINIMUM_VALUE_DEFAULT);
            EYELESS_DOG_SAMPLE_MAXIMUM_VALUE = cfg.BindSyncedEntry(topSection, LguConstants.EYELESS_DOG_SAMPLE_MAXIMUM_VALUE_KEY, LguConstants.EYELESS_DOG_SAMPLE_MAXIMUM_VALUE_DEFAULT);
            BABOON_HAWK_SAMPLE_MINIMUM_VALUE = cfg.BindSyncedEntry(topSection, LguConstants.BABOON_HAWK_SAMPLE_MINIMUM_VALUE_KEY, LguConstants.BABOON_HAWK_SAMPLE_MINIMUM_VALUE_DEFAULT);
            BABOON_HAWK_SAMPLE_MAXIMUM_VALUE = cfg.BindSyncedEntry(topSection, LguConstants.BABOON_HAWK_SAMPLE_MAXIMUM_VALUE_KEY, LguConstants.BABOON_HAWK_SAMPLE_MAXIMUM_VALUE_DEFAULT);
            THUMPER_SAMPLE_MINIMUM_VALUE = cfg.BindSyncedEntry(topSection, LguConstants.THUMPER_SAMPLE_MINIMUM_VALUE_KEY, LguConstants.THUMPER_SAMPLE_MINIMUM_VALUE_DEFAULT);
            THUMPER_SAMPLE_MAXIMUM_VALUE = cfg.BindSyncedEntry(topSection, LguConstants.THUMPER_SAMPLE_MAXIMUM_VALUE_KEY, LguConstants.THUMPER_SAMPLE_MAXIMUM_VALUE_DEFAULT);
            FOREST_KEEPER_SAMPLE_MINIMUM_VALUE = cfg.BindSyncedEntry(topSection, LguConstants.FOREST_KEEPER_SAMPLE_MINIMUM_VALUE_KEY, LguConstants.FOREST_KEEPER_SAMPLE_MINIMUM_VALUE_DEFAULT);
            FOREST_KEEPER_SAMPLE_MAXIMUM_VALUE = cfg.BindSyncedEntry(topSection, LguConstants.FOREST_KEEPER_SAMPLE_MAXIMUM_VALUE_KEY, LguConstants.FOREST_KEEPER_SAMPLE_MAXIMUM_VALUE_DEFAULT);
            MANTICOIL_SAMPLE_MINIMUM_VALUE = cfg.BindSyncedEntry(topSection, LguConstants.MANTICOIL_SAMPLE_MINIMUM_VALUE_KEY, LguConstants.MANTICOIL_SAMPLE_MINIMUM_VALUE_DEFAULT);
            MANTICOIL_SAMPLE_MAXIMUM_VALUE = cfg.BindSyncedEntry(topSection, LguConstants.MANTICOIL_SAMPLE_MAXIMUM_VALUE_KEY, LguConstants.MANTICOIL_SAMPLE_MAXIMUM_VALUE_DEFAULT);
            TULIP_SNAKE_SAMPLE_MINIMUM_VALUE = cfg.BindSyncedEntry(topSection, LguConstants.TULIP_SNAKE_SAMPLE_MINIMUM_VALUE_KEY, LguConstants.TULIP_SNAKE_SAMPLE_MINIMUM_VALUE_DEFAULT);
            TULIP_SNAKE_SAMPLE_MAXIMUM_VALUE = cfg.BindSyncedEntry(topSection, LguConstants.TULIP_SNAKE_SAMPLE_MAXIMUM_VALUE_KEY, LguConstants.TULIP_SNAKE_SAMPLE_MAXIMUM_VALUE_DEFAULT);
            KIDNAPPER_FOX_SAMPLE_MINIMUM_VALUE = cfg.BindSyncedEntry(topSection, LguConstants.KIDNAPPER_FOX_SAMPLE_MINIMUM_VALUE_KEY, LguConstants.KIDNAPPER_FOX_SAMPLE_MINIMUM_VALUE_DEFAULT);
            KIDNAPPER_FOX_SAMPLE_MAXIMUM_VALUE = cfg.BindSyncedEntry(topSection, LguConstants.KIDNAPPER_FOX_SAMPLE_MAXIMUM_VALUE_KEY, LguConstants.KIDNAPPER_FOX_SAMPLE_MAXIMUM_VALUE_DEFAULT);
            SPORE_LIZARD_SAMPLE_MINIMUM_VALUE = cfg.BindSyncedEntry(topSection, LguConstants.SPORE_LIZARD_SAMPLE_MINIMUM_VALUE_KEY, LguConstants.SPORE_LIZARD_SAMPLE_MINIMUM_VALUE_DEFAULT);
            SPORE_LIZARD_SAMPLE_MAXIMUM_VALUE = cfg.BindSyncedEntry(topSection, LguConstants.SPORE_LIZARD_SAMPLE_MAXIMUM_VALUE_KEY, LguConstants.SPORE_LIZARD_SAMPLE_MAXIMUM_VALUE_DEFAULT);
            MANEATER_SAMPLE_MINIMUM_VALUE = cfg.BindSyncedEntry(topSection, LguConstants.MANEATER_SAMPLE_MINIMUM_VALUE_KEY, LguConstants.MANEATER_SAMPLE_MINIMUM_VALUE_DEFAULT);
            MANEATER_SAMPLE_MAXIMUM_VALUE = cfg.BindSyncedEntry(topSection, LguConstants.MANEATER_SAMPLE_MAXIMUM_VALUE_KEY, LguConstants.MANEATER_SAMPLE_MAXIMUM_VALUE_DEFAULT);
            HUNTER_ITEM_PROGRESSION_ITEMS = cfg.BindSyncedEntry(topSection, LguConstants.ITEM_PROGRESSION_ITEMS_KEY, LguConstants.ITEM_PROGRESSION_ITEMS_DEFAULT, LguConstants.ITEM_PROGRESSION_ITEMS_DESCRIPTION);

            #endregion

            #region Lethal Deals

            topSection = LethalDeals.UPGRADE_NAME;
            LETHAL_DEALS_ENABLED = cfg.BindSyncedEntry(topSection, LguConstants.LETHAL_DEALS_ENABLED_KEY, LguConstants.LETHAL_DEALS_ENABLED_DEFAULT, LguConstants.LETHAL_DEALS_ENABLED_DESCRIPTION);
            LETHAL_DEALS_PRICE = cfg.BindSyncedEntry(topSection, LguConstants.LETHAL_DEALS_PRICE_KEY, LguConstants.LETHAL_DEALS_PRICE_DEFAULT);
            LETHAL_DEALS_ITEM_PROGRESSION_ITEMS = cfg.BindSyncedEntry(topSection, LguConstants.ITEM_PROGRESSION_ITEMS_KEY, LguConstants.ITEM_PROGRESSION_ITEMS_DEFAULT, LguConstants.ITEM_PROGRESSION_ITEMS_DESCRIPTION);

            #endregion

            #region Lightning Rod

            topSection = LightningRod.UPGRADE_NAME;
            LIGHTNING_ROD_ENABLED = cfg.BindSyncedEntry(topSection, LightningRod.ENABLED_SECTION, LightningRod.ENABLED_DEFAULT, LightningRod.ENABLED_DESCRIPTION);
            LIGHTNING_ROD_PRICE = cfg.BindSyncedEntry(topSection, LightningRod.PRICE_SECTION, LightningRod.PRICE_DEFAULT);
            LIGHTNING_ROD_ACTIVE = cfg.BindSyncedEntry(topSection, LightningRod.ACTIVE_SECTION, LightningRod.ACTIVE_DEFAULT, LightningRod.ACTIVE_DESCRIPTION);
            LIGHTNING_ROD_DIST = cfg.BindSyncedEntry(topSection, LightningRod.DIST_SECTION, LightningRod.DIST_DEFAULT, LightningRod.DIST_DESCRIPTION);
            LIGHTNING_ROD_UPGRADE_MODE = cfg.BindSyncedEntry(topSection, LightningRod.UPGRADE_MODE_SECTION, LightningRod.UPGRADE_MODE_DEFAULT, LightningRod.UPGRADE_MODE_DESCRIPTION);
            LIGHTNING_ROD_ITEM_PROGRESSION_ITEMS = cfg.BindSyncedEntry(topSection, LguConstants.ITEM_PROGRESSION_ITEMS_KEY, LguConstants.ITEM_PROGRESSION_ITEMS_DEFAULT, LguConstants.ITEM_PROGRESSION_ITEMS_DESCRIPTION);

            #endregion

            #region Lithium Batteries

            topSection = LithiumBatteries.UPGRADE_NAME;
            LITHIUM_BATTERIES_ENABLED = cfg.BindSyncedEntry(topSection, LguConstants.LITHIUM_BATTERIES_ENABLED_KEY, LguConstants.LITHIUM_BATTERIES_ENABLED_DEFAULT, LguConstants.LITHIUM_BATTERIES_ENABLED_DESCRIPTION);
            LITHIUM_BATTERIES_INDIVIDUAL = cfg.BindSyncedEntry(topSection, BaseUpgrade.INDIVIDUAL_SECTION, BaseUpgrade.INDIVIDUAL_DEFAULT, BaseUpgrade.INDIVIDUAL_DESCRIPTION);
            LITHIUM_BATTERIES_PRICE = cfg.BindSyncedEntry(topSection, LguConstants.LITHIUM_BATTERIES_PRICE_KEY, LguConstants.LITHIUM_BATTERIES_PRICE_DEFAULT);
            LITHIUM_BATTERIES_PRICES = cfg.BindSyncedEntry(topSection, BaseUpgrade.PRICES_SECTION, LithiumBatteries.PRICES_DEFAULT, BaseUpgrade.PRICES_DESCRIPTION);
            LITHIUM_BATTERIES_INITIAL_MULTIPLIER = cfg.BindSyncedEntry(topSection, LguConstants.LITHIUM_BATTERIES_INITIAL_MULTIPLIER_KEY, LguConstants.LITHIUM_BATTERIES_INITIAL_MULTIPLIER_DEFAULT, LguConstants.LITHIUM_BATTERIES_INITIAL_MULTIPLIER_DESCRIPTION);
            LITHIUM_BATTERIES_INCREMENTAL_MULTIPLIER = cfg.BindSyncedEntry(topSection, LguConstants.LITHIUM_BATTERIES_INCREMENTAL_MULTIPLIER_KEY, LguConstants.LITHIUM_BATTERIES_INCREMENTAL_MULTIPLIER_DEFAULT, LguConstants.LITHIUM_BATTERIES_INCREMENTAL_MULTIPLIER_DESCRIPTION);
            LITHIUM_BATTERIES_ITEM_PROGRESSION_ITEMS = cfg.BindSyncedEntry(topSection, LguConstants.ITEM_PROGRESSION_ITEMS_KEY, LguConstants.ITEM_PROGRESSION_ITEMS_DEFAULT, LguConstants.ITEM_PROGRESSION_ITEMS_DESCRIPTION);

            #endregion

            #region Locksmith

            topSection = LockSmith.UPGRADE_NAME;
            LOCKSMITH_ENABLED = cfg.BindSyncedEntry(topSection, LguConstants.LOCKSMITH_ENABLED_KEY, LguConstants.LOCKSMITH_ENABLED_DEFAULT, LguConstants.LOCKSMITH_ENABLED_DESCRIPTION);
            LOCKSMITH_PRICE = cfg.BindSyncedEntry(topSection, LguConstants.LOCKSMITH_PRICE_KEY, LguConstants.LOCKSMITH_PRICE_DEFAULT, LguConstants.LOCKSMITH_PRICE_DESCRIPTION);
            LOCKSMITH_INDIVIDUAL = cfg.BindSyncedEntry(topSection, BaseUpgrade.INDIVIDUAL_SECTION, BaseUpgrade.INDIVIDUAL_DEFAULT, BaseUpgrade.INDIVIDUAL_DESCRIPTION);
            LOCKSMITH_ITEM_PROGRESSION_ITEMS = cfg.BindSyncedEntry(topSection, LguConstants.ITEM_PROGRESSION_ITEMS_KEY, LguConstants.ITEM_PROGRESSION_ITEMS_DEFAULT, LguConstants.ITEM_PROGRESSION_ITEMS_DESCRIPTION);

            #endregion

            #region Malware Broadcaster

            topSection = MalwareBroadcaster.UPGRADE_NAME;
            MALWARE_BROADCASTER_ENABLED = cfg.BindSyncedEntry(topSection, LguConstants.MALWARE_BROADCASTER_ENABLED_KEY, LguConstants.MALWARE_BROADCASTER_ENABLED_DEFAULT, LguConstants.MALWARE_BROADCASTER_ENABLED_DESCRIPTION);
            MALWARE_BROADCASTER_PRICE = cfg.BindSyncedEntry(topSection, LguConstants.MALWARE_BROADCASTER_PRICE_KEY, LguConstants.MALWARE_BROADCASTER_PRICE_DEFAULT);
            DESTROY_TRAP = cfg.BindSyncedEntry(topSection, LguConstants.MALWARE_BROADCASTER_DESTROY_TRAPS_KEY, LguConstants.MALWARE_BROADCASTER_DESTROY_TRAPS_DEFAULT, LguConstants.MALWARE_BROADCASTER_DESTROY_TRAPS_DESCRIPTION);
            DISARM_TIME = cfg.BindSyncedEntry(topSection, LguConstants.MALWARE_BROADCASTER_DISARM_TIME_KEY, LguConstants.MALWARE_BROADCASTER_DISARM_TIME_DEFAULT, LguConstants.MALWARE_BROADCASTER_DISARM_TIME_DESCRIPTION);
            EXPLODE_TRAP = cfg.BindSyncedEntry(topSection, LguConstants.MALWARE_BROADCASTER_EXPLODE_TRAPS_KEY, LguConstants.MALWARE_BROADCASTER_EXPLODE_TRAPS_DEFAULT, LguConstants.MALWARE_BROADCASTER_EXPLODE_TRAPS_DESCRIPTION);
            MALWARE_BROADCASTER_INDIVIDUAL = cfg.BindSyncedEntry(topSection, BaseUpgrade.INDIVIDUAL_SECTION, BaseUpgrade.INDIVIDUAL_DEFAULT, BaseUpgrade.INDIVIDUAL_DESCRIPTION);
            MALWARE_BROADCASTER_ITEM_PROGRESSION_ITEMS = cfg.BindSyncedEntry(topSection, LguConstants.ITEM_PROGRESSION_ITEMS_KEY, LguConstants.ITEM_PROGRESSION_ITEMS_DEFAULT, LguConstants.ITEM_PROGRESSION_ITEMS_DESCRIPTION);

            #endregion

            #region Market Influence

            topSection = MarketInfluence.UPGRADE_NAME;
            MARKET_INFLUENCE_ENABLED = cfg.BindSyncedEntry(topSection, LguConstants.MARKET_INFLUENCE_ENABLED_KEY, LguConstants.MARKET_INFLUENCE_ENABLED_DEFAULT, LguConstants.MARKET_INFLUENCE_ENABLED_DESCRIPTION);
            MARKET_INFLUENCE_PRICE = cfg.BindSyncedEntry(topSection, LguConstants.MARKET_INFLUENCE_PRICE_KEY, LguConstants.MARKET_INFLUENCE_PRICE_DEFAULT);
            MARKET_INFLUENCE_PRICES = cfg.BindSyncedEntry(topSection, BaseUpgrade.PRICES_SECTION, MarketInfluence.PRICES_DEFAULT, BaseUpgrade.PRICES_DESCRIPTION);
            MARKET_INFLUENCE_INITIAL_PERCENTAGE = cfg.BindSyncedEntry(topSection, LguConstants.MARKET_INFLUENCE_INITIAL_PERCENTAGE_KEY, LguConstants.MARKET_INFLUENCE_INITIAL_PERCENTAGE_DEFAULT);
            MARKET_INFLUENCE_INCREMENTAL_PERCENTAGE = cfg.BindSyncedEntry(topSection, LguConstants.MARKET_INFLUENCE_INCREMENTAL_PERCENTAGE_KEY, LguConstants.MARKET_INFLUENCE_INCREMENTAL_PERCENTAGE_DEFAULT);
            MARKET_INFLUENCE_ITEM_PROGRESSION_ITEMS = cfg.BindSyncedEntry(topSection, LguConstants.ITEM_PROGRESSION_ITEMS_KEY, LguConstants.ITEM_PROGRESSION_ITEMS_DEFAULT, LguConstants.ITEM_PROGRESSION_ITEMS_DESCRIPTION);

            #endregion

            #region Night Vision

            topSection = "Night Vision";
            NIGHT_VISION_ENABLED = cfg.BindSyncedEntry(topSection, LguConstants.NIGHT_VISION_ENABLED_KEY, LguConstants.NIGHT_VISION_ENABLED_DEFAULT, LguConstants.NIGHT_VISION_ENABLED_DESCRIPTION);
            NIGHT_VISION_PRICE = cfg.BindSyncedEntry(topSection, LguConstants.NIGHT_VISION_PRICE_KEY, LguConstants.NIGHT_VISION_PRICE_DEFAULT);
            NIGHT_BATTERY_MAX = cfg.BindSyncedEntry(topSection, LguConstants.NIGHT_VISION_BATTERY_MAX_KEY, LguConstants.NIGHT_VISION_BATTERY_MAX_DEFAULT, LguConstants.NIGHT_VISION_BATTERY_MAX_DESCRIPTION);
            NIGHT_VIS_DRAIN_SPEED = cfg.BindSyncedEntry(topSection, LguConstants.NIGHT_VISION_DRAIN_SPEED_KEY, LguConstants.NIGHT_VISION_DRAIN_SPEED_DEFAULT, LguConstants.NIGHT_VISION_DRAIN_SPEED_DESCRIPTION);
            NIGHT_VIS_REGEN_SPEED = cfg.BindSyncedEntry(topSection, LguConstants.NIGHT_VISION_REGEN_SPEED_KEY, LguConstants.NIGHT_VISION_REGEN_SPEED_DEFAULT, LguConstants.NIGHT_VISION_REGEN_SPEED_DESCRIPTION);
            NIGHT_VIS_COLOR = cfg.BindSyncedEntry(topSection, LguConstants.NIGHT_VISION_COLOR_KEY, LguConstants.NIGHT_VISION_COLOR_DEFAULT, LguConstants.NIGHT_VISION_COLOR_DESCRIPTION);
            NIGHT_VIS_UI_TEXT_COLOR = cfg.BindSyncedEntry(topSection, LguConstants.NIGHT_VISION_UI_TEXT_COLOR_KEY, LguConstants.NIGHT_VISION_UI_TEXT_COLOR_DEFAULT, LguConstants.NIGHT_VISION_UI_TEXT_COLOR_DESCRIPTION);
            NIGHT_VIS_UI_BAR_COLOR = cfg.BindSyncedEntry(topSection, LguConstants.NIGHT_VISION_UI_BAR_COLOR_KEY, LguConstants.NIGHT_VISION_UI_BAR_COLOR_DEFAULT, LguConstants.NIGHT_VISION_UI_BAR_COLOR_DESCRIPTION);
            NIGHT_VIS_RANGE = cfg.BindSyncedEntry(topSection, LguConstants.NIGHT_VISION_RANGE_KEY, LguConstants.NIGHT_VISION_RANGE_DEFAULT, LguConstants.NIGHT_VISION_RANGE_DESCRIPTION);
            NIGHT_VIS_RANGE_INCREMENT = cfg.BindSyncedEntry(topSection, LguConstants.NIGHT_VISION_RANGE_INCREMENT_KEY, LguConstants.NIGHT_VISION_RANGE_INCREMENT_DEFAULT, LguConstants.NIGHT_VISION_RANGE_INCREMENT_DESCRIPTION);
            NIGHT_VIS_INTENSITY = cfg.BindSyncedEntry(topSection, LguConstants.NIGHT_VISION_INTENSITY_KEY, LguConstants.NIGHT_VISION_INTENSITY_DEFAULT, LguConstants.NIGHT_VISION_INTENSITY_DESCRIPTION);
            NIGHT_VIS_INTENSITY_INCREMENT = cfg.BindSyncedEntry(topSection, LguConstants.NIGHT_VISION_INTENSITY_INCREMENT_KEY, LguConstants.NIGHT_VISION_INTENSITY_INCREMENT_DEFAULT, LguConstants.NIGHT_VISION_INTENSITY_INCREMENT_DESCRIPTION);
            NIGHT_VIS_STARTUP = cfg.BindSyncedEntry(topSection, LguConstants.NIGHT_VISION_STARTUP_KEY, LguConstants.NIGHT_VISION_STARTUP_DEFAULT, LguConstants.NIGHT_VISION_STARTUP_DESCRIPTION);
            NIGHT_VIS_EXHAUST = cfg.BindSyncedEntry(topSection, LguConstants.NIGHT_VISION_EXHAUST_KEY, LguConstants.NIGHT_VISION_EXHAUST_DEFAULT, LguConstants.NIGHT_VISION_EXHAUST_DESCRIPTION);
            NIGHT_VIS_DRAIN_INCREMENT = cfg.BindSyncedEntry(topSection, LguConstants.NIGHT_VISION_DRAIN_INCREMENT_KEY, LguConstants.NIGHT_VISION_DRAIN_INCREMENT_DEFAULT, LguConstants.NIGHT_VISION_DRAIN_INCREMENT_DESCRIPTION);
            NIGHT_VIS_REGEN_INCREMENT = cfg.BindSyncedEntry(topSection, LguConstants.NIGHT_VISION_REGEN_INCREMENT_KEY, LguConstants.NIGHT_VISION_REGEN_INCREMENT_DEFAULT, LguConstants.NIGHT_VISION_REGEN_INCREMENT_DESCRIPTION);
            NIGHT_VIS_BATTERY_INCREMENT = cfg.BindSyncedEntry(topSection, LguConstants.NIGHT_VISION_BATTERY_INCREMENT_KEY, LguConstants.NIGHT_VISION_BATTERY_INCREMENT_DEFAULT, LguConstants.NIGHT_VISION_BATTERY_INCREMENT_DESCRIPTION);
            LOSE_NIGHT_VIS_ON_DEATH = cfg.BindSyncedEntry(topSection, LguConstants.LOSE_NIGHT_VISION_ON_DEATH_KEY, LguConstants.LOSE_NIGHT_VISION_ON_DEATH_DEFAULT, LguConstants.LOSE_NIGHT_VISION_ON_DEATH_DESCRIPTION);
            NIGHT_VISION_DROP_ON_DEATH = cfg.BindSyncedEntry(topSection, LguConstants.NIGHT_VISION_DROP_ON_DEATH_KEY, LguConstants.NIGHT_VISION_DROP_ON_DEATH_DEFAULT, LguConstants.NIGHT_VISION_DROP_ON_DEATH_DESCRIPTION);
            NIGHT_VISION_UPGRADE_PRICES = cfg.BindSyncedEntry(topSection, BaseUpgrade.PRICES_SECTION, NightVision.PRICES_DEFAULT, BaseUpgrade.PRICES_DESCRIPTION);
            NIGHT_VISION_INDIVIDUAL = cfg.BindSyncedEntry(topSection, BaseUpgrade.INDIVIDUAL_SECTION, BaseUpgrade.INDIVIDUAL_DEFAULT, BaseUpgrade.INDIVIDUAL_DESCRIPTION);
            NIGHT_VISION_ITEM_PROGRESSION_ITEMS = cfg.BindSyncedEntry(topSection, LguConstants.ITEM_PROGRESSION_ITEMS_KEY, LguConstants.ITEM_PROGRESSION_ITEMS_DEFAULT, LguConstants.ITEM_PROGRESSION_ITEMS_DESCRIPTION);

            #endregion

            #region Protein Powder

            topSection = ProteinPowder.UPGRADE_NAME;
            PROTEIN_ENABLED = cfg.BindSyncedEntry(topSection, ProteinPowder.ENABLED_SECTION, ProteinPowder.ENABLED_DEFAULT, ProteinPowder.ENABLED_DESCRIPTION);
            PROTEIN_PRICE = cfg.BindSyncedEntry(topSection, ProteinPowder.PRICE_SECTION, ProteinPowder.PRICE_DEFAULT);
            PROTEIN_UNLOCK_FORCE = cfg.BindSyncedEntry(topSection, ProteinPowder.UNLOCK_FORCE_SECTION, ProteinPowder.UNLOCK_FORCE_DEFAULT, ProteinPowder.UNLOCK_FORCE_DESCRIPTION);
            PROTEIN_INCREMENT = cfg.BindSyncedEntry(topSection, ProteinPowder.INCREMENT_FORCE_SECTION, ProteinPowder.INCREMENT_FORCE_DEFAULT, ProteinPowder.INCREMENT_FORCE_DESCRIPTION);
            PROTEIN_INDIVIDUAL = cfg.BindSyncedEntry(topSection, BaseUpgrade.INDIVIDUAL_SECTION, BaseUpgrade.INDIVIDUAL_DEFAULT, BaseUpgrade.INDIVIDUAL_DESCRIPTION);
            PROTEIN_UPGRADE_PRICES = cfg.BindSyncedEntry(topSection, BaseUpgrade.PRICES_SECTION, ProteinPowder.PRICES_DEFAULT, BaseUpgrade.PRICES_DESCRIPTION);
            PROTEIN_CRIT_CHANCE = cfg.BindSyncedEntry(topSection, ProteinPowder.CRIT_CHANCE_SECTION, ProteinPowder.CRIT_CHANCE_DEFAULT, ProteinPowder.CRIT_CHANCE_DESCRIPTION);
            PROTEIN_POWDER_ITEM_PROGRESSION_ITEMS = cfg.BindSyncedEntry(topSection, LguConstants.ITEM_PROGRESSION_ITEMS_KEY, LguConstants.ITEM_PROGRESSION_ITEMS_DEFAULT, LguConstants.ITEM_PROGRESSION_ITEMS_DESCRIPTION);

            #endregion

            #region Quantum Disruptor

            topSection = QuantumDisruptor.UPGRADE_NAME;
            QUANTUM_DISRUPTOR_ENABLED = cfg.BindSyncedEntry(topSection, LguConstants.QUANTUM_DISRUPTOR_ENABLED_KEY, LguConstants.QUANTUM_DISRUPTOR_ENABLED_DEFAULT, LguConstants.QUANTUM_DISRUPTOR_ENABLED_DESCRIPTION);
            QUANTUM_DISRUPTOR_PRICE = cfg.BindSyncedEntry(topSection, LguConstants.QUANTUM_DISRUPTOR_PRICE_KEY, LguConstants.QUANTUM_DISRUPTOR_PRICE_DEFAULT);
            QUANTUM_DISRUPTOR_INITIAL_MULTIPLIER = cfg.BindSyncedEntry(topSection, LguConstants.QUANTUM_DISRUPTOR_INITIAL_MULTIPLIER_KEY, LguConstants.QUANTUM_DISRUPTOR_INITIAL_MULTIPLIER_DEFAULT);
            QUANTUM_DISRUPTOR_INCREMENTAL_MULTIPLIER = cfg.BindSyncedEntry(topSection, LguConstants.QUANTUM_DISRUPTOR_INCREMENTAL_MULTIPLIER_KEY, LguConstants.QUANTUM_DISRUPTOR_INCREMENTAL_MULTIPLIER_DEFAULT);
            QUANTUM_DISRUPTOR_PRICES = cfg.BindSyncedEntry(topSection, BaseUpgrade.PRICES_SECTION, QuantumDisruptor.PRICES_DEFAULT, BaseUpgrade.PRICES_DESCRIPTION);

            QUANTUM_DISRUPTOR_RESET_MODE = cfg.BindSyncedEntry(topSection, LguConstants.QUANTUM_DISRUPTOR_RESET_MODE_KEY, LguConstants.QUANTUM_DISRUPTOR_RESET_MODE_DEFAULT, LguConstants.QUANTUM_DISRUPTOR_RESET_MODE_DESCRIPTION);
            QUANTUM_DISRUPTOR_UPGRADE_MODE = cfg.BindSyncedEntry(topSection, LguConstants.QUANTUM_DISRUPTOR_UPGRADE_MODE_KEY, LguConstants.QUANTUM_DISRUPTOR_UPGRADE_MODE_DEFAULT, LguConstants.QUANTUM_DISRUPTOR_UPGRADE_MODE_DESCRIPTION);
            QUANTUM_DISRUPTOR_INITIAL_USES = cfg.BindSyncedEntry(topSection, LguConstants.QUANTUM_DISRUPTOR_INITIAL_USES_KEY, LguConstants.QUANTUM_DISRUPTOR_INITIAL_USES_DEFAULT, LguConstants.QUANTUM_DISRUPTOR_INITIAL_USES_DESCRIPTION);
            QUANTUM_DISRUPTOR_INCREMENTAL_USES = cfg.BindSyncedEntry(topSection, LguConstants.QUANTUM_DISRUPTOR_INCREMENTAL_USES_KEY, LguConstants.QUANTUM_DISRUPTOR_INCREMENTAL_USES_DEFAULT, LguConstants.QUANTUM_DISRUPTOR_INCREMENTAL_USES_DESCRIPTION);
            QUANTUM_DISRUPTOR_INITIAL_HOURS_REVERT_ON_USE = cfg.BindSyncedEntry(topSection, LguConstants.QUANTUM_DISRUPTOR_INITIAL_HOURS_ON_REVERT_KEY, LguConstants.QUANTUM_DISRUPTOR_INITIAL_HOURS_ON_REVERT_DEFAULT, LguConstants.QUANTUM_DISRUPTOR_INITIAL_HOURS_ON_REVERT_DESCRIPTION);
            QUANTUM_DISRUPTOR_INCREMENTAL_HOURS_REVERT_ON_USE = cfg.BindSyncedEntry(topSection, LguConstants.QUANTUM_DISRUPTOR_INCREMENTAL_HOURS_ON_REVERT_KEY, LguConstants.QUANTUM_DISRUPTOR_INCREMENTAL_HOURS_ON_REVERT_DEFAULT, LguConstants.QUANTUM_DISRUPTOR_INCREMENTAL_HOURS_ON_REVERT_DESCRIPTION);
            QUANTUM_DISRUPTOR_ITEM_PROGRESSION_ITEMS = cfg.BindSyncedEntry(topSection, LguConstants.ITEM_PROGRESSION_ITEMS_KEY, LguConstants.ITEM_PROGRESSION_ITEMS_DEFAULT, LguConstants.ITEM_PROGRESSION_ITEMS_DESCRIPTION);

            #endregion

            #region Running Shoes

            topSection = RunningShoes.UPGRADE_NAME;
            RUNNING_SHOES_ENABLED = cfg.BindSyncedEntry(topSection, LguConstants.RUNNING_SHOES_ENABLED_KEY, LguConstants.RUNNING_SHOES_ENABLED_DEFAULT, LguConstants.RUNNING_SHOES_ENABLED_DESCRIPTION);
            RUNNING_SHOES_PRICE = cfg.BindSyncedEntry(topSection, LguConstants.RUNNING_SHOES_PRICE_KEY, LguConstants.RUNNING_SHOES_PRICE_DEFAULT);
            MOVEMENT_SPEED_UNLOCK = cfg.BindSyncedEntry(topSection, LguConstants.RUNNING_SHOES_INITIAL_MOVEMENT_BOOST_KEY, LguConstants.RUNNING_SHOES_INITIAL_MOVEMENT_BOOST_DEFAULT, LguConstants.RUNNING_SHOES_INITIAL_MOVEMENT_BOOST_DESCRIPTION);
            MOVEMENT_INCREMENT = cfg.BindSyncedEntry(topSection, LguConstants.RUNNING_SHOES_INCREMENTAL_MOVEMENT_BOOST_KEY, LguConstants.RUNNING_SHOES_INCREMENTAL_MOVEMENT_BOOST_DEFAULT, LguConstants.RUNNING_SHOES_INCREMENTAL_MOVEMENT_BOOST_DESCRIPTION);
            RUNNING_SHOES_UPGRADE_PRICES = cfg.BindSyncedEntry(topSection, BaseUpgrade.PRICES_SECTION, BiggerLungs.PRICES_DEFAULT, BaseUpgrade.PRICES_DESCRIPTION);
            RUNNING_SHOES_INDIVIDUAL = cfg.BindSyncedEntry(topSection, BaseUpgrade.INDIVIDUAL_SECTION, BaseUpgrade.INDIVIDUAL_DEFAULT, BaseUpgrade.INDIVIDUAL_DESCRIPTION);
            NOISE_REDUCTION = cfg.BindSyncedEntry(topSection, LguConstants.RUNNING_SHOES_NOISE_REDUCTION_KEY, LguConstants.RUNNING_SHOES_NOISE_REDUCTION_DEFAULT, LguConstants.RUNNING_SHOES_NOISE_REDUCTION_DESCRIPTION);
            RUNNING_SHOES_ITEM_PROGRESSION_ITEMS = cfg.BindSyncedEntry(topSection, LguConstants.ITEM_PROGRESSION_ITEMS_KEY, LguConstants.ITEM_PROGRESSION_ITEMS_DEFAULT, LguConstants.ITEM_PROGRESSION_ITEMS_DESCRIPTION);

            #endregion

            #region Shutter Batteries

            topSection = ShutterBatteries.UPGRADE_NAME;
            DOOR_HYDRAULICS_BATTERY_ENABLED = cfg.BindSyncedEntry(topSection, ShutterBatteries.ENABLED_SECTION, true, ShutterBatteries.ENABLED_DESCRIPTION);
            DOOR_HYDRAULICS_BATTERY_PRICE = cfg.BindSyncedEntry(topSection, ShutterBatteries.PRICE_SECTION, ShutterBatteries.PRICE_DEFAULT, "");
            DOOR_HYDRAULICS_BATTERY_PRICES = cfg.BindSyncedEntry(topSection, BaseUpgrade.PRICES_SECTION, ShutterBatteries.PRICES_DEFAULT, BaseUpgrade.PRICES_DESCRIPTION);
            DOOR_HYDRAULICS_BATTERY_INITIAL = cfg.BindSyncedEntry(topSection, ShutterBatteries.INITIAL_SECTION, ShutterBatteries.INITIAL_DEFAULT, ShutterBatteries.INITIAL_DESCRIPTION);
            DOOR_HYDRAULICS_BATTERY_INCREMENTAL = cfg.BindSyncedEntry(topSection, ShutterBatteries.INCREMENTAL_SECTION, ShutterBatteries.INCREMENTAL_DEFAULT, ShutterBatteries.INCREMENTAL_DESCRIPTION);
            SHUTTER_BATTERIES_ITEM_PROGRESSION_ITEMS = cfg.BindSyncedEntry(topSection, LguConstants.ITEM_PROGRESSION_ITEMS_KEY, LguConstants.ITEM_PROGRESSION_ITEMS_DEFAULT, LguConstants.ITEM_PROGRESSION_ITEMS_DESCRIPTION);

            #endregion

            #region Sick Beats

            topSection = SickBeats.UPGRADE_NAME;
            BEATS_ENABLED = cfg.BindSyncedEntry(topSection, LguConstants.SICK_BEATS_ENABLED_KEY, LguConstants.SICK_BEATS_ENABLED_DEFAULT, LguConstants.SICK_BEATS_ENABLED_DESCRIPTION);
            BEATS_INDIVIDUAL = cfg.BindSyncedEntry(topSection, BaseUpgrade.INDIVIDUAL_SECTION, BaseUpgrade.INDIVIDUAL_DEFAULT, BaseUpgrade.INDIVIDUAL_DESCRIPTION);
            BEATS_PRICE = cfg.BindSyncedEntry(topSection, LguConstants.SICK_BEATS_PRICE_KEY, LguConstants.SICK_BEATS_PRICE_DEFAULT);
            BEATS_SPEED = cfg.BindSyncedEntry(topSection, LguConstants.SICK_BEATS_SPEED_KEY, LguConstants.SICK_BEATS_SPEED_DEFAULT);
            BEATS_DMG = cfg.BindSyncedEntry(topSection, LguConstants.SICK_BEATS_DAMAGE_KEY, LguConstants.SICK_BEATS_DAMAGE_DEFAULT);
            BEATS_STAMINA = cfg.BindSyncedEntry(topSection, LguConstants.SICK_BEATS_STAMINA_KEY, LguConstants.SICK_BEATS_STAMINA_DEFAULT);
            BEATS_DEF = cfg.BindSyncedEntry(topSection, LguConstants.SICK_BEATS_DEFENSE_KEY, LguConstants.SICK_BEATS_DEFENSE_DEFAULT);
            BEATS_DEF_CO = cfg.BindSyncedEntry(topSection, LguConstants.SICK_BEATS_DEFENSE_MULTIPLIER_KEY, LguConstants.SICK_BEATS_DEFENSE_MULTIPLIER_DEFAULT, LguConstants.SICK_BEATS_DEFENSE_MULTIPLIER_DESCRIPTION);
            BEATS_STAMINA_CO = cfg.BindSyncedEntry(topSection, LguConstants.SICK_BEATS_STAMINA_MULTIPLIER_KEY, LguConstants.SICK_BEATS_STAMINA_MULTIPLIER_DEFAULT, LguConstants.SICK_BEATS_STAMINA_MULTIPLIER_DESCRIPTION);
            BEATS_DMG_INC = cfg.BindSyncedEntry(topSection, LguConstants.SICK_BEATS_ADDITIONAL_DAMAGE_KEY, LguConstants.SICK_BEATS_ADDITIONAL_DAMAGE_DEFAULT);
            BEATS_SPEED_INC = cfg.BindSyncedEntry(topSection, LguConstants.SICK_BEATS_ADDITIONAL_SPEED_KEY, LguConstants.SICK_BEATS_ADDITIONAL_SPEED_DEFAULT);
            BEATS_RADIUS = cfg.BindSyncedEntry(topSection, LguConstants.SICK_BEATS_EFFECT_RADIUS_KEY, LguConstants.SICK_BEATS_EFFECT_RADIUS_DEFAULT, LguConstants.SICK_BEATS_EFFECT_RADIUS_DESCRIPTION);
            SICK_BEATS_ITEM_PROGRESSION_ITEMS = cfg.BindSyncedEntry(topSection, LguConstants.ITEM_PROGRESSION_ITEMS_KEY, LguConstants.ITEM_PROGRESSION_ITEMS_DEFAULT, LguConstants.ITEM_PROGRESSION_ITEMS_DESCRIPTION);

            #endregion

            #region Sigurd Access

            topSection = Sigurd.UPGRADE_NAME;
            SIGURD_ENABLED = cfg.BindSyncedEntry(topSection, LguConstants.SIGURD_ACCESS_ENABLED_KEY, LguConstants.SIGURD_ACCESS_ENABLED_DEFAULT, LguConstants.SIGURD_ACCESS_ENABLED_DESCRIPTION);
            SIGURD_LAST_DAY_ENABLED = cfg.BindSyncedEntry(topSection, LguConstants.SIGURD_ACCESS_ENABLED_LAST_DAY_KEY, LguConstants.SIGURD_ACCESS_ENABLED_LAST_DAY_DEFAULT, LguConstants.SIGURD_ACCESS_ENABLED_LAST_DAY_DESCRIPTION);
            SIGURD_PRICE = cfg.BindSyncedEntry(topSection, LguConstants.SIGURD_ACCESS_PRICE_KEY, LguConstants.SIGURD_ACCESS_PRICE_DEFAULT, LguConstants.SIGURD_ACCESS_PRICE_DESCRIPTION);
            SIGURD_CHANCE = cfg.BindSyncedEntry(topSection, LguConstants.SIGURD_ACCESS_CHANCE_KEY, LguConstants.SIGURD_ACCESS_CHANCE_DEFAULT);
            SIGURD_LAST_DAY_CHANCE = cfg.BindSyncedEntry(topSection, LguConstants.SIGURD_ACCESS_CHANCE_LAST_DAY_KEY, LguConstants.SIGURD_ACCESS_CHANCE_LAST_DAY_DEFAULT);
            SIGURD_PERCENT = cfg.BindSyncedEntry(topSection, LguConstants.SIGURD_ACCESS_ADDITIONAL_PERCENT_KEY, LguConstants.SIGURD_ACCESS_ADDITIONAL_PERCENT_DEFAULT);
            SIGURD_LAST_DAY_PERCENT = cfg.BindSyncedEntry(topSection, LguConstants.SIGURD_ACCESS_ADDITIONAL_PERCENT_LAST_DAY_KEY, LguConstants.SIGURD_ACCESS_ADDITIONAL_PERCENT_LAST_DAY_DEFAULT);
            SIGURD_ACCESS_ITEM_PROGRESSION_ITEMS = cfg.BindSyncedEntry(topSection, LguConstants.ITEM_PROGRESSION_ITEMS_KEY, LguConstants.ITEM_PROGRESSION_ITEMS_DEFAULT, LguConstants.ITEM_PROGRESSION_ITEMS_DESCRIPTION);

            #endregion

            #region Stimpack

            topSection = Stimpack.UPGRADE_NAME;
            PLAYER_HEALTH_ENABLED = cfg.BindSyncedEntry(topSection, Stimpack.ENABLED_SECTION, Stimpack.ENABLED_DEFAULT, Stimpack.ENABLED_DESCRIPTION);
            PLAYER_HEALTH_PRICE = cfg.BindSyncedEntry(topSection, Stimpack.PRICE_SECTION, Stimpack.PRICE_DEFAULT);
            PLAYER_HEALTH_INDIVIDUAL = cfg.BindSyncedEntry(topSection, BaseUpgrade.INDIVIDUAL_SECTION, BaseUpgrade.INDIVIDUAL_DEFAULT, BaseUpgrade.INDIVIDUAL_DESCRIPTION);
            PLAYER_HEALTH_UPGRADE_PRICES = cfg.BindSyncedEntry(topSection, BaseUpgrade.PRICES_SECTION, Stimpack.PRICES_DEFAULT, BaseUpgrade.PRICES_DESCRIPTION);
            PLAYER_HEALTH_ADDITIONAL_HEALTH_UNLOCK = cfg.BindSyncedEntry(topSection, Stimpack.ADDITIONAL_HEALTH_UNLOCK_SECTION, Stimpack.ADDITIONAL_HEALTH_UNLOCK_DEFAULT, Stimpack.ADDITIONAL_HEALTH_UNLOCK_DESCRIPTION);
            PLAYER_HEALTH_ADDITIONAL_HEALTH_INCREMENT = cfg.BindSyncedEntry(topSection, Stimpack.ADDITIONAL_HEALTH_INCREMENT_SECTION, Stimpack.ADDITIONAL_HEALTH_INCREMENT_DEFAULT, Stimpack.ADDITIONAL_HEALTH_INCREMENT_DESCRIPTION);
            STIMPACK_ITEM_PROGRESSION_ITEMS = cfg.BindSyncedEntry(topSection, LguConstants.ITEM_PROGRESSION_ITEMS_KEY, LguConstants.ITEM_PROGRESSION_ITEMS_DEFAULT, LguConstants.ITEM_PROGRESSION_ITEMS_DESCRIPTION);

            #endregion

            #region Strong Legs

            topSection = StrongLegs.UPGRADE_NAME;
            STRONG_LEGS_ENABLED = cfg.BindSyncedEntry(topSection, LguConstants.STRONG_LEGS_ENABLED_KEY, LguConstants.STRONG_LEGS_ENABLED_DEFAULT, LguConstants.STRONG_LEGS_ENABLED_DESCRIPTION);
            STRONG_LEGS_PRICE = cfg.BindSyncedEntry(topSection, LguConstants.STRONG_LEGS_PRICE_KEY, LguConstants.STRONG_LEGS_PRICE_DEFAULT);
            JUMP_FORCE_UNLOCK = cfg.BindSyncedEntry(topSection, LguConstants.STRONG_LEGS_INITIAL_JUMP_FORCE_KEY, LguConstants.STRONG_LEGS_INITIAL_JUMP_FORCE_DEFAULT, LguConstants.STRONG_LEGS_INITIAL_JUMP_FORCE_DESCRIPTION);
            JUMP_FORCE_INCREMENT = cfg.BindSyncedEntry(topSection, LguConstants.STRONG_LEGS_INCREMENTAL_JUMP_FORCE_KEY, LguConstants.STRONG_LEGS_INCREMENTAL_JUMP_FORCE_DEFAULT, LguConstants.STRONG_LEGS_INCREMENTAL_JUMP_FORCE_DESCRIPTION);
            STRONG_LEGS_UPGRADE_PRICES = cfg.BindSyncedEntry(topSection, BaseUpgrade.PRICES_SECTION, StrongLegs.PRICES_DEFAULT, BaseUpgrade.PRICES_DESCRIPTION);
            STRONG_LEGS_INDIVIDUAL = cfg.BindSyncedEntry(topSection, BaseUpgrade.INDIVIDUAL_SECTION, BaseUpgrade.INDIVIDUAL_DEFAULT, BaseUpgrade.INDIVIDUAL_DESCRIPTION);
            STRONG_LEGS_ITEM_PROGRESSION_ITEMS = cfg.BindSyncedEntry(topSection, LguConstants.ITEM_PROGRESSION_ITEMS_KEY, LguConstants.ITEM_PROGRESSION_ITEMS_DEFAULT, LguConstants.ITEM_PROGRESSION_ITEMS_DESCRIPTION);

            #endregion

            #region Walkie GPS

            topSection = WalkieGPS.UPGRADE_NAME;
            WALKIE_ENABLED = cfg.BindSyncedEntry(topSection, LguConstants.WALKIE_GPS_ENABLED_KEY, LguConstants.WALKIE_GPS_ENABLED_DEFAULT, LguConstants.WALKIE_GPS_ENABLED_DESCRIPTION);
            WALKIE_PRICE = cfg.BindSyncedEntry(topSection, LguConstants.WALKIE_GPS_PRICE_KEY, LguConstants.WALKIE_GPS_PRICE_DEFAULT, LguConstants.WALKIE_GPS_PRICE_DESCRIPTION);
            WALKIE_INDIVIDUAL = cfg.BindSyncedEntry(topSection, BaseUpgrade.INDIVIDUAL_SECTION, BaseUpgrade.INDIVIDUAL_DEFAULT, BaseUpgrade.INDIVIDUAL_DESCRIPTION);
            WALKIE_GPS_ITEM_PROGRESSION_ITEMS = cfg.BindSyncedEntry(topSection, LguConstants.ITEM_PROGRESSION_ITEMS_KEY, LguConstants.ITEM_PROGRESSION_ITEMS_DEFAULT, LguConstants.ITEM_PROGRESSION_ITEMS_DESCRIPTION);

            #endregion

            #endregion

            #region Commands

            #region Interns

            topSection = Interns.NAME;
            INTERN_ENABLED = cfg.BindSyncedEntry(topSection, LguConstants.INTERNS_ENABLED_KEY, LguConstants.INTERNS_ENABLED_DEFAULT, LguConstants.INTERNS_ENABLED_DESCRIPTION);
            INTERN_PRICE = cfg.BindSyncedEntry(topSection, LguConstants.INTERNS_PRICE_KEY, LguConstants.INTERNS_PRICE_DEFAULT, LguConstants.INTERNS_PRICE_DESCRIPTION);

            #endregion

            #endregion

            InitialSyncCompleted += PluginConfig_InitialSyncCompleted;
            ConfigManager.Register(this);
        }

        #endregion

        private void PluginConfig_InitialSyncCompleted(object sender, EventArgs e)
        {
            CheckMedkit();
            UpgradeBus.Instance.Reconstruct();
        }

        void CheckMedkit()
        {
            int amount = UpgradeBus.Instance.spawnableMapObjectsAmount["MedkitMapItem"];
            if (amount == UpgradeBus.Instance.PluginConfiguration.EXTRACTION_CONTRACT_AMOUNT_MEDKITS.Value) return;
            MapObjects.RemoveMapObject(UpgradeBus.Instance.spawnableMapObjects["MedkitMapItem"], Levels.LevelTypes.All);
            AnimationCurve curve = new(new Keyframe(0f, UpgradeBus.Instance.PluginConfiguration.EXTRACTION_CONTRACT_AMOUNT_MEDKITS.Value), new Keyframe(1f, UpgradeBus.Instance.PluginConfiguration.EXTRACTION_CONTRACT_AMOUNT_MEDKITS.Value));
            MapObjects.RegisterMapObject(mapObject: UpgradeBus.Instance.spawnableMapObjects["MedkitMapItem"], levels: Levels.LevelTypes.All, spawnRateFunction: (_) => curve);
        }
    }
}
