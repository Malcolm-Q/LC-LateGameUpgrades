using HarmonyLib;
using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc;
using UnityEngine;

namespace MoreShipUpgrades.Patches
{
    [HarmonyPatch(typeof(GameNetworkManager))]
    internal class GameNetworkManagerPatcher
    {
        private static LGULogger logger = new LGULogger(nameof(GameNetworkManagerPatcher));
        [HarmonyPostfix]
        [HarmonyPatch(nameof(GameNetworkManager.Disconnect))]
        private static void ResetUpgradeBus()
        {
            logger.LogDebug("Resetting the Upgrade Bus due to disconnecting...");
            BaseUpgrade[] upgradeObjects = Object.FindObjectsOfType<BaseUpgrade>();
            foreach (BaseUpgrade upgrade in upgradeObjects)
            {
                Object.Destroy(upgrade.gameObject);
            }           
            UpgradeBus.instance.ResetAllValues();
        }

        [HarmonyPrefix]
        [HarmonyPatch(nameof(GameNetworkManager.SaveGame))]
        private static void saveLGU(GameNetworkManager __instance)
        {
            if (!__instance.isHostingGame) return;
            logger.LogDebug("Saving the LGU upgrades unto a json file...");
            LGUStore.instance.ServerSaveFileServerRpc();
        }
    }
}
