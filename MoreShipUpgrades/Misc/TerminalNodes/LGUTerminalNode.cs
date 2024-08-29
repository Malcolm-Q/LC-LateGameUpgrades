using MoreShipUpgrades.Managers;
using System.Collections.Generic;

namespace MoreShipUpgrades.Misc.TerminalNodes
{
    /// <summary>
    /// Handler for Terminal nodes stored in the terminal for customization when required
    /// Later on, this can also be used for when we decide to store our own TerminalNodes into the terminal
    /// instead of generating new terminal nodes left and right through CommandParser
    /// </summary>
    internal static class LguTerminalNode
    {
        static Terminal terminal;
        /// <summary>
        /// Handler's logger
        /// </summary>
        static readonly LguLogger logger = new LguLogger(nameof(LguTerminalNode));
        /// <summary>
        /// Dictionary used as cache to optimize unnecessary searches when already found the reference.
        /// </summary>
        static readonly Dictionary<string, TerminalNode> retrievedTerminalNodes = new Dictionary<string, TerminalNode>();

        const string HELP_NOUN = "help";
        public static Terminal GetTerminal()
        {
            if (terminal == null) terminal = UpgradeBus.Instance.GetTerminal();
            return terminal;
        }
        /// <summary>
        /// Finds the associated TerminalKeyword stored in the terminal which has the requested word
        /// </summary>
        /// <param name="word">Word used to prompt a terminal node</param>
        /// <returns>Associated keyword if it's stored in the terminal, null if it doesn't exist</returns>
        internal static TerminalKeyword FindTerminalKeyword(string word)
        {
            TerminalKeyword terminalKeyword = GetTerminal().CheckForExactSentences(word);
            if (terminalKeyword == null) logger.LogError($"Couldn't find terminal node for the word \"{word}\"");
            return terminalKeyword;
        }
        /// <summary>
        /// Retrieves the TerminalNode associated with provided special keyword
        /// </summary>
        /// <param name="word">Word used to prompt the special TerminalNode</param>
        /// <returns>Associated TerminalNode if it's stored in the terminal, null if it doesn't exist</returns>
        internal static TerminalNode GetSpecialTerminalNodeByWord(string word)
        {
            TerminalNode result = retrievedTerminalNodes.GetValueOrDefault(word, null);
            if (result != null) return result;
            result = FindTerminalKeyword(word).specialKeywordResult;
            if (!retrievedTerminalNodes.ContainsKey(word)) retrievedTerminalNodes[word] = result;
            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns>Retrieves the TerminalNode associated with the keyword "help"</returns>
        internal static TerminalNode GetHelpTerminalNode()
        {
            return GetSpecialTerminalNodeByWord(HELP_NOUN);
        }
    }
}
