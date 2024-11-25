using MoreShipUpgrades.Managers;
using MoreShipUpgrades.UI.TerminalNodes;
using System.Collections.Generic;
using System.Linq;

namespace MoreShipUpgrades.API
{
    public static class UpgradeApi
    {
        public static void TriggerUpgradeRankup(string upgradeName)
        {
            CustomTerminalNode pickedNode = UpgradeBus.Instance.GetUpgradeNode(upgradeName);
            TriggerUpgradeRankup(pickedNode);
        }
        public static void TriggerUpgradeRankup(CustomTerminalNode node)
        {
            if (!CheckGeneralNodeConditions(node)) return;
            LguStore.Instance.HandleUpgrade(node, node.Unlocked);
        }

        public static void ContributeTowardsUpgrade(string upgradeName, int scrapValue)
        {
            CustomTerminalNode pickedNode = UpgradeBus.Instance.GetUpgradeNode(upgradeName);
            ContributeTowardsUpgrade(pickedNode, scrapValue);
        }
        public static void ContributeTowardsUpgrade(CustomTerminalNode node, int scrapValue)
        {
            if (!CheckGeneralNodeConditions(node)) return;
            ItemProgressionManager.ContributeTowardsUpgrade(node, scrapValue);
        }

        public static bool CheckGeneralNodeConditions(CustomTerminalNode node)
        {
            if (node == null)
            {
                Plugin.mls.LogWarning("Provided upgrade node does not exist. Stopping requested execution...");
                return false;
            }
            if (!UpgradeBus.ContainsUpgradeNode(node))
            {
                Plugin.mls.LogWarning("Provided upgrade node was created outside of LGU's generation. Stopping requested execution...");
                return false;
            }
            return true;
        }

        public static List<CustomTerminalNode> GetUpgradeNodes()
        {
            return UpgradeBus.GetUpgradeNodes();
        }

        private static IEnumerable<CustomTerminalNode> GetVisibleUpgradeNodesInternal()
        {
            return GetUpgradeNodes().Where(x => x.Visible);
        }

        public static List<CustomTerminalNode> GetVisibleUpgradeNodes()
        {
            return GetVisibleUpgradeNodesInternal().ToList();
        }

        private static IEnumerable<CustomTerminalNode> GetPurchaseableUpgradeNodesInternal()
        {
            return GetVisibleUpgradeNodesInternal().Where(x => x.UnlockPrice > 0 || x.Prices.Length > 0);
        }

        public static List<CustomTerminalNode> GetPurchaseableUpgradeNodes()
        {
            return GetPurchaseableUpgradeNodesInternal().ToList();
        }
        private static IEnumerable<CustomTerminalNode> GetRankableUpgradeNodesInternal()
        {
            return GetPurchaseableUpgradeNodesInternal().Where(x => x.UnlockPrice > 0 || x.Prices.Length > 0);
        }

        public static List<CustomTerminalNode> GetRankableUpgradeNodes()
        {
            return GetRankableUpgradeNodesInternal().ToList();
        }

    }
}
