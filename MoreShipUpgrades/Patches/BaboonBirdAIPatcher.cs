using HarmonyLib;
using MoreShipUpgrades.Misc;
using MoreShipUpgrades.UpgradeComponents.Items.Wheelbarrow;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;

namespace MoreShipUpgrades.Patches
{
    [HarmonyPatch(typeof(BaboonBirdAI))]
    internal class BaboonBirdAIPatcher
    {
        private static LGULogger logger = new LGULogger(nameof(BaboonBirdAIPatcher));
        [HarmonyPatch("DoLOSCheck")]
        [HarmonyTranspiler]
        private static IEnumerable<CodeInstruction> DoLOSCheckTranspiler(IEnumerable<CodeInstruction> instructions)
        {
            List<CodeInstruction> codes = new List<CodeInstruction>(instructions);
            int index = 0;
            index = PatchCheckItemInWheelbarrow(index, ref codes);
            return codes;
        }

        /// <summary>
        /// Adds a condition for the baboon bird to pick up the item only if it's not deposited in a wheelbarrow.
        /// If it is, they will not focus on that item and look for alternatives.
        /// This should solve the issues where the baboon birds would camp a wheelbarrow if it had any items in it
        /// </summary>
        /// <param name="index">Current index transpiling through the code instructions of a given method</param>
        /// <param name="codes">Code instructions of a given method</param>
        /// <returns>Index in which it found the necessary code instruction to make replacements or the end if it didn't find any (this means that our comparisons are wrong)</returns>
        private static int PatchCheckItemInWheelbarrow(int index, ref List<CodeInstruction> codes)
        {
            MethodInfo checkIfInWheelbarrow = typeof(WheelbarrowScript).GetMethod(nameof(WheelbarrowScript.CheckIfItemInWheelbarrow));
            for(; index < codes.Count; index++)
            {
                if (!(codes[index].opcode == OpCodes.Ldloc_S && codes[index].operand.ToString() == "GrabbableObject (18)")) continue;
                if (!(codes[index + 1].opcode == OpCodes.Ldnull)) continue;
                codes.Insert(index + 3, new CodeInstruction(OpCodes.And));
                codes.Insert(index + 3, new CodeInstruction(OpCodes.Not));
                codes.Insert(index + 3, new CodeInstruction(OpCodes.Call, checkIfInWheelbarrow));
                codes.Insert(index + 3, new CodeInstruction(OpCodes.Ldloc_S, codes[index].operand));
                break;
            }
            return index+1;
        }
    }
}
