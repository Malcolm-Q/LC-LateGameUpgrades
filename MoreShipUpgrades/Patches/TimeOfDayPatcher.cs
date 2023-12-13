using HarmonyLib;
using MoreShipUpgrades.Managers;

namespace MoreShipUpgrades.Patches
{
    [HarmonyPatch(typeof(TimeOfDay))]
    internal class TimeOfDayPatcher
    {
        [HarmonyPostfix]
        [HarmonyPatch("SyncNewProfitQuotaClientRpc")]
        private static void GenerateNewSales(DeleteFileButton __instance)
        {
            UpgradeBus.instance.GenerateSales();
        }
    }

}
