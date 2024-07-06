using HarmonyLib;
using MoreShipUpgrades.Misc.Util;
using MoreShipUpgrades.UpgradeComponents.TierUpgrades.AttributeUpgrades;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace MoreShipUpgrades.Patches.Vehicle
{
    [HarmonyPatch(typeof(VehicleController))]
    internal static class VehicleControllerPatcher
    {
        [HarmonyTranspiler]
        [HarmonyPatch(nameof(VehicleController.Start))]
        [HarmonyPatch(nameof(VehicleController.Update))]
        [HarmonyPatch(nameof(VehicleController.ReactToDamage))]
        static IEnumerable<CodeInstruction> IncreaseMaximumHealthTranspiler(IEnumerable<CodeInstruction> instructions)
        {
            FieldInfo baseCarHP = typeof(VehicleController).GetField(nameof(VehicleController.baseCarHP));
            MethodInfo getAdditionalMaximumHealth = typeof(VehiclePlating).GetMethod(nameof(VehiclePlating.GetAdditionalMaximumHealth));

            List<CodeInstruction> codes = new(instructions);
            int index = 0;

            Tools.FindField(ref index, ref codes, findField: baseCarHP, addCode: getAdditionalMaximumHealth, errorMessage: "Couldn't find the baseCarHP field");
            return codes;
        }

        [HarmonyPostfix]
        [HarmonyPatch(nameof(VehicleController.Start))]
        static void StartPostfix(VehicleController __instance)
        {
            __instance.carAcceleration = RapidMotors.GetAdditionalAcceleration(__instance.carAcceleration);
            __instance.EngineTorque = SuperchargedPistons.GetAdditionalEngineTorque(__instance.EngineTorque);
            __instance.steeringWheelTurnSpeed = ImprovedSteering.GetAdditionalTurningSpeed(__instance.steeringWheelTurnSpeed);
        }
    }
}
