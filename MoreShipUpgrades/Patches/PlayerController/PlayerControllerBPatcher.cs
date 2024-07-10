using GameNetcodeStuff;
using HarmonyLib;
using MoreShipUpgrades.Managers;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using System.Linq;
using MoreShipUpgrades.UpgradeComponents.Items.Wheelbarrow;
using UnityEngine;
using MoreShipUpgrades.UpgradeComponents.OneTimeUpgrades;
using MoreShipUpgrades.UpgradeComponents.TierUpgrades;
using MoreShipUpgrades.UpgradeComponents.TierUpgrades.AttributeUpgrades;
using MoreShipUpgrades.Misc.Upgrades;
using MoreShipUpgrades.Misc.Util;
using MoreShipUpgrades.UpgradeComponents.Items;
using MoreShipUpgrades.UpgradeComponents.TierUpgrades.Player;

namespace MoreShipUpgrades.Patches.PlayerController
{
    [HarmonyPatch(typeof(PlayerControllerB))]
    internal static class PlayerControllerBPatcher
    {
        [HarmonyPostfix]
        [HarmonyPatch(nameof(PlayerControllerB.Awake))]
        static void StartPostfix(PlayerControllerB __instance)
        {
            __instance.gameObject.AddComponent<PlayerManager>();
        }

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

            Plugin.mls.LogDebug($"Player that died: {__instance.playerUsername}");
            Plugin.mls.LogDebug($"Local player we are deactivating to: {UpgradeBus.Instance.GetLocalPlayer().playerUsername}");

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
                    codes.Insert(i+1, new CodeInstruction(OpCodes.Call, boomboxDefenseMethod));
                    continue;
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
        static bool DamagePlayerPrefix(ref PlayerControllerB __instance, ref int damageNumber)
        {
            if (!__instance.IsOwner || __instance.isPlayerDead || !__instance.AllowPlayerDeath()) return true;
            if (UpgradeBus.Instance.wearingHelmet)
            {
                Helmet.ExecuteHelmetDamageMitigation(ref __instance, ref damageNumber);
            }
            return damageNumber != 0;
        }

