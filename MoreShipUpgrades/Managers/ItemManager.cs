using LethalLib.Modules;
using MoreShipUpgrades.API;
using MoreShipUpgrades.Misc;
using MoreShipUpgrades.UpgradeComponents.Interfaces;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace MoreShipUpgrades.Managers
{
    internal class ItemManager : NetworkBehaviour
    {
        static LguLogger logger = new(nameof(ItemManager));
        internal static ItemManager Instance { get; private set; }
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

        internal static TerminalNode SetupInfoNode(Item storeItem)
        {
            TerminalNode infoNode = ScriptableObject.CreateInstance<TerminalNode>();
            GrabbableObject grabbableObject = storeItem.spawnPrefab.GetComponent<GrabbableObject>();
            if (grabbableObject is IDisplayInfo displayInfo) infoNode.displayText += displayInfo.GetDisplayInfo() + "\n";
            if (grabbableObject is IItemWorldBuilding worldBuilding) infoNode.displayText += worldBuilding.GetWorldBuildingText() + "\n";
            infoNode.clearPreviousText = true;
            return infoNode;
        }
        internal static void SetupStoreItem(Item storeItem)
        {
            TerminalNode infoNode = SetupInfoNode(storeItem);
            Items.RegisterShopItem(shopItem: storeItem, itemInfo: infoNode, price: storeItem.creditsWorth);
        }
    }
}
