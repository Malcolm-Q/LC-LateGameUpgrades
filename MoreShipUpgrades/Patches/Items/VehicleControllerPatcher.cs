using HarmonyLib;
using MoreShipUpgrades.Managers;
using MoreShipUpgrades.UpgradeComponents.OneTimeUpgrades.Items;

namespace MoreShipUpgrades.Patches.Items
{
    [HarmonyPatch(typeof(VehicleController))]
    internal static class VehicleControllerPatcher
    {
        [HarmonyPatch(nameof(VehicleController.SetRadioOnLocalClient))]
        [HarmonyPrefix]
        static void SetRadioOnLocalClientPrefix(VehicleController __instance)
        {
            if (UpgradeBus.Instance.PluginConfiguration.BEATS_ENABLED.Value && !SickBeats.Instance.vehicleControllers.Contains(__instance)) SickBeats.Instance.vehicleControllers.Add(__instance);
        }
    }
}
