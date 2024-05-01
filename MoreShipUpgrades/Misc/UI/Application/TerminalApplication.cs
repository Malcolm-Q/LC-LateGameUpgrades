using MoreShipUpgrades.Input;
using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc.UI.Cursor;
using MoreShipUpgrades.Misc.UI.Screen;
using System;
using static UnityEngine.InputSystem.InputAction;

namespace MoreShipUpgrades.Misc.UI.Application
{
    internal abstract class TerminalApplication
    {
        protected IScreen currentScreen;
        protected readonly Terminal terminal = UpgradeBus.Instance.GetTerminal();

        public abstract void Initialization();
        protected abstract string GetApplicationText();
        protected abstract Action PreviousScreen();
        public void UpdateText()
        {
            string text = GetApplicationText();
            terminal.screenText.text = text;
            terminal.currentText = text;
        }
        internal void UpdateInputBindings(bool enable = false)
        {
            if (enable) AddInputBindings();
            else RemoveInputBindings();
        }
        protected virtual void AddInputBindings()
        {
            Keybinds.cursorExitAction.performed += OnUpgradeStoreCursorExit;
            terminal.playerActions.Movement.OpenMenu.performed -= terminal.PressESC;
        }

        protected virtual void RemoveInputBindings()
        {
            Keybinds.cursorExitAction.performed -= OnUpgradeStoreCursorExit;
            terminal.playerActions.Movement.OpenMenu.performed += terminal.PressESC;
        }
        internal void OnUpgradeStoreCursorExit(CallbackContext context)
        {
            UnityEngine.Object.Destroy(LguInteractiveTerminal.Instance);
        }
    }
}
