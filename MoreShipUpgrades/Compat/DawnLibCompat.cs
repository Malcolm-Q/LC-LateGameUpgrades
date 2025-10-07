using Dawn;
using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc;
using UnityEngine;

namespace MoreShipUpgrades.Compat
{
	internal static class DawnLibCompat
	{
		public static bool Enabled =>
			BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey(DawnLib.PLUGIN_GUID);

		public static void RegisterNetworkPrefab(GameObject prefab)
		{
			DawnLib.RegisterNetworkPrefab(prefab);
		}

		public static void SetupItem(Item item)
		{
			NamespacedKey<DawnItemInfo> dawnItem = NamespacedKey<DawnItemInfo>.From(Metadata.DAWN_ID, item.itemName);
			DawnLib.DefineItem(dawnItem, item, _ => { });
		}

		public static void FixMixerGroups(GameObject prefab)
		{
			DawnLib.FixMixerGroups(prefab);
		}

		internal static void RemoveShopItem(Item storeItem)
		{
			NamespacedKey<DawnItemInfo> shopItem = NamespacedKey<DawnItemInfo>.From(Metadata.DAWN_ID, storeItem.itemName);
			DawnLib.DefineItem(shopItem, storeItem, builder => builder
			.DefineShop(shopBuilder => shopBuilder
				.SetPurchasePredicate(ITerminalPurchasePredicate.AlwaysHide())));
		}

		internal static void UpdateShopItemPrice(Item storeItem, int configuredPrice)
		{
			TerminalNode infoNode = ItemManager.SetupInfoNode(storeItem);
			NamespacedKey<DawnItemInfo> shopItem = NamespacedKey<DawnItemInfo>.From(Metadata.DAWN_ID, storeItem.itemName);
			DawnLib.DefineItem(shopItem, storeItem, builder => builder
			.DefineShop(shopBuilder => shopBuilder
				.OverrideCost(configuredPrice)
				.OverrideInfoNode(infoNode)));
		}

		internal static void SetupMapObject(Item item, AnimationCurve curve)
		{
			NamespacedKey<DawnMapObjectInfo> dawnItem = NamespacedKey<DawnMapObjectInfo>.From(Metadata.DAWN_ID, item.itemName);
			DawnLib.DefineMapObject(dawnItem, item.spawnPrefab, builder => builder
			.DefineInside(insideBuilder => insideBuilder
				.SetWeights(curveBuilder => curveBuilder
				.SetGlobalCurve(curve))));
		}

		internal static void SetupStoreItem(Item storeItem, TerminalNode infoNode)
		{
			NamespacedKey<DawnItemInfo> shopItem = NamespacedKey<DawnItemInfo>.From(Metadata.DAWN_ID, storeItem.itemName);
			DawnLib.DefineItem(shopItem, storeItem, builder => builder
			.DefineShop(shopBuilder => shopBuilder
				.OverrideCost(storeItem.creditsWorth)
				.OverrideInfoNode(infoNode)));
		}
	}
}
