using HarmonyLib;
using System.Collections.Generic;
using System.Linq;
using static System.Reflection.Emit.OpCodes;

namespace MoreShipUpgrades.Patches
{
    [HarmonyPatch(typeof(SandSpiderAI))]
    internal class SandSpiderAIPatcher
    {
        /// <summary>
        /// Transpiler for the HitEnemy function of SandSpiderAI script.
        /// This is only here for when the developer of the game solves the issue of the health decrease being affected by force instead of only being decremented by one.
        /// </summary>
        /// <param name="instructions">List of IL code instructions from compiling SandSpiderAI script</param>
        /// <returns>Same list of IL code instructions where we modified the instruction related to decrementing by one to be decremented by the parameter "force"</returns>
        [HarmonyTranspiler]
        [HarmonyPatch("HitEnemy")]
        public static IEnumerable<CodeInstruction> HitEnenmyTranspiler(IEnumerable<CodeInstruction> instructions)
        {
            List<CodeInstruction> codes = new List<CodeInstruction>(instructions);
            bool found = false;
            for (int i = 1; i < codes.Count-1; i++)
            {
                if (found) break;
                if (!(codes[i-1].opcode == Ldfld && codes[i-1].operand.ToString() == "System.Int32 health")) continue;
                if (!(codes[i].opcode == Ldc_I4_1)) continue;
                if (!(codes[i+1].opcode == Sub)) continue;

                codes[i] = new CodeInstruction(Ldarg_1);
                found = true;
            }

            return codes.AsEnumerable();
        }
    }
}
