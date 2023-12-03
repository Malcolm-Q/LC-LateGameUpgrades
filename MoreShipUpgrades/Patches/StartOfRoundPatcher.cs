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
            UpgradeBus.instance.ResetAllValues();
            if(__instance.NetworkManager.IsHost ||  __instance.NetworkManager.IsServer)
            {
                LGUStore.instance.PlayersFired();
            }
            PlayerControllerB[] players = GameObject.FindObjectsOfType<PlayerControllerB>();
            foreach (PlayerControllerB player in players)
            {
                player.movementSpeed = 4.6f;
                player.sprintTime = 11;
                player.jumpForce = 13;
            }
        }
    }
}
