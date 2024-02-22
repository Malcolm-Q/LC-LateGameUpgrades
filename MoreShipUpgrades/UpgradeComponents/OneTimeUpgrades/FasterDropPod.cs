using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc.Upgrades;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace MoreShipUpgrades.UpgradeComponents.OneTimeUpgrades
{
    class FasterDropPod : OneTimeUpgrade
    {
        public const string UPGRADE_NAME = "Faster Drop Ship";
        public static FasterDropPod Instance;

        internal override void Start()
        {
            upgradeName = UPGRADE_NAME;
            base.Start();
        }

        void Awake()
        {
            Instance = this;
        }

        public static float GetUpgradedTimer(float defaultLimit)
        {
            if (!GetActiveUpgrade(UPGRADE_NAME)) return defaultLimit;
            return UpgradeBus.Instance.PluginConfiguration.FASTER_DROP_POD_TIMER.Value;
        }

        public override string GetDisplayInfo(int price = -1)
        {
            return "Make the drop pod land faster.";
        }
    }
}
