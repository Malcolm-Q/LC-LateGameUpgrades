using HarmonyLib;
using MoreShipUpgrades.UpgradeComponents.Commands;
using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Reflection;
using System.Text;
using MoreShipUpgrades.Misc;

namespace MoreShipUpgrades.Patches.RoundComponents
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
            int index = 0;
            index = Tools.FindField(index, ref codes, findField: allPlayersDead, addCode: scrapInsuranceStatus, notInstruction: true, andInstruction: true, errorMessage: "Couldn't find all players dead field");
            return codes;
        }
    }
}
