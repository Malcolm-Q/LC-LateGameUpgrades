using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc;
using System.Collections.Generic;
using System.Linq;

namespace MoreShipUpgrades.UpgradeComponents
{
    internal class hunterScript : BaseUpgrade
    {
        public static string UPGRADE_NAME = "Hunter";

        static string[] lvl1 = new string[] { "Hoarding bug", "Centipede" };
        static string[] lvl2 = new string[] { "Bunker Spider", "Hoarding bug", "Centipede", "Baboon Hawk" };
        static string[] lvl3 = new string[] { "Bunker Spider", "Hoarding bug", "Centipede", "Baboon Hawk", "Flowerman", "Crawler", "MouthDog" };
        static Dictionary<string, string> monsterNames = new Dictionary<string, string>()
            {
            { "Hoarding bug", "Hoarding Bug" },
            { "Centipede", "Snare Flea" },
            { "Bunker Spider", "Bunker Spider" },
            { "Baboon Hawk", "Baboon Hawk" },
            { "Flowerman", "Bracken" },
            { "Crawler", "Half/Thumper" },
            { "MouthDog", "Eyeless Dog" },
            };
        static public Dictionary<int, string[]> tiers = new Dictionary<int, string[]>
        {
            {0,  lvl1 },
            {1, lvl2 },
            {2, lvl3 },
        };
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

        public override void load()
        {
            base.load();

            UpgradeBus.instance.hunter = true;
        }
        public override void Unwind()
        {
            base.Unwind();

            UpgradeBus.instance.hunter = false;
            UpgradeBus.instance.huntLevel = 0;
        }
        public override void Register()
        {
            base.Register();
        }

        public static string GetHunterInfo(int level, int price)
        {
            string enems;
            if(level != 1) enems = string.Join(", ", tiers[level-1].Except(tiers[level - 2]).ToArray());
            else enems = string.Join(", ", tiers[level-1]);
            string result = "";
            foreach (string monsterTypeName in enems.Split(", "))
            {
                result += monsterNames[monsterTypeName] + ", ";
            }
            result = result.Substring(0, result.Length - 2);
            result += "\n";
            return string.Format(AssetBundleHandler.GetInfoFromJSON("Hunter"), level, price, result);
        }
    }
}
