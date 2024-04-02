using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace MoreShipUpgrades.Misc.TerminalNodes
{
    internal class OneTimeTerminalNode : CustomTerminalNode
    {
        const string DELIMITER = "//";
        const string UNLOCKED_TEXT = "UNLOCKED";
        public OneTimeTerminalNode(string name, int unlockPrice, string description, GameObject prefab, string simplifiedDescription = "") : base(name, unlockPrice, description, prefab, simplifiedDescription: simplifiedDescription)
        {

        }

        internal override string GetTerminalNodeText()
        {
            bool unlocked = Unlocked && MaxUpgrade == 0;
            if (unlocked) return $"{Name} {DELIMITER} {UNLOCKED_TEXT}\n";

            string saleStatus = salePerc < 1f ? $"- {((1 - salePerc) * 100).ToString("F0")}% OFF!" : "";
            return $"{Name} {DELIMITER} {(int)(UnlockPrice*salePerc)} {saleStatus}\n";
        }
    }
}
