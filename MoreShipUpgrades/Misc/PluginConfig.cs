using BepInEx.Configuration;
using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using MoreShipUpgrades.UpgradeComponents.OneTimeUpgrades;
using MoreShipUpgrades.UpgradeComponents.TierUpgrades;
using MoreShipUpgrades.UpgradeComponents.Commands;


namespace MoreShipUpgrades.Misc
{
    [Serializable]
    public class PluginConfig
    {
        readonly ConfigFile configFile;

        // enabled disabled
        public bool CONTRACTS_ENABLED { get; set; }
        public bool ADVANCED_TELE_ENABLED { get; set; }
        public bool WEAK_TELE_ENABLED { get; set; }
        public bool BEEKEEPER_ENABLED { get; set; }
        public bool PROTEIN_ENABLED { get; set; }
        public bool BIGGER_LUNGS_ENABLED { get; set; }
        public bool BACK_MUSCLES_ENABLED { get; set; }
        public bool LIGHT_FOOTED_ENABLED { get; set; }
        public bool NIGHT_VISION_ENABLED { get; set; }
        public bool RUNNING_SHOES_ENABLED { get; set; }
        public bool BETTER_SCANNER_ENABLED { get; set; }
        public bool STRONG_LEGS_ENABLED { get; set; }
        public bool DISCOMBOBULATOR_ENABLED { get; set; }
        public bool MALWARE_BROADCASTER_ENABLED { get; set; }
        public bool LIGHTNING_ROD_ENABLED { get; set; }
        public bool HUNTER_ENABLED { get; set; }
        public bool PLAYER_HEALTH_ENABLED { get; set; }
        public bool PEEPER_ENABLED { get; set; }
        public bool EXTEND_DEADLINE_ENABLED { get; set; }
        public bool WHEELBARROW_ENABLED { get; set; }
        public bool SCRAP_WHEELBARROW_ENABLED { get; set; }
        public bool DOOR_HYDRAULICS_BATTERY_ENABLED {  get; set; }
        public bool SCRAP_INSURANCE_ENABLED {  get; set; }

        // individual or shared
        public bool ADVANCED_TELE_INDIVIDUAL { get; set; }
        public bool WEAK_TELE_INDIVIDUAL { get; set; }
        public bool BEEKEEPER_INDIVIDUAL { get; set; }
        public bool PROTEIN_INDIVIDUAL { get; set; }
        public bool BIGGER_LUNGS_INDIVIDUAL { get; set; }
        public bool BACK_MUSCLES_INDIVIDUAL { get; set; }
        public bool LIGHT_FOOTED_INDIVIDUAL { get; set; }
        public bool NIGHT_VISION_INDIVIDUAL { get; set; }
        public bool HUNTER_INDIVIDUAL { get; set; }
        public bool PLAYER_HEALTH_INDIVIDUAL { get; set; }

        public bool RUNNING_SHOES_INDIVIDUAL { get; set; }
        public bool BETTER_SCANNER_INDIVIDUAL { get; set; }
        public bool STRONG_LEGS_INDIVIDUAL { get; set; }
        public bool DISCOMBOBULATOR_INDIVIDUAL { get; set; }
        public bool MALWARE_BROADCASTER_INDIVIDUAL { get; set; }
        public bool INTERN_INDIVIDUAL { get; set; }
        public bool LOCKSMITH_INDIVIDUAL { get; set; }

        // prices
        public int PEEPER_PRICE { get; set; }
        public int HUNTER_PRICE { get; set; }
        public int ADVANCED_TELE_PRICE { get; set; }
        public int WEAK_TELE_PRICE { get; set; }
        public int BEEKEEPER_PRICE { get; set; }
        public int PROTEIN_PRICE { get; set; }
        public int BIGGER_LUNGS_PRICE { get; set; }
        public int BACK_MUSCLES_PRICE { get; set; }
        public int LIGHT_FOOTED_PRICE { get; set; }
        public int NIGHT_VISION_PRICE { get; set; }
        public int RUNNING_SHOES_PRICE { get; set; }
        public int BETTER_SCANNER_PRICE { get; set; }
        public int STRONG_LEGS_PRICE { get; set; }
        public int DISCOMBOBULATOR_PRICE { get; set; }
        public int MALWARE_BROADCASTER_PRICE { get; set; }
        public int WALKIE_PRICE { get; set; }
        public int LIGHTNING_ROD_PRICE { get; set; }
        public int PLAYER_HEALTH_PRICE { get; set; }
        public int EXTEND_DEADLINE_PRICE { get; set; }
        public int CONTRACT_PRICE { get; set; }
        public int CONTRACT_SPECIFY_PRICE { get; set; }
        public int WHEELBARROW_PRICE { get; set; }
        public int DOOR_HYDRAULICS_BATTERY_PRICE { get; set; }
        public int SCRAP_INSURANCE_PRICE { get; set; }

