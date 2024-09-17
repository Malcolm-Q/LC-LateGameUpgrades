using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc;
using MoreShipUpgrades.Misc.Upgrades;
using MoreShipUpgrades.Misc.Util;
using MoreShipUpgrades.UI.TerminalNodes;
using System.Text;
using Unity.Netcode;
using UnityEngine;

namespace MoreShipUpgrades.UpgradeComponents.TierUpgrades.Ship
{
    public class QuantumDisruptor : TierUpgrade
    {
        public enum UpgradeModes
        {
            SlowdownTime,
            RevertTime,
        }
        public enum ResetModes
        {
            MoonLanding,
            MoonRerouting,
            NewQuota,
        }
        internal const string UPGRADE_NAME = "Quantum Disruptor";
        internal const string PRICES_DEFAULT = "1200,1500,1800";
        internal static QuantumDisruptor Instance { get; set; }

        internal static UpgradeModes CurrentMode
        {
            get
            {
                return GetConfiguration().QUANTUM_DISRUPTOR_UPGRADE_MODE;
            }
        }
        internal static ResetModes CurrentResetMode
        {
            get
            {
                return GetConfiguration().QUANTUM_DISRUPTOR_RESET_MODE.Value;
            }
        }
        internal int availableUsages = -1;
        internal int currentUsages = 0;
        internal int hoursToReduce = -1;
        static void SetInstance(QuantumDisruptor instance)
        {
            Instance = instance;
        }
        void Awake()
        {
            SetInstance(this);
            upgradeName = UPGRADE_NAME;
            overridenUpgradeName = GetConfiguration().QUANTUM_DISRUPTOR_OVERRIDE_NAME;
        }

        public override void Unwind()
        {
            switch (CurrentMode)
            {
                case UpgradeModes.SlowdownTime: base.Unwind(); break;
                case UpgradeModes.RevertTime:
                    {
                        availableUsages = -1;
                        hoursToReduce = -1;
                        base.Unwind();
                        break;
                    }
            }
        }

        public override void Increment()
        {
            switch (CurrentMode)
            {
                case UpgradeModes.SlowdownTime: base.Increment(); break;
                case UpgradeModes.RevertTime:
                    {
                        LategameConfiguration config = GetConfiguration();
                        availableUsages += config.QUANTUM_DISRUPTOR_INCREMENTAL_USES;
                        hoursToReduce += config.QUANTUM_DISRUPTOR_INCREMENTAL_HOURS_REVERT_ON_USE;
                        base.Increment();
                        break;
                    }
            }
        }

        public override void Load()
        {
            switch (CurrentMode)
            {
                case UpgradeModes.SlowdownTime: base.Load(); break;
                case UpgradeModes.RevertTime:
                    {
                        LategameConfiguration config = GetConfiguration();
                        availableUsages = config.QUANTUM_DISRUPTOR_INITIAL_USES.Value + (GetUpgradeLevel(UPGRADE_NAME) * config.QUANTUM_DISRUPTOR_INCREMENTAL_USES);
                        hoursToReduce = config.QUANTUM_DISRUPTOR_INITIAL_HOURS_REVERT_ON_USE.Value + (GetUpgradeLevel(UPGRADE_NAME) * config.QUANTUM_DISRUPTOR_INCREMENTAL_HOURS_REVERT_ON_USE);
                        base.Load();
                        break;
                    }
            }
        }

        public static float GetGlobalSpeedMultiplier(float defaultValue)
        {
            LategameConfiguration config = GetConfiguration();
            if (!config.QUANTUM_DISRUPTOR_ENABLED) return defaultValue;
            if (!GetActiveUpgrade(UPGRADE_NAME)) return defaultValue;
            if (CurrentMode != UpgradeModes.SlowdownTime) return defaultValue;

            float additionalValue = config.QUANTUM_DISRUPTOR_INITIAL_MULTIPLIER + (GetUpgradeLevel(UPGRADE_NAME) * config.QUANTUM_DISRUPTOR_INCREMENTAL_MULTIPLIER);
            return Mathf.Clamp(defaultValue - additionalValue, 0.01f, defaultValue);
        }

