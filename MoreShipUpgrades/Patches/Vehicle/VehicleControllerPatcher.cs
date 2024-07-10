using HarmonyLib;
using MoreShipUpgrades.Misc.Util;
using MoreShipUpgrades.UpgradeComponents.TierUpgrades.Vehicle;
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

        [HarmonyTranspiler]
        [HarmonyPatch(nameof(VehicleController.DamagePlayerInVehicle))]
        static IEnumerable<CodeInstruction> DamagePlayerInVehicleTranspiler(IEnumerable<CodeInstruction> instructions)
        {
            MethodInfo getPlayerDamageMitigation = typeof(FluffySeats).GetMethod(nameof(FluffySeats.GetPlayerDamageMitigation));

            List<CodeInstruction> codes = new(instructions);
            int index = 0;

            Tools.FindInteger(ref index, ref codes, findValue: 40, addCode: getPlayerDamageMitigation, errorMessage: "Couldn't find the damage player with value of 40");
            Tools.FindInteger(ref index, ref codes, findValue: 30, addCode: getPlayerDamageMitigation, errorMessage: "Couldn't find the damage player with value of 30");
            Tools.FindInteger(ref index, ref codes, findValue: 10, addCode: getPlayerDamageMitigation, errorMessage: "Couldn't find the damage player with value of 10");

            return codes;
        }

        [HarmonyTranspiler]
        [HarmonyPatch(nameof(VehicleController.TryIgnition), MethodType.Enumerator)]
        static IEnumerable<CodeInstruction> TryIgnitionTranspiler(IEnumerable<CodeInstruction> instructions)
        {
            FieldInfo chanceToStartIgnition = typeof(VehicleController).GetField(nameof(VehicleController.chanceToStartIgnition), BindingFlags.NonPublic | BindingFlags.Instance);
            MethodInfo getAdditionalIgnitionChance = typeof(IgnitionCoil).GetMethod(nameof(IgnitionCoil.GetAdditionalIgnitionChance));

            List<CodeInstruction> codes = new(instructions);
            int index = 0;

            Tools.FindField(ref index, ref codes, findField: chanceToStartIgnition, addCode: getAdditionalIgnitionChance, errorMessage: "Couldn't find usage of chanceToStartIgnition field");

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
