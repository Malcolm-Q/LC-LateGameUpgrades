using LethalCompanyInputUtils.Api;
using UnityEngine.InputSystem;

namespace MoreShipUpgrades.Input
{
    internal class IngameKeybinds : LcInputActions
    {
        public static IngameKeybinds Instance = new();
        internal static InputActionAsset GetAsset()
        {
            return Instance.Asset;
        }

        [InputAction("<Mouse>/middleButton", Name = "Drop all items from wheelbarrow")]
        public InputAction WheelbarrowKey { get; set; }

        [InputAction("<Keyboard>/leftAlt", Name = "Toggle NVG")]
        public InputAction NvgKey { get; set; }

        [InputAction("<Keyboard>/w", Name = "Upgrade store cursor up")]
        public InputAction UpgradeStoreCursorUpKey { get; set; }

        [InputAction("<Keyboard>/s", Name = "Upgrade store cursor down")]
        public InputAction UpgradeStoreCursorDownKey { get; set; }
        [InputAction("<Keyboard>/escape", Name = "Upgrade store cursor down")]
        public InputAction UpgradeStoreCursorExitKey { get; set; }

        [InputAction("<Keyboard>/e", Name = "Upgrade store page up")]
        public InputAction UpgradeStorePageUpKey { get; set; }

        [InputAction("<Keyboard>/q", Name = "Upgrade store page down")]
        public InputAction UpgradeStorePageDownKey { get; set; }
        [InputAction("<Keyboard>/enter", Name = "Enter upgrade store prompt")]
        public InputAction LguStoreConfirmKey { get; set; }

    }
}
