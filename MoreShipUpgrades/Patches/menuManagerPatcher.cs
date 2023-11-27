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
    [HarmonyPatch(typeof(MenuManager))]
    internal class menuManagerPatcher
    {
        [HarmonyPostfix]
        [HarmonyPatch("Start")]
        private static void resetUpgradeManager()
        {
            UpgradeBus.instance.ResetAllValues();
        }
    }
}
