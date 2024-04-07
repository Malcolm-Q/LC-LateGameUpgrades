using HarmonyLib;
using MoreShipUpgrades.Misc;
using MoreShipUpgrades.Misc.TerminalNodes;
using MoreShipUpgrades.Misc.UI;
using MoreShipUpgrades.Misc.Util;
using MoreShipUpgrades.UpgradeComponents.OneTimeUpgrades;
using MoreShipUpgrades.UpgradeComponents.TierUpgrades;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

namespace MoreShipUpgrades.Patches.TerminalComponents
{
    [HarmonyPatch(typeof(Terminal))]
    internal static class TerminalPatcher
    {
        [HarmonyPostfix]
        [HarmonyPatch(nameof(Terminal.Start))]
        static void StartPostfix()
        {
            HelpTerminalNode.SetupLGUHelpCommand();
        }
        [HarmonyTranspiler]
        [HarmonyPatch(nameof(Terminal.Update))]
        static IEnumerable<CodeInstruction> UpdateTranspiler(IEnumerable<CodeInstruction> instructions)
        {
            MethodInfo LategameStoreBeingUsed = typeof(UpgradesStore).GetMethod(nameof(UpgradesStore.UpgradeStoreBeingUsed));
            MethodInfo wasPressed = typeof(ButtonControl).GetMethod("get_wasPressedThisFrame");
            List<CodeInstruction> codes = new List<CodeInstruction>(instructions);
            int index = 0;
            Tools.FindMethod(ref index, ref codes, wasPressed, LategameStoreBeingUsed, notInstruction: true, andInstruction: true, errorMessage: "Couldn't find field used to check if the terminal is being used");
            return codes;
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
            Tools.FindInteger(ref index, ref codes, -10, addCode: lethalDealsGuaranteedItems, errorMessage: "Couldn't find negative value representing no sales");
            Tools.FindInteger(ref index, ref codes, 5, addCode: bargainConnectionsAmount, errorMessage: "Couldn't find first maximum amount of items to go on sale");
            Tools.FindInteger(ref index, ref codes, 5, addCode: bargainConnectionsAmount, errorMessage: "Couldn't find second maximum amount of items to go on sale");
            Tools.FindInteger(ref index, ref codes, findValue: 0, skip: true);
            Tools.FindInteger(ref index, ref codes, findValue: 0, skip: true);
            Tools.FindInteger(ref index, ref codes, findValue: 0, skip: true);
            Tools.FindInteger(ref index, ref codes, findValue: 0, skip: true);
            Tools.FindInteger(ref index, ref codes, findValue: 0, skip: true);
            Tools.FindInteger(ref index, ref codes, findValue: 0, skip: true);
            Tools.FindInteger(ref index, ref codes, 0, addCode: guaranteedMinimumSale, errorMessage: "Couldn't find minimum sale percentage");
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
            Tools.FindInteger(ref index, ref codes, findValue: 10, skip: true, errorMessage: "Couldn't find the 10 value which is used as character limit");
            codes.Insert(index, new CodeInstruction(OpCodes.Call, newLimitCharactersTransmit));
            codes.Insert(index, new CodeInstruction(OpCodes.Ldloc_S, 12));
            return codes;
        }
        [HarmonyPostfix]
        [HarmonyPatch(nameof(Terminal.LoadNewNode))]
        static void LoadNewNodePostfix(Terminal __instance, TerminalNode node) 
        {
            if (node.buyRerouteToMoon != -2) return;
            string toReplace = EfficientEngines.GetDiscountedMoonPrice(node.itemCost).ToString();
            __instance.screenText.text = __instance.currentText.Replace(node.itemCost.ToString(), toReplace);
            __instance.currentText = __instance.screenText.text;
        }
        [HarmonyTranspiler]
        [HarmonyPatch(nameof(Terminal.LoadNewNodeIfAffordable))]
        static IEnumerable<CodeInstruction> LoadNewNodeIfAffordableTranspiler(IEnumerable<CodeInstruction> instructions)
        {
            FieldInfo itemCost = typeof(TerminalNode).GetField(nameof(TerminalNode.itemCost));
            MethodInfo applyMoonDiscount = typeof(EfficientEngines).GetMethod(nameof(EfficientEngines.GetDiscountedMoonPrice));
            List<CodeInstruction> codes = new List<CodeInstruction>(instructions);
            int index = 0;
            Tools.FindField(ref index, ref codes, itemCost, skip: true, errorMessage: "Couldn't find the item cost applied to a specific item with index 7");
            Tools.FindField(ref index, ref codes, itemCost, addCode: applyMoonDiscount, errorMessage: "Couldn't find the item cost applied to moon routing");
            return codes;
        }
    }
}
