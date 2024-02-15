using GameNetcodeStuff;
using HarmonyLib;
using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc;
using MoreShipUpgrades.Misc.Upgrades;
using MoreShipUpgrades.UpgradeComponents.OneTimeUpgrades;
using MoreShipUpgrades.UpgradeComponents.TierUpgrades.AttributeUpgrades;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;

namespace MoreShipUpgrades.Patches.RoundComponents
{
    [HarmonyPatch(typeof(StartOfRound))]
    internal static class StartOfRoundPatcher
    {
        private static LguLogger logger = new LguLogger(nameof(StartOfRoundPatcher));
        [HarmonyPrefix]
        [HarmonyPatch(nameof(StartOfRound.Start))]
        private static void InitLguStore(PlayerControllerB __instance)
        {
            logger.LogDebug("Initiating components...");
            if (__instance.NetworkManager.IsHost || __instance.NetworkManager.IsServer)
            {
                GameObject refStore = Object.Instantiate(UpgradeBus.Instance.modStorePrefab);
                refStore.GetComponent<NetworkObject>().Spawn();
                logger.LogDebug("LguStore component initiated...");
            }
            foreach (GameObject sample in UpgradeBus.Instance.samplePrefabs.Values)
            {
                Item item = sample.GetComponent<PhysicsProp>().itemProperties;
                if (!StartOfRound.Instance.allItemsList.itemsList.Contains(item))
                {
                    StartOfRound.Instance.allItemsList.itemsList.Add(item);
                }

                logger.LogDebug($"{item.itemName} component initiated...");
            }
        }
        [HarmonyPrefix]
        [HarmonyPatch(nameof(StartOfRound.playersFiredGameOver))]
        private static void GameOverResetUpgradeManager(StartOfRound __instance)
        {
            if (UpgradeBus.Instance.PluginConfiguration.KEEP_UPGRADES_AFTER_FIRED_CUTSCENE.Value) return;
            logger.LogDebug("Configurations do not wish to keep upgrades, erasing...");

            if (!(__instance.NetworkManager.IsHost || __instance.NetworkManager.IsServer)) return;
            LguStore.Instance.PlayersFiredServerRpc();
        }
        [HarmonyPrefix]
        [HarmonyPatch(nameof(StartOfRound.PowerSurgeShip))]
        private static bool PowerSurgeShip()
        {
            if (BaseUpgrade.GetActiveUpgrade(LightningRod.UPGRADE_NAME)) return false;
            return true;
        }

        [HarmonyTranspiler]
        [HarmonyPatch(nameof(StartOfRound.ReviveDeadPlayers))]
        public static IEnumerable<CodeInstruction> ReviveDeadPlayers_Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var maximumHealthMethod = typeof(Stimpack).GetMethod(nameof(Stimpack.CheckForAdditionalHealth));
            List<CodeInstruction> codes = instructions.ToList();
            int index = 0;
            index = Tools.FindInteger(index, ref codes, findValue: 100, addCode: maximumHealthMethod, errorMessage: "Couldn't find maximum health on update health UI");
            index = Tools.FindInteger(index, ref codes, findValue: 100, addCode: maximumHealthMethod, errorMessage: "Couldn't find first maximum health on player's health attribute");
            index = Tools.FindInteger(index, ref codes, findValue: 100, addCode: maximumHealthMethod, errorMessage: "Couldn't find second maximum health on player's health attribute");
            index = Tools.FindInteger(index, ref codes, findValue: 100, addCode: maximumHealthMethod, errorMessage: "Couldn't find third maximum health on player's health attribute");
            return codes.AsEnumerable();
        }

        [HarmonyPatch(nameof(StartOfRound.ReviveDeadPlayers))]
        [HarmonyPostfix]
        private static void ResetContract(StartOfRound __instance)
        {
            if (UpgradeBus.Instance.contractLevel != RoundManager.Instance.currentLevel.PlanetName) return;
            if (!__instance.IsHost) return;

            LguStore.Instance.SyncContractDetailsClientRpc("None", -1);
        }

        [HarmonyPatch(nameof(StartOfRound.AutoSaveShipData))]
        [HarmonyPostfix]
        private static void AutoSaveShipDataPostfix()
        {
            if (!GameNetworkManager.Instance.isHostingGame) return;
            logger.LogDebug("Saving the LGU upgrades unto a json file...");
            LguStore.Instance.ServerSaveFileServerRpc();
        }
    }
}
