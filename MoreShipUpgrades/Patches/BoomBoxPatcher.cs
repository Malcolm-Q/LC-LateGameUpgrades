using HarmonyLib;
using MoreShipUpgrades.Managers;

namespace MoreShipUpgrades.Patches
{
    [HarmonyPatch(typeof(BoomboxItem))]
    internal class BoomBoxPatcher
    {
        [HarmonyPostfix]
        [HarmonyPatch(nameof(BoomboxItem.Start))]
        private static void AddToList(BoomboxItem __instance)
        {
            UpgradeBus.instance.boomBoxes.Add(__instance);
        }
    }
}
