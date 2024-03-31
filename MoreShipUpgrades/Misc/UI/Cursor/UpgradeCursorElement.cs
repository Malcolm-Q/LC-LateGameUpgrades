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
        const int NAME_LENGTH = 17;
        const int LEVEL_LENGTH = 7;

        public override string GetText(int availableLength)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(new string(MainUpgradeApplication.WHITE_SPACE, 2));
            string name = Node.Name.Length > NAME_LENGTH ? Node.Name.Substring(0, NAME_LENGTH) : Node.Name + new string(MainUpgradeApplication.WHITE_SPACE, NAME_LENGTH-Node.Name.Length);
            sb.Append(name);

            int currentLevel = Node.Unlocked ? Node.CurrentUpgrade + 1 : 0;
            int remainingLevels = Node.Unlocked ? 0 : 1;
            remainingLevels += Node.MaxUpgrade != 0 ? Node.MaxUpgrade - Node.CurrentUpgrade : 0;
            string levels = new string(FILLED_LEVEL, currentLevel) + new string(EMPTY_LEVEL, remainingLevels) + new string(MainUpgradeApplication.WHITE_SPACE, LEVEL_LENGTH - currentLevel - remainingLevels );
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
            return sb.ToString();
        }
    }
}
