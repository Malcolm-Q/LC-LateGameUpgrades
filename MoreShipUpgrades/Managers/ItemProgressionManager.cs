using MoreShipUpgrades.Misc.Upgrades;
using MoreShipUpgrades.Misc.Util;
using MoreShipUpgrades.UI.TerminalNodes;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using static Unity.Audio.Handle;

namespace MoreShipUpgrades.Managers
{
    public static class ItemProgressionManager
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
        static readonly List<string> blacklistedItems = [];
        static readonly Dictionary<string, int> contributedRecently = [];
        static readonly Dictionary<string, int> apparatusItems = [];
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
        static void ExecuteApparaticeLogic(GrabbableObject scrapItem, string scrapName)
        {
            if (!VerifyApparatus(ref scrapItem, ref scrapName)) return;
            int upgrades = apparatusItems[scrapName];

            for(int i = 0; i < upgrades; i++)
            {
                RankUpUpgrade(PickRandomUpgrade());
            }
        }

        static void ExecuteChancerPerScrapLogic()
        {
            if (UnityEngine.Random.Range(0f, 1f) >= ConfiguredChancePerScrapValue) return;
            RankUpUpgrade(SelectChancePerScrapUpgrade());
        }

        static void ExecuteSpecificScrapLogic(GrabbableObject scrapItem, string scrapName)
        {
            if (!VerifyScrap(ref scrapItem, ref scrapName)) return;
            int scrapValue = scrapItem.scrapValue;
            scrapValue = Mathf.CeilToInt(StartOfRound.Instance.companyBuyingRate * scrapValue);
            scrapValue = Mathf.CeilToInt(ConfiguredItemContributionMultiplier * scrapValue);
            foreach (string upgradeName in UpgradeBus.Instance.scrapToCollectionUpgrade[scrapName])
            {
                ContributeTowardsUpgrade(upgradeName, scrapValue);
            }
        }
        static void RankUpUpgrade(CustomTerminalNode node)
        {
            LguStore.Instance.HandleUpgradeForNoHostClientRpc(node.OriginalName, node.Unlocked);
            LguStore.Instance.UpdateUpgrades(node, node.Unlocked);
        }
        static void ContributeTowardsUpgrade(string upgradeName, int scrapValue)
        {
            CustomTerminalNode assignedUpgrade = GetCustomTerminalNode(upgradeName);
            int contributed = UpgradeBus.Instance.contributionValues[assignedUpgrade.OriginalName];
            int currentPrice = assignedUpgrade.GetCurrentPrice() + contributed;
            contributed += scrapValue;
            while (contributed >= currentPrice && assignedUpgrade.GetRemainingLevels() > 0)
            {
                RankUpUpgrade(assignedUpgrade);
                contributed -= currentPrice;
                currentPrice = assignedUpgrade.GetCurrentPrice();
            }
            LguStore.Instance.SetContributionValueClientRpc(assignedUpgrade.OriginalName, contributed);
        }

