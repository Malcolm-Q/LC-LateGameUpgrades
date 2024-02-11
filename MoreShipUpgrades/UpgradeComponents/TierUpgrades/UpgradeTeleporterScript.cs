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
		internal override void Start() {
			upgradeName = UPGRADE_NAME;
			logger = new LGULogger(UPGRADE_NAME);
			base.Start();
		}

		public string GetWorldBuildingText(bool shareStatus = false) {
			return WORLD_BUILDING_TEXT;
		}

		public override string GetDisplayInfo(int initialPrice = -1, int maxLevels = -1, int[] incrementalPrices = null) {
			return AssetBundleHandler.GetInfoFromJSON(UPGRADE_NAME);
		}

		public static int GetTPUpgradeLevel() {
			int enabled = GetActiveUpgrade(UPGRADE_NAME) ? 1 : 0;
			return enabled + GetUpgradeLevel(UPGRADE_NAME);
		}
	}
}