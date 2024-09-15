namespace MoreShipUpgrades.Misc.Upgrades
{
    /// <summary>
    /// Class which represents upgrades that are only purchased only once to gain the full effect of it
    /// </summary>
    public abstract class OneTimeUpgrade : BaseUpgrade
    {
        #region Interface Methods
        /// <summary>
        /// Shows the info related to the selected upgrade
        /// </summary>
        /// <param name="price">Price set for the selected upgrade</param>
        /// <returns>The info associated to the upgrade for player readibility</returns>
        public abstract string GetDisplayInfo(int price = -1);
        #endregion
    }
}
