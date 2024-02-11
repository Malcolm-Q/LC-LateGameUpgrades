using HarmonyLib;
using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc;
using MoreShipUpgrades.Misc.Upgrades;
using MoreShipUpgrades.UpgradeComponents.TierUpgrades;
using System.Linq;
using Unity.Netcode;
using UnityEngine;

namespace MoreShipUpgrades.Patches.Enemies
{
    [HarmonyPatch(typeof(EnemyAI))]
    internal class EnemyAIPatcher
    {
        private static LGULogger logger = new LGULogger(nameof(EnemyAIPatcher));
        private static ulong currentEnemy = 0;
        [HarmonyPostfix]
        [HarmonyPatch(nameof(EnemyAI.KillEnemy))]
        private static void SpawnSample(EnemyAI __instance)
        {
            if (currentEnemy == __instance.NetworkObject.NetworkObjectId) return;
            currentEnemy = __instance.NetworkObject.NetworkObjectId;
            string name = __instance.enemyType.enemyName;

            if (!(BaseUpgrade.GetActiveUpgrade(Hunter.UPGRADE_NAME) && Hunter.tiers[BaseUpgrade.GetUpgradeLevel(Hunter.UPGRADE_NAME)].Contains(name.ToLower())))
            {
                logger.LogDebug($"No sample was found to spawn for {name.ToLower()}");
                logger.LogDebug("Enemies in the Hunter list");
                foreach (string monsterName in Hunter.tiers[BaseUpgrade.GetUpgradeLevel(Hunter.UPGRADE_NAME)])
                    logger.LogDebug($"{monsterName}");
                return;
            }

            logger.LogDebug($"Spawning sample for {name}");
            GameObject go = Object.Instantiate(UpgradeBus.instance.samplePrefabs[name.ToLower()], __instance.transform.position + Vector3.up, Quaternion.identity);
            go.GetComponent<NetworkObject>().Spawn();
        }
    }

}
