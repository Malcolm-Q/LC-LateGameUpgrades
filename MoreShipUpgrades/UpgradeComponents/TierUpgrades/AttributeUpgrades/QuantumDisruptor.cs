using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc;
using MoreShipUpgrades.Misc.Upgrades;
using MoreShipUpgrades.UpgradeComponents.Interfaces;
using System;

namespace MoreShipUpgrades.UpgradeComponents.TierUpgrades.AttributeUpgrades
{
    internal class QuantumDisruptor : GameAttributeTierUpgrade, IServerSync
    {
        internal const string UPGRADE_NAME = "Quantum Disruptor";
        internal const string PRICES_DEFAULT = "1200,1500,1800";
        void Awake()
        {
            upgradeName = UPGRADE_NAME;
            logger = new LguLogger(UPGRADE_NAME);
            changingAttribute = GameAttribute.TIME_GLOBAL_TIME_MULTIPLIER;
            initialValue = UpgradeBus.Instance.PluginConfiguration.QUANTUM_DISRUPTOR_INITIAL_MULTIPLIER.Value;
            incrementalValue = UpgradeBus.Instance.PluginConfiguration.QUANTUM_DISRUPTOR_INCREMENTAL_MULTIPLIER.Value;
        }
        public override string GetDisplayInfo(int initialPrice = -1, int maxLevels = -1, int[] incrementalPrices = null)
        {
            Func<int, float> infoFunction = level => (1f - (UpgradeBus.Instance.PluginConfiguration.QUANTUM_DISRUPTOR_INITIAL_MULTIPLIER.Value + level * UpgradeBus.Instance.PluginConfiguration.QUANTUM_DISRUPTOR_INCREMENTAL_MULTIPLIER.Value)) * 100f;
            string infoFormat = "LVL {0} - ${1} - Decreases the landed moon's rotation force (time passing) by {2}%\n";
            return Tools.GenerateInfoForUpgrade(infoFormat, initialPrice, incrementalPrices, infoFunction);
        }
    }
}
