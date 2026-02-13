using MoreShipUpgrades.Configuration.Upgrades.Interfaces.TierUpgrades;
using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc.Upgrades;
using MoreShipUpgrades.Misc.Util;
using MoreShipUpgrades.UI.TerminalNodes;
using UnityEngine;

namespace MoreShipUpgrades.UpgradeComponents.TierUpgrades.Items
{
	internal class SmarterLockpick : TierUpgrade
	{

		internal const string UPGRADE_NAME = "Smarter Lockpick";
		internal const string PRICES_DEFAULT = "200,300,350,450";
		internal override void Start()
		{
			upgradeName = UPGRADE_NAME;
			overridenUpgradeName = GetConfiguration().SmarterLockpickUpgradeConfiguration.OverrideName;
			base.Start();
		}
		public override bool CanInitializeOnStart
		{
			get
			{
				ITierUpgradeConfiguration upgradeConfig = GetConfiguration().SmarterLockpickUpgradeConfiguration;
				string[] prices = upgradeConfig.Prices.Value.Split(',');
				return prices.Length == 0 || (prices.Length == 1 && (prices[0].Length == 0 || prices[0] == "0"));
			}
		}
		public override string GetDisplayInfo(int initialPrice = -1, int maxLevels = -1, int[] incrementalPrices = null)
		{
			static float infoFunction(int level)
			{
				ITierEffectUpgradeConfiguration<int> upgradeConfig = GetConfiguration().SmarterLockpickUpgradeConfiguration;
				return upgradeConfig.InitialEffect.Value + (level * upgradeConfig.IncrementalEffect.Value);
			}
			const string infoFormat = "LVL {0} - {1} - In case of a full team wipe, each scrap present in the ship has a {2}% chance of not being discarded.\n";
			return Tools.GenerateInfoForUpgrade(infoFormat, initialPrice, incrementalPrices, infoFunction, purchaseMode: GetConfiguration().SmarterLockpickUpgradeConfiguration.PurchaseMode);
		}

		public new static (string, string[]) RegisterScrapToUpgrade()
		{
			return (UPGRADE_NAME, GetConfiguration().SmarterLockpickUpgradeConfiguration.ItemProgressionItems.Value.Split(","));
		}
		public new static void RegisterUpgrade()
		{
			GameObject prefab = ItemManager.CreateNetworkPrefab(UPGRADE_NAME);
			prefab.AddComponent<SmarterLockpick>();
			ItemManager.RegisterNetworkPrefab(prefab);
			Plugin.networkPrefabs[UPGRADE_NAME] = prefab;
		}
		public new static CustomTerminalNode RegisterTerminalNode()
		{
			return UpgradeBus.Instance.SetupMultiplePurchaseableTerminalNode(UPGRADE_NAME, GetConfiguration().SmarterLockpickUpgradeConfiguration, Plugin.networkPrefabs[UPGRADE_NAME]);
		}

		public static float IncreaseLockpickEfficiency(float defaultAmount)
		{
			ITierEffectUpgradeConfiguration<int> upgradeConfig = GetConfiguration().SmarterLockpickUpgradeConfiguration;
			if (!upgradeConfig.Enabled) return defaultAmount;
			if (!GetActiveUpgrade(UPGRADE_NAME)) return defaultAmount;
			float efficiencyMultiplier = (upgradeConfig.InitialEffect + (upgradeConfig.IncrementalEffect * GetUpgradeLevel(UPGRADE_NAME))) / 100f;
			efficiencyMultiplier = Mathf.Clamp(efficiencyMultiplier, 1f, float.MaxValue);
			return defaultAmount + (defaultAmount *  efficiencyMultiplier);
		}
	}
}
