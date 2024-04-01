using CSync.Lib;
using GameNetcodeStuff;
using HarmonyLib;
using MoreShipUpgrades.Compat;
using MoreShipUpgrades.Misc;
using MoreShipUpgrades.UpgradeComponents.Items.Wheelbarrow;
using MoreShipUpgrades.UpgradeComponents.TierUpgrades;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

namespace MoreShipUpgrades.Input
{
    [HarmonyPatch]
    internal static class Keybinds
    {
        public static InputActionAsset Asset;

        public static InputActionMap ActionMap;

        public static InputAction WheelbarrowAction;

        public static InputAction NvgAction;
        public static InputAction cursorUpAction;
        public static InputAction cursorDownAction;
        public static InputAction cursorExitAction;
        public static InputAction pageUpAction;
        public static InputAction pageDownAction;
        public static InputAction storeConfirmAction;

        public static PlayerControllerB localPlayerController => StartOfRound.Instance?.localPlayerController;

        [HarmonyPatch(typeof(PreInitSceneScript), "Awake")]
        [HarmonyPrefix]
        public static void AddToKeybindMenu()
        {
            if (InputUtils_Compat.Enabled)
            {
                Asset = InputUtils_Compat.Asset;
                ActionMap = Asset.actionMaps[0];
                WheelbarrowAction = InputUtils_Compat.WheelbarrowKey;
                NvgAction = InputUtils_Compat.NvgKey;
                cursorUpAction = InputUtils_Compat.CursorUpKey;
                cursorDownAction = InputUtils_Compat.CursorDownKey;
                cursorExitAction = InputUtils_Compat.CursorExitKey;
                pageUpAction = InputUtils_Compat.PageUpKey;
                pageDownAction = InputUtils_Compat.PageDownKey;
                storeConfirmAction = InputUtils_Compat.LguStoreConfirmKey;
            }
            else
            {
                Asset = ScriptableObject.CreateInstance<InputActionAsset>();
                ActionMap = new InputActionMap("MoreShipUpgrades");
                InputActionSetupExtensions.AddActionMap(Asset, ActionMap);
                WheelbarrowAction = InputActionSetupExtensions.AddAction(ActionMap, "Drop all items from wheelbarrow", binding: SyncedInstance<PluginConfig>.Default.WHEELBARROW_DROP_ALL_CONTROL_BIND.Value, interactions: "Press");
                NvgAction = InputActionSetupExtensions.AddAction(ActionMap, "Toggle NVG", binding: SyncedInstance<PluginConfig>.Default.TOGGLE_NIGHT_VISION_KEY.Value, interactions: "Press");
                cursorUpAction = InputActionSetupExtensions.AddAction(ActionMap, "Move Cursor Up in Lategame Upgrades Store", binding: "<Keyboard>/w", interactions: "Press");
                cursorDownAction = InputActionSetupExtensions.AddAction(ActionMap, "Move Cursor Down in Lategame Upgrades Store", binding: "<Keyboard>/s", interactions: "Press");
                cursorExitAction = InputActionSetupExtensions.AddAction(ActionMap, "Exit Lategame Upgrades Store", binding: "<Keyboard>/escape", interactions: "Press");
                pageUpAction = InputActionSetupExtensions.AddAction(ActionMap, "Next Page in Lategame Upgrades Store", binding: "<Keyboard>/d", interactions: "Press");
                pageDownAction = InputActionSetupExtensions.AddAction(ActionMap, "Previous Page in Lategame Upgrades Store", binding: "<Keyboard>/a", interactions: "Press");
                storeConfirmAction = InputActionSetupExtensions.AddAction(ActionMap, "Submit Prompt in Lategame Upgrades Store", binding: "<Keyboard>/enter", interactions: "Press");
            }
        }

        [HarmonyPatch(typeof(StartOfRound), "OnEnable")]
        [HarmonyPostfix]
        public static void OnEnable()
        {
            Asset.Enable();
            WheelbarrowAction.performed += OnWheelbarrowActionPerformed;
            NvgAction.performed += OnNvgActionPerformed;
        }

        [HarmonyPatch(typeof(StartOfRound), "OnDisable")]
        [HarmonyPostfix]
        public static void OnDisable()
        {
            Asset.Disable();
            WheelbarrowAction.performed -= OnWheelbarrowActionPerformed;
            NvgAction.performed -= OnNvgActionPerformed;
        }

        private static void OnWheelbarrowActionPerformed(CallbackContext context)
        {
            if (localPlayerController == null || !localPlayerController.isPlayerControlled || (localPlayerController.IsServer && !localPlayerController.isHostPlayerObject))
            {
                return;
            }

            if (!localPlayerController.currentlyHeldObjectServer) return;
            WheelbarrowScript wheelbarrow = localPlayerController.currentlyHeldObjectServer.GetComponent<WheelbarrowScript>();
            if (!wheelbarrow) return;

            wheelbarrow.UpdateWheelbarrowDrop();
        }

        private static void OnNvgActionPerformed(CallbackContext context)
        {
            if (localPlayerController == null || !localPlayerController.isPlayerControlled || (localPlayerController.IsServer && !localPlayerController.isHostPlayerObject))
            {
                return;
            }

            if(NightVision.Instance && !NightVision.Instance.batteryExhaustion)
            {
                NightVision.Instance.Toggle();
            }
        }
    }
}
