using LethalLib.Modules;
using MoreShipUpgrades.Managers;
using MoreShipUpgrades.UpgradeComponents.Commands;
using MoreShipUpgrades.UpgradeComponents.Items;
using MoreShipUpgrades.UpgradeComponents.OneTimeUpgrades;
using MoreShipUpgrades.UpgradeComponents.OneTimeUpgrades.Enemies;
using MoreShipUpgrades.UpgradeComponents.OneTimeUpgrades.Items;
using MoreShipUpgrades.UpgradeComponents.OneTimeUpgrades.Player;
using MoreShipUpgrades.UpgradeComponents.OneTimeUpgrades.Ship;
using MoreShipUpgrades.UpgradeComponents.OneTimeUpgrades.Store;
using MoreShipUpgrades.UpgradeComponents.TierUpgrades;
using MoreShipUpgrades.UpgradeComponents.TierUpgrades.AttributeUpgrades;
using MoreShipUpgrades.UpgradeComponents.TierUpgrades.Enemies;
using MoreShipUpgrades.UpgradeComponents.TierUpgrades.Items;
using MoreShipUpgrades.UpgradeComponents.TierUpgrades.Items.Jetpack;
using MoreShipUpgrades.UpgradeComponents.TierUpgrades.Items.RadarBooster;
using MoreShipUpgrades.UpgradeComponents.TierUpgrades.Items.Shotgun;
using MoreShipUpgrades.UpgradeComponents.TierUpgrades.Items.WeedKiller;
using MoreShipUpgrades.UpgradeComponents.TierUpgrades.Items.Zapgun;
using MoreShipUpgrades.UpgradeComponents.TierUpgrades.Player;
using MoreShipUpgrades.UpgradeComponents.TierUpgrades.Ship;
using MoreShipUpgrades.UpgradeComponents.TierUpgrades.Store;
using UnityEngine;

namespace MoreShipUpgrades.Misc.Util
{
    internal static class LguConstants
    {
        #region General

        #region Characters
        internal const char WHITE_SPACE = ' ';
        #endregion

        #region Hex Colors

        internal const string HEXADECIMAL_RED = "#FF0000";
        internal const string HEXADECIMAL_DARK_RED = "#8B0000";
        internal const string HEXADECIMAL_WHITE = "#FFFFFF";
        internal const string HEXADECIMAL_GREEN = "#00FF00";
        internal const string HEXADECIMAL_DARK_GREEN = "#026440";
        internal const string HEXADECIMAL_GREY = "#666666";

        #endregion

        #endregion

        #region Keybinds

        internal const string DROP_ALL_ITEMS_WHEELBARROW_KEYBIND_NAME = "Drop all items from wheelbarrow";
        internal const string DROP_ALL_ITEMS_WHEELBARROW_DEFAULT_KEYBIND = "<Mouse>/middleButton";

        internal const string TOGGLE_NIGHT_VISION_KEYBIND_NAME = "Toggle NVG";
        internal const string TOGGLE_NIGHT_VISION_DEFAULT_KEYBIND = "<Keyboard>/leftAlt";

        internal const string MOVE_CURSOR_UP_KEYBIND_NAME = "Move Cursor Up in Lategame Upgrades Store";
        internal const string MOVE_CURSOR_UP_DEFAULT_KEYBIND = "<Keyboard>/w";

        internal const string MOVE_CURSOR_DOWN_KEYBIND_NAME = "Move Cursor Down in Lategame Upgrades Store";
        internal const string MOVE_CURSOR_DOWN_DEFAULT_KEYBIND = "<Keyboard>/s";

        internal const string EXIT_STORE_KEYBIND_NAME = "Exit Lategame Upgrades Store";
        internal const string EXIT_STORE_DEFAULT_KEYBIND = "<Keyboard>/escape";

        internal const string NEXT_PAGE_KEYBIND_NAME = "Next Page in Lategame Upgrades Store";
        internal const string NEXT_PAGE_DEFAULT_KEYBIND = "<Keyboard>/d";

        internal const string PREVIOUS_PAGE_KEYBIND_NAME = "Previous Page in Lategame Upgrades Store";
        internal const string PREVIOUS_PAGE_DEFAULT_KEYBIND = "<Keyboard>/a";

        internal const string SUBMIT_PROMPT_KEYBIND_NAME = "Submit Prompt in Lategame Upgrades Store";
        internal const string SUBMIT_PROMPT_DEFAULT_KEYBIND = "<Keyboard>/enter";

        #endregion

        #region Plugin Configuration

        #region Randomize Available Upgrades

        internal const string RANDOMIZE_UPGRADES_SECTION = "_Randomize Upgrades_";

        internal const string RANDOMIZE_UPGRADES_ENABLED_KEY = "Enable Randomize Upgrades";
        internal const bool RANDOMIZE_UPGRADES_ENABLED_DEFAULT = false;
        internal const string RANDOMIZE_UPGRADES_ENABLED_DESCRIPTION = "Upgrades will be randomized per selected event to be allowed purchased on the store.";

        internal const string RANDOMIZE_UPGRADES_AMOUNT_KEY = "Amount of Upgrades";
        internal const int RANDOMIZE_UPGRADES_AMOUNT_DEFAULT = 5;
        internal const string RANDOMIZE_UPGRADES_AMOUNT_DESCRIPTION = "Amount of Upgrades that will always appear in the store (if allowed).";

        internal const string RANDOMIZE_UPGRADES_ALWAYS_SHOW_PURCHASED_KEY = "Show purchased upgrades";
        internal const bool RANDOMIZE_UPGRADES_ALWAYS_SHOW_PURCHASED_DEFAULT = true;
        internal const string RANDOMIZE_UPGRADES_ALWAYS_SHOW_PURCHASED_DESCRIPTION = "Wether purchased upgrades will always show in the store and the specified amount of random upgrades will not include these in the selection.";

        internal const string RANDOMIZE_UPGRADES_CHANGE_UPGRADES_EVENT_KEY = "Randomize Upgrades Event";
        internal const RandomizeUpgradeManager.RandomizeUpgradeEvents RANDOMIZE_UPGRADES_CHANGE_UPGRADES_EVENT_DEFAULT = RandomizeUpgradeManager.RandomizeUpgradeEvents.PerQuota;
        internal const string RANDOMIZE_UPGRADES_CHANGE_UPGRADES_EVENT_DESCRIPTION = "Event which triggers the random upgrades being changed in the store.";

        #endregion

        #region Item Progression

        internal const string ITEM_PROGRESSION_SECTION = "_Item Progression Route_";

        internal const string ALTERNATIVE_ITEM_PROGRESSION_KEY = "Enable Item Progression";
        internal const bool ALTERNATIVE_ITEM_PROGRESSION_DEFAULT = false;
        internal const string ALTERNATIVE_ITEM_PROGRESSION_DESCRIPTION = "Alternative Item Progression\n" +
                                                                        "If true, items retrieved can be used for ship upgrades.";

        internal const string ITEM_PROGRESSION_MODE_KEY = "Item Progression Mode";
        internal const ItemProgressionManager.CollectionModes ITEM_PROGRESSION_MODE_DEFAULT = ItemProgressionManager.CollectionModes.CustomScrap;
        internal const string ITEM_PROGRESSION_MODE_DESCRIPTION = "Item Progression Mode\n" +
                                                                "Supported Modes: \n" +
                                                                "CustomScrap: Specified scrap will contribute towards the level of the associated upgrade.\n" +
                                                                "UniqueScrap: All scrap (specifications will be ignored) will contribute towards the level of a random assigned upgrade\n" +
                                                                "NearestValue: At the end of each Quota, grant a random upgrade worth the nearest total scrap collected, rounded down.\n" +
                                                                "ChancePerScrap: For each scrap sold, run a configured chance to receive a random/cheap/lowest tier upgrade.\n" +
                                                                "Apparatice: Always receive an upgrade when an apparatus has been successfully sold.";

        internal const string ITEM_PROGRESSION_CONTRIBUTION_MULTIPLIER_KEY = "Item Progression Multiplier";
        internal const float ITEM_PROGRESSION_CONTRIBUTION_MULTIPLIER_DEFAULT = 0.85f;
        internal const string ITEM_PROGRESSION_CONTRIBUTION_MULTIPLIER_DESCRIPTION = "Item Progression Contribution Multiplier\n" +
                                                                                    "The multiplier for scrap's value contribution towards an upgrade.";

        internal const string SCRAP_UPGRADE_CHANCE_KEY = "Scrap Upgrade Chance";
        internal const float SCRAP_UPGRADE_CHANCE_DEFAULT = 0.05f;
        internal const string SCRAP_UPGRADE_CHANCE_DESCRIPTION = "Upgrade Chance Calculation\n" +
                                                                "Only used if Item Progression Mode is set to ChancePerScrap.\n" +
                                                                "X -> (X * 100) %";

        internal const string SCRAP_UPGRADE_MODE_KEY = "Upgrade Chance Priority";
        internal const ItemProgressionManager.ChancePerScrapModes SCRAP_UPGRADE_MODE_DEFAULT = ItemProgressionManager.ChancePerScrapModes.LowestLevel;
        internal const string SCRAP_UPGRADE_MODE_DESCRIPTION = "Upgrade Chance Priority\n" +
                                                            "Only used if Item Progression Mode is set to ChancePerScrap.\n" +
                                                            "Acceptable Values:\nRandom: Pick a random upgrade to apply.\n" +
                                                            "Cheapest: Pick the cheapest upgrade to apply.\n" +
                                                            "Lowest Tier: Pick the upgrade with the lowest level to apply.";

        internal const string ITEM_PROGRESSION_BLACKLISTED_ITEMS_KEY = "Blacklisted Items";
        internal const string ITEM_PROGRESSION_BLACKLISTED_ITEMS_DEFAULT = "Clipboard";
        internal const string ITEM_PROGRESSION_BLACKLISTED_ITEMS_DESCRIPTION = "Blacklisted items for Item Progression.\nOnly used if Item Progression Mode is set to UniqueScrap. These items will NOT contribute towards ANY upgrade";

        internal const string ITEM_PROGRESSION_BLACKLIST_ITEMS_ENTRY_DELIMITER = ",";

        internal const string ITEM_PROGRESSION_APPARATICE_ITEMS_KEY = "Apparatus Items";
        internal const string ITEM_PROGRESSION_APPARATICE_ITEMS_DEFAULT = $"Apparatus{ITEM_PROGRESSION_APPARATICE_ITEMS_ATTRIBUTE_DELIMITER}1{ITEM_PROGRESSION_APPARATICE_ITEMS_ENTRY_DELIMITER}ExampleItemName{ITEM_PROGRESSION_APPARATICE_ITEMS_ATTRIBUTE_DELIMITER}2";
        internal const string ITEM_PROGRESSION_APPARATICE_ITEMS_DESCRIPTION = "Items that are considered \"Apparatice\" and provide a set amount of upgrades once sold to The Company.\n" +
                                                                            "The item name is either the one displayed in the scan node or its internal name.\n" +
                                                                            "This list is only valid if Item Progression Mode is set to Apparatice.";

        internal const string ITEM_PROGRESSION_NO_PURCHASE_UPGRADES_KEY = "Non-Purchaseable Upgrades";
        internal const bool ITEM_PROGRESSION_NO_PURCHASE_UPGRADES_DEFAULT = false;
        internal const string ITEM_PROGRESSION_NO_PURCHASE_UPGRADES_DESCRIPTION = "Prevents the upgrades from being purchaseable in the store to be only used to know how far are they to acquiring one. Only valid if Item Progression mode is toggled on";

        internal const string ITEM_PROGRESSION_ALWAYS_SHOW_ITEMS_KEY = "Always Show Contribution Items";
        internal const bool ITEM_PROGRESSION_ALWAYS_SHOW_ITEMS_DEFAULT = false;
        internal const string ITEM_PROGRESSION_ALWAYS_SHOW_ITEMS_DESCRIPTION = "Shows all the items associated to each upgrade regardless if they were sold before or not.";

        internal const string ITEM_PROGRESSION_APPARATICE_ITEMS_ENTRY_DELIMITER = ",";
        internal const string ITEM_PROGRESSION_APPARATICE_ITEMS_ATTRIBUTE_DELIMITER = "@";

        internal const string ITEM_PROGRESSION_ITEMS_KEY = "Contribution Items";
        internal const string ITEM_PROGRESSION_ITEMS_DEFAULT = "";
        internal const string ITEM_PROGRESSION_ITEMS_DESCRIPTION = "Items that when sold contribute to the purchase of the upgrade. Either the scan node's name or ItemProperties.itemName can be inserted here";

        #endregion

        #region Miscellaneous

        internal const string MISCELLANEOUS_SECTION = "_Misc_";

        internal const string SHARE_ALL_UPGRADES_KEY = "Convert all upgrades to be shared.";
        internal const bool SHARE_ALL_UPGRADES_DEFAULT = true;
        internal const string SHARE_ALL_UPGRADES_DESCRIPTION = "If true this will ignore the individual shared upgrade option for all other upgrades and set all upgrades to be shared.";

        internal const string SALE_PERCENT_KEY = "Chance of upgrades going on sale";
        internal const float SALE_PERCENT_DEFAULT = 0.85f;
        internal const string SALE_PERCENT_DESCRIPTION = "0.85 = 15% chance of an upgrade going on sale.";

        internal const string KEEP_UPGRADES_AFTER_FIRED_KEY = "Keep upgrades after quota failure";
        internal const bool KEEP_UPGRADES_AFTER_FIRED_DEFAULT = false;
        internal const string KEEP_UPGRADES_AFTER_FIRED_DESCRIPTION = "If true, you will keep your upgrades after being fired by The Company.";

        internal const string SHOW_UPGRADES_CHAT_KEY = "Show upgrades being loaded in chat";
        internal const bool SHOW_UPGRADES_CHAT_DEFAULT = true;
        internal const string SHOW_UPGRADES_CHAT_DESCRIPTION = "If enabled, chat messages will be displayed when loading an upgrade for the first time.";

        internal const string SALE_APPLY_ONCE_KEY = "Apply upgrade sale on one purchase";
        internal const bool SALE_APPLY_ONCE_DEFAULT = false;
        internal const string SALE_APPLY_ONCE_DESCRIPTION = "When an upgrade is on sale, apply the sale only on the first ever purchase of it while on sale. Consecutive purchases will not have the sale applied";

        internal const string SHOW_WORLD_BUILDING_TEXT_KEY = "Show World Building text in Upgrade information";
        internal const bool SHOW_WORLD_BUILDING_TEXT_DEFAULT = true;
        internal const string SHOW_WORLD_BUILDING_TEXT_DESCRIPTION = "When viewing an upgrade's information, wether show world-building text along with pratical information or not.";
        #endregion

        #region Name Overrides
        internal const string OVERRIDE_NAMES_SECTION = "Name Overrides";

        internal const string OVERRIDE_NAMES_ENABLED_KEY = "Override Upgrade Names";
        internal const bool OVERRIDE_NAMES_ENABLED_DEFAULT = false;
        internal const string OVERRIDE_NAMES_ENABLED_DESCRIPTION = "When enabled, replaces the names of each upgrades with the ones defined in this section";

        internal const string OVERRIDE_NAME_KEY_FORMAT = "Alternative name for {0} upgrade";

