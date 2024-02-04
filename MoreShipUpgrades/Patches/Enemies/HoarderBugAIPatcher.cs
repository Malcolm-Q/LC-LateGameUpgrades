using HarmonyLib;
using MoreShipUpgrades.Managers;

namespace MoreShipUpgrades.Patches.Enemies
{
    [HarmonyPatch(typeof(HoarderBugAI))]
    internal class HoarderBugAIPatcher
    {
        [HarmonyPostfix]
        [HarmonyPatch(nameof(HoarderBugAI.IsHoarderBugAngry))]
        private static void MakeHoarderBugSwarmAngry(ref bool __result)
        {
            if (UpgradeBus.instance.contractType != "exterminator") return;

            if (UpgradeBus.instance.contractLevel == RoundManager.Instance.currentLevel.PlanetName)
            {
                __result = true;
            }
        }
    }

}
