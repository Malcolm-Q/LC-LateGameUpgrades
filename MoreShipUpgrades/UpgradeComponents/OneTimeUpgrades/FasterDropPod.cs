using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc;
using MoreShipUpgrades.Misc.TerminalNodes;
using MoreShipUpgrades.Misc.Upgrades;
using UnityEngine;

namespace MoreShipUpgrades.UpgradeComponents.OneTimeUpgrades
{
    class FasterDropPod : OneTimeUpgrade
    {
        public const string UPGRADE_NAME = "Drop Pod Thrusters";
        public static FasterDropPod Instance;

        internal override void Start()
        {
            upgradeName = UPGRADE_NAME;
            overridenUpgradeName = UpgradeBus.Instance.PluginConfiguration.DROP_POD_THRUSTERS_OVERRIDE_NAME;
            base.Start();
        }

        void Awake()
        {
            Instance = this;
        }
        public static float GetFirstOrderTimer(float defaultValue)
        {
            if (!UpgradeBus.Instance.PluginConfiguration.FASTER_DROP_POD_ENABLED.Value) return defaultValue;
            if (!GetActiveUpgrade(UPGRADE_NAME)) return defaultValue;
            return Mathf.Clamp(defaultValue + UpgradeBus.Instance.PluginConfiguration.FASTER_DROP_POD_INITIAL_TIMER.Value, defaultValue, defaultValue + UpgradeBus.Instance.PluginConfiguration.FASTER_DROP_POD_TIMER.Value);
        }
        public static float GetUpgradedTimer(float defaultValue)
        {
            if (!UpgradeBus.Instance.PluginConfiguration.FASTER_DROP_POD_ENABLED.Value) return defaultValue;
            if (!GetActiveUpgrade(UPGRADE_NAME)) return defaultValue;
            return Mathf.Clamp(defaultValue - UpgradeBus.Instance.PluginConfiguration.FASTER_DROP_POD_TIMER.Value, 0f, defaultValue);
        }
        public static bool CanLeaveEarly(float shipTimer)
        {
            return shipTimer > UpgradeBus.Instance.PluginConfiguration.FASTER_DROP_POD_LEAVE_TIMER.Value;
        }
        public static bool IsUpgradeActive()
        {
            return GetActiveUpgrade(UPGRADE_NAME);
        }
        public override string GetDisplayInfo(int price = -1)
        {
            return $"${price} - Make the Drop Pod, the ship that deliver items bought on the terminal, land faster.";
        }
        public override bool CanInitializeOnStart => UpgradeBus.Instance.PluginConfiguration.FASTER_DROP_POD_PRICE.Value <= 0;
        public new static (string, string[]) RegisterScrapToUpgrade()
        {
            return (UPGRADE_NAME, UpgradeBus.Instance.PluginConfiguration.DROP_POD_THRUSTERS_ITEM_PROGRESSION_ITEMS.Value.Split(","));
        }
        public new static void RegisterUpgrade()
        {
            SetupGenericPerk<FasterDropPod>(UPGRADE_NAME);
        }
        public new static CustomTerminalNode RegisterTerminalNode()
        {
            LategameConfiguration configuration = UpgradeBus.Instance.PluginConfiguration;

            return UpgradeBus.Instance.SetupOneTimeTerminalNode(UPGRADE_NAME,
                                    shareStatus: true,
                                    configuration.FASTER_DROP_POD_ENABLED.Value,
                                    configuration.FASTER_DROP_POD_PRICE.Value,
                                    configuration.OVERRIDE_UPGRADE_NAMES ? configuration.DROP_POD_THRUSTERS_OVERRIDE_NAME : "");
        }
    }
}
