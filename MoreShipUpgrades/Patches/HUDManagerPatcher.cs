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
            float rangeIncrease = (node.headerText == "Main entrance" || node.headerText == "Ship") ? 150f : 20f;
            if (node == null)
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
