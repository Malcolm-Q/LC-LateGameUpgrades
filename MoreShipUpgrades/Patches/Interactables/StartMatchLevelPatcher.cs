using GameNetcodeStuff;
using HarmonyLib;
using MoreShipUpgrades.Managers;
using Unity.Netcode;

namespace MoreShipUpgrades.Patches.Interactables
{
    [HarmonyPatch(typeof(StartMatchLever))]
    internal class StartMatchLevelPatcher
    {
        [HarmonyPostfix]
        [HarmonyPatch(nameof(StartMatchLever.StartGame))]
        static void SyncHelmets()
        {
            if (!(UpgradeBus.instance.wearingHelmet && UpgradeBus.instance.helmetDesync)) return;
            PlayerControllerB localPlayer = UpgradeBus.instance.GetLocalPlayer();
            LGUStore.instance.ReqSpawnAndMoveHelmetServerRpc(new NetworkObjectReference(localPlayer.gameObject), localPlayer.playerClientId);
        }
    }

}
