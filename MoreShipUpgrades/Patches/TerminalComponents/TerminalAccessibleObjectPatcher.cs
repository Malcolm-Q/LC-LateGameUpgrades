using HarmonyLib;
using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc.Upgrades;
using MoreShipUpgrades.UpgradeComponents.OneTimeUpgrades;
using Unity.Netcode;
using UnityEngine;

namespace MoreShipUpgrades.Patches.TerminalComponents
{
    [HarmonyPatch(typeof(TerminalAccessibleObject))]
    internal class TerminalAccessibleObjectPatcher
    {
        [HarmonyPrefix]
        [HarmonyPatch(nameof(TerminalAccessibleObject.CallFunctionFromTerminal))]
        private static bool DestroyObject(ref TerminalAccessibleObject __instance, ref float ___codeAccessCooldownTimer, ref bool ___inCooldown)
        {
            if (!BaseUpgrade.GetActiveUpgrade(MalwareBroadcaster.UPGRADE_NAME) || __instance.gameObject.layer != LayerMask.NameToLayer("MapHazards")) { return true; }
            if (UpgradeBus.instance.cfg.DESTROY_TRAP.Value)
            {
                MalwareBroadcaster.instance.ReqDestroyObjectServerRpc(new NetworkObjectReference(__instance.gameObject.transform.parent.gameObject.GetComponent<NetworkObject>()));
                return false;
            }
            if (!___inCooldown)
            {
                ___codeAccessCooldownTimer = UpgradeBus.instance.cfg.DISARM_TIME.Value;
            }
            return true;
        }
    }
}