        // attributes
        public float BIGGER_LUNGS_STAMINA_REGEN_INCREASE { get; set; }
        public float BIGGER_LUNGS_JUMP_STAMINA_COST_DECREASE { get; set; }
        public int PROTEIN_INCREMENT { get; set; }
        public bool KEEP_ITEMS_ON_TELE { get; set; }
        public float SPRINT_TIME_INCREASE_UNLOCK { get; set; }
        public float MOVEMENT_SPEED_UNLOCK { get; set; }
        public float JUMP_FORCE_UNLOCK { get; set; }
        public bool DESTROY_TRAP { get; set; }
        public float DISARM_TIME { get; set; }
        public bool EXPLODE_TRAP { get; set; }
        public bool REQUIRE_LINE_OF_SIGHT { get; set; }
        public float CARRY_WEIGHT_REDUCTION { get; set; }
        public float NODE_DISTANCE_INCREASE { get; set; }
        public float SHIP_AND_ENTRANCE_DISTANCE_INCREASE { get; set; }
        public float NOISE_REDUCTION { get; set; }
        public float DISCOMBOBULATOR_COOLDOWN { get; set; }
        public float ADV_CHANCE_TO_BREAK { get; set; }
        public bool ADV_KEEP_ITEMS_ON_TELE { get; set; }
        public float CHANCE_TO_BREAK { get; set; }
        public float BEEKEEPER_DAMAGE_MULTIPLIER { get; set; }
        public float BEEKEEPER_DAMAGE_MULTIPLIER_INCREMENT { get; set; }
        public float DISCOMBOBULATOR_RADIUS { get; set; }
        public float DISCOMBOBULATOR_STUN_DURATION { get; set; }
        public bool DISCOMBOBULATOR_NOTIFY_CHAT { get; set; }
        [JsonIgnore]
        public UnityEngine.Color NIGHT_VIS_COLOR { get; set; }
        public float NIGHT_VIS_DRAIN_SPEED { get; set; }
        public float NIGHT_VIS_REGEN_SPEED { get; set; }
        public float NIGHT_BATTERY_MAX { get; set; }
        public float NIGHT_VIS_RANGE { get; set; }
        public float NIGHT_VIS_RANGE_INCREMENT { get; set; }
        public float NIGHT_VIS_INTENSITY { get; set; }
        public float NIGHT_VIS_INTENSITY_INCREMENT { get; set; }
        public float NIGHT_VIS_STARTUP { get; set; }
        public float NIGHT_VIS_EXHAUST { get; set; }
        public float NIGHT_VIS_DRAIN_INCREMENT { get; set; }
        public float NIGHT_VIS_REGEN_INCREMENT { get; set; }
        public float NIGHT_VIS_BATTERY_INCREMENT { get; set; }
        public float CARRY_WEIGHT_INCREMENT { get; set; }
        public float MOVEMENT_INCREMENT { get; set; }
        public float SPRINT_TIME_INCREMENT { get; set; }
        public float JUMP_FORCE_INCREMENT { get; set; }
        public float DISCOMBOBULATOR_INCREMENT { get; set; }
        public int INTERN_PRICE { get; set; }
        public int LOCKSMITH_PRICE { get; set; }
        public bool INTERN_ENABLED { get; set; }
        public bool LOCKSMITH_ENABLED { get; set; }
        public string TOGGLE_NIGHT_VISION_KEY { get; set; }
        public float SALE_PERC { get; set; }
        public bool LOSE_NIGHT_VIS_ON_DEATH { get; set; }
        public bool NIGHT_VISION_DROP_ON_DEATH { get; set; }
        public string BEEKEEPER_UPGRADE_PRICES { get; set; }
        public float BEEKEEPER_HIVE_VALUE_INCREASE { get; set; }
        public string BACK_MUSCLES_UPGRADE_PRICES { get; set; }
        public string BIGGER_LUNGS_UPGRADE_PRICES { get; set; }
        public string LIGHT_FOOTED_UPGRADE_PRICES { get; set; }
        public string NIGHT_VISION_UPGRADE_PRICES { get; set; }
        public string RUNNING_SHOES_UPGRADE_PRICES { get; set; }
        public string STRONG_LEGS_UPGRADE_PRICES { get; set; }
        public string DISCO_UPGRADE_PRICES { get; set; }
        public string PROTEIN_UPGRADE_PRICES { get; set; }
        public string PLAYER_HEALTH_UPGRADE_PRICES { get; set; }
        public string HUNTER_UPGRADE_PRICES { get; set; }
        public string HUNTER_SAMPLE_TIERS {  get; set; }
        public bool SHARED_UPGRADES { get; set; }
        public bool WALKIE_ENABLED { get; set; }
        public bool WALKIE_INDIVIDUAL { get; set; }
        public int PROTEIN_UNLOCK_FORCE {  get; set; }
        public float PROTEIN_CRIT_CHANCE { get; set; }
        public int BETTER_SCANNER_PRICE2 {  get; set; }
        public int BETTER_SCANNER_PRICE3 {  get; set; }
        public bool BETTER_SCANNER_ENEMIES {  get; set; }
        public bool INTRO_ENABLED { get; set; }
        public bool LIGHTNING_ROD_ACTIVE { get; set; }
        public float LIGHTNING_ROD_DIST {  get; set; }
        public bool PAGER_ENABLED {  get; set; }
        public int PAGER_PRICE {  get; set; }
        public bool VERBOSE_ENEMIES {  get; set; }
        public int PLAYER_HEALTH_ADDITIONAL_HEALTH_UNLOCK { get; set; }
        public int PLAYER_HEALTH_ADDITIONAL_HEALTH_INCREMENT { get; set; }
        public bool MEDKIT_ENABLED { get; set; }
        public int MEDKIT_PRICE { get; set; }
        public int MEDKIT_HEAL_VALUE { get; set; }
        public int MEDKIT_USES { get; set; }
        public int DIVEKIT_PRICE { get; set; }
        public bool DIVEKIT_ENABLED { get; set; }
        public float DIVEKIT_WEIGHT { get; set; }
        public bool DIVEKIT_TWO_HANDED { get; set; }
        public int DISCOMBOBULATOR_DAMAGE_LEVEL { get; set; }
        public int DISCOMBOBULATOR_INITIAL_DAMAGE {  get; set; }
        public int DISCOMBOBULATOR_DAMAGE_INCREASE { get; set; }
        public float STRONG_LEGS_REDUCE_FALL_DAMAGE_MULTIPLIER { get; set; }
        public bool KEEP_UPGRADES_AFTER_FIRED_CUTSCENE { get; set; }
        public int CONTRACT_BUG_REWARD {  get; set; }
        public int CONTRACT_EXOR_REWARD {  get; set; }
        public int CONTRACT_DEFUSE_REWARD {  get; set; }
        public int CONTRACT_BUG_SPAWNS {  get; set; }
        public int CONTRACT_EXTRACT_REWARD {  get; set; }
        public float CONTRACT_EXTRACT_WEIGHT {  get; set; }
        public int CONTRACT_DATA_REWARD {  get; set; }
        public bool BEATS_INDIVIDUAL { get; set; }
        public bool BEATS_ENABLED { get; set; }
        public int BEATS_PRICE { get; set; }
        public bool BEATS_SPEED { get; set; }
        public bool BEATS_DMG { get; set; }
        public bool BEATS_STAMINA { get; set; }
        public float BEATS_STAMINA_CO { get; set; }
        public bool BEATS_DEF { get; set; }
        public float BEATS_DEF_CO { get; set; }
        public float BEATS_SPEED_INC { get; set; }
        public float BEATS_RADIUS { get; set; }
        public int BEATS_DMG_INC { get; set; }
        public bool HELMET_ENABLED { get; set; }
        public int HELMET_PRICE { get; set; }
        public int HELMET_HITS_BLOCKED { get; set; }
        public int SNARE_FLEA_SAMPLE_MINIMUM_VALUE { get; set; }
        public int SNARE_FLEA_SAMPLE_MAXIMUM_VALUE { get; set; }
        public int BUNKER_SPIDER_SAMPLE_MINIMUM_VALUE { get; set; }
        public int BUNKER_SPIDER_SAMPLE_MAXIMUM_VALUE { get; set; }
        public int HOARDING_BUG_SAMPLE_MINIMUM_VALUE { get; set; }
        public int HOARDING_BUG_SAMPLE_MAXIMUM_VALUE { get; set; }
        public int BRACKEN_SAMPLE_MINIMUM_VALUE { get; set; }
        public int BRACKEN_SAMPLE_MAXIMUM_VALUE { get; set; }
        public int EYELESS_DOG_SAMPLE_MINIMUM_VALUE { get; set; }
        public int EYELESS_DOG_SAMPLE_MAXIMUM_VALUE { get; set; }
        public int BABOON_HAWK_SAMPLE_MINIMUM_VALUE { get; set; }
        public int BABOON_HAWK_SAMPLE_MAXIMUM_VALUE { get; set; }
        public int THUMPER_SAMPLE_MINIMUM_VALUE { get; set; }
        public int THUMPER_SAMPLE_MAXIMUM_VALUE { get; set; }
        public int CONTRACT_GHOST_SPAWN { get; set; }
        public string WHEELBARROW_RESTRICTION_MODE { get; set; }
        public int WHEELBARROW_MAXIMUM_AMOUNT_ITEMS {  get; set; }
        public float WHEELBARROW_MAXIMUM_WEIGHT_ALLOWED { get; set; }
        public float WHEELBARROW_WEIGHT_REDUCTION_MULTIPLIER { get; set; }
        public float WHEELBARROW_WEIGHT {  get; set; }
        public float WHEELBARROW_LOOK_SENSITIVITY_DRAWBACK {  get; set; }
        public float WHEELBARROW_MOVEMENT_SLOPPY {  get; set; }
        public float WHEELBARROW_NOISE_RANGE { get; set; }
        public bool WHEELBARROW_PLAY_NOISE {  get; set; }
        public string SCRAP_WHEELBARROW_RESTRICTION_MODE { get; set; }
        public int SCRAP_WHEELBARROW_MAXIMUM_AMOUNT_ITEMS { get; set; }
        public float SCRAP_WHEELBARROW_MAXIMUM_WEIGHT_ALLOWED { get; set; }
        public float SCRAP_WHEELBARROW_WEIGHT_REDUCTION_MULTIPLIER { get; set; }
        public float SCRAP_WHEELBARROW_WEIGHT { get; set; }
        public float SCRAP_WHEELBARROW_LOOK_SENSITIVITY_DRAWBACK { get; set; }
        public float SCRAP_WHEELBARROW_NOISE_RANGE { get; set; }
        public int SCRAP_WHEELBARROW_MINIMUM_VALUE { get; set; }
        public int SCRAP_WHEELBARROW_MAXIMUM_VALUE { get; set; }
        public float SCRAP_WHEELBARROW_MOVEMENT_SLOPPY { get; set; }
        public float SCRAP_WHEELBARROW_RARITY { get; set; }
        public bool SCRAP_WHEELBARROW_PLAY_NOISE {  get; set; }
        public float SCAV_VOLUME { get; set; }
        public bool CONTRACT_FREE_MOONS_ONLY { get; set; }
        public string DOOR_HYDRAULICS_BATTERY_PRICES { get; set; }
        public float DOOR_HYDRAULICS_BATTERY_INITIAL { get; set; }
        public float DOOR_HYDRAULICS_BATTERY_INCREMENTAL {  get; set; }
        public bool DATA_CONTRACT {  get; set; }
        public bool EXTERMINATOR_CONTRACT {  get; set; }
        public bool EXORCISM_CONTRACT {  get; set; }
        public bool EXTRACTION_CONTRACT {  get; set; }
        public bool DEFUSAL_CONTRACT {  get; set; }
        public bool MAIN_OBJECT_FURTHEST {  get; set; }


