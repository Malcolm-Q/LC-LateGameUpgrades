using GameNetcodeStuff;
using MoreShipUpgrades.Input;
using MoreShipUpgrades.Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

namespace MoreShipUpgrades.Misc.UI
{
    internal class UpgradesStore : MonoBehaviour
    {
        static UpgradesStore Instance;
        UpgradeApplication cursorElements;
        Terminal terminalReference;
        TerminalNode lastTerminalNode;
        void Start()
        {
            Instance = this;
            cursorElements = new UpgradeApplication(UpgradeBus.Instance.terminalNodes.ToArray());
            terminalReference = UpgradeBus.Instance.GetTerminal();
            lastTerminalNode = terminalReference.currentNode;
            StartCoroutine(UpdateInput(false));
            UpdateInputBindings(enable: true);
        }
        void Update()
        {
            if (terminalReference == null) return;
            if (cursorElements == null) return;
            terminalReference.screenText.text = cursorElements.GetText();
            terminalReference.currentText = cursorElements.GetText();
        }
        void OnDestroy()
        {
            UpdateInputBindings(enable : false);
            terminalReference.LoadNewNode(lastTerminalNode);
            StartCoroutine(UpdateInput(true));
        }
        internal IEnumerator UpdateInput(bool enable)
        {
            yield return new WaitForEndOfFrame();
            if (enable)
            {
                terminalReference.screenText.interactable = true;
                terminalReference.screenText.ActivateInputField();
                terminalReference.screenText.Select();
            }
            else
            {
                terminalReference.screenText.DeactivateInputField();
                terminalReference.screenText.interactable = false;
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
            terminalReference.playerActions.Movement.OpenMenu.performed -= terminalReference.PressESC;
        }
        void RemoveInputBindings()
        {
            Keybinds.cursorUpAction.performed -= OnUpgradeStoreCursorUp;
            Keybinds.cursorDownAction.performed -= OnUpgradeStoreCursorDown;
            Keybinds.cursorExitAction.performed -= OnUpgradeStoreCursorExit;
            Keybinds.pageUpAction.performed -= OnUpgradeStorePageUp;
            Keybinds.pageDownAction.performed -= OnUpgradeStorePageDown;
            terminalReference.playerActions.Movement.OpenMenu.performed += terminalReference.PressESC;
        }
        void MoveCursorUp()
        {
            cursorElements.Backward();
        }
        void MoveCursorDown()
        {
            cursorElements.Forward();
        }
        void MovePageUp()
        {
            cursorElements.PageUp();
        }
        void MovePageDown()
        {
            cursorElements.PageDown();
        }
        static void OnUpgradeStoreCursorUp(CallbackContext context)
        {
            Instance.MoveCursorUp();
        }
        static void OnUpgradeStoreCursorDown(CallbackContext context)
        {
            Instance.MoveCursorDown();
        }
        static void OnUpgradeStorePageUp(CallbackContext context)
        {
            Instance.MovePageUp();
        }
        static void OnUpgradeStorePageDown(CallbackContext context)
        {
            Instance.MovePageDown();
        }
        static void OnUpgradeStoreCursorExit(CallbackContext context)
        {
            Destroy(Instance);
        }
    }
}
