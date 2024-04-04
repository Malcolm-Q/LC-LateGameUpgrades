using LethalLib.Modules;
using MoreShipUpgrades.Managers;
using MoreShipUpgrades.UpgradeComponents.Commands;
using MoreShipUpgrades.UpgradeComponents.TierUpgrades;
using System;
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

        #region LGU Store Interactive UI

        #region Cursor Display
        internal const char CURSOR = '>';

        internal const string SELECTED_CURSOR_ELEMENT_FORMAT = "<mark={0}><color={1}>{2}</color></mark>";
        internal const string DEFAULT_BACKGROUND_SELECTED_COLOR = HEXADECIMAL_GREEN + "33";
        internal const string DEFAULT_TEXT_SELECTED_COLOR = HEXADECIMAL_WHITE + "FF";
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

        #endregion

        #region Chat Notifications

        internal const string UPGRADE_LOADED_NOTIFICATION_DEFAULT_COLOR = HEXADECIMAL_RED;
        internal const string UPGRADE_UNLOADED_NOTIFICATION_DEFAULT_COLOR = HEXADECIMAL_RED;

        #endregion
    }
}
