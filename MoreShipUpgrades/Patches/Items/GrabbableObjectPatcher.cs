﻿using HarmonyLib;
using MoreShipUpgrades.Misc.Util;
using MoreShipUpgrades.UpgradeComponents.TierUpgrades.Items;
using System.Collections.Generic;
using System.Reflection;

namespace MoreShipUpgrades.Patches.Items
{
    [HarmonyPatch(typeof(GrabbableObject))]
    internal static class GrabbableObjectPatcher
    {
        [HarmonyTranspiler]
        [HarmonyPatch(nameof(GrabbableObject.UseItemBatteries))]
        static IEnumerable<CodeInstruction> UseItemBatteriesTranspiler(IEnumerable<CodeInstruction> instructions)
        {
            MethodInfo GetChargeOnUse = typeof(LithiumBatteries).GetMethod(nameof(LithiumBatteries.GetChargeRateMultiplier));
            FieldInfo batteryUsage = typeof(Item).GetField(nameof(Item.batteryUsage));
            List<CodeInstruction> codes = new(instructions);
            int index = 0;
            Tools.FindField(ref index, ref codes, batteryUsage, GetChargeOnUse, errorMessage: "Couldn't find the field which is used to drain the item's battery on use");
            return codes;
        }
        [HarmonyTranspiler]
        [HarmonyPatch(nameof(GrabbableObject.Update))]
        static IEnumerable<CodeInstruction> UpdateTranspiler(IEnumerable<CodeInstruction> instructions)
        {
            MethodInfo GetChargeRate = typeof(LithiumBatteries).GetMethod(nameof(LithiumBatteries.GetChargeRateMultiplier));
            FieldInfo batteryUsage = typeof(Item).GetField(nameof(Item.batteryUsage));
            List<CodeInstruction> codes = new(instructions);
            int index = 0;
            Tools.FindField(ref index, ref codes, batteryUsage, skip: true, errorMessage: "Couldn't find the field which is used to drain the item's battery passively");
            Tools.FindDiv(ref index, ref codes, GetChargeRate, errorMessage: "Couldn't find the field which is used to drain the item's battery passively");
            return codes;
        }
    }
}
