using GameNetcodeStuff;
using HarmonyLib;
using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc;
using MoreShipUpgrades.UpgradeComponents.Commands;
using MoreShipUpgrades.UpgradeComponents.Items.Wheelbarrow;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using UnityEngine;

namespace MoreShipUpgrades.Patches.HUD
{
    [HarmonyPatch(typeof(HUDManager))]
    internal class HUDManagerPatcher
    {
        [HarmonyPostfix]
        [HarmonyPatch(nameof(HUDManager.MeetsScanNodeRequirements))]
        private static void alterReqs(ScanNodeProperties node, ref bool __result, PlayerControllerB playerScript)
        {
            if (node != null && node.GetComponentInParent<WheelbarrowScript>() != null && node.headerText != "Shopping Cart" && node.headerText != "Wheelbarrow") { __result = false; return; }
            if (!UpgradeBus.instance.scannerUpgrade) { return; }
            if (node == null) { __result = false; return; }
            bool throughWall = Physics.Linecast(playerScript.gameplayCamera.transform.position, node.transform.position, 256, QueryTriggerInteraction.Ignore);
            bool cannotSeeEnemiesThroughWalls = node.nodeType == 1 && !UpgradeBus.instance.cfg.BETTER_SCANNER_ENEMIES;
            if (throughWall)
            {
                if (UpgradeBus.instance.scanLevel < 2 || UpgradeBus.instance.scanLevel == 2 && cannotSeeEnemiesThroughWalls)
                {
                    __result = false;
                    return;
                }
            }
            float rangeIncrease = node.headerText == "Main entrance" || node.headerText == "Ship" ? UpgradeBus.instance.cfg.SHIP_AND_ENTRANCE_DISTANCE_INCREASE : UpgradeBus.instance.cfg.NODE_DISTANCE_INCREASE;
            float num = Vector3.Distance(playerScript.transform.position, node.transform.position);
            __result = num < node.maxRange + rangeIncrease && num > node.minRange;
        }

        [HarmonyPrefix]
        [HarmonyPatch(nameof(HUDManager.UseSignalTranslatorServerRpc))]
        static bool CancelSignal(SignalTranslator __instance)
        {
            if (UpgradeBus.instance.pager) return false; // return false gaming
            else return true;
        }

        [HarmonyPatch(nameof(HUDManager.FillEndGameStats))]
        [HarmonyTranspiler]
        private static IEnumerable<CodeInstruction> FillEndGameStatsTranspiler(IEnumerable<CodeInstruction> instructions)
        {
            FieldInfo allPlayersDead = typeof(StartOfRound).GetField(nameof(StartOfRound.allPlayersDead));
            MethodInfo scrapInsuranceStatus = typeof(ScrapInsurance).GetMethod(nameof(ScrapInsurance.GetScrapInsuranceStatus));
            List<CodeInstruction> codes = new List<CodeInstruction>(instructions);
            int index = 0;
            index = Tools.FindField(index, ref codes, findField: allPlayersDead, addCode: scrapInsuranceStatus, notInstruction: true, andInstruction: true, errorMessage: "Couldn't find all players dead field");
            return codes;
        }
    }
}
