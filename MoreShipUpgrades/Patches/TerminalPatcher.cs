using HarmonyLib;
using MoreShipUpgrades.Misc;

namespace MoreShipUpgrades.Patches
{
    [HarmonyPatch(typeof(Terminal))]
    internal class TerminalPatcher
    {
        private static LGULogger logger = new LGULogger(nameof(TerminalPatcher));
        private static int HELP_TERMINAL_NODE = 13;
        [HarmonyPostfix]
        [HarmonyPatch("Start")]
        private static void StartPostfix(ref Terminal __instance)
        {
            logger.LogDebug("Start");
            TerminalNode helpNode = __instance.terminalNodes.specialNodes[HELP_TERMINAL_NODE];
            if (helpNode.displayText.Contains("Lategame Upgrades")) return;
            helpNode.displayText += ">LATEGAME\nDisplays information related with Lategame-Upgrades mod\n\n";
            helpNode.displayText += ">LGU / LATEGAME STORE\nDisplays the purchaseable upgrades from Lategame store.\n\n";
        }
        [HarmonyPostfix]
        [HarmonyPatch("ParsePlayerSentence")]
        private static void CustomParser(ref Terminal __instance, ref TerminalNode __result)
        {
            string text = __instance.screenText.text.Substring(__instance.screenText.text.Length - __instance.textAdded);
            CommandParser.ParseLGUCommands(text, ref __instance, ref __result);
        }
    }
}
