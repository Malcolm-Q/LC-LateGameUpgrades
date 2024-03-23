﻿using HarmonyLib;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Reflection;
using MoreShipUpgrades.Misc;
using MoreShipUpgrades.UpgradeComponents.Items;

namespace MoreShipUpgrades.Patches.Enemies
{
    [HarmonyPatch(typeof(SpringManAI))]
    internal static class SpringManAIPatcher
    {
        private static LguLogger logger = new LguLogger(nameof(SpringManAIPatcher));
        [HarmonyPrefix]
        [HarmonyPatch(nameof(SpringManAI.DoAIInterval))]
        static void DoAllIntervalPrefix(ref SpringManAI __instance)
        {
            if (Peeper.HasLineOfSightToPeepers(__instance.transform.position)) __instance.currentBehaviourStateIndex = 1;
        }
        [HarmonyTranspiler]
        [HarmonyPatch(nameof(SpringManAI.Update))]
        static IEnumerable<CodeInstruction> Update_Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            MethodInfo peeperMethod = typeof(Peeper).GetMethod("HasLineOfSightToPeepers", BindingFlags.Public | BindingFlags.Static);
            MethodInfo transformMethod = typeof(SpringManAI).GetMethod("get_transform");
            MethodInfo positionMethod = typeof(UnityEngine.Transform).GetMethod("get_position");

            bool foundStopMovementFlag = false;
            List<CodeInstruction> codes = new List<CodeInstruction>(instructions);
            for (int i = 1; i < codes.Count && !foundStopMovementFlag; i++)
            {
                if (codes[i - 1].opcode != OpCodes.Ldc_I4_0) continue;
                if (codes[i].opcode != OpCodes.Stloc_1) continue;

                codes.Insert(i, new CodeInstruction(OpCodes.Or));
                codes.Insert(i, new CodeInstruction(OpCodes.Call, peeperMethod));
                codes.Insert(i, new CodeInstruction(OpCodes.Callvirt, positionMethod));
                codes.Insert(i, new CodeInstruction(OpCodes.Call, transformMethod));
                codes.Insert(i, new CodeInstruction(OpCodes.Ldarg_0));

                foundStopMovementFlag = true;
            }
            if (!foundStopMovementFlag) logger.LogError("Could not find the attribution of flag that stops movement");
            return codes;
        }
    }

}
