using MoreShipUpgrades.Managers;

namespace MoreShipUpgrades.Misc.TerminalNodes
{
    /// <summary>
    /// Handler that changes the contents of the "help" terminal node
    /// </summary>
    internal static class HelpTerminalNode
    {
        static int startingIndex = -1;
        static int endingIndex = -1;
        const string EXTEND_HELP_COMMAND = ">EXTEND DEADLINE <DAYS>\nExtends the deadline by specified amount. Consumes {0} for each day extended.\n\n";
        const string INTERNS_HELP_COMMAND = ">INTERNS / INTERN\nRevives the selected player in radar with a new employee. Consumes {0} credits for each revive.\n\n";
        const string WEATHER_PROBE_HELP_COMMAND = ">PROBE <MOON> [WEATHER]\nSends out a weather probe to the specified moon and changes its current weather.\nIt will cost {0} Company credits for a randomized weather selection or {1} Company Credits when specifying the weather on a given moon.\nIt cannot change to a weather which is not possible to happen on that moon.\n";

        const string CONTRACT_HELP_COMMAND = ">CONTRACT [moon]\nGives you a random contract for a scrap item with considerable value and lasts til you leave from assigned planet.\nConsumes {0} credits for each contract and will be unable to get another contract til current has expired.\nIf a moon is specified, it will generate a contract for that moon for the cost of {1} Company credits instead.\n\n";
        const string ATK_HELP_COMMAND = ">ATK / INITATTACK\nStuns nearby enemies for a set period of time. Only applicable when Discombobulator has been purchased\n\n";
        const string CD_HELP_COMMAND = ">CD / COOLDOWN\nShows the current cooldown of the ship stun ability. Only applicable when Discombobulator has been purchased\n\n";
        const string SCRAP_INSURANCE_COMMAND = ">SCRAP INSURANCE\nActivates an insurance policy on scrap stored in the ship incase of a team wipe occurs.\nCan only be bought while in orbit and will only apply in the next moon land after purchase.\nConsumes {0} credits for each activation of insurance.\n\n";
        const string INFO_CONTRACT_HELP_COMMAND = ">CONTRACT INFO\nDisplays all information related to each contract and how to complete it.\n\n";

        /// <summary>
        /// Prepares the "help" terminal node to add information related to LGU-related commands such as "Scrap Insurance", "Interns", etc.
        /// </summary>
        internal static void SetupLGUHelpCommand(TerminalNode helpNode = null)
        {
            if (helpNode == null)
            {
                helpNode = LguTerminalNode.GetHelpTerminalNode();
            }

            if (startingIndex != -1 && endingIndex != -1) helpNode.displayText = helpNode.displayText.Remove(startingIndex, endingIndex - startingIndex);
            startingIndex = helpNode.displayText.Length;
            helpNode.displayText += ">LATEGAME\nDisplays information related with Lategame-Upgrades mod\n\n";
            endingIndex = helpNode.displayText.Length;
        }
        public static string HandleHelpWeatherProbe()
        {
            if(UpgradeBus.Instance.PluginConfiguration.WEATHER_PROBE_ENABLED.Value)
            {
                return string.Format(WEATHER_PROBE_HELP_COMMAND, UpgradeBus.Instance.PluginConfiguration.WEATHER_PROBE_PRICE.Value, UpgradeBus.Instance.PluginConfiguration.WEATHER_PROBE_PICKED_WEATHER_PRICE.Value);
            }
            return "";
        }
        /// <summary>
        /// Adds information related to the "Scrap Insurance" command to the given terminal node
        /// </summary>
        public static string HandleHelpScrapInsurance()
        {
            if(UpgradeBus.Instance.PluginConfiguration.SCRAP_INSURANCE_ENABLED.Value)
            {
                return string.Format(SCRAP_INSURANCE_COMMAND, UpgradeBus.Instance.PluginConfiguration.SCRAP_INSURANCE_PRICE.Value);
            }
            return "";
        }
        /// <summary>
        /// Adds information related to the Discombobulator's commands to the given terminal node
        /// </summary>
        public static string HandleHelpDiscombobulator()
        {
            if(UpgradeBus.Instance.PluginConfiguration.DISCOMBOBULATOR_ENABLED.Value)
            {
                return ATK_HELP_COMMAND + CD_HELP_COMMAND;
            }
            return "";
        }
        /// <summary>
        /// Adds information related to the contract's commands to the given terminal node
        /// </summary>
        public static string HandleHelpContract()
        {
            if(UpgradeBus.Instance.PluginConfiguration.CONTRACTS_ENABLED.Value)
            {
                return string.Format(CONTRACT_HELP_COMMAND + INFO_CONTRACT_HELP_COMMAND, UpgradeBus.Instance.PluginConfiguration.CONTRACT_PRICE.Value, UpgradeBus.Instance.PluginConfiguration.CONTRACT_SPECIFY_PRICE.Value);
            }
            return "";
        }
        /// <summary>
        /// Adds information related to the intern's command to the given terminal node
        /// </summary>
        public static string HandleHelpInterns()
        {
            if(UpgradeBus.Instance.PluginConfiguration.INTERN_ENABLED.Value)
            {
                return string.Format(INTERNS_HELP_COMMAND, UpgradeBus.Instance.PluginConfiguration.INTERN_PRICE.Value);
            }
            return "";
        }
        /// <summary>
        /// Adds information related to the Extend Deadline's command to the given terminal node
        /// </summary>
        public static string HandleHelpExtendDeadline()
        {
            if(UpgradeBus.Instance.PluginConfiguration.EXTEND_DEADLINE_ENABLED.Value)
            {
                return string.Format(EXTEND_HELP_COMMAND, UpgradeBus.Instance.PluginConfiguration.EXTEND_DEADLINE_PRICE.Value);
            }
            return "";
        }
    }
}
