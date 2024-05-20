using System;
using System.Collections.Generic;
using System.Text;

namespace MoreShipUpgrades.UpgradeComponents.Contracts
{
    internal class ExterminatorContract : ContractObject
    {
        public override void Start()
        {
            contractType = "Exterminator";
            base.Start();
        }
    }
}
