using InteractiveTerminalAPI.UI.Cursor;
using MoreShipUpgrades.Misc.TerminalNodes;
using MoreShipUpgrades.Misc.Util;
using System.Text;
using UnityEngine;

namespace MoreShipUpgrades.Misc.UI.Cursor
{
    internal class UpgradeCursorElement : CursorElement
    {
        internal CustomTerminalNode Node { get; set; }

        public override string GetText(int availableLength)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(new string(LguConstants.WHITE_SPACE, 2));
            string name = Node.Name.Length > LguConstants.NAME_LENGTH ? Node.Name.Substring(0, LguConstants.NAME_LENGTH) : Node.Name + new string(LguConstants.WHITE_SPACE, Mathf.Max(0, LguConstants.NAME_LENGTH-Node.Name.Length));
            if (!Active(this))
            {
                if (Node.Unlocked && Node.CurrentUpgrade >= Node.MaxUpgrade)
                {
                    sb.Append(string.Format(LguConstants.COLOR_INITIAL_FORMAT, LguConstants.HEXADECIMAL_DARK_GREEN));
                }
                else
                {
                    sb.Append(string.Format(LguConstants.COLOR_INITIAL_FORMAT, LguConstants.HEXADECIMAL_GREY));
                }
            }
            sb.Append(name);

            int currentLevel = Node.GetCurrentLevel();
            int remainingLevels = Node.GetRemainingLevels();
            string levels = new string(LguConstants.FILLED_LEVEL, currentLevel) + new string(LguConstants.EMPTY_LEVEL, remainingLevels) + new string(LguConstants.WHITE_SPACE, Mathf.Max(0, LguConstants.LEVEL_LENGTH - currentLevel - remainingLevels));
            sb.Append(LguConstants.WHITE_SPACE);
            sb.Append(levels);
            sb.Append(LguConstants.WHITE_SPACE);
            if (remainingLevels > 0)
            {
                sb.Append(Node.GetCurrentPrice() + "$");
                if (Node.SalePercentage < 1f)
                {
                    sb.Append(LguConstants.WHITE_SPACE);
                    sb.Append($"({(1 - Node.SalePercentage) * 100:F0}% OFF)");
                }
            }
            else
            {
                sb.Append("Maxed!");
            }
            if (!Active(this)) sb.Append(LguConstants.COLOR_FINAL_FORMAT);
            return sb.ToString();
        }
    }
}
