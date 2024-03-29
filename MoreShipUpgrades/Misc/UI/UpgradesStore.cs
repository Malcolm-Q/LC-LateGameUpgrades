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
        MainUpgradeApplication mainUpgradeApplication;
        Terminal terminalReference;
        TerminalNode lastTerminalNode;
        bool updateText;
        void Start()
        {
            Instance = this;
            cursorElements = new UpgradeApplication(UpgradeBus.Instance.terminalNodes.ToArray());
            mainUpgradeApplication = new MainUpgradeApplication();
            mainUpgradeApplication.Initialization();
            terminalReference = UpgradeBus.Instance.GetTerminal();
            lastTerminalNode = terminalReference.currentNode;
            StartCoroutine(UpdateInput(false));
            UpdateInputBindings(enable: true);
            updateText = true;
        }
        void Update()
        {
            if (terminalReference == null) return;
            if (cursorElements == null) return;
            if (!updateText) return;
            //terminalReference.screenText.text = cursorElements.GetText();
            //terminalReference.currentText = cursorElements.GetText();
            mainUpgradeApplication.UpdateText();
            updateText = true;
        }
        void OnDestroy()
        {
            UpdateInputBindings(enable : false);
            terminalReference.LoadNewNode(lastTerminalNode);
            terminalReference.screenText.interactable = true;
            terminalReference.screenText.ActivateInputField();
            terminalReference.screenText.Select();
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
            cursorElements.Backward();
            mainUpgradeApplication.MoveCursorUp();
            updateText = true;
        }
        void MoveCursorDown()
        {
            cursorElements.Forward();
            mainUpgradeApplication.MoveCursorDown();
            updateText = true;
        }
        void MovePageUp()
        {
            cursorElements.PageUp();
            mainUpgradeApplication.MovePageUp();
            updateText = true;
        }
        void MovePageDown()
        {
            cursorElements.PageDown();
            mainUpgradeApplication.MovePageDown();
            updateText = true;
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
