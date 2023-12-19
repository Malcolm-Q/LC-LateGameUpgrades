using HarmonyLib;
using MoreShipUpgrades.Managers;
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
            if (!UpgradeBus.instance.forceMults.ContainsKey(__instance.playerHeldBy.playerSteamId)){ return; }
            Vector3 start = __instance.playerHeldBy.gameplayCamera.transform.position;
            for(int i = 0; i < ___objectsHitByShovelList.Count; i++)
            {
                IHittable hittable;
                RaycastHit hit;

                if (___objectsHitByShovelList[i].transform.TryGetComponent<IHittable>(out hittable) && !(___objectsHitByShovelList[i].transform == __instance.playerHeldBy.transform) && (___objectsHitByShovelList[i].point == Vector3.zero || !Physics.Linecast(start, ___objectsHitByShovelList[i].point, out hit, StartOfRound.Instance.collidersAndRoomMaskAndDefault)))
                {
                    try
                    {
                        hittable.Hit(UpgradeBus.instance.forceMults[__instance.playerHeldBy.playerSteamId], __instance.playerHeldBy.gameplayCamera.transform.forward, __instance.playerHeldBy, true);
                        Debug.Log($"$$$\n{UpgradeBus.instance.forceMults[__instance.playerHeldBy.playerSteamId]} FORCE\n$$$");
                    }
                    catch(Exception e)
                    {
                        Debug.Log($"Error hitting with protein powder {e}");
                    }
                }
                else if (___objectsHitByShovelList[i].transform.gameObject.layer == 8 || ___objectsHitByShovelList[i].transform.gameObject.layer == 11)
                {
                    break;
                }
            }
        }
    }
}
