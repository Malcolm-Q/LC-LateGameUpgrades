using HarmonyLib;
using MoreShipUpgrades.Managers;

namespace MoreShipUpgrades.Patches
{
    [HarmonyPatch(typeof(GameNetworkManager))]
    internal class GameNetworkManagerPatcher
    {
        [HarmonyPostfix]
        [HarmonyPatch("Disconnect")]
        private static void ResetUpgradeBus()
        {
            UpgradeBus.instance.ResetAllValues();
        }

        [HarmonyPrefix]
        [HarmonyPatch("SaveGame")]
        private static void saveLGU(GameNetworkManager __instance)
        {
            if(__instance.isHostingGame)
            {
                LGUStore.instance.ServerSaveFileServerRpc();
            }
        }
    }
}
