using HarmonyLib;
using MoreShipUpgrades.Misc.Util;
using MoreShipUpgrades.UpgradeComponents.TierUpgrades.Player;
using Oxygen.GameObjects;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace MoreShipUpgrades.Compat
{
    internal static class OxygenCompat
    {
        public static bool Enabled =>
            BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey(Oxygen.OxygenBase.modGUID);

    }

    [HarmonyPatch(typeof(OxygenLogic))]
    internal static class OxygenLogicPatcher
    {
        [HarmonyPatch(nameof(OxygenLogic.RunLogic))]
        [HarmonyTranspiler]
        static IEnumerable<CodeInstruction> RunLogicTranspiler(IEnumerable<CodeInstruction> instructions)
        {
            MethodInfo ReduceOxygenConsumption = typeof(OxygenCanisters).GetMethod(nameof(OxygenCanisters.ReduceOxygenConsumption));
            List<CodeInstruction> codes = new(instructions);
            int index = codes.Count-1;
            Tools.FindLocalFieldReverse(ref index, ref codes, localIndex: 2, addCode: ReduceOxygenConsumption, errorMessage: "Couldn't find the property used to deplete oxygen while underwater");
            return codes;
        }
    }

}