        internal static readonly string FUSION_MATTER_OVERRIDE_NAME_KEY = string.Format(OVERRIDE_NAME_KEY_FORMAT, FusionMatter.UPGRADE_NAME);
        internal static readonly string LONG_BARREL_OVERRIDE_NAME_KEY = string.Format(OVERRIDE_NAME_KEY_FORMAT, LongBarrel.UPGRADE_NAME);
        internal static readonly string HOLLOW_POINT_OVERRIDE_NAME_KEY = string.Format(OVERRIDE_NAME_KEY_FORMAT, HollowPoint.UPGRADE_NAME);
        internal static readonly string JETPACK_THRUSTERS_OVERRIDE_NAME_KEY = string.Format(OVERRIDE_NAME_KEY_FORMAT, JetpackThrusters.UPGRADE_NAME);
        internal static readonly string JET_FUEL_OVERRIDE_NAME_KEY = string.Format(OVERRIDE_NAME_KEY_FORMAT, JetFuel.UPGRADE_NAME);
        internal static readonly string QUICK_HANDS_OVERRIDE_NAME_KEY = string.Format(OVERRIDE_NAME_KEY_FORMAT, QuickHands.UPGRADE_NAME);
        internal static readonly string MIDAS_TOUCH_OVERRIDE_NAME_KEY = string.Format(OVERRIDE_NAME_KEY_FORMAT, MidasTouch.UPGRADE_NAME);
        internal static readonly string CARBON_KNEEJOINTS_OVERRIDE_NAME_KEY = string.Format(OVERRIDE_NAME_KEY_FORMAT, CarbonKneejoints.UPGRADE_NAME);
        internal static readonly string LIFE_INSURANCE_OVERRIDE_NAME_KEY = string.Format(OVERRIDE_NAME_KEY_FORMAT, LifeInsurance.UPGRADE_NAME);
        internal static readonly string RUBBER_BOOTS_OVERRIDE_NAME_KEY = string.Format(OVERRIDE_NAME_KEY_FORMAT, RubberBoots.UPGRADE_NAME);
        internal static readonly string OXYGEN_CANISTERS_OVERRIDE_NAME_KEY = string.Format(OVERRIDE_NAME_KEY_FORMAT, OxygenCanisters.UPGRADE_NAME);
        internal static readonly string SLEIGHT_OF_HAND_OVERRIDE_NAME_KEY = string.Format(OVERRIDE_NAME_KEY_FORMAT, SleightOfHand.UPGRADE_NAME);
        internal static readonly string HIKING_BOOTS_OVERRIDE_NAME_KEY = string.Format(OVERRIDE_NAME_KEY_FORMAT, HikingBoots.UPGRADE_NAME);
        internal static readonly string TRACTION_BOOTS_OVERRIDE_NAME_KEY = string.Format(OVERRIDE_NAME_KEY_FORMAT, TractionBoots.UPGRADE_NAME);
        internal static readonly string FEDORA_SUIT_OVERRIDE_NAME_KEY = string.Format(OVERRIDE_NAME_KEY_FORMAT, FedoraSuit.UPGRADE_NAME);
        internal static readonly string WEED_GENETIC_MANIPULATION_OVERRIDE_NAME_KEY = string.Format(OVERRIDE_NAME_KEY_FORMAT, WeedGeneticManipulation.UPGRADE_NAME);
        internal static readonly string CLAY_GLASSES_OVERRIDE_NAME_KEY = string.Format(OVERRIDE_NAME_KEY_FORMAT, ClayGlasses.UPGRADE_NAME);
        internal static readonly string MECHANICAL_ARMS_OVERRIDE_NAME_KEY = string.Format(OVERRIDE_NAME_KEY_FORMAT, MechanicalArms.UPGRADE_NAME);
        internal static readonly string SCAVENGER_INSTINCTS_OVERRIDE_NAME_KEY = string.Format(OVERRIDE_NAME_KEY_FORMAT, ScavengerInstincts.UPGRADE_NAME);
        internal static readonly string LANDING_THRUSTERS_OVERRIDE_NAME_KEY = string.Format(OVERRIDE_NAME_KEY_FORMAT, LandingThrusters.UPGRADE_NAME);
        internal static readonly string REINFORCED_BOOTS_OVERRIDE_NAME_KEY = string.Format(OVERRIDE_NAME_KEY_FORMAT, ReinforcedBoots.UPGRADE_NAME);
        internal static readonly string DEEPER_POCKETS_OVERRIDE_NAME_KEY = string.Format(OVERRIDE_NAME_KEY_FORMAT, DeepPockets.UPGRADE_NAME);
        internal static readonly string ALUMINIUM_COILS_OVERRIDE_NAME_KEY = string.Format(OVERRIDE_NAME_KEY_FORMAT, AluminiumCoils.UPGRADE_NAME);
        internal static readonly string BACK_MUSCLES_OVERRIDE_NAME_KEY = string.Format(OVERRIDE_NAME_KEY_FORMAT, BackMuscles.UPGRADE_NAME);
        internal static readonly string BARGAIN_CONNECTIONS_OVERRIDE_NAME_KEY = string.Format(OVERRIDE_NAME_KEY_FORMAT, BargainConnections.UPGRADE_NAME);
        internal static readonly string BEEKEEPER_OVERRIDE_NAME_KEY = string.Format(OVERRIDE_NAME_KEY_FORMAT, Beekeeper.UPGRADE_NAME);
        internal static readonly string BETTER_SCANNER_OVERRIDE_NAME_KEY = string.Format(OVERRIDE_NAME_KEY_FORMAT, BetterScanner.UPGRADE_NAME);
        internal static readonly string CHARGING_BOOSTER_OVERRIDE_NAME_KEY = string.Format(OVERRIDE_NAME_KEY_FORMAT, ChargingBooster.UPGRADE_NAME);
        internal static readonly string DISCOMBOBULATOR_OVERRIDE_NAME_KEY = string.Format(OVERRIDE_NAME_KEY_FORMAT, Discombobulator.UPGRADE_NAME);
        internal static readonly string EFFICIENT_ENGINES_OVERRIDE_NAME_KEY = string.Format(OVERRIDE_NAME_KEY_FORMAT, EfficientEngines.UPGRADE_NAME);
        internal static readonly string HUNTER_OVERRIDE_NAME_KEY = string.Format(OVERRIDE_NAME_KEY_FORMAT, Hunter.UPGRADE_NAME);
        internal static readonly string LITHIUM_BATTERIES_OVERRIDE_NAME_KEY = string.Format(OVERRIDE_NAME_KEY_FORMAT, LithiumBatteries.UPGRADE_NAME);
        internal static readonly string MARKET_INFLUENCE_OVERRIDE_NAME_KEY = string.Format(OVERRIDE_NAME_KEY_FORMAT, MarketInfluence.UPGRADE_NAME);
        internal static readonly string NIGHT_VISION_OVERRIDE_NAME_KEY = string.Format(OVERRIDE_NAME_KEY_FORMAT, NightVision.UPGRADE_NAME);
        internal static readonly string PROTEIN_POWDER_OVERRIDE_NAME_KEY = string.Format(OVERRIDE_NAME_KEY_FORMAT, ProteinPowder.UPGRADE_NAME);
        internal static readonly string BIGGER_LUNGS_OVERRIDE_NAME_KEY = string.Format(OVERRIDE_NAME_KEY_FORMAT, BiggerLungs.UPGRADE_NAME);
        internal static readonly string CLIMBING_GLOVES_OVERRIDE_NAME_KEY = string.Format(OVERRIDE_NAME_KEY_FORMAT, ClimbingGloves.UPGRADE_NAME);
        internal static readonly string SHUTTER_BATTERIES_OVERRIDE_NAME_KEY = string.Format(OVERRIDE_NAME_KEY_FORMAT, ShutterBatteries.UPGRADE_NAME);
        internal static readonly string QUANTUM_DISRUPTOR_OVERRIDE_NAME_KEY = string.Format(OVERRIDE_NAME_KEY_FORMAT, QuantumDisruptor.UPGRADE_NAME);
        internal static readonly string RUNNING_SHOES_OVERRIDE_NAME_KEY = string.Format(OVERRIDE_NAME_KEY_FORMAT, RunningShoes.UPGRADE_NAME);
        internal static readonly string STIMPACK_OVERRIDE_NAME_KEY = string.Format(OVERRIDE_NAME_KEY_FORMAT, Stimpack.UPGRADE_NAME);
        internal static readonly string STRONG_LEGS_OVERRIDE_NAME_KEY = string.Format(OVERRIDE_NAME_KEY_FORMAT, StrongLegs.UPGRADE_NAME);
        internal static readonly string FAST_ENCRYPTION_OVERRIDE_NAME_KEY = string.Format(OVERRIDE_NAME_KEY_FORMAT, FastEncryption.UPGRADE_NAME);
        internal static readonly string DROP_POD_THRUSTERS_OVERRIDE_NAME_KEY = string.Format(OVERRIDE_NAME_KEY_FORMAT, FasterDropPod.UPGRADE_NAME);
        internal static readonly string LETHAL_DEALS_OVERRIDE_NAME_KEY = string.Format(OVERRIDE_NAME_KEY_FORMAT, LethalDeals.UPGRADE_NAME);
        internal static readonly string LIGHTNING_ROD_OVERRIDE_NAME_KEY = string.Format(OVERRIDE_NAME_KEY_FORMAT, LightningRod.UPGRADE_NAME);
        internal static readonly string LOCKSMITH_OVERRIDE_NAME_KEY = string.Format(OVERRIDE_NAME_KEY_FORMAT, LockSmith.UPGRADE_NAME);
        internal static readonly string MALWARE_BROADCASTER_OVERRIDE_NAME_KEY = string.Format(OVERRIDE_NAME_KEY_FORMAT, MalwareBroadcaster.UPGRADE_NAME);
        internal static readonly string SICK_BEATS_OVERRIDE_NAME_KEY = string.Format(OVERRIDE_NAME_KEY_FORMAT, SickBeats.UPGRADE_NAME);
        internal static readonly string SIGURD_ACCESS_OVERRIDE_NAME_KEY = string.Format(OVERRIDE_NAME_KEY_FORMAT, Sigurd.UPGRADE_NAME);
        internal static readonly string WALKIE_GPS_OVERRIDE_NAME_KEY = string.Format(OVERRIDE_NAME_KEY_FORMAT, WalkieGPS.UPGRADE_NAME);

        #endregion

        #region Contracts
        internal const string CONTRACTS_SECTION = "Contracts";

        internal const string ENABLE_CONTRACTS_KEY = "Enable the ability to purchase contracts / missions";
        internal const bool ENABLE_CONTRACTS_DEFAULT = true;

        internal const string CONTRACT_PROVIDE_RANDOM_ONLY_KEY = "Random Contract on Keyword";
        internal const bool CONTRACT_PROVIDE_RANDOM_ONLY_DEFAULT = false;
        internal const string CONTRACT_PROVIDE_RANDOM_ONLY_DESCRIPTION = "When enabled, typing contract will automatically assign you a random contract when possible. (Essentialy old behaviour)";

        internal const string CONTRACT_FREE_MOONS_ONLY_KEY = "Random contracts on free moons only";
        internal const bool CONTRACT_FREE_MOONS_ONLY_DEFAULT = true;
        internal const string CONTRACT_FREE_MOONS_ONLY_DESCRIPTION = "If true, \"contract\" command will only generate contracts on free moons.";

        internal const string CONTRACT_PRICE_KEY = "Price of a random contract";
        internal const int CONTRACT_PRICE_DEFAULT = 500;

        internal const string CONTRACT_SPECIFY_PRICE_KEY = "Price of a specified moon contract";
        internal const int CONTRACT_SPECIFY_PRICE_DEFAULT = 750;

        internal const string CONTRACT_BUG_REWARD_KEY = "Value of an exterminator contract reward";
        internal const int CONTRACT_BUG_REWARD_DEFAULT = CONTRACT_PRICE_DEFAULT;

        internal const string CONTRACT_EXORCISM_REWARD_KEY = "Value of an exorcism contract reward";
        internal const int CONTRACT_EXORCISM_REWARD_DEFAULT = CONTRACT_PRICE_DEFAULT;

        internal const string CONTRACT_DEFUSAL_REWARD_KEY = "Value of an defusal contract reward";
        internal const int CONTRACT_DEFUSAL_REWARD_DEFAULT = CONTRACT_PRICE_DEFAULT;

        internal const string CONTRACT_EXTRACTION_REWARD_KEY = "Value of an extraction contract reward";
        internal const int CONTRACT_EXTRACTION_REWARD_DEFAULT = CONTRACT_PRICE_DEFAULT;

        internal const string CONTRACT_DATA_REWARD_KEY = "Value of a data contract reward";
        internal const int CONTRACT_DATA_REWARD_DEFAULT = CONTRACT_PRICE_DEFAULT;

        internal const string EXTERMINATION_BUG_SPAWNS_KEY = "Hoarder Bug Spawn Number";
        internal const int EXTERMINATION_BUG_SPAWNS_DEFAULT = 20;
        internal const string EXTERMINATION_BUG_SPAWNS_DESCRIPTION = "How many bugs to spawn during exterminator contracts.";

        internal const string EXORCISM_GHOST_SPAWN_KEY = "Dress Girl / Thumper Spawn Number";
        internal const int EXORCISM_GHOST_SPAWN_DEFAULT = 3;
        internal const string EXORCISM_GHOST_SPAWN_DESCRIPTION = "How many ghosts/thumpers to spawn when failing exorcism contracts";

        internal const string EXTRACTION_SCAVENGER_WEIGHT_KEY = "Weight of an extraction human";
        internal const float EXTRACTION_SCAVENGER_WEIGHT_DEFAULT = 2.5f;
        internal const string EXTRACTION_SCAVENGER_WEIGHT_DESCRIPTION = "Subtract 1 and multiply by 100 (2.5 = 150lbs).";

        internal const string EXTRACTION_SCAVENGER_SOUND_VOLUME_KEY = "Volume of the scavenger voice clips";
        internal const float EXTRACTION_SCAVENGER_SOUND_VOLUME_DEFAULT = 0.25f;
        internal const string EXTRACTION_SCAVENGER_SOUND_VOLUME_DESCRIPTION = "0.0 - 1.0";

        internal const string CONTRACT_FAR_FROM_MAIN_KEY = "Spawn main object far away";
        internal const bool CONTRACT_FAR_FROM_MAIN_DEFAULT = true;
        internal const string CONTRACT_FAR_FROM_MAIN_DESCRIPTION = "If true the main object for contracts will try spawn as far away from the main entrance as possible. If false it will spawn at a random location.";

        internal const string EXTRACTION_MEDKIT_AMOUNT_KEY = "Amount of medkits that can spawn in the Extraction contract";
        internal const int EXTRACTION_MEDKIT_AMOUNT_DEFAULT = 3;

        internal const string CONTRACT_DATA_ENABLED_KEY = "Enable the data contract";
        internal const bool CONTRACT_DATA_ENABLED_DEFAULT = true;
        internal const string CONTRACT_DATA_ENABLED_DESCRIPTION = "Make this false if you don't want the data contract";

        internal const string CONTRACT_EXTRACTION_ENABLED_KEY = "Enable the extraction contract";
        internal const bool CONTRACT_EXTRACTION_ENABLED_DEFAULT = true;
        internal const string CONTRACT_EXTRACTION_ENABLED_DESCRIPTION = "Make this false if you don't want the extraction contract";

        internal const string CONTRACT_EXORCISM_ENABLED_KEY = "Enable the exorcism contract";
        internal const bool CONTRACT_EXORCISM_ENABLED_DEFAULT = true;
        internal const string CONTRACT_EXORCISM_ENABLED_DESCRIPTION = "Make this false if you don't want the exorcism contract";

        internal const string CONTRACT_DEFUSAL_ENABLED_KEY = "Enable the defusal contract";
        internal const bool CONTRACT_DEFUSAL_ENABLED_DEFAULT = true;
        internal const string CONTRACT_DEFUSAL_ENABLED_DESCRIPTION = "Make this false if you don't want the defusal contract";

        internal const string CONTRACT_EXTERMINATION_ENABLED_KEY = "Enable the exterminator contract";
        internal const bool CONTRACT_EXTERMINATION_ENABLED_DEFAULT = true;
        internal const string CONTRACT_EXTERMINATION_ENABLED_DESCRIPTION = "Make this false if you don't want the exterminator contract";

        internal const string CONTRACT_QUOTA_MULTIPLIER_KEY = "Multiplier applied to the contract loot items dependant on the current applied quota";
        internal const int CONTRACT_QUOTA_MULTIPLIER_DEFAULT = 25;
        internal const string CONTRACT_QUOTA_MULTIPLIER_DESCRIPTION = "0 = None of the quota value will influence the loot's value.\n100 = The quota value will be added fully to the loot's value.";
        #endregion

        #region Interns

        internal const string INTERNS_ENABLED_KEY = "Enable hiring of interns";
        internal const bool INTERNS_ENABLED_DEFAULT = true;
        internal const string INTERNS_ENABLED_DESCRIPTION = "Pay x amount of credits to revive a player.";

        internal const string INTERNS_PRICE_KEY = $"{Interns.NAME} Price";
        internal const int INTERNS_PRICE_DEFAULT = 1000;
        internal const string INTERNS_PRICE_DESCRIPTION = "Default price to hire an intern.";

        #endregion

        #region Items

        internal const string ITEM_SCAN_NODE_KEY_FORMAT = "Enable scan node of {0}";
        internal const bool ITEM_SCAN_NODE_DEFAULT = true;
        internal const string ITEM_SCAN_NODE_DESCRIPTION = "Shows a scan node on the item when scanning";

        #region Medkit

        internal const string MEDKIT_ENABLED_KEY = $"Enable the {Medkit.ITEM_NAME} item";
        internal const bool MEDKIT_ENABLED_DEFAULT = true;
        internal const string MEDKIT_ENABLED_DESCRIPTION = $"Allows you to buy a {Medkit.ITEM_NAME} to heal yourself.";

        internal const string MEDKIT_PRICE_KEY = $"{Medkit.ITEM_NAME} price";
        internal const int MEDKIT_PRICE_DEFAULT = 300;
        internal const string MEDKIT_PRICE_DESCRIPTION = $"Default price for {Medkit.ITEM_NAME}.";

        internal const string MEDKIT_HEAL_AMOUNT_KEY = "Heal Amount";
        internal const int MEDKIT_HEAL_AMOUNT_DEFAULT = 20;
        internal const string MEDKIT_HEAL_AMOUNT_DESCRIPTION = $"The amount the {Medkit.ITEM_NAME} heals you.";

        internal const string MEDKIT_USES_KEY = "Uses";
        internal const int MEDKIT_USES_DEFAULT = 3;
        internal const string MEDKIT_USES_DESCRIPTION = $"The amount of times the {Medkit.ITEM_NAME} can heal you.";

        internal static readonly string MEDKIT_SCAN_NODE_KEY = string.Format(ITEM_SCAN_NODE_KEY_FORMAT, Medkit.ITEM_NAME);
        #endregion

        #endregion

        #region Upgrades

        #region Fusion Matter

        internal const string FUSION_MATTER_ENABLED_KEY = $"Enable {FusionMatter.UPGRADE_NAME} Upgrade";
        internal const bool FUSION_MATTER_ENABLED_DEFAULT = true;
        internal const string FUSION_MATTER_ENABLED_DESCRIPTION = "Tier upgrade which allows you to teleport with specified items during teleportation (both in and out)";

        internal const string FUSION_MATTER_PRICE_KEY = $"Price of {FusionMatter.UPGRADE_NAME} Upgrade";
        internal const int FUSION_MATTER_PRICE_DEFAULT = 500;

        internal const string FUSION_MATTER_TIERS_KEY = "Fusion Matter Tiers";
        internal static readonly string FUSION_MATTER_TIERS_DEFAULT = $"key{FUSION_MATTER_ITEM_DELIMITER} flashlight{FUSION_MATTER_ITEM_DELIMITER} walkie-talkie{FUSION_MATTER_TIER_DELIMITER}" +
            $"shovel{FUSION_MATTER_ITEM_DELIMITER} pro-flashlight{FUSION_MATTER_TIER_DELIMITER}" +
            $"belt bag{FUSION_MATTER_ITEM_DELIMITER} radar-booster";
        internal static readonly string FUSION_MATTER_TIERS_DESCRIPTION = "List of items that will be kept in the player's inventory once reached a certain level of the upgrade. You can include names shown in their scan nodes or the names shown in the Company store.\n" +
            $"\'{FUSION_MATTER_ITEM_DELIMITER}\' is used to separate items in a tier.\n" +
            $"\'{FUSION_MATTER_TIER_DELIMITER}\' is used to separate tiers of the upgrade";

