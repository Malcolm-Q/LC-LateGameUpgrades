using UnityEngine;

namespace MoreShipUpgrades.UI.TerminalNodes
{
    internal class OneTimeTerminalNode : CustomTerminalNode
    {
        public OneTimeTerminalNode(string name, int unlockPrice, string description, GameObject prefab, string originalName = "", bool sharedUpgrade = false, bool alternateCurrency = true, PurchaseMode purchaseMode = default) : base(name, unlockPrice, description, prefab, originalName: originalName, sharedUpgrade: sharedUpgrade, alternateCurrency: alternateCurrency, purchaseMode: purchaseMode)
        {

        }
    }
}
