using HarmonyLib;
using MoreShipUpgrades.Misc.Util;
using MoreShipUpgrades.UpgradeComponents.TierUpgrades.Enemies;
using System.Collections.Generic;
using System.Reflection;

namespace MoreShipUpgrades.Patches.Enemies
{
    [HarmonyPatch(typeof(ClaySurgeonAI))]
    internal static class ClaySurgeonAIPatcher
    {
        [HarmonyTranspiler]
        [HarmonyPatch(nameof(ClaySurgeonAI.SetVisibility))]
        static IEnumerable<CodeInstruction> SetVisibilityTranspiler(IEnumerable<CodeInstruction> instructions)
        {
            MethodInfo getAdditionalMaximumDistance = typeof(ClayGlasses).GetMethod(nameof(ClayGlasses.GetAdditionalMaximumDistance));
            FieldInfo maxDistance = typeof(ClaySurgeonAI).GetField(nameof(ClaySurgeonAI.maxDistance));

            List<CodeInstruction> codes = new List<CodeInstruction>(instructions);
            int index = 0;

            Tools.FindField(ref index, ref codes, findField: maxDistance, addCode: getAdditionalMaximumDistance, errorMessage: "Couldn't find the first occurence of maxDistance field");
            Tools.FindField(ref index, ref codes, findField: maxDistance, addCode: getAdditionalMaximumDistance, errorMessage: "Couldn't find the second occurence of maxDistance field");
            return codes;
        }
        [HarmonyTranspiler]
        [HarmonyPatch(nameof(ClaySurgeonAI.AnimationEventA))]
        static IEnumerable<CodeInstruction> AnimationEventATranspiler(IEnumerable<CodeInstruction> instructions)
        {
            MethodInfo getAdditionalMaximumDistance = typeof(ClayGlasses).GetMethod(nameof(ClayGlasses.GetAdditionalMaximumDistance));
            FieldInfo maxDistance = typeof(ClaySurgeonAI).GetField(nameof(ClaySurgeonAI.maxDistance));

            List<CodeInstruction> codes = new List<CodeInstruction>(instructions);
            int index = 0;

            Tools.FindField(ref index, ref codes, findField: maxDistance, addCode: getAdditionalMaximumDistance, errorMessage: "Couldn't find the occurence of maxDistance field");
            return codes;
        }
    }
}
