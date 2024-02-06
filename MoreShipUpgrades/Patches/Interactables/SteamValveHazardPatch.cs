using HarmonyLib;
using MoreShipUpgrades.UpgradeComponents.TierUpgrades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;

namespace MoreShipUpgrades.Patches.Interactables
{
    [HarmonyPatch(typeof(SteamValveHazard))]
    internal class SteamValveHazardPatch
    {
        [HarmonyPostfix]
        [HarmonyPatch(nameof(SteamValveHazard.BurstValve))]
        public static void BurstValvePostFix(ref SteamValveHazard __instance)
        {
            BetterScanner.AddScannerNodeToValve(ref __instance);
        }

        [HarmonyPostfix]
        [HarmonyPatch(nameof(SteamValveHazard.FixValveLocalClient))]
        public static void FixValvePostFix(ref SteamValveHazard __instance)
        {
            BetterScanner.RemoveScannerNodeFromValve(ref __instance);
        }
    }
}
