using MoreShipUpgrades.Input;
using UnityEngine.InputSystem;

namespace MoreShipUpgrades.Compat
{
    /// <summary>
    /// Class responsible for compatibility with LethalCompany_InputUtils mod
    /// </summary>
    public static class InputUtilsCompat
    {
        /// <summary>
        /// Asset used to store all the input bindings defined for our controls
        /// </summary>
        internal static InputActionAsset Asset => IngameKeybinds.GetAsset();
        /// <summary>
        /// Input binding used to trigger the toggle of Night Vision action
        /// </summary>
        public static InputAction NvgKey => IngameKeybinds.Instance.NvgKey;

        /// <summary>
        /// Initialization of the compatibility class
        /// </summary>
        internal static void Init()
        {
            IngameKeybinds.Instance = new();
        }
    }
}
