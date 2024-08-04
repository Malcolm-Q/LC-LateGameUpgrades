using UnityEngine;

namespace MoreShipUpgrades.Managers
{
    internal class PlayerManager : MonoBehaviour
    {
        internal int upgradeSpendCredits = 0;
        internal static PlayerManager instance;
        void Awake()
        {
            instance = this;
        }

        internal void IncreaseUpgradeSpentCredits(int amount)
        {
            upgradeSpendCredits += amount;
        }

        internal void ResetUpgradeSpentCredits()
        {
            upgradeSpendCredits = 0;
        }

        internal int GetUpgradeSpentCredits()
        {
            return upgradeSpendCredits;
        }
    }
}
