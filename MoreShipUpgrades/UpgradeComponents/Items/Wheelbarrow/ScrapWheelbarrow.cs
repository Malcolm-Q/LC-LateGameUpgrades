using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc;
using System;

namespace MoreShipUpgrades.UpgradeComponents.Items.Wheelbarrow
{
    internal class ScrapWheelbarrow : WheelbarrowScript
    {
        internal const string ITEM_NAME = "Shopping Cart";

        protected override bool KeepScanNode
        {
            get
            {
                return true;
            }
        }

        public override void Start()
        {
            base.Start();
            LategameConfiguration config = UpgradeBus.Instance.PluginConfiguration;
            maximumAmountItems = config.SCRAP_WHEELBARROW_MAXIMUM_AMOUNT_ITEMS.Value;
            weightReduceMultiplier = config.SCRAP_WHEELBARROW_WEIGHT_REDUCTION_MULTIPLIER.Value;
            restriction = config.SCRAP_WHEELBARROW_RESTRICTION_MODE;
            maximumWeightAllowed = config.SCRAP_WHEELBARROW_MAXIMUM_WEIGHT_ALLOWED.Value;
            noiseRange = config.SCRAP_WHEELBARROW_NOISE_RANGE.Value;
            sloppiness = config.SCRAP_WHEELBARROW_MOVEMENT_SLOPPY.Value;
            lookSensitivityDrawback = config.SCRAP_WHEELBARROW_LOOK_SENSITIVITY_DRAWBACK.Value;
            playSounds = config.SCRAP_WHEELBARROW_PLAY_NOISE.Value;
            Random random = new Random(StartOfRound.Instance.randomMapSeed + 45);
            GetComponent<ScrapValueSyncer>().SetScrapValue(random.Next(config.SCRAP_WHEELBARROW_MINIMUM_VALUE.Value, config.SCRAP_WHEELBARROW_MAXIMUM_VALUE.Value));
        }
        protected override void SetupScanNodeProperties()
        {
            ScanNodeProperties scanNodeProperties = GetComponentInChildren<ScanNodeProperties>();
            if (scanNodeProperties != null) LguScanNodeProperties.ChangeScanNode(ref scanNodeProperties, (LguScanNodeProperties.NodeType)scanNodeProperties.nodeType, header: ITEM_NAME);
            else LguScanNodeProperties.AddScrapScanNode(objectToAddScanNode: gameObject, header: ITEM_NAME);
        }
    }
}
