using LethalCompanyInputUtils.Api;
using MoreShipUpgrades.Misc.Util;
using UnityEngine.InputSystem;

namespace MoreShipUpgrades.Input
{
    /// <summary>
    /// Class which stores the used key binds in the mod
    /// </summary>
    internal class IngameKeybinds : LcInputActions
    {
        public static IngameKeybinds Instance;
        /// <summary>
        /// Asset used to store all the input bindings defined for our controls
        /// </summary>
        internal static InputActionAsset GetAsset()
        {
            return Instance.Asset;
        }

        /// <summary>
        /// Input binding used to trigger the drop all items in the wheelbarrow action
        /// </summary>
        [InputAction(LGUConstants.DROP_ALL_ITEMS_WHEELBARROW_DEFAULT_KEYBIND, Name = LGUConstants.DROP_ALL_ITEMS_WHEELBARROW_KEYBIND_NAME)]
        public InputAction WheelbarrowKey { get; set; }

        /// <summary>
        /// Input binding used to trigger the toggle of Night Vision action
        /// </summary>
        [InputAction(LGUConstants.TOGGLE_NIGHT_VISION_DEFAULT_KEYBIND, Name = LGUConstants.TOGGLE_NIGHT_VISION_KEYBIND_NAME)]
        public InputAction NvgKey { get; set; }

        /// <summary>
        /// Input binding used to trigger the move cursor up in the LGU interface action
        /// </summary>
        [InputAction(LGUConstants.MOVE_CURSOR_UP_DEFAULT_KEYBIND, Name = LGUConstants.MOVE_CURSOR_UP_KEYBIND_NAME)]
        public InputAction UpgradeStoreCursorUpKey { get; set; }

        /// <summary>
        /// Input binding used to trigger the move cursor down in the LGU interface action
        /// </summary>
        [InputAction(LGUConstants.MOVE_CURSOR_DOWN_DEFAULT_KEYBIND, Name = LGUConstants.MOVE_CURSOR_DOWN_KEYBIND_NAME)]
        public InputAction UpgradeStoreCursorDownKey { get; set; }

        /// <summary>
        /// Input binding used to trigger the exit LGU interface action
        /// </summary>
        [InputAction(LGUConstants.EXIT_STORE_DEFAULT_KEYBIND, Name = LGUConstants.EXIT_STORE_KEYBIND_NAME)]
        public InputAction UpgradeStoreCursorExitKey { get; set; }

        /// <summary>
        /// Input binding used to change to next page in the LGU interface action
        /// </summary>
        [InputAction(LGUConstants.NEXT_PAGE_DEFAULT_KEYBIND, Name = LGUConstants.NEXT_PAGE_KEYBIND_NAME)]
        public InputAction UpgradeStorePageUpKey { get; set; }

        /// <summary>
        /// Input binding used to change to previous page in the LGU interface action
        /// </summary>
        [InputAction(LGUConstants.PREVIOUS_PAGE_DEFAULT_KEYBIND, Name = LGUConstants.PREVIOUS_PAGE_KEYBIND_NAME)]
        public InputAction UpgradeStorePageDownKey { get; set; }

        /// <summary>
        /// Input binding used to submit current prompt in the LGU interface action
        /// </summary>
        [InputAction(LGUConstants.SUBMIT_PROMPT_DEFAULT_KEYBIND, Name = LGUConstants.SUBMIT_PROMPT_KEYBIND_NAME)]
        public InputAction LguStoreConfirmKey { get; set; }

    }
}
