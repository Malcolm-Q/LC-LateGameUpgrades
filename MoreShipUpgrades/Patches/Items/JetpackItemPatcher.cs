﻿using MoreShipUpgrades.Misc.Util;
using HarmonyLib;
using MoreShipUpgrades.UpgradeComponents.TierUpgrades.Items.Jetpack;
using System.Collections.Generic;
using System.Reflection;

namespace MoreShipUpgrades.Patches.Items
{
    [HarmonyPatch(typeof(JetpackItem))]
    internal static class JetpackItemPatcher
    {
        [HarmonyPatch(nameof(JetpackItem.Update))]
        [HarmonyTranspiler]
        static IEnumerable<CodeInstruction> UpdateTranspiler(IEnumerable<CodeInstruction> instructions)
        {
            FieldInfo jetpackAcceleration = typeof(JetpackItem).GetField(nameof(JetpackItem.jetpackAcceleration));
            MethodInfo IncreaseJetpackAcceleration = typeof(JetFuel).GetMethod(nameof(JetFuel.IncreaseJetpackAcceleration));
            MethodInfo IncreaseJetpackMaximumPower = typeof(JetpackThrusters).GetMethod(nameof(JetpackThrusters.IncreaseJetpackMaximumPower));

            int index = 0;
            List<CodeInstruction> codes = new(instructions);

            Tools.FindField(ref index, ref codes, findField: jetpackAcceleration, addCode: IncreaseJetpackAcceleration, errorMessage: "Couldn't find the jetpack acceleration field used to increase speed while in flight");
            Tools.FindFloat(ref index, ref codes, findValue: 500f, addCode: IncreaseJetpackMaximumPower, errorMessage: "Couldn't find the maximum power allowed when flying.");
            return codes;
        }
    }
}
