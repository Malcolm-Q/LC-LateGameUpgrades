﻿using GameNetcodeStuff;
using HarmonyLib;
using MoreShipUpgrades.Managers;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using System.Linq;
using MoreShipUpgrades.Misc;
using MoreShipUpgrades.UpgradeComponents;

namespace MoreShipUpgrades.Patches
{
    [HarmonyPatch(typeof(PlayerControllerB))]
    internal class PlayerControllerBPatcher
    {
        internal static LGULogger logger = new LGULogger(nameof(PlayerControllerBPatcher));
        [HarmonyPrefix]
        [HarmonyPatch("KillPlayer")]
        private static void DisableUpgradesOnDeath(PlayerControllerB __instance)
        {
            if (!UpgradeBus.instance.cfg.LOSE_NIGHT_VIS_ON_DEATH) { return; }
            if (!__instance.IsOwner) { return; }
            else if (__instance.isPlayerDead) { return; }
            else if (!__instance.AllowPlayerDeath()) { return; }
            if (!UpgradeBus.instance.nightVision) return;

            UpgradeBus.instance.UpgradeObjects[nightVisionScript.UPGRADE_NAME].GetComponent<nightVisionScript>().DisableOnClient();
            if (!UpgradeBus.instance.cfg.NIGHT_VISION_DROP_ON_DEATH) return;
            LGUStore.instance.SpawnNightVisionItemOnDeathServerRpc(__instance.transform.position);
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

        [HarmonyTranspiler]
        [HarmonyPatch("PlayerHitGroundEffects")]
        private static IEnumerable<CodeInstruction> PlayerHitGroundEffectsTranspiler(IEnumerable<CodeInstruction> instructions)
        {
            MethodInfo reduceFallDamageMethod = typeof(strongLegsScript).GetMethod("ReduceFallDamage", BindingFlags.Static | BindingFlags.Public);
            List<CodeInstruction> codes = new List<CodeInstruction>(instructions);
            bool instakillFall = false;
            bool smallFall = false;
            bool bigFall = false;
            for(int i = 0; i < codes.Count; i++)
            {
                if (instakillFall && smallFall && bigFall) break;

                if (codes[i].opcode == OpCodes.Ldc_I4_S)
                {
                    switch (codes[i].operand.ToString())
                    {
                        case "100":
                        case "50":
                        case "30":
                            {
                                codes.Insert(i + 1, new CodeInstruction(OpCodes.Call, reduceFallDamageMethod));
                                if (!instakillFall) instakillFall = true;
                                else if( instakillFall ) smallFall = true;
                                else bigFall = true;
                                continue;
                            }
                        default: continue;
                    }
                }
            }

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
            ReplaceClampForBackMusclesFunction(ref instructions);
            return instructions.AsEnumerable();
        }

        [HarmonyTranspiler]
        [HarmonyPatch("GrabObjectClientRpc")]
        public static IEnumerable<CodeInstruction> GrabObjectClientRpcTranspiler(IEnumerable<CodeInstruction> instructions)
        {
            ReplaceClampForBackMusclesFunction(ref instructions);
            return instructions.AsEnumerable();
        }
        [HarmonyPostfix]
        [HarmonyPatch("GrabObjectClientRpc")]
        public static void GrabObjectClientRpcPostfix(PlayerControllerB __instance)
        {
            WheelbarrowScript wheelbarrow = __instance.currentlyHeldObjectServer.GetComponentInParent<WheelbarrowScript>();
            if (wheelbarrow == null) return;
            logger.LogDebug("Removing item's parent to allow placing it back in again");
            __instance.currentlyHeldObjectServer.transform.parent = null;
            wheelbarrow.DecrementStoredItems();
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

        /// <summary>
        /// Function responsible to multiply the result of the operation through vanilla code to our own multiplier.
        /// This assumes that any other mods that do decide to transpile the function won't put an Add or Sub operations between the storing
        /// of the "carryWeight" variable and actual sub/add associated with the vanila code
        /// </summary>
        /// <param name="instructions">List of IL instructions from a given function/method</param>
        private static void ReplaceClampForBackMusclesFunction(ref IEnumerable<CodeInstruction> instructions)
        {
            MethodInfo affectWeight = typeof(exoskeletonScript).GetMethod("DecreasePossibleWeight");
            bool found = false;
            List<CodeInstruction> codes = new List<CodeInstruction>(instructions);
            for (int i = codes.Count - 1; i >= 0 && !found; i--)
            {
                if (!(codes[i].opcode == OpCodes.Stfld && codes[i].operand.ToString() == "System.Single carryWeight")) continue;
                for (int j = i - 2; j >= 0 && !found; j--)
                {
                    if (!(codes[j].opcode == OpCodes.Sub || codes[j].opcode == OpCodes.Add)) continue;

                    codes.Insert(j+1, new CodeInstruction(OpCodes.Call, affectWeight));
                    found = true;
                }
            }
            instructions = codes.AsEnumerable();
        }

        [HarmonyTranspiler]
        [HarmonyPatch("LateUpdate")]
        private static IEnumerable<CodeInstruction> LateUpdateTranspiler(IEnumerable<CodeInstruction> instructions)
        {
            MethodInfo biggerLungsRegenMethod = typeof(biggerLungScript).GetMethod("ApplyPossibleIncreasedStaminaRegen", BindingFlags.Static | BindingFlags.Public);
            List<CodeInstruction> codes = new List<CodeInstruction>(instructions);
            bool first = false;
            bool second = false;
            for(int i = 0; i < codes.Count-3; i++) 
            {
                if (first && second) break;
                if (!(codes[i].opcode == OpCodes.Add)) continue;
                if (!(codes[i + 3].opcode == OpCodes.Call && codes[i + 3].operand.ToString() == "Single Clamp(Single, Single, Single)")) continue;

                codes.Insert(i, new CodeInstruction(OpCodes.Call, biggerLungsRegenMethod));
                if (!first) first = true;
                else second = true;

            }
            return codes.AsEnumerable();
        }

        [HarmonyTranspiler]
        [HarmonyPatch("Jump_performed")]
        private static IEnumerable<CodeInstruction> JumpPerformedTranspiler(IEnumerable<CodeInstruction> instructions)
        {
            MethodInfo biggerLungsReduceJumpCost = typeof(biggerLungScript).GetMethod("ApplyPossibleReducedJumpStaminaCost", BindingFlags.Static | BindingFlags.Public);
            List<CodeInstruction> codes = new List<CodeInstruction>(instructions);
            bool found = false;
            for (int i = 0; i < codes.Count; i++)
            {
                if (found) break;
                if (!(codes[i].opcode == OpCodes.Ldc_R4 && codes[i].operand.ToString() == "0,08")) continue;

                codes.Insert(i+1, new CodeInstruction(OpCodes.Call, biggerLungsReduceJumpCost));
                found = true;
            }
            return codes.AsEnumerable();
        }

        [HarmonyTranspiler]
        [HarmonyPatch("PlayFootstepLocal")]
        private static IEnumerable<CodeInstruction> PlayFootstepLocalTranspiler(IEnumerable<CodeInstruction> instructions)
        {
            return AddReduceNoiseRangeFunctionToPlayerFootsteps(instructions);
        }

        [HarmonyTranspiler]
        [HarmonyPatch("PlayFootstepServer")]
        private static IEnumerable<CodeInstruction> PlayFootstepServerTranspiler(IEnumerable<CodeInstruction> instructions)
        {
            return AddReduceNoiseRangeFunctionToPlayerFootsteps(instructions);
        }

        private static IEnumerable<CodeInstruction> AddReduceNoiseRangeFunctionToPlayerFootsteps(IEnumerable<CodeInstruction> instructions)
        {
            MethodInfo runningShoesReduceNoiseRange = typeof(runningShoeScript).GetMethod("ApplyPossibleReducedNoiseRange", BindingFlags.Static | BindingFlags.Public);
            List<CodeInstruction> codes = new List<CodeInstruction>(instructions);
            bool walking = false;
            bool sprinting = false;
            for (int i = 0; i < codes.Count; i++)
            {
                if (walking && sprinting) break;
                if (codes[i].opcode != OpCodes.Ldc_R4) continue;
                switch (codes[i].operand.ToString())
                {
                    case "22":
                        {
                            codes.Insert(i + 1, new CodeInstruction(OpCodes.Call, runningShoesReduceNoiseRange));
                            sprinting = true;
                            break;
                        }
                    case "17":
                        {
                            codes.Insert(i + 1, new CodeInstruction(OpCodes.Call, runningShoesReduceNoiseRange));
                            walking = true;
                            break;
                        }
                    default: break;
                }
            }
            return codes.AsEnumerable();
        }
    }
}
