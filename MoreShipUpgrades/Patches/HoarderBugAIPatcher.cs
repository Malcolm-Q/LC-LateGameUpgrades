using HarmonyLib;
using MoreShipUpgrades.Managers;

namespace MoreShipUpgrades.Patches
{
    [HarmonyPatch(typeof(HoarderBugAI))]
    internal class HoarderBugAIPatcher
    {
        [HarmonyPostfix]
        [HarmonyPatch("IsHoarderBugAngry")]
        private static void MakeHoarderBugSwarmAngry(ref bool __result)
        {
            if (UpgradeBus.instance.contractType != "exterminator") return;

            if(UpgradeBus.instance.contractLevel == RoundManager.Instance.currentLevel.PlanetName)
            {
                __result = true;
            }
        }
    }

}