        internal const char FUSION_MATTER_TIER_DELIMITER = '@';
        internal const char FUSION_MATTER_ITEM_DELIMITER = ',';

        #endregion

        #region Long Barrel

        internal const string LONG_BARREL_ENABLED_KEY = $"Enable {LongBarrel.UPGRADE_NAME} Upgrade";
        internal const bool LONG_BARREL_ENABLED_DEFAULT = true;
        internal const string LONG_BARREL_ENABLED_DESCRIPTION = "Tier upgrade which increases the overall range of the shotgun and its effective damage ranges.";

        internal const string LONG_BARREL_PRICE_KEY = $"Price of {LongBarrel.UPGRADE_NAME} Upgrade";
        internal const int LONG_BARREL_PRICE_DEFAULT = 500;

        internal const string LONG_BARREL_INITIAL_SHOTGUN_RANGE_INCREASE_KEY = "Initial Shotgun Range Increase";
        internal const int LONG_BARREL_INITIAL_SHOTGUN_RANGE_INCREASE_DEFAULT = 10;
        internal const string LONG_BARREL_INITIAL_SHOTGUN_RANGE_INCREASE_DESCRIPTION = "Initial range percentage increase of the shotgun when first purchasing the upgrade";

        internal const string LONG_BARREL_INCREMENTAL_SHOTGUN_RANGE_INCREASE_KEY = "Incremental Shotgun Range Increase";
        internal const int LONG_BARREL_INCREMENTAL_SHOTGUN_RANGE_INCREASE_DEFAULT = 10;
        internal const string LONG_BARREL_INCREMENTAL_SHOTGUN_RANGE_INCREASE_DESCRIPTION = "Incremental range percentage increase of the shotgun when purchasing further levels of the upgrade";

        #endregion

        #region Hollow Point

        internal const string HOLLOW_POINT_ENABLED_KEY = $"Enable {HollowPoint.UPGRADE_NAME} Upgrade";
        internal const bool HOLLOW_POINT_ENABLED_DEFAULT = true;
        internal const string HOLLOW_POINT_ENABLED_DESCRIPTION = "Tier upgrade which increases the amount of damage done by the shotgun.";

        internal const string HOLLOW_POINT_PRICE_KEY = $"Price of {HollowPoint.UPGRADE_NAME} Upgrade";
        internal const int HOLLOW_POINT_PRICE_DEFAULT = 750;

        internal const string HOLLOW_POINT_INITIAL_SHOTGUN_DAMAGE_INCREASE_KEY = "Initial Shotgun Damage Increase";
        internal const int HOLLOW_POINT_INITIAL_SHOTGUN_DAMAGE_INCREASE_DEFAULT = 1;
        internal const string HOLLOW_POINT_INITIAL_SHOTGUN_DAMAGE_INCREASE_DESCRIPTION = "Initial damage increase of the shotgun when first purchasing the upgrade";

        internal const string HOLLOW_POINT_INCREMENTAL_SHOTGUN_DAMAGE_INCREASE_KEY = "Incremental Shotgun Damage Increase";
        internal const int HOLLOW_POINT_INCREMENTAL_SHOTGUN_DAMAGE_INCREASE_DEFAULT = 1;
        internal const string HOLLOW_POINT_INCREMENTAL_SHOTGUN_DAMAGE_INCREASE_DESCRIPTION = "Incremental damage increase of the shotgun when purchasing further levels of the upgrade";

        #endregion

        #region Jetpack Thrusters

        internal const string JETPACK_THRUSTERS_ENABLED_KEY = $"Enable {JetpackThrusters.UPGRADE_NAME} Upgrade";
        internal const bool JETPACK_THRUSTERS_ENABLED_DEFAULT = true;
        internal const string JETPACK_THRUSTERS_ENABLED_DESCRIPTION = "Tier upgrade which increases the maximum speed of the jetpack during flight.";

        internal const string JETPACK_THRUSTERS_PRICE_KEY = $"Price of {JetpackThrusters.UPGRADE_NAME} Upgrade";
        internal const int JETPACK_THRUSTERS_PRICE_DEFAULT = 300;

        internal const string JETPACK_THRUSTERS_INITIAL_MAXIMUM_POWER_INCREASE_KEY = "Initial Maximum Speed Increase";
        internal const int JETPACK_THRUSTERS_INITIAL_MAXIMUM_POWER_INCREASE_DEFAULT = 20;
        internal const string JETPACK_THRUSTERS_INITIAL_MAXIMUM_POWER_INCREASE_DESCRIPTION = "Initial percentage increase of jetpack's maximum speed during flight when first purchasing the upgrade";

        internal const string JETPACK_THRUSTERS_INCREMENTAL_MAXIMUM_POWER_INCREASE_KEY = "Incremental Maximum Speed Increase";
        internal const int JETPACK_THRUSTERS_INCREMENTAL_MAXIMUM_POWER_INCREASE_DEFAULT = 20;
        internal const string JETPACK_THRUSTERS_INCREMENTAL_MAXIMUM_POWER_INCREASE_DESCRIPTION = "Incremental percentage increase of jetpack's maximum speed during flight when purchasing further levels of the upgrade";

        #endregion

        #region Jet Fuel

        internal const string JET_FUEL_ENABLED_KEY = $"Enable {JetFuel.UPGRADE_NAME} Upgrade";
        internal const bool JET_FUEL_ENABLED_DEFAULT = true;
        internal const string JET_FUEL_ENABLED_DESCRIPTION = "Tier upgrade which increases the acceleration of the jetpack during flight.";

        internal const string JET_FUEL_PRICE_KEY = $"Price of {JetFuel.UPGRADE_NAME} Upgrade";
        internal const int JET_FUEL_PRICE_DEFAULT = 400;

        internal const string JET_FUEL_INITIAL_ACCELERATION_INCREASE_KEY = "Initial Acceleration Increase";
        internal const int JET_FUEL_INITIAL_ACCELERATION_INCREASE_DEFAULT = 20;
        internal const string JET_FUEL_INITIAL_ACCELERATION_INCREASE_DESCRIPTION = "Initial percentage increase of jetpack acceleration when first purchasing the upgrade";

        internal const string JET_FUEL_INCREMENTAL_ACCELERATION_INCREASE_KEY = "Incremental Acceleration Increase";
        internal const int JET_FUEL_INCREMENTAL_ACCELERATION_INCREASE_DEFAULT = 20;
        internal const string JET_FUEL_INCREMENTAL_ACCELERATION_INCREASE_DESCRIPTION = "Incremental percentage increase of jetpack acceleration when purchasing further levels of the upgrade";

        #endregion

        #region Quick Hands

        internal const string QUICK_HANDS_ENABLED_KEY = $"Enable {QuickHands.UPGRADE_NAME} Upgrade";
        internal const bool QUICK_HANDS_ENABLED_DEFAULT = true;
        internal const string QUICK_HANDS_ENABLED_DESCRIPTION = "Tier upgrade which increases the interaction speed of the player.";

        internal const string QUICK_HANDS_PRICE_KEY = $"Price of {QuickHands.UPGRADE_NAME} Upgrade";
        internal const int QUICK_HANDS_PRICE_DEFAULT = 100;

        internal const string QUICK_HANDS_INITIAL_INTERACTION_SPEED_INCREASE_KEY = "Initial Interaction Speed Increase";
        internal const int QUICK_HANDS_INITIAL_INTERACTION_SPEED_INCREASE_DEFAULT = 20;
        internal const string QUICK_HANDS_INITIAL_INTERACTION_SPEED_INCREASE_DESCRIPTION = "Initial percentage increase of interaction speed when first purchasing the upgrade";

        internal const string QUICK_HANDS_INCREMENTAL_INTERACTION_SPEED_INCREASE_KEY = "Incremental Interaction Speed Increase";
        internal const int QUICK_HANDS_INCREMENTAL_INTERACTION_SPEED_INCREASE_DEFAULT = 20;
        internal const string QUICK_HANDS_INCREMENTAL_INTERACTION_SPEED_INCREASE_DESCRIPTION = "Incremental percentage increase of interaction speed when purchasing further levels of the upgrade";

        #endregion

        #region Midas Touch

        internal const string MIDAS_TOUCH_ENABLED_KEY = $"Enable {MidasTouch.UPGRADE_NAME} Upgrade";
        internal const bool MIDAS_TOUCH_ENABLED_DEFAULT = true;
        internal const string MIDAS_TOUCH_ENABLED_DESCRIPTION = "Tier upgrade which increases the value of the scrap found in the moons.";

        internal const string MIDAS_TOUCH_PRICE_KEY = $"Price of {MidasTouch.UPGRADE_NAME} Upgrade";
        internal const int MIDAS_TOUCH_PRICE_DEFAULT = 1000;

        internal const string MIDAS_TOUCH_INITIAL_SCRAP_VALUE_INCREASE_KEY = "Initial Scrap Value Increase";
        internal const int MIDAS_TOUCH_INITIAL_SCRAP_VALUE_INCREASE_DEFAULT = 20;
        internal const string MIDAS_TOUCH_INITIAL_SCRAP_VALUE_INCREASE_DESCRIPTION = "Initial percentage increase of value of the scrap found in the moons when first purchasing the upgrade";

        internal const string MIDAS_TOUCH_INCREMENTAL_SCRAP_VALUE_INCREASE_KEY = "Incremental Scrap Value Increase";
        internal const int MIDAS_TOUCH_INCREMENTAL_SCRAP_VALUE_INCREASE_DEFAULT = 20;
        internal const string MIDAS_TOUCH_INCREMENTAL_SCRAP_VALUE_INCREASE_DESCRIPTION = "Incremental percentage increase of value of the scrap found in the moons when purchasing further levels of the upgrade";

        #endregion

        #region Carbon Kneejoints

        internal const string CARBON_KNEEJOINTS_ENABLED_KEY = $"Enable {CarbonKneejoints.UPGRADE_NAME} Upgrade";
        internal const bool CARBON_KNEEJOINTS_ENABLED_DEFAULT = true;
        internal const string CARBON_KNEEJOINTS_ENABLED_DESCRIPTION = "Tier upgrade which reduces the movement speed loss due to crouching.";

        internal const string CARBON_KNEEJOINTS_PRICE_KEY = $"Price of {CarbonKneejoints.UPGRADE_NAME} Upgrade";
        internal const int CARBON_KNEEJOINTS_PRICE_DEFAULT = 100;

        internal const string CARBON_KNEEJOINTS_INITIAL_CROUCH_DEBUFF_DECREASE_KEY = "Initial Crouch Movement Speed Debuff Decrease";
        internal const int CARBON_KNEEJOINTS_INITIAL_CROUCH_DEBUFF_DECREASE_DEFAULT = 20;
        internal const string CARBON_KNEEJOINTS_INITIAL_CROUCH_DEBUFF_DECREASE_DESCRIPTION = "Initial percentage decrease of the movement speed loss while crouching when first purchasing the upgrade";

        internal const string CARBON_KNEEJOINTS_INCREMENTAL_CROUCH_DEBUFF_DECREASE_KEY = "Incremental Crouch Movement Speed Debuff Decrease";
        internal const int CARBON_KNEEJOINTS_INCREMENTAL_CROUCH_DEBUFF_DECREASE_DEFAULT = 20;
        internal const string CARBON_KNEEJOINTS_INCREMENTAL_CROUCH_DEBUFF_DECREASE_DESCRIPTION = "Incremental percentage decrease of the movement speed loss while crouching when purchasing further levels of the upgrade";

        #endregion

        #region Life Insurance

        internal const string LIFE_INSURANCE_ENABLED_KEY = $"Enable {LifeInsurance.UPGRADE_NAME} Upgrade";
        internal const bool LIFE_INSURANCE_ENABLED_DEFAULT = true;
        internal const string LIFE_INSURANCE_ENABLED_DESCRIPTION = "Tier upgrade which reduces the credit loss from leaving a body behind when exiting a moon.";

        internal const string LIFE_INSURANCE_PRICE_KEY = $"Price of {LifeInsurance.UPGRADE_NAME} Upgrade";
        internal const int LIFE_INSURANCE_PRICE_DEFAULT = 200;

        internal const string LIFE_INSURANCE_INITIAL_COST_PERCENTAGE_DECREASE_KEY = "Initial Credit Loss Decrease";
        internal const int LIFE_INSURANCE_INITIAL_COST_PERCENTAGE_DECREASE_DEFAULT = 20;
        internal const string LIFE_INSURANCE_INITIAL_COST_PERCENTAGE_DECREASE_DESCRIPTION = "Initial percentage decrease of the credit loss when first purchasing the upgrade";

        internal const string LIFE_INSURANCE_INCREMENTAL_COST_PERCENTAGE_DECREASE_KEY = "Incremental Credit Loss Decrease";
        internal const int LIFE_INSURANCE_INCREMENTAL_COST_PERCENTAGE_DECREASE_DEFAULT = 20;
        internal const string LIFE_INSURANCE_INCREMENTAL_COST_PERCENTAGE_DECREASE_DESCRIPTION = "Incremental percentage decrease of the credit loss when purchasing further levels of the upgrade";

        #endregion

        #region Rubber Boots

        internal const string RUBBER_BOOTS_ENABLED_KEY = $"Enable {RubberBoots.UPGRADE_NAME} Upgrade";
        internal const bool RUBBER_BOOTS_ENABLED_DEFAULT = true;
        internal const string RUBBER_BOOTS_ENABLED_DESCRIPTION = "Tier upgrade which reduces the movement hinderance when walking on water surfaces.";

        internal const string RUBBER_BOOTS_PRICE_KEY = $"Price of {RubberBoots.UPGRADE_NAME} Upgrade";
        internal const int RUBBER_BOOTS_PRICE_DEFAULT = 50;

        internal const string RUBBER_BOOTS_INITIAL_MOVEMENT_HINDERANCE_DECREASE_KEY = "Initial Movement Hinderance Decrease";
        internal const int RUBBER_BOOTS_INITIAL_MOVEMENT_HINDERANCE_DECREASE_DEFAULT = 20;
        internal const string RUBBER_BOOTS_INITIAL_MOVEMENT_HINDERANCE_DECREASE_DESCRIPTION = "Initial percentage decrease of movement hinderance on water surfaces when first purchasing the upgrade";

        internal const string RUBBER_BOOTS_INCREMENTAL_MOVEMENT_HINDERANCE_DECREASE_KEY = "Incremental Oxygen Consumption Decrease";
        internal const int RUBBER_BOOTS_INCREMENTAL_MOVEMENT_HINDERANCE_DECREASE_DEFAULT = 20;
        internal const string RUBBER_BOOTS_INCREMENTAL_MOVEMENT_HINDERANCE_DECREASE_DESCRIPTION = "Incremental percentage decrease of movement hinderance on water surfaces when purchasing further levels of the upgrade";

        #endregion

        #region Oxygen Canisters

        internal const string OXYGEN_CANISTERS_ENABLED_KEY = $"Enable {OxygenCanisters.UPGRADE_NAME} Upgrade";
        internal const bool OXYGEN_CANISTERS_ENABLED_DEFAULT = true;
        internal const string OXYGEN_CANISTERS_ENABLED_DESCRIPTION = "Tier upgrade which reduces the consumption rate of oxygen while underwater.";

        internal const string OXYGEN_CANISTERS_PRICE_KEY = $"Price of {OxygenCanisters.UPGRADE_NAME} Upgrade";
        internal const int OXYGEN_CANISTERS_PRICE_DEFAULT = 100;

        internal const string OXYGEN_CANISTERS_INITIAL_OXYGEN_CONSUMPTION_DECREASE_KEY = "Initial Oxygen Consumption Decrease";
        internal const int OXYGEN_CANISTERS_INITIAL_OXYGEN_CONSUMPTION_DECREASE_DEFAULT = 20;
        internal const string OXYGEN_CANISTERS_INITIAL_OXYGEN_CONSUMPTION_DECREASE_DESCRIPTION = "Initial percentage of oxygen consumption decrease when first purchasing the upgrade";

        internal const string OXYGEN_CANISTERS_INCREMENTAL_OXYGEN_CONSUMPTION_DECREASE_KEY = "Incremental Oxygen Consumption Decrease";
        internal const int OXYGEN_CANISTERS_INCREMENTAL_OXYGEN_CONSUMPTION_DECREASE_DEFAULT = 20;
        internal const string OXYGEN_CANISTERS_INCREMENTAL_OXYGEN_CONSUMPTION_DECREASE_DESCRIPTION = "Incremental percentage of oxygen consumption decrease when purchasing further levels of the upgrade";

        #endregion

        #region Sleight of Hand

        internal const string SLEIGHT_OF_HAND_ENABLED_KEY = $"Enable {SleightOfHand.UPGRADE_NAME} Upgrade";
        internal const bool SLEIGHT_OF_HAND_ENABLED_DEFAULT = true;
        internal const string SLEIGHT_OF_HAND_ENABLED_DESCRIPTION = "Tier upgrade which reduces the reload time of the shotgun.";

        internal const string SLEIGHT_OF_HAND_PRICE_KEY = $"Price of {SleightOfHand.UPGRADE_NAME} Upgrade";
        internal const int SLEIGHT_OF_HAND_PRICE_DEFAULT = 100;

