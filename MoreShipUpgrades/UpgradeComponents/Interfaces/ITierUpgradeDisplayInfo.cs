namespace MoreShipUpgrades.UpgradeComponents.Interfaces
{
    /// <summary>
    /// Interface for upgrades which can be purchased multiple times so we have to show each effect whenever a new level is purchased
    /// </summary>
    internal interface ITierUpgradeDisplayInfo
    {
        /// <summary>
        /// Shows the info related to the selected upgrade
        /// </summary>
        /// <param name="initialPrice">Price when purchasing the upgrade for the first time</param>
        /// <param name="maxLevels">Maximum amount of times the upgrade can be purchased</param>
        /// <param name="incrementalPrices">Prices for each increase of level of the upgrade</param>
        /// <returns>The info associated to the upgrade for player readibility</returns>
        string GetDisplayInfo(int initialPrice = -1, int maxLevels = -1, int[] incrementalPrices = null);
    }
}
