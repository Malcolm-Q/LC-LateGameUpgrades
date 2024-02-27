using MoreShipUpgrades.UpgradeComponents.Items.Contracts.BombDefusal;
using MoreShipUpgrades.UpgradeComponents.Items.Contracts.Extraction;
using System;
using System.Collections.Generic;
using System.Text;

namespace MoreShipUpgrades.UpgradeComponents.Contracts
{
    internal class ExtractionContract : ContractObject
    {
        public override void Start()
        {
            contractType = "extraction";
            if (GetComponent<PhysicsProp>().isInShipRoom || GetComponent<PhysicsProp>().isInElevator) // Already in the ship, don't make it disappear
            {
                ExtractPlayerScript scavenger = GetComponent<ExtractPlayerScript>();
                scavenger.MakeScavengerGrabbable();
            }
            else base.Start();
        }
    }
}
