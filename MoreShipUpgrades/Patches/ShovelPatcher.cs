using HarmonyLib;
using MoreShipUpgrades.Managers;
using MoreShipUpgrades.UpgradeComponents;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace MoreShipUpgrades.Patches
{
    [HarmonyPatch(typeof(Shovel))]
    internal class ShovelPatcher
    {
        [HarmonyPostfix]
        [HarmonyPatch("HitShovel")]
        private static void HitAgain(Shovel __instance, List<RaycastHit> ___objectsHitByShovelList)
        {
            bool playerHasProtein = UpgradeBus.instance.forceMults.ContainsKey(__instance.playerHeldBy.playerSteamId);
            if (!playerHasProtein) return;

            proteinPowderScript.DamageNearbyEnemies(ref __instance, ref ___objectsHitByShovelList);
        }
    }
}
