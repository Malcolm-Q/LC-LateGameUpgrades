using HarmonyLib;
using MoreShipUpgrades.Managers;
using MoreShipUpgrades.UpgradeComponents;
using System.Collections;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Reflection;

namespace MoreShipUpgrades.Patches
{
    [HarmonyPatch(typeof(SpringManAI))]
    internal class SpringManAIPatcher
    {
        [HarmonyTranspiler]
        [HarmonyPatch("Update")]
        private static IEnumerable<CodeInstruction> Update_Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            MethodInfo peeperMethod = typeof(coilHeadItem).GetMethod("HasLineOfSightToPeepers", BindingFlags.Public | BindingFlags.Static);
            MethodInfo transformMethod = typeof(SpringManAI).GetMethod("get_transform");
            MethodInfo positionMethod = typeof(UnityEngine.Transform).GetMethod("get_position");
            
            bool foundStopMovementFlag = false;
            List<CodeInstruction> codes = new List<CodeInstruction>(instructions);
            for (int i = 3; i < codes.Count; i++)
            {
                if (codes[i - 1].opcode != OpCodes.Ldc_I4_1) continue;
                if (codes[i].opcode != OpCodes.Stloc_1) continue;
                if (codes[i+1].opcode != OpCodes.Ldloc_1) continue;

                codes.Insert(i + 2, new CodeInstruction(OpCodes.Ldloc_1));
                codes.Insert(i+2, new CodeInstruction(OpCodes.Stloc_1)); 
                codes.Insert(i+2, new CodeInstruction(OpCodes.Or));
                codes.Insert(i+2, new CodeInstruction(OpCodes.Call, peeperMethod));
                codes.Insert(i+2, new CodeInstruction(OpCodes.Callvirt, positionMethod));
                codes.Insert(i+2, new CodeInstruction(OpCodes.Call, transformMethod));
                codes.Insert(i+2, new CodeInstruction(OpCodes.Ldarg_0));

                foundStopMovementFlag = true;

                if (foundStopMovementFlag) break;
            }
            if (!foundStopMovementFlag) Plugin.mls.LogError("Could not find the attribution of flag that stops movement");
            return codes;
        }
    }

}
