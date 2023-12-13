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
            }
            float rangeIncrease = (node.headerText == "Main entrance" || node.headerText == "Ship") ? UpgradeBus.instance.cfg.SHIP_AND_ENTRANCE_DISTANCE_INCREASE : UpgradeBus.instance.cfg.NODE_DISTANCE_INCREASE;
            if(UpgradeBus.instance.cfg.REQUIRE_LINE_OF_SIGHT && Physics.Linecast(playerScript.gameplayCamera.transform.position, node.transform.position, 256, QueryTriggerInteraction.Ignore))
            {
                __result = false;
            }
            else
            {
                float num = Vector3.Distance(playerScript.transform.position, node.transform.position);
                __result = (num < (float)node.maxRange + rangeIncrease && num > (float)node.minRange);
            }
        }
    }
}
