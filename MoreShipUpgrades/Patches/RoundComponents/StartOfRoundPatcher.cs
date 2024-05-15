using GameNetcodeStuff;
using HarmonyLib;
using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc;
using MoreShipUpgrades.Misc.Upgrades;
using MoreShipUpgrades.Misc.Util;
using MoreShipUpgrades.UpgradeComponents.OneTimeUpgrades;
using MoreShipUpgrades.UpgradeComponents.TierUpgrades;
using MoreShipUpgrades.UpgradeComponents.TierUpgrades.AttributeUpgrades;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using Unity.Netcode;
using UnityEngine;

namespace MoreShipUpgrades.Patches.RoundComponents
{
    [HarmonyPatch(typeof(StartOfRound))]
    internal static class StartOfRoundPatcher
    {
        static LguLogger logger = new LguLogger(nameof(StartOfRoundPatcher));
        [HarmonyPrefix]
        [HarmonyPatch(nameof(StartOfRound.Awake))]
        static void InitLguStore(StartOfRound __instance)
        {
            logger.LogDebug("Initiating components...");
            if (__instance.NetworkManager.IsHost || __instance.NetworkManager.IsServer)
            {
                GameObject refStore = Object.Instantiate(UpgradeBus.Instance.modStorePrefab);
                refStore.GetComponent<NetworkObject>().Spawn();
                logger.LogDebug("LguStore component initiated...");
            }
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
            Tools.FindInteger(ref index, ref codes, findValue: 100, addCode: maximumHealthMethod, errorMessage: "Couldn't find maximum health on update health UI");
            Tools.FindInteger(ref index, ref codes, findValue: 100, addCode: maximumHealthMethod, errorMessage: "Couldn't find first maximum health on player's health attribute");
            Tools.FindInteger(ref index, ref codes, findValue: 100, addCode: maximumHealthMethod, errorMessage: "Couldn't find second maximum health on player's health attribute");
            Tools.FindInteger(ref index, ref codes, findValue: 100, addCode: maximumHealthMethod, errorMessage: "Couldn't find third maximum health on player's health attribute");
            return codes.AsEnumerable();
        }

        [HarmonyPatch(nameof(StartOfRound.ReviveDeadPlayers))]
        [HarmonyPostfix]
        static void ResetContract(StartOfRound __instance)
        {
            if (ContractManager.Instance.contractLevel != RoundManager.Instance.currentLevel.PlanetName) return;
            if (!__instance.IsHost) return;

            ContractManager.Instance.SyncContractDetailsClientRpc("None", -1);
            if (!UpgradeBus.Instance.PluginConfiguration.QUANTUM_DISRUPTOR_ENABLED) return;
            QuantumDisruptor.Instance.TryResetValues(QuantumDisruptor.ResetModes.MoonLanding);
        }
        [HarmonyPatch(nameof(StartOfRound.AutoSaveShipData))]
        [HarmonyPrefix]
        static void AutoSaveShipDataPrefix()
        {
            PlayerManager.instance.ResetUpgradeSpentCredits();
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
            Tools.FindFloat(ref index, ref codes, findValue: 0.3f, addCode: sigurdChance, errorMessage: "Couldn't find the 0.3 value which is used as buying rate");
            return codes.AsEnumerable();
        }
        [HarmonyPatch(nameof(StartOfRound.SetPlanetsWeather))]
        [HarmonyPostfix]
        static void SetPlanetsWeatherPostfix(StartOfRound __instance)
        {
            if (__instance.IsHost || __instance.IsServer) return;
            if (LguStore.Instance == null) return;
            LguStore.Instance.SyncProbeWeathersServerRpc();
        }

        [HarmonyPatch(nameof(StartOfRound.OnPlayerConnectedClientRpc))]
        [HarmonyPostfix]
        static void ShareSaveShipDataPostfix(StartOfRound __instance)
        {
            if (__instance.IsHost || __instance.IsServer) return;
            LguStore.Instance.ShareSaveServerRpc();
        }

        [HarmonyPatch(nameof(StartOfRound.ShipLeave))]
        [HarmonyPrefix]
        static void ShipLeavePrefix(StartOfRound __instance)
        {
            if (!UpgradeBus.Instance.PluginConfiguration.LANDING_THRUSTERS_ENABLED) return;
            if (UpgradeBus.Instance.PluginConfiguration.LANDING_THRUSTERS_AFFECT_LANDING && !UpgradeBus.Instance.PluginConfiguration.LANDING_THRUSTERS_AFFECT_DEPARTING)
            {
                __instance.shipAnimator.speed /= LandingThrusters.GetLandingSpeedMultiplier();
                return;
            }
            if (!UpgradeBus.Instance.PluginConfiguration.LANDING_THRUSTERS_AFFECT_LANDING && UpgradeBus.Instance.PluginConfiguration.LANDING_THRUSTERS_AFFECT_DEPARTING)
                __instance.shipAnimator.speed *= LandingThrusters.GetLandingSpeedMultiplier();

        }
        [HarmonyPatch(nameof(StartOfRound.EndOfGame))]
        [HarmonyPostfix]
        static void EndOfGamePostfix(StartOfRound __instance)
        {
            if (!UpgradeBus.Instance.PluginConfiguration.LANDING_THRUSTERS_ENABLED) return;
            if (!UpgradeBus.Instance.PluginConfiguration.LANDING_THRUSTERS_AFFECT_DEPARTING) return;

            __instance.shipAnimator.speed /= LandingThrusters.GetLandingSpeedMultiplier();
        }

        [HarmonyPatch(nameof(StartOfRound.openingDoorsSequence))]
        [HarmonyPrefix]
        static void openingDoorsSequencePrefix(StartOfRound __instance)
        {
            if (!UpgradeBus.Instance.PluginConfiguration.LANDING_THRUSTERS_ENABLED) return;
            if (!UpgradeBus.Instance.PluginConfiguration.LANDING_THRUSTERS_AFFECT_LANDING) return;

            __instance.shipAnimator.speed *= LandingThrusters.GetLandingSpeedMultiplier();
        }

        [HarmonyPatch(nameof(StartOfRound.openingDoorsSequence), MethodType.Enumerator)]
        [HarmonyTranspiler]
        static IEnumerable<CodeInstruction> openingDoorsSequenceTranspiler(IEnumerable<CodeInstruction> instructions)
        {
            MethodInfo getLandingSpeedMultiplier = typeof(LandingThrusters).GetMethod(nameof(LandingThrusters.GetInteractMutliplier));
            List<CodeInstruction> codes = instructions.ToList();
            int index = 0;
            Tools.FindFloat(ref index, ref codes, findValue: 5f, addCode: getLandingSpeedMultiplier, errorMessage: "Couldn't find the 5f value used on WaitForSeconds");
            codes.Insert(index+1, new CodeInstruction(OpCodes.Div));
            Tools.FindFloat(ref index, ref codes, findValue: 10f, addCode: getLandingSpeedMultiplier, errorMessage: "Couldn't find the 10f value used on WaitForSeconds");
            codes.Insert(index+1, new CodeInstruction(OpCodes.Div));
            return codes;
        }

        [HarmonyPatch(nameof(StartOfRound.ChangeLevel))]
        [HarmonyPostfix]
        static void ChangeLevelPostfix()
        {
            if (!UpgradeBus.Instance.PluginConfiguration.QUANTUM_DISRUPTOR_ENABLED) return;
            QuantumDisruptor.Instance.TryResetValues(QuantumDisruptor.ResetModes.MoonRerouting);
        }
    }
}
