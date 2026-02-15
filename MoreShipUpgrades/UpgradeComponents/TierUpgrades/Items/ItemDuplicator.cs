using MoreShipUpgrades.Configuration.Upgrades.Interfaces.TierUpgrades;
using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc.Upgrades;
using MoreShipUpgrades.Misc.Util;
using MoreShipUpgrades.UI.TerminalNodes;
using System.Collections.Generic;
using UnityEngine;

namespace MoreShipUpgrades.UpgradeComponents.TierUpgrades.Items
{
	internal class ItemDuplicator : TierUpgrade
	{
		public const string UPGRADE_NAME = "Item Duplicator";
		internal const string PRICES_DEFAULT = "250,350,500,750";

		internal override void Start()
		{
			upgradeName = UPGRADE_NAME;
			overridenUpgradeName = GetConfiguration().ItemDuplicatorUpgradeConfiguration.OverrideName;
			base.Start();
		}
		public override bool CanInitializeOnStart
		{
			get
			{
				ITierEffectUpgradeConfiguration<int> upgradeConfig = GetConfiguration().ItemDuplicatorUpgradeConfiguration;
				string[] prices = upgradeConfig.Prices.Value.Split(',');
				return prices.Length == 0 || (prices.Length == 1 && (prices[0].Length == 0 || prices[0] == "0"));
			}
		}
		public static float GetChanceForDuplication()
		{
			ITierEffectUpgradeConfiguration<int> upgradeConfig = GetConfiguration().ItemDuplicatorUpgradeConfiguration;
			if (!upgradeConfig.Enabled) return 0f;
			if (!GetActiveUpgrade(UPGRADE_NAME)) return 0f;
			return (upgradeConfig.InitialEffect + (GetUpgradeLevel(UPGRADE_NAME) * upgradeConfig.IncrementalEffect)) / 100f;
		}
		public static void AttemptDuplication(ref List<int> itemsToDeliver)
		{
			List<int> result = [.. itemsToDeliver];
			float chance = GetChanceForDuplication();
			for (int i = 0; i < itemsToDeliver.Count; i++)
			{
				if (UnityEngine.Random.Range(0f, 1f) < chance) result.Add(itemsToDeliver[i]);
			}
			itemsToDeliver = result;
		}
		public override string GetDisplayInfo(int initialPrice = -1, int maxLevels = -1, int[] incrementalPrices = null)
		{
			static float infoFunction(int level)
			{
				ITierEffectUpgradeConfiguration<int> upgradeConfig = GetConfiguration().ItemDuplicatorUpgradeConfiguration;
				return upgradeConfig.InitialEffect.Value + (level * upgradeConfig.IncrementalEffect.Value);
			}
			const string infoFormat = "LVL {0} - {1} - Whenever purchasing an item from the Company store, it has {2}% chance of being duplicated when dropped (without occupying space).\n";
			return Tools.GenerateInfoForUpgrade(infoFormat, initialPrice, incrementalPrices, infoFunction, purchaseMode: GetConfiguration().ClayGlassesConfiguration.PurchaseMode.Value);
		}

		public new static (string, string[]) RegisterScrapToUpgrade()
		{
			return (UPGRADE_NAME, GetConfiguration().ItemDuplicatorUpgradeConfiguration.ItemProgressionItems.Value.Split(","));
		}
		public new static void RegisterUpgrade()
		{
			GameObject prefab = ItemManager.CreateNetworkPrefab(UPGRADE_NAME);
			prefab.AddComponent<ItemDuplicator>();
			ItemManager.RegisterNetworkPrefab(prefab);
			Plugin.networkPrefabs[UPGRADE_NAME] = prefab;
		}
		public new static CustomTerminalNode RegisterTerminalNode()
		{
			return UpgradeBus.Instance.SetupMultiplePurchaseableTerminalNode(UPGRADE_NAME, GetConfiguration().ItemDuplicatorUpgradeConfiguration, Plugin.networkPrefabs[UPGRADE_NAME]);
		}
	}
}
