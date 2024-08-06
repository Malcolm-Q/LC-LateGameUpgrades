using GoodItemScan;
using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc.Upgrades;
using MoreShipUpgrades.UpgradeComponents.TierUpgrades.Player;

namespace MoreShipUpgrades.Compat
{
    internal static class GoodItemScanCompat
    {
        public static bool Enabled =>
            BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey("TestAccount666.GoodItemScan");

        static internal void IncreaseScanDistance(int distance)
        {
            CheatsAPI.additionalDistance += distance;
        }

        static internal void IncreaseEnemyScanDistance(int distance)
        {
            CheatsAPI.additionalEnemyDistance += distance;
        }

        static internal void ToggleScanThroughWalls(bool scanThroughWalls)
        {
            CheatsAPI.noLineOfSightDistance += scanThroughWalls ? (int)UpgradeBus.Instance.PluginConfiguration.NODE_DISTANCE_INCREASE : 0;
        }

        internal static void Reset()
        {
            if (!BaseUpgrade.GetActiveUpgrade(BetterScanner.UPGRADE_NAME)) return;
            int level = BaseUpgrade.GetUpgradeLevel(BetterScanner.UPGRADE_NAME);
            if (level > 0)
            {
                IncreaseScanDistance((int)-UpgradeBus.Instance.PluginConfiguration.NODE_DISTANCE_INCREASE);
                IncreaseEnemyScanDistance((int)-UpgradeBus.Instance.PluginConfiguration.NODE_DISTANCE_INCREASE);
            }
            if (level == 2)
            {
                ToggleScanThroughWalls(false);
            }
        }
    }
}
