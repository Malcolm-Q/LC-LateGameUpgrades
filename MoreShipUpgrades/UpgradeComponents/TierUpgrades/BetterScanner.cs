using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc;
using MoreShipUpgrades.Misc.Upgrades;
using MoreShipUpgrades.UpgradeComponents.Interfaces;
using UnityEngine;

namespace MoreShipUpgrades.UpgradeComponents.TierUpgrades
{
    class BetterScanner : TierUpgrade, IUpgradeWorldBuilding
    {
        public const string UPGRADE_NAME = "Better Scanner";
        internal const string WORLD_BUILDING_TEXT = "\n\nA serialized Company-Issue Magazine subscription, called 'Stuff Finders'." +
            " Uniquely, {0} must subscribe to each issue of 'Stuff Finders' individually. Each separate subscription promises and delivers a weekly issuance" +
            " of a magazine with the exact same information in it as last time, organized in a different order and with slightly different printing qualities each time." +
            " There are only three 'unique' issues, and each issue only has one or two pieces of actual useful information in it. The rest of the magazine is just ads" +
            " for The Company's other offerings. There is an extra fee for cancelling a subscription of 'Stuff Finders' before terminating your employment." +
            " The useful information always comes in the form of an unlabelled service key or Ship terminal hyperlink.\n\n";
        private static LGULogger logger;
        internal override void Start()
        {
            upgradeName = UPGRADE_NAME;
            logger = new LGULogger(UPGRADE_NAME);
            base.Start();
        }

        public override void Increment()
        {
            UpgradeBus.instance.scanLevel++;
        }

        public override void Load()
        {
            base.Load();

            UpgradeBus.instance.scannerUpgrade = true;
        }
        public override void Unwind()
        {
            base.Unwind();

            UpgradeBus.instance.scannerUpgrade = false;
            UpgradeBus.instance.scanLevel = 0;
        }

        public static void AddScannerNodeToValve(ref SteamValveHazard steamValveHazard)
        {
            if (!UpgradeBus.instance.scannerUpgrade) return;
            logger.LogDebug("Inserting a Scan Node on a broken steam valve...");
            GameObject ScanNodeObject = Instantiate(GameObject.Find("ScanNode"), steamValveHazard.transform.position, Quaternion.Euler(Vector3.zero), steamValveHazard.transform);
            ScanNodeProperties node = ScanNodeObject.GetComponent<ScanNodeProperties>();
            node.headerText = "Bursted Steam Valve";
            node.subText = "Fix it to get rid of the steam";
            node.nodeType = 0;
            node.creatureScanID = -1;
        }

        public static void RemoveScannerNodeFromValve(ref SteamValveHazard steamValveHazard)
        {
            logger.LogDebug("Removing the Scan Node from a fixed steam valve...");
            Destroy(steamValveHazard.gameObject.GetComponentInChildren<ScanNodeProperties>());
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

        public string GetWorldBuildingText(bool shareStatus = false)
        {
            return string.Format(WORLD_BUILDING_TEXT, shareStatus ? "a department" : "one");
        }

        public override string GetDisplayInfo(int initialPrice = -1, int maxLevels = -1, int[] incrementalPrices = null)
        {
            string info = GetBetterScannerInfo(1, initialPrice);
            for (int i = 0; i < maxLevels; i++)
                info += GetBetterScannerInfo(i + 2, incrementalPrices[i]);
            return info;
        }
    }
}
