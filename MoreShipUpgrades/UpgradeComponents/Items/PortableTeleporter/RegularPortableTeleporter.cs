using GameNetcodeStuff;
using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc;
using MoreShipUpgrades.UpgradeComponents.Interfaces;
using System.Collections;
using System.Linq;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MoreShipUpgrades.UpgradeComponents.Items.PortableTeleporter
{
    internal class RegularPortableTeleporter : BasePortableTeleporter, IDisplayInfo, IItemWorldBuilding
    {
        internal static string WORLD_BUILDING_TEXT = "Early hand-held teleportation device based on an invention incepted in 2391 with public money," +
            " which was then sold to a private company for manufacturing. Innovations present in the device allow the user to return to a designated 'home location'" +
            " with any objects they possess once used. In 2406, a massive stock of liquidated TeleMax Teleportation Remotes was acquired by The Company in aftermarket" +
            " following a battery-related recall. As a result, the Company is able to offer them to its employees for cheap, but they are prone to swelling and combustion. Handle with care.";

        public string GetDisplayInfo()
        {
            return string.Format(AssetBundleHandler.GetInfoFromJSON("Portable Tele"), (int)(UpgradeBus.instance.cfg.CHANCE_TO_BREAK.Value * 100));
        }

        public string GetWorldBuildingText()
        {
            return WORLD_BUILDING_TEXT;
        }

        public override void Start()
        {
            base.Start();
            breakChance = UpgradeBus.instance.cfg.CHANCE_TO_BREAK.Value;
            keepItems = UpgradeBus.instance.cfg.KEEP_ITEMS_ON_TELE.Value;
        }
    }
}
