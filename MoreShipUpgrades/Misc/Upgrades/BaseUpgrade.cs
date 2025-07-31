using MoreShipUpgrades.Configuration;
using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc.Util;
using MoreShipUpgrades.UI.TerminalNodes;
using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace MoreShipUpgrades.Misc.Upgrades
{
    /// <summary>
    /// Class which represents the many upgrades present in the Lategame Upgrades' store for purchase for extra effects such as attribute increases, new enviromental scenarios and more...
    /// </summary>
    public abstract class BaseUpgrade : NetworkBehaviour
    {
        #region Variables
        /// <summary>
        /// Name of the upgrade
        /// </summary>
        protected string upgradeName = "Base Upgrade";

        /// <summary>
        /// Overriden name of the upgrade
        /// </summary>
        protected string overridenUpgradeName;
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
            gameObject.hideFlags = HideFlags.HideAndDontSave;
            Register();
        }
        /// <summary>
        /// Function called when the upgrade is being purchased for the first time
        /// </summary>
        public virtual void Load()
        {
            LategameConfiguration config = GetConfiguration();
            UpgradeBus.Instance.activeUpgrades[upgradeName] = true;
            if (!config.SHOW_UPGRADES_CHAT.LocalValue) return;
            ShowUpgradeNotification(LguConstants.UPGRADE_UNLOADED_NOTIFICATION_DEFAULT_COLOR, $"{(config.OVERRIDE_UPGRADE_NAMES ? overridenUpgradeName : upgradeName)} is active!");
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
            LategameConfiguration config = GetConfiguration();
            UpgradeBus.Instance.activeUpgrades[upgradeName] = false;
            if (!config.SHOW_UPGRADES_CHAT.LocalValue) return;
            ShowUpgradeNotification(LguConstants.UPGRADE_LOADED_NOTIFICATION_DEFAULT_COLOR, $"{(config.OVERRIDE_UPGRADE_NAMES ? overridenUpgradeName : upgradeName)} has been disabled!");
        }
        /// <summary>
        /// Shows a notification for when an upgrade is loaded or unloaded from the player
        /// </summary>
        /// <param name="message">Message displayed to the player</param>
        protected void ShowUpgradeNotification(string colourHex, string message)
        {
            string notification = $"\n<color={colourHex}>{message}</color>";
            HUDManager.Instance.chatText.text += notification;
        }

        #endregion
        /// <summary>
        /// Returns if the upgrade with provided name has been activated/loaded or not
        /// </summary>
        /// <param name="upgradeName">Name of the upgrade we wish to check its activity</param>
        /// <returns>The given upgrade is active in the game or not</returns>
        public static bool GetActiveUpgrade(string upgradeName)
        {
            return UpgradeBus.Instance.activeUpgrades.GetValueOrDefault(upgradeName, false);
        }
        /// <summary>
        /// Returns the current level for a given upgrade. <para></para>
        /// 0 is considered level 1 in any UI that shows this stat and so on
        /// </summary>
        /// <param name="upgradeName">Name of the upgrade we wish to check the level of</param>
        /// <returns>Currentl level of the upgrade</returns>
        public static int GetUpgradeLevel(string upgradeName)
        {
            return UpgradeBus.Instance.upgradeLevels.GetValueOrDefault(upgradeName, 0);
        }
        /// <summary>
        /// Returns the configuration used for the mod
        /// </summary>
        /// <returns>Returns the configuration used for the mod</returns>
        public static LategameConfiguration GetConfiguration()
        {
            return UpgradeBus.Instance.PluginConfiguration;
        }

        /// <summary>
        /// </summary>
        /// <returns>Wether the upgrade can be loaded immediately upon game being loaded or not</returns>
        public abstract bool CanInitializeOnStart { get; }

        public static string GetUpgradePrice(int price, PurchaseMode mode)
        {
            if (!CurrencyManager.Enabled) return $"${price}";
            mode = GetConfiguration().AlternativeCurrencyConfiguration.EnableGlobalPurchase ? GetConfiguration().AlternativeCurrencyConfiguration.GlobalPurchaseMode : mode;
            switch (mode)
            {
                case PurchaseMode.Both: return $"${price}/{CurrencyManager.Instance.GetCurrencyAmountFromCredits(price)} PC";
                case PurchaseMode.AlternateCurrency: return $"{CurrencyManager.Instance.GetCurrencyAmountFromCredits(price)} PC";
                case PurchaseMode.CompanyCredits: return $"${price}";
                default: return string.Empty;
            }
        }

        /// <summary>
        /// Registers the associated upgrade to the game to be initialized correctly
        /// <para></para>
        /// This method is to be overriden by their subclasses through "new"
        /// </summary>
        /// <exception cref="NotSupportedException"></exception>
        public static void RegisterUpgrade() { throw new NotSupportedException(); }

        /// <summary>
        /// Registers the associated terminal node of the upgrade
        /// <para></para>
        /// This method is to be overriden by their subclasses through "new"
        /// </summary>
        /// <exception cref="NotSupportedException"></exception>
        public static CustomTerminalNode RegisterTerminalNode() { throw new NotSupportedException(); }

        /// <summary>
        /// Registers the scrap associated with the upgrade relevant for Item Progression mode
        /// <para></para>
        /// This method is to be overriden by their subclasses through "new"
        /// </summary>
        /// <exception cref="NotSupportedException"></exception>
        public static (string,string[]) RegisterScrapToUpgrade()
        {
            throw new NotSupportedException();
        }

        internal static void SetupGenericPerk<T>(string upgradeName) where T : Component
        {
            Tools.SetupGameObject<T>(upgradeName);
        }
    }
}
