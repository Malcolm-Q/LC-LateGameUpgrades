using HarmonyLib;
using MoreShipUpgrades.UpgradeComponents.TierUpgrades;
using System;
namespace MoreShipUpgrades.Patches.Interactables
{
    [HarmonyPatch(typeof(SteamValveHazard))]
    internal static class SteamValveHazardPatch
    {
        [HarmonyPostfix]
        [HarmonyPatch(nameof(SteamValveHazard.BurstValve))]
        static void BurstValvePostFix(ref SteamValveHazard __instance)
        {
            BetterScanner.AddScannerNodeToValve(ref __instance);
        }

        [HarmonyPostfix]
        [HarmonyPatch(nameof(SteamValveHazard.FixValveLocalClient))]
        static void FixValvePostFix(ref SteamValveHazard __instance)
        {
            BetterScanner.RemoveScannerNodeFromValve(ref __instance);
        }
    }
}
