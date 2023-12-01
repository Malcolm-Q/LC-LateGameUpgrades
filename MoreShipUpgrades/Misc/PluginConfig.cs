using BepInEx.Configuration;
using UnityEngine;
using System.Drawing;

namespace MoreShipUpgrades.Misc
{
    public class PluginConfig
    {
        readonly ConfigFile configFile;
        
        // enabled disabled
        public bool ADVANCED_TELE_ENABLED { get; set; }
        public bool WEAK_TELE_ENABLED { get; set; }
        public bool BEEKEEPER_ENABLED { get; set; }
        public bool BIGGER_LUNGS_ENABLED { get; set; }
        public bool BACK_MUSCLES_ENABLED { get; set; }
        public bool LIGHT_FOOTED_ENABLED { get; set; }
        public bool NIGHT_VISION_ENABLED { get; set; }
        public bool RUNNING_SHOES_ENABLED { get; set; }
        public bool BETTER_SCANNER_ENABLED { get; set; }
        public bool STRONG_LEGS_ENABLED { get; set; }
        public bool DISCOMBOBULATOR_ENABLED { get; set; }
        public bool MALWARE_BROADCASTER_ENABLED { get; set; }

        // prices
        public int ADVANCED_TELE_PRICE { get; set; }
        public int WEAK_TELE_PRICE { get; set; }
        public int BEEKEEPER_PRICE { get; set; }
        public int BIGGER_LUNGS_PRICE { get; set; }
        public int BACK_MUSCLES_PRICE { get; set; }
        public int LIGHT_FOOTED_PRICE { get; set; }
        public int NIGHT_VISION_PRICE { get; set; }
        public int RUNNING_SHOES_PRICE { get; set; }
        public int BETTER_SCANNER_PRICE { get; set; }
        public int STRONG_LEGS_PRICE { get; set; }
        public int DISCOMBOBULATOR_PRICE { get; set; }
        public int MALWARE_BROADCASTER_PRICE { get; set; }

        // attributes
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
        public float CHANCE_TO_BREAK { get; set; }
        public float BEEKEEPER_DAMAGE_MULTIPLIER { get; set; }
        public float BEEKEEPER_DAMAGE_MULTIPLIER_INCREMENT { get; set; }
        public float NIGHT_VIS_DRAIN_SPEED { get; set; }
        public float NIGHT_VIS_REGEN_SPEED { get; set; }
        public float DISCOMBOBULATOR_RADIUS { get; set; }
        public float DISCOMBOBULATOR_STUN_DURATION { get; set; }
        public bool DISCOMBOBULATOR_NOTIFY_CHAT { get; set; }
        public UnityEngine.Color NIGHT_VIS_COLOR { get; set; }
        public float NIGHT_VIS_RANGE { get; set; }
        public float NIGHT_VIS_INTENSITY { get; set; }
        public float NIGHT_VIS_STARTUP { get; set; }
        public float NIGHT_VIS_EXHAUST { get; set; }
        public float CARRY_WEIGHT_INCREMENT { get; set; }
        public float MOVEMENT_INCREMENT { get; set; }
        public float SPRINT_TIME_INCREMENT { get; set; }
        public float NOISE_REDUCTION_INCREMENT { get; set; }
        public float JUMP_FORCE_INCREMENT { get; set; }
        public float DISCOMBOBULATOR_INCREMENT { get; set; }

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
            ADVANCED_TELE_ENABLED = ConfigEntry("Advanced Portable Teleporter","Enable Advanced Portable Teleporter", true, "");
            ADVANCED_TELE_PRICE = ConfigEntry("Advanced Portable Teleporter","Price of Advanced Portable Teleporter", 1750, "");
            ADV_CHANCE_TO_BREAK = ConfigEntry("Advanced Portable Teleporter","Chance to break on use", 0.1f, "value should be 0.00 - 1.00");


            WEAK_TELE_ENABLED = ConfigEntry("Portable Teleporter", "Enable Portable Teleporter", true, "");
            WEAK_TELE_PRICE = ConfigEntry("Portable Teleporter", "Price of Portable Teleporter", 300, "");
            CHANCE_TO_BREAK = ConfigEntry("Portable Teleporter","Chance to break on use", 0.9f, "value should be 0.00 - 1.00");

