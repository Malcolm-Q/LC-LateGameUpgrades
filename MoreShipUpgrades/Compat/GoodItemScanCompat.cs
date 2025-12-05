using GoodItemScan;
using MoreShipUpgrades.Managers;
using System.Runtime.CompilerServices;

namespace MoreShipUpgrades.Compat
{
    internal static class GoodItemScanCompat
    {
        public static bool Enabled =>
            BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey("TestAccount666.GoodItemScan");

		[MethodImpl(MethodImplOptions.NoOptimization | MethodImplOptions.NoInlining)]
		static internal void IncreaseScanDistance(int distance)
        {
            CheatsAPI.additionalDistance += distance;
        }

		[MethodImpl(MethodImplOptions.NoOptimization | MethodImplOptions.NoInlining)]
		static internal void IncreaseEnemyScanDistance(int distance)
        {
            CheatsAPI.additionalEnemyDistance += distance;
        }

		[MethodImpl(MethodImplOptions.NoOptimization | MethodImplOptions.NoInlining)]
		static internal void ToggleScanThroughWalls(bool scanThroughWalls)
        {
            CheatsAPI.noLineOfSightDistance += scanThroughWalls ? (int)UpgradeBus.Instance.PluginConfiguration.BetterScannerUpgradeConfiguration.NodeRangeIncrease : -(int)UpgradeBus.Instance.PluginConfiguration.BetterScannerUpgradeConfiguration.NodeRangeIncrease;
        }
    }
}
