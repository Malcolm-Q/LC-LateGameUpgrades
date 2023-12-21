using GameNetcodeStuff;
using HarmonyLib;
using MoreShipUpgrades.Managers;
using MoreShipUpgrades.UpgradeComponents;
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
            foreach(GameObject sample in UpgradeBus.instance.samplePrefabs.Values)
            {
                Item item = sample.GetComponent<PhysicsProp>().itemProperties;
                if(StartOfRound.Instance.allItemsList.itemsList.Contains(item))
                {
                    StartOfRound.Instance.allItemsList.itemsList.Add(item);
                }
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

        [HarmonyPostfix]
        [HarmonyPatch("OpenShipDoors")]
        private static void UpdatePlayersHealth(StartOfRound __instance)
        {
            playerHealthScript.CheckAdditionalHealth(__instance);
        }
    }
}
