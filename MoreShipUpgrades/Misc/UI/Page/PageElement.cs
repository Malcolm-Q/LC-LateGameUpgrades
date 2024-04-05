using MoreShipUpgrades.Misc.UI.Screen;
using MoreShipUpgrades.Misc.Util;
using System;
using System.Text;

namespace MoreShipUpgrades.Misc.UI.Page
{
    internal class PageElement : ITextElement
    {
        internal int pageIndex;
        internal IScreen[] elements;

        public string GetText(int availableLength)
        {
            IScreen selectedScreen = elements[pageIndex];
            StringBuilder sb = new StringBuilder();
            sb.Append(selectedScreen.GetText(availableLength));
            sb.Append(new string(LGUConstants.WHITE_SPACE, availableLength - LGUConstants.START_PAGE_COUNTER))
                .Append($"Page {pageIndex + 1}/{elements.Length}");

            return sb.ToString();
        }
        public void PageUp()
        {
            pageIndex = (pageIndex + 1) % (elements.Length);
        }
        public void PageDown()
        {
            pageIndex--;
            if (pageIndex < 0) pageIndex = elements.Length - 1;
        }
        public void ResetPage()
        {
            pageIndex = 0;
        }
        public IScreen GetCurrentScreen()
        {
            return elements[pageIndex];
        }
    }
}
