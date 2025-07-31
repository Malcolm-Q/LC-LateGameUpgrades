using UnityEngine;

namespace MoreShipUpgrades.UI.TerminalNodes
{
    internal class OneTimeTerminalNode : CustomTerminalNode
    {
        public OneTimeTerminalNode(string name, int unlockPrice, string description, GameObject prefab, string originalName = "", bool sharedUpgrade = false, bool alternateCurrency = true, PurchaseMode purchaseMode = default, bool refundable = false, float refundPercentage = 1f) : base(name, unlockPrice, description, prefab, originalName: originalName, sharedUpgrade: sharedUpgrade, alternateCurrency: alternateCurrency, purchaseMode: purchaseMode, refundable: refundable, refundPercentage: refundPercentage)
        {

        }
    }
}
