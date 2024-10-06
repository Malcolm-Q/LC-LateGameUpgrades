using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc;
using MoreShipUpgrades.Misc.Upgrades;
using MoreShipUpgrades.UI.TerminalNodes;
using UnityEngine;

namespace MoreShipUpgrades.UpgradeComponents.OneTimeUpgrades.Enemies
{
    internal class SilverBullets : OneTimeUpgrade
    {
        public const string UPGRADE_NAME = "Silver Bullets";
        public override bool CanInitializeOnStart => GetConfiguration().SILVER_BULLETS_PRICE.Value <= 0;
        void Awake()
        {
            upgradeName = UPGRADE_NAME;
            overridenUpgradeName = GetConfiguration().SILVER_BULLETS_OVERRIDE_NAME;
        }
        public override string GetDisplayInfo(int price = -1)
        {
            return $"${price} - The pellets in the shotgun are replaced with silver, allowing you to cleanse some paranormal activity if necessary.";
        }
        public new static (string, string[]) RegisterScrapToUpgrade()
        {
            return (UPGRADE_NAME, GetConfiguration().SILVER_BULLETS_ITEM_PROGRESSION_ITEMS.Value.Split(","));
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
            LategameConfiguration configuration = GetConfiguration();

            return UpgradeBus.Instance.SetupOneTimeTerminalNode(UPGRADE_NAME,
                                    shareStatus: configuration.SHARED_UPGRADES || !configuration.SILVER_BULLETS_INDIVIDUAL,
                                    configuration.SILVER_BULLETS_ENABLED.Value,
                                    configuration.SILVER_BULLETS_PRICE.Value,
                                    configuration.OVERRIDE_UPGRADE_NAMES ? configuration.SILVER_BULLETS_OVERRIDE_NAME : "",
                                    Plugin.networkPrefabs[UPGRADE_NAME]);
        }
    }
}
