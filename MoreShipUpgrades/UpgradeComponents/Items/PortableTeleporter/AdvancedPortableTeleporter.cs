using GameNetcodeStuff;
using MoreShipUpgrades.Managers;
using System.Collections;
using System.Linq;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MoreShipUpgrades.UpgradeComponents.Items.PortableTeleporter
{
    internal class AdvancedPortableTeleporter : BasePortableTeleporter
    {
        public override void Start()
        {
            base.Start();
            breakChance = UpgradeBus.instance.cfg.ADV_CHANCE_TO_BREAK;
            keepItems = UpgradeBus.instance.cfg.ADV_KEEP_ITEMS_ON_TELE;
        }
    }
}
