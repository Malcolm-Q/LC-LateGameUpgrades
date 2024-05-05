using InteractiveTerminalAPI.UI;
using InteractiveTerminalAPI.UI.Application;
using InteractiveTerminalAPI.UI.Cursor;
using InteractiveTerminalAPI.UI.Screen;
using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc.TerminalNodes;
using MoreShipUpgrades.Misc.UI.Cursor;
using MoreShipUpgrades.Misc.Util;
using System;
using System.Xml.Linq;
using UnityEngine;

namespace MoreShipUpgrades.Misc.UI.Application
{
    internal class UpgradeStoreApplication : PageApplication
    {
        protected override int GetEntriesPerPage<T>(T[] entries)
        {
            return Mathf.CeilToInt(entries.Length / 2.5f);
        }
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
                cursorMenus[i] = CursorMenu.Create(startingCursorIndex: 0, elements: elements,
                    sorting: [
                        CompareName,
                        CompareCurrentPrice,
                        CompareCurrentPriceReversed
                        ]
                );
                CursorMenu cursorMenu = cursorMenus[i];
                screens[i] = new BoxedOutputScreen<string, string>()
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
                        cursorMenu,
                    ],
                    Output = (x) => x,
                    Input = GetCurrentSort,
                };
                for (int j = 0; j < upgrades.Length; j++)
                {
                    CustomTerminalNode upgrade = upgrades[j];
                    if (upgrade == null) continue;
                    elements[j] = new UpgradeCursorElement()
                    {
                        Node = upgrade,
                        Action = () => BuyUpgrade(upgrade, PreviousScreen()),
                        Active = (x) => CanBuyUpgrade(((UpgradeCursorElement)x).Node)
                    };
                }
            }
            currentPage = initialPage;
            currentCursorMenu = initialPage.GetCurrentCursorMenu();
            currentScreen = initialPage.GetCurrentScreen();
        }
        string GetCurrentSort()
        {
            int currentSort = currentCursorMenu.sortingIndex;
            switch(currentSort)
            {
                case 0: return "Sorted by: Alphabetical";
                case 1: return "Sorted by: Price (Ascending)";
                case 2: return "Sorted by: Price (Descending)";
                default: return "";
            }
        }
        int CompareName(CursorElement cursor1, CursorElement cursor2)
        {
            if (cursor1 == null) return 1;
            if (cursor2 == null) return -1;
            UpgradeCursorElement element = cursor1 as UpgradeCursorElement;
            UpgradeCursorElement element2 = cursor2 as UpgradeCursorElement;
            string name1 = element.Node.Name;
            string name2 = element2.Node.Name;
            return name1.CompareTo(name2);
        }
        int CompareCurrentPriceReversed(CursorElement cursor1, CursorElement cursor2)
        {
            if (cursor1 == null) return 1;
            if (cursor2 == null) return -1;
            UpgradeCursorElement element = cursor1 as UpgradeCursorElement;
            UpgradeCursorElement element2 = cursor2 as UpgradeCursorElement;
            int currentPrice1 = element.Node.Unlocked ? element.Node.CurrentUpgrade >= element.Node.MaxUpgrade ? int.MinValue : (int)(element.Node.Prices[element.Node.CurrentUpgrade] * element.Node.salePerc) : (int)(element.Node.UnlockPrice * element.Node.salePerc);
            int currentPrice2 = element2.Node.Unlocked ? element2.Node.CurrentUpgrade >= element2.Node.MaxUpgrade ? int.MinValue :(int)(element2.Node.Prices[element2.Node.CurrentUpgrade] * element2.Node.salePerc) : (int)(element2.Node.UnlockPrice * element2.Node.salePerc);
            return currentPrice2.CompareTo(currentPrice1);
        }
        int CompareCurrentPrice(CursorElement cursor1, CursorElement cursor2)
        {
            if (cursor1 == null) return 1;
            if (cursor2 == null) return -1;
            UpgradeCursorElement element = cursor1 as UpgradeCursorElement;
            UpgradeCursorElement element2 = cursor2 as UpgradeCursorElement;
            int currentPrice1 = element.Node.Unlocked ? element.Node.CurrentUpgrade >=  element.Node.MaxUpgrade ? int.MaxValue : (int)(element.Node.Prices[element.Node.CurrentUpgrade] * element.Node.salePerc) : (int)(element.Node.UnlockPrice * element.Node.salePerc);
            int currentPrice2 = element2.Node.Unlocked ? element2.Node.CurrentUpgrade >= element2.Node.MaxUpgrade ? int.MaxValue : (int)(element2.Node.Prices[element2.Node.CurrentUpgrade] * element2.Node.salePerc) : (int)(element2.Node.UnlockPrice * element2.Node.salePerc);
            return currentPrice1.CompareTo(currentPrice2);
        }
        static bool CanBuyUpgrade(CustomTerminalNode node)
        {
            bool maxLevel = node.CurrentUpgrade >= node.MaxUpgrade;
            if (maxLevel && node.Unlocked)
                return false;

            int groupCredits = UpgradeBus.Instance.GetTerminal().groupCredits;
            int price = node.Unlocked ? (int)(node.Prices[node.CurrentUpgrade] * node.salePerc) : (int)(node.UnlockPrice * node.salePerc);
            if (groupCredits < price)
                return false;

            return true;
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
            PlayerManager.instance.IncreaseUpgradeSpentCredits(price);
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
