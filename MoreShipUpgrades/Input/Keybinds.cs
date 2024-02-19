using GameNetcodeStuff;
using HarmonyLib;
using MoreShipUpgrades.Compat;
using MoreShipUpgrades.Managers;
using MoreShipUpgrades.UpgradeComponents.Items.Wheelbarrow;
using MoreShipUpgrades.UpgradeComponents.TierUpgrades;
using System;
using System.Collections.Generic;
using System.Text;
using Unity.Netcode;
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

        private static InputAction WheelbarrowAction;

        private static InputAction NvgAction;

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
            }
            else
            {
                Asset = ScriptableObject.CreateInstance<InputActionAsset>();
                ActionMap = new InputActionMap("MoreShipUpgrades");
                InputActionSetupExtensions.AddActionMap(Asset, ActionMap);
                WheelbarrowAction = InputActionSetupExtensions.AddAction(ActionMap, "MoreShipUpgrades.DropAll", binding: UpgradeBus.Instance.PluginConfiguration.WHEELBARROW_DROP_ALL_CONTROL_BIND.Value, interactions: "Press");
                NvgAction = InputActionSetupExtensions.AddAction(ActionMap, "MoreShipUpgrades.NvgToggle", binding: UpgradeBus.Instance.PluginConfiguration.TOGGLE_NIGHT_VISION_KEY.Value, interactions: "Press");
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
