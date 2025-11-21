using GameNetcodeStuff;
using HarmonyLib;
using MoreShipUpgrades.Configuration;
using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc;
using MoreShipUpgrades.Misc.Upgrades;
using MoreShipUpgrades.Misc.Util;
using MoreShipUpgrades.UpgradeComponents.Commands;
using MoreShipUpgrades.UpgradeComponents.OneTimeUpgrades;
using MoreShipUpgrades.UpgradeComponents.OneTimeUpgrades.Store;
using MoreShipUpgrades.UpgradeComponents.TierUpgrades;
using MoreShipUpgrades.UpgradeComponents.TierUpgrades.AttributeUpgrades;
using MoreShipUpgrades.UpgradeComponents.TierUpgrades.Ship;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using Unity.Netcode;
using UnityEngine;
using static MoreShipUpgrades.Managers.ItemProgressionManager;

namespace MoreShipUpgrades.Patches.RoundComponents
{
    [HarmonyPatch(typeof(StartOfRound))]
    internal static class StartOfRoundPatcher
    {
        static readonly LguLogger logger = new(nameof(StartOfRoundPatcher));
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

        [HarmonyPatch(nameof(StartOfRound.LateUpdate))]
        [HarmonyPostfix]
        static void LateUpdatePostfix()
        {
            LategameConfiguration config = UpgradeBus.Instance.PluginConfiguration;
            if (!config.INTERN_ENABLED || config.INTERNS_TELEPORT_RESTRICTION != Interns.TeleportRestriction.EnterShip) return;
            PlayerControllerB player = GameNetworkManager.Instance.localPlayerController;
            if (Interns.instance.ContainsRecentlyInterned(player) && (player.isInHangarShipRoom || player.isInElevator))
            {
                Interns.instance.RemoveRecentlyInternedServerRpc(player);
            }
        }

        [HarmonyPrefix]
        [HarmonyPatch(nameof(StartOfRound.LoadShipGrabbableItems))]
        static void LoadShipGrabbableItemsPrefix(StartOfRound __instance)
        {
            if (!__instance.IsServer) return;

            AssignScrapToUpgrades();
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
            return !BaseUpgrade.GetActiveUpgrade(LightningRod.UPGRADE_NAME);
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
        static void ReviveDeadPlayersPostfix(StartOfRound __instance)
        {
            ResetContract(ref __instance);
            QuantumDisruptor.TryResetQuantum(QuantumDisruptor.ResetModes.MoonLanding);
            RandomizeUpgradeManager.RandomizeUpgrades(RandomizeUpgradeManager.RandomizeUpgradeEvents.PerMoonLanding);
            Interns.instance.currentUsages = 0;
            if (UpgradeBus.Instance.PluginConfiguration.INTERN_ENABLED)
            {
                Interns.instance.ResetRecentlyInterned();
            }
        }
        static void ResetContract(ref StartOfRound __instance)
        {
            if (ContractManager.Instance.contractLevel != RoundManager.Instance.currentLevel.PlanetName) return;
            if (!__instance.IsHost) return;

            ContractManager.Instance.SyncContractDetailsClientRpc("None", -1);
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

        [HarmonyPatch(nameof(StartOfRound.OnPlayerConnectedClientRpc))]
        [HarmonyPostfix]
        static void ShareSaveShipDataPostfix(StartOfRound __instance)
        {
            if (__instance.IsHost || __instance.IsServer) return;
            LguStore.Instance.ShareSaveServerRpc();
        }

        #region Landing Speed

        [HarmonyPatch(nameof(StartOfRound.ShipLeave))]
        [HarmonyPrefix]
        static void ShipLeavePrefix(StartOfRound __instance)
        {
            if (!UpgradeBus.Instance.PluginConfiguration.LandingThrustersConfiguration.Enabled) return;
            if (UpgradeBus.Instance.PluginConfiguration.LandingThrustersConfiguration.AffectLanding && !UpgradeBus.Instance.PluginConfiguration.LandingThrustersConfiguration.AffectDeparture)
			{
				Plugin.mls.LogDebug("Removing speed in ShipLeave callback");
				__instance.shipAnimator.speed /= LandingThrusters.GetLandingSpeedMultiplier();
                return;
            }
            if (!UpgradeBus.Instance.PluginConfiguration.LandingThrustersConfiguration.AffectLanding && UpgradeBus.Instance.PluginConfiguration.LandingThrustersConfiguration.AffectDeparture)
            {
				Plugin.mls.LogDebug("Adding speed in ShipLeave callback");
				__instance.shipAnimator.speed *= LandingThrusters.GetLandingSpeedMultiplier();
			}
        }
        [HarmonyPatch(nameof(StartOfRound.EndOfGame))]
        [HarmonyPostfix]
        static void EndOfGamePostfix(StartOfRound __instance)
        {
            if (!UpgradeBus.Instance.PluginConfiguration.LandingThrustersConfiguration.Enabled) return;
            if (!UpgradeBus.Instance.PluginConfiguration.LandingThrustersConfiguration.AffectDeparture) return;
            Plugin.mls.LogDebug("Removing speed in EndOfGame callback");
            __instance.shipAnimator.speed /= LandingThrusters.GetLandingSpeedMultiplier();
        }

        [HarmonyPatch(nameof(StartOfRound.openingDoorsSequence), MethodType.Enumerator)]
        [HarmonyTranspiler]
        static IEnumerable<CodeInstruction> OpeningDoorsSequenceTranspiler(IEnumerable<CodeInstruction> instructions)
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

        #endregion

        [HarmonyPatch(nameof(StartOfRound.ChangeLevel))]
        [HarmonyPostfix]
        static void ChangeLevelPostfix()
        {
            QuantumDisruptor.TryResetQuantum(QuantumDisruptor.ResetModes.MoonRerouting);
            RandomizeUpgradeManager.RandomizeUpgrades(RandomizeUpgradeManager.RandomizeUpgradeEvents.PerMoonRouting);
        }
    }
}
