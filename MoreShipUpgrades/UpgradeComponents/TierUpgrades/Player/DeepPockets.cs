using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc;
using MoreShipUpgrades.Misc.TerminalNodes;
using MoreShipUpgrades.Misc.Upgrades;
using MoreShipUpgrades.Misc.Util;

namespace MoreShipUpgrades.UpgradeComponents.TierUpgrades.Player
{
    internal class DeepPockets : TierUpgrade
    {
        internal const string UPGRADE_NAME = "Deeper Pockets";
        internal const string DEFAULT_PRICES = "750";

        internal override void Start()
        {
            upgradeName = UPGRADE_NAME;
            overridenUpgradeName = UpgradeBus.Instance.PluginConfiguration.DEEPER_POCKETS_OVERRIDE_NAME;
            base.Start();
        }
        public override string GetDisplayInfo(int initialPrice = -1, int maxLevels = -1, int[] incrementalPrices = null)
        {
            System.Func<int, float> infoFunction = level => UpgradeBus.Instance.PluginConfiguration.DEEPER_POCKETS_INITIAL_TWO_HANDED_ITEMS.Value + level * UpgradeBus.Instance.PluginConfiguration.DEEPER_POCKETS_INCREMENTAL_TWO_HANDED_ITEMS.Value;
            string infoFormat = "LVL {0} - ${1} - Increases the two handed carry capacity of the player by {2}\n";
            return Tools.GenerateInfoForUpgrade(infoFormat, initialPrice, incrementalPrices, infoFunction);
        }

        public override bool CanInitializeOnStart
        {
            get
            {
                string[] prices = UpgradeBus.Instance.PluginConfiguration.DEEPER_POCKETS_PRICES.Value.Split(',');
                bool free = UpgradeBus.Instance.PluginConfiguration.DEEPER_POCKETS_PRICE.Value <= 0 && prices.Length == 1 && (prices[0] == "" || prices[0] == "0");
                return free;
            }
        }
        public new static (string, string[]) RegisterScrapToUpgrade()
        {
            return (UPGRADE_NAME, UpgradeBus.Instance.PluginConfiguration.DEEPER_POCKETS_ITEM_PROGRESSION_ITEMS.Value.Split(","));
        }
        public new static void RegisterUpgrade()
        {
            SetupGenericPerk<DeepPockets>(UPGRADE_NAME);
        }
        public new static CustomTerminalNode RegisterTerminalNode()
        {
            LategameConfiguration configuration = UpgradeBus.Instance.PluginConfiguration;

            return UpgradeBus.Instance.SetupMultiplePurchasableTerminalNode(UPGRADE_NAME,
                                                configuration.SHARED_UPGRADES || !configuration.DEEPER_POCKETS_INDIVIDUAL,
                                                configuration.DEEPER_POCKETS_ENABLED,
                                                configuration.DEEPER_POCKETS_PRICE,
                                                UpgradeBus.ParseUpgradePrices(configuration.DEEPER_POCKETS_PRICES),
                                                configuration.OVERRIDE_UPGRADE_NAMES ? configuration.DEEPER_POCKETS_OVERRIDE_NAME : "");
        }
    }
}
