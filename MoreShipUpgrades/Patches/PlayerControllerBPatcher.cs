using GameNetcodeStuff;
using HarmonyLib;
using MoreShipUpgrades.Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace MoreShipUpgrades.Patches
{
    [HarmonyPatch(typeof(PlayerControllerB))]
    internal class PlayerControllerBPatcher
    {
        [HarmonyPrefix]
        [HarmonyPatch("DamagePlayer")]
        private static void beekeeperReduceDamage(ref int damageNumber, CauseOfDeath causeOfDeath)
        {
            if (!UpgradeBus.instance.beekeeper || causeOfDeath != CauseOfDeath.Electrocution || damageNumber != 10) { return; }
            damageNumber = Mathf.Clamp((int)(damageNumber * (UpgradeBus.instance.cfg.BEEKEEPER_DAMAGE_MULTIPLIER - (UpgradeBus.instance.beeLevel * UpgradeBus.instance.cfg.BEEKEEPER_DAMAGE_MULTIPLIER_INCREMENT))),0,100);
        }
        
        [HarmonyPrefix]
        [HarmonyPatch("DamagePlayerServerRpc")]
        private static void beekeeperReduceDamageServer(ref int damageNumber)
        {
            if (!UpgradeBus.instance.beekeeper || damageNumber != 10) { return; }
            damageNumber = Mathf.Clamp((int)(damageNumber * (UpgradeBus.instance.cfg.BEEKEEPER_DAMAGE_MULTIPLIER - (UpgradeBus.instance.beeLevel * UpgradeBus.instance.cfg.BEEKEEPER_DAMAGE_MULTIPLIER_INCREMENT))),0,100);
        }

        [HarmonyPrefix]
        [HarmonyPatch("DamagePlayerClientRpc")]
        private static void beekeeperReduceDamageClient(ref int damageNumber)
        {
            if (!UpgradeBus.instance.beekeeper || damageNumber != 10) { return; }
            damageNumber = Mathf.Clamp((int)(damageNumber * (UpgradeBus.instance.cfg.BEEKEEPER_DAMAGE_MULTIPLIER - (UpgradeBus.instance.beeLevel * UpgradeBus.instance.cfg.BEEKEEPER_DAMAGE_MULTIPLIER_INCREMENT))),0,100);
        }

        [HarmonyPrefix]
        [HarmonyPatch("DamageOnOtherClients")]
        private static void beekeeperReduceDamageOther(ref int damageNumber)
        {
            if (!UpgradeBus.instance.beekeeper || damageNumber != 10) { return; }
            damageNumber = Mathf.Clamp((int)(damageNumber * (UpgradeBus.instance.cfg.BEEKEEPER_DAMAGE_MULTIPLIER - (UpgradeBus.instance.beeLevel * UpgradeBus.instance.cfg.BEEKEEPER_DAMAGE_MULTIPLIER_INCREMENT))),0,100);
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

            // previous optimized solution caused some client side failure. TODO: Optimize this horrific bandaid fix.
            if (UpgradeBus.instance.exoskeleton && __instance.ItemSlots.Length > 0 && GameNetworkManager.Instance.localPlayerController == __instance)
            {
                UpgradeBus.instance.alteredWeight = 1f;
                for(int i = 0;  i < __instance.ItemSlots.Length; i++)
                {
                    GrabbableObject obj = __instance.ItemSlots[i];
                    if(obj != null)
                    {
                        UpgradeBus.instance.alteredWeight += (Mathf.Clamp(obj.itemProperties.weight - 1f, 0f, 10f) * (UpgradeBus.instance.cfg.CARRY_WEIGHT_REDUCTION - (UpgradeBus.instance.backLevel * UpgradeBus.instance.cfg.CARRY_WEIGHT_INCREMENT)));
                    }
                }
                __instance.carryWeight = UpgradeBus.instance.alteredWeight;
                if(__instance.carryWeight < 1f) { __instance.carryWeight = 1f; }
            }
        }
    }
}
