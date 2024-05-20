using InteractiveTerminalAPI.UI;
using InteractiveTerminalAPI.UI.Application;
using InteractiveTerminalAPI.UI.Cursor;
using InteractiveTerminalAPI.UI.Screen;
using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc.Util;
using MoreShipUpgrades.UpgradeComponents.Commands;
using System;

namespace MoreShipUpgrades.Misc.UI.Application
{
    internal class ExtendDeadlineApplication : InteractiveCounterApplication<CursorCounterMenu, CursorCounterElement>
    {
        public override void Initialization()
        {
            CursorOutputElement<string>[] cursorCounterElements = new CursorOutputElement<string>[1];
            Func<int, string>[] array = new Func<int, string>[1];
            CursorCounterMenu cursorCounterMenu = CursorCounterMenu.Create(0, '>', cursorCounterElements);
            IScreen screen = BoxedOutputScreen<string, string>.Create(ExtendDeadlineScript.NAME, [cursorCounterMenu], () => cursorCounterElements[0].ApplyFunction(), (string x) => x);
            for (int i = 0; i < cursorCounterElements.Length; i++)
            {
                int counter = i;
                array[i] = (int x) => $"${ExtendDeadlineScript.instance.GetTotalCostPerDay(x)}";
                cursorCounterElements[i] = CursorOutputElement<string>.Create(name: "Days to extend",
                                                                            description: "",
                                                                            action: () => TryPurchaseExtendedDays(cursorCounterElements[counter], backAction: () => SwitchScreen(screen, cursorCounterMenu, previous: true)),
                                                                            counter: 0,
                                                                            func: array[counter]);
            }

            currentCursorMenu = cursorCounterMenu;
            currentScreen = screen;
        }

        void TryPurchaseExtendedDays<T>(CursorOutputElement<T> element, Action backAction)
        {
            int days = element.Counter;
            int totalCost = ExtendDeadlineScript.instance.GetTotalCostPerDay(days);
            if (terminal.groupCredits < totalCost)
            {
                ErrorMessage(ExtendDeadlineScript.NAME, LGUConstants.NOT_ENOUGH_CREDITS_EXTEND, backAction, "");
                return;
            }
            Confirm(ExtendDeadlineScript.NAME, string.Format(LGUConstants.PURCHASE_EXTEND_DEADLINE_FORMAT, days, totalCost), () => PurchaseExtendedDays(days, totalCost, backAction), backAction);
        }
        void PurchaseExtendedDays(int days, int totalCost, Action backAction)
        {
            terminal.groupCredits -= totalCost;
            LguStore.Instance.SyncCreditsServerRpc(terminal.groupCredits);
            ExtendDeadlineScript.instance.ExtendDeadlineServerRpc(days);
            backAction();
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
