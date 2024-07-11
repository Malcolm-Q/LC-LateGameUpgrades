using HarmonyLib;
using MoreShipUpgrades.Misc.Util;
using MoreShipUpgrades.UpgradeComponents.TierUpgrades.Items.WeedKiller;
using System.Collections.Generic;
using System.Reflection;

namespace MoreShipUpgrades.Patches.Items
{
    [HarmonyPatch(typeof(SprayPaintItem))]
    internal static class SprayPaintItemPatcher
    {
        [HarmonyTranspiler]
        [HarmonyPatch(nameof(SprayPaintItem.LateUpdate))]
        static IEnumerable<CodeInstruction> LateUpdateTranspiler(IEnumerable<CodeInstruction> instructions)
        {
            FieldInfo killWeedSpeed = typeof(SprayPaintItem).GetField(nameof(SprayPaintItem.killWeedSpeed), BindingFlags.NonPublic | BindingFlags.Instance);
            MethodInfo getAdditionalEffectiveness = typeof(WeedGeneticManipulation).GetMethod(nameof(WeedGeneticManipulation.GetWeedKillerEffectiveness));

            List<CodeInstruction> codes = new(instructions);
            int index = 0;

            Tools.FindField(ref index, ref codes, findField: killWeedSpeed, addCode: getAdditionalEffectiveness, errorMessage: "Couldn't find the usage of killWeedSpeed attribute");

            return codes;
        }
    }
}
