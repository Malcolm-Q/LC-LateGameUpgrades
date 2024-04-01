using GameNetcodeStuff;
using HarmonyLib;
using MoreShipUpgrades.Managers;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using System.Linq;
using MoreShipUpgrades.Misc;
using MoreShipUpgrades.UpgradeComponents.Items.Wheelbarrow;
using Unity.Netcode;
using UnityEngine;
using MoreShipUpgrades.UpgradeComponents.OneTimeUpgrades;
using MoreShipUpgrades.UpgradeComponents.TierUpgrades;
using MoreShipUpgrades.UpgradeComponents.TierUpgrades.AttributeUpgrades;
using MoreShipUpgrades.Misc.Upgrades;
using MoreShipUpgrades.Misc.Util;

namespace MoreShipUpgrades.Patches.PlayerController
{
    [HarmonyPatch(typeof(PlayerControllerB))]
    internal static class PlayerControllerBPatcher
    {
        static readonly LguLogger logger = new LguLogger(nameof(PlayerControllerBPatcher));

        [HarmonyPrefix]
        [HarmonyPatch(nameof(PlayerControllerB.KillPlayer))]
        static void DisableUpgradesOnDeath(PlayerControllerB __instance)
        {
            if (!UpgradeBus.Instance.PluginConfiguration.LOSE_NIGHT_VIS_ON_DEATH.Value) return;
            if (!__instance.IsOwner) return;
            if (__instance.isPlayerDead) return;
            if (!__instance.AllowPlayerDeath()) return;

            if (__instance != UpgradeBus.Instance.GetLocalPlayer()) return;
            if (!BaseUpgrade.GetActiveUpgrade(NightVision.UPGRADE_NAME)) return;

            UpgradeBus.Instance.UpgradeObjects[NightVision.UPGRADE_NAME].GetComponent<NightVision>().DisableOnClient();
            if (!UpgradeBus.Instance.PluginConfiguration.NIGHT_VISION_DROP_ON_DEATH.Value) return;
            NightVision.Instance.SpawnNightVisionItemOnDeathServerRpc(__instance.transform.position);
        }

        [HarmonyPatch(nameof(PlayerControllerB.DamagePlayer))]
        [HarmonyTranspiler]
        static IEnumerable<CodeInstruction> DamagePlayerTranspiler(IEnumerable<CodeInstruction> instructions)
        {
            MethodInfo maximumHealthMethod = typeof(Stimpack).GetMethod(nameof(Stimpack.CheckForAdditionalHealth));
            MethodInfo boomboxDefenseMethod = typeof(SickBeats).GetMethod(nameof(SickBeats.CalculateDefense));

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
            for (int i = 0; i < codes.Count && !foundHealthMaximum; i++)
            {
                if (codes[i].opcode == OpCodes.Ldarg_1)
                {
                    codes[i] = new CodeInstruction(OpCodes.Ldarg_1, boomboxDefenseMethod);
                }
                if (!(codes[i].opcode == OpCodes.Ldc_I4_S && codes[i].operand.ToString() == "100")) continue;
                if (!(codes[i + 1] != null && codes[i + 1].opcode == OpCodes.Call && codes[i + 1].operand.ToString() == "Int32 Clamp(Int32, Int32, Int32)")) continue;
                if (!(codes[i + 2] != null && codes[i + 2].opcode == OpCodes.Stfld && codes[i + 2].operand.ToString() == "System.Int32 health")) continue;

                //Mathf.Clamp(health - damageNumber, 0, playerHealthScript.CheckForAdditionalHealth(100))
                codes.Insert(i + 1, new CodeInstruction(OpCodes.Call, maximumHealthMethod));
                foundHealthMaximum = true;
            }
            if (!foundHealthMaximum) Plugin.mls.LogError("Could not find the maximum of Mathf.Clamp that changes the health value");
            return codes.AsEnumerable();
        }

