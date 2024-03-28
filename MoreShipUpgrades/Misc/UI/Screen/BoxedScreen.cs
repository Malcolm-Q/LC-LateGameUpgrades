using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MoreShipUpgrades.Misc.UI.Screen
{
    internal class BoxedScreen : IScreen
    {
        internal const char TOP_LEFT_CORNER = '╭';
        internal const char TOP_RIGHT_CORNER = '╮';
        internal const char BOTTOM_LEFT_CORNER = '╰';
        internal const char BOTTOM_RIGHT_CORNER = '╯';
        internal const char VERTICAL_LINE = '│';
        internal const char HORIZONTAL_LINE = '─';
        internal const char WHITE_SPACE = ' ';

        internal const char CONNECTING_TITLE_LEFT = '╢';
        internal const char CONNECTING_TITLE_RIGHT = '╟';
        internal const char TOP_RIGHT_TITLE_CORNER = '╗';
        internal const char TOP_LEFT_TITLE_CORNER = '╔';
        internal const char BOTTOM_RIGHT_TITLE_CORNER = '╝';
        internal const char BOTTOM_LEFT_TITLE_CORNER = '╚';
        internal const char VERTICAL_TITLE_LINE = '║';
        internal const char HORIZONTAL_TITLE_LINE = '═';

        internal string Title;
        internal ITextElement[] elements;
        public string GetText(int availableLength)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(new string(WHITE_SPACE, 2))
                .Append(TOP_LEFT_TITLE_CORNER)
                .Append(new string(HORIZONTAL_TITLE_LINE, Title.Length))
                .Append(TOP_RIGHT_TITLE_CORNER)
                .AppendLine();
            sb.Append(TOP_LEFT_CORNER)
                .Append(CONNECTING_TITLE_LEFT)
                .Append(WHITE_SPACE)
                .Append(Title)
                .Append(WHITE_SPACE)
                .Append(CONNECTING_TITLE_RIGHT)
                .Append(new string(HORIZONTAL_LINE, availableLength - 6 - Title.Length))
                .AppendLine();
            sb.Append(VERTICAL_LINE)
                .Append(BOTTOM_LEFT_TITLE_CORNER)
                .Append(new string(HORIZONTAL_TITLE_LINE, Title.Length))
                .Append(BOTTOM_RIGHT_TITLE_CORNER)
                .Append(new string(WHITE_SPACE, availableLength - 6 - Title.Length))
                .Append(VERTICAL_LINE)
                .AppendLine();
            for(int i = 0; i < elements.Length; i++)
            {
                //TODO: Get element text wrapped over our screen (surround with VERTICAL_LINE and limit each line to availableLength-4 (4 because of lines and white spaces in each side)
            }
            sb.Append(BOTTOM_LEFT_CORNER)
                .Append(new string(HORIZONTAL_LINE, availableLength - 2))
                .Append(BOTTOM_RIGHT_CORNER)
                .AppendLine();


            return sb.ToString();
        }
    }
}
