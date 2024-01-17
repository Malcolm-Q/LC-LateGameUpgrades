using HarmonyLib;
using MoreShipUpgrades.UpgradeComponents.Commands;
using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Reflection;
using System.Text;
using MoreShipUpgrades.Misc;

namespace MoreShipUpgrades.Patches
{
    [HarmonyPatch(typeof(RoundManager))]
    internal class RoundManagerTranspilerPatcher
    {
        private static LGULogger logger = new LGULogger(nameof(RoundManagerPatcher));
        [HarmonyPatch(nameof(RoundManager.DespawnPropsAtEndOfRound))]
        [HarmonyTranspiler]
        public static IEnumerable<CodeInstruction> DespawnPropsAtEndOfRoundTranspiler(IEnumerable<CodeInstruction> instructions)
        {
            FieldInfo allPlayersDead = typeof(StartOfRound).GetField(nameof(StartOfRound.allPlayersDead));
            MethodInfo scrapInsuranceStatus = typeof(ScrapInsurance).GetMethod(nameof(ScrapInsurance.GetScrapInsuranceStatus));

            List<CodeInstruction> codes = new List<CodeInstruction>(instructions);
            for (int i = 0; i < codes.Count; i++)
            {
                if (!(codes[i].opcode == OpCodes.Ldfld && codes[i].operand == (object)allPlayersDead)) continue;
                codes.Insert(i + 1, new CodeInstruction(OpCodes.And));
                codes.Insert(i + 1, new CodeInstruction(OpCodes.Not));
                codes.Insert(i + 1, new CodeInstruction(OpCodes.Call, scrapInsuranceStatus));
                break;
            }
            return codes;
        }
    }
}
