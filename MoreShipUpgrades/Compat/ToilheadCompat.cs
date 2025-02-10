using com.github.zehsteam.ToilHead;
using com.github.zehsteam.ToilHead.MonoBehaviours;
using HarmonyLib;
using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc.Upgrades;
using MoreShipUpgrades.UpgradeComponents.OneTimeUpgrades;
using Unity.Netcode;
using UnityEngine;

namespace MoreShipUpgrades.Compat
{
    internal static class ToilheadCompat
    {
        public static bool Enabled =>
            BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey(MyPluginInfo.PLUGIN_GUID);

        [HarmonyPatch(typeof(FollowTerminalAccessibleObjectBehaviour))]
        internal static class FollowTerminalAccessibleObjectBehaviourPatcher
        {
            [HarmonyPrefix]
            [HarmonyPatch(nameof(FollowTerminalAccessibleObjectBehaviour.CallFunctionFromTerminal))]
            internal static bool DestroyObject(ref FollowTerminalAccessibleObjectBehaviour __instance)
            {
                if (!BaseUpgrade.GetActiveUpgrade(MalwareBroadcaster.UPGRADE_NAME)) { return true; }
                if (__instance.gameObject.layer != LayerMask.NameToLayer("MapHazards") &&
                    (__instance.transform.parent == null || __instance.transform.parent.gameObject.layer != LayerMask.NameToLayer("MapHazards"))) return true;
                if (UpgradeBus.Instance.PluginConfiguration.MalwareBroadcasterUpgradeConfiguration.DestroyTraps.Value)
                {
                    MalwareBroadcaster.instance.ReqDestroyObjectServerRpc(new NetworkObjectReference(__instance.gameObject.GetComponentInParent<NetworkObject>()));
                    return false;
                }
                if (!__instance.inCooldown)
                {
                    __instance.codeAccessCooldownTimer = UpgradeBus.Instance.PluginConfiguration.MalwareBroadcasterUpgradeConfiguration.DisarmTime.Value;
                }
                return true;
            }
        }
    }
}
