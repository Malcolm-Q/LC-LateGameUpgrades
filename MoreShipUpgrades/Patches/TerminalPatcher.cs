using GameNetcodeStuff;
using HarmonyLib;
using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc;
using MoreShipUpgrades.UpgradeComponents;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using UnityEngine;

namespace MoreShipUpgrades.Patches
{
    [HarmonyPatch(typeof(Terminal))]
    internal class TerminalPatcher
    {
        private static int HELP_TERMINAL_NODE = 13;
        [HarmonyPostfix]
        [HarmonyPatch("Start")]
        private static void StartPostfix(ref Terminal __instance)
        {
            TerminalNode val2 = __instance.terminalNodes.specialNodes[HELP_TERMINAL_NODE];
            val2.displayText += "Lategame Upgrades\n";
            val2.displayText += ">LATEGAME\nDisplays information related with Lategame-Upgrades mod\n";
            val2.displayText += ">LGU/LATEGAME STORE\nDisplays the purchaseable upgrades from Lategame store.\n";
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
