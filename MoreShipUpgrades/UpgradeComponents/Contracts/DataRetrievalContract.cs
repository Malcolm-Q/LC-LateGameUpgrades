using MoreShipUpgrades.Misc.Util;

namespace MoreShipUpgrades.UpgradeComponents.Contracts
{
    internal class DataRetrievalContract : ContractObject
    {
        public override void Start()
        {
            contractType = LguConstants.DATA_CONTRACT_NAME;
            base.Start();
        }
    }
}
