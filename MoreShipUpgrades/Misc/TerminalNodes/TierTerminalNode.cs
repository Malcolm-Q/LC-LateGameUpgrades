using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;
using UnityEngine;

namespace MoreShipUpgrades.Misc.TerminalNodes
{
    internal class TierTerminalNode : CustomTerminalNode
    {
        const string DELIMITER = "//";
        const string MAX_LEVEL_TEXT = "MAX LVL";
        public TierTerminalNode(string name, int unlockPrice, string description, GameObject prefab, int[] prices, int maxUpgrade, string simplifiedDescription = "") : base(name, unlockPrice, description, prefab, prices, maxUpgrade, simplifiedDescription: simplifiedDescription)
        {

        }

        internal override string GetTerminalNodeText()
        {
            bool maxed = Unlocked && CurrentUpgrade >= MaxUpgrade;
            if (maxed) return $"{Name} {DELIMITER} {MAX_LEVEL_TEXT}\n";

            string saleStatus = salePerc < 1f ? $"- {((1 - salePerc) * 100).ToString("F0")}% OFF!" : "";
            if (!Unlocked) return $"{Name} {DELIMITER} {(int)(UnlockPrice * salePerc)} {saleStatus}\n";

            return $"{Name} {DELIMITER} {(int)(Prices[CurrentUpgrade]*salePerc)} {DELIMITER} LVL {CurrentUpgrade+1} {saleStatus}\n";
        }
    }
}
