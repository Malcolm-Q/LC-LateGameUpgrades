using MoreShipUpgrades.Misc.Util;

namespace MoreShipUpgrades.UpgradeComponents.Contracts
{
    internal class ExorcismContract : ContractObject
    {
        public override void Start()
        {
            contractType = LguConstants.EXORCISM_CONTRACT_NAME;
            base.Start();
        }
    }
}
