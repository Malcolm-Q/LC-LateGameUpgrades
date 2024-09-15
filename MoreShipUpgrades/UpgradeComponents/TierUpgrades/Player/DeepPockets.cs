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
            overridenUpgradeName = GetConfiguration().DEEPER_POCKETS_OVERRIDE_NAME;
            base.Start();
        }
        public override string GetDisplayInfo(int initialPrice = -1, int maxLevels = -1, int[] incrementalPrices = null)
        {
            static float infoFunction(int level)
            {
                LategameConfiguration config = GetConfiguration();
                return config.DEEPER_POCKETS_INITIAL_TWO_HANDED_ITEMS.Value + (level * config.DEEPER_POCKETS_INCREMENTAL_TWO_HANDED_ITEMS.Value);
            }
            const string infoFormat = "LVL {0} - ${1} - Increases the two handed carry capacity of the player by {2}\n";
            return Tools.GenerateInfoForUpgrade(infoFormat, initialPrice, incrementalPrices, infoFunction);
        }

        public override bool CanInitializeOnStart
        {
            get
            {
                LategameConfiguration config = GetConfiguration();
                string[] prices = config.DEEPER_POCKETS_PRICES.Value.Split(',');
                return config.DEEPER_POCKETS_PRICE.Value <= 0 && prices.Length == 1 && (prices[0].Length == 0 || prices[0] == "0");
            }
        }
        public new static (string, string[]) RegisterScrapToUpgrade()
        {
            return (UPGRADE_NAME, GetConfiguration().DEEPER_POCKETS_ITEM_PROGRESSION_ITEMS.Value.Split(","));
        }
        public new static void RegisterUpgrade()
        {
            SetupGenericPerk<DeepPockets>(UPGRADE_NAME);
        }
        public new static CustomTerminalNode RegisterTerminalNode()
        {
            LategameConfiguration configuration = GetConfiguration();

            return UpgradeBus.Instance.SetupMultiplePurchasableTerminalNode(UPGRADE_NAME,
                                                configuration.SHARED_UPGRADES || !configuration.DEEPER_POCKETS_INDIVIDUAL,
                                                configuration.DEEPER_POCKETS_ENABLED,
                                                configuration.DEEPER_POCKETS_PRICE,
                                                UpgradeBus.ParseUpgradePrices(configuration.DEEPER_POCKETS_PRICES),
                                                configuration.OVERRIDE_UPGRADE_NAMES ? configuration.DEEPER_POCKETS_OVERRIDE_NAME : "");
        }
    }
}
