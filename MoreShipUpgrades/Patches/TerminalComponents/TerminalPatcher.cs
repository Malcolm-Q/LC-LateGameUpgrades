using HarmonyLib;
using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc;

namespace MoreShipUpgrades.Patches.TerminalComponents
{
    [HarmonyPatch(typeof(Terminal))]
    internal class TerminalPatcher
    {
        private static LGULogger logger = new LGULogger(nameof(TerminalPatcher));
        private static int HELP_TERMINAL_NODE = 13;
        private static int startingIndex = -1;
        private static int endingIndex = -1;
        private const string EXTEND_HELP_COMMAND = ">EXTEND DEADLINE <DAYS>\nExtends the deadline by specified amount. Consumes {0} for each day extended.\n\n";
        private const string INTERNS_HELP_COMMAND = ">INTERNS / INTERN \nRevives the selected player in radar with a new employee. Consumes {0} credits for each revive.\n\n";

        private const string CONTRACT_HELP_COMMAND = ">CONTRACT [moon]\nGives you a random contract for a scrap item with considerable value and lasts til you leave from assigned planet.\nConsumes {0} credits for each contract and will be unable to get another contract til current has expired.\nIf a moon is specified, it will generate a contract for that moon for the cost of {1} Company credits instead.\n\n";
        private const string ATK_HELP_COMMAND = ">ATK / INITATTACK \nStuns nearby enemies for a set period of time. Only applicable when Discombobulator has been purchased\n\n";
        private const string CD_HELP_COMMAND = ">CD / COOLDOWN \nShows the current cooldown of the ship stun ability. Only applicable when Discombobulator has been purchased\n\n";
        private const string SCRAP_INSURANCE_COMMAND = "> SCRAP INSURANCE \nActivates an insurance policy on scrap stored in the ship incase of a team wipe occurs.\nCan only be bought while in orbit and will only apply in the next moon land after purchase.\nConsumes {0} credits for each activation of insurance.\n\n";
        private const string INFO_CONTRACT_HELP_COMMAND = ">CONTRACT INFO \nDisplays all information related to each contract and how to complete it.\n\n";

        [HarmonyPostfix]
        [HarmonyPatch(nameof(Terminal.Start))]
        private static void StartPostfix(ref Terminal __instance)
        {
            TerminalNode helpNode = __instance.terminalNodes.specialNodes[HELP_TERMINAL_NODE];
            if (startingIndex != -1 && endingIndex != -1) helpNode.displayText = helpNode.displayText.Remove(startingIndex, endingIndex - startingIndex);
            startingIndex = helpNode.displayText.Length;
            helpNode.displayText += ">LATEGAME\nDisplays information related with Lategame-Upgrades mod\n\n";
            helpNode.displayText += ">LGU / LATEGAME STORE\nDisplays the purchaseable upgrades from Lategame store.\n\n";
            HandleHelpExtendDeadline(ref helpNode);
            HandleHelpInterns(ref helpNode);
            HandleHelpContract(ref helpNode);
            HandleHelpDiscombobulator(ref helpNode);
            HandleHelpScrapInsurance(ref helpNode);
            endingIndex = helpNode.displayText.Length;
        }
        private static void HandleHelpCommand(ref TerminalNode helpNode, string helpText, bool enabled = true)
        {
            string text = helpNode.displayText;
            int index = text.IndexOf(helpText);
            if (index != -1 && !enabled)
            {
                text.Remove(index, helpText.Length);
                helpNode.displayText = text;
                return;
            }
            if (index == -1 && enabled)
            {
                text += helpText;
                helpNode.displayText = text;
                return;
            }
        }
        private static void HandleHelpScrapInsurance(ref TerminalNode helpNode)
        {
            HandleHelpCommand(ref helpNode, string.Format(SCRAP_INSURANCE_COMMAND, UpgradeBus.instance.cfg.SCRAP_INSURANCE_PRICE), UpgradeBus.instance.cfg.SCRAP_INSURANCE_ENABLED);
        }
        private static void HandleHelpDiscombobulator(ref TerminalNode helpNode)
        {
            bool enabled = UpgradeBus.instance.cfg.DISCOMBOBULATOR_ENABLED;
            HandleHelpCommand(ref helpNode, ATK_HELP_COMMAND, enabled);
            HandleHelpCommand(ref helpNode, CD_HELP_COMMAND, enabled);
        }
        private static void HandleHelpContract(ref TerminalNode helpNode)
        {
            bool enabled = UpgradeBus.instance.cfg.CONTRACTS_ENABLED;
            HandleHelpCommand(ref helpNode, string.Format(CONTRACT_HELP_COMMAND, UpgradeBus.instance.cfg.CONTRACT_PRICE, UpgradeBus.instance.cfg.CONTRACT_SPECIFY_PRICE), enabled);
            HandleHelpCommand(ref helpNode, INFO_CONTRACT_HELP_COMMAND, enabled);
        }
        private static void HandleHelpInterns(ref TerminalNode helpNode)
        {
            HandleHelpCommand(ref helpNode, string.Format(INTERNS_HELP_COMMAND, UpgradeBus.instance.cfg.INTERN_PRICE), UpgradeBus.instance.cfg.INTERN_ENABLED);
        }
        private static void HandleHelpExtendDeadline(ref TerminalNode helpNode)
        {
            HandleHelpCommand(ref helpNode, string.Format(EXTEND_HELP_COMMAND, UpgradeBus.instance.cfg.EXTEND_DEADLINE_PRICE), UpgradeBus.instance.cfg.EXTEND_DEADLINE_ENABLED);
        }
        [HarmonyPostfix]
        [HarmonyPatch(nameof(Terminal.ParsePlayerSentence))]
        private static void CustomParser(ref Terminal __instance, ref TerminalNode __result)
        {
            string text = __instance.screenText.text.Substring(__instance.screenText.text.Length - __instance.textAdded);
            CommandParser.ParseLGUCommands(text, ref __instance, ref __result);
        }
    }
}
