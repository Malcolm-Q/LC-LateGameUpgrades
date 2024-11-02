using HarmonyLib;
using MoreShipUpgrades.Misc.Util;
using MoreShipUpgrades.UpgradeComponents.TierUpgrades.Items;
using ShipInventory.Patches;
using System.Collections.Generic;
using System.Reflection;

namespace MoreShipUpgrades.Compat
{
    internal static class ShipInventoryCompat
    {
        public static bool Enabled =>
            BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey(ShipInventory.MyPluginInfo.PLUGIN_GUID);
    }

    [HarmonyPatch(typeof(RoundManager_Patches))]
    internal static class RoundManagerPatchesPatcher // lol
    {
        // Temporary bandaid til https://github.com/WarperSan/ShipInventory/issues/25 gets implemented for a better solution.
        [HarmonyPatch(nameof(RoundManager_Patches.ClearInventory))]
        [HarmonyTranspiler]
        static IEnumerable<CodeInstruction> ClearInventoryTranspiler(IEnumerable<CodeInstruction> instructions)
        {
            FieldInfo allPlayersDead = typeof(StartOfRound).GetField(nameof(StartOfRound.allPlayersDead));

            MethodInfo keepScrapChance = typeof(ScrapKeeper).GetMethod(nameof(ScrapKeeper.CanKeepScrapBasedOnChance));

            List<CodeInstruction> codes = new(instructions);
            int index = 0;
            Tools.FindField(ref index, ref codes, findField: allPlayersDead, addCode: keepScrapChance, andInstruction: true, notInstruction: true, errorMessage: "Couldn't find all players dead field");
            return codes;
        }
    }
}