            KEEP_ITEMS_ON_TELE = ConfigEntry("Portable Teleporter","Keep Items When Using Portable Teleporters", true, "If set to false you will drop your items like when using the vanilla TP.");

            BEEKEEPER_ENABLED = ConfigEntry("Beekeeper","Enable Beekeeper Upgrade", true, "Take no damage from bees");
            BEEKEEPER_PRICE = ConfigEntry("Beekeeper","Price of Beekeeper Upgrade", 450, "");
            BEEKEEPER_DAMAGE_MULTIPLIER = ConfigEntry("Beekeeper","Multiplied to incoming damage (rounded to int)", 0.64f, "Incoming damage from bees is 10.");
            BEEKEEPER_DAMAGE_MULTIPLIER_INCREMENT = ConfigEntry("Beekeeper","Additional % Reduced per level", 0.15f, "Every time beekeeper is upgraded this value will be subtracted to the base multiplier above.");

            BIGGER_LUNGS_ENABLED = ConfigEntry("Bigger Lungs","Enable Bigger Lungs Upgrade", true, "More Stamina");
            BIGGER_LUNGS_PRICE = ConfigEntry("Bigger Lungs","Price of Bigger Lungs Upgrade", 700, "");
            SPRINT_TIME_INCREASE = ConfigEntry("Bigger Lungs","SprintTime value", 17f, "Vanilla value is 11");
            SPRINT_TIME_INCREMENT = ConfigEntry("Bigger Lungs","SprintTime Increment", 1.25f,"How much the above value is increased on upgrade.");

            RUNNING_SHOES_ENABLED = ConfigEntry("Running Shoes","Enable Running Shoes Upgrade", true, "Run Faster");
            RUNNING_SHOES_PRICE = ConfigEntry("Running Shoes","Price of Running Shoes Upgrade", 1000, "");
            MOVEMENT_SPEED = ConfigEntry("Running Shoes","Movement Speed Value", 6f, "Vanilla value is 4.6");
            MOVEMENT_INCREMENT = ConfigEntry("Running Shoes", "Movement Speed Increment", 0.5f, "How much the above value is increased on upgrade.");

            STRONG_LEGS_ENABLED = ConfigEntry("Strong Legs","Enable Strong Legs Upgrade", true, "Jump Higher");
            STRONG_LEGS_PRICE = ConfigEntry("Strong Legs","Price of Strong Legs Upgrade", 300, "");
            JUMP_FORCE = ConfigEntry("Strong Legs","Jump Force", 16f, "Vanilla value is 13");
            JUMP_FORCE_INCREMENT = ConfigEntry("Strong Legs","Jump Force Increment", 0.75f, "How much the above value is increased on upgrade.");

            MALWARE_BROADCASTER_ENABLED = ConfigEntry("Malware Broadcaster", "Enable Malware Broadcaster Upgrade", true, "Explode Map Hazards");
            MALWARE_BROADCASTER_PRICE = ConfigEntry("Malware Broadcaster", "Price of Malware Broadcaster Upgrade", 650, "");
            DESTROY_TRAP = ConfigEntry("Malware Broadcaster", "Destroy Trap", true, "If false Malware Broadcaster will disable the trap for a long time instead of destroying.");
            DISARM_TIME = ConfigEntry("Malware Broadcaster", "Disarm Time", 7f, "If `Destroy Trap` is false this is the duration traps will be disabled.");
            EXPLODE_TRAP = ConfigEntry("Malware Broadcaster", "Explode Trap", true, "Destroy Trap must be true! If this is true when destroying a trap it will also explode.");


            LIGHT_FOOTED_ENABLED = ConfigEntry("Light Footed", "Enable Light Footed Upgrade", true, "Make less noise moving.");
            LIGHT_FOOTED_PRICE = ConfigEntry("Light Footed", "Price of Light Footed Upgrade", 350, "");
            NOISE_REDUCTION = ConfigEntry("Light Footed", "Noise Reduction", 7f, "Distance units to subtract from footstep noise.");
            NOISE_REDUCTION_INCREMENT = ConfigEntry("Light Footed", "Noise Reduction Increment", 1f, "The amount added to above value on upgrade.");



