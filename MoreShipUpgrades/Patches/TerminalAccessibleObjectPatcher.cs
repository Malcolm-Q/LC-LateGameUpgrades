using HarmonyLib;
using MoreShipUpgrades.Managers;
using Unity.Netcode;
using UnityEngine;

namespace MoreShipUpgrades.Patches
{
    [HarmonyPatch(typeof(TerminalAccessibleObject))]
    internal class TerminalAccessibleObjectPatcher
    {
        [HarmonyPrefix]
        [HarmonyPatch(nameof(TerminalAccessibleObject.CallFunctionFromTerminal))]
        private static bool DestroyObject(ref TerminalAccessibleObject __instance, ref float ___codeAccessCooldownTimer, ref bool ___inCooldown)
        {
            if(!UpgradeBus.instance.DestroyTraps || __instance.gameObject.layer != LayerMask.NameToLayer("MapHazards")) { return true; }
            if (UpgradeBus.instance.cfg.DESTROY_TRAP)
            {
                UpgradeBus.instance.trapHandler.ReqDestroyObjectServerRpc(new NetworkObjectReference(__instance.gameObject.transform.parent.gameObject.GetComponent<NetworkObject>()));
                return false;
            }
            if (!___inCooldown)
            {
                ___codeAccessCooldownTimer = UpgradeBus.instance.cfg.DISARM_TIME;
            }
            return true;
        }
    }
}
