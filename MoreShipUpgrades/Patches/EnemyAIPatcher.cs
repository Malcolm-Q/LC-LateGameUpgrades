using HarmonyLib;
using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc;
using MoreShipUpgrades.UpgradeComponents;
using System.Linq;
using Unity.Netcode;
using UnityEngine;

namespace MoreShipUpgrades.Patches
{
    [HarmonyPatch(typeof(EnemyAI))]
    internal class EnemyAIPatcher
    {
        private static LGULogger logger = new LGULogger(nameof(EnemyAIPatcher));
        private static ulong currentEnemy = 0;
        [HarmonyPostfix]
        [HarmonyPatch("KillEnemyServerRpc")]
        private static void SpawnSample(EnemyAI __instance)
        {
            if (currentEnemy == __instance.NetworkObject.NetworkObjectId) return;
            currentEnemy = __instance.NetworkObject.NetworkObjectId;
            string name = __instance.enemyType.enemyName;
            if (UpgradeBus.instance.hunter && hunterScript.tiers[UpgradeBus.instance.huntLevel].Contains(name))
            {
                logger.LogDebug($"Spawning sample for {name}");
                GameObject go = Object.Instantiate(UpgradeBus.instance.samplePrefabs[name],__instance.transform.position + Vector3.up,Quaternion.identity);
                PhysicsProp prop = go.GetComponent<PhysicsProp>();
                int value = Random.Range(prop.itemProperties.minValue, prop.itemProperties.maxValue);
                go.GetComponent<NetworkObject>().Spawn();
                LGUStore.instance.SyncValuesClientRpc(value, new NetworkBehaviourReference(prop));
            }
        }
    }

}
