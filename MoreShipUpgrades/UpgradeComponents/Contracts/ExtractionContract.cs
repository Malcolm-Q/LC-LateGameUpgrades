using System;
using System.Collections.Generic;
using System.Text;

namespace MoreShipUpgrades.UpgradeComponents.Contracts
{
    internal class ExtractionContract : ContractObject
    {
        public override void Start()
        {
            contractType = "extraction";
            base.Start();
        }
    }
}
