using GameNetcodeStuff;
using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc;
using System;
using System.Collections.Generic;
using System.Text;

namespace MoreShipUpgrades.UpgradeComponents.TierUpgrades
{
    internal class DoorsHydraulicsBattery : BaseUpgrade
    {
        private static LGULogger logger;
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


        private static bool active;
        private int currentLevel;

        void Start()
        {
            upgradeName = UPGRADE_NAME;
            logger = new LGULogger(upgradeName);
            DontDestroyOnLoad(gameObject);
            Register();
        }
        public override void load()
        {
            HangarShipDoor shipDoors = UpgradeBus.instance.GetShipDoors();
            if (!active)
            {
                logger.LogDebug($"Adding initial {UpgradeBus.instance.cfg.DOOR_HYDRAULICS_BATTERY_INITIAL} to the door's power duration...");
                shipDoors.doorPowerDuration += UpgradeBus.instance.cfg.DOOR_HYDRAULICS_BATTERY_INITIAL;
            }
            UpgradeBus.instance.doorsHydraulicsBattery = true;
            active = true;
            base.load();

            float amountToIncrement = 0;
            for (int i = 1; i < UpgradeBus.instance.doorsHydraulicsBatteryLevel + 1; i++)
            {
                if (i <= currentLevel) continue;
                logger.LogDebug($"Adding incremental {UpgradeBus.instance.cfg.DOOR_HYDRAULICS_BATTERY_INCREMENTAL} to the door's power duration...");
                amountToIncrement += UpgradeBus.instance.cfg.DOOR_HYDRAULICS_BATTERY_INCREMENTAL;
            }
            logger.LogDebug(amountToIncrement);
            logger.LogDebug(shipDoors.doorPowerDuration);
            shipDoors.doorPowerDuration += amountToIncrement;
            currentLevel = UpgradeBus.instance.doorsHydraulicsBatteryLevel;
        }


        public override void Increment()
        {
            HangarShipDoor shipDoors = UpgradeBus.instance.GetShipDoors();
            shipDoors.doorPowerDuration += UpgradeBus.instance.cfg.DOOR_HYDRAULICS_BATTERY_INCREMENTAL;
            logger.LogDebug($"Adding incremental {UpgradeBus.instance.cfg.DOOR_HYDRAULICS_BATTERY_INCREMENTAL} to the ship door's battery...");
            UpgradeBus.instance.doorsHydraulicsBatteryLevel++;
            currentLevel++;
        }

        public override void Unwind()
        {
            HangarShipDoor shipDoors = UpgradeBus.instance.GetShipDoors();
            if (active) ResetDoorsHydraulicsBattery(ref shipDoors);
            base.Unwind();

            UpgradeBus.instance.doorsHydraulicsBattery = false;
            UpgradeBus.instance.doorsHydraulicsBatteryLevel = 0;
            active = false;
            currentLevel = 0;
        }
        public static void ResetDoorsHydraulicsBattery(ref HangarShipDoor shipDoors)
        {
            float doorsBatteryRemoval = UpgradeBus.instance.cfg.DOOR_HYDRAULICS_BATTERY_INITIAL;
            for (int i = 0; i < UpgradeBus.instance.doorsHydraulicsBatteryLevel; i++)
            {
                doorsBatteryRemoval += UpgradeBus.instance.cfg.DOOR_HYDRAULICS_BATTERY_INCREMENTAL;
            }
            logger.LogDebug($"Removing ship door's battery boost ({shipDoors.doorPowerDuration}) with a boost of {doorsBatteryRemoval}");
            shipDoors.doorPowerDuration -= doorsBatteryRemoval;
            logger.LogDebug($"Upgrade reset on ship doors");
            active = false;
        }

    }
}
