using HarmonyLib;
using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc;
using MoreShipUpgrades.Misc.Upgrades;
using MoreShipUpgrades.UpgradeComponents.OneTimeUpgrades;
using UnityEngine;


namespace MoreShipUpgrades.Patches.Weather
{
    [HarmonyPatch(typeof(StormyWeather))]
    internal static class StormyWeatherPatcher
    {
        [HarmonyPrefix]
        [HarmonyPatch(nameof(StormyWeather.LightningStrike))]
        static void CheckIfLightningRodPresent(StormyWeather __instance, ref Vector3 strikePosition, bool useTargetedObject)
        {
            if (LightningRod.instance != null && LightningRod.instance.LightningIntercepted && useTargetedObject)
            {
                // we need to check useTargetedObject so we're not rerouting random strikes to the ship.
                LightningRod.RerouteLightningBolt(ref strikePosition, ref __instance);
            }
        }

        [HarmonyPostfix]
        [HarmonyPatch(nameof(StormyWeather.Update))]
        static void InterceptSelectedObject(StormyWeather __instance, GrabbableObject ___targetingMetalObject)
        {
            if (!BaseUpgrade.GetActiveUpgrade(LightningRod.UPGRADE_NAME) || !LguStore.Instance.IsHost || !LguStore.Instance.IsServer) { return; }
            if (___targetingMetalObject == null)
            {
                if (LightningRod.instance != null) // Lightning rod could be disabled so we wouldn't have an instance
                    LightningRod.instance.CanTryInterceptLightning = true;
            }
            else
            {
                LightningRod.TryInterceptLightning(ref __instance, ref ___targetingMetalObject);
            }
        }
    }
}
