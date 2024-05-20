using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc;
using MoreShipUpgrades.Misc.TerminalNodes;
using MoreShipUpgrades.Misc.Upgrades;
using MoreShipUpgrades.Misc.Util;
using MoreShipUpgrades.UpgradeComponents.Interfaces;

namespace MoreShipUpgrades.UpgradeComponents.TierUpgrades.AttributeUpgrades
{
    internal class ClimbingGloves : GameAttributeTierUpgrade, IPlayerSync
    {
        internal const string UPGRADE_NAME = "Climbing Gloves";
        internal const string DEFAULT_PRICES = "200,250,300";
        void Awake()
        {
            upgradeName = UPGRADE_NAME;
            overridenUpgradeName = UpgradeBus.Instance.PluginConfiguration.CLIMBING_GLOVES_OVERRIDE_NAME;
            logger = new LguLogger(UPGRADE_NAME);
            changingAttribute = GameAttribute.PLAYER_CLIMB_SPEED;
            initialValue = UpgradeBus.Instance.PluginConfiguration.INITIAL_CLIMBING_SPEED_BOOST.Value;
            incrementalValue = UpgradeBus.Instance.PluginConfiguration.INCREMENTAL_CLIMBING_SPEED_BOOST.Value;
        }

        public override string GetDisplayInfo(int initialPrice = -1, int maxLevels = -1, int[] incrementalPrices = null)
        {
            System.Func<int, float> infoFunction = level => UpgradeBus.Instance.PluginConfiguration.INITIAL_CLIMBING_SPEED_BOOST.Value + level * UpgradeBus.Instance.PluginConfiguration.INCREMENTAL_CLIMBING_SPEED_BOOST.Value;
            string infoFormat = "LVL {0} - ${1} - Increases the speed of climbing ladders by {2} units.\n";
            return Tools.GenerateInfoForUpgrade(infoFormat, initialPrice, incrementalPrices, infoFunction);
        }
        public override bool CanInitializeOnStart
        {
            get
            {
                string[] prices = UpgradeBus.Instance.PluginConfiguration.CLIMBING_GLOVES_PRICES.Value.Split(',');
                bool free = UpgradeBus.Instance.PluginConfiguration.CLIMBING_GLOVES_PRICE.Value <= 0 && prices.Length == 1 && (prices[0] == "" || prices[0] == "0");
                return free;
            }
        }

        public new static void RegisterUpgrade()
        {
            SetupGenericPerk<ClimbingGloves>(UPGRADE_NAME);
        }
        public new static CustomTerminalNode RegisterTerminalNode()
        {
            LategameConfiguration configuration = UpgradeBus.Instance.PluginConfiguration;

            return UpgradeBus.Instance.SetupMultiplePurchasableTerminalNode(UPGRADE_NAME,
                                                configuration.SHARED_UPGRADES.Value || !configuration.CLIMBING_GLOVES_INDIVIDUAL.Value,
                                                configuration.CLIMBING_GLOVES_ENABLED.Value,
                                                configuration.CLIMBING_GLOVES_PRICE.Value,
                                                UpgradeBus.ParseUpgradePrices(configuration.CLIMBING_GLOVES_PRICES.Value),
                                                configuration.OVERRIDE_UPGRADE_NAMES ? configuration.CLIMBING_GLOVES_OVERRIDE_NAME : "");
        }
    }
}
