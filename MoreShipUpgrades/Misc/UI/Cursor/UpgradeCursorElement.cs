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
            sb.Append(new string(LGUConstants.WHITE_SPACE, 2));
            string name = Node.Name.Length > LGUConstants.NAME_LENGTH ? Node.Name.Substring(0, LGUConstants.NAME_LENGTH) : Node.Name + new string(LGUConstants.WHITE_SPACE, Mathf.Max(0, LGUConstants.NAME_LENGTH-Node.Name.Length));
            if (!Active(this)) sb.Append(string.Format(LGUConstants.COLOR_INITIAL_FORMAT, LGUConstants.HEXADECIMAL_GREY));
            sb.Append(name);

            int currentLevel = Node.GetCurrentLevel();
            int remainingLevels = Node.GetRemainingLevels();
            string levels = new string(LGUConstants.FILLED_LEVEL, currentLevel) + new string(LGUConstants.EMPTY_LEVEL, remainingLevels) + new string(LGUConstants.WHITE_SPACE, Mathf.Max(0, LGUConstants.LEVEL_LENGTH - currentLevel - remainingLevels));
            sb.Append(LGUConstants.WHITE_SPACE);
            sb.Append(levels);
            if (remainingLevels > 0)
            {
                sb.Append(LGUConstants.WHITE_SPACE);
                sb.Append(Node.GetCurrentPrice() + "$");
                if (Node.SalePercentage < 1f)
                {
                    sb.Append(LGUConstants.WHITE_SPACE);
                    sb.Append($"({(1 - Node.SalePercentage) * 100:F0}% OFF)");
                }
            }
            if (!Active(this)) sb.Append(LGUConstants.COLOR_FINAL_FORMAT);
            return sb.ToString();
        }
    }
}
