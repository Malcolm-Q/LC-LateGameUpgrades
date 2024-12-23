using MoreShipUpgrades.Managers;

namespace MoreShipUpgrades.Misc.Upgrades
{
    /// <summary>
    /// Class which represents upgrades that can be purchased more than once for additional effects
    /// </summary>
    public abstract class TierUpgrade : BaseUpgrade
    {

        #region Overriden Methods

        public override void Unwind()
        {
            base.Unwind();
            UpgradeBus.Instance.upgradeLevels[upgradeName] = 0;
        }
        #endregion
        #region Interface Methods
        /// <summary>
        /// Shows the info related to the selected upgrade
        /// </summary>
        /// <param name="initialPrice">Price when purchasing the upgrade for the first time</param>
        /// <param name="maxLevels">Maximum amount of times the upgrade can be purchased</param>
        /// <param name="incrementalPrices">Prices for each increase of level of the upgrade</param>
        /// <returns>The info associated to the upgrade for player readibility</returns>
        public abstract string GetDisplayInfo(int initialPrice = -1, int maxLevels = -1, int[] incrementalPrices = null);
        #endregion
        #region Abstract Methods
        public virtual void Increment()
        {
            UpgradeBus.Instance.upgradeLevels[upgradeName] = GetUpgradeLevel(upgradeName) + 1;
        }
        #endregion
    }
}
