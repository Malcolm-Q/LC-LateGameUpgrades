using GameNetcodeStuff;
using MoreShipUpgrades.Extensions;
using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc;
using MoreShipUpgrades.Misc.Upgrades;
using MoreShipUpgrades.Misc.Util;
using MoreShipUpgrades.UI.TerminalNodes;
using MoreShipUpgrades.UpgradeComponents.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MoreShipUpgrades.UpgradeComponents.TierUpgrades.Ship
{
    internal class FusionMatter: TierUpgrade, IUpgradeWorldBuilding
    {
        internal const string UPGRADE_NAME = "Fusion Matter";
        internal const string DEFAULT_PRICES = "500, 650, 700";
        internal const string WORLD_BUILDING_TEXT = "\n\nBy default, the Ship's onboard Teleporter system is configured not to bring any objects along with it..." +
            " but isn't it kind of strange how you don't arrive naked anytime you use the Teleporter? As it turns out, this limitation is imposed and not inherent." +
            " By requesting & signing a handful of certain liability waivers by their technical names and paying forward a series of fees," +
            " you can expand the capabilities of your Ship's Teleporter. The Teleporter can still only safely transport Company-issued equipment," +
            " since the rough dimensions of these objects are well-documented. The same cannot be said for salvage materials.\n\n";

        public enum ItemCategories
        {
            All,
            Tools,
            Scrap
        }

        public string GetWorldBuildingText(bool shareStatus = false)
        {
            return WORLD_BUILDING_TEXT;
        }

        static private Dictionary<string, int> levels;
        static private Dictionary<ItemCategories, int> categoryLevels;

        internal static void SetupLevels()
        {
            levels = [];
            categoryLevels = [];
            string[] tiersList = GetConfiguration().FusionMatterConfiguration.TierCollection.Value.ToLower().Split(LguConstants.FUSION_MATTER_TIER_DELIMITER);
            for (int level = 0; level < tiersList.Length; ++level)
            {
                foreach (string itemName in tiersList[level].Split(LguConstants.FUSION_MATTER_ITEM_DELIMITER).Select(x => x.Trim().ToLower()))
                {
                    if (levels.ContainsKey(itemName))
                        Plugin.mls.LogWarning($"{itemName} is already registered in the tiers collection of {UPGRADE_NAME}");
                    else
                    {
                        if (System.Enum.TryParse(itemName, ignoreCase: true, out ItemCategories category) && !categoryLevels.ContainsKey(category))
                        {
                            Plugin.mls.LogInfo($"Registering \"{itemName}\" category under level {level} of {UPGRADE_NAME}");
                            categoryLevels[category] = level;
                        }
                        else
                        {
                            Plugin.mls.LogInfo($"Registering {itemName} item under level {level} of {UPGRADE_NAME}");
                            levels[itemName] = level;
                        }
                    }
                }
            }
        }
        public static bool IsItemWithinCategory(GrabbableObject grabbableObject, ItemCategories category)
        {
            switch(category)
            {
                case ItemCategories.All: return true;
                case ItemCategories.Tools: return !grabbableObject.itemProperties.isScrap;
                case ItemCategories.Scrap: return grabbableObject.itemProperties.isScrap;
            }
            return false;
        }

        public static bool CanHoldItem(GrabbableObject grabbableObject, PlayerControllerB player)
        {
            if (grabbableObject == null || !player.IsTeleporting() || player.isPlayerDead) return false;
            bool result = false;
            foreach (KeyValuePair<ItemCategories,int> category in categoryLevels)
            {
                result |= IsItemWithinCategory(grabbableObject, category.Key) && GetUpgradeLevel(UPGRADE_NAME) >= category.Value;
            }
            if (result) return result;

            string itemName = grabbableObject.itemProperties.itemName.Trim().ToLower();
            if (levels.TryGetValue(itemName, out int level))
            {
                Plugin.mls.LogDebug($"{itemName} can be safeguarded by Fusion Matter at level {level+1}");
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
            overridenUpgradeName = GetConfiguration().FusionMatterConfiguration.OverrideName;
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
            IEnumerable<string> levelKeys = levels.Where(item => item.Value == level).Select(item => item.Key);
            IEnumerable<string> categoryLevelKeys = categoryLevels.Where(category => category.Value == level).Select(category => category.Key.ToString());
            string itemList = string.Join(", ", levelKeys.Concat(categoryLevelKeys));

            return string.Format("LVL {0} - ${1} - Allows safekeeping the following items when teleporting: {2}\n",
                level + 1, price, itemList);
        }

        public override bool CanInitializeOnStart
        {
            get
            {
                string[] prices = GetConfiguration().FusionMatterConfiguration.Prices.Value.Split(',');
                return prices.Length == 0 || (prices.Length == 1 && (prices[0].Length == 0 || prices[0] == "0"));
            }
        }

        public new static (string, string[]) RegisterScrapToUpgrade()
        {
            return (UPGRADE_NAME, GetConfiguration().FusionMatterConfiguration.ItemProgressionItems.Value.Split(","));
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
            SetupLevels();
            return UpgradeBus.Instance.SetupMultiplePurchaseableTerminalNode(UPGRADE_NAME, UpgradeBus.Instance.PluginConfiguration.FusionMatterConfiguration, Plugin.networkPrefabs[UPGRADE_NAME]);
        }
    }
}
