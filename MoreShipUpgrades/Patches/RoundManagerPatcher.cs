using HarmonyLib;
using MoreShipUpgrades.Managers;

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
            noiseRange -= UpgradeBus.instance.cfg.NOISE_REDUCTION;
        }
    }
}
