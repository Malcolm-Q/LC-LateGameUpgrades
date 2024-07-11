using HarmonyLib;
using MoreShipUpgrades.Misc.Upgrades;
using MoreShipUpgrades.UpgradeComponents.OneTimeUpgrades.Player;

namespace MoreShipUpgrades.Patches.Interactables
{
    [HarmonyPatch(typeof(DoorLock))]
    internal static class DoorLockPatcher
    {
        [HarmonyPatch(nameof(DoorLock.Update))]
        [HarmonyPostfix]
        static void UpdatePostfix(DoorLock __instance)
        {
            if (__instance.isLocked && !__instance.isPickingLock && BaseUpgrade.GetActiveUpgrade(LockSmith.UPGRADE_NAME))
            {
                __instance.doorTrigger.hoverTip = "Lockpick: [LMB]";
                __instance.doorTrigger.interactable = true;
            }
            else
            {
                __instance.doorTrigger.hoverTip = "Use door : [LMB]";
            }
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
