using System;
using System.Collections.Generic;
using System.Text;

namespace MoreShipUpgrades.UpgradeComponents.Contracts
{
    internal class DataRetrievalContract : ContractObject
    {
        public override void Start()
        {
            contractType = "Data";
            base.Start();
        }
    }
}
