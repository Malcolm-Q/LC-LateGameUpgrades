using HarmonyLib;
using Mono.Cecil.Cil;
using MoreShipUpgrades.Misc.Util;
using MoreShipUpgrades.UpgradeComponents.TierUpgrades;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace MoreShipUpgrades.Patches.Items
{
    [HarmonyPatch(typeof(PatcherTool))]
    internal static class PatchToolPatcher
    {
        [HarmonyTranspiler]
        [HarmonyPatch(nameof(PatcherTool.BeginShockingAnomalyOnClient))]
        static IEnumerable<CodeInstruction> BeginShockingAnomalyOnClientTranspiler(IEnumerable<CodeInstruction> instructions)
        {
            return PatchDifficulty(new List<CodeInstruction>(instructions));
        }

        [HarmonyTranspiler]
        [HarmonyPatch(nameof(PatcherTool.StopShockingAnomalyOnClient))]
        static IEnumerable<CodeInstruction> StopShockingAnomalyOnClientTranspiler(IEnumerable<CodeInstruction> instructions)
        {
            MethodInfo applyDifficultyMultiplier = typeof(AluminiumCoils).GetMethod(nameof(AluminiumCoils.ApplyCooldownDecrease));
            int index = 0;
            List<CodeInstruction> codes = new List<CodeInstruction>(instructions);
            Tools.FindFloat(ref index, ref codes, findValue: 5f, addCode: applyDifficultyMultiplier, errorMessage: "Couldn't find the method call where it retrieves the enemy's difficulty multiplier");
            return codes;
        }

        static IEnumerable<CodeInstruction> PatchDifficulty(List<CodeInstruction> codes)
        {
            MethodInfo getDifficultyMultiplier = typeof(IShockableWithGun).GetMethod(nameof(IShockableWithGun.GetDifficultyMultiplier));
            MethodInfo applyDifficultyMultiplier = typeof(AluminiumCoils).GetMethod(nameof(AluminiumCoils.ApplyDifficultyDecrease));
            int index = 0;
            Tools.FindMethod(ref index, ref codes, findMethod: getDifficultyMultiplier, addCode: applyDifficultyMultiplier, errorMessage: "Couldn't find the method call where it retrieves the enemy's difficulty multiplier");
            return codes;
        }
        [HarmonyTranspiler]
        [HarmonyPatch(nameof(PatcherTool.ScanGun), MethodType.Enumerator)]
        static IEnumerable<CodeInstruction> ScanGunTranspiler(IEnumerable<CodeInstruction> instructions)
        {
            MethodInfo ApplyIncreasedStunRange = typeof(AluminiumCoils).GetMethod(nameof(AluminiumCoils.ApplyIncreasedStunRange));
            int index = 0;
            List<CodeInstruction> codes = new List<CodeInstruction>(instructions);
            Tools.FindFloat(ref index, ref codes, findValue: 5f, skip: true, errorMessage: "Couldn't skip first occurence of 5f value");
            Tools.FindFloat(ref index, ref codes, findValue: 5f, skip: true, errorMessage: "Couldn't skip second occurence of 5f value");
            Tools.FindFloat(ref index, ref codes, findValue: 5f, addCode: ApplyIncreasedStunRange, errorMessage: "Couldn't find the range used for maximum distance of SphereNonAlloc");
            return codes;
        }
        [HarmonyTranspiler]
        [HarmonyPatch(nameof(PatcherTool.GunMeetsConditionsToShock))]
        static IEnumerable<CodeInstruction> GunMeetsConditionsToShockTranspiler(IEnumerable<CodeInstruction> instructions) 
        {
            MethodInfo ApplyIncreasedStunRange = typeof(AluminiumCoils).GetMethod(nameof(AluminiumCoils.ApplyIncreasedStunRange));
            int index = 0;
            List<CodeInstruction> codes = new List<CodeInstruction>(instructions);
            Tools.FindFloat(ref index, ref codes, findValue: 13f, addCode: ApplyIncreasedStunRange, errorMessage: "Couldn't find the value used for maximum distance");
            return codes;
        }
        [HarmonyTranspiler]
        [HarmonyPatch(nameof(PatcherTool.SetCurrentDifficultyValue))]
        static IEnumerable<CodeInstruction> SetCurrentDifficultyValueTranspiler(IEnumerable<CodeInstruction> instructions)
        {
            return PatchDifficulty(new List<CodeInstruction>(instructions));
        }
        [HarmonyTranspiler]
        [HarmonyPatch(nameof(PatcherTool.InitialDifficultyValues))]
        static IEnumerable<CodeInstruction> InitialDifficultyValuesTranspiler(IEnumerable<CodeInstruction> instructions)
        {
            MethodInfo applyDifficultyMultiplier = typeof(AluminiumCoils).GetMethod(nameof(AluminiumCoils.ApplyDifficultyDecrease));
            FieldInfo endStrengthCap = typeof(PatcherTool).GetField(nameof(PatcherTool.endStrengthCap));
            FieldInfo endChangeSpeedMultiplier = typeof(PatcherTool).GetField(nameof(PatcherTool.endChangeSpeedMultiplier));
            FieldInfo endPullStrength = typeof(PatcherTool).GetField(nameof(PatcherTool.endPullStrength));
            FieldInfo endChangePerFrame = typeof(PatcherTool).GetField(nameof(PatcherTool.endChangePerFrame));
            int index = 0;
            List<CodeInstruction> codes = new List<CodeInstruction>(instructions);
            Tools.FindField(ref index, ref codes, findField: endStrengthCap, addCode: applyDifficultyMultiplier, errorMessage: "Couldn't find the maximum current end strength cap");
            Tools.FindFloat(ref index, ref codes, findValue: 1.4f, addCode: applyDifficultyMultiplier, errorMessage: "Couldn't find the minimum current end strength cap");
            Tools.FindField(ref index, ref codes, findField: endChangeSpeedMultiplier, addCode: applyDifficultyMultiplier, errorMessage: "Couldn't find the maximum current end change speed multiplier");
            Tools.FindFloat(ref index, ref codes, findValue: 7f, addCode: applyDifficultyMultiplier, errorMessage: "Couldn't find the minimum current end change speed multiplier");
            Tools.FindField(ref index, ref codes, findField: endPullStrength, addCode: applyDifficultyMultiplier, errorMessage: "Couldn't find the maximum current end pull strength multiplier");
            Tools.FindFloat(ref index, ref codes, findValue: 0.4f, addCode: applyDifficultyMultiplier, errorMessage: "Couldn't find the minimum current end pull strength multiplier");
            Tools.FindField(ref index, ref codes, findField: endChangePerFrame, addCode: applyDifficultyMultiplier, errorMessage: "Couldn't find the maximum current end change per frame multiplier");
            Tools.FindFloat(ref index, ref codes, findValue: 0.12f, addCode: applyDifficultyMultiplier, errorMessage: "Couldn't find the minimum current end change per frame multiplier");
            return codes;
        }
    }
}