        [HarmonyPrefix]
        [HarmonyPatch(nameof(PlayerControllerB.DamagePlayer))]
        static bool WeDoALittleReturningFalse(PlayerControllerB __instance)
        {
            if (!__instance.IsOwner || __instance.isPlayerDead || !__instance.AllowPlayerDeath()) return true;
            if (UpgradeBus.Instance.wearingHelmet)
            {
                logger.LogDebug($"Player {__instance.playerUsername} is wearing a helmet, executing helmet logic...");
                UpgradeBus.Instance.helmetHits--;
                if (UpgradeBus.Instance.helmetHits <= 0)
                {
                    logger.LogDebug("Helmet has ran out of durability, breaking the helmet...");
                    UpgradeBus.Instance.wearingHelmet = false;
                    if (__instance.IsHost || __instance.IsServer) LguStore.Instance.DestroyHelmetClientRpc(__instance.playerClientId);
                    else LguStore.Instance.ReqDestroyHelmetServerRpc(__instance.playerClientId);
                    if (__instance.IsHost || __instance.IsServer) LguStore.Instance.PlayAudioOnPlayerClientRpc(new NetworkBehaviourReference(__instance), "breakWood");
                    else LguStore.Instance.ReqPlayAudioOnPlayerServerRpc(new NetworkBehaviourReference(__instance), "breakWood");
                }
                else
                {
                    logger.LogDebug($"Helmet still has some durability ({UpgradeBus.Instance.helmetHits}), decreasing it...");
                    if (__instance.IsHost || __instance.IsServer) LguStore.Instance.PlayAudioOnPlayerClientRpc(new NetworkBehaviourReference(__instance), "helmet");
                    else LguStore.Instance.ReqPlayAudioOnPlayerServerRpc(new NetworkBehaviourReference(__instance), "helmet");
                }
                return false;
            }
            return true;
        }

        [HarmonyTranspiler]
        [HarmonyPatch(nameof(PlayerControllerB.PlayerHitGroundEffects))]
        static IEnumerable<CodeInstruction> PlayerHitGroundEffectsTranspiler(IEnumerable<CodeInstruction> instructions)
        {
            MethodInfo reduceFallDamageMethod = typeof(StrongLegs).GetMethod(nameof(StrongLegs.ReduceFallDamage));
            List<CodeInstruction> codes = new List<CodeInstruction>(instructions);
            int index = 0;
            index = Tools.FindInteger(index, ref codes, findValue: 100, addCode: reduceFallDamageMethod, errorMessage: "Couldn't find 100 fall damage");
            index = Tools.FindInteger(index, ref codes, findValue: 50, addCode: reduceFallDamageMethod, errorMessage: "Couldn't find 50 fall damage");
            index = Tools.FindInteger(index, ref codes, findValue: 30, addCode: reduceFallDamageMethod, errorMessage: "Couldn't find 30 fall damage");

            return codes.AsEnumerable();
        }

        [HarmonyPrefix]
        [HarmonyPatch(nameof(PlayerControllerB.DropAllHeldItems))]
        static bool DontDropItems(PlayerControllerB __instance)
        {
            if (!UpgradeBus.Instance.TPButtonPressed) return true;

            UpgradeBus.Instance.TPButtonPressed = false;
            __instance.isSinking = false;
            __instance.isUnderwater = false;
            __instance.sinkingValue = 0;
            __instance.statusEffectAudio.Stop();
            return false;
        }
        [HarmonyPostfix]
        [HarmonyPatch(nameof(PlayerControllerB.GrabObjectClientRpc))]
        static void GrabObjectClientRpcPostfix(PlayerControllerB __instance)
        {
            if (__instance.currentlyHeldObjectServer == null) return;

            WheelbarrowScript wheelbarrow = __instance.currentlyHeldObjectServer.GetComponentInParent<WheelbarrowScript>();
            if (wheelbarrow == null || __instance.currentlyHeldObjectServer is WheelbarrowScript) return;
            logger.LogDebug("Removing item's parent to allow placing it back in again");
            __instance.currentlyHeldObjectServer.transform.SetParent(__instance.currentlyHeldObjectServer.parentObject);
            __instance.currentlyHeldObjectServer.transform.localScale = __instance.currentlyHeldObjectServer.originalScale;
            wheelbarrow.DecrementStoredItems();
        }

        [HarmonyTranspiler]
        [HarmonyPatch(nameof(PlayerControllerB.BeginGrabObject))]
        [HarmonyPatch(nameof(PlayerControllerB.GrabObjectClientRpc))]
        [HarmonyPatch(nameof(PlayerControllerB.DestroyItemInSlot))]
        [HarmonyPatch(nameof(PlayerControllerB.DespawnHeldObjectOnClient))]
        [HarmonyPatch(nameof(PlayerControllerB.SetObjectAsNoLongerHeld))]
        [HarmonyPatch(nameof(PlayerControllerB.PlaceGrabbableObject))]
        static IEnumerable<CodeInstruction> ItemWeightTranspiler(IEnumerable<CodeInstruction> instructions)
        {
            List<CodeInstruction> codes = new List<CodeInstruction>(instructions);
            int index = 0;
            AddBackMusclesCodeInstruction(ref index, ref codes);

            return codes;
        }
        static void AddBackMusclesCodeInstruction(ref int index, ref List<CodeInstruction> codes)
        {
            MethodInfo affectWeight = typeof(BackMuscles).GetMethod(nameof(BackMuscles.DecreasePossibleWeight));
            index = Tools.FindSub(index, ref codes, addCode: affectWeight, errorMessage: "Couldn't find item weight");
        }

