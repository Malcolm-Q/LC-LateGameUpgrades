using CSync.Lib;
using MoreShipUpgrades.Configuration;
using MoreShipUpgrades.Configuration.Interfaces;
using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc.Upgrades;
using MoreShipUpgrades.UI.TerminalNodes;
using MoreShipUpgrades.UpgradeComponents.Interfaces;
using System.Text;
using UnityEngine;

namespace MoreShipUpgrades.UpgradeComponents.TierUpgrades.Items.Zapgun
{
    internal class AluminiumCoils : TierUpgrade, IUpgradeWorldBuilding
    {
        internal const string UPGRADE_NAME = "Aluminium Coils";
        internal const string DEFAULT_PRICES = "750,600, 800, 1000";
        internal const string WORLD_BUILDING_TEXT = "\n\nOn-the-job training package that instructs your crew on how to more safely and efficiently wield the Zap Gun.\n\n";
        internal override void Start()
        {
            upgradeName = UPGRADE_NAME;
            overridenUpgradeName = GetConfiguration().AluminiumCoilConfiguration.OverrideName;
            base.Start();
        }
        public static float ApplyDifficultyDecrease(float defaultDifficulty)
        {
            ITierMultipleEffectUpgradeConfiguration<int,float> config = GetConfiguration().AluminiumCoilConfiguration;
            if (!config.Enabled) return defaultDifficulty;
            if (!GetActiveUpgrade(UPGRADE_NAME)) return defaultDifficulty;
            (SyncedEntry<int>, SyncedEntry<int>) difficultyPair = config.GetEffectPair(0);
            float multiplier = 1f - ((difficultyPair.Item1.Value + (GetUpgradeLevel(UPGRADE_NAME) * difficultyPair.Item2.Value)) / 100f);
            return Mathf.Clamp(defaultDifficulty * multiplier, 0f, defaultDifficulty);
        }

        public static float ApplyCooldownDecrease(float defaultCooldown)
        {
            ITierMultipleEffectUpgradeConfiguration<int, float> config = GetConfiguration().AluminiumCoilConfiguration;
            if (!config.Enabled) return defaultCooldown;
            if (!GetActiveUpgrade(UPGRADE_NAME)) return defaultCooldown;
            (SyncedEntry<int>, SyncedEntry<int>) cooldownPair = config.GetEffectPair(1);
            float multiplier = 1f - ((cooldownPair.Item1.Value + (GetUpgradeLevel(UPGRADE_NAME) * cooldownPair.Item2.Value)) / 100f);
            return Mathf.Clamp(defaultCooldown * multiplier, 0f, defaultCooldown);
        }

        public static float ApplyIncreasedStunTimer(float defaultStunTimer)
        {
            ITierMultipleEffectUpgradeConfiguration<int, float> config = GetConfiguration().AluminiumCoilConfiguration;
            if (!config.Enabled) return defaultStunTimer;
            if (!GetActiveUpgrade(UPGRADE_NAME)) return defaultStunTimer;
            (SyncedEntry<float>, SyncedEntry<float>) stunTimerPair = config.GetSecondEffectPair(0);
            float additionalStunTimer = stunTimerPair.Item1.Value + (GetUpgradeLevel(UPGRADE_NAME) * stunTimerPair.Item2.Value);
            return defaultStunTimer + additionalStunTimer;
        }