        static bool VerifyScrap(ref GrabbableObject scrapItem, ref string scrapName)
        {
            return VerifyItem(UpgradeBus.Instance.scrapToCollectionUpgrade, ref scrapItem, ref scrapName);
        }
        static bool VerifyApparatus(ref GrabbableObject scrapItem, ref string scrapName)
        {
            return VerifyItem(apparatusItems, ref scrapItem, ref scrapName);
        }
        static bool VerifyItem<T>(Dictionary<string, T> collection, ref GrabbableObject scrapItem, ref string scrapName)
        {
            if (!collection.ContainsKey(scrapName))
            {
                Plugin.mls.LogInfo($"{scrapName} from ItemProperties was not found in the dictionary, looking through scan node...");
                ScanNodeProperties node = scrapItem.GetComponentInChildren<ScanNodeProperties>();
                if (node == null)
                {
                    Plugin.mls.LogWarning($"{scrapName} doesn't have a scan node, skipping...");
                    return false;
                }
                scrapName = node.headerText.ToLower().Trim();
                if (!collection.ContainsKey(scrapName))
                {
                    Plugin.mls.LogWarning($"{scrapName} from Scan Node was not found in the dictionary.");
                    return false;
                }
            }
            return !IsBlacklisted(scrapName);
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
                        ExecuteApparaticeLogic(scrapItem, scrapName);
                        break;
                    }
                case CollectionModes.ChancePerScrap:
                    {
                        ExecuteChancerPerScrapLogic();
                        break;
                    }
                case CollectionModes.CustomScrap:
                case CollectionModes.UniqueScrap:
                    {
                        ExecuteSpecificScrapLogic(scrapItem, scrapName);
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
                if (delta > 0 && delta < currentDelta)
                {
                    currentDelta = delta;
                    currentNode = node;
                }
            }
            return currentNode;
        }
        public static void InitializeBlacklistItems()
        {
            blacklistedItems.Clear();
            string configuredValue = UpgradeBus.Instance.PluginConfiguration.ITEM_PROGRESSION_BLACKLISTED_ITEMS;
            foreach (string entry in configuredValue.Split(LguConstants.ITEM_PROGRESSION_BLACKLIST_ITEMS_ENTRY_DELIMITER))
            {
                blacklistedItems.Add(entry.ToLower());
            }
        }
        const int ITEM_NAME_INDEX = 0;
        const int ITEM_UPGRADE_COUNTER_INDEX = 1;
        public static void InitializeApparatusItems()
        {
            apparatusItems.Clear();
            string configuredValue = UpgradeBus.Instance.PluginConfiguration.ITEM_PROGRESSION_APPARATICE_ITEMS;
            foreach (string entry in configuredValue.Split(LguConstants.ITEM_PROGRESSION_APPARATICE_ITEMS_ENTRY_DELIMITER))
            {
                string[] item = entry.Split(LguConstants.ITEM_PROGRESSION_APPARATICE_ITEMS_ATTRIBUTE_DELIMITER);
                string itemName = item[ITEM_NAME_INDEX];
                int.TryParse(item[ITEM_UPGRADE_COUNTER_INDEX], out int value);
                apparatusItems[itemName.ToLower()] = value;
            }
        }
        private static bool IsBlacklisted(string scrapName)
        {
            return blacklistedItems.Contains(scrapName);
        }

        public static CustomTerminalNode PickRandomUpgrade()
        {
            return UpgradeBus.Instance.terminalNodes[UnityEngine.Random.Range(0, UpgradeBus.Instance.terminalNodes.Count)];
        }
        static void AssignRandomScrap()
        {
            AllItemsList allItemsList = StartOfRound.Instance.allItemsList;
            if (UpgradeBus.Instance.scrapToCollectionUpgrade.Count == allItemsList.itemsList.Count) return;

            foreach (Item item in allItemsList.itemsList)
            {
                if (!item.isScrap) continue;
                string itemName = item.itemName.ToLower();
                if (UpgradeBus.Instance.scrapToCollectionUpgrade.ContainsKey(itemName)) continue;
                CustomTerminalNode node = PickRandomUpgrade();
                AddScrapToUpgrade(ref node, itemName);
            }
        }
        static void AssignConfiguredScrap()
        {
            UpgradeBus.Instance.scrapToCollectionUpgrade.Clear();
            foreach (Type type in UpgradeBus.Instance.upgradeTypes)
            {
                MethodInfo method = type.GetMethod(nameof(BaseUpgrade.RegisterScrapToUpgrade), BindingFlags.Static | BindingFlags.Public);
                (string, string[]) pair = ((string, string[]))method.Invoke(null, null);
                AddScrapToUpgrade(pair.Item1, pair.Item2);
            }
        }

        public static void AssignScrapToUpgrades()
        {
            switch(CurrentCollectionMode)
            {
                case CollectionModes.UniqueScrap:
                    {
                        AssignRandomScrap();
                        break;
                    }
                case CollectionModes.CustomScrap:
                    {
                        AssignConfiguredScrap();
                        break;
                    }
            }
            LguStore.Instance.ServerSaveFile();
        }

        public static void AddScrapToUpgrade(string upgradeName, string[] scrapNames)
        {
            foreach (string scrapName in scrapNames)
                AddScrapToUpgrade(upgradeName, scrapName);
        }

        public static void AddScrapToUpgrade(ref CustomTerminalNode node, string scrapName)
        {
            AddScrapToUpgrade(node.OriginalName, scrapName);
        }

        public static void AddScrapToUpgrade(string upgradeName, string scrapName)
        {
            string key = scrapName.ToLower().Trim();
            if (!UpgradeBus.Instance.scrapToCollectionUpgrade.ContainsKey(key))
                UpgradeBus.Instance.scrapToCollectionUpgrade[key] = [];
            UpgradeBus.Instance.scrapToCollectionUpgrade[key].Add(upgradeName);
        }

        static void SelectTerminalNode(ref CustomTerminalNode selectedNode, CustomTerminalNode possibleNode)
        {
            if (selectedNode == null)
            {
                selectedNode = possibleNode;
                return;
            }
            switch (CurrentChancePerScrapMode)
            {
                case ChancePerScrapModes.LowestLevel:
                    {
                        if (selectedNode.Unlocked &&
                            (!possibleNode.Unlocked || selectedNode.MaxUpgrade <= selectedNode.CurrentUpgrade || (selectedNode.CurrentUpgrade > possibleNode.CurrentUpgrade && possibleNode.MaxUpgrade <= possibleNode.CurrentUpgrade)))

                        {
                            selectedNode = possibleNode;
                        }
                        break;
                    }
                case ChancePerScrapModes.Cheapest:
                    {
                        int nodePrice = selectedNode.GetCurrentPrice();
                        int randomNodePrice = possibleNode.GetCurrentPrice();
                        if (nodePrice > randomNodePrice) selectedNode = possibleNode;
                        break;
                    }
                default: selectedNode = possibleNode; break;
            }
        }
        public static CustomTerminalNode SelectChancePerScrapUpgrade()
        {
            CustomTerminalNode node = null;
            if (CurrentChancePerScrapMode == ChancePerScrapModes.Random)
            {
                int tries = 0;
                node = PickRandomUpgrade();
                while (node.MaxUpgrade <= node.CurrentUpgrade && tries < UpgradeBus.Instance.terminalNodes.Count*2)
                {
                    node = PickRandomUpgrade();
                    tries++;
                }
                return node;
            }

            foreach (CustomTerminalNode randomNode in UpgradeBus.Instance.terminalNodes)
            {
                SelectTerminalNode(ref node, randomNode);
            }
            return node;
        }

        internal static void InitializeContributionValues()
        {
            foreach (CustomTerminalNode node in UpgradeBus.Instance.terminalNodes)
            {
                if (!UpgradeBus.Instance.contributionValues.ContainsKey(node.OriginalName))
                    UpgradeBus.Instance.contributionValues.Add(node.OriginalName, 0);
            }
        }

        internal static void SetContributionValue(string key, int value)
        {
            UpgradeBus.Instance.contributionValues[key] = value;
            contributedRecently[key] = value;
        }
        internal static int GetCurrentContribution(CustomTerminalNode node)
        {
            return UpgradeBus.Instance.contributionValues[node.OriginalName];
        }

        internal static List<string> GetDiscoveredItems(CustomTerminalNode node)
        {
            List<string> result = [];
            foreach (KeyValuePair<string, List<string>> pair in UpgradeBus.Instance.scrapToCollectionUpgrade)
            {
                List<string> nodes = pair.Value;
                string scrapName = pair.Key;
                foreach (string nodeName in nodes)
                {
                    if (nodeName == node.OriginalName && (UpgradeBus.Instance.discoveredItems.Contains(scrapName) || UpgradeBus.Instance.PluginConfiguration.ITEM_PROGRESSION_ALWAYS_SHOW_ITEMS))
                        result.Add(scrapName);
                }
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
