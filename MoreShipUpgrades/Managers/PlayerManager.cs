using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace MoreShipUpgrades.Managers
{
    internal class PlayerManager : MonoBehaviour
    {
        internal const float DEFAULT_MULTIPLIER = 1.0f;
        internal float sensitivityMultiplier = DEFAULT_MULTIPLIER;
        internal float sloppyMultiplier = DEFAULT_MULTIPLIER;
        internal bool holdingWheelbarrow = false;
        internal int upgradeSpendCredits = 0;
        internal static PlayerManager instance;
        void Awake()
        {
            instance = this;
        }

        internal void SetSensitivityMultiplier(float sensitivityMultiplier)
        {
            this.sensitivityMultiplier = sensitivityMultiplier;
        }

        internal void SetSloppyMultiplier(float sloppyMultiplier)
        {
            this.sloppyMultiplier = sloppyMultiplier;
        }

        internal void SetHoldingWheelbarrow(bool holdingWheelbarrow)
        {
            this.holdingWheelbarrow = holdingWheelbarrow;
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

        internal void ResetSensitivityMultiplier()
        {
            this.sensitivityMultiplier = DEFAULT_MULTIPLIER;
        }
        internal void ResetSloppyMultiplier()
        {
            this.sloppyMultiplier = DEFAULT_MULTIPLIER;
        }

        public float GetSensitivityMultiplier()
        {
            return sensitivityMultiplier;
        }
        public float GetSloppyMultiplier()
        {
            return sloppyMultiplier;
        }
        public bool GetHoldingWheelbarrow()
        {
            return holdingWheelbarrow;
        }
    }
}
