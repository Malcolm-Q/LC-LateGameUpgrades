using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc.TerminalNodes;
using MoreShipUpgrades.Misc.UI.Cursor;
using MoreShipUpgrades.Misc.UI.Page;
using MoreShipUpgrades.Misc.UI.Screen;
using System;

namespace MoreShipUpgrades.Misc.UI
{
    internal class MainUpgradeApplication
    {
        PageElement MainPage;
        BoxedScreen MainScreen;
        CursorMenu MainCursorMenu;

        IScreen currentScreen;
        CursorMenu currentCursorMenu;
        Terminal terminal = UpgradeBus.Instance.GetTerminal();

        public void Initialization()
        {
        }
        void MoveCursorUp()
        {
            currentCursorMenu.Backward();
        }
        void MoveCursorDown()
        {
            currentCursorMenu.Forward();
        }
        void MovePageUp()
        {
            MainPage.PageUp();
        }
        void MovePageDown()
        {
            MainPage.PageDown();
        }
        public void Submit()
        {
            MainCursorMenu.Execute();
        }
        public void UpdateText()
        {
            terminal.screenText.text = currentScreen.GetText(UpgradeApplication.AVAILABLE_CHARACTERS_PER_LINE);
        }
        void NotEnoughCredits(CustomTerminalNode node, Action backAction)
        {
            ErrorMessage(node, backAction, "You do not have enough credits to purchase this upgrade.");
        }
        void MaxUpgrade(CustomTerminalNode node, Action backAction)
        {
            ErrorMessage(node, backAction, "You have reached the maximum level of this upgrade.");
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
                            Name = "Back",
                            Description = null,
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
                            Text = node.SimplifiedDescription,
                        },
                        new TextElement()
                        {
                            Text = " ",
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
            SwitchScreen(screen, cursorMenu, false);
        }
        public void BuyUpgrade(CustomTerminalNode node, Action backAction)
        {
            int groupCredits = UpgradeBus.Instance.GetTerminal().groupCredits;
            bool maxLevel = node.CurrentUpgrade >= node.MaxUpgrade;
            if (maxLevel)
            {
                MaxUpgrade(node, backAction);
                return;
            }
            int price = node.Unlocked ? node.Prices[node.CurrentUpgrade] : node.UnlockPrice;
            if (groupCredits < price)
            {
                NotEnoughCredits(node, backAction);
                return;
            }
            Confirm(node.Name, node.SimplifiedDescription, () => PurchaseUpgrade(node, price), backAction);
        }
        void PurchaseUpgrade(CustomTerminalNode node, int price)
        {
            LguStore.Instance.SyncCreditsServerRpc(terminal.groupCredits - price);
            LguStore.Instance.HandleUpgrade(node.Name);
            if (!node.Unlocked)
            {
                LguStore.Instance.HandleUpgrade(node.Name);
            }
            else if (node.Unlocked && node.MaxUpgrade > node.CurrentUpgrade)
            {
                LguStore.Instance.HandleUpgrade(node.Name, true);
            }
            if (node.salePerc != 1f && UpgradeBus.Instance.PluginConfiguration.SALE_APPLY_ONCE.Value) node.salePerc = 1f;
        }
        public void Confirm(string title, string description, Action confirmAction, Action declineAction)
        {
            CursorMenu cursorMenu = new CursorMenu()
            {
                elements =
                [
                    new CursorElement()
                    {
                        Name = "Confirm",
                        Description = null,
                        Action = () => { confirmAction(); }
                    },
                    new CursorElement()
                    {
                        Name = "Abort",
                        Description = null,
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
                    cursorMenu
                ]
            };
            SwitchScreen(screen, cursorMenu, false);
        }

        public void SwitchScreen(IScreen screen, CursorMenu cursorMenu, bool previous)
        {
            currentScreen = screen;
            currentCursorMenu = cursorMenu;
            if (!previous) cursorMenu.cursorIndex = 0;
        }
    }
}
