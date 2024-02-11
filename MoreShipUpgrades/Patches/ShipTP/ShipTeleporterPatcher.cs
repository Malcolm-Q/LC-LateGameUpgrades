using HarmonyLib;
using MoreShipUpgrades.Managers;
using MoreShipUpgrades.UpgradeComponents.TierUpgrades;

namespace MoreShipUpgrades.Patches.ShipTP {
	[HarmonyPatch(typeof(ShipTeleporter))]
	internal class ShipTeleporterPatcher {
		[HarmonyPrefix]
		[HarmonyPatch(nameof(ShipTeleporter.PressTeleportButtonClientRpc))]
		private static void getActivatedTeleporterType(ShipTeleporter __instance) {
			UpgradeBus.instance.mostRecentShipTPButtonPressed = __instance.isInverseTeleporter ? 2 : 1;
			UpgradeTeleportersScript.logger.LogDebug($"Setting most recently pressed teleporter button: {UpgradeBus.instance.mostRecentShipTPButtonPressed}");
		}
	}
}
