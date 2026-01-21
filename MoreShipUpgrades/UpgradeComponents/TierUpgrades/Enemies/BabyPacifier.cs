using MoreShipUpgrades.Configuration.Upgrades.Interfaces.TierUpgrades;
using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc.Upgrades;
using MoreShipUpgrades.Misc.Util;
using MoreShipUpgrades.UI.TerminalNodes;
using MoreShipUpgrades.UpgradeComponents.TierUpgrades.Items;
using UnityEngine;

namespace MoreShipUpgrades.UpgradeComponents.TierUpgrades.Enemies
{
	public class BabyPacifier : TierUpgrade
	{
		public const string UPGRADE_NAME = "Baby Pacifier";
		internal const string PRICES_DEFAULT = "100,100,200,200,400";

		internal override void Start()
		{
			upgradeName = UPGRADE_NAME;

			base.Start();
		}
		public override bool CanInitializeOnStart
		{
			get
			{
				ITierEffectUpgradeConfiguration<int> upgradeConfig = GetConfiguration().BabyPacifierUpgradeConfiguration;
				string[] prices = upgradeConfig.Prices.Value.Split(',');
				return prices.Length == 0 || (prices.Length == 1 && (prices[0].Length == 0 || prices[0] == "0"));
			}
		}
		public static float GetDecreasedGrowthRate(float defaultValue)
		{
			ITierEffectUpgradeConfiguration<int> upgradeConfig = GetConfiguration().BabyPacifierUpgradeConfiguration;
			if (!upgradeConfig.Enabled) return defaultValue;
			if (!GetActiveUpgrade(UPGRADE_NAME)) return defaultValue;
			float additionalValue = (upgradeConfig.InitialEffect + (GetUpgradeLevel(UPGRADE_NAME) * upgradeConfig.IncrementalEffect)) / 100f;
			return Mathf.Clamp(defaultValue - (defaultValue * additionalValue), 0f, defaultValue);
		}
		public override string GetDisplayInfo(int initialPrice = -1, int maxLevels = -1, int[] incrementalPrices = null)
		{
			static float infoFunction(int level)
			{
				ITierEffectUpgradeConfiguration<int> upgradeConfig = GetConfiguration().BabyPacifierUpgradeConfiguration;
				return upgradeConfig.InitialEffect.Value + (level * upgradeConfig.IncrementalEffect.Value);
			}
			const string infoFormat = "LVL {0} - {1} - The amount of growth increase when the maneater is crying is decreased by {2}%\n";
			return Tools.GenerateInfoForUpgrade(infoFormat, initialPrice, incrementalPrices, infoFunction, purchaseMode: GetConfiguration().ClayGlassesConfiguration.PurchaseMode.Value);
		}

		public new static (string, string[]) RegisterScrapToUpgrade()
		{
			return (UPGRADE_NAME, GetConfiguration().BabyPacifierUpgradeConfiguration.ItemProgressionItems.Value.Split(","));
		}
		public new static void RegisterUpgrade()
		{
			GameObject prefab = ItemManager.CreateNetworkPrefab(UPGRADE_NAME);
			prefab.AddComponent<BabyPacifier>();
			ItemManager.RegisterNetworkPrefab(prefab);
			Plugin.networkPrefabs[UPGRADE_NAME] = prefab;
		}
		public new static CustomTerminalNode RegisterTerminalNode()
		{
			return UpgradeBus.Instance.SetupMultiplePurchaseableTerminalNode(UPGRADE_NAME, GetConfiguration().BabyPacifierUpgradeConfiguration, Plugin.networkPrefabs[UPGRADE_NAME]);
		}
	}
}
