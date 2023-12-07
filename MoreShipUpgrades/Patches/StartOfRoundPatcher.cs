using GameNetcodeStuff;
using HarmonyLib;
using MoreShipUpgrades.Managers;
using Unity.Netcode;
using UnityEngine;

namespace MoreShipUpgrades.Patches
{
    [HarmonyPatch(typeof(StartOfRound))]
    internal class StartOfRoundPatcher
    {
        [HarmonyPrefix]
        [HarmonyPatch("Start")]
        private static void InitLGUStore(PlayerControllerB __instance)
        {
            if(__instance.NetworkManager.IsHost || __instance.NetworkManager.IsServer)
            {
                GameObject refStore = GameObject.Instantiate(UpgradeBus.instance.modStorePrefab);
                refStore.GetComponent<NetworkObject>().Spawn();
            }
        }
        [HarmonyPrefix]
        [HarmonyPatch("playersFiredGameOver")]
        private static void GameOverResetUpgradeManager(StartOfRound __instance)
        {
            if(__instance.NetworkManager.IsHost ||  __instance.NetworkManager.IsServer)
            {
                LGUStore.instance.PlayersFiredServerRpc();
            }
        }
    }
}
