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
        static void CheckIfLightningRodPresent(StormyWeather __instance,ref Vector3 strikePosition, bool useTargetedObject)
        {
            if (lightningRodScript.instance.LightningIntercepted && useTargetedObject)
            {
                // we need to check useTargetedObject so we're not rerouting random strikes to the ship.
                lightningRodScript.RerouteLightningBolt(ref strikePosition, ref __instance);
            }
        }

        [HarmonyPostfix]
        [HarmonyPatch("Update")]
        static void InterceptSelectedObject(StormyWeather __instance, GrabbableObject ___targetingMetalObject)
        {
            if(!UpgradeBus.instance.lightningRod || !LGUStore.instance.IsHost || !LGUStore.instance.IsServer) { return; }
            if(___targetingMetalObject == null)
            {
                lightningRodScript.instance.CanTryInterceptLightning = true;
            }
            else
            {
                lightningRodScript.TryInterceptLightning(ref __instance, ref ___targetingMetalObject);
            }
        }
    }
}
