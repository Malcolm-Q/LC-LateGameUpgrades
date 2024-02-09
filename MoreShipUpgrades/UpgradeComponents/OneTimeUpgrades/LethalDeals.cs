﻿using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc.Upgrades;
using System;
using System.Collections.Generic;
using System.Text;

namespace MoreShipUpgrades.UpgradeComponents.OneTimeUpgrades
{
    internal class LethalDeals : OneTimeUpgrade
    {
        public const string UPGRADE_NAME = "Lethal Deals";
        const int GUARANTEED_ITEMS_AMOUNT = 1;
        internal override void Start()
        {
            upgradeName = UPGRADE_NAME;
            base.Start();
        }

        public override void Load()
        {
            base.Load();

            UpgradeBus.instance.lethalDeals = true;
        }

        public override void Unwind()
        {
            base.Unwind();
            UpgradeBus.instance.lethalDeals = false;
        }
        public static int GetLethalDealsGuaranteedItems(int amount)
        {
            if (!UpgradeBus.instance.lethalDeals) return amount;
            return GUARANTEED_ITEMS_AMOUNT;
        }
        public override string GetDisplayInfo(int price = -1)
        {
            return $"${price} - Guarantees at least one item will be on sale in the store.";
        }
    }
}
