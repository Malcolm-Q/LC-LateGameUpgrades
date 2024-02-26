using HarmonyLib;
using MoreShipUpgrades.Managers;
using MoreShipUpgrades.UpgradeComponents.OneTimeUpgrades;

namespace MoreShipUpgrades.Patches.Items
{
    [HarmonyPatch(typeof(BoomboxItem))]
    internal static class BoomBoxPatcher
    {
        [HarmonyPostfix]
        [HarmonyPatch(nameof(BoomboxItem.Start))]
        static void AddToList(BoomboxItem __instance)
        {
            if (UpgradeBus.Instance.PluginConfiguration.BEATS_ENABLED.Value) SickBeats.Instance.boomBoxes.Add(__instance);
        }
    }
}
    