using GameNetcodeStuff;
using InteractiveTerminalAPI.UI;
using InteractiveTerminalAPI.UI.Application;
using InteractiveTerminalAPI.UI.Cursor;
using InteractiveTerminalAPI.UI.Page;
using InteractiveTerminalAPI.UI.Screen;
using MoreShipUpgrades.Managers;
using System;
using System.Linq;

namespace MoreShipUpgrades.UI.Application
{
    internal class TradePlayerCreditsApplication : PageApplication
    {
        const string MAIN_SCREEN_TITLE = "Player Credits Trading";
        const string NO_PLAYERS_TO_TRADE_TEXT = "There are no players you can trade your player credits with.";
        const string NO_PLAYER_CREDITS_TO_TRADE_TEXT = "You currently don't possess any player credit to trade with.";

        const string TRADE_PLAYER_TEXT = "How many player credits do you wish to provide for selected player?";
        const string TRADE_CREDITS_INFO_TEXT = "Select the player you wish to provide player credits to:";
        const string CONFIRM_TRADE_PLAYER_TEXT = "Do you wish to trade {0} player credits to {1}?";
        public override void Initialization()
        {
            PlayerControllerB[] activePlayers = StartOfRound.Instance.allPlayerScripts.Where(x => (x.isPlayerControlled || x.isPlayerDead) && x != GameNetworkManager.Instance.localPlayerController).ToArray();
            if (activePlayers.Length == 0)
            {
                CursorElement[] elements =
                [
                    CursorElement.Create(name: "Leave", action: () => UnityEngine.Object.Destroy(InteractiveTerminalManager.Instance)),
                ];
                CursorMenu cursorMenu = CursorMenu.Create(startingCursorIndex: 0, elements: elements);
                IScreen screen = new BoxedScreen()
                {
                    Title = MAIN_SCREEN_TITLE,
                    elements =
                    [
                        new TextElement()
                        {
                            Text = NO_PLAYERS_TO_TRADE_TEXT,
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
            if (CurrencyManager.Instance.CurrencyAmount <= 0)
            {
                CursorElement[] elements =
                [
                    CursorElement.Create(name: "Leave", action: () => UnityEngine.Object.Destroy(InteractiveTerminalManager.Instance)),
                ];
                CursorMenu cursorMenu = CursorMenu.Create(startingCursorIndex: 0, elements: elements);
                IScreen screen = new BoxedScreen()
                {
                    Title = MAIN_SCREEN_TITLE,
                    elements =
                    [
                        new TextElement()
                        {
                            Text = NO_PLAYER_CREDITS_TO_TRADE_TEXT,
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
            (PlayerControllerB[][], CursorMenu[], IScreen[]) entries = GetPageEntries(activePlayers);

            PlayerControllerB[][] pagesPlayers = entries.Item1;
            CursorMenu[] cursorMenus = entries.Item2;
            IScreen[] screens = entries.Item3;
            PageCursorElement page = null;
            for (int i = 0; i < pagesPlayers.Length; i++)
            {
                PlayerControllerB[] players = pagesPlayers[i];
                CursorElement[] cursorElements = new CursorElement[players.Length];
                cursorMenus[i] = CursorMenu.Create(startingCursorIndex: 0, elements: cursorElements);
                CursorMenu cursorMenu = cursorMenus[i];
                screens[i] = new BoxedScreen()
                {
                    Title = MAIN_SCREEN_TITLE,
                    elements =
                    [
                        new TextElement()
                        {
                            Text = TRADE_CREDITS_INFO_TEXT,
                        },
                        new TextElement()
                        {
                            Text = " "
                        },
                        cursorMenu,
                    ],
                };

                for (int j = 0; j < players.Length; j++)
                {
                    PlayerControllerB player = players[j];
                    if (player == null) continue;
                    cursorElements[j] = new CursorElement()
                    {
                        Name = player.playerUsername,
                        Action = () => TradePlayerCredits(player, () => SwitchScreen(page, previous: true)),
                    };
                }
            }
            page = PageCursorElement.Create(0, screens, cursorMenus);
            initialPage = page;
            currentPage = initialPage;
            currentCursorMenu = currentPage.GetCurrentCursorMenu();
            currentScreen = currentPage.GetCurrentScreen();

        }

        void TradePlayerCredits(PlayerControllerB tradingPlayer, Action backAction)
        {
            int playerCredits = CurrencyManager.Instance.CurrencyAmount;
            CursorElement[] elements =
                {
                CursorElement.Create("Give 1 Player Credit", "", () => ConfirmTradePlayerCredits(tradingPlayer, 1, backAction), selectInactive: false),
                CursorElement.Create("Give 5 Player Credits", "", () => ConfirmTradePlayerCredits(tradingPlayer, 5, backAction), active: (_) => playerCredits >= 5, selectInactive: false),
                CursorElement.Create("Give All Player Credits", "", () => ConfirmTradePlayerCredits(tradingPlayer, playerCredits, backAction), selectInactive: false),
                CursorElement.Create("Cancel", "", backAction)
                };
            CursorMenu cursorMenu = CursorMenu.Create(0, '>', elements);
            ITextElement[] elements2 =
            {
                TextElement.Create(TRADE_PLAYER_TEXT),
                TextElement.Create(" "),
                cursorMenu
                };
            IScreen screen = BoxedScreen.Create(tradingPlayer.playerUsername, elements2);
            SwitchScreen(screen, cursorMenu, previous: false);
        }

        void ConfirmTradePlayerCredits(PlayerControllerB tradingPlayer, int playerCreditAmount,  Action backAction)
        {
            Confirm(tradingPlayer.playerUsername, string.Format(CONFIRM_TRADE_PLAYER_TEXT, playerCreditAmount, tradingPlayer.playerUsername), () => ExecuteTrade(tradingPlayer, playerCreditAmount, backAction), backAction);
        }

        void ExecuteTrade(PlayerControllerB tradingPlayer, int playerCreditAmount, Action backAction)
        {
            CurrencyManager.Instance.RemoveCurrencyAmount(playerCreditAmount);
            CurrencyManager.Instance.TradePlayerCreditsServerRpc(tradingPlayer.actualClientId, playerCreditAmount);
            if (CurrencyManager.Instance.CurrencyAmount <= 0)
                UnityEngine.Object.Destroy(InteractiveTerminalManager.Instance);
            else backAction();
        }
    }
}
