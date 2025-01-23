using MoreShipUpgrades.Configuration;
using MoreShipUpgrades.Configuration.Custom;
using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc.Upgrades;
using MoreShipUpgrades.UI.TerminalNodes;
using MoreShipUpgrades.UpgradeComponents.Interfaces;
using UnityEngine;

namespace MoreShipUpgrades.UpgradeComponents.OneTimeUpgrades.Store
{
    class FasterDropPod : OneTimeUpgrade, IUpgradeWorldBuilding
    {
        public const string UPGRADE_NAME = "Drop Pod Thrusters";
        internal const string WORLD_BUILDING_TEXT = "\n\nPlaces your department on the 'Priority Shipping List'; your purchases should arrive ~30 minutes faster than before." +
            " It's estimated that for every department that signs up for this package, every department on the standard shipping list has their orders delayed by roughly 1.7 seconds.\n\n";
        public static FasterDropPod Instance;
        public override bool CanInitializeOnStart => GetConfiguration().DropPodThrustersConfiguration.Price.Value <= 0;

        public string GetWorldBuildingText(bool shareStatus = false)
        {
            return WORLD_BUILDING_TEXT;
        }
        internal override void Start()
        {
            upgradeName = UPGRADE_NAME;
            overridenUpgradeName = GetConfiguration().DropPodThrustersConfiguration.OverrideName;
            base.Start();
        }

        void Awake()
        {
            Instance = this;
        }
        public static float GetFirstOrderTimer(float defaultValue)
        {
            DropPodThrustersUpgradeConfiguration config = GetConfiguration().DropPodThrustersConfiguration;
            if (!config.Enabled.Value) return defaultValue;
            if (!GetActiveUpgrade(UPGRADE_NAME)) return defaultValue;
            return Mathf.Clamp(defaultValue + config.InitialTimer.Value, defaultValue, defaultValue + config.Timer.Value);
        }
        public static float GetUpgradedTimer(float defaultValue)
        {
            DropPodThrustersUpgradeConfiguration config = GetConfiguration().DropPodThrustersConfiguration;
            if (!config.Enabled.Value) return defaultValue;
            if (!GetActiveUpgrade(UPGRADE_NAME)) return defaultValue;
            return Mathf.Clamp(defaultValue - config.Timer.Value, 0f, defaultValue);
        }
        public static bool CanLeaveEarly(float shipTimer)
        {
            return shipTimer > GetConfiguration().DropPodThrustersConfiguration.ExitTimer.Value;
        }
        public static bool IsUpgradeActive()
        {
            return GetActiveUpgrade(UPGRADE_NAME);
        }
        public override string GetDisplayInfo(int price = -1)
        {
            return $"${price} - Make the Drop Pod, the ship that deliver items bought on the terminal, land faster.";
        }
        public new static (string, string[]) RegisterScrapToUpgrade()
        {
            return (UPGRADE_NAME, GetConfiguration().DropPodThrustersConfiguration.ItemProgressionItems.Value.Split(","));
        }
        public new static void RegisterUpgrade()
        {
            SetupGenericPerk<FasterDropPod>(UPGRADE_NAME);
        }
        public new static CustomTerminalNode RegisterTerminalNode()
        {
            return UpgradeBus.Instance.SetupOneTimeTerminalNode(UPGRADE_NAME, GetConfiguration().DropPodThrustersConfiguration);
        }
    }
}
