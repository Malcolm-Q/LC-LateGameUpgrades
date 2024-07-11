using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc.TerminalNodes;
using MoreShipUpgrades.Misc;
using MoreShipUpgrades.Misc.Upgrades;
using MoreShipUpgrades.Misc.Util;
using UnityEngine;

namespace MoreShipUpgrades.UpgradeComponents.TierUpgrades.Vehicle
{
    internal class FluffySeats : TierUpgrade
    {
        internal const string UPGRADE_NAME = "Fluffy Seats";
        internal const string PRICES_DEFAULT = "100,150,200";

        internal override void Start()
        {
            upgradeName = UPGRADE_NAME;
            overridenUpgradeName = UpgradeBus.Instance.PluginConfiguration.FLUFFY_SEATS_OVERRIDE_NAME;
            base.Start();
        }
        public static float ComputePlayerDamageMitigation()
        {
            int percentage = UpgradeBus.Instance.PluginConfiguration.FLUFFY_SEATS_DAMAGE_MITIGATION_INITIAL_INCREASE + (GetUpgradeLevel(UPGRADE_NAME) * UpgradeBus.Instance.PluginConfiguration.FLUFFY_SEATS_DAMAGE_MITIGATION_INCREMENTAL_INCREASE);
            return (100f - percentage) / 100f;
        }
        public static int GetPlayerDamageMitigation(int defaultValue)
        {
            if (!UpgradeBus.Instance.PluginConfiguration.FLUFFY_SEATS_ENABLED) return defaultValue;
            if (!GetActiveUpgrade(UPGRADE_NAME)) return defaultValue;
            float multiplier = ComputePlayerDamageMitigation();
            return Mathf.Clamp((int)(defaultValue * multiplier), 0, defaultValue);
        }
        public override bool CanInitializeOnStart
        {
            get
            {
                string[] prices = UpgradeBus.Instance.PluginConfiguration.FLUFFY_SEATS_PRICES.Value.Split(',');
                return UpgradeBus.Instance.PluginConfiguration.FLUFFY_SEATS_PRICE.Value <= 0 && prices.Length == 1 && (prices[0] == "" || prices[0] == "0");
            }
        }
        public override string GetDisplayInfo(int initialPrice = -1, int maxLevels = -1, int[] incrementalPrices = null)
        {
            static float infoFunction(int level) => UpgradeBus.Instance.PluginConfiguration.FLUFFY_SEATS_DAMAGE_MITIGATION_INITIAL_INCREASE.Value + level * UpgradeBus.Instance.PluginConfiguration.FLUFFY_SEATS_DAMAGE_MITIGATION_INCREMENTAL_INCREASE.Value;
            const string infoFormat = "LVL {0} - ${1} - Player damage is reduced by {2}% when bumping too hard with the Company Cruiser Vehicle.\n";
            return Tools.GenerateInfoForUpgrade(infoFormat, initialPrice, incrementalPrices, infoFunction);
        }
        public new static (string, string[]) RegisterScrapToUpgrade()
        {
            return (UPGRADE_NAME, UpgradeBus.Instance.PluginConfiguration.FLUFFY_SEATS_ITEM_PROGRESSION_ITEMS.Value.Split(","));
        }
        public new static void RegisterUpgrade()
        {
            SetupGenericPerk<FluffySeats>(UPGRADE_NAME);
        }
        public new static CustomTerminalNode RegisterTerminalNode()
        {
            LategameConfiguration configuration = UpgradeBus.Instance.PluginConfiguration;

            return UpgradeBus.Instance.SetupMultiplePurchasableTerminalNode(UPGRADE_NAME,
                                                shareStatus: true,
                                                configuration.FLUFFY_SEATS_ENABLED.Value,
                                                configuration.FLUFFY_SEATS_PRICE.Value,
                                                UpgradeBus.ParseUpgradePrices(configuration.FLUFFY_SEATS_PRICES.Value),
                                                configuration.OVERRIDE_UPGRADE_NAMES ? configuration.FLUFFY_SEATS_OVERRIDE_NAME : "");
        }
    }
}
