using MoreShipUpgrades.Misc.TerminalNodes;
using MoreShipUpgrades.Misc.Upgrades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MoreShipUpgrades.Managers
{
    internal class CollectionUpgradeManager
    {
        enum CollectionModes
        {
            CustomScrap,
            UniqueScrap,
            NearestValue,
            ChancePerScrap,
            Apparatice,
        }

        enum ChancePerScrapModes
        {
            Random,
            Cheapest,
            LowestLevel,
        }

        static Dictionary<CustomTerminalNode, string[]> scrapToCollectionUpgrade = new Dictionary<CustomTerminalNode, string[]>();
        static CollectionModes currentCollectionMode = CollectionModes.Apparatice;
        public static void CheckCollectionScrap(GrabbableObject scrapItem)
        {
            switch(currentCollectionMode)
            {
                case CollectionModes.Apparatice:
                    {
                        string scrapName = scrapItem.itemProperties.itemName.ToLower();
                        if (scrapName != "apparatus") break;

                        CustomTerminalNode randomNode = PickRandomCollectionUpgrade();
                        LguStore.Instance.HandleUpgradeClientRpc(randomNode.OriginalName, randomNode.Unlocked);
                        break;
                    }
            }
        }

        public static CustomTerminalNode PickRandomCollectionUpgrade()
        {
            return scrapToCollectionUpgrade.Keys.ToArray()[UnityEngine.Random.Range(0, scrapToCollectionUpgrade.Count)];
        }

        public static void AddCollectionUpgrade(CustomTerminalNode node, string[] scrapNames)
        {
            scrapToCollectionUpgrade.Add(node, scrapNames);
        }

    }
}
