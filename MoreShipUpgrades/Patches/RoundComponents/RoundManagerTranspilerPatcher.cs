using HarmonyLib;
using MoreShipUpgrades.UpgradeComponents.Commands;
using System.Collections.Generic;
using System.Reflection;
using MoreShipUpgrades.Misc.Util;

namespace MoreShipUpgrades.Patches.RoundComponents
{
    [HarmonyPatch(typeof(RoundManager))]
    internal static class RoundManagerTranspilerPatcher
    {
        [HarmonyPatch(nameof(RoundManager.DespawnPropsAtEndOfRound))]
        [HarmonyTranspiler]
        static IEnumerable<CodeInstruction> DespawnPropsAtEndOfRoundTranspiler(IEnumerable<CodeInstruction> instructions)
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
