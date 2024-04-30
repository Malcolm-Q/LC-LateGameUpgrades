using MoreShipUpgrades.Input;
using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc.UI.Cursor;
using MoreShipUpgrades.Misc.UI.Page;
using MoreShipUpgrades.Misc.UI.Screen;
using MoreShipUpgrades.Misc.Util;
using System;

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
        protected void Confirm(string title, string description, Action confirmAction, Action declineAction, string additionalMessage = "")
        {
            CursorMenu cursorMenu = new CursorMenu()
            {
                elements =
                [
                    new CursorElement()
                    {
                        Name = LGUConstants.CONFIRM_PROMPT,
                        Description = "",
                        Action = () => { confirmAction(); }
                    },
                    new CursorElement()
                    {
                        Name = LGUConstants.CANCEL_PROMPT,
                        Description = "",
                        Action = () => { declineAction(); }
                    }
                ]
            };

            IScreen screen = new BoxedScreen()
            {
                Title = title,
                elements =
                [
                    new TextElement()
                    {
                        Text = description
                    },
                    new TextElement()
                    {
                        Text = " "
                    },
                    new TextElement()
                    {
                        Text = additionalMessage
                    },
                    cursorMenu
                ]
            };
            SwitchScreen(screen, cursorMenu, false, false);
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
