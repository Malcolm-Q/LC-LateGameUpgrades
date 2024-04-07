﻿using HarmonyLib;
using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc.Upgrades;
using MoreShipUpgrades.UpgradeComponents.OneTimeUpgrades;
using Unity.Netcode;
using UnityEngine;

namespace MoreShipUpgrades.Patches.TerminalComponents
{
    [HarmonyPatch(typeof(TerminalAccessibleObject))]
    internal static class TerminalAccessibleObjectPatcher
    {
        [HarmonyPrefix]
        [HarmonyPatch(nameof(TerminalAccessibleObject.CallFunctionFromTerminal))]
        static bool DestroyObject(ref TerminalAccessibleObject __instance, ref float ___codeAccessCooldownTimer, ref bool ___inCooldown)
        {
            if (!BaseUpgrade.GetActiveUpgrade(MalwareBroadcaster.UPGRADE_NAME)) { return true; }
            if (!MalwareBroadcaster.IsMapHazard(ref __instance)) return true;
            if (UpgradeBus.Instance.PluginConfiguration.DESTROY_TRAP.Value)
            {
                MalwareBroadcaster.instance.ReqDestroyObjectServerRpc(new NetworkObjectReference(__instance.gameObject.GetComponentInParent<NetworkObject>()));
                return false;
            }
            if (!___inCooldown)
            {
                ___codeAccessCooldownTimer = UpgradeBus.Instance.PluginConfiguration.DISARM_TIME.Value;
            }
            return true;
        }
    }
}
