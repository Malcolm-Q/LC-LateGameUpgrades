using GameNetcodeStuff;
using HarmonyLib;
using MoreShipUpgrades.Misc;
using MoreShipUpgrades.Misc.Upgrades;
using MoreShipUpgrades.UpgradeComponents.OneTimeUpgrades.Player;
using UnityEngine;

namespace MoreShipUpgrades.Patches.Interactables
{
    [HarmonyPatch(typeof(InteractTrigger))]
    internal static class InteractTriggerPatcher
    {
        [HarmonyPrefix]
        [HarmonyPatch(nameof(InteractTrigger.OnTriggerEnter))]
        private static bool OnTriggerEnterPrefix(InteractTrigger __instance, Collider other)
        {
            if (!BaseUpgrade.GetActiveUpgrade(LockSmith.UPGRADE_NAME)) { return true; }
            PlayerControllerB player = other.gameObject.GetComponent<PlayerControllerB>();
            if (player == null) { return true; }
            if (!player.IsOwner) { return true; }
            DoorLock door = __instance.gameObject.GetComponent<DoorLock>();
            if (door == null) { return true; }
            if (!door.isLocked) { return true; }
            if (LockSmith.instance.gameObject.transform.GetChild(0).gameObject.activeInHierarchy) return true;

            LockSmith.instance.BeginLockPick(door);
            return false;
        }
    }
}
