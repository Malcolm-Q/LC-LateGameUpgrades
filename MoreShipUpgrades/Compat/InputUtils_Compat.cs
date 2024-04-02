using MoreShipUpgrades.Input;
using UnityEngine.InputSystem;

namespace MoreShipUpgrades.Compat
{
    public static class InputUtils_Compat
    {
        internal static InputActionAsset Asset => IngameKeybinds.GetAsset();
        public static InputAction WheelbarrowKey => IngameKeybinds.Instance.WheelbarrowKey;
        public static InputAction NvgKey => IngameKeybinds.Instance.NvgKey;
        public static InputAction CursorUpKey => IngameKeybinds.Instance.UpgradeStoreCursorUpKey;
        public static InputAction CursorDownKey => IngameKeybinds.Instance.UpgradeStoreCursorDownKey;
        public static InputAction CursorExitKey => IngameKeybinds.Instance.UpgradeStoreCursorExitKey;
        public static InputAction PageUpKey => IngameKeybinds.Instance.UpgradeStorePageUpKey;
        public static InputAction PageDownKey => IngameKeybinds.Instance.UpgradeStorePageDownKey;
        public static InputAction LguStoreConfirmKey => IngameKeybinds.Instance.LguStoreConfirmKey;

        internal static void Init()
        {
            IngameKeybinds.Instance = new();
        }
    }
}
