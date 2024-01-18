using HarmonyLib;
using MoreShipUpgrades.Managers;
using Unity.Netcode;

namespace MoreShipUpgrades.Patches
{
    [HarmonyPatch(typeof(StartMatchLever))]
    internal class StartMatchLevelPatcher
    {
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
