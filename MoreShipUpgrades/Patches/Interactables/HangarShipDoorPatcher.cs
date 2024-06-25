using HarmonyLib;
using MoreShipUpgrades.Misc.Util;
using MoreShipUpgrades.UpgradeComponents.TierUpgrades;
using System.Collections.Generic;
using System.Reflection;

namespace MoreShipUpgrades.Patches.Interactables
{
    [HarmonyPatch(typeof(HangarShipDoor))]
    internal static class HangarShipDoorPatcher
    {
        [HarmonyPatch(nameof(HangarShipDoor.Update))]
        [HarmonyTranspiler]
        static IEnumerable<CodeInstruction> UpdateTranspiler(IEnumerable<CodeInstruction> instructions)
        {
            MethodInfo additionalBatteryDuration = typeof(ShutterBatteries).GetMethod(nameof(ShutterBatteries.GetAdditionalDoorTime));

            FieldInfo doorBattery = typeof(HangarShipDoor).GetField(nameof(HangarShipDoor.doorPowerDuration));
            int index = 0;
            List<CodeInstruction> codes = new(instructions);
            Tools.FindField(ref index, ref codes, findField: doorBattery, addCode: additionalBatteryDuration, errorMessage: "Couldn't find first occurence of door battery duration field");
            Tools.FindField(ref index, ref codes, findField: doorBattery, addCode: additionalBatteryDuration, errorMessage: "Couldn't find second occurence of door battery duration field");
            return codes;
        }
    }
}
