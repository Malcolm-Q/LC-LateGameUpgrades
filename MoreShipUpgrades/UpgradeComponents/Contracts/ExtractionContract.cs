using MoreShipUpgrades.UpgradeComponents.Items;
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
            contractType = "Extraction";
            if (GetComponent<Medkit>() == null && GetComponent<GrabbableObject>().isInShipRoom || GetComponent<GrabbableObject>().isInElevator) // Already in the ship, don't make it disappear
            {
                ExtractPlayerScript scavenger = GetComponent<ExtractPlayerScript>();
                scavenger.MakeScavengerGrabbable();
            }
            else base.Start();
        }
    }
}
