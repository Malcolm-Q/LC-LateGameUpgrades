using System;
using System.Collections.Generic;
using System.Text;

namespace MoreShipUpgrades.UpgradeComponents.Interfaces
{
    internal interface IUpgradeWorldBuilding
    {
        string GetWorldBuildingText(bool shareStatus = false);
    }
}
