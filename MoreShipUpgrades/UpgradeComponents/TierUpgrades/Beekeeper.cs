using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc;
using MoreShipUpgrades.Misc.Upgrades;
using MoreShipUpgrades.UpgradeComponents.Interfaces;
using Unity.Netcode;
using UnityEngine;

namespace MoreShipUpgrades.UpgradeComponents.TierUpgrades
{
    class Beekeeper : TierUpgrade, IUpgradeWorldBuilding
    {
        internal bool increaseHivePrice = false;
        internal static Beekeeper Instance;

        public const string UPGRADE_NAME = "Beekeeper";
        public const string PRICES_DEFAULT = "225,280,340";
        internal const string WORLD_BUILDING_TEXT = "\n\nOn-the-job training package that teaches {0} proper Circuit Bee Nest handling techniques." +
            " Also comes with a weekly issuance of alkaline pills to partially inoculate {0} against Circuit Bee Venom.\n\n";
        void Awake()
        {
            upgradeName = UPGRADE_NAME;
            Instance = this;
        }

        public override void Increment()
        {
            base.Increment();
            if (GetUpgradeLevel(UPGRADE_NAME) == UpgradeBus.Instance.PluginConfiguration.BEEKEEPER_UPGRADE_PRICES.Value.Split(',').Length)
                ToggleIncreaseHivePriceServerRpc();
        }

        public static int CalculateBeeDamage(int damageNumber)
        {
            if (!GetActiveUpgrade(UPGRADE_NAME)) return damageNumber;
            return Mathf.Clamp((int)(damageNumber * (UpgradeBus.Instance.PluginConfiguration.BEEKEEPER_DAMAGE_MULTIPLIER.Value - GetUpgradeLevel(UPGRADE_NAME) * UpgradeBus.Instance.PluginConfiguration.BEEKEEPER_DAMAGE_MULTIPLIER_INCREMENT.Value)), 0, damageNumber);
        }

        public static int GetHiveScrapValue(int originalValue)
        {
            if (!Instance.increaseHivePrice) return originalValue;
            return (int)(originalValue * UpgradeBus.Instance.PluginConfiguration.BEEKEEPER_HIVE_VALUE_INCREASE.Value);
        }

        public string GetWorldBuildingText(bool shareStatus = false)
        {
            return string.Format(WORLD_BUILDING_TEXT, shareStatus ? "your crew" : "you");
        }

        public override string GetDisplayInfo(int initialPrice = -1, int maxLevels = -1, int[] incrementalPrices = null)
        {
            System.Func<int, float> infoFunction = level => 100 * (UpgradeBus.Instance.PluginConfiguration.BEEKEEPER_DAMAGE_MULTIPLIER.Value - (level * UpgradeBus.Instance.PluginConfiguration.BEEKEEPER_DAMAGE_MULTIPLIER_INCREMENT.Value));
            string infoFormat = AssetBundleHandler.GetInfoFromJSON(UPGRADE_NAME);
            return Tools.GenerateInfoForUpgrade(infoFormat, initialPrice, incrementalPrices, infoFunction);
        }
        [ServerRpc(RequireOwnership = false)]
        public void ToggleIncreaseHivePriceServerRpc()
        {
            ToggleIncreaseHivePriceClientRpc();
        }

        [ClientRpc]
        public void ToggleIncreaseHivePriceClientRpc()
        {
            increaseHivePrice = true;
        }
    }
}
