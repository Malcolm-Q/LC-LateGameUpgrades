using MoreShipUpgrades.Misc;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace MoreShipUpgrades.Managers
{
    internal class SpawnItemManager : NetworkBehaviour
    {
        static LguLogger logger = new(nameof(SpawnItemManager));
        internal static SpawnItemManager Instance { get; private set; }
        internal Dictionary<string, GameObject> samplePrefabs = new Dictionary<string, GameObject>();
        void Awake()
        {
            Instance = this;
        }
        internal void SetupSpawnableItems()
        {
            foreach (GameObject sample in Instance.samplePrefabs.Values)
            {
                Item item = sample.GetComponent<PhysicsProp>().itemProperties;
                if (!StartOfRound.Instance.allItemsList.itemsList.Contains(item))
                {
                    StartOfRound.Instance.allItemsList.itemsList.Add(item);
                }

                logger.LogDebug($"{item.itemName} component initiated...");
            }
        }
        internal void SpawnSample(string name, Vector3 position)
        {
            GameObject go = Instantiate(samplePrefabs[name], position + Vector3.up, Quaternion.identity);
            go.GetComponent<NetworkObject>().Spawn();
        }

    }
}
