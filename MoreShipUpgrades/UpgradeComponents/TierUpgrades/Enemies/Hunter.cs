using MoreShipUpgrades.API;
using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc;
using MoreShipUpgrades.Misc.TerminalNodes;
using MoreShipUpgrades.Misc.Upgrades;
using MoreShipUpgrades.UpgradeComponents.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MoreShipUpgrades.UpgradeComponents.TierUpgrades.Enemies
{
    class Hunter : TierUpgrade, IUpgradeWorldBuilding
    {
        private static readonly LguLogger logger = new(UPGRADE_NAME);
        internal static Hunter Instance;

        public const string UPGRADE_NAME = "Hunter";
        public const string PRICES_DEFAULT = "500,600,700";
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
            { "dog", "Eyeless Dog" },
            { "tulip snake", "Tulip Snake" },
            { "slowersnake", "Tulip Snake" },
            { "flower snake", "Tulip Snake" },
            { "snake", "Tulip Snake" },
            { "forest giant", "Forest Giant" },
            { "forest keeper", "Forest Giant" },
            { "forestgiant", "Forest Giant" },
            { "forestkeeper", "Forest Giant" },
            { "manticoil", "Manticoil" },
            { "manti coil", "Manticoil" },
            { "bird", "Manticoil" },
            { "puffer", "Spore Lizard" },
            { "sporelizard", "Spore Lizard" },
            { "spore lizard", "Spore Lizard" },
            { "lizard", "Spore Lizard" },
            { "fox", "Kidnapper Fox" },
            { "kidnapper fox", "Kidnapper Fox" },
            { "kidnapperfox", "Kidnapper Fox" },
            { "bush wolf", "Kidnapper Fox" },
            { "bushwolf", "Kidnapper Fox" },
            { "wolf", "Kidnapper Fox" },
            
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
            for (int level = 0; level < tiersList.Length; ++level)
            {
                foreach (string monster in tiersList[level].Split(',').Select(x => x.Trim().ToLower()))
                {
                    if (monsterNames.TryGetValue(monster, out string fullName))
                    {
                        if (levels.ContainsKey(fullName))
                        {
                            logger.LogError($"{fullName} appears twice in samples config! Appearing now as {monster}");
                        }
                        else
                        {
                            logger.LogInfo($"{fullName} set to be harvestable at level {level + 1}");
                            levels[fullName] = level;
                        }
                    }
                    else
                    {
                        logger.LogError($"Unrecognized enemy name: {monster}");
                    }
                }
            }
            foreach (string moddedMonster in HunterSamples.moddedLevels.Keys)
            {
                levels[moddedMonster] = HunterSamples.moddedLevels[moddedMonster];
                monsterNames[moddedMonster.ToLower()] = moddedMonster;
            }
        }

        public static bool CanHarvest(string shortName)
        {
            if (!monsterNames.TryGetValue(shortName.ToLower(), out string monsterName))
            {
                logger.LogDebug($"{shortName} is not harvestable");
                return false;
            }

            if (levels.TryGetValue(monsterName, out int harvestLevel))
            {
                logger.LogDebug($"{monsterName} can be harvested at level {harvestLevel + 1}");
                return harvestLevel <= GetUpgradeLevel(UPGRADE_NAME);
            }

            logger.LogDebug($"{monsterName} cannot be harvested at any level");
            return false;
        }
        void Awake()
        {
            upgradeName = UPGRADE_NAME;
            overridenUpgradeName = UpgradeBus.Instance.PluginConfiguration.HUNTER_OVERRIDE_NAME;
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
            StringBuilder sb = new();
            sb.Append(GetHunterInfo(0, initialPrice));
            for (int i = 0; i < maxLevels; i++)
                sb.Append(GetHunterInfo(i + 1, incrementalPrices[i]));
            return sb.ToString();
        }
        public override bool CanInitializeOnStart
        {
            get
            {
                string[] prices = UpgradeBus.Instance.PluginConfiguration.HUNTER_UPGRADE_PRICES.Value.Split(',');
                bool free = UpgradeBus.Instance.PluginConfiguration.HUNTER_PRICE.Value <= 0 && prices.Length == 1 && (prices[0].Length == 0 || prices[0] == "0");
                return free;
            }
        }

        public new static (string, string[]) RegisterScrapToUpgrade()
        {
            return (UPGRADE_NAME, UpgradeBus.Instance.PluginConfiguration.HUNTER_ITEM_PROGRESSION_ITEMS.Value.Split(","));
        }
        public new static void RegisterUpgrade()
        {
            SetupGenericPerk<Hunter>(UPGRADE_NAME);
        }
        public new static CustomTerminalNode RegisterTerminalNode()
        {
            LategameConfiguration configuration = UpgradeBus.Instance.PluginConfiguration;

            SetupLevels();
            return UpgradeBus.Instance.SetupMultiplePurchasableTerminalNode(UPGRADE_NAME,
                                                shareStatus: true,
                                                configuration.HUNTER_ENABLED.Value,
                                                configuration.HUNTER_PRICE.Value,
                                                UpgradeBus.ParseUpgradePrices(configuration.HUNTER_UPGRADE_PRICES.Value),
                                                configuration.OVERRIDE_UPGRADE_NAMES ? configuration.HUNTER_OVERRIDE_NAME : "");
        }
    }
}
