using LethalCompanyInputUtils.Api;
using MoreShipUpgrades.Misc.Util;
using UnityEngine.InputSystem;

namespace MoreShipUpgrades.Input
{
    internal class IngameKeybinds : LcInputActions
    {
        public static IngameKeybinds Instance;
        internal static InputActionAsset GetAsset()
        {
            return Instance.Asset;
        }

        [InputAction(LGUConstants.DROP_ALL_ITEMS_WHEELBARROW_DEFAULT_KEYBIND, Name = LGUConstants.DROP_ALL_ITEMS_WHEELBARROW_KEYBIND_NAME)]
        public InputAction WheelbarrowKey { get; set; }

        [InputAction(LGUConstants.TOGGLE_NIGHT_VISION_DEFAULT_KEYBIND, Name = LGUConstants.TOGGLE_NIGHT_VISION_KEYBIND_NAME)]
        public InputAction NvgKey { get; set; }

        [InputAction(LGUConstants.MOVE_CURSOR_UP_DEFAULT_KEYBIND, Name = LGUConstants.MOVE_CURSOR_UP_KEYBIND_NAME)]
        public InputAction UpgradeStoreCursorUpKey { get; set; }

        [InputAction(LGUConstants.MOVE_CURSOR_DOWN_DEFAULT_KEYBIND, Name = LGUConstants.MOVE_CURSOR_DOWN_KEYBIND_NAME)]
        public InputAction UpgradeStoreCursorDownKey { get; set; }
        [InputAction(LGUConstants.EXIT_STORE_DEFAULT_KEYBIND, Name = LGUConstants.EXIT_STORE_KEYBIND_NAME)]
        public InputAction UpgradeStoreCursorExitKey { get; set; }

        [InputAction(LGUConstants.NEXT_PAGE_DEFAULT_KEYBIND, Name = LGUConstants.NEXT_PAGE_KEYBIND_NAME)]
        public InputAction UpgradeStorePageUpKey { get; set; }

        [InputAction(LGUConstants.PREVIOUS_PAGE_DEFAULT_KEYBIND, Name = LGUConstants.PREVIOUS_PAGE_KEYBIND_NAME)]
        public InputAction UpgradeStorePageDownKey { get; set; }
        [InputAction(LGUConstants.SUBMIT_PROMPT_DEFAULT_KEYBIND, Name = LGUConstants.SUBMIT_PROMPT_KEYBIND_NAME)]
        public InputAction LguStoreConfirmKey { get; set; }

    }
}
