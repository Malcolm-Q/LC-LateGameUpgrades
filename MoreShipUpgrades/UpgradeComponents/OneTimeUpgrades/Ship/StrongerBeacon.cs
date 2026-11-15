using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc.Upgrades;
using MoreShipUpgrades.UI.TerminalNodes;
using MoreShipUpgrades.UpgradeComponents.OneTimeUpgrades.Enemies;
using UnityEngine;

namespace MoreShipUpgrades.UpgradeComponents.OneTimeUpgrades.Ship
{
    internal class StrongerBeacon : OneTimeUpgrade
    {
        internal const string UPGRADE_NAME = "Stronger Beacon";
        internal const int PRICE_DEFAULT = 200;

        void Awake()
        {
            upgradeName = UPGRADE_NAME;
            overridenUpgradeName = GetConfiguration().StrongerBeaconConfiguration.OverrideName;
        }

        public override bool CanInitializeOnStart => GetConfiguration().StrongerBeaconConfiguration.Price.Value <= 0;

        public static bool IsBeaconStronger()
        {
            return GetConfiguration().StrongerBeaconConfiguration.Enabled && GetActiveUpgrade(UPGRADE_NAME);
        }

        public override string GetDisplayInfo(int price = -1)
        {
            return $"{GetUpgradePrice(price, GetConfiguration().StrongerBeaconConfiguration.PurchaseMode)} - Terminal radar does not lose signal of players while they are inside of caves.";
        }
        public new static (string, string[]) RegisterScrapToUpgrade()
        {
            return (UPGRADE_NAME, GetConfiguration().StrongerBeaconConfiguration.ItemProgressionItems.Value.Split(","));
        }
        public new static void RegisterUpgrade()
        {
            GameObject prefab = ItemManager.CreateNetworkPrefab(UPGRADE_NAME);
            prefab.AddComponent<StrongerBeacon>();
            ItemManager.RegisterNetworkPrefab(prefab);
            Plugin.networkPrefabs[UPGRADE_NAME] = prefab;
        }
        public new static CustomTerminalNode RegisterTerminalNode()
        {
            return UpgradeBus.Instance.SetupOneTimeTerminalNode(UPGRADE_NAME, GetConfiguration().StrongerBeaconConfiguration, prefab: Plugin.networkPrefabs[UPGRADE_NAME]);
        }
    }
}
