using HarmonyLib;
using MoreShipUpgrades.Managers;
using MoreShipUpgrades.UpgradeComponents.OneTimeUpgrades;

namespace MoreShipUpgrades.Patches.Items
{
    [HarmonyPatch(typeof(BoomboxItem))]
    internal static class BoomBoxPatcher
    {
        [HarmonyPrefix]
        [HarmonyPatch(nameof(BoomboxItem.StartMusic))]
        static void StartMusicPrefix(BoomboxItem __instance)
        {
            if (UpgradeBus.Instance.PluginConfiguration.BEATS_ENABLED.Value && !SickBeats.Instance.boomBoxes.Contains(__instance)) SickBeats.Instance.boomBoxes.Add(__instance);
        }
    }
}