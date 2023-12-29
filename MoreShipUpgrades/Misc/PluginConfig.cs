using BepInEx.Configuration;
using System;
using Newtonsoft.Json;
using MoreShipUpgrades.UpgradeComponents;


namespace MoreShipUpgrades.Misc
{
    [Serializable]
    public class PluginConfig
    {
        readonly ConfigFile configFile;

        // enabled disabled
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
        public bool PEEPER_ENABLED { get; set; }

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

        // attributes
        public float BIGGER_LUNGS_STAMINA_REGEN_INCREASE { get; set; }
        public float BIGGER_LUNGS_JUMP_STAMINA_COST_DECREASE { get; set; }
        public int PROTEIN_INCREMENT { get; set; }
        public int HUNTER_PRICE2 { get; set; }
        public int HUNTER_PRICE3 { get; set; }
        public bool KEEP_ITEMS_ON_TELE { get; set; }
        public float SPRINT_TIME_INCREASE { get; set; }
        public float MOVEMENT_SPEED { get; set; }
        public float JUMP_FORCE { get; set; }
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

        public PluginConfig(ConfigFile cfg)
        {
            configFile = cfg;
        }

        private T ConfigEntry<T>(string section, string key, T defaultVal, string description)
        {
            return configFile.Bind(section, key, defaultVal, description).Value;
        }

        public void InitBindings()
        {
            string topSection = "Misc";
            SHARED_UPGRADES = ConfigEntry(topSection, "Convert all upgrades to be shared.", true, "Mod is designed to be played with this off.");
            SALE_PERC = ConfigEntry(topSection, "Chance of upgrades going on sale", 0.85f, "0.85 = 15% chance of an upgrade going on sale.");
            INTRO_ENABLED = ConfigEntry(topSection, "Intro Enabled", true, "If true shows a splashscreen with some info once per update of LGU.");

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
            SPRINT_TIME_INCREASE = ConfigEntry(topSection, "SprintTime value", 17f, "Vanilla value is 11");
            SPRINT_TIME_INCREMENT = ConfigEntry(topSection, "SprintTime Increment", 1.25f,"How much the above value is increased on upgrade.");
            BIGGER_LUNGS_UPGRADE_PRICES = ConfigEntry(topSection, BaseUpgrade.PRICES_SECTION, biggerLungScript.PRICES_DEFAULT, BaseUpgrade.PRICES_DESCRIPTION);
            BIGGER_LUNGS_INDIVIDUAL = ConfigEntry(topSection, BaseUpgrade.INDIVIDUAL_SECTION, BaseUpgrade.INDIVIDUAL_DEFAULT, BaseUpgrade.INDIVIDUAL_DESCRIPTION);
            BIGGER_LUNGS_STAMINA_REGEN_INCREASE = ConfigEntry(topSection, "Stamina Regeneration Increase", 1.05f, "Increase of stamina regeneration applied past level 1");
            BIGGER_LUNGS_JUMP_STAMINA_COST_DECREASE = ConfigEntry(topSection, "Stamina cost decrease on jumps", 0.90f, "Multiplied with the vanilla cost of jumping");
          
            topSection = runningShoeScript.UPGRADE_NAME;
            RUNNING_SHOES_ENABLED = ConfigEntry(topSection, "Enable Running Shoes Upgrade", true, "Run Faster");
            RUNNING_SHOES_PRICE = ConfigEntry(topSection, "Price of Running Shoes Upgrade", 650, "");
            MOVEMENT_SPEED = ConfigEntry(topSection, "Movement Speed Value", 6f, "Vanilla value is 4.6");
            MOVEMENT_INCREMENT = ConfigEntry(topSection, "Movement Speed Increment", 0.5f, "How much the above value is increased on upgrade.");
            RUNNING_SHOES_UPGRADE_PRICES = ConfigEntry(topSection, BaseUpgrade.PRICES_SECTION, biggerLungScript.PRICES_DEFAULT, BaseUpgrade.PRICES_DESCRIPTION);
            RUNNING_SHOES_INDIVIDUAL = ConfigEntry(topSection, BaseUpgrade.INDIVIDUAL_SECTION, BaseUpgrade.INDIVIDUAL_DEFAULT, BaseUpgrade.INDIVIDUAL_DESCRIPTION);
            NOISE_REDUCTION = ConfigEntry(topSection, "Noise Reduction", 10f, "Distance units to subtract from footstep noise when reached final level.");

            topSection = strongLegsScript.UPGRADE_NAME;
            STRONG_LEGS_ENABLED = ConfigEntry(topSection, "Enable Strong Legs Upgrade", true, "Jump Higher");
            STRONG_LEGS_PRICE = ConfigEntry(topSection, "Price of Strong Legs Upgrade", 300, "");
            JUMP_FORCE = ConfigEntry(topSection, "Jump Force", 16f, "Vanilla value is 13");
            JUMP_FORCE_INCREMENT = ConfigEntry(topSection, "Jump Force Increment", 0.75f, "How much the above value is increased on upgrade.");
            STRONG_LEGS_UPGRADE_PRICES = ConfigEntry(topSection, BaseUpgrade.PRICES_SECTION, strongLegsScript.PRICES_DEFAULT, BaseUpgrade.PRICES_DESCRIPTION);
            STRONG_LEGS_INDIVIDUAL = ConfigEntry(topSection, BaseUpgrade.INDIVIDUAL_SECTION, BaseUpgrade.INDIVIDUAL_DEFAULT, BaseUpgrade.INDIVIDUAL_DESCRIPTION);

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
            LOSE_NIGHT_VIS_ON_DEATH = ConfigEntry(topSection, "Lose Night Vision On Death", true, "If true when you die you will have to re purchase and equip night vision goggles.");
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
            HUNTER_PRICE2 = ConfigEntry(topSection, "Second Hunter level price", 500, "");
            HUNTER_PRICE3 = ConfigEntry(topSection, "Third Hunter level price", 600, "");

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

        }
    }
}
