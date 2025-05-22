using CSync.Lib;
using MoreShipUpgrades.Configuration.Upgrades.Custom;
using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc;
using MoreShipUpgrades.Misc.Upgrades;
using MoreShipUpgrades.UI.TerminalNodes;
using MoreShipUpgrades.UpgradeComponents.Interfaces;
using System.Text;
using UnityEngine;

namespace MoreShipUpgrades.UpgradeComponents.TierUpgrades
{
    class BiggerLungs : TierUpgrade, IUpgradeWorldBuilding
    {
        public const string UPGRADE_NAME = "Bigger Lungs";
        internal const string PRICES_DEFAULT = "600,350,450,550";
        internal const string WORLD_BUILDING_TEXT = "\n\nService package for {0}." +
            " Opting into every maintenance procedure will arrange for your suit's pipes to be cleaned and repaired, filters re-issued," +
            " and DRM removed from the integrated air conditioning system.\n\n";

        void Awake()
        {
            upgradeName = UPGRADE_NAME;
            overridenUpgradeName = GetConfiguration().BiggerLungsConfiguration.OverrideName;
        }
        public static float ApplyPossibleIncreasedStaminaRegen(float regenValue)
        {
            BiggerLungsUpgradeConfiguration config = GetConfiguration().BiggerLungsConfiguration;
            if (!config.Enabled.Value) return regenValue;
            if (!GetActiveUpgrade(UPGRADE_NAME) || GetUpgradeLevel(UPGRADE_NAME) < config.StaminaRegenerationLevel.Value - 1) return regenValue;
            (SyncedEntry<float>, SyncedEntry<float>) staminaRegenPair = config.GetEffectPair(1);
            return regenValue * Mathf.Clamp(staminaRegenPair.Item1.Value + (staminaRegenPair.Item2 * Mathf.Abs(GetUpgradeLevel(UPGRADE_NAME) - config.StaminaRegenerationLevel.Value - 1)), 0f, 10f);
        }
        public static float GetAdditionalStaminaTime(float defaultValue)
        {
            BiggerLungsUpgradeConfiguration config = GetConfiguration().BiggerLungsConfiguration;
            if (!config.Enabled) return defaultValue;
            if (!GetActiveUpgrade(UPGRADE_NAME)) return defaultValue;
            (SyncedEntry<float>, SyncedEntry<float>) additionalStaminaPair = config.GetEffectPair(0);
            float additionalValue = additionalStaminaPair.Item1 + (GetUpgradeLevel(UPGRADE_NAME) * additionalStaminaPair.Item2);
            return Mathf.Clamp(defaultValue + additionalValue, defaultValue, float.MaxValue);
        }

        public static float ApplyPossibleReducedJumpStaminaCost(float jumpCost)
        {
            BiggerLungsUpgradeConfiguration config = GetConfiguration().BiggerLungsConfiguration;
            if (!config.Enabled.Value) return jumpCost;
            if (!GetActiveUpgrade(UPGRADE_NAME) || GetUpgradeLevel(UPGRADE_NAME) < config.JumpReductionLevel.Value - 1) return jumpCost;
            (SyncedEntry<float>, SyncedEntry<float>) jumpReductionPair = config.GetEffectPair(2);
            return jumpCost * Mathf.Clamp(jumpReductionPair.Item1.Value - (jumpReductionPair.Item2 * Mathf.Abs(GetUpgradeLevel(UPGRADE_NAME) - config.JumpReductionLevel.Value - 1)), 0f, 10f);
        }
        public string GetWorldBuildingText(bool shareStatus = false)
        {
            return string.Format(WORLD_BUILDING_TEXT, shareStatus ? "your crew's suit oxigen delivery systems" : "your suit's oxygen delivery system");
        }

        string GetBiggerlungsInfo(int level, int price)
        {
            static float infoFunction(int level)
            {
                BiggerLungsUpgradeConfiguration config = GetConfiguration().BiggerLungsConfiguration;
                (SyncedEntry<float>, SyncedEntry<float>) additionalStaminaPair = config.GetEffectPair(0);
                return additionalStaminaPair.Item1.Value + (level * additionalStaminaPair.Item2.Value);
            }
            static float costReductionInfo(int level)
            {
                BiggerLungsUpgradeConfiguration config = GetConfiguration().BiggerLungsConfiguration;
                (SyncedEntry<float>, SyncedEntry<float>) jumpReductionPair = config.GetEffectPair(2);
                return 1f - (jumpReductionPair.Item1.Value - (level * jumpReductionPair.Item2.Value));
            }
            static float staminaRegenerationInfo(int level)
            {
                BiggerLungsUpgradeConfiguration config = GetConfiguration().BiggerLungsConfiguration;
                (SyncedEntry<float>, SyncedEntry<float>) staminaRegenPair = config.GetEffectPair(1);
                return staminaRegenPair.Item1.Value + (level * staminaRegenPair.Item2.Value) - 1f;
            }
            StringBuilder sb = new();
            sb.AppendFormat(AssetBundleHandler.GetInfoFromJSON(UPGRADE_NAME), level, GetUpgradePrice(price, GetConfiguration().BiggerLungsConfiguration.PurchaseMode), infoFunction(level - 1));
            BiggerLungsUpgradeConfiguration config = GetConfiguration().BiggerLungsConfiguration;
            if (level >= config.StaminaRegenerationLevel) sb.Append($"Stamina regeneration is increased by {Mathf.FloorToInt(staminaRegenerationInfo(level) * 100f)}%\n");
            if (level >= config.JumpReductionLevel.Value) sb.Append($"Stamina used when jumping is reduced by {Mathf.FloorToInt(costReductionInfo(level) * 100f)}%\n");
            return sb.ToString();
        }

        public override string GetDisplayInfo(int initialPrice = -1, int maxLevels = -1, int[] incrementalPrices = null)
        {
            StringBuilder sb = new();
            sb.Append(GetBiggerlungsInfo(1, initialPrice));
            for (int i = 0; i < maxLevels; i++)
                sb.Append(GetBiggerlungsInfo(i + 2, incrementalPrices[i]));
            return sb.ToString();
        }
        public override bool CanInitializeOnStart
        {
            get
            {
                BiggerLungsUpgradeConfiguration upgradeConfig = GetConfiguration().BiggerLungsConfiguration;
                string[] prices = upgradeConfig.Prices.Value.Split(',');
                return prices.Length == 0 || (prices.Length == 1 && (prices[0].Length == 0 || prices[0] == "0"));
            }
        }

        public new static (string, string[]) RegisterScrapToUpgrade()
        {
            return (UPGRADE_NAME, GetConfiguration().BiggerLungsConfiguration.ItemProgressionItems.Value.Split(","));
        }
        public new static void RegisterUpgrade()
        {
            SetupGenericPerk<BiggerLungs>(UPGRADE_NAME);
        }
        public new static CustomTerminalNode RegisterTerminalNode()
        {
            return UpgradeBus.Instance.SetupMultiplePurchaseableTerminalNode(UPGRADE_NAME, GetConfiguration().BiggerLungsConfiguration);
        }
    }
}
