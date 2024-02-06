namespace MoreShipUpgrades.UpgradeComponents.Interfaces
{
    /// <summary>
    /// Interfaces given to upgrades which have text which give some provide world building (entirely cosmetic, no logical operations are executed)
    /// </summary>
    internal interface IUpgradeWorldBuilding
    {
        /// <summary>
        /// Shows the upgrade's world building text for the player
        /// </summary>
        /// <param name="shareStatus">Wether the selected upgrade is shared between players or not</param>
        /// <returns>Upgrade's World building text based on its share status</returns>
        string GetWorldBuildingText(bool shareStatus = false);
    }
}
