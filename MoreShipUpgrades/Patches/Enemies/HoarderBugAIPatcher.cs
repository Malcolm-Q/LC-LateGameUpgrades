using HarmonyLib;
using MoreShipUpgrades.Managers;

namespace MoreShipUpgrades.Patches.Enemies
{
    [HarmonyPatch(typeof(HoarderBugAI))]
    internal static class HoarderBugAIPatcher
    {
        [HarmonyPostfix]
        [HarmonyPatch(nameof(HoarderBugAI.IsHoarderBugAngry))]
        private static void MakeHoarderBugSwarmAngry(ref bool __result)
        {
            if (UpgradeBus.Instance.contractType != "exterminator") return;

            if (UpgradeBus.Instance.contractLevel == RoundManager.Instance.currentLevel.PlanetName)
            {
                __result = true;
            }
        }
    }

}
