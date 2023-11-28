using GameNetcodeStuff;
using HarmonyLib;
using MoreShipUpgrades.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace MoreShipUpgrades.Patches
{
    [HarmonyPatch(typeof(HUDManager))]
    internal class HUDManagerPatcher
    {
        [HarmonyPostfix]
        [HarmonyPatch("MeetsScanNodeRequirements")]
        private static void alterReqs(ref HUDManager __instance, ScanNodeProperties node, ref bool __result, PlayerControllerB playerScript)
        {
            if (!UpgradeBus.instance.scannerUpgrade) { return; }
            float rangeIncrease = (node.headerText == "Main entrance" || node.headerText == "Ship") ? Plugin.cfg.SHIP_AND_ENTRANCE_DISTANCE_INCREASE : Plugin.cfg.NODE_DISTANCE_INCREASE;
            if (node == null)
            {
                __result = false;
            }
            else
            {
                // if we need line of sight and we don't have it return false
                if(Plugin.cfg.REQUIRE_LINE_OF_SIGHT && Physics.Linecast(playerScript.gameplayCamera.transform.position, node.transform.position, 256, QueryTriggerInteraction.Ignore))
                {
                    __result = false;
                }
                // otherwise check distance.
                else
                {
                    float num = Vector3.Distance(playerScript.transform.position, node.transform.position);
                    __result = (num < (float)node.maxRange + rangeIncrease && num > (float)node.minRange);
                }
            }
        }
    }
}
