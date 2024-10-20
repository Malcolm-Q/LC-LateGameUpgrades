﻿using MoreShipUpgrades.Misc.Util;
using MoreShipUpgrades.UpgradeComponents.Items.Contracts.BombDefusal;

namespace MoreShipUpgrades.UpgradeComponents.Contracts
{
    internal class DefusalContract : ContractObject
    {
        public override void Start()
        {
            contractType = LguConstants.DEFUSAL_CONTRACT_NAME;
            if (GetComponent<PhysicsProp>().isInShipRoom || GetComponent<PhysicsProp>().isInElevator) // Already in the ship, don't make it disappear
            {
                BombDefusalScript bomb = GetComponent<BombDefusalScript>();
                bomb.MakeBombGrabbable();
            }
            else
            {
                base.Start();
            }
        }
    }
}
