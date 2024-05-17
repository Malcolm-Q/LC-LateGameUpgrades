using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;
using UnityEngine;

namespace MoreShipUpgrades.Misc.TerminalNodes
{
    internal class TierTerminalNode : CustomTerminalNode
    {
        public TierTerminalNode(string name, int unlockPrice, string description, GameObject prefab, int[] prices, int maxUpgrade, string originalName = "", bool sharedUpgrade = false) : base(name, unlockPrice, description, prefab, prices, maxUpgrade, originalName: originalName, sharedUpgrade: sharedUpgrade)
        {

        }
    }
}
