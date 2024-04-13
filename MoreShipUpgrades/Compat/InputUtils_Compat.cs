using MoreShipUpgrades.Input;
using UnityEngine.InputSystem;

namespace MoreShipUpgrades.Compat
{
    /// <summary>
    /// Class responsible for compatibility with LethalCompany_InputUtils mod
    /// </summary>
    public static class InputUtils_Compat
    {
        /// <summary>
        /// Asset used to store all the input bindings defined for our controls
        /// </summary>
        internal static InputActionAsset Asset => IngameKeybinds.GetAsset();
        /// <summary>
        /// Input binding used to trigger the drop all items in the wheelbarrow action
        /// </summary>
        public static InputAction WheelbarrowKey => IngameKeybinds.Instance.WheelbarrowKey;
        /// <summary>
        /// Input binding used to trigger the toggle of Night Vision action
        /// </summary>
        public static InputAction NvgKey => IngameKeybinds.Instance.NvgKey;
        /// <summary>
        /// Input binding used to trigger the move cursor up in the LGU interface action
        /// </summary>
        public static InputAction CursorUpKey => IngameKeybinds.Instance.UpgradeStoreCursorUpKey;
        /// <summary>
        /// Input binding used to trigger the move cursor down in the LGU interface action
        /// </summary>
        public static InputAction CursorDownKey => IngameKeybinds.Instance.UpgradeStoreCursorDownKey;
        /// <summary>
        /// Input binding used to trigger the exit LGU interface action
        /// </summary>
        public static InputAction CursorExitKey => IngameKeybinds.Instance.UpgradeStoreCursorExitKey;
        /// <summary>
        /// Input binding used to change to next page in the LGU interface action
        /// </summary>
        public static InputAction PageUpKey => IngameKeybinds.Instance.UpgradeStorePageUpKey;
        /// <summary>
        /// Input binding used to change to previous page in the LGU interface action
        /// </summary>
        public static InputAction PageDownKey => IngameKeybinds.Instance.UpgradeStorePageDownKey;
        /// <summary>
        /// Input binding used to submit current prompt in the LGU interface action
        /// </summary>
        public static InputAction LguStoreConfirmKey => IngameKeybinds.Instance.LguStoreConfirmKey;

        /// <summary>
        /// Initialization of the compatibility class
        /// </summary>
        internal static void Init()
        {
            IngameKeybinds.Instance = new();
        }
    }
}
