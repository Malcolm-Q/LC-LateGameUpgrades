using MoreShipUpgrades.Misc.UI.Cursor;
using MoreShipUpgrades.Misc.UI.Screen;

namespace MoreShipUpgrades.Misc.UI.Page
{
    internal class PageCursorElement : PageElement
    {
        internal CursorMenu[] cursorMenus;
        public CursorMenu GetCurrentCursorMenu()
        {
            return cursorMenus[pageIndex];
        }

        public static PageCursorElement Create(int startingPageIndex = 0, IScreen[] elements = default, CursorMenu[] cursorMenus = default)
        {
            return new PageCursorElement()
            {
                pageIndex = startingPageIndex,
                elements = elements,
                cursorMenus = cursorMenus
            };
        }
    }
}
