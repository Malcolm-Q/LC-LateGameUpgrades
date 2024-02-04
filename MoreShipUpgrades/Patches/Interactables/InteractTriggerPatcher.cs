using GameNetcodeStuff;
using HarmonyLib;
using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc;
using UnityEngine;

namespace MoreShipUpgrades.Patches.Interactables
{
    [HarmonyPatch(typeof(InteractTrigger))]
    internal class InteractTriggerPatcher
    {
        private static LGULogger logger = new LGULogger(nameof(InteractTriggerPatcher));
        [HarmonyPrefix]
        [HarmonyPatch(nameof(InteractTrigger.OnTriggerEnter))]
        private static bool pickDoor(InteractTrigger __instance, Collider other)
        {
            if (!UpgradeBus.instance.lockSmith) { return true; }
            PlayerControllerB player = other.gameObject.GetComponent<PlayerControllerB>();
            if (player == null) { return true; }
            if (!player.IsOwner) { return true; }
            DoorLock door = __instance.gameObject.GetComponent<DoorLock>();
            if (door == null) { return true; }
            if (!door.isLocked) { return true; }
            if (UpgradeBus.instance.lockScript.gameObject.transform.GetChild(0).gameObject.activeInHierarchy) return true;


            logger.LogDebug("Starting lockpicking minigame...");
            UpgradeBus.instance.lockScript.currentDoor = door;
            UpgradeBus.instance.lockScript.BeginLockPick();
            UpgradeBus.instance.lockScript.timesStruck = 0;
            return false;
        }
    }
}
