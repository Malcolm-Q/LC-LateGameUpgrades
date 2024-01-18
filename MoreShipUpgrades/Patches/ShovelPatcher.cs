using HarmonyLib;
using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc;
using MoreShipUpgrades.UpgradeComponents.TierUpgrades;
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
        private static LGULogger logger = new LGULogger(nameof(ShovelPatcher));
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

                codes.Insert(i+1, new CodeInstruction(OpCodes.Call, proteinHitFoce));
                found = true;
            }
            if (!found) { logger.LogError($"Did not find the hit force of the shovel to influence with {proteinPowderScript.UPGRADE_NAME}"); }
            return codes.AsEnumerable();
        }
    }
}
