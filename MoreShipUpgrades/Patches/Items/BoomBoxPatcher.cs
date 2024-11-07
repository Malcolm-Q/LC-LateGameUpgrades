using HarmonyLib;
using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc.Util;
using MoreShipUpgrades.UpgradeComponents.OneTimeUpgrades.Items;
using System.Collections.Generic;
using System.Reflection;

namespace MoreShipUpgrades.Patches.Items
{
    [HarmonyPatch(typeof(BoomboxItem))]
    internal static class BoomBoxPatcher
    {
        [HarmonyPrefix]
        [HarmonyPatch(nameof(BoomboxItem.StartMusic))]
        static void StartMusicPrefix(BoomboxItem __instance)
        {
            if (UpgradeBus.Instance.PluginConfiguration.BEATS_ENABLED.Value && !SickBeats.Instance.boomBoxes.Contains(__instance)) SickBeats.Instance.boomBoxes.Add(__instance);
        }

        [HarmonyTranspiler]
        [HarmonyPatch(nameof(BoomboxItem.Update))]
        static IEnumerable<CodeInstruction> UpdateTranspiler(IEnumerable<CodeInstruction> instructions)
        {
            MethodInfo MuteBoomboxToEnemies = typeof(SickBeats).GetMethod(nameof(SickBeats.MuteBoomboxToEnemies));

            List<CodeInstruction> codes = new(instructions);
            int index = 0;

            Tools.FindFloat(ref index, ref codes, findValue: 16f, addCode: MuteBoomboxToEnemies, errorMessage: "Couldn't find the sound range to attract nearby enemies");
            return codes;
        }
    }
}