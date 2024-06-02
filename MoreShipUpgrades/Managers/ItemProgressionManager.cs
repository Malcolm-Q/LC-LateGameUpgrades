using MoreShipUpgrades.Misc.TerminalNodes;
using MoreShipUpgrades.Misc.Upgrades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using static Unity.Audio.Handle;

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

        static Dictionary<string, int> contributedRecently = new Dictionary<string, int>();
        internal static CollectionModes CurrentCollectionMode
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
        static float ConfiguredItemContributionMultiplier
        {
            get
            {
                return UpgradeBus.Instance.PluginConfiguration.ITEM_PROGRESSION_CONTRIBUTION_MULTIPLIER;
            }
        }
        
        public static void CheckCollectionScrap(GrabbableObject scrapItem)
        {
            if (!UpgradeBus.Instance.PluginConfiguration.ALTERNATIVE_ITEM_PROGRESSION) return;

            string scrapName = scrapItem.itemProperties.itemName.ToLower();
            LguStore.Instance.DiscoverItemClientRpc(scrapName);
            switch (CurrentCollectionMode)
            {
                case CollectionModes.Apparatice:
                    {
                        if (scrapName != "apparatus") break;

                        CustomTerminalNode randomNode = PickRandomUpgrade();
                        LguStore.Instance.HandleUpgradeForNoHostClientRpc(randomNode.OriginalName, randomNode.Unlocked);
                        LguStore.Instance.UpdateUpgrades(randomNode, randomNode.Unlocked);
                        break;
                    }
                case CollectionModes.ChancePerScrap:
                    {
                        if (UnityEngine.Random.Range(0, 1) >= ConfiguredChancePerScrapValue) break;
                        CustomTerminalNode node = SelectChancePerScrapUpgrade();
                        LguStore.Instance.HandleUpgradeForNoHostClientRpc(node.OriginalName, node.Unlocked);
                        LguStore.Instance.UpdateUpgrades(node, node.Unlocked);
                        break;
                    }
                case CollectionModes.UniqueScrap:
                    {
                        int scrapValue = scrapItem.scrapValue;
                        scrapValue = Mathf.CeilToInt(StartOfRound.Instance.companyBuyingRate * scrapValue);
                        scrapValue = Mathf.CeilToInt(ConfiguredItemContributionMultiplier * scrapValue);
                        CustomTerminalNode assignedUpgrade = GetCustomTerminalNode(UpgradeBus.Instance.scrapToCollectionUpgrade[scrapName]);
                        int contributed = UpgradeBus.Instance.contributionValues[assignedUpgrade.OriginalName];
                        int currentPrice = assignedUpgrade.GetCurrentPrice();
                        contributed += scrapValue;
                        while (contributed > currentPrice)
                        {
                            LguStore.Instance.HandleUpgradeForNoHostClientRpc(assignedUpgrade.OriginalName, assignedUpgrade.Unlocked);
                            LguStore.Instance.UpdateUpgrades(assignedUpgrade, assignedUpgrade.Unlocked);
                            contributed -= currentPrice;
                            currentPrice = assignedUpgrade.GetCurrentPrice();
                        }
                        LguStore.Instance.SetContributionValueClientRpc(assignedUpgrade.OriginalName, contributed);
                        break;
                    }
            }
        }

        public static void CheckNewQuota(int fullfilledQuota)
        {
            if (!LguStore.Instance.IsServer || !UpgradeBus.Instance.PluginConfiguration.ALTERNATIVE_ITEM_PROGRESSION) return;

            switch(CurrentCollectionMode)
            {
                case CollectionModes.NearestValue:
                    {
                        CustomTerminalNode node = SelectNearestValueUpgrade(fullfilledQuota);
                        if (node == null) return; // No upgrades had a price below the fullfilled quota
                        LguStore.Instance.HandleUpgradeClientRpc(node.OriginalName, node.Unlocked);
                        break;
                    }
            }
        }

        public static CustomTerminalNode SelectNearestValueUpgrade(int fullfilledQuota)
        {
            CustomTerminalNode currentNode = null;
            int currentDelta = int.MaxValue;
            foreach (CustomTerminalNode node in UpgradeBus.Instance.terminalNodes)
            {
                int price = node.GetCurrentPrice();
                int delta = fullfilledQuota - price;
                if (delta > 0 && delta < currentDelta) currentNode = node;
            }
            return currentNode;
        }

        private static bool IsBlacklisted(string scrapName)
        {
            return blacklistedItems.Contains(scrapName);
        }

        public static CustomTerminalNode PickRandomUpgrade()
        {
            return UpgradeBus.Instance.terminalNodes.ToArray()[UnityEngine.Random.Range(0, UpgradeBus.Instance.terminalNodes.Count)];
        }

        public static void AssignRandomScrapToUpgrades()
        {
            AllItemsList allItemsList = StartOfRound.Instance.allItemsList;
            foreach (Item item in allItemsList.itemsList)
            {
                string itemName = item.itemName.ToLower();
                if (UpgradeBus.Instance.scrapToCollectionUpgrade.ContainsKey(itemName)) continue;
                CustomTerminalNode node = PickRandomUpgrade();
                AddScrapToUpgrade(ref node, itemName);
            }
        }

        public static void AddScrapToUpgrade(ref CustomTerminalNode node, List<string> scrapNames)
        {
            foreach (string scrapName in scrapNames)
                AddScrapToUpgrade(ref node, scrapName);
        }

        public static void AddScrapToUpgrade(ref CustomTerminalNode node, string scrapName)
        {
            if (UpgradeBus.Instance.scrapToCollectionUpgrade.ContainsKey(scrapName)) UpgradeBus.Instance.scrapToCollectionUpgrade[scrapName] = node.OriginalName;
            else UpgradeBus.Instance.scrapToCollectionUpgrade.Add(scrapName, node.OriginalName);
        }

        public static void AddScrapToUpgrade(string upgradeName, string scrapName)
        {
            if (UpgradeBus.Instance.scrapToCollectionUpgrade.ContainsKey(scrapName)) UpgradeBus.Instance.scrapToCollectionUpgrade[scrapName] = upgradeName;
            else UpgradeBus.Instance.scrapToCollectionUpgrade.Add(scrapName, upgradeName);
        }

        public static CustomTerminalNode SelectChancePerScrapUpgrade()
        {
            CustomTerminalNode node = null;
            foreach (CustomTerminalNode randomNode in UpgradeBus.Instance.terminalNodes)
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

        internal static void InitializeContributionValues()
        {
            UpgradeBus.Instance.contributionValues.Clear();
            foreach (CustomTerminalNode node in UpgradeBus.Instance.terminalNodes)
                UpgradeBus.Instance.contributionValues.Add(node.OriginalName, 0);
        }

        internal static void SetContributionValue(string key, int value)
        {
            UpgradeBus.Instance.contributionValues[key] = value;
            if (!contributedRecently.ContainsKey(key)) contributedRecently.Add(key, value);
            else contributedRecently[key] = value;
        }
        internal static int GetCurrentContribution(CustomTerminalNode node)
        {
            return UpgradeBus.Instance.contributionValues[node.OriginalName];
        }

        internal static List<string> GetDiscoveredItems(CustomTerminalNode node)
        {
            List<string> result = new();
            foreach (KeyValuePair<string, string> pair in UpgradeBus.Instance.scrapToCollectionUpgrade)
            {
                string nodeName = pair.Value;
                string scrapName = pair.Key;
                if (nodeName == node.OriginalName && UpgradeBus.Instance.discoveredItems.Contains(scrapName))
                    result.Add(scrapName);
            }
            return result;
        }

        internal static CustomTerminalNode GetCustomTerminalNode(string name)
        {
            foreach (CustomTerminalNode node in UpgradeBus.Instance.terminalNodes)
                if (node.OriginalName == name) return node;
            return null;
        }

        internal static void DiscoverScrap(string scrapName)
        {
            if (!UpgradeBus.Instance.discoveredItems.Contains(scrapName)) UpgradeBus.Instance.discoveredItems.Add(scrapName);
        }
    }
}
