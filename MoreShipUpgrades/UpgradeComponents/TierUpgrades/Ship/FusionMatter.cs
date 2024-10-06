using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc;
using MoreShipUpgrades.Misc.Upgrades;
using MoreShipUpgrades.Misc.Util;
using MoreShipUpgrades.UI.TerminalNodes;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MoreShipUpgrades.UpgradeComponents.TierUpgrades.Ship
{
    internal class FusionMatter: TierUpgrade
    {
        internal const string UPGRADE_NAME = "Fusion Matter";
        internal const string DEFAULT_PRICES = "650, 700";

        static private Dictionary<string, int> levels;

        internal static void SetupLevels()
        {
            levels = [];
            string[] tiersList = GetConfiguration().FUSION_MATTER_ITEM_TIERS.Value.ToLower().Split(LguConstants.FUSION_MATTER_TIER_DELIMITER);
            for (int level = 0; level < tiersList.Length; ++level)
            {
                foreach (string itemName in tiersList[level].Split(LguConstants.FUSION_MATTER_ITEM_DELIMITER).Select(x => x.Trim().ToLower()))
                {
                    if (levels.ContainsKey(itemName))
                        Plugin.mls.LogWarning($"{itemName} is already registered in the tiers collection of {UPGRADE_NAME}");
                    else
                    {
                        Plugin.mls.LogInfo($"Registering {itemName} item under level {level} of {UPGRADE_NAME}");
                        levels[itemName] = level;
                    }
                }
            }
        }
        public static bool CanHoldItem(GrabbableObject grabbableObject)
        {
            if (grabbableObject == null) return false;
            string itemName = grabbableObject.itemProperties.itemName.Trim().ToLower();
            if (levels.TryGetValue(itemName, out int level))
            {
                Plugin.mls.LogDebug($"{itemName} can be safeguarded by Fusion Matter at level {level+1}");
                Plugin.mls.LogDebug(GetUpgradeLevel(UPGRADE_NAME) >= level);
                return GetUpgradeLevel(UPGRADE_NAME) >= level;
            }

            Plugin.mls.LogDebug($"{itemName} from item properties was not found, checking other sources...");
            ScanNodeProperties node = grabbableObject.GetComponentInChildren<ScanNodeProperties>();
            if (node == null)
            {
                Plugin.mls.LogDebug($"{itemName} does not contain a scan node, ran out of sources...");
                return false;
            }
            itemName = node.headerText.Trim().ToLower();
            if (levels.TryGetValue(itemName, out level))
            {
                Plugin.mls.LogDebug($"{itemName} can be safeguarded by Fusion Matter at level {level + 1}");
                return GetUpgradeLevel(UPGRADE_NAME) >= level;
            }
            Plugin.mls.LogDebug($"{itemName} from scan node was not found, ran out of sources...");
            return false;
        }
        void Awake()
        {
            upgradeName = UPGRADE_NAME;
            overridenUpgradeName = GetConfiguration().FUSION_MATTER_OVERRIDE_NAME;
        }
        public override string GetDisplayInfo(int initialPrice = -1, int maxLevels = -1, int[] incrementalPrices = null)
        {
            StringBuilder sb = new();
            sb.Append(GetFusionMatterInfo(0, initialPrice));
            for (int i = 0; i < maxLevels; i++)
                sb.Append(GetFusionMatterInfo(i + 1, incrementalPrices[i]));
            return sb.ToString();
        }

        public static string GetFusionMatterInfo(int level, int price)
        {
            string itemList = string.Join(", ",
                levels.Where(item => item.Value == level)
                .Select(item => item.Key));

            return string.Format("LVL {0} - ${1} - Allows safekeeping the following items when teleporting: {2}\n",
                level + 1, price, itemList);
        }

        public override bool CanInitializeOnStart
        {
            get
            {
                LategameConfiguration config = GetConfiguration();
                string[] prices = config.FUSION_MATTER_PRICES.Value.Split(',');
                return config.FUSION_MATTER_PRICE.Value <= 0 && prices.Length == 1 && (prices[0].Length == 0 || prices[0] == "0");
            }
        }

        public new static (string, string[]) RegisterScrapToUpgrade()
        {
            return (UPGRADE_NAME, GetConfiguration().FUSION_MATTER_ITEM_PROGRESSION_ITEMS.Value.Split(","));
        }
        public new static void RegisterUpgrade()
        {
            GameObject prefab = LethalLib.Modules.NetworkPrefabs.CreateNetworkPrefab(UPGRADE_NAME);
            prefab.AddComponent<FusionMatter>();
            LethalLib.Modules.NetworkPrefabs.RegisterNetworkPrefab(prefab);
            Plugin.networkPrefabs[UPGRADE_NAME] = prefab;
        }
        public new static CustomTerminalNode RegisterTerminalNode()
        {
            LategameConfiguration configuration = GetConfiguration();

            SetupLevels();
            return UpgradeBus.Instance.SetupMultiplePurchasableTerminalNode(UPGRADE_NAME,
                                                shareStatus: true,
                                                configuration.FUSION_MATTER_ENABLED,
                                                configuration.FUSION_MATTER_PRICE,
                                                UpgradeBus.ParseUpgradePrices(configuration.FUSION_MATTER_PRICES),
                                                configuration.OVERRIDE_UPGRADE_NAMES ? configuration.FUSION_MATTER_OVERRIDE_NAME : "",
                                                Plugin.networkPrefabs[UPGRADE_NAME]);
        }
    }
}
