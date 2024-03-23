﻿using MoreShipUpgrades.Managers;
using MoreShipUpgrades.UpgradeComponents.Interfaces;

namespace MoreShipUpgrades.Misc.Upgrades
{
    /// <summary>
    /// Class which represents upgrades that can be purchased more than once for additional effects
    /// </summary>
    abstract class TierUpgrade : BaseUpgrade, ITierUpgradeDisplayInfo
    {
        #region Overriden Methods

        public override void Unwind()
        {
            base.Unwind();
            UpgradeBus.Instance.upgradeLevels[upgradeName] = 0;
        }
        #endregion
        #region Interface Methods
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
