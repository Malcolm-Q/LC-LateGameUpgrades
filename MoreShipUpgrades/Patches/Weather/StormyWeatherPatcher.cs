using HarmonyLib;
using MoreShipUpgrades.Managers;
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
            if (LightningRod.instance != null && LightningRod.instance.LightningIntercepted)
            {
                switch(LightningRod.CurrentUpgradeMode)
                {
                    case LightningRod.UpgradeMode.EffectiveRange:
                    case LightningRod.UpgradeMode.AlwaysRerouteItem:
                        {
                            if (!useTargetedObject) return;
                            break;
                        }
                    case LightningRod.UpgradeMode.AlwaysRerouteRandom:
                        {
                            if (useTargetedObject) return;
                            break;
                        }
                    case LightningRod.UpgradeMode.AlwaysRerouteAll:
                        break;
                }
                LightningRod.RerouteLightningBolt(ref strikePosition, ref __instance);
            }
        }

        [HarmonyPostfix]
        [HarmonyPatch(nameof(StormyWeather.Update))]
        static void InterceptSelectedObject(StormyWeather __instance, GrabbableObject ___targetingMetalObject)
        {
            if (!BaseUpgrade.GetActiveUpgrade(LightningRod.UPGRADE_NAME) || !LguStore.Instance.IsHost || !LguStore.Instance.IsServer) { return; }
            LightningRod.TryInterceptLightning(ref __instance, ref ___targetingMetalObject);
        }
    }
}
