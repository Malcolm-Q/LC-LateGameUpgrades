﻿using MoreShipUpgrades.Managers;
using MoreShipUpgrades.UpgradeComponents.OneTimeUpgrades;
using System.Collections.Generic;
using Unity.Netcode;

namespace MoreShipUpgrades.Misc.Upgrades
{
    /// <summary>
    /// Class which represents the many upgrades present in the Lategame Upgrades' store for purchase for extra effects such as attribute increases, new enviromental scenarios and more...
    /// </summary>
    abstract class BaseUpgrade : NetworkBehaviour
    {
        #region Variables
        /// <summary>
        /// Name of the upgrade
        /// </summary>
        protected string upgradeName = "Base Upgrade";
        #endregion
        #region Constants
        public const string INDIVIDUAL_SECTION = "Individual Purchase";
        public const bool INDIVIDUAL_DEFAULT = true;
        public const string INDIVIDUAL_DESCRIPTION = "If true: upgrade will apply only to the client that purchased it. (Overriden by 'Convert all upgrades to be shared' option in Misc section)";

        public const string PRICES_SECTION = "Price of each additional upgrade";
        public const string PRICES_DESCRIPTION = "Value must be seperated by commas EX: '123,321,222'";
        #endregion
        #region Virtual Methods
        internal virtual void Start()
        {
            DontDestroyOnLoad(gameObject);
            Register();
        }
        /// <summary>
        /// Function called when the upgrade is being purchased for the first time
        /// </summary>
        public virtual void Load()
        {
            UpgradeBus.Instance.activeUpgrades[upgradeName] = true;
            if (!UpgradeBus.Instance.PluginConfiguration.SHOW_UPGRADES_CHAT.Value) return;
            ShowUpgradeNotification("#FF0000", $"{upgradeName} is active!");
        }
        /// <summary>
        /// Function responsible to insert this upgrade's gameObject into the UpgradeBus' list of gameObjects for handling
        /// </summary>
        public virtual void Register()
        {
            if (!UpgradeBus.Instance.UpgradeObjects.ContainsKey(upgradeName)) { UpgradeBus.Instance.UpgradeObjects.Add(upgradeName, gameObject); }
        }
        /// <summary>
        /// Function called when the upgrade is being unloaded or the save is being reset
        /// </summary>
        public virtual void Unwind()
        {
            UpgradeBus.Instance.activeUpgrades[upgradeName] = false;
            if (!UpgradeBus.Instance.PluginConfiguration.SHOW_UPGRADES_CHAT.Value) return;
            ShowUpgradeNotification("#FF0000", $"{upgradeName} has been disabled!");
        }
        /// <summary>
        /// Shows a notification for when an upgrade is loaded or unloaded from the player
        /// </summary>
        /// <param name="message">Message displayed to the player</param>
        void ShowUpgradeNotification(string colourHex, string message)
        {
            string notification = $"\n<color={colourHex}>{message}</color>";
            HUDManager.Instance.chatText.text += notification;
        }

        #endregion
        internal static bool GetActiveUpgrade(string upgradeName)
        {
            return UpgradeBus.Instance.activeUpgrades.GetValueOrDefault(upgradeName, false);
        }

        internal static int GetUpgradeLevel(string upgradeName)
        {
            return UpgradeBus.Instance.upgradeLevels.GetValueOrDefault(upgradeName, 0);
        }
    }
}
