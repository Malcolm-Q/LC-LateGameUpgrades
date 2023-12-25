using HarmonyLib;
using MoreShipUpgrades.UpgradeComponents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;

namespace MoreShipUpgrades.Patches
{
    [HarmonyPatch(typeof(SteamValveHazard))]
    internal class SteamValveHazardPatch
    {
        [HarmonyPostfix]
        [HarmonyPatch("BurstValve")]
        public static void BurstValvePostFix(ref SteamValveHazard __instance)
        {
            strongerScannerScript.AddScannerNodeToValve(ref __instance);
        }

        [HarmonyPostfix]
        [HarmonyPatch("FixValveLocalClient")]
        public static void FixValvePostFix(ref SteamValveHazard __instance)
        {
            strongerScannerScript.RemoveScannerNodeFromValve(ref __instance);
        }
    }
}