        [HarmonyPostfix]
        [HarmonyPatch(nameof(PlayerControllerB.Update))]
        static void CheckForBoomboxes(PlayerControllerB __instance)
        {
            if (!UpgradeBus.Instance.PluginConfiguration.BEATS_ENABLED.Value) return;
            if (!BaseUpgrade.GetActiveUpgrade(SickBeats.UPGRADE_NAME) || __instance != GameNetworkManager.Instance.localPlayerController) return;
            SickBeats.Instance.boomBoxes.RemoveAll(b => b == null);
            bool result = false;
            if (__instance.isPlayerDead)
            {
                if (!SickBeats.Instance.EffectsActive) return; // No need to do anything
                SickBeats.Instance.EffectsActive = result;
                SickBeats.HandlePlayerEffects(__instance);
                return; // Clean all effects from Sick Beats since the player's dead
            }
            foreach (BoomboxItem boom in SickBeats.Instance.boomBoxes)
            {
                if (!boom.isPlayingMusic) continue;

                if (Vector3.Distance(boom.transform.position, __instance.transform.position) >= UpgradeBus.Instance.PluginConfiguration.BEATS_RADIUS.Value) continue;

                result = true;
                break;
            }

            if (result == SickBeats.Instance.EffectsActive) return;

            SickBeats.Instance.EffectsActive = result;
            SickBeats.HandlePlayerEffects(__instance);
        }

        [HarmonyTranspiler]
        [HarmonyPatch(nameof(PlayerControllerB.LateUpdate))]
        static IEnumerable<CodeInstruction> LateUpdateTranspiler(IEnumerable<CodeInstruction> instructions)
        {
            MethodInfo biggerLungsRegenMethod = typeof(BiggerLungs).GetMethod(nameof(BiggerLungs.ApplyPossibleIncreasedStaminaRegen));
            MethodInfo sickBeatsRegenMethod = typeof(SickBeats).GetMethod(nameof(SickBeats.ApplyPossibleIncreasedStaminaRegen));
            List<CodeInstruction> codes = new List<CodeInstruction>(instructions);
            int index = 0;
            index = Tools.FindMul(index, ref codes, skip: true, errorMessage: "Couldn't skip first mul instruction");
            index = Tools.FindMul(index, ref codes, skip: true, errorMessage: "Couldn't skip second mul instruction");
            index = Tools.FindMul(index, ref codes, skip: true, errorMessage: "Couldn't skip third mul instruction");
            index = Tools.FindMul(index, ref codes, skip: true, errorMessage: "Couldn't skip fourth mul instruction");
            index = Tools.FindMul(index, ref codes, skip: true, errorMessage: "Couldn't skip fifth mul instruction");
            index = Tools.FindMul(index, ref codes, skip: true, errorMessage: "Couldn't skip sixth mul instruction");
            index = Tools.FindMul(index, ref codes, addCode: biggerLungsRegenMethod, errorMessage: "Couldn't find first mul instruction to include our regen method from Bigger Lungs");
            codes.Insert(index + 1, new CodeInstruction(OpCodes.Call, sickBeatsRegenMethod));
            index = Tools.FindMul(index, ref codes, addCode: biggerLungsRegenMethod, errorMessage: "Couldn't find second mul instruction to include our regen method from Bigger Lungs");
            codes.Insert(index + 1, new CodeInstruction(OpCodes.Call, sickBeatsRegenMethod));
            return codes;
        }

        [HarmonyTranspiler]
        [HarmonyPatch(nameof(PlayerControllerB.Jump_performed))]
        static IEnumerable<CodeInstruction> JumpPerformedTranspiler(IEnumerable<CodeInstruction> instructions)
        {
            MethodInfo biggerLungsReduceJumpCost = typeof(BiggerLungs).GetMethod(nameof(BiggerLungs.ApplyPossibleReducedJumpStaminaCost));
            List<CodeInstruction> codes = new List<CodeInstruction>(instructions);
            int index = 0;
            index = Tools.FindFloat(index, ref codes, findValue: 0.08f, addCode: biggerLungsReduceJumpCost, errorMessage: "Couldn't find jump stamina cost");
            return codes;
        }

