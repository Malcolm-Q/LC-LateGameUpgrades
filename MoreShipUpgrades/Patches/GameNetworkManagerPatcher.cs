using HarmonyLib;
using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc;
using UnityEngine;

namespace MoreShipUpgrades.Patches
{
    [HarmonyPatch(typeof(GameNetworkManager))]
    internal class GameNetworkManagerPatcher
    {
        [HarmonyPostfix]
        [HarmonyPatch("Disconnect")]
        private static void ResetUpgradeBus()
        {
            BaseUpgrade[] upgradeObjects = Object.FindObjectsOfType<BaseUpgrade>();
            foreach (BaseUpgrade upgrade in upgradeObjects)
            {
                Object.Destroy(upgrade.gameObject);
            }           
            UpgradeBus.instance.ResetAllValues();
        }

        [HarmonyPrefix]
        [HarmonyPatch("SaveGame")]
        private static void saveLGU(GameNetworkManager __instance)
        {
            if (!__instance.isHostingGame) return;
            LGUStore.instance.ServerSaveFileServerRpc();
        }
    }
}