        public static float ApplyIncreasedStunRange(float defaultRange)
        {
            ITierMultipleEffectUpgradeConfiguration<int, float> config = GetConfiguration().AluminiumCoilConfiguration;
            if (!config.Enabled) return defaultRange;
            if (!GetActiveUpgrade(UPGRADE_NAME)) return defaultRange;
            (SyncedEntry<float>, SyncedEntry<float>) rangePair = config.GetSecondEffectPair(1);
            float AdditionalRange = rangePair.Item1.Value + (GetUpgradeLevel(UPGRADE_NAME) * rangePair.Item2.Value);
            return defaultRange + AdditionalRange;
        }
        string GetAluminiumCoilsInfo(int level, int price)
        {
            static float difficultyInfo(int level)
            {
                ITierMultipleEffectUpgradeConfiguration<int, float> config = GetConfiguration().AluminiumCoilConfiguration;
                (SyncedEntry<int>, SyncedEntry<int>) difficultyPair = config.GetEffectPair(0);
                return difficultyPair.Item1.Value + (level * difficultyPair.Item2.Value);
            }
            static float rangeInfo(int level)
            {
                ITierMultipleEffectUpgradeConfiguration<int, float> config = GetConfiguration().AluminiumCoilConfiguration;
                (SyncedEntry<float>, SyncedEntry<float>) rangePair = config.GetSecondEffectPair(1);
                return rangePair.Item1.Value + (level * rangePair.Item2.Value);
            }
            static float stunTimerInfo(int level)
            {
                ITierMultipleEffectUpgradeConfiguration<int, float> config = GetConfiguration().AluminiumCoilConfiguration;
                (SyncedEntry<float>, SyncedEntry<float>) stunTimerPair = config.GetSecondEffectPair(0);
                return stunTimerPair.Item1.Value + (level * stunTimerPair.Item2.Value);
            }
            static float cooldownInfo(int level)
            {
                ITierMultipleEffectUpgradeConfiguration<int, float> config = GetConfiguration().AluminiumCoilConfiguration;
                (SyncedEntry<int>, SyncedEntry<int>) cooldownPair = config.GetEffectPair(1);
                return cooldownPair.Item1.Value + (level * cooldownPair.Item2.Value);
            }
            StringBuilder sb = new();
            sb.Append($"LVL {level} - ${price}: Upgrades to zap gun:\n");
            sb.Append($"- Increases zap gun's range by {rangeInfo(level - 1) / 13f * 100f:F0}%\n");
            sb.Append($"- Stun time increased by {stunTimerInfo(level - 1)} seconds\n");
            sb.Append($"- Decreases the minigame's difficulty by {difficultyInfo(level - 1):F0}%\n");
            sb.Append($"- Decreases the zap gun's cooldown by {cooldownInfo(level - 1):F0}%\n");
            sb.Append('\n');
            return sb.ToString();
        }
        public override string GetDisplayInfo(int initialPrice = -1, int maxLevels = -1, int[] incrementalPrices = null)
        {
            StringBuilder sb = new();
            sb.Append(GetAluminiumCoilsInfo(1, initialPrice));
            for (int i = 0; i < maxLevels; i++)
                sb.Append(GetAluminiumCoilsInfo(i + 2, incrementalPrices[i]));
            return sb.ToString();
        }

        public override bool CanInitializeOnStart
        {
            get
            {
                ITierMultipleEffectUpgradeConfiguration<int, float> config = GetConfiguration().AluminiumCoilConfiguration;
                string[] prices = config.Prices.Value.Split(',');
                return prices.Length == 0 || (prices.Length == 1 && (prices[0].Length == 0 || prices[0] == "0"));
            }
        }

        public new static void RegisterUpgrade()
        {
            SetupGenericPerk<AluminiumCoils>(UPGRADE_NAME);
        }

        public new static (string, string[]) RegisterScrapToUpgrade()
        {
            return (UPGRADE_NAME, GetConfiguration().AluminiumCoilConfiguration.ItemProgressionItems.Value.Split(","));
        }
        public new static CustomTerminalNode RegisterTerminalNode()
        {
            return UpgradeBus.Instance.SetupMultiplePurchaseableTerminalNode(UPGRADE_NAME, GetConfiguration().AluminiumCoilConfiguration);
        }

        public string GetWorldBuildingText(bool shareStatus = false)
        {
            return WORLD_BUILDING_TEXT;
        }
    }
}
