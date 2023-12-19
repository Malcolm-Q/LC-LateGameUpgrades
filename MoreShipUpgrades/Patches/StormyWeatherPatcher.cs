using HarmonyLib;
using MoreShipUpgrades.Managers;
using MoreShipUpgrades.UpgradeComponents;
using UnityEngine;


namespace MoreShipUpgrades.Patches
{
    [HarmonyPatch(typeof(StormyWeather))]
    internal class StormyWeatherPatcher
    {
        [HarmonyPrefix]
        [HarmonyPatch("LightningStrike")]
        public static void CheckIfLightningRodPresent(ref Vector3 strikePosition)
        {
            if (UpgradeBus.instance.lightningRod)
            {
                lightningRodScript.TryCatchLightningBolt(ref strikePosition);
            }
        }
    }
}
