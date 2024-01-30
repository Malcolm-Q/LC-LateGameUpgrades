using HarmonyLib;
using MoreShipUpgrades.Managers;

namespace MoreShipUpgrades.Patches
{
    [HarmonyPatch(typeof(WalkieTalkie))]
    internal class WalkiePatcher
    {
        [HarmonyPrefix]
        [HarmonyPatch(nameof(WalkieTalkie.PocketItem))]
        private static void DisableHUD(WalkieTalkie __instance)
        {
            if (!UpgradeBus.instance.walkies || !__instance.playerHeldBy.IsOwner){ return; }
            UpgradeBus.instance.walkieHandler.WalkieDeactivate();
        }

        [HarmonyPrefix]
        [HarmonyPatch(nameof(WalkieTalkie.DiscardItem))]
        private static void DisableHUDDiscard(WalkieTalkie __instance)
        {
            if (!UpgradeBus.instance.walkies || !__instance.playerHeldBy.IsOwner){ return; }
            UpgradeBus.instance.walkieHandler.WalkieDeactivate();
        }


        [HarmonyPrefix]
        [HarmonyPatch(nameof(WalkieTalkie.EquipItem))]
        private static void EnableHUD(WalkieTalkie __instance)
        {
            if (!UpgradeBus.instance.walkies || !__instance.playerHeldBy.IsOwner){ return; }
            UpgradeBus.instance.walkieHandler.WalkieActive();
        }
    }

}