        string GetQuantumDisruptorRevertInfo(int level, int price)
        {
            static float infoFunctionUsages(int level)
            {
                LategameConfiguration config = GetConfiguration();
                return config.QUANTUM_DISRUPTOR_INITIAL_USES.Value + (level * config.QUANTUM_DISRUPTOR_INCREMENTAL_USES.Value);
            }
            static float infoFunctionHours(int level)
            {
                LategameConfiguration config = GetConfiguration();
                return config.QUANTUM_DISRUPTOR_INITIAL_HOURS_REVERT_ON_USE.Value + (level * config.QUANTUM_DISRUPTOR_INCREMENTAL_HOURS_REVERT_ON_USE.Value);
            }

            string resetString = string.Empty;
            switch (CurrentResetMode)
            {
                case ResetModes.MoonLanding: resetString = "moon landing"; break;
                case ResetModes.MoonRerouting: resetString = "moon routing"; break;
                case ResetModes.NewQuota: resetString = "completed quota"; break;
            }
            StringBuilder sb = new();
            switch (level)
            {
                case 1: sb.Append($"LVL {level} - ${price}: Unlocks \'quantum\' command which reverts curret moon's time by {infoFunctionHours(level - 1)} hours and can only be used {infoFunctionUsages(level - 1)} times per {resetString}\n"); break;
                default: sb.Append($"LVL {level} - ${price}: Reverts curret moon's time by {infoFunctionHours(level - 1)} hours and can only be used {infoFunctionUsages(level - 1)} times per {resetString}\n"); break;
            }
            return sb.ToString();
        }
        public override string GetDisplayInfo(int initialPrice = -1, int maxLevels = -1, int[] incrementalPrices = null)
        {
            switch (CurrentMode)
            {
                case UpgradeModes.SlowdownTime:
                    {
                        static float infoFunction(int level)
                        {
                            LategameConfiguration config = GetConfiguration();
                            return (config.QUANTUM_DISRUPTOR_INITIAL_MULTIPLIER.Value + (level * config.QUANTUM_DISRUPTOR_INCREMENTAL_MULTIPLIER.Value)) * 100f;
                        }
                        const string infoFormat = "LVL {0} - ${1} - Decreases the landed moon's rotation force (time passing) by {2}%\n";
                        return Tools.GenerateInfoForUpgrade(infoFormat, initialPrice, incrementalPrices, infoFunction);
                    }
                case UpgradeModes.RevertTime:
                    {
                        StringBuilder sb = new();
                        sb.Append(GetQuantumDisruptorRevertInfo(1, initialPrice));
                        for (int i = 0; i < maxLevels; i++)
                            sb.Append(GetQuantumDisruptorRevertInfo(i + 2, incrementalPrices[i]));
                        return sb.ToString();
                    }
            }
            return string.Empty;
        }
        public override bool CanInitializeOnStart
        {
            get
            {
                LategameConfiguration config = GetConfiguration();
                string[] prices = config.QUANTUM_DISRUPTOR_PRICES.Value.Split(',');
                return config.QUANTUM_DISRUPTOR_PRICE.Value <= 0 && prices.Length == 1 && (prices[0].Length == 0 || prices[0] == "0");
            }
        }

        internal (bool, string) CanRevertTime()
        {
            if (CurrentMode != UpgradeModes.RevertTime) return (false, $"This command is not available for selected \'{overridenUpgradeName}\' upgrade.\n");
            if (StartOfRound.Instance.inShipPhase) return (false, "Cannot execute this command while in orbit.\n");
            if (!StartOfRound.Instance.shipHasLanded) return (false, "Cannot execute this command while landing on a moon.\n");
            if (!TimeOfDay.Instance.currentDayTimeStarted) return (false, "Cannot execute this command during a time freeze.\n");
            float globalTime = TimeOfDay.Instance.globalTime;
            globalTime -= TimeOfDay.Instance.lengthOfHours * hoursToReduce;
            if (globalTime < 0) return (false, "This command cannot be executed due to the new time being before you started landing on the moon (Leads to negative time).\n");
            string resetString = string.Empty;
            switch (CurrentResetMode)
            {
                case ResetModes.MoonLanding: resetString = "Come back to orbit"; break;
                case ResetModes.MoonRerouting: resetString = "Route to a different moon"; break;
                case ResetModes.NewQuota: resetString = "Finish your current quota"; break;
            }
            return (currentUsages < availableUsages, $"Reached maximum amount of usages during this moon trip. {resetString} to recharge the disruptor.\n");
        }
        internal void TryResetValues(ResetModes requestedMode)
        {
            bool canReset = CurrentMode == UpgradeModes.RevertTime && CurrentResetMode == requestedMode;
            if (canReset) ResetUsageCounterClientRpc();
        }
        internal static void TryResetQuantum(ResetModes mode)
        {
            LategameConfiguration config = GetConfiguration();
            if (!config.QUANTUM_DISRUPTOR_ENABLED) return;
            if (!Instance.IsHost) return;
            Instance.TryResetValues(mode);
        }
        [ClientRpc]
        internal void RevertTimeClientRpc()
        {
            RevertTime();
        }
        internal void RevertTime()
        {
            float timeOffset = TimeOfDay.Instance.lengthOfHours * hoursToReduce;
            TimeOfDay.Instance.globalTime -= timeOffset;
            Plugin.mls.LogDebug($"Before reverting time on quota: {TimeOfDay.Instance.timeUntilDeadline}");
            TimeOfDay.Instance.timeUntilDeadline += timeOffset;
            Plugin.mls.LogDebug($"After reverting time on quota: {TimeOfDay.Instance.timeUntilDeadline}");
            currentUsages++;
        }
        [ClientRpc]
        internal void ResetUsageCounterClientRpc()
        {
            ResetUsageCounter();
        }
        internal void ResetUsageCounter()
        {
            currentUsages = 0;
        }
        public new static (string, string[]) RegisterScrapToUpgrade()
        {
            return (UPGRADE_NAME, GetConfiguration().QUANTUM_DISRUPTOR_ITEM_PROGRESSION_ITEMS.Value.Split(","));
        }
        public new static void RegisterUpgrade()
        {
            SetupGenericPerk<QuantumDisruptor>(UPGRADE_NAME);
        }
        public new static CustomTerminalNode RegisterTerminalNode()
        {
            LategameConfiguration configuration = GetConfiguration();

            return UpgradeBus.Instance.SetupMultiplePurchasableTerminalNode(UPGRADE_NAME,
                                                shareStatus: true,
                                                configuration.QUANTUM_DISRUPTOR_ENABLED.Value,
                                                configuration.QUANTUM_DISRUPTOR_PRICE.Value,
                                                UpgradeBus.ParseUpgradePrices(configuration.QUANTUM_DISRUPTOR_PRICES.Value),
                                                configuration.OVERRIDE_UPGRADE_NAMES ? configuration.QUANTUM_DISRUPTOR_OVERRIDE_NAME : "");
        }
    }
}
