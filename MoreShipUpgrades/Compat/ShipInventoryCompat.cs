using HarmonyLib;
using MoreShipUpgrades.Misc.Util;
using MoreShipUpgrades.UpgradeComponents.TierUpgrades.Items;
using ShipInventory.Patches;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;

namespace MoreShipUpgrades.Compat
{
    internal static class ShipInventoryCompat
    {
        public static bool Enabled =>
            BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey("ShipInventoryUpdated");
    }

    [HarmonyPatch(typeof(RoundManager_Patches))]
    internal static class RoundManagerPatchesPatcher // lol
    {
        [HarmonyPatch(nameof(RoundManager_Patches.ClearInventory))]
        [HarmonyTranspiler]
        [HarmonyDebug]
        static IEnumerable<CodeInstruction> ClearInventoryTranspiler(IEnumerable<CodeInstruction> instructions)
        {
            MethodInfo keepScrapChance = typeof(ScrapKeeper).GetMethod(nameof(ScrapKeeper.ComputeScrapKeeperKeepScrapChance));

            List<CodeInstruction> codes = new(instructions);
            int index = 0;
            Tools.FindLocalField(ref index, ref codes, localIndex: 1, addCode: keepScrapChance, errorMessage: "Couldn't find the keep rate of the ship inventory");
            codes.Insert(index+1, new CodeInstruction(opcode: OpCodes.Add));
            return codes;
        }
    }
}