        [HarmonyTranspiler]
        [HarmonyPatch(nameof(PlayerControllerB.PlayFootstepLocal))]
        static IEnumerable<CodeInstruction> PlayFootstepLocalTranspiler(IEnumerable<CodeInstruction> instructions)
        {
            return AddReduceNoiseRangeFunctionToPlayerFootsteps(instructions);
        }

        [HarmonyTranspiler]
        [HarmonyPatch(nameof(PlayerControllerB.PlayFootstepServer))]
        static IEnumerable<CodeInstruction> PlayFootstepServerTranspiler(IEnumerable<CodeInstruction> instructions)
        {
            return AddReduceNoiseRangeFunctionToPlayerFootsteps(instructions);
        }

        static IEnumerable<CodeInstruction> AddReduceNoiseRangeFunctionToPlayerFootsteps(IEnumerable<CodeInstruction> instructions)
        {
            MethodInfo runningShoesReduceNoiseRange = typeof(RunningShoes).GetMethod(nameof(RunningShoes.ApplyPossibleReducedNoiseRange));
            List<CodeInstruction> codes = new List<CodeInstruction>(instructions);
            int index = 0;
            index = Tools.FindFloat(index, ref codes, findValue: 22, addCode: runningShoesReduceNoiseRange, errorMessage: "Couldn't find footstep noise");
            index = Tools.FindFloat(index, ref codes, findValue: 17, addCode: runningShoesReduceNoiseRange, errorMessage: "Couldn't find footstep noise");
            return codes.AsEnumerable();
        }

        [HarmonyPatch(nameof(PlayerControllerB.PlayerLookInput))]
        [HarmonyTranspiler]
        static IEnumerable<CodeInstruction> PlayerLookInputTranspiler(IEnumerable<CodeInstruction> instructions)
        {
            MethodInfo reduceLookSensitivity = typeof(WheelbarrowScript).GetMethod(nameof(WheelbarrowScript.CheckIfPlayerCarryingWheelbarrowLookSensitivity));
            List<CodeInstruction> codes = new List<CodeInstruction>(instructions);
            int index = 0;
            index = Tools.FindFloat(index, ref codes, findValue: 0.008f, addCode: reduceLookSensitivity, errorMessage: "Couldn't find look sensitivity value we wanted to influence");
            return codes;
        }

        [HarmonyPatch(nameof(PlayerControllerB.Update))] // We're all going to die
        [HarmonyTranspiler] // Just kidding
        static IEnumerable<CodeInstruction> UpdateTranspiler(IEnumerable<CodeInstruction> instructions)
        {
            MethodInfo reduceMovement = typeof(WheelbarrowScript).GetMethod(nameof(WheelbarrowScript.CheckIfPlayerCarryingWheelbarrowMovement));
            FieldInfo carryWeight = typeof(PlayerControllerB).GetField(nameof(PlayerControllerB.carryWeight));
            List<CodeInstruction> codes = new List<CodeInstruction>(instructions);
            int index = 0;
            index = Tools.FindField(index, ref codes, findField: carryWeight, skip: true, errorMessage: "Couldn't find ignore first occurence");
            index = Tools.FindField(index, ref codes, findField: carryWeight, addCode: reduceMovement, errorMessage: "Couldn't find second occurence");
            index = Tools.FindField(index, ref codes, findField: carryWeight, addCode: reduceMovement, errorMessage: "Couldn't find third occurence");
            return codes;
        }

        [HarmonyPatch(nameof(PlayerControllerB.Crouch_performed))]
        [HarmonyTranspiler]
        static IEnumerable<CodeInstruction> CrouchPerformmedTranspiler(IEnumerable<CodeInstruction> instructions)
        {
            MethodInfo carryingWheelbarrow = typeof(WheelbarrowScript).GetMethod(nameof(WheelbarrowScript.CheckIfPlayerCarryingWheelbarrow));
            FieldInfo isMenuOpen = typeof(QuickMenuManager).GetField(nameof(QuickMenuManager.isMenuOpen));
            List<CodeInstruction> codes = new List<CodeInstruction>(instructions);
            int index = 0;
            index = Tools.FindField(index, ref codes, findField: isMenuOpen, addCode: carryingWheelbarrow, orInstruction: true, requireInstance: true, errorMessage: "Couldn't find isMenuOpen field");
            return codes;
        }
    }
}