        [HarmonyTranspiler]
        [HarmonyPatch(nameof(PlayerControllerB.PlayerHitGroundEffects))]
        static IEnumerable<CodeInstruction> PlayerHitGroundEffectsTranspiler(IEnumerable<CodeInstruction> instructions)
        {
            MethodInfo reduceFallDamageMethod = typeof(ReinforcedBoots).GetMethod(nameof(ReinforcedBoots.ReduceFallDamage));
            List<CodeInstruction> codes = new(instructions);
            int index = 0;
            Tools.FindInteger(ref index, ref codes, findValue: 100, addCode: reduceFallDamageMethod, errorMessage: "Couldn't find 100 fall damage");
            Tools.FindInteger(ref index, ref codes, findValue: 80, addCode: reduceFallDamageMethod, errorMessage: "Couldn't find 80 fall damage");
            Tools.FindInteger(ref index, ref codes, findValue: 50, addCode: reduceFallDamageMethod, errorMessage: "Couldn't find 50 fall damage");
            Tools.FindInteger(ref index, ref codes, findValue: 30, addCode: reduceFallDamageMethod, errorMessage: "Couldn't find 30 fall damage");

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
        [HarmonyPatch(nameof(PlayerControllerB.SwitchToItemSlot))]
        [HarmonyPatch(nameof(PlayerControllerB.BeginGrabObject))]
        static void SwitchToItemSlotPostfix(PlayerControllerB __instance)
        {
            DeepPocketsTwoHandedCheck(__instance);
        }

        [HarmonyPostfix]
        [HarmonyPatch(nameof(PlayerControllerB.GrabObjectClientRpc))]
        static void GrabObjectClientRpcPostfix(PlayerControllerB __instance)
        {
            WheelbarrowUnparenting(__instance.currentlyHeldObjectServer);
            DeepPocketsTwoHandedCheck(__instance);
        }
        static void DeepPocketsTwoHandedCheck(PlayerControllerB player)
        {
            if (!player.twoHanded) return;
            if (!BaseUpgrade.GetActiveUpgrade(DeepPockets.UPGRADE_NAME)) return;

            if (!UpgradeBus.Instance.PluginConfiguration.DEEPER_POCKETS_ALLOW_WHEELBARROWS)
            {
                // No putting wheelbarrows in your deeper pockets
                if (player.currentlyHeldObjectServer is WheelbarrowScript) return;
                if (player.currentlyHeldObject is WheelbarrowScript) return;
            }
            int twoHandedCount = 0;
            int maxTwoHandedCount = 1 + UpgradeBus.Instance.PluginConfiguration.DEEPER_POCKETS_INITIAL_TWO_HANDED_ITEMS + (BaseUpgrade.GetUpgradeLevel(DeepPockets.UPGRADE_NAME) * UpgradeBus.Instance.PluginConfiguration.DEEPER_POCKETS_INCREMENTAL_TWO_HANDED_ITEMS);

            for(int i = 0; i < player.ItemSlots.Length && twoHandedCount < maxTwoHandedCount; i++)
            {
                GrabbableObject item = player.ItemSlots[i];
                if (item == null) continue;
                if (!item.itemProperties.twoHanded) continue;
                twoHandedCount++;
            }
            if (twoHandedCount < maxTwoHandedCount)
            {
                player.twoHanded = false;
                HUDManager.Instance.holdingTwoHandedItem.enabled = false;
            }
        }
        static void WheelbarrowUnparenting(GrabbableObject heldObject)
        {
            if (heldObject == null) return;

            WheelbarrowScript wheelbarrow = heldObject.GetComponentInParent<WheelbarrowScript>();
            if (wheelbarrow == null || heldObject is WheelbarrowScript) return;
            heldObject.transform.SetParent(heldObject.parentObject);
            heldObject.transform.localScale = heldObject.originalScale;
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
            List<CodeInstruction> codes = new(instructions);
            int index = 0;
            AddBackMusclesCodeInstruction(ref index, ref codes);

            return codes;
        }
        static void AddBackMusclesCodeInstruction(ref int index, ref List<CodeInstruction> codes)
        {
            MethodInfo affectWeight = typeof(BackMuscles).GetMethod(nameof(BackMuscles.DecreasePossibleWeight));
            Tools.FindSub(ref index, ref codes, addCode: affectWeight, errorMessage: "Couldn't find item weight");
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
            MethodInfo additionalSprintTime = typeof(BiggerLungs).GetMethod(nameof(BiggerLungs.GetAdditionalStaminaTime));
            MethodInfo backMusclesStaminaWeight = typeof(BackMuscles).GetMethod(nameof(BackMuscles.DecreaseStrain));

            FieldInfo sprintTime = typeof(PlayerControllerB).GetField(nameof(PlayerControllerB.sprintTime));
            FieldInfo carryWeight = typeof(PlayerControllerB).GetField(nameof(PlayerControllerB.carryWeight));

            List<CodeInstruction> codes = new(instructions);
            int index = 0;
            Tools.FindField(ref index, ref codes, findField: sprintTime, addCode: additionalSprintTime, errorMessage: "Couldn't find the first occurence of sprintTime field");
            Tools.FindField(ref index, ref codes, findField: carryWeight, addCode: backMusclesStaminaWeight, errorMessage: "Couldn't find the occurence of carryWeight field");
            Tools.FindField(ref index, ref codes, findField: sprintTime, addCode: additionalSprintTime, errorMessage: "Couldn't find the second occurence of sprintTime field");
            Tools.FindField(ref index, ref codes, findField: sprintTime, addCode: additionalSprintTime, errorMessage: "Couldn't find the third occurence of sprintTime field");
            Tools.FindMul(ref index, ref codes, addCode: biggerLungsRegenMethod, errorMessage: "Couldn't find first mul instruction to include our regen method from Bigger Lungs");
            codes.Insert(index + 1, new CodeInstruction(OpCodes.Call, sickBeatsRegenMethod));
            Tools.FindField(ref index, ref codes, findField: sprintTime, addCode: additionalSprintTime, errorMessage: "Couldn't find the fourth occurence of sprintTime field");
            Tools.FindMul(ref index, ref codes, addCode: biggerLungsRegenMethod, errorMessage: "Couldn't find second mul instruction to include our regen method from Bigger Lungs");
            codes.Insert(index + 1, new CodeInstruction(OpCodes.Call, sickBeatsRegenMethod));
            return codes;
        }

        [HarmonyTranspiler]
        [HarmonyPatch(nameof(PlayerControllerB.Jump_performed))]
        static IEnumerable<CodeInstruction> JumpPerformedTranspiler(IEnumerable<CodeInstruction> instructions)
        {
            MethodInfo biggerLungsReduceJumpCost = typeof(BiggerLungs).GetMethod(nameof(BiggerLungs.ApplyPossibleReducedJumpStaminaCost));
            List<CodeInstruction> codes = new(instructions);
            int index = 0;
            Tools.FindFloat(ref index, ref codes, findValue: 0.08f, addCode: biggerLungsReduceJumpCost, errorMessage: "Couldn't find jump stamina cost");
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
            List<CodeInstruction> codes = new(instructions);
            int index = 0;
            Tools.FindFloat(ref index, ref codes, findValue: 22, addCode: runningShoesReduceNoiseRange, errorMessage: "Couldn't find footstep noise");
            Tools.FindFloat(ref index, ref codes, findValue: 17, addCode: runningShoesReduceNoiseRange, errorMessage: "Couldn't find footstep noise");
            return codes.AsEnumerable();
        }

        [HarmonyPatch(nameof(PlayerControllerB.PlayerLookInput))]
        [HarmonyTranspiler]
        static IEnumerable<CodeInstruction> PlayerLookInputTranspiler(IEnumerable<CodeInstruction> instructions)
        {
            MethodInfo reduceLookSensitivity = typeof(WheelbarrowScript).GetMethod(nameof(WheelbarrowScript.CheckIfPlayerCarryingWheelbarrowLookSensitivity));
            List<CodeInstruction> codes = new(instructions);
            int index = 0;
            Tools.FindFloat(ref index, ref codes, findValue: 0.008f, addCode: reduceLookSensitivity, errorMessage: "Couldn't find look sensitivity value we wanted to influence");
            return codes;
        }

        [HarmonyPatch(nameof(PlayerControllerB.PlayerJump), MethodType.Enumerator)]
        [HarmonyTranspiler]
        static IEnumerable<CodeInstruction> PlayerJumpTranspiler(IEnumerable<CodeInstruction> instructions)
        {
            MethodInfo additionalJumpForce = typeof(StrongLegs).GetMethod(nameof(StrongLegs.GetAdditionalJumpForce));
            FieldInfo jumpForce = typeof(PlayerControllerB).GetField(nameof(PlayerControllerB.jumpForce));

            List<CodeInstruction> codes = new(instructions);
            int index = 0;

            Tools.FindField(ref index, ref codes, findField: jumpForce, addCode: additionalJumpForce, errorMessage: "Couldn't find first occurence of jump force field");
            Tools.FindField(ref index, ref codes, findField: jumpForce, addCode: additionalJumpForce, errorMessage: "Couldn't find second occurence of jump force field");
            return codes;
        }

        [HarmonyPatch(nameof(PlayerControllerB.Update))] // We're all going to die
        [HarmonyTranspiler] // Just kidding
        static IEnumerable<CodeInstruction> UpdateTranspiler(IEnumerable<CodeInstruction> instructions)
        {
            MethodInfo reduceCarryLoss = typeof(BackMuscles).GetMethod(nameof(BackMuscles.DecreaseCarryLoss));
            MethodInfo reduceMovement = typeof(WheelbarrowScript).GetMethod(nameof(WheelbarrowScript.CheckIfPlayerCarryingWheelbarrowMovement));
            MethodInfo additionalMovement = typeof(RunningShoes).GetMethod(nameof(RunningShoes.GetAdditionalMovementSpeed));
            MethodInfo additionalClimbSpeed = typeof(ClimbingGloves).GetMethod(nameof(ClimbingGloves.GetAdditionalClimbingSpeed));
            MethodInfo additionalJumpForce = typeof(StrongLegs).GetMethod(nameof(StrongLegs.GetAdditionalJumpForce));

            FieldInfo carryWeight = typeof(PlayerControllerB).GetField(nameof(PlayerControllerB.carryWeight));
            FieldInfo movementSpeed = typeof(PlayerControllerB).GetField(nameof(PlayerControllerB.movementSpeed));
            FieldInfo climbSpeed = typeof(PlayerControllerB).GetField(nameof(PlayerControllerB.climbSpeed));
            FieldInfo jumpForce = typeof(PlayerControllerB).GetField(nameof(PlayerControllerB.jumpForce));

            List<CodeInstruction> codes = new(instructions);
            int index = 0;
            Tools.FindField(ref index, ref codes, findField: movementSpeed, addCode: additionalMovement, errorMessage: "Couldn't find first occurence of movement speed field");
            Tools.FindField(ref index, ref codes, findField: movementSpeed, addCode: additionalMovement, errorMessage: "Couldn't find second occurence of movement speed field");
            Tools.FindField(ref index, ref codes, findField: carryWeight, addCode: reduceCarryLoss, errorMessage: "Couldn't find first carryWeight occurence");
            Tools.FindField(ref index, ref codes, findField: carryWeight, addCode: reduceMovement, errorMessage: "Couldn't find second carryWeight occurence");
            Tools.FindField(ref index, ref codes, findField: carryWeight, addCode: reduceMovement, errorMessage: "Couldn't find third carryWeight occurence");
            Tools.FindField(ref index, ref codes, findField: jumpForce, addCode: additionalJumpForce, errorMessage: "Couldn't find occurence of jump force field");
            Tools.FindField(ref index, ref codes, findField: climbSpeed, addCode: additionalClimbSpeed, errorMessage: "Couldn't find occurence of climb speed field");
            return codes;
        }

        [HarmonyPatch(nameof(PlayerControllerB.Crouch_performed))]
        [HarmonyTranspiler]
        static IEnumerable<CodeInstruction> CrouchPerformmedTranspiler(IEnumerable<CodeInstruction> instructions)
        {
            MethodInfo carryingWheelbarrow = typeof(WheelbarrowScript).GetMethod(nameof(WheelbarrowScript.CheckIfPlayerCarryingWheelbarrow));
            FieldInfo isMenuOpen = typeof(QuickMenuManager).GetField(nameof(QuickMenuManager.isMenuOpen));
            List<CodeInstruction> codes = new(instructions);
            int index = 0;
            Tools.FindField(ref index, ref codes, findField: isMenuOpen, addCode: carryingWheelbarrow, orInstruction: true, errorMessage: "Couldn't find isMenuOpen field");
            return codes;
        }

        [HarmonyPatch(nameof(PlayerControllerB.BeginGrabObject))]
        [HarmonyPatch(nameof(PlayerControllerB.SetHoverTipAndCurrentInteractTrigger))]
        [HarmonyTranspiler]
        static IEnumerable<CodeInstruction> SetGrabDistanceTranspiler(IEnumerable<CodeInstruction> instructions)
        {
            FieldInfo grabDistance = typeof(PlayerControllerB).GetField(nameof(PlayerControllerB.grabDistance));
            MethodInfo getIncreasedRange = typeof(MechanicalArms).GetMethod(nameof(MechanicalArms.GetIncreasedGrabDistance));
            List<CodeInstruction> codes = new(instructions);
            int index = 0;
            Tools.FindField(ref index, ref codes, findField: grabDistance, addCode: getIncreasedRange, errorMessage: "Couldn't find the grab distance field");
            return codes;
        }
    }
}
