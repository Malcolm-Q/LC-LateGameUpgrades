using MoreShipUpgrades.Misc.Util;

namespace MoreShipUpgrades.Misc.UI
{
    internal class TextElement : ITextElement
    {
        internal string Text { get; set; }
        public string GetText(int availableLength)
        {
            return Tools.WrapText(Text, availableLength);
        }
        public static TextElement Create(string text = "")
        {
            return new TextElement()
            {
                Text = text
            };
        }
    }
}
