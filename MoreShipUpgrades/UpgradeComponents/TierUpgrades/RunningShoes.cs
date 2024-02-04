using GameNetcodeStuff;
using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc;
using MoreShipUpgrades.Misc.Upgrades;
using UnityEngine;

namespace MoreShipUpgrades.UpgradeComponents.TierUpgrades
{
    internal class RunningShoes : PlayerAttributeTierUpgrade
    {
        public const string UPGRADE_NAME = "Running Shoes";
        public static string PRICES_DEFAULT = "500,750,1000";
        internal override void Start()
        {
            upgradeName = UPGRADE_NAME;
            logger = new LGULogger(UPGRADE_NAME);
            base.Start();
            changingAttribute = PlayerAttribute.MOVEMENT_SPEED;
            initialValue = UpgradeBus.instance.cfg.MOVEMENT_SPEED_UNLOCK;
            incrementalValue = UpgradeBus.instance.cfg.MOVEMENT_INCREMENT;
        }

        public override void Increment()
        {
            base.Increment();
            UpgradeBus.instance.runningLevel++;
        }

        public override void Unwind()
        {
            UnloadUpgradeAttribute(ref UpgradeBus.instance.runningShoes, ref UpgradeBus.instance.runningLevel);
            base.Unwind();
        }

        public override void Load()
        {
            LoadUpgradeAttribute(ref UpgradeBus.instance.runningShoes, UpgradeBus.instance.runningLevel);
            base.Load();
        }
        public static float ApplyPossibleReducedNoiseRange(float defaultValue)
        {
            if (!(UpgradeBus.instance.runningShoes && UpgradeBus.instance.runningLevel == UpgradeBus.instance.cfg.RUNNING_SHOES_UPGRADE_PRICES.Split(',').Length)) return defaultValue;
            return Mathf.Clamp(defaultValue - UpgradeBus.instance.cfg.NOISE_REDUCTION, 0f, defaultValue);
        }
    }
}
