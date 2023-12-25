using HarmonyLib;
using MoreShipUpgrades.Managers;
using MoreShipUpgrades.UpgradeComponents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using UnityEngine;

namespace MoreShipUpgrades.Patches
{
    [HarmonyPatch(typeof(Shovel))]
    internal class ShovelPatcher
    {
        [HarmonyTranspiler]
        [HarmonyPatch("HitShovel")]
        public static IEnumerable<CodeInstruction> HitShovelTranspiler(IEnumerable<CodeInstruction> instructions)
        {
            MethodInfo proteinHitFoce = typeof(proteinPowderScript).GetMethod("GetShovelHitForce", BindingFlags.Static | BindingFlags.Public);
            List<CodeInstruction> codes = new List<CodeInstruction>(instructions);
            bool found = false;
            for (int i = 1; i < codes.Count; i++)
            {
                if (found) break;
                if (!(codes[i - 1].opcode == OpCodes.Ldarg_0)) continue;
                if (!(codes[i].opcode == OpCodes.Ldfld && codes[i].operand.ToString() == "System.Int32 shovelHitForce")) continue;

                codes[i] = new CodeInstruction(OpCodes.Call, proteinHitFoce);
                codes.Remove(codes[i-1]);
                found = true;
            }
            return codes.AsEnumerable();
        }
    }
}
