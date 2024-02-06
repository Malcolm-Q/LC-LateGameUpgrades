using GameNetcodeStuff;
using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc;
using MoreShipUpgrades.Misc.Upgrades;
using System;
using System.Collections.Generic;
using System.Text;

namespace MoreShipUpgrades.UpgradeComponents.TierUpgrades
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
    }
}
