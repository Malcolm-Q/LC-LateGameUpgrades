using GameNetcodeStuff;
using HarmonyLib;
using MoreShipUpgrades.Managers;
using MoreShipUpgrades.UpgradeComponents;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using System.Linq;
using UnityEngine;
using MoreShipUpgrades.Misc;

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
            if(!UpgradeBus.instance.cfg.LOSE_NIGHT_VIS_ON_DEATH) { return; }
            if (!__instance.IsOwner) { return; }
            else if (__instance.isPlayerDead) { return; }
            else if (!__instance.AllowPlayerDeath()) { return; }
            if(UpgradeBus.instance.nightVision) { UpgradeBus.instance.UpgradeObjects[nightVisionScript.UPGRADE_NAME].GetComponent<nightVisionScript>().DisableOnClient(); }
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
            bool regularFall = false;
            for(int i = 0; i < codes.Count; i++)
            {
                if (instakillFall && regularFall) break;

                if (codes[i].opcode == OpCodes.Ldc_I4_S)
                {
                    switch (codes[i].operand.ToString())
                    {
                        case "100":
                        case "40":
                            {
                                codes.Insert(i + 1, new CodeInstruction(OpCodes.Call, reduceFallDamageMethod));
                                if (!instakillFall) instakillFall = true; else regularFall = true;
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
            MethodInfo affectWeight = typeof(exoskeletonScript).GetMethod("DecreasePossibleWeight");
            bool found = false;
            List<CodeInstruction> codes = new List<CodeInstruction>(instructions);
            for (int i = 0; i < codes.Count; i++)
            {
                if (found) break;
                if (!(codes[i].opcode == OpCodes.Ldc_R4 && codes[i].operand.ToString() == "0")) continue;
                if (!(codes[i+1].opcode == OpCodes.Ldc_R4 && codes[i+1].operand.ToString() == "10")) continue;
                if (!(codes[i+2].opcode == OpCodes.Call && codes[i+2].operand.ToString() == "Single Clamp(Single, Single, Single)")) continue;
                codes.Insert(i, new CodeInstruction(OpCodes.Call, affectWeight));
                found = true;
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
