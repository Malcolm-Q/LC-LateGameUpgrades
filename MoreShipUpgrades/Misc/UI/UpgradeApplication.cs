using MoreShipUpgrades.Misc.TerminalNodes;
using System.Text;
using UnityEngine;

namespace MoreShipUpgrades.Misc.UI
{
    internal class UpgradeApplication
    {
        internal const int AVAILABLE_CHARACTERS_PER_LINE = 51;
        internal const int LEVEL_DISPLAY_INDEX = 30;
        internal const char TOP_LEFT_CORNER = '╭';
        internal const char TOP_RIGHT_CORNER = '╮';
        internal const char BOTTOM_LEFT_CORNER = '╰';
        internal const char BOTTOM_RIGHT_CORNER = '╯';
        internal const char VERTICAL_LINE = '│';
        internal const char HORIZONTAL_LINE = '─';
        internal const char WHITE_SPACE = ' ';
        internal const char CURSOR = '>';

        internal const char EMPTY_LEVEL = '○';
        internal const char FILLED_LEVEL = '●';

        int cursorIndex;
        int pageIndex;
        readonly CustomTerminalNode[][] pages;
        public string GetText()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine().AppendLine(); // To not appear at the top where credits are shown (for now, at least)
            sb.Append(TOP_LEFT_CORNER).Append(new string(HORIZONTAL_LINE, AVAILABLE_CHARACTERS_PER_LINE-2)).Append(TOP_RIGHT_CORNER).AppendLine();
            CustomTerminalNode[] textElements = pages[pageIndex];
            for (int i = 0; i < textElements.Length; i++)
            {
                sb.Append(VERTICAL_LINE).Append(WHITE_SPACE);
                CustomTerminalNode node = textElements[i];
                if (node == null) break;
                string text = node.Name;
                int currentLevel = node.Unlocked ? node.CurrentUpgrade + 1 : 0;
                int remainingLevels = node.Unlocked ? 0 : 1;
                remainingLevels += node.MaxUpgrade != 0 ? node.MaxUpgrade - node.CurrentUpgrade : 0;
                string levels = new string(FILLED_LEVEL, currentLevel) + new string(EMPTY_LEVEL, remainingLevels);
                if (i == cursorIndex) sb.Append(CURSOR).Append(WHITE_SPACE);
                sb.Append(text).Append(new string(WHITE_SPACE, LEVEL_DISPLAY_INDEX - 3 - text.Length - (i == cursorIndex ? 2 : 0))).Append(levels).Append(new string(WHITE_SPACE, AVAILABLE_CHARACTERS_PER_LINE - LEVEL_DISPLAY_INDEX - levels.Length)).Append(VERTICAL_LINE).AppendLine();

            }
            sb.Append(BOTTOM_LEFT_CORNER).Append(new string(HORIZONTAL_LINE, AVAILABLE_CHARACTERS_PER_LINE - 2)).Append(BOTTOM_RIGHT_CORNER).AppendLine();
            sb.AppendLine().AppendLine();
            return sb.ToString();
        }
        public void PageUp()
        {
            pageIndex = (pageIndex + 1) % (pages.Length);
            ResetCursor();
        }
        public void PageDown()
        {
            pageIndex--;
            if (pageIndex < 0) pageIndex = pages.Length - 1;
            ResetCursor();
        }
        public void ResetPage()
        {
            pageIndex = 0;
        }
        public void Forward()
        {
            cursorIndex = (cursorIndex + 1) % (pages[pageIndex].Length);
        }
        public void Backward()
        {
            cursorIndex--;
            if (cursorIndex < 0) cursorIndex = pages[pageIndex].Length-1;
        }

        public void ResetCursor()
        {
            cursorIndex = 0;
        }

        public UpgradeApplication(CustomTerminalNode[] terminalNodes)
        {
            cursorIndex = 0;
            pageIndex = 0;
            int lengthPerPage = terminalNodes.Length / 2;
            int amountPages = Mathf.CeilToInt((float)terminalNodes.Length / lengthPerPage);
            pages = new CustomTerminalNode[amountPages][];
            for (int i = 0; i < amountPages - 1; i++)
                pages[i] = new CustomTerminalNode[lengthPerPage];
            pages[amountPages - 1] = new CustomTerminalNode[terminalNodes.Length % lengthPerPage];
            for(int i  = 0; i < terminalNodes.Length; i++)
            {
                int row = i / lengthPerPage;
                int col = i % lengthPerPage;
                pages[row][col] = terminalNodes[i];
            }
        }
    }
}
