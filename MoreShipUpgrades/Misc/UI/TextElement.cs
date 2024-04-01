using System;
using System.Collections.Generic;
using System.Text;
using MoreShipUpgrades.Misc.Util;

namespace MoreShipUpgrades.Misc.UI
{
    internal class TextElement : ITextElement
    {
        internal string Text { get; set; }
        public string GetText(int availableLength)
        {
            return Tools.WrapText(Text, availableLength);
        }
    }
}
