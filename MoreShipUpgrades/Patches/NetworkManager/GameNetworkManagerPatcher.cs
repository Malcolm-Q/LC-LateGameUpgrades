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
        [HarmonyPostfix]
        [HarmonyPatch(nameof(GameNetworkManager.Disconnect))]
        static void ResetUpgradeBus(GameNetworkManager __instance)
        {
            if (__instance.isDisconnecting || StartOfRound.Instance == null) return;
            logger.LogDebug("Resetting the Upgrade Bus due to disconnecting...");
            if (__instance.isHostingGame)
            {
                BaseUpgrade[] upgradeObjects = Object.FindObjectsOfType<BaseUpgrade>();
                foreach (BaseUpgrade upgrade in upgradeObjects)
                {
                    Object.Destroy(upgrade.gameObject);
                }
            }
            UpgradeBus.Instance.ResetAllValues();
        }
    }
}
