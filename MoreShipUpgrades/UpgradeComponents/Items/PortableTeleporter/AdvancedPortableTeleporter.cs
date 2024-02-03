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
    internal class AdvancedPortableTeleporter : BasePortableTeleporter, IDisplayInfo, IItemWorldBuilding
    {
        internal static string WORLD_BUILDING_TEXT = "A newer, ostensibly-safer Teleportation Remote made by PortaCorp. PortaCorp was a subsidiary of Shipping Solutions Interplanetary," +
            " which was acquired by The Company in 2420, along with their entire stock of PortaCorp Teleportation Remotes. Looking to distance itself from the many safety scandals incurred" +
            " by earlier adopters of the 'Remote Teleportation' technology, PortaCorp marketed its line of Teleportation Remotes with claims of their comparative ruggedness and reliability.";
        public override void Start()
        {
            base.Start();
            breakChance = UpgradeBus.instance.cfg.ADV_CHANCE_TO_BREAK;
            keepItems = UpgradeBus.instance.cfg.ADV_KEEP_ITEMS_ON_TELE;
        }
        public string GetDisplayInfo()
        {
            return string.Format(AssetBundleHandler.GetInfoFromJSON("Advanced Portable Tele"), (int)(UpgradeBus.instance.cfg.ADV_CHANCE_TO_BREAK * 100));
        }

        public string GetWorldBuildingText()
        {
            return WORLD_BUILDING_TEXT;
        }
    }
}
