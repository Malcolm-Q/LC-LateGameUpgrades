using CustomItemBehaviourLibrary.Misc;
using HarmonyLib;
using MoreShipUpgrades.UpgradeComponents.TierUpgrades;
using System.Collections.Generic;
using System.Reflection;

namespace MoreShipUpgrades.Compat
{
    internal static class LethalConstellationsCompat
    {
        public static bool Enabled
        {
            get
            {
                return BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey(LethalConstellations.Plugin.PluginInfo.PLUGIN_GUID);
            }
        }
    }

    [HarmonyPatch(typeof(LethalConstellations.PluginCore.LevelStuff))]
    internal static class LevelStuffPatcher
    {
        [HarmonyPatch(nameof(LethalConstellations.PluginCore.LevelStuff.GetConstPrice))]
        [HarmonyTranspiler]
        static IEnumerable<CodeInstruction> GetConstPriceTranspiler(IEnumerable<CodeInstruction> instructions)
        {
            FieldInfo constelPrice = typeof(LethalConstellations.PluginCore.ClassMapper).GetField(nameof(LethalConstellations.PluginCore.ClassMapper.constelPrice));
            MethodInfo efficientEnginesPriceReduction = typeof(EfficientEngines).GetMethod(nameof(EfficientEngines.GetDiscountedMoonPrice));

            int index = 0;
            List<CodeInstruction> codes = new(instructions);

            Tools.FindField(ref index, ref codes, findField: constelPrice, addCode: efficientEnginesPriceReduction, errorMessage: "Couldn't find the price of the constellations");
            return codes;
        }
    }
}
