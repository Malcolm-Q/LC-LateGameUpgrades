using GameNetcodeStuff;
using HarmonyLib;
using MoreShipUpgrades.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

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
        [HarmonyPatch("DamagePlayer")]
        private static void beekeeperReduceDamage(ref int damageNumber, CauseOfDeath causeOfDeath)
        {
            if (!UpgradeBus.instance.beekeeper || causeOfDeath != CauseOfDeath.Electrocution) { return; }
            damageNumber = (int)(damageNumber * Plugin.cfg.BEEKEEPER_DAMAGE_MULTIPLIER);
            Debug.Log(damageNumber);
        }


        [HarmonyPrefix]
        [HarmonyPatch("DropAllHeldItems")]
        private static bool DontDropItems(PlayerControllerB __instance)
        {
            if (UpgradeBus.instance.TPButtonPressed)
            {
                UpgradeBus.instance.TPButtonPressed = false;
                return false;
            }
            return true;
        }

        [HarmonyPrefix]
        [HarmonyPatch("Update")]
        private static void noCarryWeight(ref PlayerControllerB __instance)
        {
            /*
            Doing carryWeight /= 2 will break it and not have the desired effect.
            carryWeight is ~(-= 1 then * 100). Ex: when your carryweight is 86 lb it's actually 1.86.
            Tallying it up in a for loop and dividing by two in the += in the best way imo.
             */
            if (UpgradeBus.instance.exoskeleton)
            {
                if(__instance.carryWeight != UpgradeBus.instance.alteredWeight && __instance.carryWeight != 1f)
                {
                    UpgradeBus.instance.alteredWeight = 1f;
                    for(int i = 0;  i < __instance.ItemSlots.Length; i++)
                    {
                        GrabbableObject obj = __instance.ItemSlots[i];
                        if(obj != null)
                        {
                            UpgradeBus.instance.alteredWeight += (Mathf.Clamp(obj.itemProperties.weight - 1f, 0f, 10f) * Plugin.cfg.CARRY_WEIGHT_REDUCTION);
                        }
                    }
                    __instance.carryWeight = UpgradeBus.instance.alteredWeight;
                    if(__instance.carryWeight < 1f) { __instance.carryWeight = 1f; }
                }
            }
        }
    }
}
