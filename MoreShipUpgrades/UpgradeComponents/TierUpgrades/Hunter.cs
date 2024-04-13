using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc;
using MoreShipUpgrades.Misc.Upgrades;
using MoreShipUpgrades.UpgradeComponents.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MoreShipUpgrades.UpgradeComponents.TierUpgrades
{
    class Hunter : TierUpgrade, IUpgradeWorldBuilding
    {
        private static readonly LguLogger logger = new(UPGRADE_NAME);
        internal static Hunter Instance;

        public const string UPGRADE_NAME = "Hunter";
        internal const string WORLD_BUILDING_TEXT = "\n\nOn-the-job training program that teaches your crew how to properly collect lab-ready samples of blood," +
            " skin, and organ tissue from entities found within the facility. These samples are valuable to The Company. Used to be a part of the standard onboarding procedure," +
            " but was made opt-in only in 2005 to cut onboarding costs.\n\n";

        static readonly Dictionary<string, string> monsterNames = new()
            {
            { "hoarding", "Hoarding Bug" },
            { "hoarding bug", "Hoarding Bug" },
            { "snare", "Snare Flea" },
            { "flea", "Snare Flea" },
            { "snare flea", "Snare Flea" },
            { "centipede", "Snare Flea" },
            { "bunker spider", "Bunker Spider" },
            { "bunker", "Bunker Spider" },
            { "bunk", "Bunker Spider" },
            { "spider", "Bunker Spider" },
            { "baboon hawk", "Baboon Hawk" },
            { "baboon", "Baboon Hawk" },
            { "hawk", "Baboon Hawk" },
            { "flowerman", "Bracken" },
            { "bracken", "Bracken" },
            { "crawler", "Half/Thumper" },
            { "half", "Half/Thumper" },
            { "thumper", "Half/Thumper" },
            { "mouthdog", "Eyeless Dog" },
            { "eyeless dog", "Eyeless Dog" },
            { "eyeless", "Eyeless Dog" },
            { "dog", "Eyeless Dog" }
            };

        /**
         * A mapping from monster names to the Hunter level required to harvest them.
         * This mapping uses the value from the above list, translating from any key in the list
         */
        static private Dictionary<string, int> levels;

        public static void SetupLevels()
        {
            levels = [];
            string[] tiersList = UpgradeBus.Instance.PluginConfiguration.HUNTER_SAMPLE_TIERS.Value.ToLower().Split('-');
            for (int level = 0; level < tiersList.Length;  ++level)
            {
                foreach (string monster in tiersList[level].Split(',').Select(x => x.Trim().ToLower()))
                {
                    if (monsterNames.TryGetValue(monster, out string fullName))
                    {
                        if (levels.ContainsKey(fullName))
                        {
                            logger.LogError("{fullName} appears twice is samples config! Appearing now as {monster}");
                        }
                        else
                        {
                            logger.LogInfo($"{fullName} set to be harvestable at level {level + 1}");
                            levels[fullName] = level;
                        }
                    }
                    else
                    {
                        logger.LogError("Unrecognized enemy name: {monster}");
                    }
                }
            }
        }

        public static bool CanHarvest(string shortName)
        {
            if (!monsterNames.TryGetValue(shortName.ToLower(), out string monsterName))
            {
                logger.LogDebug($"{shortName} is not harvestable");
                return false;
            }

            if (levels.TryGetValue(monsterName, out int harvestLevel)) {
                logger.LogDebug($"{monsterName} can be harvested at level {harvestLevel+1}");
                return harvestLevel <= BaseUpgrade.GetUpgradeLevel(UPGRADE_NAME);
            }

            logger.LogDebug($"{monsterName} cannot be harvested at any level");
            return false;

        }

        void Awake()
        {
            upgradeName = UpgradeBus.Instance.PluginConfiguration.OVERRIDE_UPGRADE_NAMES ? UpgradeBus.Instance.PluginConfiguration.HUNTER_OVERRIDE_NAME : UPGRADE_NAME;
            Instance = this;
        }

        public static string GetHunterInfo(int level, int price)
        {
            string monsterList = string.Join(", ",
                levels.Where(item => item.Value == level)
                .Select(item => item.Key));

            return string.Format(AssetBundleHandler.GetInfoFromJSON(UPGRADE_NAME),
                level + 1, price, monsterList);
        }

        public string GetWorldBuildingText(bool shareStatus = false)
        {
            return WORLD_BUILDING_TEXT;
        }

        public override string GetDisplayInfo(int initialPrice = -1, int maxLevels = -1, int[] incrementalPrices = null)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(GetHunterInfo(0, initialPrice));
            for (int i = 0; i < maxLevels; i++)
                sb.Append(GetHunterInfo(i + 1, incrementalPrices[i]));
            return sb.ToString();
        }
        internal override bool CanInitializeOnStart()
        {
            string[] prices = UpgradeBus.Instance.PluginConfiguration.HUNTER_UPGRADE_PRICES.Value.Split(',');
            bool free = UpgradeBus.Instance.PluginConfiguration.HUNTER_PRICE.Value <= 0 && prices.Length == 1 && (prices[0] == "" || prices[0] == "0");
            return free;
        }
    }
}
