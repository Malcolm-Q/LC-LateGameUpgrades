using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace MoreShipUpgrades.Misc.TerminalNodes
{
    internal class OneTimeTerminalNode : CustomTerminalNode
    {
        public OneTimeTerminalNode(string name, int unlockPrice, string description, GameObject prefab, string originalName = "", bool sharedUpgrade = false) : base(name, unlockPrice, description, prefab, originalName: originalName, sharedUpgrade: sharedUpgrade)
        {

        }
    }
}
