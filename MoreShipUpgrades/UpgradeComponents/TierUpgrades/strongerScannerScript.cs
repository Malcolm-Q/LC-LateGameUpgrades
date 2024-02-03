using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc;
using UnityEngine;

namespace MoreShipUpgrades.UpgradeComponents.TierUpgrades
{
    internal class strongerScannerScript : BaseUpgrade
    {
        public const string UPGRADE_NAME = "Better Scanner";
        internal const string WORLD_BUILDING_TEXT = "\n\nA serialized Company-Issue Magazine subscription, called 'Stuff Finders'." +
            " Uniquely, {0} must subscribe to each issue of 'Stuff Finders' individually. Each separate subscription promises and delivers a weekly issuance" +
            " of a magazine with the exact same information in it as last time, organized in a different order and with slightly different printing qualities each time." +
            " There are only three 'unique' issues, and each issue only has one or two pieces of actual useful information in it. The rest of the magazine is just ads" +
            " for The Company's other offerings. There is an extra fee for cancelling a subscription of 'Stuff Finders' before terminating your employment." +
            " The useful information always comes in the form of an unlabelled service key or Ship terminal hyperlink.\n\n";
        private static LGULogger logger;
        void Awake()
        {
            logger = new LGULogger(UPGRADE_NAME);
        }
        void Start()
        {
            upgradeName = UPGRADE_NAME;
            DontDestroyOnLoad(gameObject);
            Register();
        }

        public override void Increment()
        {
            UpgradeBus.instance.scanLevel++;
        }

        public override void load()
        {
            base.load();

            UpgradeBus.instance.scannerUpgrade = true;
        }
        public override void Unwind()
        {
            base.Unwind();

            UpgradeBus.instance.scannerUpgrade = false;
            UpgradeBus.instance.scanLevel = 0;
        }
        public override void Register()
        {
            base.Register();
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
