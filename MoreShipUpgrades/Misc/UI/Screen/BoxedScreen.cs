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
            sb.Append(new string(Constants.WHITE_SPACE, 1))
                .Append(Constants.TOP_LEFT_TITLE_CORNER)
                .Append(new string(Constants.HORIZONTAL_TITLE_LINE, Title.Length+2))
                .Append(Constants.TOP_RIGHT_TITLE_CORNER)
                .AppendLine();
            sb.Append(Constants.TOP_LEFT_CORNER)
                .Append(Constants.CONNECTING_TITLE_LEFT)
                .Append(Constants.WHITE_SPACE)
                .Append(Title)
                .Append(Constants.WHITE_SPACE)
                .Append(Constants.CONNECTING_TITLE_RIGHT)
                .Append(new string(Constants.HORIZONTAL_LINE, availableLength - 6 - Title.Length))
                .Append(Constants.TOP_RIGHT_CORNER)
                .AppendLine();
            sb.Append(Constants.VERTICAL_LINE)
                .Append(Constants.BOTTOM_LEFT_TITLE_CORNER)
                .Append(new string(Constants.HORIZONTAL_TITLE_LINE, Title.Length+2))
                .Append(Constants.BOTTOM_RIGHT_TITLE_CORNER)
                .Append(new string(Constants.WHITE_SPACE, availableLength - 6 - Title.Length))
                .Append(Constants.VERTICAL_LINE)
                .AppendLine();
            for(int i = 0; i < elements.Length; i++)
            {
                sb.Append(Tools.WrapText(elements[i].GetText(availableLength - 4), availableLength, leftPadding: "│ ", rightPadding: " │"));
            }
            sb.Append(Constants.BOTTOM_LEFT_CORNER)
                .Append(new string(Constants.HORIZONTAL_LINE, availableLength - 2))
                .Append(Constants.BOTTOM_RIGHT_CORNER)
                .AppendLine();


            return sb.ToString();
        }
    }
}
