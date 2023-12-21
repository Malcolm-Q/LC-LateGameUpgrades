using HarmonyLib;
using MoreShipUpgrades.Managers;
using MoreShipUpgrades.UpgradeComponents;
using System.Linq;
using Unity.Netcode;
using UnityEngine;

namespace MoreShipUpgrades.Patches
{
    [HarmonyPatch(typeof(EnemyAI))]
    internal class EnemyAIPatcher
    {
        private static ulong currentEnemy = 0;
        [HarmonyPostfix]
        [HarmonyPatch("KillEnemyServerRpc")]
        private static void SpawnSample(EnemyAI __instance)
        {
            if (currentEnemy == __instance.NetworkObject.NetworkObjectId) return;
            currentEnemy = __instance.NetworkObject.NetworkObjectId;
            string name = __instance.GetComponentInChildren<ScanNodeProperties>().headerText.ToLower();
            Debug.Log(name);
            if (hunterScript.tiers[UpgradeBus.instance.huntLevel].Contains(name))
            {
                Debug.Log("SPAWNING SAMPLE");
                GameObject go = GameObject.Instantiate(UpgradeBus.instance.samplePrefabs[name],__instance.transform.position + Vector3.up,Quaternion.identity);
                PhysicsProp prop = go.GetComponent<PhysicsProp>();
                int value = Random.Range(prop.itemProperties.minValue, prop.itemProperties.maxValue);
                prop.scrapValue = value;
                prop.itemProperties.creditsWorth = value;
                go.GetComponentInChildren<ScanNodeProperties>().subText = $"Value: ${value}";
                go.GetComponent<NetworkObject>().Spawn();
            }
        }
    }

}
