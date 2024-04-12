using MoreShipUpgrades.Input;
using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc.TerminalNodes;
using MoreShipUpgrades.Misc.UI.Cursor;
using MoreShipUpgrades.Misc.UI.Page;
using MoreShipUpgrades.Misc.UI.Screen;
using MoreShipUpgrades.Misc.Util;
using System;
using UnityEngine;

namespace MoreShipUpgrades.Misc.UI
{
    internal class MainUpgradeApplication
    {

        PageCursorElement MainPage;

        IScreen currentScreen;
        CursorMenu currentCursorMenu;
        readonly Terminal terminal = UpgradeBus.Instance.GetTerminal();

        public void Initialization()
        {
            int lengthPerPage = Mathf.CeilToInt(UpgradeBus.Instance.terminalNodes.Count / 2f);
            int amountPages = Mathf.CeilToInt((float)UpgradeBus.Instance.terminalNodes.Count / lengthPerPage);
            CustomTerminalNode[][] pagesUpgrades = new CustomTerminalNode[amountPages][];
            for (int i = 0; i < amountPages; i++)
                pagesUpgrades[i] = new CustomTerminalNode[lengthPerPage];
            for (int i = 0; i < UpgradeBus.Instance.terminalNodes.Count; i++)
            {
                int row = i / lengthPerPage;
                int col = i % lengthPerPage;
                pagesUpgrades[row][col] = UpgradeBus.Instance.terminalNodes[i];
            }
            IScreen[] screens = new IScreen[pagesUpgrades.Length];
            CursorMenu[] cursorMenus = new CursorMenu[pagesUpgrades.Length];
            for(int i = 0; i < pagesUpgrades.Length; i++)
            {
                CustomTerminalNode[] upgrades = pagesUpgrades[i];
                CursorElement[] elements = new CursorElement[upgrades.Length];
                cursorMenus[i] = new CursorMenu()
                {
                    cursorIndex = 0,
                    elements = elements
                };
                CursorMenu cursorMenu = cursorMenus[i];
                screens[i] = new BoxedScreen()
                {
                    Title = LGUConstants.MAIN_SCREEN_TITLE,
                    elements =
                    [
                        new TextElement()
                        {
                            Text = LGUConstants.MAIN_SCREEN_TOP_TEXT,
                        },
                        new TextElement()
                        {
                            Text = " "
                        },
                        cursorMenu
                    ]
                };
                for (int j = 0; j < upgrades.Length; j++)
                {
                    CustomTerminalNode upgrade = upgrades[j];
                    if (upgrade == null) continue;
                    elements[j] = new UpgradeCursorElement()
                    {
                        Node = upgrade,
                        Action = () => BuyUpgrade(upgrade, () => SwitchScreen(null, cursorMenu, true, true))
                    };
                }
            }
            MainPage = new PageCursorElement()
            {
                pageIndex = 0,
                cursorMenus = cursorMenus,
                elements = screens,
            };
            currentCursorMenu = MainPage.GetCurrentCursorMenu();
            currentScreen = null;
        }
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
        void NotEnoughCredits(CustomTerminalNode node, Action backAction)
        {
            ErrorMessage(node, backAction, LGUConstants.NOT_ENOUGH_CREDITS);
        }
        void MaxUpgrade(CustomTerminalNode node, Action backAction)
        {
            ErrorMessage(node, backAction, LGUConstants.REACHED_MAX_LEVEL);
        }
        void ErrorMessage(CustomTerminalNode node, Action backAction, string error)
        {

            CursorMenu cursorMenu = new CursorMenu()
            {
                cursorIndex = 0,
                elements =
                    [
                        new CursorElement()
                        {
                            Name = LGUConstants.GO_BACK_PROMPT,
                            Description = "",
                            Action = backAction,
                        }
                    ]
            };
            IScreen screen = new BoxedScreen()
            {
                Title = node.Name,
                elements =
                [
                    new TextElement()
                    {
                            Text = node.Description,
                        },
                        new TextElement()
                        {
                            Text = " ",
                        },
                        new TextElement()
                        {
                            Text = error,
                        },
                        new TextElement()
                        {
                            Text = " ",
                        },
                        cursorMenu
                ]
            };
            SwitchScreen(screen, cursorMenu, false, false);
        }
        public void BuyUpgrade(CustomTerminalNode node, Action backAction)
        {
            int groupCredits = UpgradeBus.Instance.GetTerminal().groupCredits;
            bool maxLevel = node.CurrentUpgrade >= node.MaxUpgrade;
            if (maxLevel && node.Unlocked)
            {
                MaxUpgrade(node, backAction);
                return;
            }
            int price = node.Unlocked ? (int)(node.Prices[node.CurrentUpgrade] * node.salePerc) : (int)(node.UnlockPrice * node.salePerc);
            if (groupCredits < price)
            {
                NotEnoughCredits(node, backAction);
                return;
            }
            Confirm(node.Name, node.Description, () => PurchaseUpgrade(node, price, backAction), backAction, string.Format(LGUConstants.PURCHASE_UPGRADE_FORMAT, price));
        }
        void PurchaseUpgrade(CustomTerminalNode node, int price, Action backAction)
        {
            LguStore.Instance.SyncCreditsServerRpc(terminal.groupCredits - price);
            if (!node.Unlocked)
            {
                LguStore.Instance.HandleUpgrade(node.Name);
            }
            else if (node.Unlocked && node.MaxUpgrade > node.CurrentUpgrade)
            {
                LguStore.Instance.HandleUpgrade(node.Name, true);
            }
            if (node.salePerc != 1f && UpgradeBus.Instance.PluginConfiguration.SALE_APPLY_ONCE.Value) node.salePerc = 1f;
            backAction();
        }
        public void Confirm(string title, string description, Action confirmAction, Action declineAction, string additionalMessage = "")
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

        public void SwitchScreen(IScreen screen, CursorMenu cursorMenu, bool previous, bool enablePage)
        {
            currentScreen = screen;
            currentCursorMenu = cursorMenu;
            if (!previous) cursorMenu.cursorIndex = 0;
            if (enablePage)
            {
                Keybinds.pageUpAction.performed += UpgradesStore.OnUpgradeStorePageUp;
                Keybinds.pageDownAction.performed += UpgradesStore.OnUpgradeStorePageDown;
            }
            else
            {
                Keybinds.pageUpAction.performed -= UpgradesStore.OnUpgradeStorePageUp;
                Keybinds.pageDownAction.performed -= UpgradesStore.OnUpgradeStorePageDown;
            }
        }
    }
}
