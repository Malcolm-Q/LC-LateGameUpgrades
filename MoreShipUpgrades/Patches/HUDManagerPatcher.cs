using GameNetcodeStuff;
using HarmonyLib;
using MoreShipUpgrades.Managers;
using UnityEngine;

namespace MoreShipUpgrades.Patches
{
    [HarmonyPatch(typeof(HUDManager))]
    internal class HUDManagerPatcher
    {
        [HarmonyPostfix]
        [HarmonyPatch("MeetsScanNodeRequirements")]
        private static void alterReqs(ScanNodeProperties node, ref bool __result, PlayerControllerB playerScript)
        {
            if (!UpgradeBus.instance.scannerUpgrade) { return; }
            if (node == null)
            {
                __result = false;
                return;
            }
            if(node.nodeType == 1 && !UpgradeBus.instance.cfg.BETTER_SCANNER_ENEMIES)
            {
                __result = false;
                return;
            }
            float rangeIncrease = (node.headerText == "Main entrance" || node.headerText == "Ship") ? UpgradeBus.instance.cfg.SHIP_AND_ENTRANCE_DISTANCE_INCREASE : UpgradeBus.instance.cfg.NODE_DISTANCE_INCREASE;
            if(UpgradeBus.instance.scanLevel < 2 && Physics.Linecast(playerScript.gameplayCamera.transform.position, node.transform.position, 256, QueryTriggerInteraction.Ignore))
            {
                __result = false;
            }
            else
            {
                float num = Vector3.Distance(playerScript.transform.position, node.transform.position);
                __result = (num < (float)node.maxRange + rangeIncrease && num > (float)node.minRange);
            }
        }

        [HarmonyPrefix]
        [HarmonyPatch("UseSignalTranslatorServerRpc")]
        static bool CancelSignal(SignalTranslator __instance)
        {
            if (UpgradeBus.instance.pager) return false; // return false gaming
            else return true;
        }
    }
}
