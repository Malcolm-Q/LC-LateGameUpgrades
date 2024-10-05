using GameNetcodeStuff;
using HarmonyLib;
using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc;
using MoreShipUpgrades.UpgradeComponents.Commands;

namespace MoreShipUpgrades.Patches.Interactables
{
    [HarmonyPatch(typeof(ShipTeleporter))]
    internal static class ShipTeleporterPatcher
    {
        [HarmonyPatch(nameof(ShipTeleporter.beamUpPlayer), MethodType.Enumerator)]
        [HarmonyPrefix]
        static bool beamUpPlayerPrefix()
        {
            LategameConfiguration config = UpgradeBus.Instance.PluginConfiguration;
            if (!config.INTERN_ENABLED || config.INTERNS_TELEPORT_RESTRICTION == Interns.TeleportRestriction.None) return true;
            PlayerControllerB playerToBeamUp = StartOfRound.Instance.mapScreen.targetedPlayer;
            return !Interns.instance.ContainsRecentlyInterned(playerToBeamUp);
        }

        [HarmonyPatch(nameof(ShipTeleporter.Update))]
        [HarmonyPostfix]
        static void UpdatePostfix(ShipTeleporter __instance)
        {
            LategameConfiguration config = UpgradeBus.Instance.PluginConfiguration;
            if (!config.INTERN_ENABLED || config.INTERNS_TELEPORT_RESTRICTION == Interns.TeleportRestriction.None) return;
            PlayerControllerB playerToBeamUp = StartOfRound.Instance.mapScreen.targetedPlayer;
            __instance.buttonTrigger.interactable = !Interns.instance.ContainsRecentlyInterned(playerToBeamUp);
            if (!__instance.buttonTrigger.interactable)
            {
                __instance.buttonTrigger.disabledHoverTip = $"[Recent intern must {(config.INTERNS_TELEPORT_RESTRICTION == Interns.TeleportRestriction.ExitBuilding ? "exit the building" : "enter the ship")} first]";
            }
        }
    }
}
