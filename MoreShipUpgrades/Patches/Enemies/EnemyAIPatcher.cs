using HarmonyLib;
using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc;
using MoreShipUpgrades.Misc.Upgrades;
using MoreShipUpgrades.UpgradeComponents.TierUpgrades;

namespace MoreShipUpgrades.Patches.Enemies
{
    [HarmonyPatch(typeof(EnemyAI))]
    internal static class EnemyAIPatcher
    {
        private static LguLogger logger = new LguLogger(nameof(EnemyAIPatcher));
        private static ulong currentEnemy = 0;
        [HarmonyPostfix]
        [HarmonyPatch(nameof(EnemyAI.KillEnemy))]
        private static void SpawnSample(EnemyAI __instance)
        {
            if (!(__instance.IsServer || __instance.IsHost)) return;

            if (currentEnemy == __instance.NetworkObject.NetworkObjectId) return;

            currentEnemy = __instance.NetworkObject.NetworkObjectId;
            string name = __instance.enemyType.enemyName;

            if (BaseUpgrade.GetActiveUpgrade(Hunter.UPGRADE_NAME) && Hunter.CanHarvest(name))
            {
                logger.LogDebug($"Spawning sample for {name}");
                SpawnItemManager.Instance.SpawnSample(name.ToLower(), __instance.transform.position);
            }
        }
    }

}