        internal const string SLEIGHT_OF_HAND_INITIAL_INCREASE_KEY = "Initial Reload Speed Increase";
        internal const int SLEIGHT_OF_HAND_INITIAL_INCREASE_DEFAULT = 25;
        internal const string SLEIGHT_OF_HAND_INITIAL_INCREASE_DESCRIPTION = "Initial percentage of the reload speed increase when first purchasing the upgrade";

        internal const string SLEIGHT_OF_HAND_INCREMENTAL_INCREASE_KEY = "Incremental Reload Speed Increase";
        internal const int SLEIGHT_OF_HAND_INCREMENTAL_INCREASE_DEFAULT = 25;
        internal const string SLEIGHT_OF_HAND_INCREMENTAL_INCREASE_DESCRIPTION = "Incremental percentage of the reload speed increase when purchasing further levels of the upgrade";

        #endregion

        #region Hiking Boots

        internal const string HIKING_BOOTS_ENABLED_KEY = $"Enable {HikingBoots.UPGRADE_NAME} Upgrade";
        internal const bool HIKING_BOOTS_ENABLED_DEFAULT = true;
        internal const string HIKING_BOOTS_ENABLED_DESCRIPTION = "Tier upgrade which reduces the impact of going through uphill slopes";

        internal const string HIKING_BOOTS_PRICE_KEY = $"Price of {HikingBoots.UPGRADE_NAME} Upgrade";
        internal const int HIKING_BOOTS_PRICE_DEFAULT = 75;

        internal const string HIKING_BOOTS_INITIAL_DECREASE_KEY = "Initial Uphill Slope Debuff Decrease";
        internal const int HIKING_BOOTS_INITIAL_DECREASE_DEFAULT = 25;
        internal const string HIKING_BOOTS_INITIAL_DECREASE_DESCRIPTION = "Initial percentage of the uphill slope debuff removed when first purchasing the upgrade";

        internal const string HIKING_BOOTS_INCREMENTAL_DECREASE_KEY = "Incremental Uphill Slope Debuff Decrease";
        internal const int HIKING_BOOTS_INCREMENTAL_DECREASE_DEFAULT = 25;
        internal const string HIKING_BOOTS_INCREMENTAL_DECREASE_DESCRIPTION = "Incremental percetange of the uphill slope debuff removed when purchasing further levels of the upgrade";

        #endregion

        #region Traction Boots

        internal const string TRACTION_BOOTS_ENABLED_KEY = $"Enable {TractionBoots.UPGRADE_NAME} Upgrade";
        internal const bool TRACTION_BOOTS_ENABLED_DEFAULT = true;
        internal const string TRACTION_BOOTS_ENABLED_DESCRIPTION = "Tier upgrade which increases the traction factor on the player's movement";

        internal const string TRACTION_BOOTS_PRICE_KEY = $"Price of {TractionBoots.UPGRADE_NAME} Upgrade";
        internal const int TRACTION_BOOTS_PRICE_DEFAULT = 100;

        internal const string TRACTION_BOOTS_INITIAL_INCREASE_KEY = "Initial Traction Increase";
        internal const int TRACTION_BOOTS_INITIAL_INCREASE_DEFAULT = 25;
        internal const string TRACTION_BOOTS_INITIAL_INCREASE_DESCRIPTION = "Multiplier value (%) applied to the player's walking force when first purchasing the upgrade.";

        internal const string TRACTION_BOOTS_INCREMENTAL_INCREASE_KEY = "Incremental Traction Increase";
        internal const int TRACTION_BOOTS_INCREMENTAL_INCREASE_DEFAULT = 25;
        internal const string TRACTION_BOOTS_INCREMENTAL_INCREASE_DESCRIPTION = "Multiplier value (%) added to the initial which then gets applied to the player's walking force when first purchasing the upgrade.";

        #endregion

        #region Fedora Suit

        internal const string FEDORA_SUIT_ENABLED_KEY = $"Enable {FedoraSuit.UPGRADE_NAME} Upgrade";
        internal const bool FEDORA_SUIT_ENABLED_DEFAULT = true;
        internal const string FEDORA_SUIT_ENABLED_DESCRIPTION = "One Time upgrade which makes Butler enemies not be angry at you when alone. (It will still be angry when you hit them)";

        internal const string FEDORA_SUIT_PRICE_KEY = $"Price of {FedoraSuit.UPGRADE_NAME} Upgrade";
        internal const int FEDORA_SUIT_PRICE_DEFAULT = 750;

        #endregion

        #region Weed Genetic Manipulation

        internal const string WEED_GENETIC_MANIPULATION_ENABLED_KEY = $"Enable {WeedGeneticManipulation.UPGRADE_NAME} Upgrade";
        internal const bool WEED_GENETIC_MANIPULATION_ENABLED_DEFAULT = true;
        internal const string WEED_GENETIC_MANIPULATION_ENABLED_DESCRIPTION = "Tier upgrade which increases the effectiveness of the Weed Killer on eradicating plants";

        internal const string WEED_GENETIC_MANIPULATION_PRICE_KEY = $"Price of {WeedGeneticManipulation.UPGRADE_NAME} Upgrade";
        internal const int WEED_GENETIC_MANIPULATION_PRICE_DEFAULT = 100;

        internal const string WEED_GENETIC_INITIAL_EFFECTIVENESS_INCREASE_KEY = "Initial Effectiveness Increase";
        internal const int WEED_GENETIC_INITIAL_EFFECTIVENESS_INCREASE_DEFAULT = 75;
        internal const string WEED_GENETIC_INITIAL_EFFECTIVENESS_INCREASE_DESCRIPTION = "Amount of effectiveness (%) when first purchasing the upgrade increased on the Weed Killer to eradicate plants";

        internal const string WEED_GENETIC_INCREMENTAL_EFFECTIVENESS_INCREASE_KEY = "Incremental Effectiveness Increase";
        internal const int WEED_GENETIC_INCREMENTAL_EFFECTIVENESS_INCREASE_DEFAULT = 50;
        internal const string WEED_GENETIC_INCREMENTAL_EFFECTIVENESS_INCREASE_DESCRIPTION = "Amount of effectiveness (%) when purchasing further levels of the upgrade increased on the Weed Killer to eradicate plants";

        #endregion

        #region Clay Glasses

        internal const string CLAY_GLASSES_ENABLED_KEY = $"Enable {ClayGlasses.UPGRADE_NAME} Upgrade";
        internal const bool CLAY_GLASSES_ENABLED_DEFAULT = true;
        internal const string CLAY_GLASSES_ENABLED_DESCRIPTION = "Tier upgrade which increases the distance to start seeing the Clay Surgeon Entity.";

        internal const string CLAY_GLASSES_PRICE_KEY = $"Price of {ClayGlasses.UPGRADE_NAME} Upgrade";
        internal const int CLAY_GLASSES_PRICE_DEFAULT = 200;

        internal const string CLAY_GLASSES_DISTANCE_INITIAL_INCREASE_KEY = "Initial Distance Increase";
        internal const float CLAY_GLASSES_DISTANCE_INITIAL_INCREASE_DEFAULT = 10f;
        internal const string CLAY_GLASSES_DISTANCE_INITIAL_INCREASE_DESCRIPTION = "Distance increased when first purchasing the upgrade to be able to spot the Clay Surgeon";

        internal const string CLAY_GLASSES_DISTANCE_INCREMENTAL_INCREASE_KEY = "Incremental Distance Increase";
        internal const float CLAY_GLASSES_DISTANCE_INCREMENTAL_INCREASE_DEFAULT = 5f;
        internal const string CLAY_GLASSES_DISTANCE_INCREMENTAL_INCREASE_DESCRIPTION = "Distance increased when purchasing further levels of the upgrade to be able to spot the Clay Surgeon";

        #endregion

        #region Hunter

        internal const string HUNTER_ENABLED_KEY = $"Enable {Hunter.UPGRADE_NAME} Upgrade";
        internal const bool HUNTER_ENABLED_DEFAULT = true;
        internal const string HUNTER_ENABLED_DESCRIPTION = "Collect and sell samples from dead enemies";

        internal const string HUNTER_PRICE_KEY = $"Price of {Hunter.UPGRADE_NAME} Upgrade";
        internal const int HUNTER_PRICE_DEFAULT = 700;

        internal const string HUNTER_SAMPLE_TIERS_KEY = "Samples dropping at each tier";
        internal const string HUNTER_SAMPLE_TIERS_DEFAULT = "Hoarding Bug, Centipede, Spore Lizard-Bunker Spider, Baboon hawk, Tulip Snake, Kidnapper Fox-Flowerman, MouthDog, Crawler, Manticoil, Maneater-Forest Giant";
        internal const string HUNTER_SAMPLE_TIERS_DESCRIPTION = "Specifies at which tier of Hunter do each sample start dropping from. Each tier is separated with a dash ('-') and each list of monsters will be separated with a comma (',')\n" +
                                                                "Supported Enemies: Hoarding Bug, Centipede (Snare Flea),Bunker Spider, Baboon Hawk, Crawler (Half/Thumper), " +
                                                                "Flowerman (Bracken), MouthDog (Eyeless Dog), Forest Giant, Tulip Snake, Manticoil, Kidnapper Fox and Spore Lizard.";

        internal const string MINIMUM_SAMPLE_VALUE_FORMAT = "Minimum scrap value of a {0} sample";
        internal const string MAXIMUM_SAMPLE_VALUE_FORMAT = "Maximum scrap value of a {0} sample";

        #region Snare Flea

        internal static readonly string SNARE_FLEA_SAMPLE_MINIMUM_VALUE_KEY = string.Format(MINIMUM_SAMPLE_VALUE_FORMAT, "Snare Flea");
        internal const int SNARE_FLEA_SAMPLE_MINIMUM_VALUE_DEFAULT = 35;

        internal static readonly string SNARE_FLEA_SAMPLE_MAXIMUM_VALUE_KEY = string.Format(MAXIMUM_SAMPLE_VALUE_FORMAT, "Snare Flea");
        internal const int SNARE_FLEA_SAMPLE_MAXIMUM_VALUE_DEFAULT = 60;

        #endregion

        #region Bunker Spider

        internal static readonly string BUNKER_SPIDER_SAMPLE_MINIMUM_VALUE_KEY = string.Format(MINIMUM_SAMPLE_VALUE_FORMAT, "Bunker Spider");
        internal const int BUNKER_SPIDER_SAMPLE_MINIMUM_VALUE_DEFAULT = 65;

        internal static readonly string BUNKER_SPIDER_SAMPLE_MAXIMUM_VALUE_KEY = string.Format(MAXIMUM_SAMPLE_VALUE_FORMAT, "Bunker Spider");
        internal const int BUNKER_SPIDER_SAMPLE_MAXIMUM_VALUE_DEFAULT = 95;

        #endregion

        #region Hoarding Bug

        internal static readonly string HOARDING_BUG_SAMPLE_MINIMUM_VALUE_KEY = string.Format(MINIMUM_SAMPLE_VALUE_FORMAT, "Hoarding Bug");
        internal const int HOARDING_BUG_SAMPLE_MINIMUM_VALUE_DEFAULT = 45;

        internal static readonly string HOARDING_BUG_SAMPLE_MAXIMUM_VALUE_KEY = string.Format(MAXIMUM_SAMPLE_VALUE_FORMAT, "Hoarding Bug");
        internal const int HOARDING_BUG_SAMPLE_MAXIMUM_VALUE_DEFAULT = 75;

        #endregion

        #region Bracken

        internal static readonly string BRACKEN_SAMPLE_MINIMUM_VALUE_KEY = string.Format(MINIMUM_SAMPLE_VALUE_FORMAT, "Bracken");
        internal const int BRACKEN_SAMPLE_MINIMUM_VALUE_DEFAULT = 80;

        internal static readonly string BRACKEN_SAMPLE_MAXIMUM_VALUE_KEY = string.Format(MAXIMUM_SAMPLE_VALUE_FORMAT, "Bracken");
        internal const int BRACKEN_SAMPLE_MAXIMUM_VALUE_DEFAULT = 125;

        #endregion

        #region Eyeless Dog

        internal static readonly string EYELESS_DOG_SAMPLE_MINIMUM_VALUE_KEY = string.Format(MINIMUM_SAMPLE_VALUE_FORMAT, "Eyeless Dog");
        internal const int EYELESS_DOG_SAMPLE_MINIMUM_VALUE_DEFAULT = 100;

        internal static readonly string EYELESS_DOG_SAMPLE_MAXIMUM_VALUE_KEY = string.Format(MAXIMUM_SAMPLE_VALUE_FORMAT, "Eyeless Dog");
        internal const int EYELESS_DOG_SAMPLE_MAXIMUM_VALUE_DEFAULT = 150;

        #endregion

        #region Baboon Hawk

        internal static readonly string BABOON_HAWK_SAMPLE_MINIMUM_VALUE_KEY = string.Format(MINIMUM_SAMPLE_VALUE_FORMAT, "Baboon Hawk");
        internal const int BABOON_HAWK_SAMPLE_MINIMUM_VALUE_DEFAULT = 75;

        internal static readonly string BABOON_HAWK_SAMPLE_MAXIMUM_VALUE_KEY = string.Format(MAXIMUM_SAMPLE_VALUE_FORMAT, "Baboon Hawk");
        internal const int BABOON_HAWK_SAMPLE_MAXIMUM_VALUE_DEFAULT = 115;

        #endregion

        #region Half

        internal static readonly string THUMPER_SAMPLE_MINIMUM_VALUE_KEY = string.Format(MINIMUM_SAMPLE_VALUE_FORMAT, "Half");
        internal const int THUMPER_SAMPLE_MINIMUM_VALUE_DEFAULT = 80;

        internal static readonly string THUMPER_SAMPLE_MAXIMUM_VALUE_KEY = string.Format(MAXIMUM_SAMPLE_VALUE_FORMAT, "Half");
        internal const int THUMPER_SAMPLE_MAXIMUM_VALUE_DEFAULT = 125;

        #endregion

        #region Forest Giant

        internal static readonly string FOREST_KEEPER_SAMPLE_MINIMUM_VALUE_KEY = string.Format(MINIMUM_SAMPLE_VALUE_FORMAT, "Forest Keeper");
        internal const int FOREST_KEEPER_SAMPLE_MINIMUM_VALUE_DEFAULT = 80;

        internal static readonly string FOREST_KEEPER_SAMPLE_MAXIMUM_VALUE_KEY = string.Format(MAXIMUM_SAMPLE_VALUE_FORMAT, "Forest Keeper");
        internal const int FOREST_KEEPER_SAMPLE_MAXIMUM_VALUE_DEFAULT = 125;

        #endregion

        #region Manticoil

        internal static readonly string MANTICOIL_SAMPLE_MINIMUM_VALUE_KEY = string.Format(MINIMUM_SAMPLE_VALUE_FORMAT, "Manticoil");
        internal const int MANTICOIL_SAMPLE_MINIMUM_VALUE_DEFAULT = 40;

        internal static readonly string MANTICOIL_SAMPLE_MAXIMUM_VALUE_KEY = string.Format(MAXIMUM_SAMPLE_VALUE_FORMAT, "Manticoil");
        internal const int MANTICOIL_SAMPLE_MAXIMUM_VALUE_DEFAULT = 75;

        #endregion

        #region Tulip Snake

        internal static readonly string TULIP_SNAKE_SAMPLE_MINIMUM_VALUE_KEY = string.Format(MINIMUM_SAMPLE_VALUE_FORMAT, "Tulip Snake");
        internal const int TULIP_SNAKE_SAMPLE_MINIMUM_VALUE_DEFAULT = 30;

        internal static readonly string TULIP_SNAKE_SAMPLE_MAXIMUM_VALUE_KEY = string.Format(MAXIMUM_SAMPLE_VALUE_FORMAT, "Tulip Snake");
        internal const int TULIP_SNAKE_SAMPLE_MAXIMUM_VALUE_DEFAULT = 50;

        #endregion

        #region Kidnapper Fox

        internal static readonly string KIDNAPPER_FOX_SAMPLE_MINIMUM_VALUE_KEY = string.Format(MINIMUM_SAMPLE_VALUE_FORMAT, "Kidnapper Fox");
        internal const int KIDNAPPER_FOX_SAMPLE_MINIMUM_VALUE_DEFAULT = 60;

        internal static readonly string KIDNAPPER_FOX_SAMPLE_MAXIMUM_VALUE_KEY = string.Format(MAXIMUM_SAMPLE_VALUE_FORMAT, "Kidnapper Fox");
        internal const int KIDNAPPER_FOX_SAMPLE_MAXIMUM_VALUE_DEFAULT = 80;

        #endregion

        #region Spore Lizard

        internal static readonly string SPORE_LIZARD_SAMPLE_MINIMUM_VALUE_KEY = string.Format(MINIMUM_SAMPLE_VALUE_FORMAT, "Spore Lizard");
        internal const int SPORE_LIZARD_SAMPLE_MINIMUM_VALUE_DEFAULT = 30;

        internal static readonly string SPORE_LIZARD_SAMPLE_MAXIMUM_VALUE_KEY = string.Format(MAXIMUM_SAMPLE_VALUE_FORMAT, "Spore Lizard");
        internal const int SPORE_LIZARD_SAMPLE_MAXIMUM_VALUE_DEFAULT = 50;

        #endregion

        #region Maneater

        internal static readonly string MANEATER_SAMPLE_MINIMUM_VALUE_KEY = string.Format(MINIMUM_SAMPLE_VALUE_FORMAT, "Maneater");
        internal const int MANEATER_SAMPLE_MINIMUM_VALUE_DEFAULT = 80;

        internal static readonly string MANEATER_SAMPLE_MAXIMUM_VALUE_KEY = string.Format(MAXIMUM_SAMPLE_VALUE_FORMAT, "Maneater");
        internal const int MANEATER_SAMPLE_MAXIMUM_VALUE_DEFAULT = 120;

        #endregion

        #endregion

        #region Night Vision

