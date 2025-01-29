using com.github.zehsteam.ToilHead.MonoBehaviours;
using HarmonyLib;
using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc.Upgrades;
using MoreShipUpgrades.UpgradeComponents.OneTimeUpgrades;
using Unity.Netcode;

namespace MoreShipUpgrades.Compat
{
    public static class ToilheadCompat
    {
        public static bool Enabled =>
            BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey(com.github.zehsteam.ToilHead.MyPluginInfo.PLUGIN_GUID);

        [HarmonyPatch(typeof(FollowTerminalAccessibleObjectBehaviour))]
        internal static class FollowTerminalAccessibleObjectBehaviourPatcher
        {
            [HarmonyPrefix]
            [HarmonyPatch(nameof(FollowTerminalAccessibleObjectBehaviour.CallFunctionFromTerminal))]
            static bool DestroyObject(ref FollowTerminalAccessibleObjectBehaviour __instance)
            {
                if (!BaseUpgrade.GetActiveUpgrade(MalwareBroadcaster.UPGRADE_NAME)) { return true; }
                if (!MalwareBroadcaster.IsMapHazard(ref __instance)) return true;
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
