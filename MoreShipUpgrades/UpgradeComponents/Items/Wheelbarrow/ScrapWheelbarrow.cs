using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc;
using System;

namespace MoreShipUpgrades.UpgradeComponents.Items.Wheelbarrow
{
    internal class ScrapWheelbarrow : WheelbarrowScript
    {
        static LguLogger logger = new LguLogger(nameof(ScrapWheelbarrow));
        const string ITEM_NAME = "Shopping Cart";
        public override void Start()
        {
            base.Start();
            maximumAmountItems = UpgradeBus.Instance.PluginConfiguration.SCRAP_WHEELBARROW_MAXIMUM_AMOUNT_ITEMS.Value;
            weightReduceMultiplier = UpgradeBus.Instance.PluginConfiguration.SCRAP_WHEELBARROW_WEIGHT_REDUCTION_MULTIPLIER.Value;
            Enum.TryParse(typeof(Restrictions), UpgradeBus.Instance.PluginConfiguration.SCRAP_WHEELBARROW_RESTRICTION_MODE.Value, out object parsedRestriction);
            if (parsedRestriction == null)
            {
                logger.LogError($"An error occured parsing the restriction mode ({UpgradeBus.Instance.PluginConfiguration.SCRAP_WHEELBARROW_RESTRICTION_MODE}), defaulting to ItemCount");
                restriction = Restrictions.ItemCount;
            }
            else restriction = (Restrictions)parsedRestriction;
            maximumWeightAllowed = UpgradeBus.Instance.PluginConfiguration.SCRAP_WHEELBARROW_MAXIMUM_WEIGHT_ALLOWED.Value;
            noiseRange = UpgradeBus.Instance.PluginConfiguration.SCRAP_WHEELBARROW_NOISE_RANGE.Value;
            sloppiness = UpgradeBus.Instance.PluginConfiguration.SCRAP_WHEELBARROW_MOVEMENT_SLOPPY.Value;
            lookSensitivityDrawback = UpgradeBus.Instance.PluginConfiguration.SCRAP_WHEELBARROW_LOOK_SENSITIVITY_DRAWBACK.Value;
            playSounds = UpgradeBus.Instance.PluginConfiguration.SCRAP_WHEELBARROW_PLAY_NOISE.Value;
            Random random = new Random(StartOfRound.Instance.randomMapSeed + 45);
            GetComponent<ScrapValueSyncer>().SetScrapValue(random.Next(UpgradeBus.Instance.PluginConfiguration.SCRAP_WHEELBARROW_MINIMUM_VALUE.Value, UpgradeBus.Instance.PluginConfiguration.SCRAP_WHEELBARROW_MAXIMUM_VALUE.Value));
            logger.LogDebug("Spawned in the scene!");
        }
        protected override void SetupScanNodeProperties()
        {
            ScanNodeProperties scanNodeProperties = GetComponentInChildren<ScanNodeProperties>();
            if (scanNodeProperties != null) LguScanNodeProperties.ChangeScanNode(ref scanNodeProperties, (LguScanNodeProperties.NodeType)scanNodeProperties.nodeType, header: ITEM_NAME);
            else LguScanNodeProperties.AddScrapScanNode(objectToAddScanNode: gameObject, header: ITEM_NAME);
        }
    }
}
