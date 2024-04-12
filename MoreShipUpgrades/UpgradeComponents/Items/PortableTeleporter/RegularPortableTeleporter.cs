﻿using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc;
using MoreShipUpgrades.UpgradeComponents.Interfaces;

namespace MoreShipUpgrades.UpgradeComponents.Items.PortableTeleporter
{
    internal class RegularPortableTeleporter : BasePortableTeleporter, IDisplayInfo, IItemWorldBuilding
    {
        internal const string ITEM_NAME = "Portable Teleporter";

        internal static string WORLD_BUILDING_TEXT = "Early hand-held teleportation device based on an invention incepted in 2391 with public money," +
            " which was then sold to a private company for manufacturing. Innovations present in the device allow the user to return to a designated 'home location'" +
            " with any objects they possess once used. In 2406, a massive stock of liquidated TeleMax Teleportation Remotes was acquired by The Company in aftermarket" +
            " following a battery-related recall. As a result, the Company is able to offer them to its employees for cheap, but they are prone to swelling and combustion. Handle with care.";

        public string GetDisplayInfo()
        {
            return string.Format(AssetBundleHandler.GetInfoFromJSON("Portable Tele"), (int)(UpgradeBus.Instance.PluginConfiguration.CHANCE_TO_BREAK.Value * 100));
        }

        public string GetWorldBuildingText()
        {
            return WORLD_BUILDING_TEXT;
        }

        public override void Start()
        {
            base.Start();
            breakChance = UpgradeBus.Instance.PluginConfiguration.CHANCE_TO_BREAK.Value;
            keepItems = UpgradeBus.Instance.PluginConfiguration.KEEP_ITEMS_ON_TELE.Value;
        }
    }
}
