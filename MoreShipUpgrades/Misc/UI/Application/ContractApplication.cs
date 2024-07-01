using InteractiveTerminalAPI.UI;
using InteractiveTerminalAPI.UI.Application;
using InteractiveTerminalAPI.UI.Cursor;
using InteractiveTerminalAPI.UI.Screen;
using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc.Util;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace MoreShipUpgrades.Misc.UI.Application
{
    internal class ContractApplication : InteractiveTerminalApplication
    {
        const string MAIN_MENU_TITLE = "Contracts";
        const string CONTRACT_INFO_CURSOR_ELEMENT = "Contract Info";
        const string PICK_CONTRACT_CURSOR_ELEMENT = "Pick a Contract";
        const string CURRENT_CONTRACT_CURSOR_ELEMENT = "Current Contract";

        const string RANDOM_MOON_CURSOR_ELEMENT = "Random Moon";
        const string SPECIFIED_MOON_CURSOR_ELEMENT = "Specified Moon";
        static readonly string[] MAIN_MENU_CURSOR_ELEMENTS = [CONTRACT_INFO_CURSOR_ELEMENT, PICK_CONTRACT_CURSOR_ELEMENT, CURRENT_CONTRACT_CURSOR_ELEMENT];

        IScreen mainScreen;
        CursorMenu mainCursorMenu;
        public override void Initialization()
        {
            IScreen screen;
            ITextElement[] textElements;
            CursorMenu menu;
            CursorElement[] cursorElements = new CursorElement[MAIN_MENU_CURSOR_ELEMENTS.Length];
            cursorElements[0] = CursorElement.Create(
                    name: MAIN_MENU_CURSOR_ELEMENTS[0],
                    description: "",
                    action: ShowContractInformation,
                    active: (x) => true,
                    selectInactive: false
                );
            cursorElements[1] = CursorElement.Create(
                    name: MAIN_MENU_CURSOR_ELEMENTS[1],
                    description: "",
                    action: PickContract,
                    active: (x) => ContractManager.Instance.contractType == "None",
                    selectInactive: true
                ); 
            cursorElements[2] = CursorElement.Create(
                    name: MAIN_MENU_CURSOR_ELEMENTS[2],
                    description: "",
                    action: ShowCurrentContract,
                    active: (x) => ContractManager.Instance.contractType != "None",
                    selectInactive: true
                );
            menu = CursorMenu.Create(startingCursorIndex: 0, elements: cursorElements);
            textElements =
                [
                    TextElement.Create("Select a prompt to execute contract related actions."),
                    TextElement.Create(" "),
                    menu,
                ];
            screen = BoxedScreen.Create(title: MAIN_MENU_TITLE, elements: textElements);
            currentCursorMenu = menu;
            currentScreen = screen;
            mainScreen = screen;
            mainCursorMenu = menu;
        }

        void ShowContractInformation()
        {
            IScreen previousScreen = currentScreen;
            CursorMenu previousCursorMenu = currentCursorMenu;
            IScreen screen;
            ITextElement[] textElements;
            CursorMenu menu;
            CursorElement[] cursorElements = new CursorElement[CommandParser.contractInfos.Count + 1];
            for(int i = 0; i < CommandParser.contracts.Count; i++)
            {
                int counter = i;
                cursorElements[i] = CursorElement.Create(
                    name: CommandParser.contracts[counter],
                    description: string.Empty,
                    action: () => ShowContractInformation(CommandParser.contracts[counter], CommandParser.contractInfos[counter]),
                    active: (x) => true,
                    selectInactive: false
                    );
            }
            cursorElements[CommandParser.contractInfos.Count] = CursorElement.Create(
                    name: "Back",
                    description: string.Empty,
                    action: () => SwitchScreen(previousScreen, previousCursorMenu, true),
                    active: (x) => true,
                    selectInactive: true
                    );
            menu = CursorMenu.Create(startingCursorIndex: 0, elements: cursorElements);
            textElements =
                [
                    TextElement.Create("Select a prompt to know the information of the contract from."),
                    TextElement.Create(" "),
                    menu,
                ];
            screen = BoxedScreen.Create(title: MAIN_MENU_TITLE, elements: textElements);
            SwitchScreen(screen, menu, previous: true);
        }

        void ShowContractInformation(string contractType, string information)
        {
            IScreen previousScreen = currentScreen;
            CursorMenu previousCursorMenu = currentCursorMenu;
            IScreen screen;
            ITextElement[] textElements;
            CursorMenu menu;
            CursorElement[] cursorElements =
            [
                CursorElement.Create(
                        name: "Back",
                        description: string.Empty,
                        action: () => SwitchScreen(previousScreen, previousCursorMenu, true),
                        active: (x) => true,
                        selectInactive: true
                        ),
            ];
            menu = CursorMenu.Create(startingCursorIndex: 0, elements: cursorElements);
            textElements =
                [
                    TextElement.Create(information),
                    TextElement.Create(" "),
                    menu,
                ];
            screen = BoxedScreen.Create(title: contractType, elements: textElements);
            SwitchScreen(screen, menu, previous: true);
        }


        void PickContract()
        {
            IScreen previousScreen = currentScreen;
            CursorMenu previousCursorMenu = currentCursorMenu;
            if (ContractManager.Instance.contractType != "None")
            {
                ErrorMessage(MAIN_MENU_TITLE, () => SwitchScreen(previousScreen, previousCursorMenu, true), "You still have an ongoing contract to finish.");
                return;
            }
            IScreen screen;
            ITextElement[] textElements;
            CursorMenu menu;
            CursorElement[] cursorElements =
            [
                CursorElement.Create(
                        name: RANDOM_MOON_CURSOR_ELEMENT,
                        description: string.Empty,
                        action: ConfirmRandomMoonContract,
                        active: (x) => terminal.groupCredits >= UpgradeBus.Instance.PluginConfiguration.CONTRACT_PRICE,
                        selectInactive: true
                        ),
                CursorElement.Create(
                        name: SPECIFIED_MOON_CURSOR_ELEMENT,
                        description: string.Empty,
                        action: PickSpecifiedMoonContract,
                        active: (x) => terminal.groupCredits >= UpgradeBus.Instance.PluginConfiguration.CONTRACT_SPECIFY_PRICE,
                        selectInactive: true
                        ),
                CursorElement.Create(
                        name: "Back",
                        description: string.Empty,
                        action: () => SwitchScreen(previousScreen, previousCursorMenu, true),
                        active: (x) => true,
                        selectInactive: true
                        ),
            ];
            menu = CursorMenu.Create(startingCursorIndex: 0, elements: cursorElements);
            textElements =
                [
                    TextElement.Create("Select a prompt to specify where you want to get the contract on."),
                    TextElement.Create(" "),
                    menu,
                ];
            screen = BoxedScreen.Create(title: MAIN_MENU_TITLE, elements: textElements);
            SwitchScreen(screen, menu, previous: true);
        }
        void PickSpecifiedMoonContract()
        {
            IScreen previousScreen = currentScreen;
            CursorMenu previousCursorMenu = currentCursorMenu;
            if (terminal.groupCredits < UpgradeBus.Instance.PluginConfiguration.CONTRACT_SPECIFY_PRICE)
            {
                ErrorMessage(MAIN_MENU_TITLE, () => SwitchScreen(previousScreen, previousCursorMenu, true), "Not enough credits to purchase a specified moon contract.");
                return;
            }
            SelectableLevel[] levels = StartOfRound.Instance.levels.Where(x => !x.PlanetName.Contains("Gordion")).ToArray();
            IScreen screen;
            ITextElement[] textElements;
            CursorMenu menu;
            CursorElement[] cursorElements = new CursorElement[levels.Length + 1];
            for( int i = 0; i < levels.Length; i++ )
            {
                int counter = i;
                cursorElements[i] = CursorElement.Create(
                    name: levels[counter].PlanetName,
                    description: string.Empty,
                    action: () => ConfirmSpecifiedMoonContract(levels[counter]),
                    active: (x) => true,
                    selectInactive: false
                    );
            }
            cursorElements[levels.Length] = CursorElement.Create(
                    name: "Back",
                    description: string.Empty,
                    action: () => SwitchScreen(previousScreen, previousCursorMenu, true),
                    active: (x) => true,
                    selectInactive: true
                    );
            menu = CursorMenu.Create(startingCursorIndex: 0, elements: cursorElements);
            textElements =
                [
                    TextElement.Create("Select the moon you wish to get a contract on."),
                    TextElement.Create(" "),
                    menu,
                ];
            screen = BoxedScreen.Create(title: MAIN_MENU_TITLE, elements: textElements);
            SwitchScreen(screen, menu, previous: true);

        }
        void ConfirmSpecifiedMoonContract(SelectableLevel level)
        {
            IScreen previousScreen = currentScreen;
            CursorMenu previousCursorMenu = currentCursorMenu;
            Confirm(MAIN_MENU_TITLE, $"Do you wish to purchase a contract on {level.PlanetName} for a price of {UpgradeBus.Instance.PluginConfiguration.CONTRACT_SPECIFY_PRICE.Value} Company credits?", () => PurchaseSpecifiedMoonContract(level, () => SwitchScreen(mainScreen, mainCursorMenu, true)), () => SwitchScreen(previousScreen, previousCursorMenu, true));
        }

        void PurchaseSpecifiedMoonContract(SelectableLevel level, Action backAction)
        {
            LguStore.Instance.SyncCreditsServerRpc(terminal.groupCredits - UpgradeBus.Instance.PluginConfiguration.CONTRACT_SPECIFY_PRICE.Value);
            int i = UnityEngine.Random.Range(0, CommandParser.contracts.Count);
            if (CommandParser.contracts.Count > 1)
            {
                while (i == ContractManager.Instance.lastContractIndex)
                {
                    i = UnityEngine.Random.Range(0, CommandParser.contracts.Count);
                }
            }
            ContractManager.Instance.contractType = CommandParser.contracts[i];
            ContractManager.Instance.contractLevel = level.PlanetName;

            if (terminal.IsHost || terminal.IsServer) ContractManager.Instance.SyncContractDetailsClientRpc(ContractManager.Instance.contractLevel, i);
            else ContractManager.Instance.ReqSyncContractDetailsServerRpc(ContractManager.Instance.contractLevel, i);
            ErrorMessage(MAIN_MENU_TITLE, backAction, $"A {ContractManager.Instance.contractType} contract has been accepted on the moon {ContractManager.Instance.contractLevel}! {CommandParser.contractInfos[i]}");
        }

        void ConfirmRandomMoonContract()
        {
            IScreen previousScreen = currentScreen;
            CursorMenu previousCursorMenu = currentCursorMenu;
            if (terminal.groupCredits < UpgradeBus.Instance.PluginConfiguration.CONTRACT_PRICE)
            {
                ErrorMessage(MAIN_MENU_TITLE, () => SwitchScreen(previousScreen, previousCursorMenu, true), "Not enough credits to purchase a randomized moon contract.");
                return;
            }
            Confirm(MAIN_MENU_TITLE, $"Do you wish to purchase a randomized moon contract for a price of {UpgradeBus.Instance.PluginConfiguration.CONTRACT_PRICE.Value} Company credits?", () => PurchaseRandomMoonContract(() => SwitchScreen(mainScreen, mainCursorMenu, true)), () => SwitchScreen(previousScreen, previousCursorMenu, true));
        }

        void PurchaseRandomMoonContract(Action backAction)
        {
            LguStore.Instance.SyncCreditsServerRpc(terminal.groupCredits - UpgradeBus.Instance.PluginConfiguration.CONTRACT_PRICE.Value);
            int i = UnityEngine.Random.Range(0, CommandParser.contracts.Count);
            if (CommandParser.contracts.Count > 1)
            {
                while (i == ContractManager.Instance.lastContractIndex)
                {
                    i = UnityEngine.Random.Range(0, CommandParser.contracts.Count);
                }
            }
            ContractManager.Instance.contractType = CommandParser.contracts[i];
            ContractManager.RandomLevel();
            if (terminal.IsHost || terminal.IsServer) ContractManager.Instance.SyncContractDetailsClientRpc(ContractManager.Instance.contractLevel, i);
            else ContractManager.Instance.ReqSyncContractDetailsServerRpc(ContractManager.Instance.contractLevel, i);
            ErrorMessage(MAIN_MENU_TITLE, backAction, $"A {ContractManager.Instance.contractType} contract has been accepted on the moon {ContractManager.Instance.contractLevel}! {CommandParser.contractInfos[i]}");
        }

        void ShowCurrentContract()
        {
            IScreen previousScreen = currentScreen;
            CursorMenu previousCursorMenu = currentCursorMenu;
            if (ContractManager.Instance.contractType == "None")
            {
                ErrorMessage(MAIN_MENU_TITLE, () => SwitchScreen(previousScreen, previousCursorMenu, true), "You currently do not have an assigned contract.");
                return;
            }
            IScreen screen;
            ITextElement[] textElements;
            CursorMenu menu;
            CursorElement[] cursorElements =
            [
                CursorElement.Create(
                        name: "Cancel Current Contract",
                        description: string.Empty,
                        action: ConfirmCancelContract,
                        active: (x) => true,
                        selectInactive: true
                        ),
                CursorElement.Create(
                        name: "Back",
                        description: string.Empty,
                        action: () => SwitchScreen(previousScreen, previousCursorMenu, true),
                        active: (x) => true,
                        selectInactive: true
                        ),
            ];
            menu = CursorMenu.Create(startingCursorIndex: 0, elements: cursorElements);
            textElements =
                [
                    TextElement.Create(CommandParser.contractInfos[CommandParser.contracts.IndexOf(ContractManager.Instance.contractType)]),
                    TextElement.Create(" "),
                    menu,
                ];
            screen = BoxedScreen.Create(title: ContractManager.Instance.contractType + " - " + ContractManager.Instance.contractLevel, elements: textElements);
            SwitchScreen(screen, menu, previous: true);
        }

        void ConfirmCancelContract()
        {
            IScreen previousScreen = currentScreen;
            CursorMenu previousCursorMenu = currentCursorMenu;
            Confirm(MAIN_MENU_TITLE, $"Do you wish to cancel the current contract?", () => CancelContract(() => SwitchScreen(mainScreen, mainCursorMenu, true)), () => SwitchScreen(previousScreen, previousCursorMenu, true));
        }

        void CancelContract(Action backAction)
        {
            if (terminal.IsHost || terminal.IsServer) ContractManager.Instance.SyncContractDetailsClientRpc("None", -1);
            else ContractManager.Instance.ReqSyncContractDetailsServerRpc("None", -1);
            ErrorMessage(MAIN_MENU_TITLE, backAction, LguConstants.CONTRACT_CANCEL_CONFIRM_PROMPT_SUCCESS);
        }
    }
}
