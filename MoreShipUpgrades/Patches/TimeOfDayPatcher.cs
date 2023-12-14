using HarmonyLib;
using MoreShipUpgrades.Managers;

namespace MoreShipUpgrades.Patches
{
    [HarmonyPatch(typeof(TimeOfDay))]
    internal class TimeOfDayPatcher
    {
        [HarmonyPostfix]
        [HarmonyPatch("SyncNewProfitQuotaClientRpc")]
        private static void GenerateNewSales(TimeOfDay __instance)
        {
            if(__instance.IsHost || __instance.IsServer)
            {
                UpgradeBus.instance.GenerateSales();
            }
        }
    }

}
