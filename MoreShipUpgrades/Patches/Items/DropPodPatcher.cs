using HarmonyLib;
using MoreShipUpgrades.Misc.Util;
using MoreShipUpgrades.UpgradeComponents.OneTimeUpgrades;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MoreShipUpgrades.Patches.Items
{
    [HarmonyPatch(typeof(ItemDropship))]
    internal static class DropPodPatcher
    {
        [HarmonyPatch(nameof(ItemDropship.Update))]
        [HarmonyTranspiler]
        static IEnumerable<CodeInstruction> UpdateTranspiler(IEnumerable<CodeInstruction> instructions)
        {
            MethodInfo upgradedTimer = typeof(FasterDropPod).GetMethod(nameof(FasterDropPod.GetUpgradedTimer));
            MethodInfo initialTimer = typeof(FasterDropPod).GetMethod(nameof(FasterDropPod.GetFirstOrderTimer));
            List<CodeInstruction> codes = new(instructions);
            int index = 0;

            index = Tools.FindFloat(index, ref codes, findValue: 20, addCode: initialTimer, errorMessage: "Couldn't find the 20 value which is used as first buy ship timer");
            index = Tools.FindFloat(index, ref codes, findValue: 40, addCode: upgradedTimer, errorMessage: "Couldn't find the 40 value which is used as ship timer");
            return codes.AsEnumerable();
        }
    }
}
