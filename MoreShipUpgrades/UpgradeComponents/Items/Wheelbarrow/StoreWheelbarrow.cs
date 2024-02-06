using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc;
using MoreShipUpgrades.UpgradeComponents.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace MoreShipUpgrades.UpgradeComponents.Items.Wheelbarrow
{
    internal class StoreWheelbarrow : WheelbarrowScript, IDisplayInfo
    {
        private static LGULogger logger = new LGULogger(nameof(StoreWheelbarrow));
        private GameObject wheel;

        public string GetDisplayInfo()
        {
            return $"A portable container which has a maximum capacity of {UpgradeBus.instance.cfg.WHEELBARROW_MAXIMUM_AMOUNT_ITEMS}" +
                $" and reduces the effective weight of the inserted items by {UpgradeBus.instance.cfg.WHEELBARROW_WEIGHT_REDUCTION_MULTIPLIER * 100} %.\n" +
                $"It weighs {UpgradeBus.instance.cfg.WHEELBARROW_WEIGHT} lbs";
        }

        public override void Start()
        {
            base.Start();
            wheel = GameObject.Find("lgu_wheelbarrow_wheel");
            if (wheel == null) logger.LogError($"Couldn't find the wheel's {nameof(GameObject)} to perform rotations");
            maximumAmountItems = UpgradeBus.instance.cfg.WHEELBARROW_MAXIMUM_AMOUNT_ITEMS;
            weightReduceMultiplier = UpgradeBus.instance.cfg.WHEELBARROW_WEIGHT_REDUCTION_MULTIPLIER;
            Enum.TryParse(typeof(Restrictions), UpgradeBus.instance.cfg.WHEELBARROW_RESTRICTION_MODE, out object parsedRestriction);
            if (parsedRestriction == null)
            {
                logger.LogError($"An error occured parsing the restriction mode ({UpgradeBus.instance.cfg.WHEELBARROW_RESTRICTION_MODE}), defaulting to ItemCount");
                restriction = Restrictions.ItemCount;
            }
            else restriction = (Restrictions)parsedRestriction;
            maximumWeightAllowed = UpgradeBus.instance.cfg.WHEELBARROW_MAXIMUM_WEIGHT_ALLOWED;
            noiseRange = UpgradeBus.instance.cfg.WHEELBARROW_NOISE_RANGE;
            sloppiness = UpgradeBus.instance.cfg.WHEELBARROW_MOVEMENT_SLOPPY;
            lookSensitivityDrawback = UpgradeBus.instance.cfg.WHEELBARROW_LOOK_SENSITIVITY_DRAWBACK;
            playSounds = UpgradeBus.instance.cfg.WHEELBARROW_PLAY_NOISE;
        }

        public override void Update()
        {
            base.Update();
            if (!(isHeld && playerHeldBy.thisController.velocity.magnitude > 0f)) return;

            wheel.transform.Rotate(Time.deltaTime, 0f, 0f, Space.Self);
            wheel.transform.rotation.Set(wheel.transform.rotation.x % 360, wheel.transform.rotation.y, wheel.transform.rotation.z, wheel.transform.rotation.w);
        }
    }
}
