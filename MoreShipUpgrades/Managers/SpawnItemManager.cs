using MoreShipUpgrades.API;
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
        internal Dictionary<string, WeightingGroup<GameObject>> samplePrefabs = new Dictionary<string, WeightingGroup<GameObject>>();
        void Awake()
        {
            Instance = this;
        }
        internal void SpawnSample(string name, Vector3 position)
        {
            GameObject sample = samplePrefabs[name].GetItem();
            GameObject go = Instantiate(sample, position + Vector3.up, Quaternion.identity);
            go.GetComponent<NetworkObject>().Spawn();
        }

    }
}
