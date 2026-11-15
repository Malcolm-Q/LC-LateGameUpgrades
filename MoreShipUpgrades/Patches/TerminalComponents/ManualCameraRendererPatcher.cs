using HarmonyLib;
using MoreShipUpgrades.Misc.Util;
using MoreShipUpgrades.UpgradeComponents.OneTimeUpgrades.Ship;
using System.Collections.Generic;
using System.Reflection;

namespace MoreShipUpgrades.Patches.TerminalComponents
{
    [HarmonyPatch(typeof(ManualCameraRenderer))]
    internal static class ManualCameraRendererPatcher
    {
        [HarmonyPatch(nameof(ManualCameraRenderer.CheckIfPlayerIsInCaves))]
        [HarmonyTranspiler]
        public static IEnumerable<CodeInstruction> CheckIfPlayerIsInCavesTranspiler(IEnumerable<CodeInstruction> instructions)
        {
            FieldInfo playerIsInCaves = typeof(ManualCameraRenderer).GetField(nameof(ManualCameraRenderer.playerIsInCaves), BindingFlags.NonPublic | BindingFlags.Instance);
            MethodInfo IsBeaconStronger = typeof(StrongerBeacon).GetMethod(nameof(StrongerBeacon.IsBeaconStronger), BindingFlags.Public | BindingFlags.Static);

            List<CodeInstruction> codes = new(instructions);
            int index = 0;
            Tools.FindInteger(ref index, ref codes, findValue: 1, addCode: IsBeaconStronger,notInstruction: true, andInstruction: true, errorMessage: "Failed to find the playerIsInCaves field");
            return codes;
        }

        [HarmonyPatch(nameof(ManualCameraRenderer.CheckIfPlayerIsInCavesInstantly))]
        [HarmonyTranspiler]
        public static IEnumerable<CodeInstruction> CheckIfPlayerIsInCavesInstantlyTranspiler(IEnumerable<CodeInstruction> instructions)
        {
            FieldInfo playerIsInCaves = typeof(ManualCameraRenderer).GetField(nameof(ManualCameraRenderer.playerIsInCaves), BindingFlags.NonPublic | BindingFlags.Instance);
            MethodInfo IsBeaconStronger = typeof(StrongerBeacon).GetMethod(nameof(StrongerBeacon.IsBeaconStronger), BindingFlags.Public | BindingFlags.Static);

            List<CodeInstruction> codes = new(instructions);
            int index = 0;
            Tools.FindInteger(ref index, ref codes, findValue: 1, skip: true, errorMessage: "Failed to find the playerIsInCaves field attribution value");
            Tools.FindInteger(ref index, ref codes, findValue: 1, addCode: IsBeaconStronger, notInstruction: true, andInstruction: true, errorMessage: "Failed to find the playerIsInCaves field attribution value");
            return codes;
        }
    }
}
