using MoreShipUpgrades.Input;
using MoreShipUpgrades.Misc.UI.Cursor;
using MoreShipUpgrades.Misc.UI.Page;
using MoreShipUpgrades.Misc.UI.Screen;
using MoreShipUpgrades.Misc.Util;
using System;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

namespace MoreShipUpgrades.Misc.UI.Application
{
    internal abstract class PageApplication : InteractiveTerminalApplication
    {
        protected PageCursorElement initialPage;
        protected PageCursorElement currentPage;
        protected override string GetApplicationText()
        {
            return currentScreen == currentPage.GetCurrentScreen() ? currentPage.GetText(LGUConstants.AVAILABLE_CHARACTERS_PER_LINE) : currentScreen.GetText(LGUConstants.AVAILABLE_CHARACTERS_PER_LINE);
        }
        protected int GetEntriesPerPage<T>(T[] entries)
        {
            return Mathf.CeilToInt(entries.Length / 2f);
        }
        protected override Action PreviousScreen()
        {
            return () => ResetScreen();
        }

        protected int GetAmountPages<T>(T[] entries)
        {
            return Mathf.CeilToInt(entries.Length / (float)GetEntriesPerPage(entries));
        }
        protected void ResetScreen()
        {
            SwitchScreen(initialPage, true);
        }
        protected void SwitchScreen(PageCursorElement pages, bool previous)
        {
            currentPage = pages;
            SwitchScreen(currentPage.GetCurrentScreen(), currentPage.GetCurrentCursorMenu(), previous);
        }
        protected override void SwitchScreen(IScreen screen, CursorMenu cursorMenu, bool previous)
        {
            base.SwitchScreen(screen, cursorMenu, previous);
            if (screen == currentPage.GetCurrentScreen())
            {
                Keybinds.pageUpAction.performed += OnUpgradeStorePageUp;
                Keybinds.pageDownAction.performed += OnUpgradeStorePageDown;
            }
            else
            {
                Keybinds.pageUpAction.performed -= OnUpgradeStorePageUp;
                Keybinds.pageDownAction.performed -= OnUpgradeStorePageDown;
            }
        }
        public void ChangeScreenForward()
        {
            currentPage.PageUp();
            SwitchScreen(currentPage.GetCurrentScreen(), currentPage.GetCurrentCursorMenu(), false);
        }
        public void ChangeScreenBackward()
        {
            currentPage.PageDown();
            SwitchScreen(currentPage.GetCurrentScreen(), currentPage.GetCurrentCursorMenu(), false);
        }
        protected override void AddInputBindings()
        {
            base.AddInputBindings();
            Keybinds.pageUpAction.performed += OnUpgradeStorePageUp;
            Keybinds.pageDownAction.performed += OnUpgradeStorePageDown;
        }
        protected override void RemoveInputBindings()
        {
            base.RemoveInputBindings();
            Keybinds.pageUpAction.performed -= OnUpgradeStorePageUp;
            Keybinds.pageDownAction.performed -= OnUpgradeStorePageDown;
        }

        protected (T[][], CursorMenu[], IScreen[]) GetPageEntries<T>(T[] entries)
        {
            int amountPages = GetAmountPages(entries);
            int lengthPerPage = GetEntriesPerPage(entries);

            T[][] pages = new T[amountPages][];
            for (int i = 0; i < amountPages; i++)
                pages[i] = new T[lengthPerPage];
            for (int i = 0; i < entries.Length; i++)
            {
                int row = i / lengthPerPage;
                int col = i % lengthPerPage;
                pages[row][col] = entries[i];
            }
            CursorMenu[] cursorMenus = new CursorMenu[pages.Length];
            IScreen[] screens = new IScreen[pages.Length];
            initialPage = PageCursorElement.Create(startingPageIndex: 0, cursorMenus: cursorMenus, elements: screens);

            return (pages, cursorMenus, screens);
        }
        internal void OnUpgradeStorePageUp(CallbackContext context)
        {
            ChangeScreenForward();
        }
        internal void OnUpgradeStorePageDown(CallbackContext context)
        {
            ChangeScreenBackward();
        }

    }
}
