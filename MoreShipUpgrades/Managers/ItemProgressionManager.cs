using MoreShipUpgrades.Misc.TerminalNodes;
using MoreShipUpgrades.Misc.Upgrades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MoreShipUpgrades.Managers
{
    public class ItemProgressionManager
    {
        public enum CollectionModes
        {
            CustomScrap,
            UniqueScrap,
            NearestValue,
            ChancePerScrap,
            Apparatice,
        }

        public enum ChancePerScrapModes
        {
            Random,
            Cheapest,
            LowestLevel,
        }
        static List<string> blacklistedItems = new List<string>();
        static Dictionary<CustomTerminalNode, List<string>> scrapToCollectionUpgrade = new Dictionary<CustomTerminalNode, List<string>>();
        static CollectionModes CurrentCollectionMode
        {
            get
            {
                return UpgradeBus.Instance.PluginConfiguration.ITEM_PROGRESSION_MODE;
            }
        }
        static ChancePerScrapModes CurrentChancePerScrapMode
        {
            get
            {
                return UpgradeBus.Instance.PluginConfiguration.SCRAP_UPGRADE_CHANCE_MODE;
            }
        }
        static float ConfiguredChancePerScrapValue
        {
            get
            {
                return UpgradeBus.Instance.PluginConfiguration.SCRAP_UPGRADE_CHANCE;
            }
        }
        public static void CheckCollectionScrap(GrabbableObject scrapItem)
        {
            string scrapName = scrapItem.itemProperties.itemName.ToLower();
            if (IsBlacklisted(scrapName)) return;
            switch (CurrentCollectionMode)
            {
                case CollectionModes.Apparatice:
                    {
                        if (scrapName != "apparatus") break;

                        CustomTerminalNode randomNode = PickRandomCollectionUpgrade();
                        LguStore.Instance.HandleUpgradeClientRpc(randomNode.OriginalName, randomNode.Unlocked);
                        break;
                    }
                case CollectionModes.ChancePerScrap:
                    {
                        if (UnityEngine.Random.Range(0, 1) >= ConfiguredChancePerScrapValue) break;
                        CustomTerminalNode node = SelectChancePerScrapCollectionUpgrade();
                        LguStore.Instance.HandleUpgradeClientRpc(node.OriginalName, node.Unlocked);
                        break;
                    }
            }
        }

        private static bool IsBlacklisted(string scrapName)
        {
            return blacklistedItems.Contains(scrapName);
        }

        public static CustomTerminalNode PickRandomCollectionUpgrade()
        {
            return scrapToCollectionUpgrade.Keys.ToArray()[UnityEngine.Random.Range(0, scrapToCollectionUpgrade.Count)];
        }

        public static void AddCollectionUpgrade(CustomTerminalNode node, List<string> scrapNames)
        {
            scrapToCollectionUpgrade.Add(node, scrapNames);
        }

        public static void AddCollectionUpgrade(CustomTerminalNode node, string scrapName)
        {
            if (!scrapToCollectionUpgrade.ContainsKey(node)) AddCollectionUpgrade(node, new List<string>());
            scrapToCollectionUpgrade[node].Add(scrapName);
        }

        public static CustomTerminalNode SelectChancePerScrapCollectionUpgrade()
        {
            CustomTerminalNode node = null;
            foreach (CustomTerminalNode randomNode in scrapToCollectionUpgrade.Keys)
            {
                if (node == null)
                {
                    node = randomNode;
                    continue;
                }
                switch (CurrentChancePerScrapMode)
                {
                    case ChancePerScrapModes.Random:
                        {
                            if (UnityEngine.Random.Range(0, 1) > 0.5) node = randomNode;
                            break;
                        }
                    case ChancePerScrapModes.LowestLevel:
                        {
                        if (node.Unlocked && (!randomNode.Unlocked || node.MaxUpgrade <= node.CurrentUpgrade || node.CurrentUpgrade > randomNode.CurrentUpgrade && randomNode.MaxUpgrade <= randomNode.CurrentUpgrade))
                            node = randomNode;
                        break;
                        }
                    case ChancePerScrapModes.Cheapest:
                        {
                            int nodePrice = node.GetCurrentPrice();
                            int randomNodePrice = randomNode.GetCurrentPrice();
                            if (nodePrice > randomNodePrice) node = randomNode;
                            break;
                        }
                }
            }
            return node;
        }
    }
}
