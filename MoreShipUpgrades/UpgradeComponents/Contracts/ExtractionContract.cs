using MoreShipUpgrades.Misc.Util;
using MoreShipUpgrades.UpgradeComponents.Items;
using MoreShipUpgrades.UpgradeComponents.Items.Contracts.Extraction;

namespace MoreShipUpgrades.UpgradeComponents.Contracts
{
    internal class ExtractionContract : ContractObject
    {
        public override void Start()
        {
            contractType = LguConstants.EXTRACTION_CONTRACT_NAME;
            if (GetComponent<Medkit>() == null && (GetComponent<GrabbableObject>().isInShipRoom || GetComponent<GrabbableObject>().isInElevator)) // Already in the ship, don't make it disappear
            {
                ExtractPlayerScript scavenger = GetComponent<ExtractPlayerScript>();
                scavenger.MakeScavengerGrabbable();
            }
            else
            {
                base.Start();
            }
        }
    }
}
