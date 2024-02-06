using GameNetcodeStuff;
using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc;
using MoreShipUpgrades.Misc.Upgrades;
using MoreShipUpgrades.UpgradeComponents.Interfaces;
using System;
using UnityEngine;

namespace MoreShipUpgrades.UpgradeComponents.TierUpgrades.AttributeUpgrades
{
    internal class RunningShoes : GameAttributeTierUpgrade, IUpgradeWorldBuilding
    {
        public const string UPGRADE_NAME = "Running Shoes";
        public static string PRICES_DEFAULT = "500,750,1000";
        internal const string WORLD_BUILDING_TEXT = "\n\nA new pair of boots {0} a whole new lease on life. In this instance," +
            " it might also result in fewer wet sock incidents and consequent trenchfoot. After all, who knows how many people have walked in {1} shoes?\n\n";
        internal override void Start()
        {
            upgradeName = UPGRADE_NAME;
            logger = new LGULogger(UPGRADE_NAME);
            base.Start();
            changingAttribute = GameAttribute.PLAYER_MOVEMENT_SPEED;
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

        public string GetWorldBuildingText(bool shareStatus = false)
        {
            return string.Format(WORLD_BUILDING_TEXT, shareStatus ? "could give your crew" : "can give you", shareStatus ? "y'all's" : "your");
        }

        public override string GetDisplayInfo(int initialPrice = -1, int maxLevels = -1, int[] incrementalPrices = null)
        {
            Func<int, float> infoFunction = level => UpgradeBus.instance.cfg.MOVEMENT_SPEED_UNLOCK + level * UpgradeBus.instance.cfg.MOVEMENT_INCREMENT;
            string infoFormat = AssetBundleHandler.GetInfoFromJSON(UPGRADE_NAME);
            return Tools.GenerateInfoForUpgrade(infoFormat, initialPrice, incrementalPrices, infoFunction);
        }
    }
}
