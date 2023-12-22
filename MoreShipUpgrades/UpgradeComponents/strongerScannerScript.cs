using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc;
using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine;

namespace MoreShipUpgrades.UpgradeComponents
{
    internal class strongerScannerScript : BaseUpgrade
    {

        void Start()
        {
            DontDestroyOnLoad(gameObject);
            Register();
        }

        public override void Increment()
        {
            UpgradeBus.instance.scanLevel++;
        }

        public override void load()
        {
            UpgradeBus.instance.scannerUpgrade = true;
            HUDManager.Instance.chatText.text += "\n<color=#FF0000>Better Scanner is active!</color>";
        }
        public override void Unwind()
        {
            UpgradeBus.instance.scannerUpgrade = false;
            UpgradeBus.instance.scanLevel = 0;
            HUDManager.Instance.chatText.text += "\n<color=#FF0000>Better Scanner has been disabled.</color>";
        }
        public override void Register()
        {
            if(!UpgradeBus.instance.UpgradeObjects.ContainsKey("Better Scanner")) { UpgradeBus.instance.UpgradeObjects.Add("Better Scanner", gameObject); }
        }

        public static string GetBetterScannerInfo(int level, int price)
        {
            switch (level)
            {
                case 1: return string.Format(AssetBundleHandler.GetInfoFromJSON("Better Scanner1"), level, price, UpgradeBus.instance.cfg.NODE_DISTANCE_INCREASE, UpgradeBus.instance.cfg.SHIP_AND_ENTRANCE_DISTANCE_INCREASE);
                case 2: return string.Format(AssetBundleHandler.GetInfoFromJSON("Better Scanner2"), level, price);
                case 3:
                    {
                        string result = string.Format(AssetBundleHandler.GetInfoFromJSON("Better Scanner3"), level, price, UpgradeBus.instance.cfg.BETTER_SCANNER_ENEMIES ? " and enemies" : "");
                        result += "hives and scrap command display the location of the most valuable hives and scrap on the map.\n";
                        return result;
                    }
            }
            return "";
        }
    }
}
