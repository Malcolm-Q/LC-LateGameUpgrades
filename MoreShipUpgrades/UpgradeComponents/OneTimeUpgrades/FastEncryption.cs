using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc;
using MoreShipUpgrades.Misc.TerminalNodes;
using MoreShipUpgrades.Misc.Upgrades;
using MoreShipUpgrades.UpgradeComponents.Interfaces;
using UnityEngine;

namespace MoreShipUpgrades.UpgradeComponents.OneTimeUpgrades
{
    class FastEncryption : OneTimeUpgrade
    {
        public const string UPGRADE_NAME = "Fast Encryption";
        public static FastEncryption instance;
        public override bool CanInitializeOnStart => UpgradeBus.Instance.PluginConfiguration.PAGER_PRICE.Value <= 0;

        const float TRANSMIT_MULTIPLIER = 0.2f;
        void Awake()
        {
            upgradeName = UPGRADE_NAME;
            overridenUpgradeName = UpgradeBus.Instance.PluginConfiguration.FAST_ENCRYPTION_OVERRIDE_NAME;
            instance = this;
        }
        public static int GetLimitOfCharactersTransmit(int defaultLimit, string message)
        {
            if (!GetActiveUpgrade(UPGRADE_NAME)) return defaultLimit;
            return message.Length;
        }
        public static float GetMultiplierOnSignalTextTimer(float defaultMultiplier)
        {
            if (!GetActiveUpgrade(UPGRADE_NAME)) return defaultMultiplier;
            return defaultMultiplier* TRANSMIT_MULTIPLIER;
        }
        public override string GetDisplayInfo(int price = -1)
        {
            return $"${price} - The transmitter will write the letters faster and the restriction of characters will be lifted.";
        }
        public new static (string, string[]) RegisterScrapToUpgrade()
        {
            return (UPGRADE_NAME, UpgradeBus.Instance.PluginConfiguration.FAST_ENCRYPTION_ITEM_PROGRESSION_ITEMS.Value.Split(","));
        }
        public new static void RegisterUpgrade()
        {
            SetupGenericPerk<FastEncryption>(UPGRADE_NAME);
        }
        public new static CustomTerminalNode RegisterTerminalNode()
        {
            LategameConfiguration configuration = UpgradeBus.Instance.PluginConfiguration;

            return UpgradeBus.Instance.SetupOneTimeTerminalNode(UPGRADE_NAME,
                                    shareStatus: true,
                                    configuration.PAGER_ENABLED.Value,
                                    configuration.PAGER_PRICE.Value,
                                    configuration.OVERRIDE_UPGRADE_NAMES ? configuration.FAST_ENCRYPTION_OVERRIDE_NAME : "");
        }
    }
}
