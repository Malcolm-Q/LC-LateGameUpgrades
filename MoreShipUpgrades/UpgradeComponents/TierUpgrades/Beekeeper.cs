using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc;
using MoreShipUpgrades.Misc.Upgrades;
using MoreShipUpgrades.UpgradeComponents.Interfaces;
using UnityEngine;

namespace MoreShipUpgrades.UpgradeComponents.TierUpgrades
{
    class Beekeeper : TierUpgrade, IUpgradeWorldBuilding
    {
        public static string UPGRADE_NAME = "Beekeeper";
        public static string PRICES_DEFAULT = "225,280,340";
        internal static string WORLD_BUILDING_TEXT = "\n\nOn-the-job training package that teaches {0} proper Circuit Bee Nest handling techniques." +
            " Also comes with a weekly issuance of alkaline pills to partially inoculate {0} against Circuit Bee Venom.\n\n";

        internal override void Start()
        {
            upgradeName = UPGRADE_NAME;
            base.Start();
        }

        public override void Increment()
        {
            base.Increment();
            if (GetUpgradeLevel(UPGRADE_NAME) == UpgradeBus.instance.cfg.BEEKEEPER_UPGRADE_PRICES.Value.Split(',').Length)
                LGUStore.instance.ToggleIncreaseHivePriceServerRpc();
        }

        public static int CalculateBeeDamage(int damageNumber)
        {
            if (!GetActiveUpgrade(UPGRADE_NAME)) return damageNumber;
            return Mathf.Clamp((int)(damageNumber * (UpgradeBus.instance.cfg.BEEKEEPER_DAMAGE_MULTIPLIER.Value - GetUpgradeLevel(UPGRADE_NAME) * UpgradeBus.instance.cfg.BEEKEEPER_DAMAGE_MULTIPLIER_INCREMENT.Value)), 0, damageNumber);
        }

        public static int GetHiveScrapValue(int originalValue)
        {
            if (!UpgradeBus.instance.increaseHivePrice) return originalValue;
            return (int)(originalValue * UpgradeBus.instance.cfg.BEEKEEPER_HIVE_VALUE_INCREASE.Value);
        }

        public string GetWorldBuildingText(bool shareStatus = false)
        {
            return string.Format(WORLD_BUILDING_TEXT, shareStatus ? "your crew" : "you");
        }

        public override string GetDisplayInfo(int initialPrice = -1, int maxLevels = -1, int[] incrementalPrices = null)
        {
            System.Func<int, float> infoFunction = level => 100 * (UpgradeBus.instance.cfg.BEEKEEPER_DAMAGE_MULTIPLIER.Value - (level * UpgradeBus.instance.cfg.BEEKEEPER_DAMAGE_MULTIPLIER_INCREMENT.Value));
            string infoFormat = AssetBundleHandler.GetInfoFromJSON(UPGRADE_NAME);
            return Tools.GenerateInfoForUpgrade(infoFormat, initialPrice, incrementalPrices, infoFunction);
        }
    }
}
