using GameNetcodeStuff;
using HarmonyLib;
using MoreShipUpgrades.Managers;
using MoreShipUpgrades.UpgradeComponents;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using System.Linq;
using UnityEngine;

namespace MoreShipUpgrades.Patches
{
    [HarmonyPatch(typeof(PlayerControllerB))]
    internal class PlayerControllerBPatcher
    {
        [HarmonyPrefix]
        [HarmonyPatch("KillPlayer")]
        private static void DisableUpgradesOnDeath(PlayerControllerB __instance)
        {
            if(!UpgradeBus.instance.cfg.LOSE_NIGHT_VIS_ON_DEATH) { return; }
            if (!__instance.IsOwner) { return; }
            else if (__instance.isPlayerDead) { return; }
            else if (!__instance.AllowPlayerDeath()) { return; }
            if(UpgradeBus.instance.nightVision) { UpgradeBus.instance.UpgradeObjects["NV Headset Batteries"].GetComponent<nightVisionScript>().DisableOnClient(); }
        }


        [HarmonyPrefix]
        [HarmonyPatch("DamagePlayer")]
        private static void beekeeperReduceDamage(ref int damageNumber, CauseOfDeath causeOfDeath, PlayerControllerB __instance)
        {
            if (!UpgradeBus.instance.beePercs.ContainsKey(__instance.playerSteamId) || damageNumber != 10) { return; }
            damageNumber = Mathf.Clamp((int)(damageNumber * (UpgradeBus.instance.cfg.BEEKEEPER_DAMAGE_MULTIPLIER - (UpgradeBus.instance.beePercs[__instance.playerSteamId] * UpgradeBus.instance.cfg.BEEKEEPER_DAMAGE_MULTIPLIER_INCREMENT))),0,100);
        }

        [HarmonyPrefix]
        [HarmonyPatch("DamagePlayerServerRpc")]
        private static void beekeeperReduceDamageServer(ref int damageNumber, PlayerControllerB __instance)
        {
            if (!UpgradeBus.instance.beePercs.ContainsKey(__instance.playerSteamId) || damageNumber != 10) { return; }
            damageNumber = Mathf.Clamp((int)(damageNumber * (UpgradeBus.instance.cfg.BEEKEEPER_DAMAGE_MULTIPLIER - (UpgradeBus.instance.beePercs[__instance.playerSteamId] * UpgradeBus.instance.cfg.BEEKEEPER_DAMAGE_MULTIPLIER_INCREMENT))), 0, 100);
        }

        [HarmonyPrefix]
        [HarmonyPatch("DamagePlayerClientRpc")]
        private static void beekeeperReduceDamageClient(ref int damageNumber, PlayerControllerB __instance)
        {
            if (!UpgradeBus.instance.beePercs.ContainsKey(__instance.playerSteamId) || damageNumber != 10) { return; }
            damageNumber = Mathf.Clamp((int)(damageNumber * (UpgradeBus.instance.cfg.BEEKEEPER_DAMAGE_MULTIPLIER - (UpgradeBus.instance.beePercs[__instance.playerSteamId] * UpgradeBus.instance.cfg.BEEKEEPER_DAMAGE_MULTIPLIER_INCREMENT))), 0, 100);
        }

        [HarmonyPrefix]
        [HarmonyPatch("DamageOnOtherClients")]
        private static void beekeeperReduceDamageOther(ref int damageNumber, PlayerControllerB __instance)
        {
            if (!UpgradeBus.instance.beePercs.ContainsKey(__instance.playerSteamId) || damageNumber != 10) { return; }
            damageNumber = Mathf.Clamp((int)(damageNumber * (UpgradeBus.instance.cfg.BEEKEEPER_DAMAGE_MULTIPLIER - (UpgradeBus.instance.beePercs[__instance.playerSteamId] * UpgradeBus.instance.cfg.BEEKEEPER_DAMAGE_MULTIPLIER_INCREMENT))), 0, 100);
        }

        [HarmonyPatch("DamagePlayer")]
        [HarmonyTranspiler]
        public static IEnumerable<CodeInstruction> DamagePlayerTranspiler(IEnumerable<CodeInstruction> instructions)
        {
            var maximumHealthMethod = typeof(playerHealthScript).GetMethod("CheckForAdditionalHealth", BindingFlags.Public | BindingFlags.Static);
            List<CodeInstruction> codes = instructions.ToList();
            /*
             * ldfld int32 GameNetcodeStuff.PlayerControllerB::health
             * ldarg.1 -> damageNumber
             * sub -> health - damageNumber
             * ldc.i4.0
             * ldc.i4.s 100
             * call int32 clamp(int32, int32, int32) -> Mathf.Clamp(health - damageNumber, 0, 100)
             * stfld int32 health -> health = Mathf.Clamp(health - damageNumber, 0, 100)
             * 
             * We want change 100 to the maximum possible value the player's health can be due to Stimpack upgrade
             */
            bool foundHealthMaximum = false;
            for (int i = 0; i < codes.Count; i++)
            {
                if (!(codes[i].opcode == OpCodes.Ldc_I4_S && codes[i].operand.ToString() == "100")) continue;
                if (!(codes[i + 1] != null && codes[i + 1].opcode == OpCodes.Call && codes[i + 1].operand.ToString() == "Int32 Clamp(Int32, Int32, Int32)")) continue;
                if (!(codes[i + 2] != null && codes[i + 2].opcode == OpCodes.Stfld && codes[i + 2].operand.ToString() == "System.Int32 health")) continue;

                //Mathf.Clamp(health - damageNumber, 0, playerHealthScript.CheckForAdditionalHealth())
                codes[i] = new CodeInstruction(OpCodes.Call, maximumHealthMethod); 
                foundHealthMaximum = true;

                if (foundHealthMaximum) break;
            }
            if (!foundHealthMaximum) Plugin.mls.LogError("Could not find the maximum of Mathf.Clamp that changes the health value");
            return codes.AsEnumerable();
        }


        [HarmonyPrefix]
        [HarmonyPatch("DropAllHeldItems")]
        private static bool DontDropItems(PlayerControllerB __instance)
        {
            if (UpgradeBus.instance.TPButtonPressed)
            {
                UpgradeBus.instance.TPButtonPressed = false;
                __instance.isSinking = false;
                __instance.isUnderwater = false;
                __instance.sinkingValue = 0;
                __instance.statusEffectAudio.Stop();
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
