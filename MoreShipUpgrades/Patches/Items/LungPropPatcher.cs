using HarmonyLib;
using MoreShipUpgrades.Managers;
using MoreShipUpgrades.UpgradeComponents.TierUpgrades.Ship;

namespace MoreShipUpgrades.Patches.Items
{
	[HarmonyPatch(typeof(LungProp))]
	internal static class LungPropPatcher
	{
		[HarmonyPatch(nameof(LungProp.Start))]
		[HarmonyPostfix]
		static void StartPostfix(LungProp __instance)
		{
			if (!UpgradeBus.Instance.PluginConfiguration.AffectApparatus)
			{
				Plugin.mls.LogDebug("Midas Touch affecting apparatus is disabled, skipping...");
				return;
			}
			int scrapValue = __instance.scrapValue;
			Plugin.mls.LogDebug($"Midas Touch affecting apparatus is enabled, increasing the original scrap value of apparatus ({scrapValue})...");
			scrapValue = MidasTouch.IncreaseScrapValueInteger(scrapValue);
			__instance.SetScrapValue(scrapValue);
			Plugin.mls.LogDebug($"Set new scrap value of apparatus to {scrapValue}...");
		}
	}
}
