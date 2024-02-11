using MoreShipUpgrades.Managers;
using System.Collections.Generic;

namespace MoreShipUpgrades.Misc.TerminalNodes
{
    /// <summary>
    /// Handler for Terminal nodes stored in the terminal for customization when required
    /// Later on, this can also be used for when we decide to store our own TerminalNodes into the terminal
    /// instead of generating new terminal nodes left and right through CommandParser
    /// </summary>
    internal class LGUTerminalNode
    {
        static Terminal terminal;
        /// <summary>
        /// Handler's logger
        /// </summary>
        static readonly LGULogger logger = new LGULogger(nameof(LGUTerminalNode));
        /// <summary>
        /// Dictionary used as cache to optimize unnecessary searches when already found the reference.
        /// </summary>
        static readonly Dictionary<string, TerminalNode> retrievedTerminalNodes = new Dictionary<string, TerminalNode>();

        const string HELP_NOUN = "help";
        const string INFO_VERB = "info";
        const string SHOVEL_NOUN = "shovel";
        static Terminal GetTerminal()
        {
            if (terminal == null) terminal = UpgradeBus.instance.GetTerminal();
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
        /// Retrieves the TerminalNode used to display info of the requested item designated through the given word
        /// </summary>
        /// <param name="word">Name of the item to gather the info terminal node</param>
        /// <returns>The info TerminalNode associated to the item if it's stored in the terminal, null if it doesn't exist</returns>
        internal static TerminalNode GetItemInfoTerminalNode(string word)
        {
            TerminalNode result = retrievedTerminalNodes.GetValueOrDefault(word, null);
            if (result != null) return result;
            TerminalKeyword infoKeyword = GetTerminal().ParseWord(INFO_VERB);
            for (int i = 0; i < infoKeyword.compatibleNouns.Length && result == null; i++)
            {
                if (infoKeyword.compatibleNouns[i].noun.word != word) continue;
                result = infoKeyword.compatibleNouns[i].result;
            }
            if (result == null) logger.LogError($"Couldn't find info terminal node for item \"{word}\"");
            if (!retrievedTerminalNodes.ContainsKey(word)) retrievedTerminalNodes[word] = result;
            return result;
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
        /// Adds or remove additional text from the given terminal node if a given feature is enabled or not
        /// </summary>
        /// <param name="node">Terminal node which we want to change the text of</param>
        /// <param name="insertText">The additional text we wish to add to the node</param>
        /// <param name="enabled">Feature associated with the additional text is enabled or not</param>
        internal static void AddTextToNode(ref TerminalNode node, string insertText, bool enabled = true)
        {
            string text = node.displayText;
            int index = text.IndexOf(insertText);
            if (index != -1 && !enabled)
            {
                text.Remove(index, insertText.Length);
                node.displayText = text;
                return;
            }
            if (index == -1 && enabled)
            {
                text += insertText;
                node.displayText = text;
                return;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns>Retrieves the TerminalNode associated with the keyword "help"</returns>
        internal static TerminalNode GetHelpTerminalNode()
        {
            return GetSpecialTerminalNodeByWord(HELP_NOUN);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns>Retrieves the TerminalNode associated with the keywords "info shovel"</returns>
        internal static TerminalNode GetInfoShovelTerminalNode()
        {
            return GetItemInfoTerminalNode(SHOVEL_NOUN);
        }
    }
}
