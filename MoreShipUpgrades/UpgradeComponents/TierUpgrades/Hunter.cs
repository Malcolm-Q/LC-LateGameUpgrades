using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc;
using MoreShipUpgrades.Misc.Upgrades;
using MoreShipUpgrades.UpgradeComponents.Interfaces;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MoreShipUpgrades.UpgradeComponents.TierUpgrades
{
    class Hunter : TierUpgrade, IUpgradeWorldBuilding
    {
        private static LguLogger logger = new LguLogger(UPGRADE_NAME);
        internal static Hunter Instance;

        public const string UPGRADE_NAME = "Hunter";
        internal const string WORLD_BUILDING_TEXT = "\n\nOn-the-job training program that teaches your crew how to properly collect lab-ready samples of blood," +
            " skin, and organ tissue from entities found within the facility. These samples are valuable to The Company. Used to be a part of the standard onboarding procedure," +
            " but was made opt-in only in 2005 to cut onboarding costs.\n\n";
        static Dictionary<string, string> monsterNames = new Dictionary<string, string>()
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
            { "dog", "Eyeless Dog" },
            };

        /**
         * A mapping from monster names to the Hunter level required to harvest them
         */
        static private Dictionary<string, int> levels;

        public static void SetupLevels()
        {
            logger = new LguLogger(UPGRADE_NAME);
            levels = new Dictionary<string, int>();
            string[] tiersList = UpgradeBus.Instance.PluginConfiguration.HUNTER_SAMPLE_TIERS.Value.ToLower().Split('-');
            for (int level = 0; level < tiersList.Length;  ++level)
            {
                foreach (string monster in tiersList[level].Split(',').Select(x => x.Trim()))
                {
                    logger.LogInfo($"{monster} set to level {level}");
                    levels[monster] = level;
                }
            }
        }

        public static bool CanHarvest(string monsterName)
        {
            int harvestLevel;
            if (levels.TryGetValue(monsterName.ToLower(), out harvestLevel)) {
                return harvestLevel <= BaseUpgrade.GetUpgradeLevel(UPGRADE_NAME);
            }
            return false;

        }

        void Awake()
        {
            upgradeName = UPGRADE_NAME;
            Instance = this;
        }



        public static string GetHunterInfo(int level, int price)
        {
            string monsterList = string.Join(", ",
                levels.Where(item => item.Value == level)
                .Select(item => monsterNames[item.Key.Trim().ToLower()]));

            return string.Format(AssetBundleHandler.GetInfoFromJSON(UPGRADE_NAME),
                level + 1, price, monsterList);
        }

        public string GetWorldBuildingText(bool shareStatus = false)
        {
            return WORLD_BUILDING_TEXT;
        }

        public override string GetDisplayInfo(int initialPrice = -1, int maxLevels = -1, int[] incrementalPrices = null)
        {
            string info = GetHunterInfo(0, initialPrice);
            for (int i = 0; i < maxLevels; i++)
                info += GetHunterInfo(i + 1, incrementalPrices[i]);
            return info;
        }
    }
}