        internal const string NIGHT_VISION_ENABLED_KEY = $"Enable {NightVision.SIMPLE_UPGRADE_NAME} Upgrade";
        internal const bool NIGHT_VISION_ENABLED_DEFAULT = true;
        internal const string NIGHT_VISION_ENABLED_DESCRIPTION = "Toggleable night vision.";

        internal const string NIGHT_VISION_PRICE_KEY = $"Price of {NightVision.SIMPLE_UPGRADE_NAME} Upgrade";
        internal const int NIGHT_VISION_PRICE_DEFAULT = 380;

        internal const string NIGHT_VISION_BATTERY_MAX_KEY = $"The max charge for your {NightVision.SIMPLE_UPGRADE_NAME} battery";
        internal const float NIGHT_VISION_BATTERY_MAX_DEFAULT = 10f;
        internal const string NIGHT_VISION_BATTERY_MAX_DESCRIPTION = "Default settings this will be the unupgraded time in seconds the battery will drain and regen in. Increase to increase battery life.";

        internal const string NIGHT_VISION_DRAIN_SPEED_KEY = "Multiplier for night vis battery drain";
        internal const float NIGHT_VISION_DRAIN_SPEED_DEFAULT = 1f;
        internal const string NIGHT_VISION_DRAIN_SPEED_DESCRIPTION = "Multiplied by timedelta, lower to increase battery life.";

        internal const string NIGHT_VISION_REGEN_SPEED_KEY = "Multiplier for night vis battery regen";
        internal const float NIGHT_VISION_REGEN_SPEED_DEFAULT = 1f;
        internal const string NIGHT_VISION_REGEN_SPEED_DESCRIPTION = "Multiplied by timedelta, raise to speed up battery regen time.";

        internal const string NIGHT_VISION_COLOR_KEY = $"{NightVision.SIMPLE_UPGRADE_NAME} Color";
        internal const string NIGHT_VISION_COLOR_DEFAULT = HEXADECIMAL_GREEN + "FF";
        internal const string NIGHT_VISION_COLOR_DESCRIPTION = $"The color your {NightVision.SIMPLE_UPGRADE_NAME} light emits.";

        internal const string NIGHT_VISION_UI_TEXT_COLOR_KEY = $"{NightVision.SIMPLE_UPGRADE_NAME} UI Text Color";
        internal const string NIGHT_VISION_UI_TEXT_COLOR_DEFAULT = HEXADECIMAL_WHITE + "FF";
        internal const string NIGHT_VISION_UI_TEXT_COLOR_DESCRIPTION = $"The color used for the {NightVision.SIMPLE_UPGRADE_NAME}'s UI text.";

        internal const string NIGHT_VISION_UI_BAR_COLOR_KEY = $"{NightVision.SIMPLE_UPGRADE_NAME} UI Bar Color";
        internal const string NIGHT_VISION_UI_BAR_COLOR_DEFAULT = HEXADECIMAL_GREEN + "FF";
        internal const string NIGHT_VISION_UI_BAR_COLOR_DESCRIPTION = $"The color used for the {NightVision.SIMPLE_UPGRADE_NAME}'s UI battery bar.";

        internal const string NIGHT_VISION_RANGE_KEY = $"{NightVision.SIMPLE_UPGRADE_NAME} Range";
        internal const float NIGHT_VISION_RANGE_DEFAULT = 2000f;
        internal const string NIGHT_VISION_RANGE_DESCRIPTION = $"Kind of like the distance your {NightVision.SIMPLE_UPGRADE_NAME} travels.";

        internal const string NIGHT_VISION_RANGE_INCREMENT_KEY = $"{NightVision.SIMPLE_UPGRADE_NAME} Range Increment";
        internal const float NIGHT_VISION_RANGE_INCREMENT_DEFAULT = 0f;
        internal const string NIGHT_VISION_RANGE_INCREMENT_DESCRIPTION = "Increases your range by this value each upgrade.";

        internal const string NIGHT_VISION_INTENSITY_KEY = $"{NightVision.SIMPLE_UPGRADE_NAME} Intensity";
        internal const float NIGHT_VISION_INTENSITY_DEFAULT = 1000f;
        internal const string NIGHT_VISION_INTENSITY_DESCRIPTION = $"Kind of like the brightness of your {NightVision.SIMPLE_UPGRADE_NAME}.";

        internal const string NIGHT_VISION_INTENSITY_INCREMENT_KEY = $"{NightVision.SIMPLE_UPGRADE_NAME} Intensity Increment";
        internal const float NIGHT_VISION_INTENSITY_INCREMENT_DEFAULT = 0f;
        internal const string NIGHT_VISION_INTENSITY_INCREMENT_DESCRIPTION = "Increases your intensity by this value each upgrade.";

        internal const string NIGHT_VISION_STARTUP_KEY = $"{NightVision.SIMPLE_UPGRADE_NAME} StartUp Cost";
        internal const float NIGHT_VISION_STARTUP_DEFAULT = 0.1f;
        internal const string NIGHT_VISION_STARTUP_DESCRIPTION = "The percent battery drained when turned on (0.1 = 10%).";

        internal const string NIGHT_VISION_EXHAUST_KEY = $"{NightVision.SIMPLE_UPGRADE_NAME} Exhaustion";
        internal const float NIGHT_VISION_EXHAUST_DEFAULT = 2f;
        internal const string NIGHT_VISION_EXHAUST_DESCRIPTION = $"How many seconds {NightVision.SIMPLE_UPGRADE_NAME} stays fully depleted.";

        internal const string NIGHT_VISION_DRAIN_INCREMENT_KEY = $"Decrease for {NightVision.SIMPLE_UPGRADE_NAME} battery drain";
        internal const float NIGHT_VISION_DRAIN_INCREMENT_DEFAULT = 0.15f;
        internal const string NIGHT_VISION_DRAIN_INCREMENT_DESCRIPTION = "Applied to drain speed on each upgrade.";

        internal const string NIGHT_VISION_REGEN_INCREMENT_KEY = $"Increase for {NightVision.SIMPLE_UPGRADE_NAME} battery regen";
        internal const float NIGHT_VISION_REGEN_INCREMENT_DEFAULT = 0.40f;
        internal const string NIGHT_VISION_REGEN_INCREMENT_DESCRIPTION = "Applied to regen speed on each upgrade.";

        internal const string NIGHT_VISION_BATTERY_INCREMENT_KEY = $"Increase for {NightVision.SIMPLE_UPGRADE_NAME} battery life";
        internal const float NIGHT_VISION_BATTERY_INCREMENT_DEFAULT = 2f;
        internal const string NIGHT_VISION_BATTERY_INCREMENT_DESCRIPTION = $"Applied to the max charge for {NightVision.SIMPLE_UPGRADE_NAME} battery on each upgrade.";

        internal const string LOSE_NIGHT_VISION_ON_DEATH_KEY = $"Lose {NightVision.SIMPLE_UPGRADE_NAME} On Death";
        internal const bool LOSE_NIGHT_VISION_ON_DEATH_DEFAULT = true;
        internal const string LOSE_NIGHT_VISION_ON_DEATH_DESCRIPTION = $"If true when you die the {NightVision.SIMPLE_UPGRADE_NAME} will disable and will need a new pair of goggles.";

        internal const string NIGHT_VISION_DROP_ON_DEATH_KEY = $"Drop {NightVisionGoggles.ITEM_NAME} on Death";
        internal const bool NIGHT_VISION_DROP_ON_DEATH_DEFAULT = true;
        internal const string NIGHT_VISION_DROP_ON_DEATH_DESCRIPTION = $"If true, when you die and lose {NightVision.SIMPLE_UPGRADE_NAME} upon death, you will drop the {NightVisionGoggles.ITEM_NAME} on your body.";
        #endregion

        #region Mechanical Arms

        internal const string MECHANICAL_ARMS_ENABLED_KEY = $"Enable {MechanicalArms.UPGRADE_NAME} Upgrade";
        internal const bool MECHANICAL_ARMS_ENABLED_DEFAULT = true;
        internal const string MECHANICAL_ARMS_ENABLED_DESCRIPTION = "Tier upgrade which increases the range of interacting with objects (both grabbing and interacting such as open or close a door)";

        internal const string MECHANICAL_ARMS_PRICE_KEY = $"Price of {MechanicalArms.UPGRADE_NAME} Upgrade";
        internal const int MECHANICAL_ARMS_PRICE_DEFAULT = 300;

        internal const string MECHANICAL_ARMS_INITIAL_RANGE_INCREASE_KEY = "Initial range increase";
        internal const float MECHANICAL_ARMS_INITIAL_RANGE_INCREASE_DEFAULT = 1f;
        internal const string MECHANICAL_ARMS_INITIAL_RANGE_INCREASE_DESCRIPTION = "Amount of interaction range increased (in Unity units) when first purchasing the upgrade";

        internal const string MECHANICAL_ARMS_INCREMENTAL_RANGE_INCREASE_KEY = "Incremental range increase";
        internal const float MECHANICAL_ARMS_INCREMENTAL_RANGE_INCREASE_DEFAULT = 1f;
        internal const string MECHANICAL_ARMS_INCREMENTAL_RANGE_INCREASE_DESCRIPTION = "Amount of interaction range increased (in Unity units) on further purchases of the upgrade";

        #endregion

        #region Scavenger Instincts

        internal const string SCAVENGER_INSTINCTS_ENABLED_KEY = $"Enable {ScavengerInstincts.UPGRADE_NAME} Upgrade";
        internal const bool SCAVENGER_INSTINCTS_ENABLED_DEFAULT = true;
        internal const string SCAVENGER_INSTINCTS_ENABLED_DESCRIPTION = "Tier upgrade which increases the amount of scrap that can spawn on a given level.";

        internal const string SCAVENGER_INSTINCTS_PRICE_KEY = $"Price of {ScavengerInstincts.UPGRADE_NAME} Upgrade";
        internal const int SCAVENGER_INSTINCTS_PRICE_DEFAULT = 800;

        internal const string SCAVENGER_INSTINCTS_INITIAL_AMOUNT_SCRAP_INCREASE_KEY = "Initial amount of scrap spawn increase";
        internal const int SCAVENGER_INSTINCTS_INITIAL_AMOUNT_SCRAP_INCREASE_DEFAULT = 4;
        internal const string SCAVENGER_INSTINCTS_INITIAL_AMOUNT_SCRAP_INCREASE_DESCRIPTION = "The amount of additional scrap that can spawn in a given level when first purchased";

        internal const string SCAVENGER_INSTINCTS_INCREMENTAL_AMOUNT_SCRAP_INCREASE_KEY = "Incremental amount of scrap spawn increase";
        internal const int SCAVENGER_INSTINCTS_INCREMENTAL_AMOUNT_SCRAP_INCREASE_DEFAULT = 2;
        internal const string SCAVENGER_INSTINCTS_INCREMENTAL_AMOUNT_SCRAP_INCREASE_DESCRIPTION = "The amount of additional scrap that can spawn in a given level on further purchases";

        #endregion

        #region Landing Thrusters

        internal const string LANDING_THRUSTERS_ENABLED_KEY = $"Enable {LandingThrusters.UPGRADE_NAME} Upgrade";
        internal const bool LANDING_THRUSTERS_ENABLED_DEFAULT = true;
        internal const string LANDING_THRUSTERS_ENABLED_DESCRIPTION = "Tier upgrade which increases the speed of the ship landing and departing between moons";

        internal const string LANDING_THURSTERS_PRICE_KEY = $"Price of {LandingThrusters.UPGRADE_NAME} Upgrade";
        internal const int LANDING_THRUSTERS_PRICE_DEFAULT = 300;

        internal const string LANDING_THRUSTERS_INITIAL_SPEED_INCREASE_KEY = "Initial speed increase";
        internal const int LANDING_THRUSTERS_INITIAL_SPEED_INCREASE_DEFAULT = 25;
        internal const string LANDING_THRUSTERS_INITIAL_SPEED_INCREASE_DESCRIPTION = "Initial amount (%) to increment to the ship speed when first purchasing the upgrade";

        internal const string LANDING_THRUSTERS_INCREMENTAL_SPEED_INCREASE_KEY = "Incremental speed increase";
        internal const int LANDING_THRUSTERS_INCREMENTAL_SPEED_INCREASE_DEFAULT = 25;
        internal const string LANDING_THRUSTERS_INCREMENTAL_SPEED_INCREASE_DESCRIPTION = "Incremental amount (%) to increment to the ship speed on further purchases of the upgrade";

        internal const string LANDING_THRUSTERS_AFFECT_LANDING_KEY = "Affect landing sequence speed";
        internal const bool LANDING_THRUSTERS_AFFECT_LANDING_DEFAULT = true;
        internal const string LANDING_THRUSTERS_AFFECT_LANDING_DESCRIPTION = "If true, the effect will be applied on the ship speed when landing on a moon";

        internal const string LANDING_THRUSTERS_AFFECT_DEPARTING_KEY = "Affect departing sequence speed";
        internal const bool LANDING_THRUSTERS_AFFECT_DEPARTING_DEFAULT = true;
        internal const string LANDING_THRUSTERS_AFFECT_DEPARTING_DESCRIPTION = "If true, the effect will be applied on the ship speed when departing from a moon";

        #endregion

        #region Reinforced Boots

        internal const string REINFORCED_BOOTS_ENABLED_KEY = $"Enable {ReinforcedBoots.UPGRADE_NAME} Upgrade";
        internal const bool REINFORCED_BOOTS_ENABLED_DEFAULT = true;
        internal const string REINFORCED_BOOTS_ENABLED_DESCRIPTION = "Tier upgrade which reduces the damage taken when falling from great heights";

        internal const string REINFORCED_BOOTS_PRICE_KEY = $"Price of {ReinforcedBoots.UPGRADE_NAME} Upgrade";
        internal const int REINFORCED_BOOTS_PRICE_DEFAULT = 250;

        internal const string REINFORCED_BOOTS_INITIAL_DAMAGE_REDUCTION_KEY = "Initial damage reduction";
        internal const int REINFORCED_BOOTS_INITIAL_DAMAGE_REDUCTION_DEFAULT = 25;
        internal const string REINFORCED_BOOTS_INITIAL_DAMAGE_REDUCTION_DESCRIPTION = "Percentage of the damage taken from great height falls mitigated when purchased the upgrade for the first time";

        internal const string REINFORCED_BOOTS_INCREMENTAL_DAMAGE_REDUCTION_KEY = "Incremental damage reduction";
        internal const int REINFORCED_BOOTS_INCREMENTAL_DAMAGE_REDUCTION_DEFAULT = 10;
        internal const string REINFORCED_BOOTS_INCREMENTAL_DAMAGE_REDUCTION_DESCRIPTION = "Percentage of the damage taken from great height falls mitigated in further purchases of the upgrade";

        #endregion

        #region Deeper Pockets

        internal const string DEEPER_POCKETS_ENABLED_KEY = $"Enable {DeepPockets.UPGRADE_NAME} Upgrade";
        internal const bool DEEPER_POCKETS_ENABLED_DEFAULT = true;
        internal const string DEEPER_POCKETS_ENABLED_DESCRIPTION = "Tier upgrade which allows the player to carry more than one two handed item in their inventory";

        internal const string DEEPER_POCKETS_PRICE_KEY = $"Price of {DeepPockets.UPGRADE_NAME} Upgrade";
        internal const int DEEPER_POCKETS_PRICE_DEFAULT = 500;

        internal const string DEEPER_POCKETS_INITIAL_TWO_HANDED_AMOUNT_KEY = "Initial carry capacity increase";
        internal const int DEEPER_POCKETS_INITIAL_TWO_HANDED_AMOUNT_DEFAULT = 1;
        internal const string DEEPER_POCKETS_INITIAL_TWO_HANDED_AMOUNT_DESCRIPTION = "The amount of two handed carry capacity increased to the player when first buying the upgrade";

        internal const string DEEPER_POCKETS_INCREMENTAL_TWO_HANDED_AMOUNT_KEY = "Incremental carry capacity increase";
        internal const int DEEPER_POCKETS_INCREMENTAL_TWO_HANDED_AMOUNT_DEFAULT = 1;
        internal const string DEEPER_POCKETS_INCREMENTAL_TWO_HANDED_AMOUNT_DESCRIPTION = "The amount of two handed carry capacity increased to the player when increase the upgrade level";

        internal const string DEEPER_POCKETS_ALLOW_WHEELBARROWS_KEY = "Allow wheelbarrows to be stored in deeper pockets";
        internal const bool DEEPER_POCKETS_ALLOW_WHEELBARROWS_DEFAULT = true;
        internal const string DEEPER_POCKETS_ALLOW_WHEELBARROWS_DESCRIPTION = "Whether or not wheelbarrows and shopping carts can be stored in deeper pockets";

        #endregion 

        #region Aluminium Coils

        internal const string ALUMINIUM_COILS_ENABLED_KEY = $"Enable {AluminiumCoils.UPGRADE_NAME} Upgrade";
        internal const bool ALUMINIUM_COILS_ENABLED_DEFAULT = true;
        internal const string ALUMINIUM_COILS_ENABLED_DESCRIPTION = "Tier upgrade which reduces the zap gun minigame's difficulty, making it easier to manage.";

        internal const string ALUMINIUM_COILS_PRICE_KEY = $"Price of {AluminiumCoils.UPGRADE_NAME} upgrade";
        internal const int ALUMINIUM_COILS_PRICE_DEFAULT = 750;

        internal const string ALUMINIUM_COILS_INITIAL_DIFFICULTY_MULTIPLIER_KEY = "Initial multiplier applied to the minigame difficulty multiplier (%)";
        internal const int ALUMINIUM_COILS_INITIAL_DIFFICULTY_MULTIPLIER_DEFAULT = 25;
        internal const string ALUMINIUM_COILS_INITIAL_DIFFICULTY_MULTIPLIER_DESCRIPTION = "Past 100%, the zap gun minigame will have no difficulty in following it.";

