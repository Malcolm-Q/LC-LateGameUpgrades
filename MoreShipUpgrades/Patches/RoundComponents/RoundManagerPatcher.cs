using HarmonyLib;
using System.Collections.Generic;
using System.Reflection;
using MoreShipUpgrades.Misc.Util;
using MoreShipUpgrades.UpgradeComponents.TierUpgrades.Ship;
using MoreShipUpgrades.Managers;
using MoreShipUpgrades.UpgradeComponents.TierUpgrades;
using MoreShipUpgrades.UpgradeComponents.TierUpgrades.Items;

namespace MoreShipUpgrades.Patches.RoundComponents
{
    [HarmonyPatch(typeof(RoundManager))]
    internal static class RoundManagerPatcher
    {
        [HarmonyPatch(nameof(RoundManager.SpawnScrapInLevel))]
        [HarmonyTranspiler]
        static IEnumerable<CodeInstruction> SpawnScrapInLevelTranspiler(IEnumerable<CodeInstruction> instructions)
        {
            FieldInfo minimumScrap = typeof(SelectableLevel).GetField(nameof(SelectableLevel.minScrap));
            FieldInfo maximumScrap = typeof(SelectableLevel).GetField(nameof(SelectableLevel.maxScrap));
            FieldInfo scrapValueMultiplier = typeof(RoundManager).GetField(nameof(RoundManager.scrapValueMultiplier));

            MethodInfo increaseScrapValueInt = typeof(MidasTouch).GetMethod(nameof(MidasTouch.IncreaseScrapValueInteger));
            MethodInfo increaseScrapValue = typeof(MidasTouch).GetMethod(nameof(MidasTouch.IncreaseScrapValue));
            MethodInfo increaseScrap = typeof(ScavengerInstincts).GetMethod(nameof(ScavengerInstincts.IncreaseScrapAmount));

            List<CodeInstruction> codes = new(instructions);
            int index = 0;
            Tools.FindField(ref index, ref codes, findField: minimumScrap, addCode: increaseScrap, errorMessage: "Couldn't find level's minimum scrap amount");
            Tools.FindField(ref index, ref codes, findField: maximumScrap, addCode: increaseScrap, errorMessage: "Couldn't find level's maximum scrap amount");
            Tools.FindField(ref index, ref codes, findField: scrapValueMultiplier, addCode: increaseScrapValue, errorMessage: "Couldn't find the round manager's scrap value multiplier");
            Tools.FindInteger(ref index, ref codes, findValue: 50, addCode: increaseScrapValueInt, errorMessage: "Couldn't find the minimum value of scrap value used when same item day happens");
            Tools.FindExplicitInteger(ref index, ref codes, findValue: 170, addCode: increaseScrapValueInt, errorMessage: "Couldn't find the maximum value of scrap value used when same item day happens");
            Tools.FindField(ref index, ref codes, findField: scrapValueMultiplier, addCode: increaseScrapValue, errorMessage: "Couldn't find the round manager's scrap value multiplier");
            return codes;
        }

        [HarmonyPatch(nameof(RoundManager.FinishGeneratingLevel))]
        [HarmonyPrefix]
        static void FinishGeneratingLevelPrefix()
        {
            if (!UpgradeBus.Instance.PluginConfiguration.LandingThrustersConfiguration.Enabled) return;
            if (!UpgradeBus.Instance.PluginConfiguration.LandingThrustersConfiguration.AffectLanding) return;

			Plugin.mls.LogDebug("Adding spped in FinishGeneratingLevel callback");
			StartOfRound.Instance.shipAnimator.speed *= LandingThrusters.GetLandingSpeedMultiplier();
        }

        [HarmonyPatch(nameof(RoundManager.DespawnPropsAtEndOfRound))]
        [HarmonyTranspiler]
        static IEnumerable<CodeInstruction> DespawnPropsAtEndOfRoundTranspiler(IEnumerable<CodeInstruction> instructions)
        {
            FieldInfo IsScrap = typeof(Item).GetField(nameof(Item.isScrap));

            MethodInfo CanKeepScrapBasedOnChance = typeof(ScrapKeeper).GetMethod(nameof(ScrapKeeper.CanKeepScrapBasedOnChance));

            List<CodeInstruction> codes = new(instructions);
            int index = 0;
            Tools.FindField(ref index, ref codes, findField: IsScrap, addCode: CanKeepScrapBasedOnChance, andInstruction: true, notInstruction: true);
            return codes;
        }
    }
}
