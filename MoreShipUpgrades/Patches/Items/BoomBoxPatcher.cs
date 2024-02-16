using HarmonyLib;
using MoreShipUpgrades.Managers;

namespace MoreShipUpgrades.Patches.Items
{
    [HarmonyPatch(typeof(BoomboxItem))]
    internal static class BoomBoxPatcher
    {
        [HarmonyPostfix]
        [HarmonyPatch(nameof(BoomboxItem.Start))]
        static void AddToList(BoomboxItem __instance)
        {
            UpgradeBus.Instance.boomBoxes.Add(__instance);
        }
    }
}
    