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
            if (ContractManager.Instance.contractType != "Exterminator") return;

            if (ContractManager.Instance.contractLevel == RoundManager.Instance.currentLevel.PlanetName)
            {
                __result = true;
            }
        }
    }

}
