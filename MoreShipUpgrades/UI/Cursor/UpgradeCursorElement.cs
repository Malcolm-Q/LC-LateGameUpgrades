using InteractiveTerminalAPI.UI.Cursor;
using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc.Util;
using MoreShipUpgrades.UI.TerminalNodes;
using System.Text;
using UnityEngine;

namespace MoreShipUpgrades.UI.Cursor
{
    internal class UpgradeCursorElement : CursorElement
    {
        internal CustomTerminalNode Node { get; set; }
        public override string GetText(int availableLength)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(new string(LguConstants.WHITE_SPACE, 2));
            string name = Node.Name.Length > LguConstants.NAME_LENGTH ? Node.Name.Substring(0, LguConstants.NAME_LENGTH) : Node.Name + new string(LguConstants.WHITE_SPACE, Mathf.Max(0, LguConstants.NAME_LENGTH - Node.Name.Length));
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
                AppendPriceText(ref sb);
                AppendSaleText(ref sb);
            }
            else
            {
                sb.Append("Maxed!");
            }
            if (!Active(this)) sb.Append(LguConstants.COLOR_FINAL_FORMAT);
            return sb.ToString();
        }

        void AppendPriceText(ref StringBuilder sb)
        {
            int price = Node.GetCurrentPrice();
            int currentCredits = UpgradeBus.Instance.GetTerminal().groupCredits;
            if (price <= currentCredits)
            {
                sb.Append(price);
                sb.Append("$");
            }
            else
            {
                sb.Append(string.Format(LguConstants.COLOR_INITIAL_FORMAT, LguConstants.HEXADECIMAL_DARK_RED));
                sb.Append(price);
                sb.Append("$");
                sb.Append(LguConstants.COLOR_FINAL_FORMAT);
            }
            if (!CurrencyManager.Enabled) return;
            sb.Append("/");
            int currencyPrice = CurrencyManager.Instance.GetCurrencyAmountFromCredits(price);
            int currentPlayerCredits = CurrencyManager.Instance.CurrencyAmount;
            if (currencyPrice <= currentPlayerCredits)
            {
                sb.Append(currencyPrice);
                sb.Append(LguConstants.WHITE_SPACE);
                sb.Append(LguConstants.ALTERNATIVE_CURRENCY_ALIAS);
            }
            else
            {
                sb.Append(string.Format(LguConstants.COLOR_INITIAL_FORMAT, LguConstants.HEXADECIMAL_DARK_RED));
                sb.Append(currencyPrice);
                sb.Append(LguConstants.WHITE_SPACE);
                sb.Append(LguConstants.ALTERNATIVE_CURRENCY_ALIAS);
                sb.Append(LguConstants.COLOR_FINAL_FORMAT);
            }
        }

        void AppendSaleText(ref StringBuilder sb)
        {
            if (Node.SalePercentage < 1f)
            {
                sb.Append(LguConstants.WHITE_SPACE);
                sb.Append($"({(1 - Node.SalePercentage) * 100:F0}% OFF)");
            }
        }

    }
}
