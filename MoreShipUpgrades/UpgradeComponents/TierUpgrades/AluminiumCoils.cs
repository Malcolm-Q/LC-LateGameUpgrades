using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc;
using MoreShipUpgrades.Misc.TerminalNodes;
using MoreShipUpgrades.Misc.Upgrades;
using MoreShipUpgrades.Misc.Util;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace MoreShipUpgrades.UpgradeComponents.TierUpgrades
{
    internal class AluminiumCoils : TierUpgrade
    {
        internal const string UPGRADE_NAME = "Aluminium Coils";
        internal const string DEFAULT_PRICES = "600, 800, 1000";
        internal override void Start()
        {
            upgradeName = UPGRADE_NAME;
            overridenUpgradeName = UpgradeBus.Instance.PluginConfiguration.ALUMINIUM_COILS_OVERRIDE_NAME;
            base.Start();
        }
        public static float ApplyDifficultyDecrease(float defaultDifficulty)
        {
            if (!GetActiveUpgrade(UPGRADE_NAME)) return defaultDifficulty;
            float multiplier = 1f - ((UpgradeBus.Instance.PluginConfiguration.ALUMINIUM_COILS_INITIAL_DIFFICULTY_DECREASE.Value + (GetUpgradeLevel(UPGRADE_NAME) * UpgradeBus.Instance.PluginConfiguration.ALUMINIUM_COILS_INCREMENTAL_DIFFICULTY_DECREASE.Value)) / 100f);
            return Mathf.Clamp(defaultDifficulty * multiplier, 0f, defaultDifficulty);
        }

        public static float ApplyCooldownDecrease(float defaultCooldown)
        {
            if (!GetActiveUpgrade(UPGRADE_NAME)) return defaultCooldown;
            float multiplier = 1f - ((UpgradeBus.Instance.PluginConfiguration.ALUMINIUM_COILS_INITIAL_COOLDOWN_DECREASE.Value + (GetUpgradeLevel(UPGRADE_NAME) * UpgradeBus.Instance.PluginConfiguration.ALUMINIUM_COILS_INCREMENTAL_COOLDOWN_DECREASE.Value)) / 100f);
            return Mathf.Clamp(defaultCooldown * multiplier, 0f, defaultCooldown);
        }

        public static float ApplyIncreasedStunTimer(float defaultStunTimer)
        {
            if (!GetActiveUpgrade(UPGRADE_NAME)) return defaultStunTimer;
            float additionalStunTimer = UpgradeBus.Instance.PluginConfiguration.ALUMINIUM_COILS_INITIAL_STUN_TIMER_INCREASE.Value + (GetUpgradeLevel(UPGRADE_NAME) * UpgradeBus.Instance.PluginConfiguration.ALUMINIUM_COILS_INCREMENTAL_STUN_TIMER_INCREASE.Value);
            return defaultStunTimer + additionalStunTimer;
        }

        public static float ApplyIncreasedStunRange(float defaultRange)
        {
            if (!GetActiveUpgrade(UPGRADE_NAME)) return defaultRange;
            float AdditionalRange = UpgradeBus.Instance.PluginConfiguration.ALUMINIUM_COILS_INITIAL_RANGE_INCREASE.Value + (GetUpgradeLevel(UPGRADE_NAME) * UpgradeBus.Instance.PluginConfiguration.ALUMINIUM_COILS_INCREMENTAL_RANGE_INCREASE.Value);
            return defaultRange + AdditionalRange;
        }
        string GetAluminiumCoilsInfo(int level, int price)
        {
            System.Func<int, float> difficultyInfo = level => UpgradeBus.Instance.PluginConfiguration.ALUMINIUM_COILS_INITIAL_DIFFICULTY_DECREASE.Value + (level * UpgradeBus.Instance.PluginConfiguration.ALUMINIUM_COILS_INCREMENTAL_DIFFICULTY_DECREASE.Value);
            System.Func<int, float> rangeInfo = level => UpgradeBus.Instance.PluginConfiguration.ALUMINIUM_COILS_INITIAL_RANGE_INCREASE.Value + (level * UpgradeBus.Instance.PluginConfiguration.ALUMINIUM_COILS_INCREMENTAL_RANGE_INCREASE.Value);
            System.Func<int, float> stunTimerInfo = level => UpgradeBus.Instance.PluginConfiguration.ALUMINIUM_COILS_INITIAL_STUN_TIMER_INCREASE.Value + (level * UpgradeBus.Instance.PluginConfiguration.ALUMINIUM_COILS_INCREMENTAL_STUN_TIMER_INCREASE.Value);
            System.Func<int, float> cooldownInfo = level => UpgradeBus.Instance.PluginConfiguration.ALUMINIUM_COILS_INITIAL_COOLDOWN_DECREASE.Value + (level * UpgradeBus.Instance.PluginConfiguration.ALUMINIUM_COILS_INCREMENTAL_COOLDOWN_DECREASE.Value);
            StringBuilder sb = new StringBuilder();
            sb.Append($"LVL {level} - ${price}: Upgrades to zap gun:\n");
            sb.Append($"- Increases zap gun's range by {(rangeInfo(level-1) / 13f * 100f):F0}%\n");
            sb.Append($"- Stun time increased by {stunTimerInfo(level-1)} seconds\n");
            sb.Append($"- Decreases the minigame's difficulty by {(difficultyInfo(level - 1)):F0}%\n");
            sb.Append($"- Decreases the zap gun's cooldown by {cooldownInfo(level - 1):F0}%\n");
            sb.Append('\n');
            return sb.ToString();
        }
        public override string GetDisplayInfo(int initialPrice = -1, int maxLevels = -1, int[] incrementalPrices = null)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(GetAluminiumCoilsInfo(1, initialPrice));
            for (int i = 0; i < maxLevels; i++)
                sb.Append(GetAluminiumCoilsInfo(i + 2, incrementalPrices[i]));
            return sb.ToString();
        }

        public override bool CanInitializeOnStart
        {
            get
            {
                string[] prices = UpgradeBus.Instance.PluginConfiguration.ALUMINIUM_COILS_PRICES.Value.Split(',');
                bool free = UpgradeBus.Instance.PluginConfiguration.ALUMINIUM_COILS_PRICE.Value <= 0 && prices.Length == 1 && (prices[0] == "" || prices[0] == "0");
                return free;
            }
        }

        public new static void RegisterUpgrade()
        {
            SetupGenericPerk<AluminiumCoils>(UPGRADE_NAME);
        }
        public new static CustomTerminalNode RegisterTerminalNode()
        {
            LategameConfiguration configuration = UpgradeBus.Instance.PluginConfiguration;

            CustomTerminalNode node = UpgradeBus.Instance.SetupMultiplePurchasableTerminalNode(UPGRADE_NAME,
                                                configuration.SHARED_UPGRADES.Value || !configuration.ALUMINIUM_COILS_INDIVIDUAL.Value,
                                                configuration.ALUMINIUM_COILS_ENABLED.Value,
                                                configuration.ALUMINIUM_COILS_PRICE.Value,
                                                UpgradeBus.ParseUpgradePrices(configuration.ALUMINIUM_COILS_PRICES.Value),
                                                configuration.OVERRIDE_UPGRADE_NAMES ? configuration.ALUMINIUM_COILS_OVERRIDE_NAME : "");
            ItemProgressionManager.AddUpgrade(node, ["apparatus"]);
            return node;
        }
    }
}
