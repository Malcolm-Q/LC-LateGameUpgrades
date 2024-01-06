using HarmonyLib;
using MoreShipUpgrades.Managers;

namespace MoreShipUpgrades.Patches
{
    [HarmonyPatch(typeof(StartMatchLever))]
    internal class StartMatchLevelPatcher
    {
        [HarmonyPostfix]
        [HarmonyPatch("EndGame")]
        private static void ResetContract(StartMatchLever __instance)
        {
            if(UpgradeBus.instance.contractLevel == RoundManager.Instance.currentLevel.PlanetName)
            {
                if(__instance.IsHost)
                {
                    LGUStore.instance.SyncContractDetailsClientRpc("None", "None");
                }
                else
                {
                    LGUStore.instance.ReqSyncContractDetailsServerRpc("None", "None");
                }
            }
        }
    }

}