        public PluginConfig(ConfigFile cfg)
        {
            configFile = cfg;
        }

        private T ConfigEntry<T>(string section, string key, T defaultVal, string description = "")
        {
            return configFile.Bind(section, key, defaultVal, description).Value;
        }

        public void InitBindings()
        {
            string topSection = "Contracts";
            CONTRACTS_ENABLED = ConfigEntry(topSection, "Enable the ability to purchase contracts / missions", true, "");
            CONTRACT_FREE_MOONS_ONLY = ConfigEntry(topSection, "Random contracts on free moons only", true, "If true, \"contract\" command will only generate contracts on free moons.");
            CONTRACT_PRICE = ConfigEntry(topSection, "Price of a random contract", 500, "");
            CONTRACT_SPECIFY_PRICE = ConfigEntry(topSection, "Price of a specified moon contract", 750, "");
            CONTRACT_BUG_REWARD = ConfigEntry(topSection, "Value of an exterminator contract reward", 500, "");
            CONTRACT_EXOR_REWARD = ConfigEntry(topSection, "Value of an exorcism contract reward", 500, "");
            CONTRACT_DEFUSE_REWARD = ConfigEntry(topSection, "Value of an defusal contract reward", 500, "");
            CONTRACT_EXTRACT_REWARD = ConfigEntry(topSection, "Value of an extraction contract reward", 500, "");
            CONTRACT_DATA_REWARD = ConfigEntry(topSection, "Value of a data contract reward", 500, "");
            CONTRACT_BUG_SPAWNS = ConfigEntry(topSection, "Hoarder Bug Spawn Number", 20, "How many bugs to spawn during exterminator contracts.");
            CONTRACT_GHOST_SPAWN = ConfigEntry(topSection, "Dress Girl / Thumper Spawn Number", 3, "How many ghosts/thumpers to spawn when failing exorcism contracts");
            CONTRACT_EXTRACT_WEIGHT = ConfigEntry(topSection,"Weight of an extraction human", 2.5f, "Subtract 1 and multiply by 100 (2.5 = 150lbs).");
            SCAV_VOLUME = ConfigEntry(topSection,"Volume of the scavenger voice clips", 0.25f, "0.0 - 1.0");
            MAIN_OBJECT_FURTHEST = ConfigEntry(topSection, "Spawn main object far away", true, "If true the main object for contracts will try spawn as far away from the main entrance as possible. If false it will spawn at a random location.");

            // this is kind of dumb and I'd like to just use a comma seperated string but this is much more foolproof
            DATA_CONTRACT = ConfigEntry(topSection,"Enable the data contract", true, "Make this false if you don't want the data contract");
            EXTRACTION_CONTRACT = ConfigEntry(topSection,"Enable the extraction contract", true, "Make this false if you don't want the extraction contract");
            EXORCISM_CONTRACT = ConfigEntry(topSection,"Enable the exorcism contract", true, "Make this false if you don't want the exorcism contract");
            DEFUSAL_CONTRACT = ConfigEntry(topSection,"Enable the defusal contract", true, "Make this false if you don't want the defusal contract");
            EXTERMINATOR_CONTRACT = ConfigEntry(topSection,"Enable the exterminator contract", true, "Make this false if you don't want the exterminator contract");
            
            topSection = "_Misc_";
            SHARED_UPGRADES = ConfigEntry(topSection, "Convert all upgrades to be shared.", true, "If true this will ignore the individual shared upgrade option for all other upgrades and set all upgrades to be shared.");
            SALE_PERC = ConfigEntry(topSection, "Chance of upgrades going on sale", 0.85f, "0.85 = 15% chance of an upgrade going on sale.");
            INTRO_ENABLED = ConfigEntry(topSection, "Intro Enabled", true, "If true shows a splashscreen with some info once per update of LGU.");
            KEEP_UPGRADES_AFTER_FIRED_CUTSCENE = ConfigEntry(topSection, "Keep upgrades after quota failure", false, "If true, you will keep your upgrades after being fired by The Company.");

            topSection = "Helmet";
            HELMET_ENABLED = ConfigEntry(topSection, "Enable the helmet for purchase", true, "");
            HELMET_PRICE = ConfigEntry(topSection, "Price of the helmet", 750, "");
            HELMET_HITS_BLOCKED = ConfigEntry(topSection, "Amount of hits blocked by helmet", 2, "");

            topSection = "Sick Beats";
            BEATS_ENABLED = ConfigEntry(topSection, "Enable Sick Beats Upgrade", true, "Get buffs from nearby active boomboxes.");
            BEATS_INDIVIDUAL = ConfigEntry(topSection, BaseUpgrade.INDIVIDUAL_SECTION, BaseUpgrade.INDIVIDUAL_DEFAULT, BaseUpgrade.INDIVIDUAL_DESCRIPTION);
            BEATS_PRICE = ConfigEntry(topSection, "Default unlock price", 500, "");
            BEATS_SPEED = ConfigEntry(topSection, "Enable Speed Boost Effect", true, "");
            BEATS_DMG = ConfigEntry(topSection, "Enable Damage Boost Effect", true, "");
            BEATS_STAMINA = ConfigEntry(topSection, "Enable Stamina Boost Effect", false, "");
            BEATS_DEF = ConfigEntry(topSection, "Enable Defense Boost Effect", false, "");
            BEATS_DEF_CO = ConfigEntry(topSection, "Defense Boost Coefficient", 0.5f, "Multiplied to incoming damage.");
            BEATS_STAMINA_CO = ConfigEntry(topSection, "Stamina Regen Coefficient", 1.25f, "Multiplied to stamina regen.");
            BEATS_DMG_INC = ConfigEntry(topSection, "Additional Damage Dealt", 1, "");
            BEATS_SPEED_INC = ConfigEntry(topSection, "Speed Boost Addition", 1.5f, "");
            BEATS_RADIUS = ConfigEntry(topSection, "Effect Radius", 15f, "Radius in unity units players will be effected by an active boombox.");

            topSection = "Advanced Portable Teleporter";
            ADVANCED_TELE_ENABLED = ConfigEntry(topSection, "Enable Advanced Portable Teleporter", true, "");
            ADVANCED_TELE_PRICE = ConfigEntry(topSection, "Price of Advanced Portable Teleporter", 1750, "");
            ADV_CHANCE_TO_BREAK = ConfigEntry(topSection, "Chance to break on use", 0.1f, "value should be 0.00 - 1.00");
            ADV_KEEP_ITEMS_ON_TELE = ConfigEntry(topSection,"Keep Items When Using Advanced Portable Teleporters", true, "If set to false you will drop your items like when using the vanilla TP.");

            topSection = "Portable Teleporter";
            WEAK_TELE_ENABLED = ConfigEntry(topSection, "Enable Portable Teleporter", true, "");
            WEAK_TELE_PRICE = ConfigEntry(topSection, "Price of Portable Teleporter", 300, "");
            CHANCE_TO_BREAK = ConfigEntry(topSection, "Chance to break on use", 0.9f, "value should be 0.00 - 1.00");
            KEEP_ITEMS_ON_TELE = ConfigEntry(topSection,"Keep Items When Using Weak Portable Teleporters", true, "If set to false you will drop your items like when using the vanilla TP.");

            topSection = beekeeperScript.UPGRADE_NAME;
            BEEKEEPER_ENABLED = ConfigEntry(topSection, "Enable Beekeeper Upgrade", true, "Take less damage from bees");
            BEEKEEPER_PRICE = ConfigEntry(topSection, "Price of Beekeeper Upgrade", 450, "");
            BEEKEEPER_DAMAGE_MULTIPLIER = ConfigEntry(topSection, "Multiplied to incoming damage (rounded to int)", 0.64f, "Incoming damage from bees is 10.");
            BEEKEEPER_UPGRADE_PRICES = ConfigEntry(topSection, BaseUpgrade.PRICES_SECTION, beekeeperScript.PRICES_DEFAULT, BaseUpgrade.PRICES_DESCRIPTION);
            BEEKEEPER_INDIVIDUAL = ConfigEntry(topSection, BaseUpgrade.INDIVIDUAL_SECTION, BaseUpgrade.INDIVIDUAL_DEFAULT, BaseUpgrade.INDIVIDUAL_DESCRIPTION);
            BEEKEEPER_DAMAGE_MULTIPLIER_INCREMENT = ConfigEntry(topSection, "Additional % Reduced per level", 0.15f, "Every time beekeeper is upgraded this value will be subtracted to the base multiplier above.");
            BEEKEEPER_HIVE_VALUE_INCREASE = ConfigEntry(topSection, "Hive value increase multiplier", 1.5f, "Multiplier applied to the value of beehive when reached max level");

            topSection = proteinPowderScript.UPGRADE_NAME;
            PROTEIN_ENABLED = ConfigEntry(topSection, proteinPowderScript.ENABLED_SECTION, proteinPowderScript.ENABLED_DEFAULT, proteinPowderScript.ENABLED_DESCRIPTION);
            PROTEIN_PRICE = ConfigEntry(topSection, proteinPowderScript.PRICE_SECTION, proteinPowderScript.PRICE_DEFAULT, "");
            PROTEIN_UNLOCK_FORCE = ConfigEntry(topSection, proteinPowderScript.UNLOCK_FORCE_SECTION, proteinPowderScript.UNLOCK_FORCE_DEFAULT, proteinPowderScript.UNLOCK_FORCE_DESCRIPTION);
            PROTEIN_INCREMENT = ConfigEntry(topSection, proteinPowderScript.INCREMENT_FORCE_SECTION, proteinPowderScript.INCREMENT_FORCE_DEFAULT, proteinPowderScript.INCREMENT_FORCE_DESCRIPTION);
            PROTEIN_INDIVIDUAL = ConfigEntry(topSection, BaseUpgrade.INDIVIDUAL_SECTION, BaseUpgrade.INDIVIDUAL_DEFAULT, BaseUpgrade.INDIVIDUAL_DESCRIPTION);
            PROTEIN_UPGRADE_PRICES = ConfigEntry(topSection, BaseUpgrade.PRICES_SECTION, proteinPowderScript.PRICES_DEFAULT, BaseUpgrade.PRICES_DESCRIPTION);
            PROTEIN_CRIT_CHANCE = ConfigEntry(topSection, proteinPowderScript.CRIT_CHANCE_SECTION, proteinPowderScript.CRIT_CHANCE_DEFAULT, proteinPowderScript.CRIT_CHANCE_DESCRIPTION);

            topSection = biggerLungScript.UPGRADE_NAME;
            BIGGER_LUNGS_ENABLED = ConfigEntry(topSection, "Enable Bigger Lungs Upgrade", true, "More Stamina");
            BIGGER_LUNGS_PRICE = ConfigEntry(topSection, "Price of Bigger Lungs Upgrade", 600, "");
            SPRINT_TIME_INCREASE_UNLOCK = ConfigEntry(topSection, "Sprint Time Unlock", 6f, "Amount of sprint time gained when unlocking the upgrade.\nDefault vanilla value is 11f.");
            SPRINT_TIME_INCREMENT = ConfigEntry(topSection, "Sprint Time Increment", 1.25f,"Amount of sprint time gained when increasing the level of upgrade.");
            BIGGER_LUNGS_UPGRADE_PRICES = ConfigEntry(topSection, BaseUpgrade.PRICES_SECTION, biggerLungScript.PRICES_DEFAULT, BaseUpgrade.PRICES_DESCRIPTION);
            BIGGER_LUNGS_INDIVIDUAL = ConfigEntry(topSection, BaseUpgrade.INDIVIDUAL_SECTION, BaseUpgrade.INDIVIDUAL_DEFAULT, BaseUpgrade.INDIVIDUAL_DESCRIPTION);
            BIGGER_LUNGS_STAMINA_REGEN_INCREASE = ConfigEntry(topSection, "Stamina Regeneration Increase", 1.05f, "Increase of stamina regeneration applied past level 1");
            BIGGER_LUNGS_JUMP_STAMINA_COST_DECREASE = ConfigEntry(topSection, "Stamina cost decrease on jumps", 0.90f, "Multiplied with the vanilla cost of jumping");

            topSection = "Extend Deadline";
            EXTEND_DEADLINE_ENABLED = ConfigEntry(topSection, "Enable Extend Deadline Purchase", true, "Increments the amount of days before deadline is reached.");
            EXTEND_DEADLINE_PRICE = ConfigEntry(topSection, "Extend Deadline Price", 800, "Price of each day extension requested in the terminal.");

            topSection = runningShoeScript.UPGRADE_NAME;
            RUNNING_SHOES_ENABLED = ConfigEntry(topSection, "Enable Running Shoes Upgrade", true, "Run Faster");
            RUNNING_SHOES_PRICE = ConfigEntry(topSection, "Price of Running Shoes Upgrade", 650, "");
            MOVEMENT_SPEED_UNLOCK = ConfigEntry(topSection, "Movement Speed Unlock", 1.4f, "Value added to player's movement speed when first purchased.\nDefault vanilla value is 4.6f.");
            MOVEMENT_INCREMENT = ConfigEntry(topSection, "Movement Speed Increment", 0.5f, "How much the above value is increased on upgrade.");
            RUNNING_SHOES_UPGRADE_PRICES = ConfigEntry(topSection, BaseUpgrade.PRICES_SECTION, biggerLungScript.PRICES_DEFAULT, BaseUpgrade.PRICES_DESCRIPTION);
            RUNNING_SHOES_INDIVIDUAL = ConfigEntry(topSection, BaseUpgrade.INDIVIDUAL_SECTION, BaseUpgrade.INDIVIDUAL_DEFAULT, BaseUpgrade.INDIVIDUAL_DESCRIPTION);
            NOISE_REDUCTION = ConfigEntry(topSection, "Noise Reduction", 10f, "Distance units to subtract from footstep noise when reached final level.");

            topSection = strongLegsScript.UPGRADE_NAME;
            STRONG_LEGS_ENABLED = ConfigEntry(topSection, "Enable Strong Legs Upgrade", true, "Jump Higher");
            STRONG_LEGS_PRICE = ConfigEntry(topSection, "Price of Strong Legs Upgrade", 300, "");
            JUMP_FORCE_UNLOCK = ConfigEntry(topSection, "Jump Force Unlock", 3f, "Amount of jump force added when unlocking the upgrade.\nDefault vanilla value is 13f.");
            JUMP_FORCE_INCREMENT = ConfigEntry(topSection, "Jump Force Increment", 0.75f, "How much the above value is increased on upgrade.");
            STRONG_LEGS_UPGRADE_PRICES = ConfigEntry(topSection, BaseUpgrade.PRICES_SECTION, strongLegsScript.PRICES_DEFAULT, BaseUpgrade.PRICES_DESCRIPTION);
            STRONG_LEGS_INDIVIDUAL = ConfigEntry(topSection, BaseUpgrade.INDIVIDUAL_SECTION, BaseUpgrade.INDIVIDUAL_DEFAULT, BaseUpgrade.INDIVIDUAL_DESCRIPTION);
            STRONG_LEGS_REDUCE_FALL_DAMAGE_MULTIPLIER = ConfigEntry(topSection, "Damage mitigation when falling", 0.5f, "Multiplier applied on fall damage that you wish to ignore when reached max level");

            topSection = trapDestroyerScript.UPGRADE_NAME;
            MALWARE_BROADCASTER_ENABLED = ConfigEntry(topSection, "Enable Malware Broadcaster Upgrade", true, "Explode Map Hazards");
            MALWARE_BROADCASTER_PRICE = ConfigEntry(topSection, "Price of Malware Broadcaster Upgrade", 550, "");
            DESTROY_TRAP = ConfigEntry(topSection, "Destroy Trap", true, "If false Malware Broadcaster will disable the trap for a long time instead of destroying.");
            DISARM_TIME = ConfigEntry(topSection, "Disarm Time", 7f, "If `Destroy Trap` is false this is the duration traps will be disabled.");
            EXPLODE_TRAP = ConfigEntry(topSection, "Explode Trap", true, "Destroy Trap must be true! If this is true when destroying a trap it will also explode.");
            MALWARE_BROADCASTER_INDIVIDUAL = ConfigEntry(topSection, BaseUpgrade.INDIVIDUAL_SECTION, BaseUpgrade.INDIVIDUAL_DEFAULT, BaseUpgrade.INDIVIDUAL_DESCRIPTION);

            topSection = "Night Vision";
            NIGHT_VISION_ENABLED = ConfigEntry(topSection, "Enable Night Vision Upgrade", true, "Toggleable night vision.");
            NIGHT_VISION_PRICE = ConfigEntry(topSection, "Price of Night Vision Upgrade", 380, "");
            NIGHT_BATTERY_MAX = ConfigEntry(topSection, "The max charge for your night vision battery", 10f, "Default settings this will be the unupgraded time in seconds the battery will drain and regen in. Increase to increase battery life.");
            NIGHT_VIS_DRAIN_SPEED = ConfigEntry(topSection, "Multiplier for night vis battery drain", 1f, "Multiplied by timedelta, lower to increase battery life.");
            NIGHT_VIS_REGEN_SPEED = ConfigEntry(topSection, "Multiplier for night vis battery regen", 1f, "Multiplied by timedelta, raise to speed up battery regen time.");
            NIGHT_VIS_COLOR = ConfigEntry(topSection, "Night Vision Color", UnityEngine.Color.green, "The color your night vision light emits.");
            NIGHT_VIS_RANGE = ConfigEntry(topSection, "Night Vision Range", 2000f, "Kind of like the distance your night vision travels.");
            NIGHT_VIS_RANGE_INCREMENT = ConfigEntry(topSection, "Night Vision Range Increment", 0f, "Increases your range by this value each upgrade.");
            NIGHT_VIS_INTENSITY = ConfigEntry(topSection, "Night Vision Intensity", 1000f, "Kind of like the brightness of your Night Vision.");
            NIGHT_VIS_INTENSITY_INCREMENT = ConfigEntry(topSection, "Night Vision Intensity Increment", 0f, "Increases your intensity by this value each upgrade.");
            NIGHT_VIS_STARTUP = ConfigEntry(topSection, "Night Vision StartUp Cost", 0.1f, "The percent battery drained when turned on (0.1 = 10%).");
            NIGHT_VIS_EXHAUST = ConfigEntry(topSection, "Night Vision Exhaustion", 2f, "How many seconds night vision stays fully depleted.");
            TOGGLE_NIGHT_VISION_KEY = ConfigEntry(topSection, "Toggle Night Vision Key", "LeftAlt", "Key to toggle Night Vision, you can use any key on your system such as LeftAlt, LeftShift, or any letter which exists.");
            NIGHT_VIS_DRAIN_INCREMENT = ConfigEntry(topSection, "Decrease for night vis battery drain", 0.15f, "Applied to drain speed on each upgrade.");
            NIGHT_VIS_REGEN_INCREMENT = ConfigEntry(topSection, "Increase for night vis battery regen", 0.40f, "Applied to regen speed on each upgrade.");
            NIGHT_VIS_BATTERY_INCREMENT = ConfigEntry(topSection, "Increase for night vis battery life", 2f, "Applied to the max charge for night vis battery on each upgrade.");
            LOSE_NIGHT_VIS_ON_DEATH = ConfigEntry(topSection, "Lose Night Vision On Death", true, "If true when you die the night vision will disable and will need a new pair of goggles.");
            NIGHT_VISION_DROP_ON_DEATH = ConfigEntry(topSection, "Drop Night Vision item on Death", true, "If true, when you die and lose night vision upon death, you will drop the night vision goggles on your body.");
            NIGHT_VISION_UPGRADE_PRICES = ConfigEntry(topSection,BaseUpgrade.PRICES_SECTION, nightVisionScript.PRICES_DEFAULT, BaseUpgrade.PRICES_DESCRIPTION);
            NIGHT_VISION_INDIVIDUAL = ConfigEntry(topSection,BaseUpgrade.INDIVIDUAL_SECTION, BaseUpgrade.INDIVIDUAL_DEFAULT, BaseUpgrade.INDIVIDUAL_DESCRIPTION);

            topSection = terminalFlashScript.UPGRADE_NAME;
            DISCOMBOBULATOR_ENABLED = ConfigEntry(topSection, "Enable Discombobulator Upgrade", true, "Stun enemies around the ship.");
            DISCOMBOBULATOR_PRICE = ConfigEntry(topSection, "Price of Discombobulator Upgrade", 450, "");
            DISCOMBOBULATOR_COOLDOWN = ConfigEntry(topSection, "Discombobulator Cooldown", 120f, "");
            DISCOMBOBULATOR_RADIUS  = ConfigEntry(topSection, "Discombobulator Effect Radius", 40f, "");
            DISCOMBOBULATOR_STUN_DURATION  = ConfigEntry(topSection, "Discombobulator Stun Duration", 7.5f, "");
            DISCOMBOBULATOR_NOTIFY_CHAT = ConfigEntry(topSection, "Notify Local Chat of Enemy Stun Duration", true, "");
            DISCOMBOBULATOR_INCREMENT  = ConfigEntry(topSection, "Discombobulator Increment", 1f, "The amount added to stun duration on upgrade.");
            DISCO_UPGRADE_PRICES = ConfigEntry(topSection, BaseUpgrade.PRICES_SECTION, terminalFlashScript.PRICES_DEFAULT, BaseUpgrade.PRICES_DESCRIPTION);
            DISCOMBOBULATOR_INDIVIDUAL = ConfigEntry(topSection, BaseUpgrade.INDIVIDUAL_SECTION, BaseUpgrade.INDIVIDUAL_DEFAULT, BaseUpgrade.INDIVIDUAL_DESCRIPTION);
            DISCOMBOBULATOR_DAMAGE_LEVEL = ConfigEntry(topSection, "Initial level of dealing damage", 2, "Level of Discombobulator in which it starts dealing damage on enemies it stuns");
            DISCOMBOBULATOR_INITIAL_DAMAGE = ConfigEntry(topSection, "Initial amount of damage", 2, "Amount of damage when reaching the first level that unlocks damage on stun\nTo give an idea of what's too much, Eyeless Dogs have 12 health.");
            DISCOMBOBULATOR_DAMAGE_INCREASE = ConfigEntry(topSection, "Damage Increase on Purchase", 1, "Damage increase when purchasing later levels");

            topSection = strongerScannerScript.UPGRADE_NAME;
            BETTER_SCANNER_ENABLED = ConfigEntry(topSection, "Enable Better Scanner Upgrade", true, "Further scan distance, no LOS needed.");
            BETTER_SCANNER_PRICE = ConfigEntry(topSection, "Price of Better Scanner Upgrade", 650, "");
            SHIP_AND_ENTRANCE_DISTANCE_INCREASE = ConfigEntry(topSection, "Ship and Entrance node distance boost", 150f, "How much further away you can scan the ship and entrance.");
            NODE_DISTANCE_INCREASE = ConfigEntry(topSection, "Node distance boost", 20f, "How much further away you can scan other nodes.");
            BETTER_SCANNER_INDIVIDUAL = ConfigEntry(topSection, BaseUpgrade.INDIVIDUAL_SECTION, BaseUpgrade.INDIVIDUAL_DEFAULT, BaseUpgrade.INDIVIDUAL_DESCRIPTION);
            BETTER_SCANNER_PRICE2 = ConfigEntry(topSection, "Price of first Better Scanner tier", 500, "This tier unlocks ship scan commands.");
            BETTER_SCANNER_PRICE3 = ConfigEntry(topSection, "Price of second Better Scanner tier", 800, "This tier unlocks scanning through walls.");
            BETTER_SCANNER_ENEMIES = ConfigEntry(topSection, "Scan enemies through walls on final upgrade", false, "If true the final upgrade will scan scrap AND enemies through walls.");
            VERBOSE_ENEMIES = ConfigEntry(topSection, "Verbose `scan enemies` command", true, "If false `scan enemies` only returns a count of outside and inside enemies, else it returns the count for each enemy type.");

            topSection = exoskeletonScript.UPGRADE_NAME;
            BACK_MUSCLES_ENABLED = ConfigEntry(topSection, "Enable Back Muscles Upgrade", true, "Reduce carry weight");
            BACK_MUSCLES_PRICE = ConfigEntry(topSection, "Price of Back Muscles Upgrade", 715, "");
            CARRY_WEIGHT_REDUCTION = ConfigEntry(topSection, "Carry Weight Multiplier", 0.5f, "Your carry weight is multiplied by this.");
            CARRY_WEIGHT_INCREMENT = ConfigEntry(topSection, "Carry Weight Increment", 0.1f, "Each upgrade subtracts this from the above coefficient.");
            BACK_MUSCLES_UPGRADE_PRICES = ConfigEntry(topSection, BaseUpgrade.PRICES_SECTION, exoskeletonScript.PRICES_DEFAULT, BaseUpgrade.PRICES_DESCRIPTION);
            BACK_MUSCLES_INDIVIDUAL = ConfigEntry(topSection,BaseUpgrade.INDIVIDUAL_SECTION, BaseUpgrade.INDIVIDUAL_DEFAULT, BaseUpgrade.INDIVIDUAL_DESCRIPTION);

            topSection = "Interns";
            INTERN_ENABLED = ConfigEntry(topSection, "Enable hiring of interns", true, "Pay x amount of credits to revive a player.");
            INTERN_PRICE = ConfigEntry(topSection, "Intern Price", 1000, "Default price to hire an intern.");
            INTERN_INDIVIDUAL = ConfigEntry(topSection, BaseUpgrade.INDIVIDUAL_SECTION, BaseUpgrade.INDIVIDUAL_DEFAULT, BaseUpgrade.INDIVIDUAL_DESCRIPTION);

            topSection = pagerScript.UPGRADE_NAME;
            PAGER_ENABLED = ConfigEntry(topSection, "Enable Fast Encryption", true, "Upgrades the transmitter.");
            PAGER_PRICE = ConfigEntry(topSection, "Fast Encryption Price", 300, "");

            topSection = lockSmithScript.UPGRADE_NAME;
            LOCKSMITH_ENABLED = ConfigEntry(topSection, "Enable Locksmith upgrade", true, "Allows you to pick locked doors by completing a minigame.");
            LOCKSMITH_PRICE = ConfigEntry(topSection, "Locksmith Price", 640, "Default price of Locksmith upgrade.");
            LOCKSMITH_INDIVIDUAL = ConfigEntry(topSection, BaseUpgrade.INDIVIDUAL_SECTION, BaseUpgrade.INDIVIDUAL_DEFAULT, BaseUpgrade.INDIVIDUAL_DESCRIPTION);

            topSection = "Peeper";
            PEEPER_ENABLED = ConfigEntry(topSection, "Enable Peeper item", true, "An item that will stare at coilheads for you.");
            PEEPER_PRICE = ConfigEntry(topSection, "Peeper Price", 500, "Default price to purchase a Peeper.");

            topSection = lightningRodScript.UPGRADE_NAME;
            LIGHTNING_ROD_ENABLED = ConfigEntry(topSection, lightningRodScript.ENABLED_SECTION, lightningRodScript.ENABLED_DEFAULT, lightningRodScript.ENABLED_DESCRIPTION);
            LIGHTNING_ROD_PRICE = ConfigEntry(topSection, lightningRodScript.PRICE_SECTION, lightningRodScript.PRICE_DEFAULT, "");
            LIGHTNING_ROD_ACTIVE = ConfigEntry(topSection, lightningRodScript.ACTIVE_SECTION, lightningRodScript.ACTIVE_DEFAULT, lightningRodScript.ACTIVE_DESCRIPTION);
            LIGHTNING_ROD_DIST = ConfigEntry(topSection, lightningRodScript.DIST_SECTION, lightningRodScript.DIST_DEFAULT, lightningRodScript.DIST_DESCRIPTION);

            topSection = "Walkie";
            WALKIE_ENABLED = ConfigEntry(topSection, "Enable the walkie talkie gps upgrade", true, "Holding a walkie talkie displays location.");
            WALKIE_PRICE = ConfigEntry(topSection, "Walkie GPS Price", 450, "Default price for upgrade.");
            WALKIE_INDIVIDUAL = ConfigEntry(topSection, BaseUpgrade.INDIVIDUAL_SECTION, BaseUpgrade.INDIVIDUAL_DEFAULT, BaseUpgrade.INDIVIDUAL_DESCRIPTION);

            topSection = hunterScript.UPGRADE_NAME;
            HUNTER_ENABLED = ConfigEntry(topSection, "Enable the Hunter upgrade", true, "Collect and sell samples from dead enemies");
            HUNTER_PRICE = ConfigEntry(topSection, "Hunter price", 700, "Default price for upgrade.");
            HUNTER_UPGRADE_PRICES = ConfigEntry(topSection, BaseUpgrade.PRICES_SECTION, "500,600", BaseUpgrade.PRICES_DESCRIPTION);
            HUNTER_SAMPLE_TIERS = ConfigEntry(topSection,
                                            "Samples dropping at each tier",
                                            "Hoarding Bug, Centipede-Bunker Spider, Baboon hawk-Flowerman, MouthDog, Crawler",
                                            "Specifies at which tier of Hunter do each sample start dropping from. Each tier is separated with a dash ('-') and each list of monsters will be separated with a comma (',')\nSupported Enemies: Hoarding Bug, Centipede (Snare Flea),Bunker Spider, Baboon Hawk, Crawler (Half/Thumper), Flowerman (Bracken) and MouthDog (Eyeless Dog)");
            SNARE_FLEA_SAMPLE_MINIMUM_VALUE = ConfigEntry(topSection, "Minimum scrap value of a Snare Flea sample", 35);
            SNARE_FLEA_SAMPLE_MAXIMUM_VALUE = ConfigEntry(topSection, "Maximum scrap value of a Snare Flea sample", 60);
            BUNKER_SPIDER_SAMPLE_MINIMUM_VALUE = ConfigEntry(topSection, "Minimum scrap value of a Bunker Spider sample", 65);
            BUNKER_SPIDER_SAMPLE_MAXIMUM_VALUE = ConfigEntry(topSection, "Maximum scrap value of a Bunker Spider sample", 95);
            HOARDING_BUG_SAMPLE_MINIMUM_VALUE = ConfigEntry(topSection, "Minimum scrap value of a Hoarding Bug sample", 45);
            HOARDING_BUG_SAMPLE_MAXIMUM_VALUE = ConfigEntry(topSection, "Maximum scrap value of a Hoarding Bug sample", 75);
            BRACKEN_SAMPLE_MINIMUM_VALUE = ConfigEntry(topSection, "Minimum scrap value of a Bracken sample", 80);
            BRACKEN_SAMPLE_MAXIMUM_VALUE = ConfigEntry(topSection, "Maximum scrap value of a Bracken sample", 125);
            EYELESS_DOG_SAMPLE_MINIMUM_VALUE = ConfigEntry(topSection, "Minimum scrap value of a Eyeless Dog sample", 100);
            EYELESS_DOG_SAMPLE_MAXIMUM_VALUE = ConfigEntry(topSection, "Maximum scrap value of a Eyeless Dog sample", 150);
            BABOON_HAWK_SAMPLE_MINIMUM_VALUE = ConfigEntry(topSection, "Minimum scrap value of a Baboon Hawk sample", 75);
            BABOON_HAWK_SAMPLE_MAXIMUM_VALUE = ConfigEntry(topSection, "Maximum scrap value of a Baboon Hawk sample", 115);
            THUMPER_SAMPLE_MINIMUM_VALUE = ConfigEntry(topSection, "Minimum scrap value of a Half sample", 80);
            THUMPER_SAMPLE_MAXIMUM_VALUE = ConfigEntry(topSection, "Maximum scrap value of a Half sample", 125);

            topSection = playerHealthScript.UPGRADE_NAME;
            PLAYER_HEALTH_ENABLED = ConfigEntry(topSection, playerHealthScript.ENABLED_SECTION, playerHealthScript.ENABLED_DEFAULT, playerHealthScript.ENABLED_DESCRIPTION);
            PLAYER_HEALTH_PRICE = ConfigEntry(topSection, playerHealthScript.PRICE_SECTION, playerHealthScript.PRICE_DEFAULT, "");
            PLAYER_HEALTH_INDIVIDUAL = ConfigEntry(topSection, BaseUpgrade.INDIVIDUAL_SECTION, BaseUpgrade.INDIVIDUAL_DEFAULT, BaseUpgrade.INDIVIDUAL_DESCRIPTION);
            PLAYER_HEALTH_UPGRADE_PRICES = ConfigEntry(topSection, BaseUpgrade.PRICES_SECTION, playerHealthScript.PRICES_DEFAULT, BaseUpgrade.PRICES_DESCRIPTION);
            PLAYER_HEALTH_ADDITIONAL_HEALTH_UNLOCK = ConfigEntry(topSection, playerHealthScript.ADDITIONAL_HEALTH_UNLOCK_SECTION, playerHealthScript.ADDITIONAL_HEALTH_UNLOCK_DEFAULT, playerHealthScript.ADDITIONAL_HEALTH_UNLOCK_DESCRIPTION);
            PLAYER_HEALTH_ADDITIONAL_HEALTH_INCREMENT = ConfigEntry(topSection, playerHealthScript.ADDITIONAL_HEALTH_INCREMENT_SECTION, playerHealthScript.ADDITIONAL_HEALTH_INCREMENT_DEFAULT, playerHealthScript.ADDITIONAL_HEALTH_INCREMENT_DESCRIPTION);

            topSection = "Medkit";
            MEDKIT_ENABLED = ConfigEntry(topSection, "Enable the medkit item", true, "Allows you to buy a medkit to heal yourself.");
            MEDKIT_PRICE = ConfigEntry(topSection, "price", 300, "Default price for Medkit.");
            MEDKIT_HEAL_VALUE = ConfigEntry(topSection, "Heal Amount", 20, "The amount the medkit heals you.");
            MEDKIT_USES = ConfigEntry(topSection, "Uses", 3, "The amount of times the medkit can heal you.");

            topSection = "Diving Kit";
            DIVEKIT_ENABLED = ConfigEntry(topSection, "Enable the Diving Kit Item", true, "Allows you to buy a diving kit to breathe underwater.");
            DIVEKIT_PRICE = ConfigEntry(topSection, "price", 650, "Price for Diving Kit.");
            DIVEKIT_WEIGHT = ConfigEntry(topSection, "Item weight", 1.65f, "-1 and multiply by 100 (1.65 = 65 lbs)");
            DIVEKIT_TWO_HANDED = ConfigEntry(topSection, "Two Handed Item", true, "One or two handed item.");

            topSection = DoorsHydraulicsBattery.UPGRADE_NAME;
            DOOR_HYDRAULICS_BATTERY_ENABLED = ConfigEntry(topSection, DoorsHydraulicsBattery.ENABLED_SECTION, true, DoorsHydraulicsBattery.ENABLED_DESCRIPTION);
            DOOR_HYDRAULICS_BATTERY_PRICE = ConfigEntry(topSection, DoorsHydraulicsBattery.PRICE_SECTION, DoorsHydraulicsBattery.PRICE_DEFAULT);
            DOOR_HYDRAULICS_BATTERY_PRICES = ConfigEntry(topSection, BaseUpgrade.PRICES_SECTION, DoorsHydraulicsBattery.PRICES_DEFAULT, BaseUpgrade.PRICES_DESCRIPTION);
            DOOR_HYDRAULICS_BATTERY_INITIAL = ConfigEntry(topSection, DoorsHydraulicsBattery.INITIAL_SECTION, DoorsHydraulicsBattery.INITIAL_DEFAULT, DoorsHydraulicsBattery.INITIAL_DESCRIPTION);
            DOOR_HYDRAULICS_BATTERY_INCREMENTAL = ConfigEntry(topSection, DoorsHydraulicsBattery.INCREMENTAL_SECTION, DoorsHydraulicsBattery.INCREMENTAL_DEFAULT, DoorsHydraulicsBattery.INCREMENTAL_DESCRIPTION);
          
            topSection = ScrapInsurance.COMMAND_NAME;
            SCRAP_INSURANCE_ENABLED = ConfigEntry(topSection, "Enable Scrap Insurance Command", true, "One time purchase which allows you to keep all your scrap upon a team wipe on a moon trip");
            SCRAP_INSURANCE_PRICE = ConfigEntry(topSection, "Price of Scrap Insurance", ScrapInsurance.DEFAULT_PRICE);

            topSection = "Wheelbarrow";
            WHEELBARROW_ENABLED = ConfigEntry(topSection, "Enable the Wheelbarrow Item", true, "Allows you to buy a wheelbarrow to carry items outside of your inventory");
            WHEELBARROW_PRICE = ConfigEntry(topSection, "Price of the Wheelbarrow Item", 400, "Price of the Wheelbarrow in the store");
            WHEELBARROW_WEIGHT = ConfigEntry(topSection, "Weight of the Wheelbarrow Item", 30f, "Weight of the wheelbarrow without any items in lbs");
            WHEELBARROW_RESTRICTION_MODE = ConfigEntry(topSection, "Restrictions on the Wheelbarrow Item", "ItemCount", "Restriction applied when trying to insert an item on the wheelbarrow.\nSupported values: None, ItemCount, TotalWeight, All");
            WHEELBARROW_MAXIMUM_WEIGHT_ALLOWED = ConfigEntry(topSection, "Maximum amount of weight", 100f, "How much weight (in lbs) a wheelbarrow can carry in items before it is considered full.");
            WHEELBARROW_MAXIMUM_AMOUNT_ITEMS = ConfigEntry(topSection, "Maximum amount of items", 4, "Amount of items allowed before the wheelbarrow is considered full");
            WHEELBARROW_WEIGHT_REDUCTION_MULTIPLIER = ConfigEntry(topSection, "Weight reduction multiplier", 0.7f, "How much an item's weight will be ignored to the wheelbarrow's total weight");
            WHEELBARROW_NOISE_RANGE = ConfigEntry(topSection, "Noise range of the Wheelbarrow Item", 14f, "How far the wheelbarrow sound propagates to nearby enemies when in movement");
            WHEELBARROW_LOOK_SENSITIVITY_DRAWBACK = ConfigEntry(topSection, "Look sensitivity drawback of the Wheelbarrow Item", 0.4f, "Value multiplied on the player's look sensitivity when moving with the wheelbarrow Item");
            WHEELBARROW_MOVEMENT_SLOPPY = ConfigEntry(topSection, "Sloppiness of the Wheelbarrow Item", 5f, "Value multiplied on the player's movement to give the feeling of drifting while carrying the Wheelbarrow Item");
            WHEELBARROW_PLAY_NOISE = ConfigEntry(topSection, "Plays noises for players with Wheelbarrow Item", true, "If false, it will just not play the sounds, it will still attract monsters to noise");
            SCRAP_WHEELBARROW_ENABLED = ConfigEntry(topSection, "Enable the Shopping Cart Item", true, "Allows you to scavenge a wheelbarrow in which you can store items on");
            SCRAP_WHEELBARROW_RARITY = ConfigEntry(topSection, "Spawn chance of Shopping Cart Item", 0.1f, "How likely it is to a scrap wheelbarrow item to spawn when landing on a moon. (0.1 = 10%)");
            SCRAP_WHEELBARROW_WEIGHT = ConfigEntry(topSection, "Weight of the Shopping Cart Item", 25f, "Weight of the scrap wheelbarrow's without any items in lbs");
            SCRAP_WHEELBARROW_MAXIMUM_AMOUNT_ITEMS = ConfigEntry(topSection, "Maximum amount of items for Shopping Cart", 6, "Amount of items allowed before the scrap wheelbarrow is considered full");
            SCRAP_WHEELBARROW_WEIGHT_REDUCTION_MULTIPLIER = ConfigEntry(topSection, "Weight reduction multiplier for Shopping Cart", 0.5f, "How much an item's weight will be ignored to the scrap wheelbarrow's total weight");
            SCRAP_WHEELBARROW_MINIMUM_VALUE = ConfigEntry(topSection, "Minimum scrap value of Shopping Cart", 50, "Lower boundary of the scrap's possible value");
            SCRAP_WHEELBARROW_MAXIMUM_VALUE = ConfigEntry(topSection, "Maximum scrap value of Shopping Cart", 100, "Higher boundary of the scrap's possible value");
            SCRAP_WHEELBARROW_RESTRICTION_MODE = ConfigEntry(topSection, "Restrictions on the Shopping Cart Item", "ItemCount", "Restriction applied when trying to insert an item on the scrap wheelbarrow.\nSupported values: None, ItemCount, TotalWeight, All");
            SCRAP_WHEELBARROW_MAXIMUM_WEIGHT_ALLOWED = ConfigEntry(topSection, "Maximum amount of weight for Shopping Cart", 100f, "How much weight (in lbs) a scrap wheelbarrow can carry in items before it is considered full.");
            SCRAP_WHEELBARROW_NOISE_RANGE = ConfigEntry(topSection, "Noise range of the Shopping Cart Item", 18f, "How far the scrap wheelbarrow sound propagates to nearby enemies when in movement");
            SCRAP_WHEELBARROW_LOOK_SENSITIVITY_DRAWBACK = ConfigEntry(topSection, "Look sensitivity drawback of the Shopping Cart Item", 0.8f, "Value multiplied on the player's look sensitivity when moving with the Scrap wheelbarrow Item");
            SCRAP_WHEELBARROW_MOVEMENT_SLOPPY = ConfigEntry(topSection, "Sloppiness of the Shopping Cart Item", 2f, "Value multiplied on the player's movement to give the feeling of drifting while carrying the Scrap Wheelbarrow Item");
            SCRAP_WHEELBARROW_PLAY_NOISE = ConfigEntry(topSection, "Plays noises for players with Shopping Cart Item", true, "If false, it will just not play the sounds, it will still attract monsters to noise");
        }
    }
}
