using GameNetcodeStuff;
using HarmonyLib;
using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc;
using MoreShipUpgrades.Misc.Upgrades;
using MoreShipUpgrades.UpgradeComponents.OneTimeUpgrades;
using MoreShipUpgrades.UpgradeComponents.TierUpgrades.AttributeUpgrades;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Unity.Netcode;
using UnityEngine;

namespace MoreShipUpgrades.Patches.RoundComponents
{
    [HarmonyPatch(typeof(StartOfRound))]
    internal static class StartOfRoundPatcher
    {
        static LguLogger logger = new LguLogger(nameof(StartOfRoundPatcher));
        [HarmonyPrefix]
        [HarmonyPatch(nameof(StartOfRound.Start))]
        static void InitLguStore(StartOfRound __instance)
        {
            logger.LogDebug("Initiating components...");
            if (__instance.NetworkManager.IsHost || __instance.NetworkManager.IsServer)
            {
                GameObject refStore = Object.Instantiate(UpgradeBus.Instance.modStorePrefab);
                refStore.GetComponent<NetworkObject>().Spawn();
                logger.LogDebug("LguStore component initiated...");
            }
            SpawnItemManager.Instance.SetupSpawnableItems();
        }
        [HarmonyPrefix]
        [HarmonyPatch(nameof(StartOfRound.playersFiredGameOver))]
        static void GameOverResetUpgradeManager(StartOfRound __instance)
        {
            if (UpgradeBus.Instance.PluginConfiguration.KEEP_UPGRADES_AFTER_FIRED_CUTSCENE.Value) return;
            logger.LogDebug("Configurations do not wish to keep upgrades, erasing...");

            if (!(__instance.NetworkManager.IsHost || __instance.NetworkManager.IsServer)) return;
            LguStore.Instance.PlayersFiredServerRpc();
        }
        [HarmonyPrefix]
        [HarmonyPatch(nameof(StartOfRound.PowerSurgeShip))]
        static bool PowerSurgeShip()
        {
            if (BaseUpgrade.GetActiveUpgrade(LightningRod.UPGRADE_NAME)) return false;
            return true;
        }

        [HarmonyTranspiler]
        [HarmonyPatch(nameof(StartOfRound.ReviveDeadPlayers))]
        static IEnumerable<CodeInstruction> ReviveDeadPlayers_Transpiler(IEnumerable<CodeInstruction> instructions)
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
        static void ResetContract(StartOfRound __instance)
        {
            if (ContractManager.Instance.contractLevel != RoundManager.Instance.currentLevel.PlanetName) return;
            if (!__instance.IsHost) return;

            ContractManager.Instance.SyncContractDetailsClientRpc("None", -1);
        }

        [HarmonyPatch(nameof(StartOfRound.AutoSaveShipData))]
        [HarmonyPostfix]
        static void AutoSaveShipDataPostfix()
        {
            if (!GameNetworkManager.Instance.isHostingGame) return;
            logger.LogDebug("Saving the LGU upgrades unto a json file...");
            LguStore.Instance.ServerSaveFileServerRpc();
        }

        [HarmonyPatch(nameof(StartOfRound.ResetShip))]
        [HarmonyTranspiler]
        static IEnumerable<CodeInstruction> ResetShip(IEnumerable<CodeInstruction> instructions)
        {
            MethodInfo sigurdChance = typeof(Sigurd).GetMethod(nameof(Sigurd.GetBuyingRate));
            List<CodeInstruction> codes = new(instructions);
            int index = 0;
            index = Tools.FindFloat(index, ref codes, findValue: 0.3f, addCode: sigurdChance, errorMessage: "Couldn't find the 0.3 value which is used as buying rate");
            return codes.AsEnumerable();
        }
        [HarmonyPatch(nameof(StartOfRound.OnPlayerConnectedClientRpc))]
        [HarmonyPostfix]
        static void SetPlanetsWeatherPostfix(StartOfRound __instance)
        {
            if (__instance.IsHost || __instance.IsServer) return;
            LguStore.Instance.SyncProbeWeathersServerRpc();
        }

        [HarmonyPatch(nameof(StartOfRound.OnPlayerConnectedClientRpc))]
        [HarmonyPostfix]
        static void ShareSaveShipDataPostfix()
        {
            if (GameNetworkManager.Instance.isHostingGame) return;
            Tools.ReplaceSaveWithHost();
        }
    }
}
