using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc;
using MoreShipUpgrades.Misc.Upgrades;
using MoreShipUpgrades.UI.TerminalNodes;
using MoreShipUpgrades.UpgradeComponents.Interfaces;
using UnityEngine;

namespace MoreShipUpgrades.UpgradeComponents.OneTimeUpgrades.Enemies
{
    internal class SilverBullets : OneTimeUpgrade, IUpgradeWorldBuilding
    {
        public const string UPGRADE_NAME = "Silver Bullets";
        internal const string WORLD_BUILDING_TEXT = "\n\nSome things are better left unexplained.\n\n";
        public string GetWorldBuildingText(bool shareStatus = false)
        {
            return WORLD_BUILDING_TEXT;
        }
        public override bool CanInitializeOnStart => GetConfiguration().SilverBulletsConfiguration.Price <= 0;
        void Awake()
        {
            upgradeName = UPGRADE_NAME;
            overridenUpgradeName = GetConfiguration().SilverBulletsConfiguration.OverrideName;
        }
        public override string GetDisplayInfo(int price = -1)
        {
            return $"{GetUpgradePrice(price, GetConfiguration().SilverBulletsConfiguration.PurchaseMode)} - The pellets in the shotgun are replaced with silver, allowing you to cleanse some paranormal activity if necessary.";
        }
        public new static (string, string[]) RegisterScrapToUpgrade()
        {
            return (UPGRADE_NAME, GetConfiguration().SilverBulletsConfiguration.OverrideName.Value.Split(","));
        }
        public new static void RegisterUpgrade()
        {
            GameObject prefab = LethalLib.Modules.NetworkPrefabs.CreateNetworkPrefab(UPGRADE_NAME);
            prefab.AddComponent<SilverBullets>();
            LethalLib.Modules.NetworkPrefabs.RegisterNetworkPrefab(prefab);
            Plugin.networkPrefabs[UPGRADE_NAME] = prefab;
        }
        public new static CustomTerminalNode RegisterTerminalNode()
        {
            return UpgradeBus.Instance.SetupOneTimeTerminalNode(UPGRADE_NAME, GetConfiguration().SilverBulletsConfiguration, Plugin.networkPrefabs[UPGRADE_NAME]);
        }
    }
}
