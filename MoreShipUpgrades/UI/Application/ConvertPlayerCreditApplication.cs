using InteractiveTerminalAPI.UI;
using InteractiveTerminalAPI.UI.Application;
using InteractiveTerminalAPI.UI.Cursor;
using InteractiveTerminalAPI.UI.Screen;
using MoreShipUpgrades.Managers;
using System;

namespace MoreShipUpgrades.UI.Application
{
    internal class ConvertPlayerCreditApplication : InteractiveCounterApplication<CursorCounterMenu, CursorCounterElement>
    {
        const string TITLE = "Player Credits Conversion";
        public override void Initialization()
        {
            CursorCounterElement[] cursorCounterElements = new CursorCounterElement[3];
            CursorCounterMenu cursorCounterMenu = CursorCounterMenu.Create(cursorCounterElements.Length - 1, '>', cursorCounterElements);
            ITextElement[] textElements =
                [
                    new TextElement()
                    {
                        Text = $"What operation of conversion do you wish to execute?"
                    },
                    new TextElement()
                    {
                        Text = ""
                    },
                    cursorCounterMenu
                ];
            IScreen screen = BoxedScreen.Create(TITLE, textElements);
            cursorCounterElements[0] = new CursorCounterElement()
            {
                Action = () => ConvertCreditsToPCsScreen(backAction: () => SwitchScreen(screen, cursorCounterMenu, previous: true)),
                Active = (_) => HasEnoughCreditsToConvert(),
                Name = "Convert Credits to Player Credits (PCs)",
                SelectInactive = false
            };
            cursorCounterElements[1] = new CursorCounterElement()
            {
                Action = () => ConvertPCsToCreditsScreen(backAction: () => SwitchScreen(screen, cursorCounterMenu, previous: true)),
                Active = (_) => HasEnoughPCsToConvert(),
                Name = "Convert Player Credits (PCs) to Credits",
                SelectInactive = false
            };
            cursorCounterElements[2] = new CursorCounterElement()
            {
                Action = () => UnityEngine.Object.Destroy(InteractiveTerminalManager.Instance.gameObject),
                Active = (_) => true,
                Name = "Back",
                SelectInactive = false
            };

            currentCursorMenu = cursorCounterMenu;
            currentScreen = screen;
        }

        private void ConvertPCsToCreditsScreen(Action backAction)
        {
            CursorOutputElement<string>[] cursorCounterElements = new CursorOutputElement<string>[2];
            Func<int, string>[] array = new Func<int, string>[1];
            array[0] = (int x) => $"{x} PCs";
            CursorCounterMenu cursorCounterMenu = CursorCounterMenu.Create(cursorCounterElements.Length - 1, '>', cursorCounterElements);
            IScreen screen = BoxedOutputScreen<string, string>.Create(TITLE, [cursorCounterMenu], input: () => $"${CurrencyManager.Instance.GetCreditsFromCurrencyAmountConversion(cursorCounterElements[0].Counter)}", output: (string x) => x);
            cursorCounterElements[0] = new CursorOutputElement<string>()
            {
                Action = () => TryConvertPCsToCredits(cursorCounterElements[0], backAction: () => SwitchScreen(screen, cursorCounterMenu, previous: true)),
                Active = (x) => HasEnoughPCsToConvert(((CursorCounterElement)x).Counter),
                Name = "Amount of PCs to convert",
                SelectInactive = true,
                Func = array[0],
            };
            cursorCounterElements[1] = new CursorOutputElement<string>()
            {
                Action = backAction,
                Active = (_) => true,
                Name = "Back",
                SelectInactive = false,
                Func = (_) => "",
            };

            currentCursorMenu = cursorCounterMenu;
            currentScreen = screen;
        }

