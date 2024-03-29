using MoreShipUpgrades.Misc.TerminalNodes;
using System;
using System.Collections.Generic;
using System.Text;
using static Unity.Audio.Handle;

namespace MoreShipUpgrades.Misc.UI.Cursor
{
    internal class UpgradeCursorElement : CursorElement
    {
        internal CustomTerminalNode Node { get; set; }

        internal const char EMPTY_LEVEL = '○';
        internal const char FILLED_LEVEL = '●';

        public override string GetText(int availableLength)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(Node.Name);

            int currentLevel = Node.Unlocked ? Node.CurrentUpgrade + 1 : 0;
            int remainingLevels = Node.Unlocked ? 0 : 1;
            remainingLevels += Node.MaxUpgrade != 0 ? Node.MaxUpgrade - Node.CurrentUpgrade : 0;
            string levels = new string(FILLED_LEVEL, currentLevel) + new string(EMPTY_LEVEL, remainingLevels);
            sb.Append(MainUpgradeApplication.WHITE_SPACE);
            sb.Append(levels);
            if (remainingLevels > 0)
            {
                sb.Append(MainUpgradeApplication.WHITE_SPACE);
                sb.Append(Node.Unlocked ? (int)(Node.Prices[Node.CurrentUpgrade] * Node.salePerc) : (int)(Node.UnlockPrice * Node.salePerc) + "$");
            }
            if (Node.salePerc < 1f)
            {
                sb.Append(MainUpgradeApplication.WHITE_SPACE);
                sb.Append($"({((1 - Node.salePerc) * 100).ToString("F0")}% OFF)");
            }
            sb.AppendLine();
            return sb.ToString();
        }
    }
}
