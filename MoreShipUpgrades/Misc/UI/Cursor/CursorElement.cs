using System;
using System.Text;
using MoreShipUpgrades.Misc.Util;

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
            sb.Append(Name);
            if (Description == null || Description == "") return sb.ToString();
            sb.AppendLine().Append(Tools.WrapText(Description, availableLength));
            return sb.ToString();
        }

        public static CursorElement Create(string name = "", string description = "", Action action = default)
        {
            return new CursorElement()
            {
                Name = name,
                Description = description,
                Action = action
            };
        }
    }
}
