namespace MoreShipUpgrades.UpgradeComponents.Interfaces
{
    /// <summary>
    /// Interface for upgrades which are only purchased once so the only factor that can change the info display is the price set for the selected upgrade
    /// </summary>
    internal interface IOneTimeUpgradeDisplayInfo
    {
        /// <summary>
        /// Shows the info related to the selected upgrade
        /// </summary>
        /// <param name="price">Price set for the selected upgrade</param>
        /// <returns>The info associated to the upgrade for player readibility</returns>
        string GetDisplayInfo(int price = -1);
    }
}
