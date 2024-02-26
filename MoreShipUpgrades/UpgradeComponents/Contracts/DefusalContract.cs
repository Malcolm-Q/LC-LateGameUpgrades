using MoreShipUpgrades.UpgradeComponents.Items.Contracts.BombDefusal;

namespace MoreShipUpgrades.UpgradeComponents.Contracts
{
    internal class DefusalContract : ContractObject
    {
        public override void Start()
        {
            contractType = "defusal";
            if (GetComponent<PhysicsProp>().isInShipRoom || GetComponent<PhysicsProp>().isInElevator) // Already in the ship, don't make it disappear
            {
                BombDefusalScript bomb = GetComponent<BombDefusalScript>();
                bomb.MakeBombGrabbable();
            }
            else base.Start();
        }
    }
}
