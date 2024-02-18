using MoreShipUpgrades.Input;
using MoreShipUpgrades.UpgradeComponents.Items.Wheelbarrow;
using MoreShipUpgrades.UpgradeComponents.TierUpgrades;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine.InputSystem;

namespace MoreShipUpgrades.Compat
{
    public static class InputUtils_Compat
    {
        internal static bool Enabled => BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey("com.rune580.LethalCompanyInputUtils");
        internal static InputActionAsset Asset => IngameKeybinds.GetAsset();
        public static InputAction WheelbarrowKey => IngameKeybinds.Instance.WheelbarrowKey;
        public static InputAction NvgKey => IngameKeybinds.Instance.NvgKey;

        internal static void Init()
        {
            if(Enabled && IngameKeybinds.Instance == null)
            {
                IngameKeybinds.Instance = new();
            }
        }
    }
}
