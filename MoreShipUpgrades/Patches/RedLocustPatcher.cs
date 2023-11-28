using HarmonyLib;
using MoreShipUpgrades.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreShipUpgrades.Patches
{
    [HarmonyPatch(typeof(RoundManager))]
    internal class RoundManagerPatcher
    {
        [HarmonyPrefix]
        [HarmonyPatch("PlayAudibleNoise")]
        private static void MakeFootstepsQuiet(ref float noiseRange)
        {
            if(!UpgradeBus.instance.softSteps) { return; }
            noiseRange -= Plugin.cfg.NOISE_REDUCTION;
        }
    }
}
