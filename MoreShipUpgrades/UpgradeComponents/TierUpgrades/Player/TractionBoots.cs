using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc.Upgrades;
using UnityEngine;
using MoreShipUpgrades.Misc.Util;
using MoreShipUpgrades.UI.TerminalNodes;
using MoreShipUpgrades.UpgradeComponents.Interfaces;
using MoreShipUpgrades.Configuration.Upgrades.Interfaces.TierUpgrades;

namespace MoreShipUpgrades.UpgradeComponents.TierUpgrades.Player
{
    internal class TractionBoots : TierUpgrade, IUpgradeWorldBuilding
    {
        internal const string UPGRADE_NAME = "Traction Boots";
        internal const string PRICES_DEFAULT = "100,100,150,250";
        internal const string WORLD_BUILDING_TEXT = "\n\nIn the 'GEAR' section of the Company Catalogue, the first ad on the first page is for these." +
            " They're magnetic cleats that hook onto the Proprietary Steel Toe & Heel Reinforcements on your boots." +
            " These things have been known to hook into grated metal flooring and come off without the wearer's notice, so be careful.\n\n";

        public string GetWorldBuildingText(bool shareStatus = false)
        {
            return WORLD_BUILDING_TEXT;
        }
        internal override void Start()
        {
            upgradeName = UPGRADE_NAME;
            overridenUpgradeName = GetConfiguration().TractionBootsConfiguration.OverrideName;
            base.Start();
        }

        public static float ComputeAdditionalTractionForce()
        {
            ITierEffectUpgradeConfiguration<int> config = GetConfiguration().TractionBootsConfiguration;
            return 1f + ((config.InitialEffect + (GetUpgradeLevel(UPGRADE_NAME) * config.IncrementalEffect)) / 100f);
        }

        public static float GetAdditionalTractionForce(float defaultValue)
        {
            if (!GetConfiguration().TractionBootsConfiguration.Enabled) return defaultValue;
            if (!GetActiveUpgrade(UPGRADE_NAME)) return defaultValue;
            float additionalTraction = ComputeAdditionalTractionForce();
            return Mathf.Clamp(additionalTraction * defaultValue, defaultValue, float.MaxValue);
        }

        public override string GetDisplayInfo(int initialPrice = -1, int maxLevels = -1, int[] incrementalPrices = null)
        {
            static float infoFunction(int level)
            {
                ITierEffectUpgradeConfiguration<int> config = GetConfiguration().TractionBootsConfiguration;
                return config.InitialEffect.Value + (level * config.IncrementalEffect.Value);
            }
            const string infoFormat = "LVL {0} - {1} - Increases the player's traction to the ground by {2}%\n";
            return Tools.GenerateInfoForUpgrade(infoFormat, initialPrice, incrementalPrices, infoFunction, purchaseMode: GetConfiguration().TractionBootsConfiguration.PurchaseMode);
        }
        public override bool CanInitializeOnStart
        {
            get
            {
                ITierEffectUpgradeConfiguration<int> config = GetConfiguration().TractionBootsConfiguration;
                string[] prices = config.Prices.Value.Split(',');
                return prices.Length == 0 || (prices.Length == 1 && (prices[0].Length == 0 || prices[0] == "0"));
            }
        }

        public new static (string, string[]) RegisterScrapToUpgrade()
        {
            return (UPGRADE_NAME, GetConfiguration().TractionBootsConfiguration.ItemProgressionItems.Value.Split(","));
        }
        public new static void RegisterUpgrade()
        {
            SetupGenericPerk<TractionBoots>(UPGRADE_NAME);
        }
        public new static CustomTerminalNode RegisterTerminalNode()
        {
            return UpgradeBus.Instance.SetupMultiplePurchaseableTerminalNode(UPGRADE_NAME, GetConfiguration().TractionBootsConfiguration);
        }
    }
}
