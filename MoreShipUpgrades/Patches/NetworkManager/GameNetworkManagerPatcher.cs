using HarmonyLib;
using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc;
using MoreShipUpgrades.Misc.Upgrades;
using UnityEngine;

namespace MoreShipUpgrades.Patches.NetworkManager
{
    [HarmonyPatch(typeof(GameNetworkManager))]
    internal static class GameNetworkManagerPatcher
    {
        static LguLogger logger = new LguLogger(nameof(GameNetworkManagerPatcher));
        internal static int originalVersion = -1;
        [HarmonyPostfix]
        [HarmonyPatch(nameof(GameNetworkManager.Disconnect))]
        static void ResetUpgradeBus(GameNetworkManager __instance)
        {
            logger.LogDebug("Resetting the Upgrade Bus due to disconnecting...");
            UpgradeBus.Instance.ResetAllValues();
            BaseUpgrade[] upgradeObjects = Object.FindObjectsOfType<BaseUpgrade>();
            foreach (BaseUpgrade upgrade in upgradeObjects)
            {
                Object.Destroy(upgrade.gameObject);
            }
        }

        [HarmonyPostfix]
        [HarmonyPatch(nameof(GameNetworkManager.Awake))]
        static void AwakePrefix(GameNetworkManager __instance)
        {
            originalVersion = __instance.gameVersionNum;
        }
    }
}
