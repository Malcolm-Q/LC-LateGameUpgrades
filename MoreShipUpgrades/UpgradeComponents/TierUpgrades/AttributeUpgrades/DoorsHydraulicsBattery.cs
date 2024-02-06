using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc;
using MoreShipUpgrades.Misc.Upgrades;
using MoreShipUpgrades.UpgradeComponents.Interfaces;
using System;

namespace MoreShipUpgrades.UpgradeComponents.TierUpgrades.AttributeUpgrades
{
    internal class DoorsHydraulicsBattery : GameAttributeTierUpgrade
    {
        public const string UPGRADE_NAME = "Shutter Batteries";
        public const string PRICES_DEFAULT = "200,300,400";

        public static string ENABLED_SECTION = $"Enable {UPGRADE_NAME}";
        public static string ENABLED_DESCRIPTION = "Increases the amount of time the doors can remain shut";

        public static string PRICE_SECTION = $"Price of {UPGRADE_NAME}";
        public static int PRICE_DEFAULT = 300;

        public static string INITIAL_SECTION = $"Initial battery boost";
        public static float INITIAL_DEFAULT = 5f;
        public static string INITIAL_DESCRIPTION = $"Initial battery boost for the doors' lock on first purchase";

        public static string INCREMENTAL_SECTION = $"Incremental battery boost";
        public static float INCREMENTAL_DEFAULT = 5f;
        public static string INCREMENTAL_DESCRIPTION = $"Incremental battery boost for the doors' lock after purchase";

        internal override void Start()
        {
            upgradeName = UPGRADE_NAME;
            logger = new LGULogger(upgradeName);
            base.Start();
            changingAttribute = GameAttribute.SHIP_DOOR_BATTERY;
            initialValue = UpgradeBus.instance.cfg.DOOR_HYDRAULICS_BATTERY_INITIAL;
            incrementalValue = UpgradeBus.instance.cfg.DOOR_HYDRAULICS_BATTERY_INCREMENTAL;
        }
        public override void Load()
        {
            LoadUpgradeAttribute(ref UpgradeBus.instance.doorsHydraulicsBattery, UpgradeBus.instance.doorsHydraulicsBatteryLevel);
            base.Load();
        }


        public override void Increment()
        {
            base.Increment();
            UpgradeBus.instance.doorsHydraulicsBatteryLevel++;
        }

        public override void Unwind()
        {
            UnloadUpgradeAttribute(ref UpgradeBus.instance.doorsHydraulicsBattery, ref UpgradeBus.instance.doorsHydraulicsBatteryLevel);
            base.Unwind();
        }
        public override string GetDisplayInfo(int initialPrice = -1, int maxLevels = -1, int[] incrementalPrices = null)
        {
            Func<int, float> infoFunction = level => UpgradeBus.instance.cfg.DOOR_HYDRAULICS_BATTERY_INITIAL + level * UpgradeBus.instance.cfg.DOOR_HYDRAULICS_BATTERY_INCREMENTAL;
            string infoFormat = "LVL {0} - ${1} - Increases the door's hydraulic capacity to remain closed by {2} units\n"; // to put in the infoStrings after
            return Tools.GenerateInfoForUpgrade(infoFormat, initialPrice, incrementalPrices, infoFunction);
        }
    }
}