        internal const string ALUMINIUM_COILS_INCREMENTAL_DIFFICULTY_MULTIPLIER_KEY = "Incremental multiplier applied to the minigame difficulty multiplier (%)";
        internal const int ALUMINIUM_COILS_INCREMENTAL_DIFFICULTY_MULTIPLIER_DEFAULT = 10;
        internal const string ALUMINIUM_COILS_INCREMENTAL_DIFFICULTY_MULTIPLIER_DESCRIPTION = ALUMINIUM_COILS_INITIAL_DIFFICULTY_MULTIPLIER_DESCRIPTION;

        internal const string ALUMINIUM_COILS_INITIAL_STUN_TIMER_KEY = "Initial value incremented to the enemy stun timer when using the zap gun";
        internal const float ALUMINIUM_COILS_INITIAL_STUN_TIMER_DEFAULT = 1f;

        internal const string ALUMINIUM_COILS_INCREMENTAL_STUN_TIMER_KEY = "Incremental value incremented to the enemy stun timer when using the zap gun";
        internal const float ALUMINIUM_COILS_INCREMENTAL_STUN_TIMER_DEFAULT = 1f;

        internal const string ALUMINIUM_COILS_INITIAL_RANGE_KEY = "Initial value incremented to the zap guns range to stun an enemy";
        internal const float ALUMINIUM_COILS_INITIAL_RANGE_DEFAULT = 2f;

        internal const string ALUMINIUM_COILS_INCREMENTAL_RANGE_KEY = "Incremental value incremented to the zap guns range to stun an enemy";
        internal const float ALUMINIUM_COILS_INCREMENTAL_RANGE_DEFAULT = 1f;

        internal const string ALUMINIUM_COILS_INITIAL_COOLDOWN_KEY = "Initial multiplier applied to the zap guns cooldown (%)";
        internal const int ALUMINIUM_COILS_INITIAL_COOLDOWN_DEFAULT = 10;
        internal const string ALUMINIUM_COILS_INITIAL_COOLDOWN_DESCRIPTION = "Past 100%, the zap gun will have no cooldown after being used";

        internal const string ALUMINIUM_COILS_INCREMENTAL_COOLDOWN_KEY = "Incremental multiplier applied to the zap guns cooldown";
        internal const int ALUMINIUM_COILS_INCREMENTAL_COOLDOWN_DEFAULT = 10;
        internal const string ALUMINIUM_COILS_INCREMENTAL_COOLDOWN_DESCRIPTION = ALUMINIUM_COILS_INITIAL_COOLDOWN_DESCRIPTION;

        #endregion

        #region Back Muscles

        internal const string BACK_MUSCLES_ENABLED_KEY = $"Enable {BackMuscles.UPGRADE_NAME} Upgrade";
        internal const bool BACK_MUSCLES_ENABLED_DEFAULT = true;
        internal const string BACK_MUSCLES_ENABLED_DESCRIPTION = "Reduce carry weight";

        internal const string BACK_MUSCLES_PRICE_KEY = $"Price of {BackMuscles.UPGRADE_NAME} Upgrade";
        internal const int BACK_MUSCLES_PRICE_DEFAULT = 715;

        internal const string BACK_MUSCLES_INITIAL_WEIGHT_MULTIPLIER_KEY = "Carry Weight Multiplier";
        internal const float BACK_MUSCLES_INITIAL_WEIGHT_MULTIPLIER_DEFAULT = 0.5f;
        internal const string BACK_MUSCLES_INITIAL_WEIGHT_MULTIPLIER_DESCRIPTION = "Your carry weight is multiplied by this.";

        internal const string BACK_MUSCLES_INCREMENTAL_WEIGHT_MULTIPLIER_KEY = "Carry Weight Increment";
        internal const float BACK_MUSCLES_INCREMENTAL_WEIGHT_MULTIPLIER_DEFAULT = 0.1f;
        internal const string BACK_MUSCLES_INCREMENTAL_WEIGHT_MULTIPLIER_DESCRIPTION = "Each upgrade subtracts this from the above coefficient.";

        internal const string BACK_MUSCLES_UPGRADE_MODE_KEY = $"Upgrade Mode for {BackMuscles.UPGRADE_NAME}";
        internal const BackMuscles.UpgradeMode BACK_MUSCLES_UPGRADE_MODE_DEFAULT = BackMuscles.UpgradeMode.ReduceWeight;
        internal const string BACK_MUSCLES_UPGRADE_MODE_DESCRIPTION = "Applied mode when purchasing the upgrade:\n" +
            "ReduceWeight (Reduces the overall weight of items when grabbed), " +
            "ReduceCarryInfluence (Reduces the carry weight influence when sprinting), " +
            "ReduceStrain (Reduces the carry weight influence on stamina consumption when running),";

        #endregion

        #region Bargain Connections

        internal const string BARGAIN_CONNECTIONS_ENABLED_KEY = $"Enable {BargainConnections.UPGRADE_NAME} Upgrade";
        internal const bool BARGAIN_CONNECTIONS_ENABLED_DEFAULT = true;
        internal const string BARGAIN_CONNECTIONS_ENABLED_DESCRIPTION = "Tier upgrade which increases the amount of items that can be on sale in the store";

        internal const string BARGAIN_CONNECTIONS_PRICE_KEY = $"Price of {BargainConnections.UPGRADE_NAME}";
        internal const int BARGAIN_CONNECTIONS_PRICE_DEFAULT = 200;

        internal const string BARGAIN_CONNECTIONS_INITIAL_AMOUNT_KEY = "Initial additional amount of items that can go on sale";
        internal const int BARGAIN_CONNECTIONS_INITIAL_AMOUNT_DEFAULT = 3;

        internal const string BARGAIN_CONNECTIONS_INCREMENTAL_AMOUNT_KEY = "Incremental additional amount of items that can go on sale";
        internal const int BARGAIN_CONNECTIONS_INCREMENTAL_AMOUNT_DEFAULT = 2;

        #endregion

        #region Beekeeper

        internal const string BEEKEEPER_ENABLED_KEY = $"Enable {Beekeeper.UPGRADE_NAME} Upgrade";
        internal const bool BEEKEEPER_ENABLED_DEFAULT = true;
        internal const string BEEKEEPER_ENABLED_DESCRIPTION = "Take less damage from bees and hornets";

        internal const string BEEKEEPER_PRICE_KEY = $"Price of {Beekeeper.UPGRADE_NAME} Upgrade";
        internal const int BEEKEEPER_PRICE_DEFAULT = 450;

        internal const string BEEKEEPER_DAMAGE_MULTIPLIER_KEY = "Multiplied to incoming damage (rounded to int)";
        internal const float BEEKEEPER_DAMAGE_MULTIPLIER_DEFAULT = 0.64f;
        internal const string BEEKEEPER_DAMAGE_MULTIPLIER_DESCRIPTION = "Incoming damage from bees (from hives) is 10 and butler's deal 40 (20 if solo).";

        internal const string BEEKEEPER_DAMAGE_INCREMENTAL_MULTIPLIER_KEY = "Additional % Reduced per level";
        internal const float BEEKEEPER_DAMAGE_INCREMENTAL_MULTIPLIER_DEFAULT = 0.15f;
        internal const string BEEKEEPER_DAMAGE_INCREMENTAL_MULTIPLIER_DESCRIPTION = $"Every time {Beekeeper.UPGRADE_NAME} is upgraded this value will be subtracted to the base multiplier above.";

        internal const string BEEKEEPER_HIVE_MULTIPLIER_KEY = "Hive value increase multiplier";
        internal const float BEEKEEPER_HIVE_MULTIPLIER_DEFAULT = 1.5f;
        internal const string BEEKEEPER_HIVE_MULTIPLIER_DESCRIPTION = "Multiplier applied to the value of beehive when reached max level";

        #endregion

        #region Better Scanner

        internal const string BETTER_SCANNER_ENABLED_KEY = $"Enable {BetterScanner.UPGRADE_NAME} Upgrade";
        internal const bool BETTER_SCANNER_ENABLED_DEFAULT = true;
        internal const string BETTER_SCANNER_ENABLED_DESCRIPTION = "Further scan distance, no LOS needed.";

        internal const string BETTER_SCANNER_PRICE_KEY = $"Price of {BetterScanner.UPGRADE_NAME} Upgrade";
        internal const int BETTER_SCANNER_PRICE_DEFAULT = 650;

        internal const string BETTER_SCANNER_OUTSIDE_NODE_DISTANCE_INCREASE_KEY = "Ship and Entrance node distance boost";
        internal const float BETTER_SCANNER_OUTSIDE_NODE_DISTANCE_INCREASE_DEFAULT = 150f;
        internal const string BETTER_SCANNER_OUTSIDE_NODE_DISTANCE_INCREASE_DESCRIPTION = "How much further away you can scan the ship and entrance.";

        internal const string BETTER_SCANNER_NODE_DISTANCE_INCREASE_KEY = "Node distance boost";
        internal const float BETTER_SCANNER_NODE_DISTANCE_INCREASE_DEFAULT = 20f;
        internal const string BETTER_SCANNER_NODE_DISTANCE_INCREASE_DESCRIPTION = "How much further away you can scan other nodes.";

        internal const string BETTER_SCANNER_SECOND_TIER_PRICE_KEY = $"Price of second {BetterScanner.UPGRADE_NAME} tier";
        internal const int BETTER_SCANNER_SECOND_TIER_PRICE_DEFAULT = 500;
        internal const string BETTER_SCANNER_SECOND_TIER_PRICE_DESCRIPTION = "This tier unlocks ship scan commands.";

        internal const string BETTER_SCANNER_THIRD_TIER_PRICE_KEY = $"Price of third {BetterScanner.UPGRADE_NAME} tier";
        internal const int BETTER_SCANNER_THIRD_TIER_PRICE_DEFAULT = 800;
        internal const string BETTER_SCANNER_THIRD_TIER_PRICE_DESCRIPTION = "This tier unlocks scanning through walls.";

        internal const string BETTER_SCANNER_ENEMIES_THROUGH_WALLS_KEY = "Scan enemies through walls on final upgrade";
        internal const bool BETTER_SCANNER_ENEMIES_THROUGH_WALLS_DEFAULT = false;
        internal const string BETTER_SCANNER_ENEMIES_THROUGH_WALLS_DESCRIPTION = "If true the final upgrade will scan scrap AND enemies through walls.";

        internal const string BETTER_SCANNER_VERBOSE_ENEMIES_KEY = "Verbose `scan enemies` command";
        internal const bool BETTER_SCANNER_VERBOSE_ENEMIES_DEFAULT = true;
        internal const string BETTER_SCANNER_VERBOSE_ENEMIES_DESCRIPTION = "If false `scan enemies` only returns a count of outside and inside enemies, else it returns the count for each enemy type.";

        #endregion

        #region Bigger Lungs

        internal const string BIGGER_LUNGS_ENABLED_KEY = $"Enable {BiggerLungs.UPGRADE_NAME} Upgrade";
        internal const bool BIGGER_LUNGS_ENABLED_DEFAULT = true;
        internal const string BIGGER_LUNGS_ENABLED_DESCRIPTION = "More Stamina";

        internal const string BIGGER_LUNGS_PRICE_KEY = $"Price of {BiggerLungs.UPGRADE_NAME} Upgrade";
        internal const int BIGGER_LUNGS_PRICE_DEFAULT = 600;

        internal const string BIGGER_LUNGS_INITIAL_SPRINT_TIME_KEY = "Sprint Time Unlock";
        internal const float BIGGER_LUNGS_INITIAL_SPRINT_TIME_DEFAULT = 6f;
        internal const string BIGGER_LUNGS_INITIAL_SPRINT_TIME_DESCRIPTION = "Amount of sprint time gained when unlocking the upgrade.\nDefault vanilla value is 11f.";

        internal const string BIGGER_LUNGS_INCREMENTAL_SPRINT_TIME_KEY = "Sprint Time Increment";
        internal const float BIGGER_LUNGS_INCREMENTAL_SPRINT_TIME_DEFAULT = 1.25f;
        internal const string BIGGER_LUNGS_INCREMENTAL_SPRINT_TIME_DESCRIPTION = "Amount of sprint time gained when increasing the level of upgrade.";

        internal const string BIGGER_LUNGS_STAMINA_APPLY_LEVEL_KEY = $"Level of {BiggerLungs.UPGRADE_NAME} to apply stamina regeneration";
        internal const int BIGGER_LUNGS_STAMINA_APPLY_LEVEL_DEFAULT = 1;
        internal const string BIGGER_LUNGS_STAMINA_APPLY_LEVEL_DESCRIPTION = "When reached provided level, the effect will start applying.";

        internal const string BIGGER_LUNGS_INITIAL_STAMINA_REGEN_KEY = "Stamina Regeneration Increase";
        internal const float BIGGER_LUNGS_INITIAL_STAMINA_REGEN_DEFAULT = 1.05f;
        internal const string BIGGER_LUNGS_INITIAL_STAMINA_REGEN_DESCRIPTION = "Affects the rate of the player regaining stamina";

        internal const string BIGGER_LUNGS_INCREMENTAL_STAMINA_REGEN_KEY = "Incremental stamina regeneration increase";
        internal const float BIGGER_LUNGS_INCREMENTAL_STAMINA_REGEN_DEFAULT = 0.05f;
        internal const string BIGGER_LUNGS_INCREMENTAL_STAMINA_REGEN_DESCRIPTION = "Added to initial multiplier";

        internal const string BIGGER_LUNGS_JUMP_STAMINA_DECREASE_APPLY_LEVEL_KEY = $"Level of {BiggerLungs.UPGRADE_NAME} to apply stamina cost decrease on jumps";
        internal const int BIGGER_LUNGS_JUMP_STAMINA_DECREASE_APPLY_LEVEL_DEFAULT = 2;
        internal const string BIGGER_LUNGS_JUMP_STAMINA_DECREASE_APPLY_LEVEL_DESCRIPTION = "When reached provided level, the effect will start applying.";

        internal const string BIGGER_LUNGS_INITIAL_JUMP_STAMINA_DECREASE_KEY = "Stamina cost decrease on jumps";
        internal const float BIGGER_LUNGS_INITIAL_JUMP_STAMINA_DECREASE_DEFAULT = 0.90f;
        internal const string BIGGER_LUNGS_INITIAL_JUMP_STAMINA_DECREASE_DESCRIPTION = "Multiplied with the vanilla cost of jumping";

        internal const string BIGGER_LUNGS_INCREMENTAL_JUMP_STAMINA_DECREASE_KEY = "Incremental stamina cost decrease on jumps";
        internal const float BIGGER_LUNGS_INCREMENTAL_JUMP_STAMINA_DECREASE_DEFAULT = 0.05f;
        internal const string BIGGER_LUNGS_INCREMENTAL_JUMP_STAMINA_DECREASE_DESCRIPTION = "Added to initial multiplier (as in reduce the factor)";

        #endregion

        #region Charging Booster

        internal const string CHARGING_BOOSTER_ENABLED_KEY = $"Enable {ChargingBooster.UPGRADE_NAME} Upgrade";
        internal const bool CHARGING_BOOSTER_ENABLED_DEFAULT = true;
        internal const string CHARGING_BOOSTER_ENABLED_DESCRIPTION = "Tier upgrade which allows charging items in a radar booster";

        internal const string CHARGING_BOOSTER_PRICE_KEY = $"Price of {ChargingBooster.UPGRADE_NAME} Upgrade";
        internal const int CHARGING_BOOSTER_PRICE_DEFAULT = 300;

        internal const string CHARGING_BOOSTER_PRICES_DEFAULT = "250,300,400";

        internal const string CHARGING_BOOSTER_COOLDOWN_KEY = "Cooldown of the charging station in radar boosters when used";
        internal const float CHARGING_BOOSTER_COOLDOWN_DEFAULT = 90f;

        internal const string CHARGING_BOOSTER_INCREMENTAL_COOLDOWN_DECREASE_KEY = "Incremental cooldown decrease whenever the level of the upgrade is incremented";
        internal const float CHARGING_BOOSTER_INCREMENTAL_COOLDOWN_DECREASE_DEFAULT = 10f;

        internal const string CHARGING_BOOSTER_CHARGE_PERCENTAGE_KEY = "Percentage of charge that is added to the held item when using the radar booster charge station";
        internal const int CHARGING_BOOSTER_CHARGE_PERCENTAGE_DEFAULT = 50;

        #endregion

        #region Climbing Gloves

        internal const string CLIMBING_GLOVES_ENABLED_KEY = $"Enable {ClimbingGloves.UPGRADE_NAME} Upgrade";
        internal const bool CLIMBING_GLOVES_ENABLED_DEFAULT = true;
        internal const string CLIMBING_GLOVES_ENABLED_DESCRIPTION = "Tier upgrade which increases the speed of climbing ladders";

        internal const string CLIMBING_GLOVES_PRICE_KEY = $"Price of {ClimbingGloves.UPGRADE_NAME} Upgrade";
        internal const int CLIMBING_GLOVES_PRICE_DEFAULT = 150;

        internal const string CLIMBING_GLOVES_INITIAL_MULTIPLIER_KEY = "Initial value added to the players climbing speed";
        internal const float CLIMBING_GLOVES_INITIAL_MULTIPLIER_DEFAULT = 1f;
        internal const string CLIMBING_GLOVES_INITIAL_MULTIPLIER_DESCRIPTION = "Vanilla climb speed is 4 units";

        internal const string CLIMBING_GLOVES_INCREMENTAL_MULTIPLIER_KEY = "Incremental value added to the players climbing speed";
        internal const float CLIMBING_GLOVES_INCREMENTAL_MULTIPLIER_DEFAULT = 0.5f;

        #endregion

