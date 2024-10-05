using GameNetcodeStuff;
using HarmonyLib;
using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc;
using MoreShipUpgrades.UpgradeComponents.Commands;

namespace MoreShipUpgrades.Patches.Interactables
{
    [HarmonyPatch(typeof(EntranceTeleport))]
    internal static class EntranceTeleportPatcher
    {
        [HarmonyPatch(nameof(EntranceTeleport.TeleportPlayerClientRpc))]
        [HarmonyPostfix]
        static void TeleportPlayerClientRpcPostfix(int playerObj)
        {
            LategameConfiguration config = UpgradeBus.Instance.PluginConfiguration;
            if (!config.INTERN_ENABLED || config.INTERNS_TELEPORT_RESTRICTION != Interns.TeleportRestriction.ExitBuilding) return;
            PlayerControllerB player = StartOfRound.Instance.allPlayerScripts[playerObj];
            Interns.instance.RemoveRecentlyInterned(player);
        }
    }
}
