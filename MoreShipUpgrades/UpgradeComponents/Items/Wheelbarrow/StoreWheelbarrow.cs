using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc;
using MoreShipUpgrades.UpgradeComponents.Interfaces;
using System;
using UnityEngine;

namespace MoreShipUpgrades.UpgradeComponents.Items.Wheelbarrow
{
    internal class StoreWheelbarrow : WheelbarrowScript, IDisplayInfo
    {
        static readonly LguLogger logger = new(nameof(StoreWheelbarrow));
        private GameObject wheel;
        internal const string ITEM_NAME = "Wheelbarrow";
        internal const string ITEM_DESCRIPTION = "Allows carrying multiple items";
        bool KeepScanNode
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
            if (wheel == null) logger.LogError($"Couldn't find the wheel's {nameof(GameObject)} to perform rotations");
            maximumAmountItems = UpgradeBus.Instance.PluginConfiguration.WHEELBARROW_MAXIMUM_AMOUNT_ITEMS.Value;
            weightReduceMultiplier = UpgradeBus.Instance.PluginConfiguration.WHEELBARROW_WEIGHT_REDUCTION_MULTIPLIER.Value;
            Enum.TryParse(typeof(Restrictions), UpgradeBus.Instance.PluginConfiguration.WHEELBARROW_RESTRICTION_MODE.Value, out object parsedRestriction);
            if (parsedRestriction == null)
            {
                logger.LogError($"An error occured parsing the restriction mode ({UpgradeBus.Instance.PluginConfiguration.WHEELBARROW_RESTRICTION_MODE}), defaulting to ItemCount");
                restriction = Restrictions.ItemCount;
            }
            else
            {
                restriction = (Restrictions)parsedRestriction;
            }
            maximumWeightAllowed = UpgradeBus.Instance.PluginConfiguration.WHEELBARROW_MAXIMUM_WEIGHT_ALLOWED.Value;
            noiseRange = UpgradeBus.Instance.PluginConfiguration.WHEELBARROW_NOISE_RANGE.Value;
            sloppiness = UpgradeBus.Instance.PluginConfiguration.WHEELBARROW_MOVEMENT_SLOPPY.Value;
            lookSensitivityDrawback = UpgradeBus.Instance.PluginConfiguration.WHEELBARROW_LOOK_SENSITIVITY_DRAWBACK.Value;
            playSounds = UpgradeBus.Instance.PluginConfiguration.WHEELBARROW_PLAY_NOISE.Value;
            if (!KeepScanNode) LguScanNodeProperties.RemoveScanNode(gameObject);
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
