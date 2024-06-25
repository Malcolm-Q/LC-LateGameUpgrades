using MoreShipUpgrades.Misc.Util;

namespace MoreShipUpgrades.UpgradeComponents.Contracts
{
    internal class ExorcismContract : ContractObject
    {
        public override void Start()
        {
            contractType = LGUConstants.EXORCISM_CONTRACT_NAME;
            base.Start();
        }
    }
}
