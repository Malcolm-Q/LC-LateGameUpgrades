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
            Enum.TryParse(typeof(Restrictions), UpgradeBus.instance.cfg.SCRAP_WHEELBARROW_RESTRICTION_MODE, out object parsedRestriction);
            if (parsedRestriction == null)
            {
                logger.LogError($"An error occured parsing the restriction mode ({UpgradeBus.instance.cfg.SCRAP_WHEELBARROW_RESTRICTION_MODE}), defaulting to ItemCount");
                restriction = Restrictions.ItemCount;
            }
            else restriction = (Restrictions)parsedRestriction;
            maximumWeightAllowed = UpgradeBus.instance.cfg.SCRAP_WHEELBARROW_MAXIMUM_WEIGHT_ALLOWED;
            noiseRange = UpgradeBus.instance.cfg.SCRAP_WHEELBARROW_NOISE_RANGE;
            sloppiness = UpgradeBus.instance.cfg.SCRAP_WHEELBARROW_MOVEMENT_SLOPPY;
            lookSensitivityDrawback = UpgradeBus.instance.cfg.SCRAP_WHEELBARROW_LOOK_SENSITIVITY_DRAWBACK;
            logger.LogDebug("Spawned in the scene!");
        }
    }
}
