using HarmonyLib;
using LCVR.Player;
using MoreShipUpgrades.Patches.PlayerController;
using System.Collections.Generic;

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
