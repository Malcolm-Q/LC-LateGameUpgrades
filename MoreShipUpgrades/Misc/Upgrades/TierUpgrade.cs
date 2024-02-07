using MoreShipUpgrades.Managers;
using MoreShipUpgrades.UpgradeComponents.Interfaces;

namespace MoreShipUpgrades.Misc.Upgrades
{
    /// <summary>
    /// Class which represents upgrades that can be purchased more than once for additional effects
    /// </summary>
    abstract class TierUpgrade : BaseUpgrade, ITierUpgradeDisplayInfo
    {
        #region Overriden Methods
        internal override void Start()
        {
            DontDestroyOnLoad(gameObject);
            Register();
        }
        /// <summary>
        /// Handles the upgrade being purchased past the unlock phase
        /// </summary>
        public override void Load()
        {
            string loadColour = "#FF0000";
            string loadMessage = $"\n<color={loadColour}>{upgradeName} is active!</color>";
            HUDManager.Instance.chatText.text += loadMessage;
        }

        public override void Register()
        {
            if (!UpgradeBus.instance.UpgradeObjects.ContainsKey(upgradeName)) { UpgradeBus.instance.UpgradeObjects.Add(upgradeName, gameObject); }
        }

        public override void Unwind()
        {
            string unloadColour = "#FF0000";
            string unloadMessage = $"\n<color={unloadColour}>{upgradeName} has been disabled!</color>";
            HUDManager.Instance.chatText.text += unloadMessage;
        }
        #endregion
        #region Interface Methods
        public abstract string GetDisplayInfo(int initialPrice = -1, int maxLevels = -1, int[] incrementalPrices = null);
        #endregion
        #region Abstract Methods
        public abstract void Increment();
        #endregion
    }
}
