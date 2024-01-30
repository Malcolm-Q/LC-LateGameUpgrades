using HarmonyLib;
using MoreShipUpgrades.Managers;

namespace MoreShipUpgrades.Patches
{
    [HarmonyPatch(typeof(TimeOfDay))]
    internal class TimeOfDayPatcher
    {
        [HarmonyPostfix]
        [HarmonyPatch(nameof(TimeOfDay.SyncNewProfitQuotaClientRpc))]
        private static void GenerateNewSales(TimeOfDay __instance)
        {
            if(UpgradeBus.instance.cfg.SHARED_UPGRADES && (__instance.IsHost || __instance.IsServer))
            {
                int seed = UnityEngine.Random.Range(0, 999999);
                LGUStore.instance.GenerateSalesClientRpc(seed);
            }
            else
            {
                UpgradeBus.instance.GenerateSales();
            }
        }
    }

}
