using Dawn;
using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace MoreShipUpgrades.Compat
{

	internal static class DawnLibCompat
	{
		public static bool Enabled =>
			BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey(DawnLib.PLUGIN_GUID);


		[MethodImpl(MethodImplOptions.NoOptimization | MethodImplOptions.NoInlining)]
		public static void RegisterNetworkPrefab(GameObject prefab)
		{
			DawnLib.RegisterNetworkPrefab(prefab);
		}

		[MethodImpl(MethodImplOptions.NoOptimization | MethodImplOptions.NoInlining)]
		public static void SetupItem(Item item)
		{
			NamespacedKey<DawnItemInfo> dawnItem = NamespacedKey<DawnItemInfo>.From(Metadata.DAWN_ID, item.itemName);
			DawnLib.DefineItem(dawnItem, item, _ => { });
		}

		[MethodImpl(MethodImplOptions.NoOptimization | MethodImplOptions.NoInlining)]
		public static void FixMixerGroups(GameObject prefab)
		{
			DawnLib.FixMixerGroups(prefab);
		}

		[MethodImpl(MethodImplOptions.NoOptimization | MethodImplOptions.NoInlining)]
		internal static void RemoveShopItem(Item storeItem)
		{
			NamespacedKey<DawnItemInfo> dawnItem = NamespacedKey<DawnItemInfo>.From(Metadata.DAWN_ID, storeItem.itemName);
			if (LethalContent.Items.TryGetValue(dawnItem, out DawnItemInfo mapObjectInfo))
			{
				return;
			}
			DawnLib.DefineItem(dawnItem, storeItem, builder => builder
			.DefineShop(shopBuilder => shopBuilder
				.SetPurchasePredicate(ITerminalPurchasePredicate.AlwaysHide())));
		}

		[MethodImpl(MethodImplOptions.NoOptimization | MethodImplOptions.NoInlining)]
		internal static void UpdateShopItemPrice(Item storeItem, int configuredPrice)
		{
			TerminalNode infoNode = ItemManager.SetupInfoNode(storeItem);
			NamespacedKey<DawnItemInfo> shopItem = NamespacedKey<DawnItemInfo>.From(Metadata.DAWN_ID, storeItem.itemName);
			if (LethalContent.Items.TryGetValue(shopItem, out DawnItemInfo mapObjectInfo))
			{
				return;
			}
			DawnLib.DefineItem(shopItem, storeItem, builder => builder
			.DefineShop(shopBuilder => shopBuilder
				.OverrideCost(configuredPrice)
				.OverrideInfoNode(infoNode)));
		}

		[MethodImpl(MethodImplOptions.NoOptimization | MethodImplOptions.NoInlining)]
		internal static void SetupMapObject(Item item, AnimationCurve curve)
		{
			NamespacedKey<DawnMapObjectInfo> dawnItem = NamespacedKey<DawnMapObjectInfo>.From(Metadata.DAWN_ID, item.itemName);
			if (LethalContent.MapObjects.TryGetValue(dawnItem, out DawnMapObjectInfo mapObjectInfo)) 
			{
				return;
			}
			DawnLib.DefineMapObject(dawnItem, item.spawnPrefab, builder => builder
			.DefineInside(insideBuilder => insideBuilder
				.SetWeights(curveBuilder => curveBuilder
				.SetGlobalCurve(curve))));
		}

		[MethodImpl(MethodImplOptions.NoOptimization | MethodImplOptions.NoInlining)]
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