        private void TryConvertPCsToCredits(CursorOutputElement<string> cursorOutputElement, Action backAction)
        {
            int count = cursorOutputElement.Counter;
            if (CurrencyManager.Instance.CurrencyAmount < count)
            {
                ErrorMessage(title: TITLE, description: "", backAction, error: "Not enough PCs to perform conversion.");
                return;
            }
            LguStore.Instance.SyncCreditsServerRpc(terminal.groupCredits + CurrencyManager.Instance.GetCreditsFromCurrencyAmountConversion(count));
            CurrencyManager.Instance.RemoveCurrencyAmount(count);
            backAction();
        }
        private void TryConvertCreditsToPCs(CursorOutputElement<string> cursorOutputElement, Action backAction)
        {
            int count = cursorOutputElement.Counter;
            int requiredCredits = CurrencyManager.Instance.GetRequiredCreditsFromCurrencyConversion(count);

			if (terminal.groupCredits < requiredCredits)
            {
                ErrorMessage(title: TITLE, description: "", backAction, error: "Not enough Company Credits to perform conversion.");
                return;
            }
            LguStore.Instance.SyncCreditsServerRpc(terminal.groupCredits - requiredCredits);
            CurrencyManager.Instance.AddCurrencyAmount(count);
            backAction();
        }

        const int MINIMUM_PC = 1;
        private bool HasEnoughCreditsToConvert(int amount = MINIMUM_PC)
        {
            return terminal.groupCredits >= CurrencyManager.Instance.GetRequiredCreditsFromCurrencyConversion(amount);
        }

        private bool HasEnoughPCsToConvert(int amount = 0)
        {
            return CurrencyManager.Instance.CurrencyAmount != 0 && CurrencyManager.Instance.CurrencyAmount >= amount;
        }

        private void ConvertCreditsToPCsScreen(Action backAction)
        {
            CursorOutputElement<string>[] cursorCounterElements = new CursorOutputElement<string>[2];
            Func<int, string>[] array = new Func<int, string>[1];
            array[0] = (int x) => $"${CurrencyManager.Instance.GetRequiredCreditsFromCurrencyConversion(cursorCounterElements[0].Counter)}";
            CursorCounterMenu cursorCounterMenu = CursorCounterMenu.Create(cursorCounterElements.Length - 1, '>', cursorCounterElements);
            IScreen screen = BoxedOutputScreen<string, string>.Create(TITLE, [cursorCounterMenu], input: () => $"{cursorCounterElements[0].Counter} PC", output: (string x) => x);
            cursorCounterElements[0] = new CursorOutputElement<string>()
            {
                Action = () => TryConvertCreditsToPCs(cursorCounterElements[0], backAction: () => SwitchScreen(screen, cursorCounterMenu, previous: true)),
                Active = (x) => HasEnoughCreditsToConvert(((CursorCounterElement)x).Counter),
                Name = "Amount of credits to convert",
                SelectInactive = true,
                Func = array[0],
            };
            cursorCounterElements[1] = new CursorOutputElement<string>()
            {
                Action = backAction,
                Active = (_) => true,
                Name = "Back",
                SelectInactive = false,
                Func = (_) => "",
            };

            currentCursorMenu = cursorCounterMenu;
            currentScreen = screen;
        }
        protected void Confirm(string title, string description, Action confirmAction, Action declineAction, string additionalMessage = "")
        {
            CursorCounterElement[] elements =
            [
            CursorCounterElement.Create("Confirm", "", confirmAction, showCounter: false),
            CursorCounterElement.Create("Abort", "", declineAction, showCounter: false)
            ];
            CursorCounterMenu cursorMenu = CursorCounterMenu.Create(0, '>', elements);
            ITextElement[] elements2 =
            [
            TextElement.Create(description),
            TextElement.Create(" "),
            TextElement.Create(additionalMessage),
            cursorMenu
            ];
            IScreen screen = BoxedScreen.Create(title, elements2);
            SwitchScreen(screen, cursorMenu, previous: false);
        }
        protected void ErrorMessage(string title, string description, Action backAction, string error)
        {
            CursorCounterElement[] elements = [CursorCounterElement.Create("Back", "", backAction, showCounter: false)];
            CursorCounterMenu cursorMenu = CursorCounterMenu.Create(0, '>', elements);
            ITextElement[] elements2 =
            [
            TextElement.Create(description),
            TextElement.Create(" "),
            TextElement.Create(error),
            TextElement.Create(" "),
            cursorMenu
            ];
            IScreen screen = BoxedScreen.Create(title, elements2);
            SwitchScreen(screen, cursorMenu, previous: false);
        }
    }
}
