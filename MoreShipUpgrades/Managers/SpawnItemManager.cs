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
        internal Dictionary<string, List<GameObject>> samplePrefabs = new Dictionary<string, List<GameObject>>();
        void Awake()
        {
            Instance = this;
        }
        internal void SpawnSample(string name, Vector3 position)
        {
            List<GameObject> samples = samplePrefabs[name];
            GameObject go = Instantiate(samples[Random.Range(0, samples.Count)], position + Vector3.up, Quaternion.identity);
            go.GetComponent<NetworkObject>().Spawn();
        }

    }
}
