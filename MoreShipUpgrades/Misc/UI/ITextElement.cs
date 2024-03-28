using System;
using System.Collections.Generic;
using System.Text;

namespace MoreShipUpgrades.Misc.UI
{
    internal interface ITextElement
    {
        public string GetText(int availableLength);
    }
}
