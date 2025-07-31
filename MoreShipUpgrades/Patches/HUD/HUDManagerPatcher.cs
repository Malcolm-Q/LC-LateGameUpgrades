using GameNetcodeStuff;
using HarmonyLib;
using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc.Upgrades;
using MoreShipUpgrades.Misc.Util;
using MoreShipUpgrades.UpgradeComponents.OneTimeUpgrades.Ship;
using MoreShipUpgrades.UpgradeComponents.TierUpgrades.Items;
using MoreShipUpgrades.UpgradeComponents.TierUpgrades.Player;
using MoreShipUpgrades.UpgradeComponents.TierUpgrades.Ship;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using UnityEngine;

namespace MoreShipUpgrades.Patches.HUD
{
    [HarmonyPatch(typeof(HUDManager))]
    internal static class HudManagerPatcher
    {
        [HarmonyPostfix]
        [HarmonyPatch(nameof(HUDManager.MeetsScanNodeRequirements))]
        static void MeetsScanNodeRequirementsPostFix(ScanNodeProperties node, ref bool __result, PlayerControllerB playerScript)
        {
            if (__result) return;
            if (!BaseUpgrade.GetActiveUpgrade(BetterScanner.UPGRADE_NAME)) { return; }
            if (node == null) { __result = false; return; }
            float rangeIncrease = node.headerText == "Main entrance" || node.headerText == "Ship" ? UpgradeBus.Instance.PluginConfiguration.BetterScannerUpgradeConfiguration.OutsideNodesRangeIncrease.Value : UpgradeBus.Instance.PluginConfiguration.BetterScannerUpgradeConfiguration.NodeRangeIncrease.Value;
            float num = Vector3.Distance(playerScript.transform.position, node.transform.position);
            __result = num <= node.maxRange + rangeIncrease && num >= node.minRange && !node.requiresLineOfSight;
			bool hasRequiredLevel = BaseUpgrade.GetUpgradeLevel(BetterScanner.UPGRADE_NAME) == 2;
            if (!hasRequiredLevel) return;
			bool throughWall = Physics.Linecast(playerScript.gameplayCamera.transform.position, node.transform.position, 134217984, QueryTriggerInteraction.Ignore);
			bool cannotSeeEnemiesThroughWalls = node.nodeType == 1 && !UpgradeBus.Instance.PluginConfiguration.BetterScannerUpgradeConfiguration.SeeEnemiesThroughWalls.Value;
			if (throughWall && cannotSeeEnemiesThroughWalls)
			{
				__result = false;
			}
		}
        [HarmonyPatch(nameof(HUDManager.UseSignalTranslatorServerRpc))]
        [HarmonyTranspiler]
        static IEnumerable<CodeInstruction> UseSignalTranslatorServerRpcTranspiler(IEnumerable<CodeInstruction> instructions)
        {
            MethodInfo newLimitCharactersTransmit = typeof(FastEncryption).GetMethod(nameof(FastEncryption.GetLimitOfCharactersTransmit));
            List<CodeInstruction> codes = new(instructions);
            int index = 0;
            Tools.FindInteger(ref index, ref codes, findValue: 12, skip: true, errorMessage: "Couldn't find the 12 value which is used as character limit");
            codes.Insert(index, new CodeInstruction(OpCodes.Call, newLimitCharactersTransmit));
            codes.Insert(index, new CodeInstruction(OpCodes.Ldarg_1));
            return codes;
        }
        [HarmonyPatch(nameof(HUDManager.UseSignalTranslatorClientRpc))]
        [HarmonyTranspiler]
        static IEnumerable<CodeInstruction> UseSignalTranslatorClientRpcTranspiler(IEnumerable<CodeInstruction> instructions)
        {
            MethodInfo newLimitCharactersTransmit = typeof(FastEncryption).GetMethod(nameof(FastEncryption.GetLimitOfCharactersTransmit));
            List<CodeInstruction> codes = new(instructions);
            int index = 0;
            Tools.FindInteger(ref index, ref codes, findValue: 10, skip: true, errorMessage: "Couldn't find the 10 value which is used as character limit");
            codes.Insert(index, new CodeInstruction(OpCodes.Call, newLimitCharactersTransmit));
            codes.Insert(index, new CodeInstruction(OpCodes.Ldarg_1));
            return codes;
        }

        [HarmonyPatch(nameof(HUDManager.DisplaySignalTranslatorMessage), MethodType.Enumerator)]
        [HarmonyTranspiler]
        static IEnumerable<CodeInstruction> DisplaySignalTranslatorMessageTranspiler(IEnumerable<CodeInstruction> instructions)
        {
            MethodInfo multiplierSignalTimer = typeof(FastEncryption).GetMethod(nameof(FastEncryption.GetMultiplierOnSignalTextTimer));
            List<CodeInstruction> codes = new(instructions);
            int index = 0;
            Tools.FindFloat(ref index, ref codes, findValue: 0.5f, addCode: multiplierSignalTimer, errorMessage: "Couldn't find the 0.5f value which is used in enumerator wait timer");
            Tools.FindFloat(ref index, ref codes, findValue: 0.7f, addCode: multiplierSignalTimer, errorMessage: "Couldn't find the 0.5f value which is used in enumerator wait timer");
            return codes;
        }

        [HarmonyTranspiler]
        [HarmonyPatch(nameof(HUDManager.ApplyPenalty))]
        static IEnumerable<CodeInstruction> ApplyPenaltyTranspiler(IEnumerable<CodeInstruction> instructions)
        {
            MethodInfo ReduceCreditCostPercentage = typeof(LifeInsurance).GetMethod(nameof(LifeInsurance.ReduceCreditCostPercentage));
            List<CodeInstruction> codes = new(instructions);
            int index = 0;
            Tools.FindFloat(ref index, ref codes, findValue: 0.2f, addCode: ReduceCreditCostPercentage, errorMessage: "Couldn't find the multiplier used to decrease the amount of Company Credits");
            return codes;
        }

        [HarmonyTranspiler]
        [HarmonyPatch(nameof(HUDManager.FillEndGameStats))]
        static IEnumerable<CodeInstruction> FillEndGameStatsTranspiler(IEnumerable<CodeInstruction> instructions)
        {
            FieldInfo allPlayersDead = typeof(StartOfRound).GetField(nameof(StartOfRound.allPlayersDead));

            MethodInfo CheckIfKeptScrap = typeof(ScrapKeeper).GetMethod(nameof(ScrapKeeper.CheckIfKeptScrap));
            List<CodeInstruction> codes = new(instructions);
            int index = 0;
            Tools.FindField(ref index, ref codes, findField: allPlayersDead, addCode: CheckIfKeptScrap, andInstruction: true, notInstruction: true);
            return codes;
        }
    }
}
