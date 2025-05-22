using MoreShipUpgrades.Managers;
using System;

namespace MoreShipUpgrades.UI.TerminalNodes
{
    /// <summary>
    /// Handler that changes the contents of the "help" terminal node
    /// </summary>
    internal static class HelpTerminalNode
    {
        static bool addedHelp = false;
        const string TRADING_HELP_COMMAND = ">LGU TRADE / TRADE PLAYER CREDITS / TRADE\n Allows trading Player Credits between players.\n\n";
        const string CONVERT_HELP_COMMAND = ">CONVERT / PC\nAllows converting Company Credits to Player Credits and vice versa.\n\n";
        const string INTERNS_HELP_COMMAND = ">INTERNS / INTERN\nRevives the selected player in radar with a new employee. Consumes {0} credits for each revive.\n\n";

        const string CONTRACT_HELP_COMMAND = ">CONTRACT [moon]\nGives you a random contract for a scrap item with considerable value and lasts til you leave from assigned planet.\nConsumes {0} credits for each contract and will be unable to get another contract til current has expired.\nIf a moon is specified, it will generate a contract for that moon for the cost of {1} Company credits instead.\n\n";
        const string ATK_HELP_COMMAND = ">ATK / INITATTACK\nStuns nearby enemies for a set period of time. Only applicable when Discombobulator has been purchased\n\n";
        const string CD_HELP_COMMAND = ">CD / COOLDOWN\nShows the current cooldown of the ship stun ability. Only applicable when Discombobulator has been purchased\n\n";
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
            if (addedHelp) return;
            helpNode.displayText += ">LATEGAME\nDisplays information related with Lategame-Upgrades mod\n\n";
            addedHelp = true;
        }
        /// <summary>
        /// Adds information related to the Discombobulator's commands to the given terminal node
        /// </summary>
        public static string HandleHelpDiscombobulator()
        {
            if (UpgradeBus.Instance.PluginConfiguration.DiscombobulatorUpgradeConfiguration.Enabled.Value)
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
            if (UpgradeBus.Instance.PluginConfiguration.CONTRACTS_ENABLED.Value)
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
            if (UpgradeBus.Instance.PluginConfiguration.INTERN_ENABLED.Value)
            {
                return string.Format(INTERNS_HELP_COMMAND, UpgradeBus.Instance.PluginConfiguration.INTERN_PRICE.Value);
            }
            return "";
        }

        internal static string HandleAlternateCurrency()
        {
            if (CurrencyManager.Enabled)
            {
                return CONVERT_HELP_COMMAND + TRADING_HELP_COMMAND;
            }
            return "";
        }
    }
}
