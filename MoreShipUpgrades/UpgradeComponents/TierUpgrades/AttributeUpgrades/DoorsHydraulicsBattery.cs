using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc;
using MoreShipUpgrades.Misc.Upgrades;
using MoreShipUpgrades.UpgradeComponents.Interfaces;
using System;

namespace MoreShipUpgrades.UpgradeComponents.TierUpgrades.AttributeUpgrades
{
    internal class DoorsHydraulicsBattery : GameAttributeTierUpgrade, IServerSync
    {
        public const string UPGRADE_NAME = "Shutter Batteries";
        public const string PRICES_DEFAULT = "200,300,400";

        public const string ENABLED_SECTION = $"Enable {UPGRADE_NAME}";
        public const string ENABLED_DESCRIPTION = "Increases the amount of time the doors can remain shut";

        public const string PRICE_SECTION = $"Price of {UPGRADE_NAME}";
        public const int PRICE_DEFAULT = 300;

        public const string INITIAL_SECTION = $"Initial battery boost";
        public const float INITIAL_DEFAULT = 5f;
        public const string INITIAL_DESCRIPTION = $"Initial battery boost for the doors' lock on first purchase";

        public const string INCREMENTAL_SECTION = $"Incremental battery boost";
        public const float INCREMENTAL_DEFAULT = 5f;
        public const string INCREMENTAL_DESCRIPTION = $"Incremental battery boost for the doors' lock after purchase";
        void Awake()
        {
            upgradeName = UPGRADE_NAME;
            logger = new LguLogger(upgradeName);
            changingAttribute = GameAttribute.SHIP_DOOR_BATTERY;
            initialValue = UpgradeBus.Instance.PluginConfiguration.DOOR_HYDRAULICS_BATTERY_INITIAL.Value;
            incrementalValue = UpgradeBus.Instance.PluginConfiguration.DOOR_HYDRAULICS_BATTERY_INCREMENTAL.Value;
        }
        public override string GetDisplayInfo(int initialPrice = -1, int maxLevels = -1, int[] incrementalPrices = null)
        {
            Func<int, float> infoFunction = level => UpgradeBus.Instance.PluginConfiguration.DOOR_HYDRAULICS_BATTERY_INITIAL.Value + level * UpgradeBus.Instance.PluginConfiguration.DOOR_HYDRAULICS_BATTERY_INCREMENTAL.Value;
            string infoFormat = "LVL {0} - ${1} - Increases the door's hydraulic capacity to remain closed by {2} units\n"; // to put in the infoStrings after
            return Tools.GenerateInfoForUpgrade(infoFormat, initialPrice, incrementalPrices, infoFunction);
        }
    }
}
