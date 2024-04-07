using HarmonyLib;
using MoreShipUpgrades.Misc.Util;
using MoreShipUpgrades.UpgradeComponents.OneTimeUpgrades;
using MoreShipUpgrades.UpgradeComponents.TierUpgrades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace MoreShipUpgrades.Patches.Items
{
    [HarmonyPatch(typeof(KnifeItem))]
    internal static class KnifePatcher
    {
        [HarmonyTranspiler]
        [HarmonyPatch(nameof(KnifeItem.HitKnife))]
        public static IEnumerable<CodeInstruction> HitKnifeTranspiler(IEnumerable<CodeInstruction> instructions)
        {
            MethodInfo proteinHitFoce = typeof(ProteinPowder).GetMethod(nameof(ProteinPowder.GetShovelHitForce));
            MethodInfo sickBeatsForce = typeof(SickBeats).GetMethod(nameof(SickBeats.GetShovelHitForce));
            FieldInfo knifeHitForce = typeof(KnifeItem).GetField(nameof(KnifeItem.knifeHitForce));

            List<CodeInstruction> codes = new List<CodeInstruction>(instructions);
            int index = 0;
            Tools.FindField(ref index, ref codes, findField: knifeHitForce, addCode: proteinHitFoce, errorMessage: "Couldn't find shovel hit force field");
            Tools.FindMethod(ref index, ref codes, findMethod: proteinHitFoce, addCode: sickBeatsForce, errorMessage: "Couldn't find our Protein Powder shovel hit force method");
            return codes.AsEnumerable();
        }
    }
}
