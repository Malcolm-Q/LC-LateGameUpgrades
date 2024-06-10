using MoreShipUpgrades.Misc.Util;

namespace MoreShipUpgrades.UpgradeComponents.Contracts
{
    internal class ExterminatorContract : ContractObject
    {
        public override void Start()
        {
            contractType = LGUConstants.EXTERMINATOR_CONTRACT_NAME;
            base.Start();
        }
    }
}
