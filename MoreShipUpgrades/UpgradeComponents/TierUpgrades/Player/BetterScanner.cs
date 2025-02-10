using MoreShipUpgrades.Compat;
using MoreShipUpgrades.Configuration;
using MoreShipUpgrades.Configuration.Custom;
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
            overridenUpgradeName = GetConfiguration().BetterScannerUpgradeConfiguration.OverrideName;
            base.Start();
        }

        public override void Load()
        {
            base.Load();
            if (GoodItemScanCompat.Enabled)
            {
                BetterScannerUpgradeConfiguration config = GetConfiguration().BetterScannerUpgradeConfiguration;
                GoodItemScanCompat.IncreaseScanDistance((int)config.NodeRangeIncrease);
                GoodItemScanCompat.IncreaseEnemyScanDistance((int)config.NodeRangeIncrease);
                int level = GetUpgradeLevel(UPGRADE_NAME);
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
            BetterScannerUpgradeConfiguration config = GetConfiguration().BetterScannerUpgradeConfiguration;
            switch (level)
            {
                case 1: return string.Format(AssetBundleHandler.GetInfoFromJSON("Better Scanner1"), level, GetUpgradePrice(price, config.PurchaseMode), config.NodeRangeIncrease.Value, config.OutsideNodesRangeIncrease.Value);
                case 2: return string.Format(AssetBundleHandler.GetInfoFromJSON("Better Scanner2"), level, GetUpgradePrice(price, config.PurchaseMode));
                case 3:
                    {
                        string result = string.Format(AssetBundleHandler.GetInfoFromJSON("Better Scanner3"), level, GetUpgradePrice(price, config.PurchaseMode), config.SeeEnemiesThroughWalls.Value ? " and enemies" : "");
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

        public override bool CanInitializeOnStart => GetConfiguration().BetterScannerUpgradeConfiguration.Price.Value <= 0 &&
                GetConfiguration().BetterScannerUpgradeConfiguration.SecondPrice.Value <= 0 &&
                GetConfiguration().BetterScannerUpgradeConfiguration.ThirdPrice.Value <= 0;
        public new static void RegisterUpgrade()
        {
            SetupGenericPerk<BetterScanner>(UPGRADE_NAME);
        }
        public new static (string, string[]) RegisterScrapToUpgrade()
        {
            return (UPGRADE_NAME, GetConfiguration().BetterScannerUpgradeConfiguration.ItemProgressionItems.Value.Split(","));
        }
        public new static CustomTerminalNode RegisterTerminalNode()
        {
            BetterScannerUpgradeConfiguration configuration = GetConfiguration().BetterScannerUpgradeConfiguration;

            return UpgradeBus.Instance.SetupMultiplePurchasableTerminalNode(UPGRADE_NAME,
                                                GetConfiguration().SHARED_UPGRADES.Value || !configuration.Individual.Value,
                                                configuration.Enabled.Value,
                                                configuration.Price.Value,
                                                [configuration.SecondPrice.Value, configuration.ThirdPrice.Value],
                                                GetConfiguration().OVERRIDE_UPGRADE_NAMES ? configuration.OverrideName : "",
                                                alternateCurrency: GetConfiguration().ALTERNATIVE_CURRENCY_ENABLED,
                                                purchaseMode: configuration.PurchaseMode
                                                );
        }
    }
}
