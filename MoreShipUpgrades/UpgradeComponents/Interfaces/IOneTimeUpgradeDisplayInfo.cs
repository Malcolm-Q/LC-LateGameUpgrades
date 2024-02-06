using System;
using System.Collections.Generic;
using System.Text;

namespace MoreShipUpgrades.UpgradeComponents.Interfaces
{
    internal interface IOneTimeUpgradeDisplayInfo
    {
        string GetDisplayInfo(int price = -1);
    }
}
