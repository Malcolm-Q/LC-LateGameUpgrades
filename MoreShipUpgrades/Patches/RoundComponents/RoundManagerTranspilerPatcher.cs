using HarmonyLib;
using MoreShipUpgrades.UpgradeComponents.Commands;
using System.Collections.Generic;
using System.Reflection;
using MoreShipUpgrades.Misc.Util;
using MoreShipUpgrades.UpgradeComponents.TierUpgrades.Ship;

namespace MoreShipUpgrades.Patches.RoundComponents
{
    [HarmonyPatch(typeof(RoundManager))]
    internal static class RoundManagerTranspilerPatcher
    {
        [HarmonyPatch(nameof(RoundManager.SpawnScrapInLevel))]
        [HarmonyTranspiler]
        static IEnumerable<CodeInstruction> SpawnScrapInLevelTranspiler(IEnumerable<CodeInstruction> instructions)
        {
            FieldInfo minimumScrap = typeof(SelectableLevel).GetField(nameof(SelectableLevel.minScrap));
            FieldInfo maximumScrap = typeof(SelectableLevel).GetField(nameof(SelectableLevel.maxScrap));
            MethodInfo increaseScrap = typeof(ScavengerInstincts).GetMethod(nameof(ScavengerInstincts.IncreaseScrapAmount));

            List<CodeInstruction> codes = new(instructions);
            int index = 0;
            Tools.FindField(ref index, ref codes, findField: minimumScrap, addCode: increaseScrap, errorMessage: "Couldn't find level's minimum scrap amount");
            Tools.FindField(ref index, ref codes, findField: maximumScrap, addCode: increaseScrap, errorMessage: "Couldn't find level's maximum scrap amount");
            return codes;
        }
    }
}
