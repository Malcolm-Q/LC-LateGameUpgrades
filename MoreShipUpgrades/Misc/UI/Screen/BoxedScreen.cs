using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MoreShipUpgrades.Misc.Util;

namespace MoreShipUpgrades.Misc.UI.Screen
{
    internal class BoxedScreen : IScreen
    {
        internal string Title;
        internal ITextElement[] elements;
        public string GetText(int availableLength)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine().AppendLine();
            sb.Append(new string(LGUConstants.WHITE_SPACE, 1))
                .Append(LGUConstants.TOP_LEFT_TITLE_CORNER)
                .Append(new string(LGUConstants.HORIZONTAL_TITLE_LINE, Title.Length+2))
                .Append(LGUConstants.TOP_RIGHT_TITLE_CORNER)
                .AppendLine();
            sb.Append(LGUConstants.TOP_LEFT_CORNER)
                .Append(LGUConstants.CONNECTING_TITLE_LEFT)
                .Append(LGUConstants.WHITE_SPACE)
                .Append(Title)
                .Append(LGUConstants.WHITE_SPACE)
                .Append(LGUConstants.CONNECTING_TITLE_RIGHT)
                .Append(new string(LGUConstants.HORIZONTAL_LINE, availableLength - 6 - Title.Length))
                .Append(LGUConstants.TOP_RIGHT_CORNER)
                .AppendLine();
            sb.Append(LGUConstants.VERTICAL_LINE)
                .Append(LGUConstants.BOTTOM_LEFT_TITLE_CORNER)
                .Append(new string(LGUConstants.HORIZONTAL_TITLE_LINE, Title.Length+2))
                .Append(LGUConstants.BOTTOM_RIGHT_TITLE_CORNER)
                .Append(new string(LGUConstants.WHITE_SPACE, availableLength - 6 - Title.Length))
                .Append(LGUConstants.VERTICAL_LINE)
                .AppendLine();
            for(int i = 0; i < elements.Length; i++)
            {
                sb.Append(Tools.WrapText(elements[i].GetText(availableLength - 4), availableLength, leftPadding: "│ ", rightPadding: " │"));
            }
            sb.Append(LGUConstants.BOTTOM_LEFT_CORNER)
                .Append(new string(LGUConstants.HORIZONTAL_LINE, availableLength - 2))
                .Append(LGUConstants.BOTTOM_RIGHT_CORNER)
                .AppendLine();


            return sb.ToString();
        }
    }
}
