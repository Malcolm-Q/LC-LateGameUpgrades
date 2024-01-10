using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc;
using System;
using System.Collections.Generic;
using System.Text;

namespace MoreShipUpgrades.UpgradeComponents.Wheelbarrow
{
    internal class ScrapWheelbarrow : WheelbarrowScript
    {
        private static LGULogger logger = new LGULogger(nameof(ScrapWheelbarrow));
        public override void Start()
        {
            base.Start();
            maximumAmountItems = UpgradeBus.instance.cfg.SCRAP_WHEELBARROW_MAXIMUM_AMOUNT_ITEMS;
            weightReduceMultiplier = UpgradeBus.instance.cfg.SCRAP_WHEELBARROW_WEIGHT_REDUCTION_MULTIPLIER;
            defaultWeight = itemProperties.weight;
            logger.LogDebug("Spawned in the scene!");
        }
    }
}
