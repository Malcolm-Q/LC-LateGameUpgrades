using HarmonyLib;
using MoreShipUpgrades.Misc;
using MoreShipUpgrades.UpgradeComponents.TierUpgrades;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

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
            FieldInfo shovelHitForce = typeof(Shovel).GetField(nameof(Shovel.shovelHitForce));

            List<CodeInstruction> codes = new List<CodeInstruction>(instructions);
            int index = 0;
            index = Tools.FindField(index, ref codes, findField: shovelHitForce, addCode: proteinHitFoce, errorMessage: "Couldn't find shovel hit force field");
            return codes.AsEnumerable();
        }
    }
}
