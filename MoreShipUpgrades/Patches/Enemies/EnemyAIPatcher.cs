using HarmonyLib;
using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc;
using MoreShipUpgrades.Misc.Upgrades;
using MoreShipUpgrades.UpgradeComponents.TierUpgrades.Enemies;

namespace MoreShipUpgrades.Patches.Enemies
{
    [HarmonyPatch(typeof(EnemyAI))]
    internal static class EnemyAIPatcher
    {
        static readonly LguLogger logger = new(nameof(EnemyAIPatcher));
        static ulong currentEnemy = 0;
        [HarmonyPostfix]
        [HarmonyPatch(nameof(EnemyAI.KillEnemy))]
        private static void SpawnSample(EnemyAI __instance, bool destroy)
        {
            if ((__instance is DoublewingAI || __instance is FlowerSnakeEnemy) && destroy) return;
            if (!(__instance.IsServer || __instance.IsHost)) return;

            if (currentEnemy == __instance.NetworkObject.NetworkObjectId) return;

            currentEnemy = __instance.NetworkObject.NetworkObjectId;
            string name = __instance.enemyType.enemyName;

            if (BaseUpgrade.GetActiveUpgrade(Hunter.UPGRADE_NAME) && Hunter.CanHarvest(name))
            {
                logger.LogDebug($"Spawning sample for {name}");
                ItemManager.Instance.SpawnSample(name.ToLower(), __instance.transform.position);
            }
        }
    }
}
