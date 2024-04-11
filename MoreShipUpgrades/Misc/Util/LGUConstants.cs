using LethalLib.Modules;
using MoreShipUpgrades.Managers;
using MoreShipUpgrades.UpgradeComponents.Commands;
using MoreShipUpgrades.UpgradeComponents.TierUpgrades;
using System;
using System.Diagnostics;
using System.Numerics;
using System.Xml.Linq;
using UnityEngine;

namespace MoreShipUpgrades.Misc.Util
{
    internal static class LGUConstants
    {
        #region General

        #region Characters
        internal const char WHITE_SPACE = ' ';
        #endregion

        #region Hex Colors

        internal const string HEXADECIMAL_RED = "#FF0000";
        internal const string HEXADECIMAL_WHITE = "#FFFFFF";
        internal const string HEXADECIMAL_GREEN = "#00FF00";

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

        #region Contracts
        internal const string CONTRACTS_SECTION = "Contracts";

        internal const string ENABLE_CONTRACTS_KEY = "Enable the ability to purchase contracts / missions";
        internal const bool ENABLE_CONTRACTS_DEFAULT = true;

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

        #region Charging Booster

        internal const string CHARGING_BOOSTER_ENABLED_KEY = "Enable Charging Booster Upgrade";
        internal const bool CHARGING_BOOSTER_ENABLED_DEFAULT = true;
        internal const string CHARGING_BOOSTER_ENABLED_DESCRIPTION = "Tier upgrade which allows charging items in a radar booster";

        internal const string CHARGING_BOOSTER_PRICE_KEY = "Price of Charging Booster Upgrade";
        internal const int CHARGING_BOOSTER_PRICE_DEFAULT = 300;

        internal const string CHARGING_BOOSTER_PRICES_DEFAULT = "250,300,400";

        internal const string CHARGING_BOOSTER_COOLDOWN_KEY = "Cooldown of the charging station in radar boosters when used";
        internal const float CHARGING_BOOSTER_COOLDOWN_DEFAULT = 90f;

        internal const string CHARGING_BOOSTER_INCREMENTAL_COOLDOWN_DECREASE_KEY = "Incremental cooldown decrease whenever the level of the upgrade is incremented";
        internal const float CHARGING_BOOSTER_INCREMENTAL_COOLDOWN_DECREASE_DEFAULT = 10f;

        internal const string CHARGING_BOOSTER_CHARGE_PERCENTAGE_KEY = "Percentage of charge that is added to the held item when using the radar booster charge station";
        internal const int CHARGING_BOOSTER_CHARGE_PERCENTAGE_DEFAULT = 50;

        #endregion

        #endregion

        #region LGU Store Interactive UI

        #region Main Screen

        internal const string MAIN_SCREEN_TITLE = "Lategame Upgrades";
        internal const string MAIN_SCREEN_TOP_TEXT = "Select an upgrade to purchase:";

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
        internal static readonly Color Invisible = new Color(0, 0, 0, 0);
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

        #endregion

        #region Chat Notifications

        internal const string UPGRADE_LOADED_NOTIFICATION_DEFAULT_COLOR = HEXADECIMAL_RED;
        internal const string UPGRADE_UNLOADED_NOTIFICATION_DEFAULT_COLOR = HEXADECIMAL_RED;

        #endregion
    }
}
