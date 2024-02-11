using HarmonyLib;
using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc;
using MoreShipUpgrades.Misc.TerminalNodes;
using MoreShipUpgrades.UpgradeComponents.OneTimeUpgrades;
using MoreShipUpgrades.UpgradeComponents.TierUpgrades;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;

namespace MoreShipUpgrades.Patches.TerminalComponents
{
    [HarmonyPatch(typeof(Terminal))]
    internal class TerminalPatcher
    {
        private static LGULogger logger = new LGULogger(nameof(TerminalPatcher));

        [HarmonyPostfix]
        [HarmonyPatch(nameof(Terminal.Start))]
        static void StartPostfix()
        {
            HelpTerminalNode.SetupLGUHelpCommand();
        }
        [HarmonyPostfix]
        [HarmonyPatch(nameof(Terminal.ParsePlayerSentence))]
        static void CustomParser(ref Terminal __instance, ref TerminalNode __result)
        {
            string text = __instance.screenText.text.Substring(__instance.screenText.text.Length - __instance.textAdded);
            CommandParser.ParseLGUCommands(text, ref __instance, ref __result);
        }

        [HarmonyTranspiler]
        [HarmonyPatch(nameof(Terminal.SetItemSales))]
        static IEnumerable<CodeInstruction> SetItemSalesTranspiler(IEnumerable<CodeInstruction> instructions)
        {
            MethodInfo bargainConnectionsAmount = typeof(BargainConnections).GetMethod(nameof(BargainConnections.GetBargainConnectionsAdditionalItems));
            MethodInfo lethalDealsGuaranteedItems = typeof(LethalDeals).GetMethod(nameof(LethalDeals.GetLethalDealsGuaranteedItems));
            MethodInfo guaranteedMinimumSale = typeof(MarketInfluence).GetMethod(nameof(MarketInfluence.GetGuaranteedPercentageSale));
            List<CodeInstruction> codes = new List<CodeInstruction>(instructions);
            int index = 0;
            index = Tools.FindInteger(index, ref codes, -10, addCode: lethalDealsGuaranteedItems, errorMessage: "Couldn't find negative value representing no sales");
            index = Tools.FindInteger(index, ref codes, 5, addCode: bargainConnectionsAmount, errorMessage: "Couldn't find first maximum amount of items to go on sale");
            index = Tools.FindInteger(index, ref codes, 5, addCode: bargainConnectionsAmount, errorMessage: "Couldn't find second maximum amount of items to go on sale");
            index = Tools.FindInteger(index, ref codes, 0, addCode: guaranteedMinimumSale, errorMessage: "Couldn't find minimum sale percentage");
            codes.Insert(index, new CodeInstruction(OpCodes.Ldloc_S, 4));
            return codes;
        }
        [HarmonyTranspiler]
        [HarmonyPatch(nameof(Terminal.ParsePlayerSentence))]
        static IEnumerable<CodeInstruction> ParsePlayerSenteceTranspiler(IEnumerable<CodeInstruction> instructions)
        {
            MethodInfo newLimitCharactersTransmit = typeof(FastEncryption).GetMethod(nameof(FastEncryption.GetLimitOfCharactersTransmit));
            List<CodeInstruction> codes = new List<CodeInstruction>(instructions);
            int index = 0;
            index = Tools.FindInteger(index, ref codes, findValue: 10, skip: true, errorMessage: "Couldn't find the 10 value which is used as character limit");
            codes.Insert(index, new CodeInstruction(OpCodes.Call, newLimitCharactersTransmit));
            codes.Insert(index, new CodeInstruction(OpCodes.Ldloc_S, 12));
            return codes;
        }
    }
}
