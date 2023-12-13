using HarmonyLib;
using MoreShipUpgrades.Managers;
using MoreShipUpgrades.UpgradeComponents;
using UnityEngine;

namespace MoreShipUpgrades.Patches
{
    [HarmonyPatch(typeof(SpringManAI))]
    internal class SpringManAIPatcher
    {
        [HarmonyPrefix]
        [HarmonyPatch("Update")]
        private static bool DetectCoilItem(ref SpringManAI __instance, bool ___stoppingMovement)
        {
            if(UpgradeBus.instance.coilHeadItems.Count > 0)
            {
                foreach(coilHeadItem item in UpgradeBus.instance.coilHeadItems)
                {
                    if(item == null) 
                    {
                        UpgradeBus.instance.coilHeadItems.Remove(item);
                        continue;
                    }
                    if (!___stoppingMovement && item.HasLineOfSightToPosition(__instance.transform.position))
                    {
                        __instance.SetAnimationStopServerRpc();
                        return false; // this might be a bad idea
                    }
                }
            }
            return true;
        }
    }

}
