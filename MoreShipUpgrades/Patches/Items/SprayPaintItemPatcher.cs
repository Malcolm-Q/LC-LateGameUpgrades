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
            //FieldInfo killWeedSpeed = typeof(SprayPaintItem).GetField(nameof(SprayPaintItem.killWeedSpeed), BindingFlags.NonPublic | BindingFlags.Instance);
            MethodInfo getAdditionalEffectiveness = typeof(WeedGeneticManipulation).GetMethod(nameof(WeedGeneticManipulation.GetWeedKillerEffectiveness));

            List<CodeInstruction> codes = new(instructions);
            int index = 0;

            Tools.FindFloat(ref index, ref codes, findValue: 2.2f, addCode: getAdditionalEffectiveness, errorMessage: "Couldn't find the usage of killWeedSpeed attribute");
			Tools.FindFloat(ref index, ref codes, findValue: 1.7f, addCode: getAdditionalEffectiveness, errorMessage: "Couldn't find the usage of killWeedSpeed attribute");

			return codes;
        }
    }
}
