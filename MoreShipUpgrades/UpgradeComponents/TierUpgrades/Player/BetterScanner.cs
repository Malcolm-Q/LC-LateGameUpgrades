using MoreShipUpgrades.Compat;
using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc;
using MoreShipUpgrades.Misc.Upgrades;
using MoreShipUpgrades.UI.TerminalNodes;
using MoreShipUpgrades.UpgradeComponents.Interfaces;
using System.Text;

namespace MoreShipUpgrades.UpgradeComponents.TierUpgrades.Player
{
    class BetterScanner : TierUpgrade, IUpgradeWorldBuilding
    {
        public const string UPGRADE_NAME = "Better Scanner";
        internal const string WORLD_BUILDING_TEXT = "\n\nSubscription to 'Stuff Finders' magazine, which comes in three tiers; 'Basic Subscription', 'Pro Package', and 'Diamond Membership'." +
            " The magazine comes every week, but the contents of it are always the same, even the ads. Some departments use unwanted issuances of 'Stuff Finders' as toilet paper.\n\n";
        internal override void Start()
        {
            upgradeName = UPGRADE_NAME;
            overridenUpgradeName = GetConfiguration().BETTER_SCANNER_OVERRIDE_NAME;
            base.Start();
        }

        public override void Load()
        {
            base.Load();
            if (GoodItemScanCompat.Enabled)
            {
                int level = GetUpgradeLevel(UPGRADE_NAME);
                LategameConfiguration config = GetConfiguration();
                GoodItemScanCompat.IncreaseScanDistance((int)config.NODE_DISTANCE_INCREASE);
                GoodItemScanCompat.IncreaseEnemyScanDistance((int)config.NODE_DISTANCE_INCREASE);
                if (level == 2) GoodItemScanCompat.ToggleScanThroughWalls(true);
            }
        }

        public override void Increment()
        {
            base.Increment();
            if (GoodItemScanCompat.Enabled)
            {
                int level = GetUpgradeLevel(UPGRADE_NAME);
                if (level == 2) GoodItemScanCompat.ToggleScanThroughWalls(true);
            }
        }

        public static void AddScannerNodeToValve(ref SteamValveHazard steamValveHazard)
        {
            if (!GetActiveUpgrade(UPGRADE_NAME)) return;
            LguScanNodeProperties.AddGeneralScanNode(objectToAddScanNode: steamValveHazard.gameObject, header: "Bursted Steam Valve", subText: "Fix it to get rid of the steam", minRange: 3);
        }

        public static void RemoveScannerNodeFromValve(ref SteamValveHazard steamValveHazard)
        {
            LguScanNodeProperties.RemoveScanNode(steamValveHazard.gameObject);
        }
        public static string GetBetterScannerInfo(int level, int price)
        {
            LategameConfiguration config = GetConfiguration();
            switch (level)
            {
                case 1: return string.Format(AssetBundleHandler.GetInfoFromJSON("Better Scanner1"), level, price, config.NODE_DISTANCE_INCREASE.Value, config.SHIP_AND_ENTRANCE_DISTANCE_INCREASE.Value);
                case 2: return string.Format(AssetBundleHandler.GetInfoFromJSON("Better Scanner2"), level, price);
                case 3:
                    {
                        string result = string.Format(AssetBundleHandler.GetInfoFromJSON("Better Scanner3"), level, price, config.BETTER_SCANNER_ENEMIES.Value ? " and enemies" : "");
                        result += "hives and scrap command display the location of the most valuable hives and scrap on the map.\n";
                        return result;
                    }
            }
            return "";
        }

        public string GetWorldBuildingText(bool shareStatus = false)
        {
            return WORLD_BUILDING_TEXT;
        }

        public override string GetDisplayInfo(int initialPrice = -1, int maxLevels = -1, int[] incrementalPrices = null)
        {
            StringBuilder stringBuilder = new();
            stringBuilder.Append(GetBetterScannerInfo(1, initialPrice));
            for (int i = 0; i < maxLevels; i++)
                stringBuilder.Append(GetBetterScannerInfo(i + 2, incrementalPrices[i]));
            return stringBuilder.ToString();
        }

        public override bool CanInitializeOnStart => GetConfiguration().BETTER_SCANNER_PRICE.Value <= 0 &&
                GetConfiguration().BETTER_SCANNER_PRICE2.Value <= 0 &&
                GetConfiguration().BETTER_SCANNER_PRICE3.Value <= 0;
        public new static void RegisterUpgrade()
        {
            SetupGenericPerk<BetterScanner>(UPGRADE_NAME);
        }
        public new static (string, string[]) RegisterScrapToUpgrade()
        {
            return (UPGRADE_NAME, GetConfiguration().BETTER_SCANNER_ITEM_PROGRESSION_ITEMS.Value.Split(","));
        }
        public new static CustomTerminalNode RegisterTerminalNode()
        {
            LategameConfiguration configuration = GetConfiguration();

            return UpgradeBus.Instance.SetupMultiplePurchasableTerminalNode(UPGRADE_NAME,
                                                configuration.SHARED_UPGRADES.Value || !configuration.BETTER_SCANNER_INDIVIDUAL.Value,
                                                configuration.BETTER_SCANNER_ENABLED.Value,
                                                configuration.BETTER_SCANNER_PRICE.Value,
                                                [configuration.BETTER_SCANNER_PRICE2.Value, configuration.BETTER_SCANNER_PRICE3.Value],
                                                configuration.OVERRIDE_UPGRADE_NAMES ? configuration.BETTER_SCANNER_OVERRIDE_NAME : ""
                                                );
        }
    }
}
