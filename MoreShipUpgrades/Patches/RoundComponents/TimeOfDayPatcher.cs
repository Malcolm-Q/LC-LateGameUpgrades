using HarmonyLib;
using MoreShipUpgrades.Managers;

namespace MoreShipUpgrades.Patches.RoundComponents
{
    [HarmonyPatch(typeof(TimeOfDay))]
    internal static class TimeOfDayPatcher
    {
        [HarmonyPostfix]
        [HarmonyPatch(nameof(TimeOfDay.SyncNewProfitQuotaClientRpc))]
        static void GenerateNewSales(TimeOfDay __instance)
        {
            if (UpgradeBus.Instance.PluginConfiguration.SHARED_UPGRADES.Value && (__instance.IsHost || __instance.IsServer))
            {
                int seed = UnityEngine.Random.Range(0, 999999);
                LguStore.Instance.GenerateSalesClientRpc(seed);
            }
            else
            {
                UpgradeBus.Instance.GenerateSales();
            }
        }
    }

}
