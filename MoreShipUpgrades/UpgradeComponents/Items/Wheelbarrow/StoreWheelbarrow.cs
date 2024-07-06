using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc;
using MoreShipUpgrades.UpgradeComponents.Interfaces;
using System;
using UnityEngine;

namespace MoreShipUpgrades.UpgradeComponents.Items.Wheelbarrow
{
    internal class StoreWheelbarrow : WheelbarrowScript, IDisplayInfo
    {
        private GameObject wheel;
        internal const string ITEM_NAME = "Wheelbarrow";
        internal const string ITEM_DESCRIPTION = "Allows carrying multiple items";
        protected override bool KeepScanNode
        {
            get
            {
                return UpgradeBus.Instance.PluginConfiguration.STORE_WHEELBARROW_SCAN_NODE;
            }
        }

        public string GetDisplayInfo()
        {
            return $"A portable container which has a maximum capacity of {UpgradeBus.Instance.PluginConfiguration.WHEELBARROW_MAXIMUM_AMOUNT_ITEMS.Value}" +
                $" and reduces the effective weight of the inserted items by {UpgradeBus.Instance.PluginConfiguration.WHEELBARROW_WEIGHT_REDUCTION_MULTIPLIER.Value * 100} %.\n" +
                $"It weighs {UpgradeBus.Instance.PluginConfiguration.WHEELBARROW_WEIGHT.Value} lbs";
        }

        public override void Start()
        {
            base.Start();
            wheel = GameObject.Find("lgu_wheelbarrow_wheel");
            LategameConfiguration config = UpgradeBus.Instance.PluginConfiguration;
            maximumAmountItems = config.WHEELBARROW_MAXIMUM_AMOUNT_ITEMS.Value;
            weightReduceMultiplier = config.WHEELBARROW_WEIGHT_REDUCTION_MULTIPLIER.Value;
            restriction = config.WHEELBARROW_RESTRICTION_MODE;
            maximumWeightAllowed = config.WHEELBARROW_MAXIMUM_WEIGHT_ALLOWED.Value;
            noiseRange = config.WHEELBARROW_NOISE_RANGE.Value;
            sloppiness = config.WHEELBARROW_MOVEMENT_SLOPPY.Value;
            lookSensitivityDrawback = config.WHEELBARROW_LOOK_SENSITIVITY_DRAWBACK.Value;
            playSounds = config.WHEELBARROW_PLAY_NOISE.Value;
        }

        public override void Update()
        {
            base.Update();
            if (!(isHeld && playerHeldBy.thisController.velocity.magnitude > 0f)) return;

            wheel.transform.Rotate(Time.deltaTime, 0f, 0f, Space.Self);
            wheel.transform.rotation.Set(wheel.transform.rotation.x % 360, wheel.transform.rotation.y, wheel.transform.rotation.z, wheel.transform.rotation.w);
        }

        protected override void SetupScanNodeProperties()
        {
            ScanNodeProperties scanNodeProperties = GetComponentInChildren<ScanNodeProperties>();
            if (scanNodeProperties != null) LguScanNodeProperties.ChangeScanNode(ref scanNodeProperties, (LguScanNodeProperties.NodeType)scanNodeProperties.nodeType, header: ITEM_NAME, subText: ITEM_DESCRIPTION);
            else LguScanNodeProperties.AddGeneralScanNode(objectToAddScanNode: gameObject, header: ITEM_NAME, subText: ITEM_DESCRIPTION);
        }
    }
}
