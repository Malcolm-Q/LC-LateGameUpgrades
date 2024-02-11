using MoreShipUpgrades.Managers;

namespace MoreShipUpgrades.Misc.TerminalNodes
{
    /// <summary>
    /// Handler that changes the contents of the "help" terminal node
    /// </summary>
    internal class HelpTerminalNode
    {
        private static int startingIndex = -1;
        private static int endingIndex = -1;
        private const string EXTEND_HELP_COMMAND = ">EXTEND DEADLINE <DAYS>\nExtends the deadline by specified amount. Consumes {0} for each day extended.\n\n";
        private const string INTERNS_HELP_COMMAND = ">INTERNS / INTERN\nRevives the selected player in radar with a new employee. Consumes {0} credits for each revive.\n\n";

        private const string CONTRACT_HELP_COMMAND = ">CONTRACT [moon]\nGives you a random contract for a scrap item with considerable value and lasts til you leave from assigned planet.\nConsumes {0} credits for each contract and will be unable to get another contract til current has expired.\nIf a moon is specified, it will generate a contract for that moon for the cost of {1} Company credits instead.\n\n";
        private const string ATK_HELP_COMMAND = ">ATK / INITATTACK\nStuns nearby enemies for a set period of time. Only applicable when Discombobulator has been purchased\n\n";
        private const string CD_HELP_COMMAND = ">CD / COOLDOWN\nShows the current cooldown of the ship stun ability. Only applicable when Discombobulator has been purchased\n\n";
        private const string SCRAP_INSURANCE_COMMAND = ">SCRAP INSURANCE\nActivates an insurance policy on scrap stored in the ship incase of a team wipe occurs.\nCan only be bought while in orbit and will only apply in the next moon land after purchase.\nConsumes {0} credits for each activation of insurance.\n\n";
        private const string INFO_CONTRACT_HELP_COMMAND = ">CONTRACT INFO\nDisplays all information related to each contract and how to complete it.\n\n";

        /// <summary>
        /// Prepares the "help" terminal node to add information related to LGU-related commands such as "Scrap Insurance", "Interns", etc.
        /// </summary>
        internal static void SetupLGUHelpCommand()
        {
            TerminalNode helpNode = LGUTerminalNode.GetHelpTerminalNode();
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
        /// <summary>
        /// Adds information related to the "Scrap Insurance" command to the given terminal node
        /// </summary>
        /// <param name="helpNode">Terminal node we wish to add the information on</param>
        static void HandleHelpScrapInsurance(ref TerminalNode helpNode)
        {
            LGUTerminalNode.AddTextToNode(ref helpNode, string.Format(SCRAP_INSURANCE_COMMAND, UpgradeBus.instance.cfg.SCRAP_INSURANCE_PRICE.Value), UpgradeBus.instance.cfg.SCRAP_INSURANCE_ENABLED.Value);
        }
        /// <summary>
        /// Adds information related to the Discombobulator's commands to the given terminal node
        /// </summary>
        /// <param name="helpNode">Terminal node we wish to add the information on</param>
        static void HandleHelpDiscombobulator(ref TerminalNode helpNode)
        {
            bool enabled = UpgradeBus.instance.cfg.DISCOMBOBULATOR_ENABLED.Value;
            LGUTerminalNode.AddTextToNode(ref helpNode, ATK_HELP_COMMAND, enabled);
            LGUTerminalNode.AddTextToNode(ref helpNode, CD_HELP_COMMAND, enabled);
        }
        /// <summary>
        /// Adds information related to the contract's commands to the given terminal node
        /// </summary>
        /// <param name="helpNode">Terminal node we wish to add the information on</param>
        static void HandleHelpContract(ref TerminalNode helpNode)
        {
            bool enabled = UpgradeBus.instance.cfg.CONTRACTS_ENABLED.Value;
            LGUTerminalNode.AddTextToNode(ref helpNode, string.Format(CONTRACT_HELP_COMMAND, UpgradeBus.instance.cfg.CONTRACT_PRICE.Value, UpgradeBus.instance.cfg.CONTRACT_SPECIFY_PRICE.Value), enabled);
            LGUTerminalNode.AddTextToNode(ref helpNode, INFO_CONTRACT_HELP_COMMAND, enabled);
        }
        /// <summary>
        /// Adds information related to the intern's command to the given terminal node
        /// </summary>
        /// <param name="helpNode">Terminal node we wish to add the information on</param>
        static void HandleHelpInterns(ref TerminalNode helpNode)
        {
            LGUTerminalNode.AddTextToNode(ref helpNode, string.Format(INTERNS_HELP_COMMAND, UpgradeBus.instance.cfg.INTERN_PRICE.Value), UpgradeBus.instance.cfg.INTERN_ENABLED.Value);
        }
        /// <summary>
        /// Adds information related to the Extend Deadline's command to the given terminal node
        /// </summary>
        /// <param name="helpNode">Terminal node we wish to add the information on</param>
        static void HandleHelpExtendDeadline(ref TerminalNode helpNode)
        {
            LGUTerminalNode.AddTextToNode(ref helpNode, string.Format(EXTEND_HELP_COMMAND, UpgradeBus.instance.cfg.EXTEND_DEADLINE_PRICE.Value), UpgradeBus.instance.cfg.EXTEND_DEADLINE_ENABLED.Value);
        }
    }
}
