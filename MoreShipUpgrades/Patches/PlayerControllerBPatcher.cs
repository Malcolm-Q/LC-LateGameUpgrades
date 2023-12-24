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

        [HarmonyTranspiler]
        [HarmonyPatch("BeginGrabObject")]
        public static IEnumerable<CodeInstruction> BeginGrabObjectTranspiler(IEnumerable<CodeInstruction> instructions)
        {
            MethodInfo affectWeight = typeof(exoskeletonScript).GetMethod("CalculateWeight");
            bool found = false;
            List<CodeInstruction> codes = new List<CodeInstruction>(instructions);
            for (int i = 0; i < codes.Count; i++)
            {
                if (found) break;
                if (!(codes[i].opcode == OpCodes.Call && codes[i].operand.ToString() == "Single Clamp(Single, Single, Single)")) continue;

                codes[i] = new CodeInstruction(OpCodes.Call, affectWeight);
                found = true;
            }
            if (!found) Plugin.mls.LogError("BeginGrabObject - Couldn't find Clamp function");
            return codes.AsEnumerable();
        }

        [HarmonyTranspiler]
        [HarmonyPatch("GrabObjectClientRpc")]
        public static IEnumerable<CodeInstruction> GrabObjectClientRpcTranspiler(IEnumerable<CodeInstruction> instructions)
        {
            MethodInfo affectWeight = typeof(exoskeletonScript).GetMethod("CalculateWeight");
            bool found = false;
            List<CodeInstruction> codes = new List<CodeInstruction>(instructions);
            for (int i = 0; i < codes.Count; i++)
            {
                if (found) break;
                if (!(codes[i].opcode == OpCodes.Call && codes[i].operand.ToString() == "Single Clamp(Single, Single, Single)")) continue;

                codes[i] = new CodeInstruction(OpCodes.Call, affectWeight);
                found = true;
            }
            if (!found) Plugin.mls.LogError("GrabObject - Couldn't find Clamp function");
            return codes.AsEnumerable();
        }

        [HarmonyTranspiler]
        [HarmonyPatch("DestroyItemInSlot")]
        public static IEnumerable<CodeInstruction> DestroyItemInSlotTranspiler(IEnumerable<CodeInstruction> instructions)
        {
            ReplaceClampForBackMusclesFunction(ref instructions);
            return instructions.AsEnumerable();
        }

        [HarmonyTranspiler]
        [HarmonyPatch("DespawnHeldObjectOnClient")]
        public static IEnumerable<CodeInstruction> DespawnHeldObjectOnClientTranspiler(IEnumerable<CodeInstruction> instructions)
        {
            ReplaceClampForBackMusclesFunction(ref instructions);
            return instructions.AsEnumerable();
        }
        [HarmonyTranspiler]
        [HarmonyPatch("SetObjectAsNoLongerHeld")]
        public static IEnumerable<CodeInstruction> SetObjectAsNoLongerHeldTranspiler(IEnumerable<CodeInstruction> instructions)
        {
            ReplaceClampForBackMusclesFunction(ref instructions);
            return instructions.AsEnumerable();
        }
        [HarmonyTranspiler]
        [HarmonyPatch("PlaceGrabbableObject")]
        public static IEnumerable<CodeInstruction> PlaceGrabbableObjectTranspiler(IEnumerable<CodeInstruction> instructions)
        {
            ReplaceClampForBackMusclesFunction(ref instructions);
            return instructions.AsEnumerable();
        }

        private static void ReplaceClampForBackMusclesFunction(ref IEnumerable<CodeInstruction> instructions) 
        {
            MethodInfo affectWeight = typeof(exoskeletonScript).GetMethod("CalculateWeight");
            bool found = false;
            List<CodeInstruction> codes = new List<CodeInstruction>(instructions);
            for (int i = 0; i < codes.Count; i++)
            {
                if (found) break;
                if (!(codes[i].opcode == OpCodes.Call && codes[i].operand.ToString() == "Single Clamp(Single, Single, Single)")) continue;
                codes[i] = new CodeInstruction(OpCodes.Call, affectWeight);
                found = true;
            }
            if (!found) Plugin.mls.LogError("MoveNext -Couldn't find Clamp function");
            instructions = codes.AsEnumerable();
        }
        
    }
}
