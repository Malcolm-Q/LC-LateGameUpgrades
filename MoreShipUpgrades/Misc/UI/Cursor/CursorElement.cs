using System;
using System.Collections.Generic;
using System.Text;

namespace MoreShipUpgrades.Misc.UI.Cursor
{
    internal class CursorElement : ITextElement
    {
        internal string Name { get; set; }
        internal string Description { get; set; }

        internal Action Action { get; set; }
        public virtual string GetText(int availableLength)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(Name).AppendLine().Append(Description); // TODO: wrap description
            return sb.ToString();
        }
    }
}
