using System;
using System.Collections.Generic;
using System.Text;

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
        }
        public void Backward()
        {
            cursorIndex--;
            if (cursorIndex < 0) cursorIndex = elements.Length - 1;
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
                if (i == cursorIndex) sb.Append(CURSOR).Append(MainUpgradeApplication.WHITE_SPACE);
                sb.Append(element.GetText(availableLength)); // TODO: Wrap text
            }
            return sb.ToString();
        }
    }
}
