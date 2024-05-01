using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc.UI.Application;
using MoreShipUpgrades.Misc.Util;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace MoreShipUpgrades.Misc.UI
{
    internal class LguInteractiveTerminal : MonoBehaviour
    {
        internal static Dictionary<string, Func<TerminalApplication>> registeredApplications = new Dictionary<string, Func<TerminalApplication>> ();
            public static LguInteractiveTerminal Instance;
        TerminalApplication mainApplication;
        Terminal terminalReference;
        TerminalNode lastTerminalNode;
        Color previousCaretColor;

        void Start()
        {
            Instance = this;
            terminalReference = UpgradeBus.Instance.GetTerminal();
            lastTerminalNode = terminalReference.currentNode;
            UpdateInput(false);
        }
        internal void Initialize(string command)
        {
            Func<TerminalApplication> function = registeredApplications.GetValueOrDefault(command, null);
            if (function == null)
            {
                Plugin.mls.LogError("An application was not selected to change the terminal's text.");
                return;
            }
            mainApplication = registeredApplications.GetValueOrDefault(command, null).Invoke();
            if (mainApplication == null)
            {
                Plugin.mls.LogError("The selected application doesn't have a valid constructor.");
                return;
            }

            mainApplication.Initialization();
            mainApplication.UpdateInputBindings(enable: true);
        }
        void Update()
        {
            if (terminalReference == null) return;
            if (mainApplication == null) return;
            mainApplication.UpdateText();
        }

        void OnDestroy()
        {
            mainApplication.UpdateInputBindings(enable : false);
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
                terminalReference.screenText.caretColor = LGUConstants.Invisible;
            }
        }
        public static bool UpgradeStoreBeingUsed()
        {
            return Instance != null;
        }
        public static bool ContainsApplication(string command)
        {
            return registeredApplications.ContainsKey(command);
        }
        public static void RegisterApplication<T>(string command) where T : TerminalApplication , new()
        {
            if (registeredApplications.ContainsKey(command))
            {
                Plugin.mls.LogError($"An application has already been registered under the command \"{command}\"");
                return;
            }
            registeredApplications.Add(command, () => new T());
        }
    }
}
