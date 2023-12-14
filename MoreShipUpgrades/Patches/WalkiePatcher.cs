using HarmonyLib;
using MoreShipUpgrades.Managers;
using MoreShipUpgrades.UpgradeComponents;
using System.Security.Cryptography;
using UnityEngine;

namespace MoreShipUpgrades.Patches
{
    [HarmonyPatch(typeof(WalkieTalkie))]
    internal class WalkiePatcher
    {
        [HarmonyPrefix]
        [HarmonyPatch("PocketItem")]
        private static void DisableHUD(WalkieTalkie __instance)
        {
            if (!UpgradeBus.instance.walkies || !__instance.playerHeldBy.IsOwner){ return; }
            UpgradeBus.instance.walkieHandler.WalkieDeactivate();
        }

        [HarmonyPrefix]
        [HarmonyPatch("DiscardItem")]
        private static void DisableHUDDiscard(WalkieTalkie __instance)
        {
            if (!UpgradeBus.instance.walkies || !__instance.playerHeldBy.IsOwner){ return; }
            UpgradeBus.instance.walkieHandler.WalkieDeactivate();
        }


        [HarmonyPrefix]
        [HarmonyPatch("EquipItem")]
        private static void EnableHUD(WalkieTalkie __instance)
        {
            if (!UpgradeBus.instance.walkies || !__instance.playerHeldBy.IsOwner){ return; }
            UpgradeBus.instance.walkieHandler.WalkieActive();
        }
    }

}
