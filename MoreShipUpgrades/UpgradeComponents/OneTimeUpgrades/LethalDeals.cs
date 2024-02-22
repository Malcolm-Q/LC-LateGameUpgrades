using MoreShipUpgrades.Misc.Upgrades;

namespace MoreShipUpgrades.UpgradeComponents.OneTimeUpgrades
{
    internal class LethalDeals : OneTimeUpgrade
    {
        public const string UPGRADE_NAME = "Lethal Deals";
        const int GUARANTEED_ITEMS_AMOUNT = 1;
        void Awake()
        {
            upgradeName = UPGRADE_NAME;
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
    }
}