        #region Discombobulator

        internal const string DISCOMBOBULATOR_ENABLED_KEY = $"Enable {Discombobulator.UPGRADE_NAME} Upgrade";
        internal const bool DISCOMBOBULATOR_ENABLED_DEFAULT = true;
        internal const string DISCOMBOBULATOR_ENABLED_DESCRIPTION = "Stun enemies around the ship.";

        internal const string DISCOMBOBULATOR_PRICE_KEY = $"Price of {Discombobulator.UPGRADE_NAME} Upgrade";
        internal const int DISCOMBOBULATOR_PRICE_DEFAULT = 450;

        internal const string DISCOMBOBULATOR_COOLDOWN_KEY = $"{Discombobulator.UPGRADE_NAME} Cooldown";
        internal const float DISCOMBOBULATOR_COOLDOWN_DEFAULT = 120f;

        internal const string DISCOMBOBULATOR_EFFECT_RADIUS_KEY = $"{Discombobulator.UPGRADE_NAME} Effect Radius";
        internal const float DISCOMBOBULATOR_EFFECT_RADIUS_DEFAULT = 40f;

        internal const string DISCOMBOBULATOR_STUN_DURATION_KEY = $"{Discombobulator.UPGRADE_NAME} Stun Duration";
        internal const float DISCOMBOBULATOR_STUN_DURATION_DEFAULT = 7.5f;

        internal const string DISCOMBOBULATOR_NOTIFY_CHAT_KEY = "Notify Local Chat of Enemy Stun Duration";
        internal const bool DISCOMBOBULATOR_NOTIFY_CHAT_DEFAULT = true;

        internal const string DISCOMBOBULATOR_INCREMENTAL_STUN_TIME_KEY = $"{Discombobulator.UPGRADE_NAME} Increment";
        internal const float DISCOMBOBULATOR_INCREMENTAL_STUN_TIME_DEFAULT = 1f;
        internal const string DISCOMBOBULATOR_INCREMENTAL_STUN_TIME_DESCRIPTION = "The amount added to stun duration on upgrade.";

        internal const string DISCOMBOBULATOR_APPLY_DAMAGE_LEVEL_KEY = "Initial level of dealing damage";
        internal const int DISCOMBOBULATOR_APPLY_DAMAGE_LEVEL_DEFAULT = 2;
        internal const string DISCOMBOBULATOR_APPLY_DAMAGE_LEVEL_DESCRIPTION = $"Level of {Discombobulator.UPGRADE_NAME} in which it starts dealing damage on enemies it stuns";

        internal const string DISCOMBOBULATOR_INITIAL_DAMAGE_KEY = "Initial amount of damage";
        internal const int DISCOMBOBULATOR_INITIAL_DAMAGE_DEFAULT = 2;
        internal const string DISCOMBOBULATOR_INITIAL_DAMAGE_DESCRIPTION = "Amount of damage when reaching the first level that unlocks damage on stun\nTo give an idea of what's too much, Eyeless Dogs have 12 health.";

        internal const string DISCOMBOBULATOR_INCREMENTAL_DAMAGE_KEY = "Damage Increase on Purchase";
        internal const int DISCOMBOBULATOR_INCREMENTAL_DAMAGE_DEFAULT = 1;
        internal const string DISCOMBOBULATOR_INCREMENTAL_DAMAGE_DESCRIPTION = "Damage increase when purchasing later levels";

        #endregion

        #region Drop Pod Thrusters

        internal const string DROP_POD_THRUSTERS_ENABLED_KEY = $"Enable the {FasterDropPod.UPGRADE_NAME} upgrade";
        internal const bool DROP_POD_THRUSTERS_ENABLED_DEFAULT = true;
        internal const string DROP_POD_THRUSTERS_ENABLED_DESCRIPTION = "Make the Drop Pod, the ship that deliver items bought on the terminal, land faster.";

        internal const string DROP_POD_THRUSTERS_PRICE_KEY = $"{FasterDropPod.UPGRADE_NAME} Price";
        internal const int DROP_POD_THRUSTERS_PRICE_DEFAULT = 300;
        internal const string DROP_POD_THRUSTERS_PRICE_DESCRIPTION = $"Default price for upgrade. If set to 0 it will enable the {FasterDropPod.UPGRADE_NAME} without buying the upgrade.";

        internal const string DROP_POD_THRUSTERS_TIME_DECREASE_KEY = "Time decrement on the timer used for subsequent item deliveries";
        internal const float DROP_POD_THRUSTERS_TIME_DECREASE_DEFAULT = 20f;

        internal const string DROP_POD_THRUSTERS_FIRST_TIME_DECREASE_KEY = "Time decrement on the timer used for the first ever item delivery";
        internal const float DROP_POD_THRUSTERS_FIRST_TIME_DECREASE_DEFAULT = 10f;

        internal const string DROP_POD_THRUSTERS_LEAVE_TIMER_KEY = "Time for item dropship to exit";
        internal const float DROP_POD_THRUSTERS_LEAVE_TIMER_DEFAULT = 0f;
        internal const string DROP_POD_THRUSTERS_LEAVE_TIMER_DESCRIPTION = "How long (in seconds) the item drop ship stays after being opened";

        #endregion

        #region Efficient Engines

        internal const string EFFICIENT_ENGINES_ENABLED_KEY = $"Enable {EfficientEngines.UPGRADE_NAME} Upgrade";
        internal const bool EFFICIENT_ENGINES_ENABLED_DEFAULT = true;
        internal const string EFFICIENT_ENGINES_ENABLED_DESCRIPTION = "Tier upgrade which applies a discount on moon routing prices for cheaper travels";

        internal const string EFFICIENT_ENGINES_PRICE_KEY = $"Price of {EfficientEngines.UPGRADE_NAME}";
        internal const int EFFICIENT_ENGINES_PRICE_DEFAULT = 450;

        internal const string EFFICIENT_ENGINES_INITIAL_MULTIPLIER_KEY = "Initial discount applied to moon routing (%)";
        internal const int EFFICIENT_ENGINES_INITIAL_MULTIPLIER_DEFAULT = 15;

        internal const string EFFICIENT_ENGINES_INCREMENTAL_MULTIPLIER_KEY = "Incremental discount applied to moon routing (%)";
        internal const int EFFICIENT_ENGINES_INCREMENTAL_MULTIPLIER_DEFAULT = 5;

        #endregion

        #region Fast Encryption

        internal const string FAST_ENCRYPTION_ENABLED_KEY = $"Enable {FastEncryption.UPGRADE_NAME}";
        internal const bool FAST_ENCRYPTION_ENABLED_DEFAULT = true;
        internal const string FAST_ENCRYPTION_ENABLED_DESCRIPTION = "Upgrades the transmitter.";

        internal const string FAST_ENCRYPTION_PRICE_KEY = $"{FastEncryption.UPGRADE_NAME} Price";
        internal const int FAST_ENCRYPTION_PRICE_DEFAULT = 300;

        #endregion

        #region Lethal Deals

        internal const string LETHAL_DEALS_ENABLED_KEY = $"Enable {LethalDeals.UPGRADE_NAME} Upgrade";
        internal const bool LETHAL_DEALS_ENABLED_DEFAULT = true;
        internal const string LETHAL_DEALS_ENABLED_DESCRIPTION = "One time upgrade which guarantees at least one item will be on sale in the store.";

        internal const string LETHAL_DEALS_PRICE_KEY = $"Price of {LethalDeals.UPGRADE_NAME}";
        internal const int LETHAL_DEALS_PRICE_DEFAULT = 300;

        #endregion

        #region Lithium Batteries

        internal const string LITHIUM_BATTERIES_ENABLED_KEY = $"Enable {LithiumBatteries.UPGRADE_NAME} Upgrade";
        internal const bool LITHIUM_BATTERIES_ENABLED_DEFAULT = true;
        internal const string LITHIUM_BATTERIES_ENABLED_DESCRIPTION = "Tier upgrade which decreases the rate of battery consumed when using the item";

        internal const string LITHIUM_BATTERIES_PRICE_KEY = $"Price of {LithiumBatteries.UPGRADE_NAME} upgrade";
        internal const int LITHIUM_BATTERIES_PRICE_DEFAULT = 100;

        internal const string LITHIUM_BATTERIES_INITIAL_MULTIPLIER_KEY = "Initial multiplier applied to the use rate of the battery (%)";
        internal const int LITHIUM_BATTERIES_INITIAL_MULTIPLIER_DEFAULT = 10;
        internal const string LITHIUM_BATTERIES_INITIAL_MULTIPLIER_DESCRIPTION = "Past 100%, it won't consume any battery charge from the item";

        internal const string LITHIUM_BATTERIES_INCREMENTAL_MULTIPLIER_KEY = "Incremental multiplier applied to the use rate of the battery (%)";
        internal const int LITHIUM_BATTERIES_INCREMENTAL_MULTIPLIER_DEFAULT = 10;
        internal const string LITHIUM_BATTERIES_INCREMENTAL_MULTIPLIER_DESCRIPTION = LITHIUM_BATTERIES_INITIAL_MULTIPLIER_DESCRIPTION;

        #endregion

        #region Locksmith

        internal const string LOCKSMITH_ENABLED_KEY = $"Enable {LockSmith.UPGRADE_NAME} upgrade";
        internal const bool LOCKSMITH_ENABLED_DEFAULT = true;
        internal const string LOCKSMITH_ENABLED_DESCRIPTION = "Allows you to pick locked doors by completing a minigame.";

        internal const string LOCKSMITH_PRICE_KEY = $"{LockSmith.UPGRADE_NAME} Price";
        internal const int LOCKSMITH_PRICE_DEFAULT = 640;
        internal const string LOCKSMITH_PRICE_DESCRIPTION = $"Default price of {LockSmith.UPGRADE_NAME} upgrade.";
        #endregion

        #region Malware Broadcaster

        internal const string MALWARE_BROADCASTER_ENABLED_KEY = $"Enable {MalwareBroadcaster.UPGRADE_NAME} Upgrade";
        internal const bool MALWARE_BROADCASTER_ENABLED_DEFAULT = true;
        internal const string MALWARE_BROADCASTER_ENABLED_DESCRIPTION = "Explode Map Hazards";

        internal const string MALWARE_BROADCASTER_PRICE_KEY = $"Price of {MalwareBroadcaster.UPGRADE_NAME} Upgrade";
        internal const int MALWARE_BROADCASTER_PRICE_DEFAULT = 550;

        internal const string MALWARE_BROADCASTER_DESTROY_TRAPS_KEY = "Destroy Trap";
        internal const bool MALWARE_BROADCASTER_DESTROY_TRAPS_DEFAULT = true;
        internal const string MALWARE_BROADCASTER_DESTROY_TRAPS_DESCRIPTION = $"If false {MalwareBroadcaster.UPGRADE_NAME} will disable the trap for a long time instead of destroying.";

        internal const string MALWARE_BROADCASTER_DISARM_TIME_KEY = "Disarm Time";
        internal const float MALWARE_BROADCASTER_DISARM_TIME_DEFAULT = 7f;
        internal const string MALWARE_BROADCASTER_DISARM_TIME_DESCRIPTION = "If `Destroy Trap` is false this is the duration traps will be disabled.";

        internal const string MALWARE_BROADCASTER_EXPLODE_TRAPS_KEY = "Explode Trap";
        internal const bool MALWARE_BROADCASTER_EXPLODE_TRAPS_DEFAULT = true;
        internal const string MALWARE_BROADCASTER_EXPLODE_TRAPS_DESCRIPTION = "Destroy Trap must be true! If this is true when destroying a trap it will also explode.";

        #endregion

        #region Market Influence

        internal const string MARKET_INFLUENCE_ENABLED_KEY = $"Enable {MarketInfluence.UPGRADE_NAME} Upgrade";
        internal const bool MARKET_INFLUENCE_ENABLED_DEFAULT = true;
        internal const string MARKET_INFLUENCE_ENABLED_DESCRIPTION = "Tier upgrade which guarantees a minimum percentage sale on the selected item in the store.";

        internal const string MARKET_INFLUENCE_PRICE_KEY = $"Price of {MarketInfluence.UPGRADE_NAME}";
        internal const int MARKET_INFLUENCE_PRICE_DEFAULT = 250;

        internal const string MARKET_INFLUENCE_INITIAL_PERCENTAGE_KEY = "Initial percentage guarantee on the items on sale";
        internal const int MARKET_INFLUENCE_INITIAL_PERCENTAGE_DEFAULT = 10;

        internal const string MARKET_INFLUENCE_INCREMENTAL_PERCENTAGE_KEY = "Incremental percentage guarantee on the items on sale";
        internal const int MARKET_INFLUENCE_INCREMENTAL_PERCENTAGE_DEFAULT = 5;

        #endregion

        #region Running Shoes

        internal const string RUNNING_SHOES_ENABLED_KEY = $"Enable {RunningShoes.UPGRADE_NAME} Upgrade";
        internal const bool RUNNING_SHOES_ENABLED_DEFAULT = true;
        internal const string RUNNING_SHOES_ENABLED_DESCRIPTION = "Run Faster";

        internal const string RUNNING_SHOES_PRICE_KEY = $"Price of {RunningShoes.UPGRADE_NAME} Upgrade";
        internal const int RUNNING_SHOES_PRICE_DEFAULT = 650;

        internal const string RUNNING_SHOES_INITIAL_MOVEMENT_BOOST_KEY = "Movement Speed Unlock";
        internal const float RUNNING_SHOES_INITIAL_MOVEMENT_BOOST_DEFAULT = 1.4f;
        internal const string RUNNING_SHOES_INITIAL_MOVEMENT_BOOST_DESCRIPTION = "Value added to player's movement speed when first purchased.\nDefault vanilla value is 4.6f.";

        internal const string RUNNING_SHOES_INCREMENTAL_MOVEMENT_BOOST_KEY = "Movement Speed Increment";
        internal const float RUNNING_SHOES_INCREMENTAL_MOVEMENT_BOOST_DEFAULT = 0.5f;
        internal const string RUNNING_SHOES_INCREMENTAL_MOVEMENT_BOOST_DESCRIPTION = "How much the above value is increased on upgrade.";

        internal const string RUNNING_SHOES_NOISE_REDUCTION_KEY = "Noise Reduction";
        internal const float RUNNING_SHOES_NOISE_REDUCTION_DEFAULT = 10f;
        internal const string RUNNING_SHOES_NOISE_REDUCTION_DESCRIPTION = "Distance units to subtract from footstep noise when reached final level.";

        #endregion

        #region Quantum Disruptor

        internal const string QUANTUM_DISRUPTOR_ENABLED_KEY = $"Enable {QuantumDisruptor.UPGRADE_NAME} Upgrade";
        internal const bool QUANTUM_DISRUPTOR_ENABLED_DEFAULT = true;
        internal const string QUANTUM_DISRUPTOR_ENABLED_DESCRIPTION = "Tier upgrade which increases the time you can stay in a moon landing";

        internal const string QUANTUM_DISRUPTOR_PRICE_KEY = $"Price of {QuantumDisruptor.UPGRADE_NAME} Upgrade";
        internal const int QUANTUM_DISRUPTOR_PRICE_DEFAULT = 1000;

        internal const string QUANTUM_DISRUPTOR_INITIAL_MULTIPLIER_KEY = $"How slower time will go by when unlocking the {QuantumDisruptor.UPGRADE_NAME} upgrade";
        internal const float QUANTUM_DISRUPTOR_INITIAL_MULTIPLIER_DEFAULT = 0.2f;

        internal const string QUANTUM_DISRUPTOR_INCREMENTAL_MULTIPLIER_KEY = $"How slower time will go by when incrementing the {QuantumDisruptor.UPGRADE_NAME} level";
        internal const float QUANTUM_DISRUPTOR_INCREMENTAL_MULTIPLIER_DEFAULT = 0.1f;

        internal const string QUANTUM_DISRUPTOR_UPGRADE_MODE_KEY = $"Mode of {QuantumDisruptor.UPGRADE_NAME}";
        internal const QuantumDisruptor.UpgradeModes QUANTUM_DISRUPTOR_UPGRADE_MODE_DEFAULT = QuantumDisruptor.UpgradeModes.SlowdownTime;
        internal const string QUANTUM_DISRUPTOR_UPGRADE_MODE_DESCRIPTION = "Applied mode when purchasing the upgrade.\n" +
            "Supported modes: SlowdownTime (makes the time go slower in moons), RevertTime (command prompt which reverts hours when executed, maximum usages per moon).";

        internal const string QUANTUM_DISRUPTOR_RESET_MODE_KEY = $"Reset Time of {QuantumDisruptor.UPGRADE_NAME} current usages";
        internal const QuantumDisruptor.ResetModes QUANTUM_DISRUPTOR_RESET_MODE_DEFAULT = QuantumDisruptor.ResetModes.MoonLanding;
        internal const string QUANTUM_DISRUPTOR_RESET_MODE_DESCRIPTION = "Reset mode of when the current counter of usages reset to use the ability. Only applied when RevertTime mode is selected.\n" +
            "Supported Values: MoonLanding (reset counter per each moon landing), MoonRerouting (reset counter per each moon routing), NewQuota (reset counter per each new quota)";

        internal const string QUANTUM_DISRUPTOR_USES_DESCRIPTION = "Amount of times you can execute the quantum command. Only applied when RevertTime mode is selected";

        internal const string QUANTUM_DISRUPTOR_INITIAL_USES_KEY = $"Initial amount of usages of {QuantumDisruptor.UPGRADE_NAME}";
        internal const int QUANTUM_DISRUPTOR_INITIAL_USES_DEFAULT = 1;
        internal const string QUANTUM_DISRUPTOR_INITIAL_USES_DESCRIPTION = QUANTUM_DISRUPTOR_USES_DESCRIPTION;

