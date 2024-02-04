using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc;
using MoreShipUpgrades.Misc.Upgrades;
using UnityEngine;

namespace MoreShipUpgrades.UpgradeComponents.TierUpgrades
{
    class BetterScanner : TierUpgrade
    {
        public static string UPGRADE_NAME = "Better Scanner";
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
    }
}
