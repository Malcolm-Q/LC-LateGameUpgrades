using LethalCompanyInputUtils.Api;
using System;
using System.Collections.Generic;
using System.Text;
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
    }
}
