using BepInEx.Logging;
using MoreShipUpgrades.Managers;
using MoreShipUpgrades.UpgradeComponents.Interfaces;

namespace MoreShipUpgrades.Misc.Upgrades
{
    /// <summary>
    /// Class which represents upgrades that are only purchased only once to gain the full effect of it
    /// </summary>
    abstract class OneTimeUpgrade : BaseUpgrade, IOneTimeUpgradeDisplayInfo
    {
        #region Interface Methods
        public abstract string GetDisplayInfo(int price = -1);
        #endregion
    }
}
