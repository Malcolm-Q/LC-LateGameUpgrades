using System;
using System.Collections.Generic;
using System.Text;
using MoreShipUpgrades.Misc.Util;

namespace MoreShipUpgrades.Misc.UI.Cursor
{
    internal class CursorMenu : ITextElement
    {
        internal const char CURSOR = '>';

        internal int cursorIndex;
        internal CursorElement[] elements;

        public void Execute()
        {
            elements[cursorIndex].Action();
        }
        public void Forward()
        {
            cursorIndex = (cursorIndex + 1) % (elements.Length);
            while (elements[cursorIndex] == null)
            {
                cursorIndex = (cursorIndex + 1) % (elements.Length);
            }
        }
        public void Backward()
        {
            cursorIndex--;
            if (cursorIndex < 0) cursorIndex = elements.Length - 1;
            while (elements[cursorIndex] == null)
            {
                cursorIndex--;
                if (cursorIndex < 0) cursorIndex = elements.Length - 1;
            }
        }

        public void ResetCursor()
        {
            cursorIndex = 0;
        }

        public string GetText(int availableLength)
        {
            StringBuilder sb = new StringBuilder();
            for(int i = 0; i < elements.Length; i++)
            {
                CursorElement element = elements[i];
                if (element == null) continue;
                if (i == cursorIndex) sb.Append(CURSOR).Append(Constants.WHITE_SPACE); else sb.Append(Constants.WHITE_SPACE).Append(Constants.WHITE_SPACE);
                string text = element.GetText(availableLength - 2);
                text = (i == cursorIndex ? string.Format(Constants.SELECTED_CURSOR_ELEMENT_FORMAT, Constants.DEFAULT_BACKGROUND_SELECTED_COLOR, Constants.DEFAULT_TEXT_SELECTED_COLOR, text) : text);
                sb.Append(Tools.WrapText(text, availableLength, leftPadding: "  ", rightPadding: "", false));
            }
            return sb.ToString();
        }
    }
}
