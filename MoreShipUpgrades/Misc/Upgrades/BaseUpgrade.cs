using MoreShipUpgrades.Managers;
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
            string loadColour = "#FF0000";
            string loadMessage = $"\n<color={loadColour}>{upgradeName} is active!</color>";
            HUDManager.Instance.chatText.text += loadMessage;
            UpgradeBus.instance.activeUpgrades[upgradeName] = true;
        }
        /// <summary>
        /// Function responsible to insert this upgrade's gameObject into the UpgradeBus' list of gameObjects for handling
        /// </summary>
        public virtual void Register()
        {
            if (!UpgradeBus.instance.UpgradeObjects.ContainsKey(upgradeName)) { UpgradeBus.instance.UpgradeObjects.Add(upgradeName, gameObject); }
        }
        /// <summary>
        /// Function called when the upgrade is being unloaded or the save is being reset
        /// </summary>
        public virtual void Unwind()
        {
            string unloadColour = "#FF0000";
            string unloadMessage = $"\n<color={unloadColour}>{upgradeName} has been disabled!</color>";
            HUDManager.Instance.chatText.text += unloadMessage;
            UpgradeBus.instance.activeUpgrades[upgradeName] = false;
        }
        #endregion
        internal static bool GetActiveUpgrade(string upgradeName)
        {
            return UpgradeBus.instance.activeUpgrades.GetValueOrDefault(upgradeName, false);
        }

        internal static int GetUpgradeLevel(string upgradeName)
        {
            return UpgradeBus.instance.upgradeLevels.GetValueOrDefault(upgradeName, 0);
        }
    }
}
