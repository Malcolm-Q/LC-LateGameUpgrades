using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc;
using System.Collections.Generic;

namespace MoreShipUpgrades.UpgradeComponents
{
    internal class hunterScript : BaseUpgrade
    {
        static string[] lvl1 = new string[] { "hoarding bug", "snare flea" };
        static string[] lvl2 = new string[] { "bunker spider","hoarding bug", "snare flea","baboon hawk" };
        static string[] lvl3 = new string[] { "bunker spider","hoarding bug","snare flea","baboon hawk","bracken","thumper","eyeless dog" };
        static public Dictionary<int, string[]> tiers = new Dictionary<int, string[]>
        {
            {0,  lvl1 },
            {1, lvl2 },
            {2, lvl3 },
        };
        void Start()
        {
            DontDestroyOnLoad(gameObject);
            Register();
        }

        public override void Increment()
        {
            UpgradeBus.instance.huntLevel++;
        }

        public override void load()
        {
            UpgradeBus.instance.hunter = true;
            HUDManager.Instance.chatText.text += "\n<color=#FF0000>Hunter is active!</color>";
        }
        public override void Unwind()
        {
            UpgradeBus.instance.hunter = false;
            UpgradeBus.instance.huntLevel = 0;
            HUDManager.Instance.chatText.text += "\n<color=#FF0000>Hunter has been disabled.</color>";
        }
        public override void Register()
        {
            if(!UpgradeBus.instance.UpgradeObjects.ContainsKey("Hunter")) { UpgradeBus.instance.UpgradeObjects.Add("Hunter", gameObject); }
        }
    }
}
