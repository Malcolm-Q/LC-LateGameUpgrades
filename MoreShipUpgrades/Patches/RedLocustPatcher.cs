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
    [HarmonyPatch(typeof(RedLocustBees))]
    internal class RedLocustPatcher
    {
        [HarmonyPrefix]
        [HarmonyPatch("OnCollideWithPlayer")]
        private static bool ReduceDamage()
        {
            if (!UpgradeBus.instance.beekeeper) { return true; }
            return false;
        }
    }
}
