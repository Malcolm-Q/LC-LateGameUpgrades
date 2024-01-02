using GameNetcodeStuff;
using HarmonyLib;
using MoreShipUpgrades.Managers;
using Unity.Netcode;
using UnityEngine;

namespace MoreShipUpgrades.Patches
{
    [HarmonyPatch(typeof(InteractTrigger))]
    internal class InteractTriggerPatcher
    {
        [HarmonyPrefix]
        [HarmonyPatch("OnTriggerEnter")]
        private static bool pickDoor(InteractTrigger __instance, Collider other)
        {
            if(!UpgradeBus.instance.lockSmith) { return true; }
            PlayerControllerB player = other.gameObject.GetComponent<PlayerControllerB>();
            if(player == null) { return true; }
            if (!player.IsOwner) { return true; }
            DoorLock door = __instance.gameObject.GetComponent<DoorLock>();
            if(door == null) { return true; }
            if(!door.isLocked) { return true; }
            if(UpgradeBus.instance.lockScript.gameObject.transform.GetChild(0).gameObject.activeInHierarchy) return true;

            UpgradeBus.instance.lockScript.currentDoor = door;
            UpgradeBus.instance.lockScript.BeginLockPick();
            UpgradeBus.instance.lockScript.timesStruck = 0;
            return false;
        }
    }
}
