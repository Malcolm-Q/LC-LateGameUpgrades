using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc.TerminalNodes;
using MoreShipUpgrades.Misc.UI.Cursor;
using MoreShipUpgrades.Misc.UI.Screen;
using MoreShipUpgrades.Misc.Util;
using System;

namespace MoreShipUpgrades.Misc.UI.Application
{
    internal class UpgradeStoreApplication : PageApplication
    {
        public override void Initialization()
        {
            (CustomTerminalNode[][], CursorMenu[], IScreen[]) entries = GetPageEntries(UpgradeBus.Instance.terminalNodes.ToArray());

            CustomTerminalNode[][] pagesUpgrades = entries.Item1;
            CursorMenu[] cursorMenus = entries.Item2;
            IScreen[] screens = entries.Item3;

            for (int i = 0; i < pagesUpgrades.Length; i++)
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
                        Action = () => BuyUpgrade(upgrade, PreviousScreen())
                    };
                }
            }
            currentPage = initialPage;
            currentCursorMenu = initialPage.GetCurrentCursorMenu();
            currentScreen = initialPage.GetCurrentScreen();
        }
        public void BuyUpgrade(CustomTerminalNode node, Action backAction)
        {
            int groupCredits = UpgradeBus.Instance.GetTerminal().groupCredits;
            bool maxLevel = node.CurrentUpgrade >= node.MaxUpgrade;
            if (maxLevel && node.Unlocked)
            {
                ErrorMessage(node.Name, node.Description, backAction, LGUConstants.REACHED_MAX_LEVEL);
                return;
            }
            int price = node.Unlocked ? (int)(node.Prices[node.CurrentUpgrade] * node.salePerc) : (int)(node.UnlockPrice * node.salePerc);
            if (groupCredits < price)
            {
                ErrorMessage(node.Name, node.Description, backAction, LGUConstants.NOT_ENOUGH_CREDITS);
                return;
            }
            Confirm(node.Name, node.Description, () => PurchaseUpgrade(node, price, backAction), backAction, string.Format(LGUConstants.PURCHASE_UPGRADE_FORMAT, price));
        }
        void PurchaseUpgrade(CustomTerminalNode node, int price, Action backAction)
        {
            LguStore.Instance.SyncCreditsServerRpc(terminal.groupCredits - price);
            if (!node.Unlocked)
            {
                LguStore.Instance.HandleUpgrade(node.OriginalName);
            }
            else if (node.Unlocked && node.MaxUpgrade > node.CurrentUpgrade)
            {
                LguStore.Instance.HandleUpgrade(node.OriginalName, true);
            }
            if (node.salePerc != 1f && UpgradeBus.Instance.PluginConfiguration.SALE_APPLY_ONCE.Value) node.salePerc = 1f;
            backAction();
        }
    }
}
