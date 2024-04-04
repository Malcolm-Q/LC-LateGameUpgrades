using MoreShipUpgrades.Input;
using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc.Util;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

namespace MoreShipUpgrades.Misc.UI
{
    internal class UpgradesStore : MonoBehaviour
    {
        static UpgradesStore Instance;
        MainUpgradeApplication mainUpgradeApplication;
        Terminal terminalReference;
        TerminalNode lastTerminalNode;
        Color previousCaretColor;
        void Start()
        {
            Instance = this;
            mainUpgradeApplication = new MainUpgradeApplication();
            mainUpgradeApplication.Initialization();
            terminalReference = UpgradeBus.Instance.GetTerminal();
            lastTerminalNode = terminalReference.currentNode;
            UpdateInput(false);
            UpdateInputBindings(enable: true);
        }
        void Update()
        {
            if (terminalReference == null) return;
            if (mainUpgradeApplication == null) return;
            mainUpgradeApplication.UpdateText();
        }

        void OnDestroy()
        {
            UpdateInputBindings(enable : false);
            terminalReference.LoadNewNode(lastTerminalNode);
            terminalReference.screenText.interactable = true;
            terminalReference.screenText.ActivateInputField();
            terminalReference.screenText.Select();
            UpdateInput(true);
            Instance = null;
        }
        internal void UpdateInput(bool enable)
        {
            if (enable)
            {
                terminalReference.screenText.interactable = true;
                terminalReference.screenText.ActivateInputField();
                terminalReference.screenText.Select();
                terminalReference.screenText.caretColor = previousCaretColor;
            }
            else
            {
                terminalReference.screenText.DeactivateInputField();
                terminalReference.screenText.interactable = false;
                previousCaretColor = terminalReference.screenText.caretColor;
                terminalReference.screenText.caretColor = Constants.Invisible;
            }
        }

        void UpdateInputBindings(bool enable = false)
        {
            if (enable) AddInputBindings();
            else RemoveInputBindings();
        }
        void AddInputBindings()
        {
            Keybinds.cursorUpAction.performed += OnUpgradeStoreCursorUp;
            Keybinds.cursorDownAction.performed += OnUpgradeStoreCursorDown;
            Keybinds.cursorExitAction.performed += OnUpgradeStoreCursorExit;
            Keybinds.pageUpAction.performed += OnUpgradeStorePageUp;
            Keybinds.pageDownAction.performed += OnUpgradeStorePageDown;
            Keybinds.storeConfirmAction.performed += OnUpgradeStoreConfirm;
            terminalReference.playerActions.Movement.OpenMenu.performed -= terminalReference.PressESC;
        }
        void RemoveInputBindings()
        {
            Keybinds.cursorUpAction.performed -= OnUpgradeStoreCursorUp;
            Keybinds.cursorDownAction.performed -= OnUpgradeStoreCursorDown;
            Keybinds.cursorExitAction.performed -= OnUpgradeStoreCursorExit;
            Keybinds.pageUpAction.performed -= OnUpgradeStorePageUp;
            Keybinds.pageDownAction.performed -= OnUpgradeStorePageDown;
            Keybinds.storeConfirmAction.performed -= OnUpgradeStoreConfirm;
            terminalReference.playerActions.Movement.OpenMenu.performed += terminalReference.PressESC;
        }
        void MoveCursorUp()
        {
            mainUpgradeApplication.MoveCursorUp();
        }
        void MoveCursorDown()
        {
            mainUpgradeApplication.MoveCursorDown();
        }
        void MovePageUp()
        {
            mainUpgradeApplication.MovePageUp();
        }
        void MovePageDown()
        {
            mainUpgradeApplication.MovePageDown();
        }
        void Submit()
        {
            mainUpgradeApplication.Submit();
        }
        static void OnUpgradeStoreConfirm(CallbackContext context)
        {
            Instance.Submit();
        }
        static void OnUpgradeStoreCursorUp(CallbackContext context)
        {
            Instance.MoveCursorUp();
        }
        static void OnUpgradeStoreCursorDown(CallbackContext context)
        {
            Instance.MoveCursorDown();
        }
        internal static void OnUpgradeStorePageUp(CallbackContext context)
        {
            Instance.MovePageUp();
        }
        internal static void OnUpgradeStorePageDown(CallbackContext context)
        {
            Instance.MovePageDown();
        }
        static void OnUpgradeStoreCursorExit(CallbackContext context)
        {
            Destroy(Instance);
        }
        public static bool UpgradeStoreBeingUsed()
        {
            return Instance != null;
        }
    }
}
