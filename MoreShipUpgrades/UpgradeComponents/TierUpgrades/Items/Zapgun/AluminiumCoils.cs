using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc;
using MoreShipUpgrades.Misc.Upgrades;
using MoreShipUpgrades.UI.TerminalNodes;
using MoreShipUpgrades.UpgradeComponents.Interfaces;
using System;
using System.Text;
using UnityEngine;

namespace MoreShipUpgrades.UpgradeComponents.TierUpgrades.Items.Zapgun
{
    internal class AluminiumCoils : TierUpgrade, IUpgradeWorldBuilding
    {
        internal const string UPGRADE_NAME = "Aluminium Coils";
        internal const string DEFAULT_PRICES = "600, 800, 1000";
        internal const string WORLD_BUILDING_TEXT = "\n\nOn-the-job training package that instructs your crew on how to more safely and efficiently wield the Zap Gun.\n\n";
        internal override void Start()
        {
            upgradeName = UPGRADE_NAME;
            overridenUpgradeName = GetConfiguration().ALUMINIUM_COILS_OVERRIDE_NAME;
            base.Start();
        }
        public static float ApplyDifficultyDecrease(float defaultDifficulty)
        {
            LategameConfiguration config = GetConfiguration();
            if (!config.ALUMINIUM_COILS_ENABLED) return defaultDifficulty;
            if (!GetActiveUpgrade(UPGRADE_NAME)) return defaultDifficulty;
            float multiplier = 1f - ((config.ALUMINIUM_COILS_INITIAL_DIFFICULTY_DECREASE.Value + (GetUpgradeLevel(UPGRADE_NAME) * config.ALUMINIUM_COILS_INCREMENTAL_DIFFICULTY_DECREASE.Value)) / 100f);
            return Mathf.Clamp(defaultDifficulty * multiplier, 0f, defaultDifficulty);
        }

        public static float ApplyCooldownDecrease(float defaultCooldown)
        {
            LategameConfiguration config = GetConfiguration();
            if (!config.ALUMINIUM_COILS_ENABLED) return defaultCooldown;
            if (!GetActiveUpgrade(UPGRADE_NAME)) return defaultCooldown;
            float multiplier = 1f - ((config.ALUMINIUM_COILS_INITIAL_COOLDOWN_DECREASE.Value + (GetUpgradeLevel(UPGRADE_NAME) * config.ALUMINIUM_COILS_INCREMENTAL_COOLDOWN_DECREASE.Value)) / 100f);
            return Mathf.Clamp(defaultCooldown * multiplier, 0f, defaultCooldown);
        }

        public static float ApplyIncreasedStunTimer(float defaultStunTimer)
        {
            LategameConfiguration config = GetConfiguration();
            if (!config.ALUMINIUM_COILS_ENABLED) return defaultStunTimer;
            if (!GetActiveUpgrade(UPGRADE_NAME)) return defaultStunTimer;
            float additionalStunTimer = config.ALUMINIUM_COILS_INITIAL_STUN_TIMER_INCREASE.Value + (GetUpgradeLevel(UPGRADE_NAME) * config.ALUMINIUM_COILS_INCREMENTAL_STUN_TIMER_INCREASE.Value);
            return defaultStunTimer + additionalStunTimer;
        }

        public static float ApplyIncreasedStunRange(float defaultRange)
        {
            LategameConfiguration config = GetConfiguration();
            if (!config.ALUMINIUM_COILS_ENABLED) return defaultRange;
            if (!GetActiveUpgrade(UPGRADE_NAME)) return defaultRange;
            float AdditionalRange = config.ALUMINIUM_COILS_INITIAL_RANGE_INCREASE.Value + (GetUpgradeLevel(UPGRADE_NAME) * config.ALUMINIUM_COILS_INCREMENTAL_RANGE_INCREASE.Value);
            return defaultRange + AdditionalRange;
        }
        string GetAluminiumCoilsInfo(int level, int price)
        {
            static float difficultyInfo(int level)
            {
                LategameConfiguration config = GetConfiguration();
                return config.ALUMINIUM_COILS_INITIAL_DIFFICULTY_DECREASE.Value + (level * config.ALUMINIUM_COILS_INCREMENTAL_DIFFICULTY_DECREASE.Value);
            }
            static float rangeInfo(int level)
            {
                LategameConfiguration config = GetConfiguration();
                return config.ALUMINIUM_COILS_INITIAL_RANGE_INCREASE.Value + (level * config.ALUMINIUM_COILS_INCREMENTAL_RANGE_INCREASE.Value);
            }
            static float stunTimerInfo(int level)
            {
                LategameConfiguration config = GetConfiguration();
                return config.ALUMINIUM_COILS_INITIAL_STUN_TIMER_INCREASE.Value + (level * config.ALUMINIUM_COILS_INCREMENTAL_STUN_TIMER_INCREASE.Value);
            }
            static float cooldownInfo(int level)
            {
                LategameConfiguration config = GetConfiguration();
                return config.ALUMINIUM_COILS_INITIAL_COOLDOWN_DECREASE.Value + (level * config.ALUMINIUM_COILS_INCREMENTAL_COOLDOWN_DECREASE.Value);
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
                LategameConfiguration config = GetConfiguration();
                string[] prices = config.ALUMINIUM_COILS_PRICES.Value.Split(',');
                return config.ALUMINIUM_COILS_PRICE.Value <= 0 && prices.Length == 1 && (prices[0].Length == 0 || prices[0] == "0");
            }
        }

        public new static void RegisterUpgrade()
        {
            SetupGenericPerk<AluminiumCoils>(UPGRADE_NAME);
        }

        public new static (string, string[]) RegisterScrapToUpgrade()
        {
            return (UPGRADE_NAME, GetConfiguration().ALUMINIUM_COILS_ITEM_PROGRESSION_ITEMS.Value.Split(","));
        }
        public new static CustomTerminalNode RegisterTerminalNode()
        {
            LategameConfiguration configuration = GetConfiguration();

            return UpgradeBus.Instance.SetupMultiplePurchasableTerminalNode(UPGRADE_NAME,
                                                configuration.SHARED_UPGRADES.Value || !configuration.ALUMINIUM_COILS_INDIVIDUAL.Value,
                                                configuration.ALUMINIUM_COILS_ENABLED.Value,
                                                configuration.ALUMINIUM_COILS_PRICE.Value,
                                                UpgradeBus.ParseUpgradePrices(configuration.ALUMINIUM_COILS_PRICES.Value),
                                                configuration.OVERRIDE_UPGRADE_NAMES ? configuration.ALUMINIUM_COILS_OVERRIDE_NAME : "");
        }

        public string GetWorldBuildingText(bool shareStatus = false)
        {
            return WORLD_BUILDING_TEXT;
        }
    }
}
