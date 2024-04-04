using LethalCompanyInputUtils.Api;
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

        [InputAction("<Mouse>/middleButton", Name = "Drop all items from wheelbarrow")]
        public InputAction WheelbarrowKey { get; set; }

        [InputAction("<Keyboard>/leftAlt", Name = "Toggle NVG")]
        public InputAction NvgKey { get; set; }

        [InputAction("<Keyboard>/w", Name = "Move Cursor Up in Lategame Upgrades Store")]
        public InputAction UpgradeStoreCursorUpKey { get; set; }

        [InputAction("<Keyboard>/s", Name = "Move Cursor Down in Lategame Upgrades Store")]
        public InputAction UpgradeStoreCursorDownKey { get; set; }
        [InputAction("<Keyboard>/escape", Name = "Exit Lategame Upgrades Store")]
        public InputAction UpgradeStoreCursorExitKey { get; set; }

        [InputAction("<Keyboard>/d", Name = "Next Page in Lategame Upgrades Store")]
        public InputAction UpgradeStorePageUpKey { get; set; }

        [InputAction("<Keyboard>/a", Name = "Previous Page in Lategame Upgrades Store")]
        public InputAction UpgradeStorePageDownKey { get; set; }
        [InputAction("<Keyboard>/enter", Name = "Submit Prompt in Lategame Upgrades Store")]
        public InputAction LguStoreConfirmKey { get; set; }

    }
}
