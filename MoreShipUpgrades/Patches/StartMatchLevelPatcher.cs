using HarmonyLib;
using MoreShipUpgrades.Managers;
using Unity.Netcode;

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

        [HarmonyPostfix]
        [HarmonyPatch("StartGame")]
        static void SyncHelmets()
        {
            if(UpgradeBus.instance.wearingHelmet && UpgradeBus.instance.helmetDesync)
            {
                LGUStore.instance.ReqSpawnAndMoveHelmetServerRpc(new NetworkObjectReference(GameNetworkManager.Instance.localPlayerController.gameObject), GameNetworkManager.Instance.localPlayerController.playerClientId);
            }
        }
    }

}
