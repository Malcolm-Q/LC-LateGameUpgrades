using HarmonyLib;
using MoreShipUpgrades.Misc.Util;
using MoreShipUpgrades.UpgradeComponents.OneTimeUpgrades;
using MoreShipUpgrades.UpgradeComponents.TierUpgrades;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace MoreShipUpgrades.Patches.Items
{
    [HarmonyPatch(typeof(Shovel))]
    internal static class ShovelPatcher
    {
        [HarmonyTranspiler]
        [HarmonyPatch(nameof(Shovel.HitShovel))]
        public static IEnumerable<CodeInstruction> HitShovelTranspiler(IEnumerable<CodeInstruction> instructions)
        {
            MethodInfo proteinHitFoce = typeof(ProteinPowder).GetMethod(nameof(ProteinPowder.GetShovelHitForce));
            MethodInfo sickBeatsForce = typeof(SickBeats).GetMethod(nameof(SickBeats.GetShovelHitForce));
            FieldInfo shovelHitForce = typeof(Shovel).GetField(nameof(Shovel.shovelHitForce));

            List<CodeInstruction> codes = new List<CodeInstruction>(instructions);
            int index = 0;
            Tools.FindField(ref index, ref codes, findField: shovelHitForce, addCode: proteinHitFoce, errorMessage: "Couldn't find shovel hit force field");
            Tools.FindMethod(ref index, ref codes, findMethod: proteinHitFoce, addCode: sickBeatsForce, errorMessage: "Couldn't find our Protein Powder shovel hit force method");
            return codes.AsEnumerable();
        }
    }
}
