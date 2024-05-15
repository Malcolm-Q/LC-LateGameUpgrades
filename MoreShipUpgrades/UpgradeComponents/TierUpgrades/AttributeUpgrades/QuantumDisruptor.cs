using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc;
using MoreShipUpgrades.Misc.Upgrades;
using MoreShipUpgrades.Misc.Util;
using MoreShipUpgrades.UpgradeComponents.Interfaces;
using System;
using System.Text;
using Unity.Netcode;

namespace MoreShipUpgrades.UpgradeComponents.TierUpgrades.AttributeUpgrades
{
    public class QuantumDisruptor : GameAttributeTierUpgrade, IServerSync
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
        internal static QuantumDisruptor Instance;

        internal static UpgradeModes CurrentMode
        {
            get
            {
                return UpgradeBus.Instance.PluginConfiguration.QUANTUM_DISRUPTOR_UPGRADE_MODE;
            }
        }
        internal static ResetModes CurrentResetMode
        {
            get
            {
                return UpgradeBus.Instance.PluginConfiguration.QUANTUM_DISRUPTOR_RESET_MODE.Value;
            }
        }
        internal int availableUsages = -1;
        internal int currentUsages = 0;
        internal int hoursToReduce = -1;
        void Awake()
        {
            Instance = this;
            upgradeName = UPGRADE_NAME;
            overridenUpgradeName = UpgradeBus.Instance.PluginConfiguration.QUANTUM_DISRUPTOR_OVERRIDE_NAME;
            logger = new LguLogger(UPGRADE_NAME);
            changingAttribute = GameAttribute.TIME_GLOBAL_TIME_MULTIPLIER;
            initialValue = UpgradeBus.Instance.PluginConfiguration.QUANTUM_DISRUPTOR_INITIAL_MULTIPLIER.Value;
            incrementalValue = UpgradeBus.Instance.PluginConfiguration.QUANTUM_DISRUPTOR_INCREMENTAL_MULTIPLIER.Value;
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
                        UpgradeBus.Instance.activeUpgrades[upgradeName] = false;
                        if (!UpgradeBus.Instance.PluginConfiguration.SHOW_UPGRADES_CHAT.LocalValue) return;
                        ShowUpgradeNotification(LGUConstants.UPGRADE_LOADED_NOTIFICATION_DEFAULT_COLOR, $"{(UpgradeBus.Instance.PluginConfiguration.OVERRIDE_UPGRADE_NAMES ? overridenUpgradeName : upgradeName)} has been disabled!");
                        UpgradeBus.Instance.upgradeLevels[upgradeName] = 0;
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
                        availableUsages += UpgradeBus.Instance.PluginConfiguration.QUANTUM_DISRUPTOR_INCREMENTAL_USES;
                        hoursToReduce += UpgradeBus.Instance.PluginConfiguration.QUANTUM_DISRUPTOR_INCREMENTAL_HOURS_REVERT_ON_USE;
                        UpgradeBus.Instance.upgradeLevels[upgradeName] = GetUpgradeLevel(upgradeName) + 1;
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
                        availableUsages = UpgradeBus.Instance.PluginConfiguration.QUANTUM_DISRUPTOR_INITIAL_USES.Value + (GetUpgradeLevel(UPGRADE_NAME) * UpgradeBus.Instance.PluginConfiguration.QUANTUM_DISRUPTOR_INCREMENTAL_USES);
                        hoursToReduce = UpgradeBus.Instance.PluginConfiguration.QUANTUM_DISRUPTOR_INITIAL_HOURS_REVERT_ON_USE.Value + (GetUpgradeLevel(UPGRADE_NAME) * UpgradeBus.Instance.PluginConfiguration.QUANTUM_DISRUPTOR_INCREMENTAL_HOURS_REVERT_ON_USE);
                        UpgradeBus.Instance.activeUpgrades[upgradeName] = true;
                        if (!UpgradeBus.Instance.PluginConfiguration.SHOW_UPGRADES_CHAT.LocalValue) return;
                        ShowUpgradeNotification(LGUConstants.UPGRADE_UNLOADED_NOTIFICATION_DEFAULT_COLOR, $"{(UpgradeBus.Instance.PluginConfiguration.OVERRIDE_UPGRADE_NAMES ? overridenUpgradeName : upgradeName)} is active!");
                        break;
                    }
            }
        }

        string GetQuantumDisruptorRevertInfo(int level, int price)
        {
            Func<int, float> infoFunctionUsages = level => (UpgradeBus.Instance.PluginConfiguration.QUANTUM_DISRUPTOR_INITIAL_USES.Value + level * UpgradeBus.Instance.PluginConfiguration.QUANTUM_DISRUPTOR_INCREMENTAL_USES.Value);
            Func<int, float> infoFunctionHours = level => (UpgradeBus.Instance.PluginConfiguration.QUANTUM_DISRUPTOR_INITIAL_HOURS_REVERT_ON_USE.Value + level * UpgradeBus.Instance.PluginConfiguration.QUANTUM_DISRUPTOR_INCREMENTAL_HOURS_REVERT_ON_USE.Value);
            string resetString = string.Empty;
            switch (CurrentResetMode)
            {
                case ResetModes.MoonLanding: resetString = "moon landing"; break;
                case ResetModes.MoonRerouting: resetString = "moon routing"; break;
                case ResetModes.NewQuota: resetString = "completed quota"; break;
            }
            StringBuilder sb = new StringBuilder();
            switch (level)
            {
                case 1: sb.Append($"LVL {level} - ${price}: Unlocks \'quantum\' command which reverts curret moon's time by {infoFunctionHours(level - 1)} hours and can only be used {infoFunctionUsages(level - 1)} times per {resetString}\n"); break;
                default: sb.Append($"LVL {level} - ${price}: Reverts curret moon's time by {infoFunctionHours(level - 1)} hours and can only be used {infoFunctionUsages(level - 1)} times per {resetString}\n"); break;
            }
            return sb.ToString();
        }
        public override string GetDisplayInfo(int initialPrice = -1, int maxLevels = -1, int[] incrementalPrices = null)
        {
            switch(CurrentMode)
            {
                case UpgradeModes.SlowdownTime:
                    {
                        Func<int, float> infoFunction = level => (UpgradeBus.Instance.PluginConfiguration.QUANTUM_DISRUPTOR_INITIAL_MULTIPLIER.Value + level * UpgradeBus.Instance.PluginConfiguration.QUANTUM_DISRUPTOR_INCREMENTAL_MULTIPLIER.Value) * 100f;
                        string infoFormat = "LVL {0} - ${1} - Decreases the landed moon's rotation force (time passing) by {2}%\n";
                        return Tools.GenerateInfoForUpgrade(infoFormat, initialPrice, incrementalPrices, infoFunction);
                    }
                case UpgradeModes.RevertTime:
                    {
                        StringBuilder sb = new StringBuilder();
                        sb.Append(GetQuantumDisruptorRevertInfo(1, initialPrice));
                        for (int i = 0; i < maxLevels; i++)
                            sb.Append(GetQuantumDisruptorRevertInfo(i + 2, incrementalPrices[i]));
                        return sb.ToString();
                    }
            }
            return string.Empty;
        }
        internal override bool CanInitializeOnStart()
        {
            string[] prices = UpgradeBus.Instance.PluginConfiguration.QUANTUM_DISRUPTOR_PRICES.Value.Split(',');
            bool free = UpgradeBus.Instance.PluginConfiguration.QUANTUM_DISRUPTOR_PRICE.Value <= 0 && prices.Length == 1 && (prices[0] == "" || prices[0] == "0");
            return free;
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
            switch(CurrentResetMode)
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
        [ClientRpc]
        internal void RevertTimeClientRpc()
        {
            RevertTime();
        }
        internal void RevertTime()
        {
            TimeOfDay.Instance.globalTime -= TimeOfDay.Instance.lengthOfHours * hoursToReduce;
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
        internal new static void RegisterUpgrade()
        {
            SetupGenericPerk<QuantumDisruptor>(UPGRADE_NAME);
        }
    }
}
