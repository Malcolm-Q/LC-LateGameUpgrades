using GameNetcodeStuff;
using HarmonyLib;
using MoreShipUpgrades.Managers;
using Unity.Netcode;

namespace MoreShipUpgrades.Patches.Interactables
{
    [HarmonyPatch(typeof(StartMatchLever))]
    internal static class StartMatchLevelPatcher
    {
        [HarmonyPostfix]
        [HarmonyPatch(nameof(StartMatchLever.StartGame))]
        static void SyncHelmets()
        {
            if (!(UpgradeBus.Instance.wearingHelmet && UpgradeBus.Instance.helmetDesync)) return;
            PlayerControllerB localPlayer = UpgradeBus.Instance.GetLocalPlayer();
            LguStore.Instance.ReqSpawnAndMoveHelmetServerRpc(new NetworkObjectReference(localPlayer.gameObject), localPlayer.playerClientId);
        }
    }

}
