using GameNetcodeStuff;
using HarmonyLib;
using MoreShipUpgrades.Extensions;
using MoreShipUpgrades.Misc.Upgrades;
using MoreShipUpgrades.UpgradeComponents.OneTimeUpgrades.Items;

namespace MoreShipUpgrades.Patches.Items
{
    [HarmonyPatch(typeof(WalkieTalkie))]
    internal static class WalkiePatcher
    {
        [HarmonyPrefix]
        [HarmonyPatch(nameof(WalkieTalkie.DiscardItem))]
        static void DisableHUD(WalkieTalkie __instance)
        {
            if (!BaseUpgrade.GetActiveUpgrade(WalkieGPS.UPGRADE_NAME) || !__instance.playerHeldBy.IsOwner) { return; }
            PlayerControllerB holdingPlayer = __instance.playerHeldBy;
            if (holdingPlayer.ContainsItemOfType(__instance)) return;
            WalkieGPS.instance.WalkieDeactivate();
        }

        [HarmonyPrefix]
        [HarmonyPatch(nameof(WalkieTalkie.EquipItem))]
        static void EnableHUD(WalkieTalkie __instance)
        {
            if (!BaseUpgrade.GetActiveUpgrade(WalkieGPS.UPGRADE_NAME) || !__instance.playerHeldBy.IsOwner) { return; }
            WalkieGPS.instance.WalkieActive();
        }
    }

}
