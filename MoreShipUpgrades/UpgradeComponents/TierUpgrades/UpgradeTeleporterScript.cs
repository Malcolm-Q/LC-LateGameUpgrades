using GameNetcodeStuff;
using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc;
using MoreShipUpgrades.Misc.Upgrades;
using System;

namespace MoreShipUpgrades.UpgradeComponents.TierUpgrades {
	internal class UpgradeTeleportersScript : TierUpgrade {
		public static string UPGRADE_NAME = "Upgrade Teleporters";
		public static UpgradeTeleportersScript instance;
		public static LGULogger logger;

		public static string ENABLED_SECTION = $"Enable {UPGRADE_NAME}";
		public static string ENABLED_DESCRIPTION = "Allows you to take items through the teleporter";

		public static string REGULAR_UPGRADE_DESCRIPTION = "Price of regular teleporter upgrade";
		public static string INVERSE_UPGRADE_DESCRIPTION = "Price of inverse teleporter upgrade";
		public static int REGULAR_TP_UPGRADE_PRICE = 500; // Price of first level
		public const string INVERSE_TP_UPGRADE_PRICE = "700"; // Prices of subsequent levels - count indicates how many additional levels

		internal const string WORLD_BUILDING_TEXT = "\n\nNew software rolled specifically to your teleporters. " + 
			"With this bleeding edge technology, you can now take as many items with you as you can fit in your inventory.\n\n";


		// Called when initializing the upgrade at runtime
		void Start() {
			upgradeName = UPGRADE_NAME;
			logger = new LGULogger(UPGRADE_NAME);
			DontDestroyOnLoad(gameObject);
			Register();
		}

		// Called when upgrade is first purchased or loaded from save
		public override void Load() {
			base.Load();

			logger.LogInfo("Running UpgradeTeleporters load function");

			logger.LogInfo($"UpgradeBus level: {UpgradeBus.instance.teleporterUpgradeLevel}");

			if (!UpgradeBus.instance.teleporterUpgrade || UpgradeBus.instance.teleporterUpgradeLevel == 0) {
				UpgradeBus.instance.teleporterUpgradeLevel = 1;
				logger.LogInfo($"Upgrading teleporter to level {UpgradeBus.instance.teleporterUpgradeLevel}");
			}
			UpgradeBus.instance.teleporterUpgrade = true;
		}

		// Called when purchasing subsequent levels
		// No need to be fancy here since there's only 2 levels
		public override void Increment() {
			UpgradeBus.instance.teleporterUpgradeLevel = 2;
			logger.LogInfo($"Upgrading teleporter level to {UpgradeBus.instance.teleporterUpgradeLevel}");
		}

		public override void Unwind() {
			base.Unwind();

			UpgradeBus.instance.teleporterUpgrade = false;
			UpgradeBus.instance.teleporterUpgradeLevel = 0;
		}

		public string GetWorldBuildingText(bool shareStatus = false) {
			return WORLD_BUILDING_TEXT;
		}

		public override string GetDisplayInfo(int initialPrice = -1, int maxLevels = -1, int[] incrementalPrices = null) {
			Func<int, float> infoFunction = level => UpgradeBus.instance.teleporterUpgradeLevel;
			string infoFormat = AssetBundleHandler.GetInfoFromJSON(UPGRADE_NAME);
			return Tools.GenerateInfoForUpgrade(infoFormat, initialPrice, incrementalPrices, infoFunction);
		}
	}
}