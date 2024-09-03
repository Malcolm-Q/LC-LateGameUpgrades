using GameNetcodeStuff;
using HarmonyLib;
using LCVR.Player;
using MoreShipUpgrades.Misc.Util;
using MoreShipUpgrades.Patches.PlayerController;
using MoreShipUpgrades.UpgradeComponents.TierUpgrades.Player;
using System.Collections.Generic;
using System.Reflection;

namespace MoreShipUpgrades.Compat
{
    internal static class LethalCompanyVRCompat
    {
        public static bool Enabled =>
            BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey(LCVR.Plugin.PLUGIN_GUID);
    }

    [HarmonyPatch(typeof(VRController))]
    internal static class VRControllerPatcher
    {
        [HarmonyTranspiler]
        [HarmonyPatch(nameof(VRController.LateUpdate))]
        static IEnumerable<CodeInstruction> LateUpdateTranspiler(IEnumerable<CodeInstruction> instructions)
        {
            FieldInfo grabDistance = typeof(PlayerControllerB).GetField(nameof(PlayerControllerB.grabDistance));
            MethodInfo getIncreasedRange = typeof(MechanicalArms).GetMethod(nameof(MechanicalArms.GetIncreasedGrabDistance));
            List<CodeInstruction> codes = new(instructions);
            int index = 0;
            Tools.FindField(ref index, ref codes, findField: grabDistance, addCode: getIncreasedRange, errorMessage: "Couldn't find the grab distance field");
            Tools.FindField(ref index, ref codes, findField: grabDistance, addCode: getIncreasedRange, errorMessage: "Couldn't find the grab distance field");
            return codes;
        }

        [HarmonyTranspiler]
        [HarmonyPatch(nameof(VRController.BeginGrabObject))]
        static IEnumerable<CodeInstruction> BeginGrabObjectTranspiler(IEnumerable<CodeInstruction> instructions)
        {
            FieldInfo grabDistance = typeof(PlayerControllerB).GetField(nameof(PlayerControllerB.grabDistance));
            MethodInfo getIncreasedRange = typeof(MechanicalArms).GetMethod(nameof(MechanicalArms.GetIncreasedGrabDistance));
            List<CodeInstruction> codes = new(instructions);
            int index = 0;
            Tools.FindField(ref index, ref codes, findField: grabDistance, addCode: getIncreasedRange, errorMessage: "Couldn't find the grab distance field");
            return codes;
        }

        [HarmonyTranspiler]
        [HarmonyPatch(nameof(VRController.GrabItem))]
        static IEnumerable<CodeInstruction> GrabItemTranspiler(IEnumerable<CodeInstruction> instructions)
        {
            List<CodeInstruction> codes = new(instructions);
            int index = 0;
            PlayerControllerBPatcher.AddBackMusclesCodeInstruction(ref index, ref codes);
            return codes;
        }
    }
}