            NIGHT_VISION_ENABLED = ConfigEntry("Night Vision", "Enable Night Vision Upgrade", true, "Toggleable night vision.");
            NIGHT_VISION_PRICE = ConfigEntry("Night Vision", "Price of Night Vision Upgrade", 700, "");
            NIGHT_VIS_DRAIN_SPEED = ConfigEntry("Night Vision", "Multiplier for night vis battery drain", 0.1f, "Multiplied by timedelta. A value of 0.1 will result in a 10 second battery life.");
            NIGHT_VIS_REGEN_SPEED = ConfigEntry("Night Vision", "Multiplier for night vis battery regen", 0.05f, "Multiplied by timedelta.");
            NIGHT_VIS_COLOR = ConfigEntry("Night Vision", "Night Vision Color", UnityEngine.Color.green, "The color your night vision light emits.");
            NIGHT_VIS_RANGE = ConfigEntry("Night Vision", "Night Vision Range", 2000f, "Kind of like the distance your night vision travels.");
            NIGHT_VIS_INTENSITY = ConfigEntry("Night Vision", "Night Vision Intensity", 1000f, "Kind of like the brightness of your Night Vision.");
            NIGHT_VIS_STARTUP = ConfigEntry("Night Vision", "Night Vision StartUp Cost", 0.1f, "The percent battery drained when turned on (0.1 = 10%).");
            NIGHT_VIS_EXHAUST = ConfigEntry("Night Vision", "Night Vision Exhaustion", 2f, "How many seconds night vision stays fully depleted.");


            DISCOMBOBULATOR_ENABLED = ConfigEntry("Discombobulator", "Enable Discombobulator Upgrade", true, "Stun enemies around the ship.");
            DISCOMBOBULATOR_PRICE = ConfigEntry("Discombobulator", "Price of Discombobulator Upgrade", 550, "");
            DISCOMBOBULATOR_COOLDOWN = ConfigEntry("Discombobulator", "Discombobulator Cooldown", 120f, "");
            DISCOMBOBULATOR_RADIUS  = ConfigEntry("Discombobulator", "Discombobulator Effect Radius", 40f, "");
            DISCOMBOBULATOR_STUN_DURATION  = ConfigEntry("Discombobulator", "Discombobulator Stun Duration", 7.5f, "");
            DISCOMBOBULATOR_NOTIFY_CHAT = ConfigEntry("Discombobulator", "Notify Local Chat of Enemy Stun Duration", true, "");
            DISCOMBOBULATOR_INCREMENT  = ConfigEntry("Discombobulator", "Discombobulator Increment", 1f, "The amount added to stun duration on upgrade.");

            BETTER_SCANNER_ENABLED = ConfigEntry("Better Scanner", "Enable Better Scanner Upgrade", true, "Further scan distance, no LOS needed.");
            BETTER_SCANNER_PRICE = ConfigEntry("Better Scanner", "Price of Better Scanner Upgrade", 650, "");
            REQUIRE_LINE_OF_SIGHT = ConfigEntry("Better Scanner", "Require Line Of Sight", false, "Default mod value is false.");
            SHIP_AND_ENTRANCE_DISTANCE_INCREASE = ConfigEntry("Better Scanner", "Ship and Entrance node distance boost", 150f, "How much further away you can scan the ship and entrance.");
            NODE_DISTANCE_INCREASE = ConfigEntry("Better Scanner", "Node distance boost", 20f, "How much further away you can scan other nodes.");


            BACK_MUSCLES_ENABLED = ConfigEntry("Back Muscles", "Enable Back Muscles Upgrade", true, "Reduce carry weight");
            BACK_MUSCLES_PRICE = ConfigEntry("Back Muscles", "Price of Back Muscles Upgrade", 835, "");
            CARRY_WEIGHT_REDUCTION = ConfigEntry("Back Muscles", "Carry Weight Multiplier", 0.5f, "Your carry weight is multiplied by this.");
            CARRY_WEIGHT_INCREMENT = ConfigEntry("Back Muscles", "Carry Weight Increment", 0.1f, "Each upgrade subtracts this from the above coefficient.");
        }
    }
}
