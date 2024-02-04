using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc;
using MoreShipUpgrades.Misc.Upgrades;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MoreShipUpgrades.UpgradeComponents.TierUpgrades
{
    class Hunter : TierUpgrade
    {
        public static string UPGRADE_NAME = "Hunter";
        private static LGULogger logger = new LGULogger(UPGRADE_NAME);
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
        static public Dictionary<int, string[]> tiers;
        public static void SetupTierList()
        {
            logger = new LGULogger(UPGRADE_NAME);
            tiers = new Dictionary<int, string[]>();
            string[] tiersList = UpgradeBus.instance.cfg.HUNTER_SAMPLE_TIERS.ToLower().Split('-');
            tiers[0] = tiersList[0].Split(",").Select(x => x.Trim()).ToArray();
            for (int i = 1; i < tiersList.Length; i++)
            {
                tiers[i] = tiers[i - 1].Concat(tiersList[i].Split(",").Select(x => x.Trim())).ToArray();
            }
        }
        void Start()
        {
            upgradeName = UPGRADE_NAME;
            DontDestroyOnLoad(gameObject);
            Register();
        }

        public override void Increment()
        {
            UpgradeBus.instance.huntLevel++;
        }

        public override void Load()
        {
            base.Load();

            UpgradeBus.instance.hunter = true;
        }
        public override void Unwind()
        {
            base.Unwind();

            UpgradeBus.instance.hunter = false;
            UpgradeBus.instance.huntLevel = 0;
        }

        public static string GetHunterInfo(int level, int price)
        {
            string enems;
            if (level != 1) enems = string.Join(", ", tiers[level - 1].Except(tiers[level - 2]).ToArray());
            else enems = string.Join(", ", tiers[level - 1]);
            string result = "";
            foreach (string monsterTypeName in enems.Split(", "))
            {
                logger.LogDebug(monsterTypeName.Trim().ToLower());
                logger.LogDebug(monsterNames[monsterTypeName.Trim().ToLower()]);
                result += monsterNames[monsterTypeName.Trim().ToLower()] + ", ";
            }
            result = result.Substring(0, result.Length - 2);
            result += "\n";
            return string.Format(AssetBundleHandler.GetInfoFromJSON(UPGRADE_NAME), level, price, result);
        }
    }
}
