using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc;
using MoreShipUpgrades.Misc.TerminalNodes;
using MoreShipUpgrades.Misc.Upgrades;

namespace MoreShipUpgrades.UpgradeComponents.OneTimeUpgrades
{
    internal class LethalDeals : OneTimeUpgrade
    {
        public const string UPGRADE_NAME = "Lethal Deals";
        const int GUARANTEED_ITEMS_AMOUNT = 1;
        public override bool CanInitializeOnStart => UpgradeBus.Instance.PluginConfiguration.LETHAL_DEALS_PRICE.Value <= 0;
        void Awake()
        {
            upgradeName = UPGRADE_NAME;
            overridenUpgradeName = UpgradeBus.Instance.PluginConfiguration.LETHAL_DEALS_OVERRIDE_NAME;
        }
        public static int GetLethalDealsGuaranteedItems(int amount)
        {
            if (!GetActiveUpgrade(UPGRADE_NAME)) return amount;
            return GUARANTEED_ITEMS_AMOUNT;
        }
        public override string GetDisplayInfo(int price = -1)
        {
            return $"${price} - Guarantees at least one item will be on sale in the store.";
        }
        public new static (string, string[]) RegisterScrapToUpgrade()
        {
            return (UPGRADE_NAME, UpgradeBus.Instance.PluginConfiguration.LETHAL_DEALS_ITEM_PROGRESSION_ITEMS.Value.Split(","));
        }
        public new static void RegisterUpgrade()
        {
            SetupGenericPerk<LethalDeals>(UPGRADE_NAME);
        }
        public new static CustomTerminalNode RegisterTerminalNode()
        {
            LategameConfiguration configuration = UpgradeBus.Instance.PluginConfiguration;

            return UpgradeBus.Instance.SetupOneTimeTerminalNode(UPGRADE_NAME,
                                    shareStatus: true,
                                    configuration.LETHAL_DEALS_ENABLED.Value,
                                    configuration.LETHAL_DEALS_PRICE.Value,
                                    configuration.OVERRIDE_UPGRADE_NAMES ? configuration.LETHAL_DEALS_OVERRIDE_NAME : "");
        }
    }
}
