using GameNetcodeStuff;
using MoreShipUpgrades.Managers;
using System.Collections;
using System.Linq;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MoreShipUpgrades.UpgradeComponents.PortableTeleporter
{
    internal class RegularPortableTeleporter : BasePortableTeleporter
    {
        public override void Start()
        {
            base.Start();
            breakChance = UpgradeBus.instance.cfg.CHANCE_TO_BREAK;
            keepItems = UpgradeBus.instance.cfg.KEEP_ITEMS_ON_TELE;
        }
    }
}