        internal const string QUANTUM_DISRUPTOR_INCREMENTAL_USES_KEY = $"Incremental amount of usages of {QuantumDisruptor.UPGRADE_NAME}";
        internal const int QUANTUM_DISRUPTOR_INCREMENTAL_USES_DEFAULT = 1;
        internal const string QUANTUM_DISRUPTOR_INCREMENTAL_USES_DESCRIPTION = QUANTUM_DISRUPTOR_USES_DESCRIPTION;

        internal const string QUANTUM_DISRUPTOR_HOURS_REVERT_DESCRIPTION = "Amount of hours to revert when executing the quantum command. Only applied when RevertTime mode is selected";

        internal const string QUANTUM_DISRUPTOR_INITIAL_HOURS_ON_REVERT_KEY = $"Initial amount of hours reverted from {QuantumDisruptor.UPGRADE_NAME}";
        internal const int QUANTUM_DISRUPTOR_INITIAL_HOURS_ON_REVERT_DEFAULT = 1;
        internal const string QUANTUM_DISRUPTOR_INITIAL_HOURS_ON_REVERT_DESCRIPTION = QUANTUM_DISRUPTOR_HOURS_REVERT_DESCRIPTION;

        internal const string QUANTUM_DISRUPTOR_INCREMENTAL_HOURS_ON_REVERT_KEY = $"Incremental amount of hours reverted from {QuantumDisruptor.UPGRADE_NAME}";
        internal const int QUANTUM_DISRUPTOR_INCREMENTAL_HOURS_ON_REVERT_DEFAULT = 1;
        internal const string QUANTUM_DISRUPTOR_INCREMENTAL_HOURS_ON_REVERT_DESCRIPTION = QUANTUM_DISRUPTOR_HOURS_REVERT_DESCRIPTION;

        #endregion

        #region Sick Beats

        internal const string SICK_BEATS_ENABLED_KEY = $"Enable {SickBeats.UPGRADE_NAME} Upgrade";
        internal const bool SICK_BEATS_ENABLED_DEFAULT = true;
        internal const string SICK_BEATS_ENABLED_DESCRIPTION = "Get buffs from nearby active boomboxes.";

        internal const string SICK_BEATS_PRICE_KEY = "Default unlock price";
        internal const int SICK_BEATS_PRICE_DEFAULT = 500;

        internal const string SICK_BEATS_SPEED_KEY = "Enable Speed Boost Effect";
        internal const bool SICK_BEATS_SPEED_DEFAULT = true;

        internal const string SICK_BEATS_DAMAGE_KEY = "Enable Damage Boost Effect";
        internal const bool SICK_BEATS_DAMAGE_DEFAULT = true;

        internal const string SICK_BEATS_STAMINA_KEY = "Enable Stamina Boost Effect";
        internal const bool SICK_BEATS_STAMINA_DEFAULT = false;

        internal const string SICK_BEATS_DEFENSE_KEY = "Enable Defense Boost Effect";
        internal const bool SICK_BEATS_DEFENSE_DEFAULT = false;

        internal const string SICK_BEATS_DEFENSE_MULTIPLIER_KEY = "Defense Boost Coefficient";
        internal const float SICK_BEATS_DEFENSE_MULTIPLIER_DEFAULT = 0.5f;
        internal const string SICK_BEATS_DEFENSE_MULTIPLIER_DESCRIPTION = "Multiplied to incoming damage.";

        internal const string SICK_BEATS_STAMINA_MULTIPLIER_KEY = "Stamina Regen Coefficient";
        internal const float SICK_BEATS_STAMINA_MULTIPLIER_DEFAULT = 1.25f;
        internal const string SICK_BEATS_STAMINA_MULTIPLIER_DESCRIPTION = "Multiplied to stamina regen.";

        internal const string SICK_BEATS_APPLY_STAMINA_CONSUMPTION_KEY = "Apply Stamina Buff on stamina consumption situations";
        internal const bool SICK_BEATS_APPLY_STAMINA_CONSUMPTION_DEFAULT = false;
        internal const string SICK_BEATS_APPLY_STAMINA_CONSUMPTION_DESCRIPTION = "If enabled, moments where stamina is consumed overtime will include the stamina buff in mind";

        internal const string SICK_BEATS_ADDITIONAL_DAMAGE_KEY = "Additional Damage Dealt";
        internal const int SICK_BEATS_ADDITIONAL_DAMAGE_DEFAULT = 1;

        internal const string SICK_BEATS_ADDITIONAL_SPEED_KEY = "Speed Boost Addition";
        internal const float SICK_BEATS_ADDITIONAL_SPEED_DEFAULT = 1.5f;

        internal const string SICK_BEATS_EFFECT_RADIUS_KEY = "Effect Radius";
        internal const float SICK_BEATS_EFFECT_RADIUS_DEFAULT = 15f;
        internal const string SICK_BEATS_EFFECT_RADIUS_DESCRIPTION = "Radius in unity units players will be effected by an active boombox.";

        #endregion

        #region Sigurd Access

        internal const string SIGURD_ACCESS_ENABLED_KEY = $"Enable the {Sigurd.UPGRADE_NAME} upgrade";
        internal const bool SIGURD_ACCESS_ENABLED_DEFAULT = true;
        internal const string SIGURD_ACCESS_ENABLED_DESCRIPTION = "There's a chance that The Company will pay more for the scrap.";

        internal const string SIGURD_ACCESS_ENABLED_LAST_DAY_KEY = $"Enable the {Sigurd.UPGRADE_NAME} upgrade for the last day";
        internal const bool SIGURD_ACCESS_ENABLED_LAST_DAY_DEFAULT = true;
        internal const string SIGURD_ACCESS_ENABLED_LAST_DAY_DESCRIPTION = "There's a chance that the last day scrap you go over 100% value.";

        internal const string SIGURD_ACCESS_PRICE_KEY = $"{Sigurd.UPGRADE_NAME} Price";
        internal const int SIGURD_ACCESS_PRICE_DEFAULT = 500;
        internal const string SIGURD_ACCESS_PRICE_DESCRIPTION = $"Default price for upgrade. If set to 0 it will enable the {Sigurd.UPGRADE_NAME} without buying the upgrade.";

        internal const string SIGURD_ACCESS_CHANCE_KEY = "Chance for the upgrade to activate";
        internal const float SIGURD_ACCESS_CHANCE_DEFAULT = 20f;

        internal const string SIGURD_ACCESS_CHANCE_LAST_DAY_KEY = "Chance for the upgrade to activate on the last day";
        internal const float SIGURD_ACCESS_CHANCE_LAST_DAY_DEFAULT = 20f;

        internal const string SIGURD_ACCESS_ADDITIONAL_PERCENT_KEY = "How much the percentage will go up";
        internal const float SIGURD_ACCESS_ADDITIONAL_PERCENT_DEFAULT = 20f;

        internal const string SIGURD_ACCESS_ADDITIONAL_PERCENT_LAST_DAY_KEY = "How much the percentage will go up on the last day";
        internal const float SIGURD_ACCESS_ADDITIONAL_PERCENT_LAST_DAY_DEFAULT = 20f;

        #endregion

        #region Strong Legs

        internal const string STRONG_LEGS_ENABLED_KEY = $"Enable {StrongLegs.UPGRADE_NAME} Upgrade";
        internal const bool STRONG_LEGS_ENABLED_DEFAULT = true;
        internal const string STRONG_LEGS_ENABLED_DESCRIPTION = "Jump Higher";

        internal const string STRONG_LEGS_PRICE_KEY = $"Price of {StrongLegs.UPGRADE_NAME} Upgrade";
        internal const int STRONG_LEGS_PRICE_DEFAULT = 300;

        internal const string STRONG_LEGS_INITIAL_JUMP_FORCE_KEY = "Jump Force Unlock";
        internal const float STRONG_LEGS_INITIAL_JUMP_FORCE_DEFAULT = 3f;
        internal const string STRONG_LEGS_INITIAL_JUMP_FORCE_DESCRIPTION = "Amount of jump force added when unlocking the upgrade.\nDefault vanilla value is 13f.";

        internal const string STRONG_LEGS_INCREMENTAL_JUMP_FORCE_KEY = "Jump Force Increment";
        internal const float STRONG_LEGS_INCREMENTAL_JUMP_FORCE_DEFAULT = 0.75f;
        internal const string STRONG_LEGS_INCREMENTAL_JUMP_FORCE_DESCRIPTION = "How much the above value is increased on upgrade.";

        #endregion

        #region Walkie GPS

        internal const string WALKIE_GPS_ENABLED_KEY = $"Enable the {WalkieGPS.UPGRADE_NAME} upgrade";
        internal const bool WALKIE_GPS_ENABLED_DEFAULT = true;
        internal const string WALKIE_GPS_ENABLED_DESCRIPTION = "Holding a walkie talkie displays location.";

        internal const string WALKIE_GPS_PRICE_KEY = $"{WalkieGPS.UPGRADE_NAME} Price";
        internal const int WALKIE_GPS_PRICE_DEFAULT = 450;
        internal const string WALKIE_GPS_PRICE_DESCRIPTION = $"Default price for {WalkieGPS.UPGRADE_NAME} upgrade.";

        #endregion

        #endregion

        #endregion

        #region LGU Store Interactive UI

        internal const string COLOR_INITIAL_FORMAT = "<color={0}>";
        internal const string COLOR_FINAL_FORMAT = "</color>";
        internal const string DEFAULT_DEACTIVATED_TEXT_COLOUR = HEXADECIMAL_GREY + "55";

        #region Main Screen

        internal const string MAIN_SCREEN_TITLE = "Lategame Upgrades";
        internal const string MAIN_SCREEN_TOP_TEXT = "Select an upgrade to purchase:";
        internal const string MAIN_SCREEN_TOP_TEXT_NO_ENTRIES = "There are no upgrades for purchase.";

        internal const string MAIN_SCREEN_INDIVIDUAL_UPGRADES_TEXT = "Individual Upgrades";
        internal const string MAIN_SCREEN_SHARED_UPGRADES_TEXT = "Shared Upgrades";

        #endregion

        #region Upgrade Display

        internal const string NOT_ENOUGH_CREDITS = "You do not have enough credits to purchase this upgrade.";
        internal const string REACHED_MAX_LEVEL = "You have reached the maximum level of this upgrade.";
        internal const string PURCHASE_UPGRADE_FORMAT = "Do you wish to purchase this upgrade for {0} credits?";

        #endregion

        #region Cursor Display
        internal const char CURSOR = '>';

        internal const string SELECTED_CURSOR_ELEMENT_FORMAT = "<mark={0}><color={1}>{2}</color></mark>";
        internal const string DEFAULT_BACKGROUND_SELECTED_COLOR = HEXADECIMAL_GREEN + "33";
        internal const string DEFAULT_TEXT_SELECTED_COLOR = HEXADECIMAL_WHITE + "FF";

        internal const string GO_BACK_PROMPT = "Back";

        internal const string CONFIRM_PROMPT = "Confirm";
        internal const string CANCEL_PROMPT = "Abort";
        #endregion

        #region Screen Display
        internal const int AVAILABLE_CHARACTERS_PER_LINE = 51;
        internal const char TOP_LEFT_CORNER = '╭';
        internal const char TOP_RIGHT_CORNER = '╮';
        internal const char BOTTOM_LEFT_CORNER = '╰';
        internal const char BOTTOM_RIGHT_CORNER = '╯';
        internal const char VERTICAL_LINE = '│';
        internal const char HORIZONTAL_LINE = '─';

        internal const char CONNECTING_TITLE_LEFT = '╢';
        internal const char CONNECTING_TITLE_RIGHT = '╟';
        internal const char TOP_RIGHT_TITLE_CORNER = '╗';
        internal const char TOP_LEFT_TITLE_CORNER = '╔';
        internal const char BOTTOM_RIGHT_TITLE_CORNER = '╝';
        internal const char BOTTOM_LEFT_TITLE_CORNER = '╚';
        internal const char VERTICAL_TITLE_LINE = '║';
        internal const char HORIZONTAL_TITLE_LINE = '═';

        #region Page Display
        internal const int START_PAGE_COUNTER = 30;
        #endregion

        #region Upgrades Display
        internal const int NAME_LENGTH = 17;
        internal const int LEVEL_LENGTH = 7;

        internal const char EMPTY_LEVEL = '○';
        internal const char FILLED_LEVEL = '●';
        #endregion

        #endregion

        #region Colours
        internal static readonly Color Invisible = new(0, 0, 0, 0);
        #endregion

        #endregion

        #region Terminal Nodes

        internal const string DISCOMBOBULATOR_NOT_ACTIVE = "You don't have access to this command yet. Purchase the 'Discombobulator'.\n\n";
        internal const string DISCOMBOBULATOR_ON_COOLDOWN_FORMAT = "You can discombobulate again in {0} seconds.\nType 'cooldown' or 'cd' to check discombobulation cooldown.\n\n";
        internal const string DISCOMBOBULATOR_NO_ENEMIES = "No stunned enemies detected.\n\n";
        internal const string DISCOMBULATOR_HIT_ENEMIES = "Discombobulator hit {0} enemies.\n\n";
        internal const string DISCOMBOBULATOR_DISPLAY_COOLDOWN = "You can discombobulate again in {0} seconds.\n\n";
        internal const string DISCOMBOBULATOR_READY = "Discombobulate is ready, Type 'initattack' or 'atk' to execute.\n\n";

        internal const string LGU_SAVE_WIPED = "LGU save has been wiped.\n\n";
        internal const string LGU_SAVE_NOT_FOUND = "LGU save was not found!\n\n";

        internal const string FORCE_CREDITS_SUCCESS_FORMAT = "All clients should now have ${0}\n\n";
        internal const string FORCE_CREDITS_HOST_ONLY = "Only the host can do this";
        internal const string FORCE_CREDITS_PARSED_FAIL_FORMAT = "Failed to parse value {0}.\n\n";

        internal const string INTERNS_NOT_ENOUGH_CREDITS_FORMAT = "Interns cost {0} credits and you have {1} credits.\n";
        internal const string INTERNS_PLAYER_ALREADY_ALIVE_FORMAT = "{0} is still alive, they can't be replaced with an intern.\n\n";

        internal const string LOAD_LGU_NO_NAME = "Enter the name of the user whos upgrades/save you want to copy. Ex: `load lgu steve`\n";
        internal const string LOAD_LGU_SUCCESS_FORMAT = "Attempting to overwrite local save data with {0}'s save data\nYou should see a popup in 5 seconds...\n.\n";
        internal const string LOAD_LGU_FAILURE_FORMAT = "The name {0} was not found. The following names were found:\n{1}\n";

        internal const string UNLOAD_LGU_SUCCESS_FORMAT = "Unwinding {0}\n\n";

        internal const string SCANNER_LEVEL_REQUIRED = "Upgrade Better Scanner to level 2 to use this command\nEnter `info better scanner` to check upgrades.\n\n";

        internal const string CONTRACT_CANCEL_FAIL = "You must have accepted a contract to execute this command...\n\n";
        internal const string CONTRACT_CANCEL_CONFIRM_PROMPT = "Type CONFIRM to cancel your current contract. There will be no refunds.\n\n";
        internal const string CONTRACT_FAIL = "Not possible to provide a contract due to configuration having it disabled.\n\n";

        internal const string BRUTEFORCE_USAGE = "Enter a valid address for a device to connect to!\n\n";

        internal const string CONTRACT_CANCEL_CONFIRM_PROMPT_FAIL = "Failed to confirm user's input. Invalidated cancel contract request.\n\n";
        internal const string CONTRACT_CANCEL_CONFIRM_PROMPT_SUCCESS = "Cancelling contract...\n\n";

        internal const string CONTRACT_SPECIFY_CONFIRM_PROMPT_FAIL = "Failed to confirm user's input. Invalidated specified moon contract request.\n\n";
        internal const string CONTRACT_SPECIFY_CONFIRM_PROMPT_SUCCESS_FORMAT = "A {0} contract has been accepted for {1}!{2}";

        internal const string SCRAP_INSURANCE_ALREADY_PURCHASED = "You already purchased insurance to protect your scrap belongings.\n\n";
        internal const string SCRAP_INSURANCE_ONLY_IN_ORBIT = "You can only acquire insurance while in orbit.\n\n";
        internal const string SCRAP_INSURANCE_NOT_ENOUGH_CREDITS_FORMAT = "Not enough credits to purchase Scrap Insurance.\nPrice: {0}\nCurrent credits: {1}\n\n";
        internal const string SCRAP_INSURANCE_SUCCESS = "Scrap Insurance has been activated.\nIn case of a team wipe in your next trip, your scrap will be preserved.\n\n";

        internal const string LOOKUP_NOT_IN_CONTRACT = "YOU MUST BE IN A DEFUSAL CONTRACT TO USE THIS COMMAND!\n\n";
        internal const string LOOKUP_USAGE = "YOU MUST ENTER A SERIAL NUMBER TO LOOK UP!\n\n";
        internal const string LOOKUP_CUT_WIRES_FORMAT = "CUT THE WIRES IN THE FOLLOWING ORDER:\n\n{0}\n\n";
        #endregion

        #region Chat Notifications

        internal const string UPGRADE_LOADED_NOTIFICATION_DEFAULT_COLOR = HEXADECIMAL_RED;
        internal const string UPGRADE_UNLOADED_NOTIFICATION_DEFAULT_COLOR = HEXADECIMAL_RED;

        #endregion

        #region Contracts

        internal const string DEFUSAL_CONTRACT_NAME = "Defusal";
        internal const string EXTRACTION_CONTRACT_NAME = "Extraction";
        internal const string DATA_CONTRACT_NAME = "Data";
        internal const string EXTERMINATOR_CONTRACT_NAME = "Exterminator";
        internal const string EXORCISM_CONTRACT_NAME = "Exorcism";

        #endregion
    }
}
