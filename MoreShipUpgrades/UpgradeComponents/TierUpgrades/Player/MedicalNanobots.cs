using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc;
using MoreShipUpgrades.Misc.Upgrades;
using MoreShipUpgrades.Misc.Util;
using MoreShipUpgrades.UI.TerminalNodes;
using MoreShipUpgrades.UpgradeComponents.TierUpgrades.AttributeUpgrades;
using UnityEngine;

namespace MoreShipUpgrades.UpgradeComponents.TierUpgrades.Player
{
    internal class MedicalNanobots : TierUpgrade
    {
        internal const string UPGRADE_NAME = "Medical Nanobots";
        internal const string DEFAULT_PRICES = "400,500,750";

        internal override void Start()
        {
            upgradeName = UPGRADE_NAME;
            overridenUpgradeName = GetConfiguration().MEDICAL_NANOBOTS_OVERRIDE_NAME;
            base.Start();
        }
        public override string GetDisplayInfo(int initialPrice = -1, int maxLevels = -1, int[] incrementalPrices = null)
        {
            static float infoFunction(int level)
            {
                LategameConfiguration config = GetConfiguration();
                return config.MEDICAL_NANOBOTS_INITIAL_HEALTH_REGEN_CAP_INCREASE.Value + (level * config.MEDICAL_NANOBOTS_INCREMENTAL_HEALTH_REGEN_CAP_INCREASE.Value);
            }
            const string infoFormat = "LVL {0} - ${1} - Increases the player's health regeneration cap by {2}%\n";
            return Tools.GenerateInfoForUpgrade(infoFormat, initialPrice, incrementalPrices, infoFunction);
        }
        public static int GetIncreasedHealthRegeneration(int defaultValue)
        {
            LategameConfiguration config = GetConfiguration();
            if (!config.MEDICAL_NANOBOTS_ENABLED) return defaultValue;
            if (!GetActiveUpgrade(UPGRADE_NAME)) return defaultValue;
            float percentage = (config.MEDICAL_NANOBOTS_INITIAL_HEALTH_REGEN_CAP_INCREASE + (GetUpgradeLevel(UPGRADE_NAME) * config.MEDICAL_NANOBOTS_INCREMENTAL_HEALTH_REGEN_CAP_INCREASE))/100f;
            return Mathf.Clamp(defaultValue + (int)(defaultValue*percentage), defaultValue, Stimpack.CheckForAdditionalHealth(100));
        }

        public override bool CanInitializeOnStart
        {
            get
            {
                LategameConfiguration config = GetConfiguration();
                string[] prices = config.MEDICAL_NANOBOTS_PRICES.Value.Split(',');
                return config.MEDICAL_NANOBOTS_PRICE.Value <= 0 && prices.Length == 1 && (prices[0].Length == 0 || prices[0] == "0");
            }
        }
        public new static (string, string[]) RegisterScrapToUpgrade()
        {
            return (UPGRADE_NAME, GetConfiguration().MEDICAL_NANOBOTS_ITEM_PROGRESSION_ITEMS.Value.Split(","));
        }
        public new static void RegisterUpgrade()
        {
            GameObject prefab = LethalLib.Modules.NetworkPrefabs.CreateNetworkPrefab(UPGRADE_NAME);
            prefab.AddComponent<MedicalNanobots>();
            LethalLib.Modules.NetworkPrefabs.RegisterNetworkPrefab(prefab);
            Plugin.networkPrefabs[UPGRADE_NAME] = prefab;
        }
        public new static CustomTerminalNode RegisterTerminalNode()
        {
            LategameConfiguration configuration = GetConfiguration();

            return UpgradeBus.Instance.SetupMultiplePurchasableTerminalNode(UPGRADE_NAME,
                                                configuration.SHARED_UPGRADES || !configuration.MEDICAL_NANOBOTS_INDIVIDUAL,
                                                configuration.MEDICAL_NANOBOTS_ENABLED,
                                                configuration.MEDICAL_NANOBOTS_PRICE,
                                                UpgradeBus.ParseUpgradePrices(configuration.MEDICAL_NANOBOTS_PRICES),
                                                configuration.OVERRIDE_UPGRADE_NAMES ? configuration.MEDICAL_NANOBOTS_OVERRIDE_NAME : "",
                                                Plugin.networkPrefabs[UPGRADE_NAME]);
        }
    }
}
