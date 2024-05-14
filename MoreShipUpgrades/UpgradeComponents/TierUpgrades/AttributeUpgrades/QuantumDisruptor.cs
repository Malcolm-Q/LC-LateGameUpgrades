using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc;
using MoreShipUpgrades.Misc.Upgrades;
using MoreShipUpgrades.Misc.Util;
using MoreShipUpgrades.UpgradeComponents.Interfaces;
using System;
using Unity.Netcode;

namespace MoreShipUpgrades.UpgradeComponents.TierUpgrades.AttributeUpgrades
{
    internal class QuantumDisruptor : GameAttributeTierUpgrade, IServerSync
    {
        internal enum UpgradeModes
        {
            SlowdownTime,
            RevertTime,
        }
        internal const string UPGRADE_NAME = "Quantum Disruptor";
        internal const string PRICES_DEFAULT = "1200,1500,1800";
        internal static QuantumDisruptor Instance;

        internal UpgradeModes currentMode;
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
            Enum.TryParse(typeof(UpgradeModes), UpgradeBus.Instance.PluginConfiguration.QUANTUM_DISRUPTOR_UPGRADE_MODE.Value, out object parsedRestriction);
            if (parsedRestriction == null)
            {
                logger.LogError($"An error occured parsing the restriction mode ({UpgradeBus.Instance.PluginConfiguration.QUANTUM_DISRUPTOR_UPGRADE_MODE.Value}), defaulting to SlowdownTime");
                currentMode = UpgradeModes.SlowdownTime;
            }
            else currentMode = (UpgradeModes)parsedRestriction;
        }

        public override void Increment()
        {
            ExecuteIncrementFunction(this);
        }

        void ExecuteIncrementFunction(TierUpgrade baseUpgrade)
        {
            switch (currentMode)
            {
                case UpgradeModes.SlowdownTime: base.Increment(); break;
                case UpgradeModes.RevertTime:
                    {
                        availableUsages += UpgradeBus.Instance.PluginConfiguration.QUANTUM_DISRUPTOR_INCREMENTAL_USES;
                        hoursToReduce += UpgradeBus.Instance.PluginConfiguration.QUANTUM_DISURPTOR_INCREMENTAL_HOURS_REVERT_ON_USE;
                        baseUpgrade.Increment();
                        break;
                    }
            }
        }
        public override void Load()
        {
            ExecuteLoadFunction(this);
        }

        void ExecuteLoadFunction(TierUpgrade baseUpgrade)
        {
            switch (currentMode)
            {
                case UpgradeModes.SlowdownTime: base.Load(); break;
                case UpgradeModes.RevertTime:
                    {
                        availableUsages = UpgradeBus.Instance.PluginConfiguration.QUANTUM_DISRUPTOR_INITIAL_USES.Value + (GetUpgradeLevel(UPGRADE_NAME) * UpgradeBus.Instance.PluginConfiguration.QUANTUM_DISRUPTOR_INCREMENTAL_USES);
                        hoursToReduce = UpgradeBus.Instance.PluginConfiguration.QUANTUM_DISRUPTOR_INITIAL_HOURS_REVERT_ON_USE.Value + (GetUpgradeLevel(UPGRADE_NAME) * UpgradeBus.Instance.PluginConfiguration.QUANTUM_DISURPTOR_INCREMENTAL_HOURS_REVERT_ON_USE);
                        baseUpgrade.Load();
                        break;
                    }
            }
        }
        public override string GetDisplayInfo(int initialPrice = -1, int maxLevels = -1, int[] incrementalPrices = null)
        {
            Func<int, float> infoFunction = level => (UpgradeBus.Instance.PluginConfiguration.QUANTUM_DISRUPTOR_INITIAL_MULTIPLIER.Value + level * UpgradeBus.Instance.PluginConfiguration.QUANTUM_DISRUPTOR_INCREMENTAL_MULTIPLIER.Value) * 100f;
            string infoFormat = "LVL {0} - ${1} - Decreases the landed moon's rotation force (time passing) by {2}%\n";
            return Tools.GenerateInfoForUpgrade(infoFormat, initialPrice, incrementalPrices, infoFunction);
        }
        internal override bool CanInitializeOnStart()
        {
            string[] prices = UpgradeBus.Instance.PluginConfiguration.QUANTUM_DISRUPTOR_PRICES.Value.Split(',');
            bool free = UpgradeBus.Instance.PluginConfiguration.QUANTUM_DISRUPTOR_PRICE.Value <= 0 && prices.Length == 1 && (prices[0] == "" || prices[0] == "0");
            return free;
        }
        internal (bool, string) CanRevertTime()
        {
            if (currentMode != UpgradeModes.RevertTime) return (false, $"This command is not available for selected \'{overridenUpgradeName}\' upgrade");
            if (!StartOfRound.Instance.shipHasLanded || StartOfRound.Instance.inShipPhase || !TimeOfDay.Instance.currentDayTimeStarted) return (false, "Cannot execute this command while in orbit.");
            float globalTime = TimeOfDay.Instance.globalTime;
            globalTime -= TimeOfDay.Instance.lengthOfHours * hoursToReduce;
            //if (globalTime < 0) return (false, "Not enough hours have passed to execute this command");

            return (currentUsages < availableUsages, "Reached maximum amount of usages during this moon trip. Come back to orbit to recharge the disruptor.");
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
