using MoreShipUpgrades.Misc.TerminalNodes;
using MoreShipUpgrades.Misc.Util;
using System.Text;

namespace MoreShipUpgrades.Misc.UI.Cursor
{
    internal class UpgradeCursorElement : CursorElement
    {
        internal CustomTerminalNode Node { get; set; }


        public override string GetText(int availableLength)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(new string(Constants.WHITE_SPACE, 2));
            string name = Node.Name.Length > Constants.NAME_LENGTH ? Node.Name.Substring(0, Constants.NAME_LENGTH) : Node.Name + new string(Constants.WHITE_SPACE, Constants.NAME_LENGTH -Node.Name.Length);
            sb.Append(name);

            int currentLevel = Node.Unlocked ? Node.CurrentUpgrade + 1 : 0;
            int remainingLevels = Node.Unlocked ? 0 : 1;
            remainingLevels += Node.MaxUpgrade != 0 ? Node.MaxUpgrade - Node.CurrentUpgrade : 0;
            string levels = new string(Constants.FILLED_LEVEL, currentLevel) + new string(Constants.EMPTY_LEVEL, remainingLevels) + new string(Constants.WHITE_SPACE, Constants.LEVEL_LENGTH - currentLevel - remainingLevels );
            sb.Append(Constants.WHITE_SPACE);
            sb.Append(levels);
            if (remainingLevels > 0)
            {
                sb.Append(Constants.WHITE_SPACE);
                sb.Append((Node.Unlocked ? (int)(Node.Prices[Node.CurrentUpgrade] * Node.salePerc) : (int)(Node.UnlockPrice * Node.salePerc)) + "$");
            }
            if (Node.salePerc < 1f)
            {
                sb.Append(Constants.WHITE_SPACE);
                sb.Append($"({(1 - Node.salePerc) * 100:F0}% OFF)");
            }
            return sb.ToString();
        }
    }
}
