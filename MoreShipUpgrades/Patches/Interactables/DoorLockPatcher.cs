using CustomItemBehaviourLibrary.Misc;
using HarmonyLib;
using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc.Upgrades;
using MoreShipUpgrades.UpgradeComponents.OneTimeUpgrades.Player;
using MoreShipUpgrades.UpgradeComponents.TierUpgrades.Items;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace MoreShipUpgrades.Patches.Interactables
{
    [HarmonyPatch(typeof(DoorLock))]
    internal static class DoorLockPatcher
    {
        [HarmonyPatch(nameof(DoorLock.Update))]
        [HarmonyPostfix]
        static void UpdatePostfix(DoorLock __instance)
        {
            if (!UpgradeBus.Instance.PluginConfiguration.LocksmithConfiguration.Enabled) return;
            if (!BaseUpgrade.GetActiveUpgrade(LockSmith.UPGRADE_NAME)) return;
            if (__instance.isLocked && !__instance.isPickingLock)
            {
                __instance.doorTrigger.hoverTip = "Lockpick: [LMB]";
                __instance.doorTrigger.interactable = true;
            }
            else
            {
                __instance.doorTrigger.hoverTip = "Use door : [LMB]";
            }
        }
        [HarmonyPatch(nameof(DoorLock.Update))]
        [HarmonyTranspiler]
        static IEnumerable<CodeInstruction> UpdateTranspiler(IEnumerable<CodeInstruction> instructions)
        {
            List<CodeInstruction> codes = new(instructions);
            int index = 0;

            MethodInfo IncreaseLockpickEfficiency = typeof(SmarterLockpick).GetMethod(nameof(SmarterLockpick.IncreaseLockpickEfficiency));
            MethodInfo deltaTime = typeof(Time).GetProperty(nameof(Time.deltaTime)).GetGetMethod();

			Tools.FindMethod(ref index, ref codes, findMethod: deltaTime, skip: true, errorMessage: "DeltaTime not found in DoorLock script");
			Tools.FindMethod(ref index, ref codes, findMethod: deltaTime, addCode: IncreaseLockpickEfficiency, errorMessage: "DeltaTime not found in DoorLock script");

            return codes;
		}

        [HarmonyPatch(nameof(DoorLock.OnHoldInteract))]
        [HarmonyPrefix]
        static bool OnHoldInteractPrefix(DoorLock __instance)
        {
            if (!BaseUpgrade.GetActiveUpgrade(LockSmith.UPGRADE_NAME)) return true;
            if (!__instance.isLocked) return true;
            if (LockSmith.instance.gameObject.transform.GetChild(0).gameObject.activeInHierarchy) return true;

            LockSmith.instance.BeginLockPick(__instance);
            return false;
        }
    }
}
