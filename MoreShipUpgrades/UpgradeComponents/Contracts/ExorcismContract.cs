using System;
using System.Collections.Generic;
using System.Text;

namespace MoreShipUpgrades.UpgradeComponents.Contracts
{
    internal class ExorcismContract : ContractObject
    {
        public override void Start()
        {
            contractType = "Exorcism";
            base.Start();
        }
    }
}
