using InteractiveTerminalAPI.UI;
using InteractiveTerminalAPI.UI.Application;
using InteractiveTerminalAPI.UI.Cursor;
using InteractiveTerminalAPI.UI.Page;
using InteractiveTerminalAPI.UI.Screen;
using MoreShipUpgrades.API;
using MoreShipUpgrades.Input;
using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc.Util;
using MoreShipUpgrades.UI.Cursor;
using MoreShipUpgrades.UI.TerminalNodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

namespace MoreShipUpgrades.UI.Application
{
    internal class UpgradeStoreApplication : PageApplication
    {
        const int UPGRADES_PER_PAGE = 12;
        protected override int GetEntriesPerPage<T>(T[] entries)
        {
            return UPGRADES_PER_PAGE;
        }
        public override void Initialization()
        {
            CustomTerminalNode[] filteredNodes = UpgradeApi.GetPurchaseableUpgradeNodes().ToArray();

            if (filteredNodes.Length == 0)
            {
                CursorElement[] elements =
                [
                    CursorElement.Create(name: "Leave", action: () => UnityEngine.Object.Destroy(InteractiveTerminalManager.Instance)),
                ];
                CursorMenu cursorMenu = CursorMenu.Create(startingCursorIndex: 0, elements: elements);
                IScreen screen = new BoxedScreen()
                {
                    Title = LguConstants.MAIN_SCREEN_TITLE,
                    elements =
                    [
                        new TextElement()
                        {
                            Text = LguConstants.MAIN_SCREEN_TOP_TEXT_NO_ENTRIES,
                        },
                        new TextElement()
                        {
                            Text = " "
                        },
                        cursorMenu
                    ]
                };
                currentPage = PageCursorElement.Create(startingPageIndex: 0, elements: [screen], cursorMenus: [cursorMenu]);
                currentCursorMenu = cursorMenu;
                currentScreen = screen;
                return;
            }

            List<CursorElement> cursorElements = [];
            PageCursorElement sharedPage = GetFilteredUpgradeNodes(ref filteredNodes, ref cursorElements, (x) => x.SharedUpgrade, LguConstants.MAIN_SCREEN_TITLE, LguConstants.MAIN_SCREEN_SHARED_UPGRADES_TEXT);
            PageCursorElement individualPage = GetFilteredUpgradeNodes(ref filteredNodes, ref cursorElements, (x) => !x.SharedUpgrade && (!UpgradeBus.Instance.lockedUpgrades.Keys.Contains(x) || UpgradeBus.Instance.PluginConfiguration.ShowLockedUpgrades), LguConstants.MAIN_SCREEN_TITLE, LguConstants.MAIN_SCREEN_INDIVIDUAL_UPGRADES_TEXT);

            if (cursorElements.Count > 1)
            {
                CursorElement[] upgradeElements = [.. cursorElements];
                CursorMenu upgradeCursorMenu = CursorMenu.Create(startingCursorIndex: 0, elements: upgradeElements);
                IScreen upgradeScreen = new BoxedScreen()
                {
                    Title = LguConstants.MAIN_SCREEN_TITLE,
                    elements =
                    [
                        upgradeCursorMenu
                    ]
                };
                initialPage = PageCursorElement.Create(startingPageIndex: 0, elements: [upgradeScreen], cursorMenus: [upgradeCursorMenu]);
            }
            else
            {
                if (sharedPage == null)
                {
                    initialPage = individualPage;
                }
                else
                {
                    initialPage = sharedPage;
                }
            }
            currentPage = initialPage;
            currentCursorMenu = currentPage.GetCurrentCursorMenu();
            currentScreen = currentPage.GetCurrentScreen();
        }
        PageCursorElement GetFilteredUpgradeNodes(ref CustomTerminalNode[] nodes, ref List<CursorElement> list, Func<CustomTerminalNode, bool> predicate, string pageTitle, string cursorName)
        {
            PageCursorElement page = null;
            CustomTerminalNode[] filteredNodes = nodes.Where(predicate).ToArray();
            if (filteredNodes.Length > 0)
            {
                page = BuildUpgradePage(filteredNodes, pageTitle);
                list.Add(CursorElement.Create(name: cursorName, action: () => SwitchToUpgradeScreen(page, previous: true)));
            }
            return page;
        }
        void SwitchToUpgradeScreen(PageCursorElement page, bool previous)
        {
            InteractiveTerminalAPI.Compat.InputUtils_Compat.CursorExitKey.performed -= OnScreenExit;
            InteractiveTerminalAPI.Compat.InputUtils_Compat.CursorExitKey.performed += OnUpgradeStoreExit;
            SwitchScreen(page, previous);
        }

        void OnUpgradeStoreExit(CallbackContext context)
        {
            ResetScreen();
            InteractiveTerminalAPI.Compat.InputUtils_Compat.CursorExitKey.performed += OnScreenExit;
            InteractiveTerminalAPI.Compat.InputUtils_Compat.CursorExitKey.performed -= OnUpgradeStoreExit;
        }
        PageCursorElement BuildUpgradePage(CustomTerminalNode[] nodes, string title)
        {
            (CustomTerminalNode[][], CursorMenu[], IScreen[]) entries = GetPageEntries(nodes);

            CustomTerminalNode[][] pagesUpgrades = entries.Item1;
            CursorMenu[] cursorMenus = entries.Item2;
            IScreen[] screens = entries.Item3;
            PageCursorElement page = null;
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
                    Title = title,
                    elements =
                    [
                        new TextElement()
                        {
                            Text = LguConstants.MAIN_SCREEN_TOP_TEXT,
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
                        Action = () => BuyUpgrade(upgrade, () => SwitchScreen(page, previous: true)),
                        Active = (x) => CanBuyUpgrade(((UpgradeCursorElement)x).Node)
                    };
                }
            }
            page = PageCursorElement.Create(0, screens, cursorMenus);
            return page;
        }

