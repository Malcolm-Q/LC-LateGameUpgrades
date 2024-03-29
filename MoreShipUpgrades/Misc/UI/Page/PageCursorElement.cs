using MoreShipUpgrades.Misc.UI.Cursor;

namespace MoreShipUpgrades.Misc.UI.Page
{
    internal class PageCursorElement : PageElement
    {
        internal CursorMenu[] cursorMenus;
        public CursorMenu GetCurrentCursorMenu()
        {
            return cursorMenus[pageIndex];
        }
    }
}
