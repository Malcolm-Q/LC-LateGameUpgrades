using GameNetcodeStuff;
using HarmonyLib;
using MoreShipUpgrades.Managers;
using Unity.Netcode;
using UnityEngine;

namespace MoreShipUpgrades.Patches
{
    [HarmonyPatch(typeof(InteractTrigger))]
    internal class DoorLockPatcher
    {
        [HarmonyPrefix]
        [HarmonyPatch("OnTriggerEnter")]
        private static bool pickDoor(InteractTrigger __instance, Collider other)
        {
            if(!UpgradeBus.instance.lockSmith) { return true; }
            if (!other.gameObject.GetComponent<PlayerControllerB>().IsOwner) { return true; }
            if(__instance.gameObject.GetComponent<DoorLock>() == null) { return true; }
            if(!__instance.gameObject.GetComponent<DoorLock>().isLocked) { return true; }
            if(UpgradeBus.instance.lockScript.gameObject.transform.GetChild(0).gameObject.activeInHierarchy)
            {
                return true;
            }
            UpgradeBus.instance.lockScript.currentDoor = __instance.gameObject.GetComponent<DoorLock>();
            UpgradeBus.instance.lockScript.BeginLockPick();
            return false;
        }
    }
}
