using CSync.Lib;
using MoreShipUpgrades.Configuration.Upgrades.Custom;
using MoreShipUpgrades.Configuration.Upgrades.Interfaces.TierUpgrades;
using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc.Upgrades;
using MoreShipUpgrades.Misc.Util;
using MoreShipUpgrades.UI.TerminalNodes;
using MoreShipUpgrades.UpgradeComponents.TierUpgrades.Items.Shotgun;
using System.Text;
using UnityEngine;

namespace MoreShipUpgrades.UpgradeComponents.TierUpgrades.Items.TZP
{
    internal class TZPBuffer : TierUpgrade
    {
        internal const string UPGRADE_NAME = "TZP Buffer";
        internal const string DEFAULT_PRICES = "100,200,300";

        internal override void Start()
        {
            upgradeName = UPGRADE_NAME;
            overridenUpgradeName = GetConfiguration().TZPBufferConfiguration.OverrideName;
            base.Start();
        }
        public override bool CanInitializeOnStart
        {
            get
            {
                ITierUpgradeConfiguration upgradeConfig = GetConfiguration().TZPBufferConfiguration;
                string[] prices = upgradeConfig.Prices.Value.Split(',');
                return prices.Length == 0 || (prices.Length == 1 && (prices[0].Length == 0 || prices[0] == "0"));
            }
        }
        string GetTZPBufferInfo(int level, int price)
        {
            static float buffEffectInfo(int level)
            {
                TZPBufferUpgradeConfiguration config = GetConfiguration().TZPBufferConfiguration;
                (SyncedEntry<int>, SyncedEntry<int>) buffDurationPair = config.GetEffectPair(0);
                return buffDurationPair.Item1.Value + (level * buffDurationPair.Item2.Value);
            }
            static float debuffEffectInfo(int level)
            {
                TZPBufferUpgradeConfiguration config = GetConfiguration().TZPBufferConfiguration;
                (SyncedEntry<int>, SyncedEntry<int>) debuffDurationPair = config.GetEffectPair(1);
                return debuffDurationPair.Item1.Value + (level * debuffDurationPair.Item2.Value);
            }
            StringBuilder sb = new();
            sb.Append($"LVL {level} - {GetUpgradePrice(price, GetConfiguration().TZPBufferConfiguration.PurchaseMode)}: Upgrades to TZP Inhaler:\n");
            sb.Append($"- Increases the buff effect (movement speed and stamina usage reduction) by {buffEffectInfo(level)}%\n");
            sb.Append($"- Decreases the debuff effect (visual impairment) by {debuffEffectInfo(level)}%\n");
            sb.Append('\n');
            return sb.ToString();
        }
        public override string GetDisplayInfo(int initialPrice = -1, int maxLevels = -1, int[] incrementalPrices = null)
        {
            StringBuilder sb = new();
            sb.Append(GetTZPBufferInfo(1, initialPrice));
            for (int i = 0; i < maxLevels; i++)
                sb.Append(GetTZPBufferInfo(i + 2, incrementalPrices[i]));
            return sb.ToString();
        }
        public static float ComputeTZPBufferBuffEffectIncrease()
        {
            TZPBufferUpgradeConfiguration upgradeConfig = GetConfiguration().TZPBufferConfiguration;
            (SyncedEntry<int>, SyncedEntry<int>) buffDurationPair = upgradeConfig.GetEffectPair(0);
            return (buffDurationPair.Item1.Value + (GetUpgradeLevel(UPGRADE_NAME) * buffDurationPair.Item2.Value)) / 100f;
        }
        public static float ComputeTZPBufferDeBuffEffectReduction()
        {
            TZPBufferUpgradeConfiguration upgradeConfig = GetConfiguration().TZPBufferConfiguration;
            (SyncedEntry<int>, SyncedEntry<int>) debuffDurationPair = upgradeConfig.GetEffectPair(1);
            return (debuffDurationPair.Item1.Value + (GetUpgradeLevel(UPGRADE_NAME) * debuffDurationPair.Item2.Value)) / 100f;
        }
        public static float GetTZPBufferBuffDurationIncrease(float defaultValue)
        {
            if (!GetConfiguration().TZPBufferConfiguration.Enabled) return defaultValue;
            if (!GetActiveUpgrade(UPGRADE_NAME)) return defaultValue;
            float additionalDamage = ComputeTZPBufferBuffEffectIncrease();
            return defaultValue + defaultValue * additionalDamage;
        }
        public static float GetTZPBufferBuffEffectIncrease(float defaultValue)
        {
            if (!GetConfiguration().TZPBufferConfiguration.Enabled) return defaultValue;
            if (!GetActiveUpgrade(UPGRADE_NAME)) return defaultValue;
            float additionalDamage = ComputeTZPBufferBuffEffectIncrease();
            return Mathf.Clamp(defaultValue - defaultValue * additionalDamage, 0f, float.MaxValue);
        }
        public static float GetTZPBufferDebuffDurationReduction(float defaultValue)
        {
            if (!GetConfiguration().TZPBufferConfiguration.Enabled) return defaultValue;
            if (!GetActiveUpgrade(UPGRADE_NAME)) return defaultValue;
            float additionalDamage = ComputeTZPBufferDeBuffEffectReduction();
            return Mathf.Clamp(defaultValue - defaultValue * additionalDamage, 0f, float.MaxValue);
        }
        public new static (string, string[]) RegisterScrapToUpgrade()
        {
            return (UPGRADE_NAME, GetConfiguration().TZPBufferConfiguration.ItemProgressionItems.Value.Split(","));
        }
        public new static void RegisterUpgrade()
        {
            GameObject prefab = ItemManager.CreateNetworkPrefab(UPGRADE_NAME);
            prefab.AddComponent<TZPBuffer>();
            ItemManager.RegisterNetworkPrefab(prefab);
            Plugin.networkPrefabs[UPGRADE_NAME] = prefab;
        }
        public new static CustomTerminalNode RegisterTerminalNode()
        {
            return UpgradeBus.Instance.SetupMultiplePurchaseableTerminalNode(UPGRADE_NAME, GetConfiguration().TZPBufferConfiguration, Plugin.networkPrefabs[UPGRADE_NAME]);
        }
    }
}
