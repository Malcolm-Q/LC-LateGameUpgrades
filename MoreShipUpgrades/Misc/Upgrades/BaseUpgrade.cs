using MoreShipUpgrades.UpgradeComponents.OneTimeUpgrades;
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
        #region Abstract Methods
        internal abstract void Start();
        /// <summary>
        /// Function called when the upgrade is being purchased for the first time
        /// </summary>
        public abstract void Load();
        /// <summary>
        /// Function responsible to insert this upgrade's gameObject into the UpgradeBus' list of gameObjects for handling
        /// </summary>
        public abstract void Register();
        /// <summary>
        /// Function called when the upgrade is being unloaded or the save is being reset
        /// </summary>
        public abstract void Unwind();
        #endregion
    }
}
