using GameNetcodeStuff;
using HarmonyLib;
using MoreShipUpgrades.Compat;
using MoreShipUpgrades.UpgradeComponents.Items.Wheelbarrow;
using MoreShipUpgrades.UpgradeComponents.TierUpgrades;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

namespace MoreShipUpgrades.Input
{
    /// <summary>
    /// Class used to initialize the keybinds on game boot
    /// </summary>
    [HarmonyPatch]
    internal static class Keybinds
    {
        /// <summary>
        /// Asset used to store all the input bindings defined for our controls
        /// </summary>
        public static InputActionAsset Asset;

        public static InputActionMap ActionMap;

        /// <summary>
        /// Input binding used to trigger the drop all items in the wheelbarrow action
        /// </summary>
        public static InputAction WheelbarrowAction;

        /// <summary>
        /// Input binding used to trigger the toggle of Night Vision action
        /// </summary>
        public static InputAction NvgAction;

        /// <summary>
        /// Input binding used to trigger the move cursor up in the LGU interface action
        /// </summary>
        public static InputAction cursorUpAction;

        /// <summary>
        /// Input binding used to trigger the move cursor down in the LGU interface action
        /// </summary>
        public static InputAction cursorDownAction;

        /// <summary>
        /// Input binding used to trigger the exit LGU interface action
        /// </summary>
        public static InputAction cursorExitAction;

        /// <summary>
        /// Input binding used to change to next page in the LGU interface action
        /// </summary>
        public static InputAction pageUpAction;

        /// <summary>
        /// Input binding used to change to previous page in the LGU interface action
        /// </summary>
        public static InputAction pageDownAction;

        /// <summary>
        /// Input binding used to submit current prompt in the LGU interface action
        /// </summary>
        public static InputAction storeConfirmAction;

        public static PlayerControllerB localPlayerController => StartOfRound.Instance?.localPlayerController;

        /// <summary>
        /// Initializes the related assets with the control bindings
        /// </summary>
        [HarmonyPatch(typeof(PreInitSceneScript), "Awake")]
        [HarmonyPrefix]
        public static void AddToKeybindMenu()
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
        /// <summary>
        /// Turn on relevant control bindings when starting a game
        /// </summary>
        [HarmonyPatch(typeof(StartOfRound), "OnEnable")]
        [HarmonyPostfix]
        public static void OnEnable()
        {
            Asset.Enable();
            WheelbarrowAction.performed += OnWheelbarrowActionPerformed;
            NvgAction.performed += OnNvgActionPerformed;
        }

        /// <summary>
        /// Turn off relevant control bindings when exiting a game
        /// </summary>
        [HarmonyPatch(typeof(StartOfRound), "OnDisable")]
        [HarmonyPostfix]
        public static void OnDisable()
        {
            Asset.Disable();
            WheelbarrowAction.performed -= OnWheelbarrowActionPerformed;
            NvgAction.performed -= OnNvgActionPerformed;
        }
        /// <summary>
        /// Function performed when triggering the "Drop all items in Wheelbarrow" control binding
        /// </summary>
        /// <param name="context">Context which triggered this function</param>
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

        /// <summary>
        /// Function performed when triggering the "Toggle the night vision" control binding
        /// </summary>
        /// <param name="context">Context which triggered this function</param>
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
