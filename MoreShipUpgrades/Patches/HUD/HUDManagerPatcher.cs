using GameNetcodeStuff;
using HarmonyLib;
using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc.Upgrades;
using MoreShipUpgrades.Misc.Util;
using MoreShipUpgrades.UpgradeComponents.Commands;
using MoreShipUpgrades.UpgradeComponents.Items.Wheelbarrow;
using MoreShipUpgrades.UpgradeComponents.OneTimeUpgrades;
using MoreShipUpgrades.UpgradeComponents.TierUpgrades;
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
        static void alterReqs(ScanNodeProperties node, ref bool __result, PlayerControllerB playerScript)
        {
            if (node != null && node.GetComponentInParent<WheelbarrowScript>() != null && node.headerText != "Shopping Cart" && node.headerText != "Wheelbarrow") { __result = false; return; }
            if (!BaseUpgrade.GetActiveUpgrade(BetterScanner.UPGRADE_NAME)) { return; }
            if (node == null) { __result = false; return; }
            bool throughWall = Physics.Linecast(playerScript.gameplayCamera.transform.position, node.transform.position, 256, QueryTriggerInteraction.Ignore);
            bool hasRequiredLevel = BaseUpgrade.GetUpgradeLevel(BetterScanner.UPGRADE_NAME) == 2;
            bool cannotSeeEnemiesThroughWalls = node.nodeType == 1 && !UpgradeBus.Instance.PluginConfiguration.BETTER_SCANNER_ENEMIES.Value;
            if (throughWall && (!hasRequiredLevel || cannotSeeEnemiesThroughWalls))
            {
                __result = false;
                return;
            }
            float rangeIncrease = node.headerText == "Main entrance" || node.headerText == "Ship" ? UpgradeBus.Instance.PluginConfiguration.SHIP_AND_ENTRANCE_DISTANCE_INCREASE.Value : UpgradeBus.Instance.PluginConfiguration.NODE_DISTANCE_INCREASE.Value;
            float num = Vector3.Distance(playerScript.transform.position, node.transform.position);
            __result = num <= node.maxRange + rangeIncrease && num >= node.minRange;
        }

        [HarmonyPatch(nameof(HUDManager.FillEndGameStats))]
        [HarmonyTranspiler]
        static IEnumerable<CodeInstruction> FillEndGameStatsTranspiler(IEnumerable<CodeInstruction> instructions)
        {
            FieldInfo allPlayersDead = typeof(StartOfRound).GetField(nameof(StartOfRound.allPlayersDead));
            MethodInfo scrapInsuranceStatus = typeof(ScrapInsurance).GetMethod(nameof(ScrapInsurance.GetScrapInsuranceStatus));
            List<CodeInstruction> codes = new List<CodeInstruction>(instructions);
            int index = 0;
            Tools.FindField(ref index, ref codes, findField: allPlayersDead, addCode: scrapInsuranceStatus, notInstruction: true, andInstruction: true, errorMessage: "Couldn't find all players dead field");
            return codes;
        }
        [HarmonyPatch(nameof(HUDManager.UseSignalTranslatorServerRpc))]
        [HarmonyTranspiler]
        static IEnumerable<CodeInstruction> UseSignalTranslatorServerRpcTranspiler(IEnumerable<CodeInstruction> instructions)
        {
            MethodInfo newLimitCharactersTransmit = typeof(FastEncryption).GetMethod(nameof(FastEncryption.GetLimitOfCharactersTransmit));
            List<CodeInstruction> codes = new List<CodeInstruction>(instructions);
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
            List<CodeInstruction> codes = new List<CodeInstruction>(instructions);
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
            List<CodeInstruction> codes = new List<CodeInstruction>(instructions);
            int index = 0;
            Tools.FindFloat(ref index, ref codes, findValue: 0.5f, addCode: multiplierSignalTimer, errorMessage: "Couldn't find the 0.5f value which is used in enumerator wait timer");
            Tools.FindFloat(ref index, ref codes, findValue: 0.7f, addCode: multiplierSignalTimer, errorMessage: "Couldn't find the 0.5f value which is used in enumerator wait timer");
            return codes;
        }
    }
}
