using LethalLib.Extras;
using LethalLib.Modules;
using MoreShipUpgrades.API;
using MoreShipUpgrades.Compat;
using MoreShipUpgrades.UpgradeComponents.Interfaces;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using Unity.Netcode;
using UnityEngine;

namespace MoreShipUpgrades.Managers
{
    internal class ItemManager : NetworkBehaviour
    {
        internal static ItemManager Instance { get; private set; }
        internal Dictionary<string, WeightingGroup<GameObject>> samplePrefabs = [];
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
		internal static void SetupSampleItem(Item sampleItem, bool registerNetworkPrefab, string monsterName, double weight)
		{
			if (registerNetworkPrefab) RegisterNetworkPrefab(sampleItem.spawnPrefab);
			if (!Instance.samplePrefabs.ContainsKey(monsterName.ToLower())) Instance.samplePrefabs.Add(monsterName.ToLower(), new WeightingGroup<GameObject>());
			Instance.samplePrefabs[monsterName.ToLower()].Add(sampleItem.spawnPrefab, weight);
		}
		internal static void SetupStoreItem(Item storeItem)
		{
            RegisterNetworkPrefab(storeItem.spawnPrefab);

			TerminalNode infoNode = SetupInfoNode(storeItem);
			if (!DawnLibCompat.Enabled || !UpgradeBus.Instance.PluginConfiguration.UseDawnLib)
			{
				Items.RegisterShopItem(shopItem: storeItem, itemInfo: infoNode, price: storeItem.creditsWorth);
			}
			else
			{
				DawnLibCompat.SetupStoreItem(storeItem, infoNode);
			}
		}

		internal static void SetupMapObject(Item item, AnimationCurve curve)
		{
			if (!DawnLibCompat.Enabled || !UpgradeBus.Instance.PluginConfiguration.UseDawnLib)
			{
				SpawnableMapObjectDef mapObjDef = ScriptableObject.CreateInstance<SpawnableMapObjectDef>();
				mapObjDef.spawnableMapObject = new SpawnableMapObject
				{
					prefabToSpawn = item.spawnPrefab
				};
				MapObjects.RegisterMapObject(mapObjDef, Levels.LevelTypes.All, (_) => curve);
			}
			else
			{
				DawnLibCompat.SetupMapObject(item, curve);
			}
		}

		internal static void RegisterNetworkPrefab(GameObject prefab)
		{
			if (!DawnLibCompat.Enabled || !UpgradeBus.Instance.PluginConfiguration.UseDawnLib)
			{
				LethalLib.Modules.NetworkPrefabs.RegisterNetworkPrefab(prefab);
			}
			else
			{
				DawnLibCompat.RegisterNetworkPrefab(prefab);
			}
		}

        internal static void SetupItem(Item item)
		{
			if (!DawnLibCompat.Enabled || !UpgradeBus.Instance.PluginConfiguration.UseDawnLib)
			{
				LethalLib.Modules.Items.RegisterItem(item);
			}
			else
			{
				DawnLibCompat.SetupItem(item);
			}
		}

        internal static void FixMixerGroups(GameObject prefab)
		{
			if (!DawnLibCompat.Enabled || !UpgradeBus.Instance.PluginConfiguration.UseDawnLib)
			{
				Utilities.FixMixerGroups(prefab);
			}
			else
			{
				DawnLibCompat.FixMixerGroups(prefab);
			}
		}

		internal static void RemoveShopItem(Item storeItem)
		{
			if (!DawnLibCompat.Enabled || !UpgradeBus.Instance.PluginConfiguration.UseDawnLib)
			{
				Items.RemoveShopItem(storeItem);
			}
			else
			{
				DawnLibCompat.RemoveShopItem(storeItem);
			}
		}

		internal static void UpdateShopItemPrice(Item storeItem, int configuredPrice)
		{
			if (!DawnLibCompat.Enabled || !UpgradeBus.Instance.PluginConfiguration.UseDawnLib)
			{
				Items.UpdateShopItemPrice(storeItem, configuredPrice);
			}
			else
			{
				DawnLibCompat.UpdateShopItemPrice(storeItem, configuredPrice);
			}
		}

		internal static GameObject CreateNetworkPrefab(string prefabName)
		{
			GameObject gameObject = new GameObject(prefabName);
			gameObject.hideFlags = HideFlags.HideAndDontSave;
			gameObject.AddComponent<NetworkObject>();
			byte[] value = MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(Assembly.GetCallingAssembly().GetName().Name + prefabName));
			gameObject.GetComponent<NetworkObject>().GlobalObjectIdHash = BitConverter.ToUInt32(value, 0);
			RegisterNetworkPrefab(gameObject);
			return gameObject;
		}
	}
}
