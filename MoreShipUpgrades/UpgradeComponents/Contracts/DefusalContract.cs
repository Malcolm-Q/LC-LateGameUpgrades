using System;
using System.Collections.Generic;
using System.Text;

namespace MoreShipUpgrades.UpgradeComponents.Contracts
{
    internal class DefusalContract : ContractObject
    {
        public override void Start()
        {
            contractType = "defusal";
            base.Start();
        }
    }
}
