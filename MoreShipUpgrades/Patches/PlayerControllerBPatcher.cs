using GameNetcodeStuff;
using HarmonyLib;
using MoreShipUpgrades.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreShipUpgrades.Patches
{
    [HarmonyPatch(typeof(PlayerControllerB))]
    internal class PlayerControllerBPatcher
    {
        [HarmonyPostfix]
        [HarmonyPatch("SetNightVisionEnabled")]
        private static void overrideNightVision(ref PlayerControllerB __instance)
        {
            __instance.nightVision.enabled = UpgradeBus.instance.nightVisionActive;
            if (__instance.isInsideFactory)
            {
                __instance.nightVision.enabled = true;
            }
        }

        [HarmonyPrefix]
        [HarmonyPatch("LateUpdate")]
        private static void noCarryWeight(ref PlayerControllerB __instance)
        {
            if (UpgradeBus.instance.exoskeleton)
            {
                __instance.carryWeight = 1f;
            }
        }

        [HarmonyPrefix]
        [HarmonyPatch("DropAllHeldItems")]
        private static bool DontDropItems()
        {
            //TODO: Properly sync this so items don't drop for other clients..
            //Netcode scares me.
            if (UpgradeBus.instance.TPButtonPressed)
            {
                UpgradeBus.instance.TPButtonPressed = false;
                return false;
            }
            return true;
        }
    }
}
