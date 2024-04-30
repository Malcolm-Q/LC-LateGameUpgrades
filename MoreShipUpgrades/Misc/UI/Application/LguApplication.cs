using MoreShipUpgrades.Input;
using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc.UI;
using MoreShipUpgrades.Misc.UI.Cursor;
using MoreShipUpgrades.Misc.UI.Page;
using MoreShipUpgrades.Misc.UI.Screen;
using MoreShipUpgrades.Misc.Util;
using System;
using System.Collections.Generic;
using System.Text;

namespace MoreShipUpgrades.Misc.UI.Application
{
    internal abstract class LguApplication
    {
        protected PageCursorElement MainPage;

        protected IScreen currentScreen;
        protected CursorMenu currentCursorMenu;
        protected readonly Terminal terminal = UpgradeBus.Instance.GetTerminal();

        public abstract void Initialization();

        internal void MoveCursorUp()
        {
            currentCursorMenu.Backward();
        }
        internal void MoveCursorDown()
        {
            currentCursorMenu.Forward();
        }
        internal void MovePageUp()
        {
            MainPage.PageUp();
            SwitchScreen(null, MainPage.GetCurrentCursorMenu(), false, true);
        }
        internal void MovePageDown()
        {
            MainPage.PageDown();
            SwitchScreen(null, MainPage.GetCurrentCursorMenu(), false, true);
        }
        public void Submit()
        {
            currentCursorMenu.Execute();
        }
        public void UpdateText()
        {
            string text = currentScreen != null ? currentScreen.GetText(LGUConstants.AVAILABLE_CHARACTERS_PER_LINE) : MainPage.GetText(LGUConstants.AVAILABLE_CHARACTERS_PER_LINE);
            terminal.screenText.text = text;
            terminal.currentText = text;
        }

        protected void SwitchScreen(IScreen screen, CursorMenu cursorMenu, bool previous, bool enablePage)
        {
            currentScreen = screen;
            currentCursorMenu = cursorMenu;
            if (!previous) cursorMenu.cursorIndex = 0;
            if (enablePage)
            {
                Keybinds.pageUpAction.performed += LguInteractiveTerminal.OnUpgradeStorePageUp;
                Keybinds.pageDownAction.performed += LguInteractiveTerminal.OnUpgradeStorePageDown;
            }
            else
            {
                Keybinds.pageUpAction.performed -= LguInteractiveTerminal.OnUpgradeStorePageUp;
                Keybinds.pageDownAction.performed -= LguInteractiveTerminal.OnUpgradeStorePageDown;
            }
        }
    }
}