        string GetCurrentSort()
        {
            int currentSort = currentCursorMenu.sortingIndex;
            return currentSort switch
            {
                0 => $"Sorted by: Alphabetical [{InteractiveTerminalAPI.Compat.InputUtils_Compat.ChangeApplicationSortingKey.GetBindingDisplayString()}]",
                1 => $"Sorted by: Price (Ascending) [{InteractiveTerminalAPI.Compat.InputUtils_Compat.ChangeApplicationSortingKey.GetBindingDisplayString()}]",
                2 => $"Sorted by: Price (Descending) [{InteractiveTerminalAPI.Compat.InputUtils_Compat.ChangeApplicationSortingKey.GetBindingDisplayString()}]",
                _ => "",
            };
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
            int currentPrice1 = element.Node.Unlocked && element.Node.CurrentUpgrade >= element.Node.MaxUpgrade ? int.MinValue : element.Node.GetCurrentPrice();
            int currentPrice2 = element2.Node.Unlocked && element2.Node.CurrentUpgrade >= element2.Node.MaxUpgrade ? int.MinValue : element2.Node.GetCurrentPrice();
            return currentPrice2.CompareTo(currentPrice1);
        }
        int CompareCurrentPrice(CursorElement cursor1, CursorElement cursor2)
        {
            if (cursor1 == null) return 1;
            if (cursor2 == null) return -1;
            UpgradeCursorElement element = cursor1 as UpgradeCursorElement;
            UpgradeCursorElement element2 = cursor2 as UpgradeCursorElement;
            int currentPrice1 = element.Node.Unlocked && element.Node.CurrentUpgrade >= element.Node.MaxUpgrade ? int.MaxValue : element.Node.GetCurrentPrice();
            int currentPrice2 = element2.Node.Unlocked && element2.Node.CurrentUpgrade >= element2.Node.MaxUpgrade ? int.MaxValue : element2.Node.GetCurrentPrice();
            return currentPrice1.CompareTo(currentPrice2);
        }
        static bool CanBuyUpgrade(CustomTerminalNode node)
        {
            if (UpgradeBus.Instance.PluginConfiguration.ALTERNATIVE_ITEM_PROGRESSION && UpgradeBus.Instance.PluginConfiguration.ITEM_PROGRESSION_NO_PURCHASE_UPGRADES) return false;
            if (UpgradeBus.Instance.PluginConfiguration.BuyableUpgradeOnce && UpgradeBus.Instance.lockedUpgrades.Keys.Contains(node)) return false;
            bool maxLevel = node.CurrentUpgrade >= node.MaxUpgrade;
            if (maxLevel && node.Unlocked)
                return false;

            int groupCredits = UpgradeBus.Instance.GetTerminal().groupCredits;
            int price = node.GetCurrentPrice();
            return groupCredits >= price;
        }
        public void BuyUpgrade(CustomTerminalNode node, Action backAction)
        {
            int groupCredits = UpgradeBus.Instance.GetTerminal().groupCredits;
            bool maxLevel = node.CurrentUpgrade >= node.MaxUpgrade;

            StringBuilder discoveredItems = new();
            List<string> items = ItemProgressionManager.GetDiscoveredItems(node);
            if (items.Count > 0)
            {
                discoveredItems.Append("\n\nDiscovered items: ");
                discoveredItems.Append(string.Join(",", items));
            }
            string finalText = node.Description + discoveredItems;
            if (maxLevel && node.Unlocked)
            {
                ErrorMessage(node.Name, finalText, backAction, LguConstants.REACHED_MAX_LEVEL);
                return;
            }
            int price = node.GetCurrentPrice();
            if (groupCredits < price)
            {
                ErrorMessage(node.Name, finalText, backAction, LguConstants.NOT_ENOUGH_CREDITS);
                return;
            }
            if (UpgradeBus.Instance.PluginConfiguration.BuyableUpgradeOnce && UpgradeBus.Instance.lockedUpgrades.Keys.Contains(node))
            {
                ErrorMessage(node.Name, finalText, backAction, LguConstants.LOCKED_UPGRADE);
                return;
            }
            if (UpgradeBus.Instance.PluginConfiguration.ALTERNATIVE_ITEM_PROGRESSION && UpgradeBus.Instance.PluginConfiguration.ITEM_PROGRESSION_NO_PURCHASE_UPGRADES)
            {
                ErrorMessage(node.Name, finalText, backAction, " ");
                return;
            }
            Confirm(node.Name, finalText, () => PurchaseUpgrade(node, price, backAction), backAction, string.Format(LguConstants.PURCHASE_UPGRADE_FORMAT, price));
        }
        void PurchaseUpgrade(CustomTerminalNode node, int price, Action backAction)
        {
            terminal.BuyItemsServerRpc([], terminal.groupCredits - price, terminal.numberOfItemsInDropship); // The only vanilla rpc that syncs credits without ownership check
            LguStore.Instance.AddUpgradeSpentCreditsServerRpc(price);
            UpgradeApi.TriggerUpgradeRankup(node);
            backAction();
        }
    }
}
