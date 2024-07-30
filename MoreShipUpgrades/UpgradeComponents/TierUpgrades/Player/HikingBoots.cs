using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc.TerminalNodes;
using MoreShipUpgrades.Misc;
using MoreShipUpgrades.Misc.Upgrades;
using MoreShipUpgrades.Misc.Util;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using GameNetcodeStuff;

namespace MoreShipUpgrades.UpgradeComponents.TierUpgrades.Player
{
    internal class HikingBoots : TierUpgrade
    {
        internal const string UPGRADE_NAME = "Hiking Boots";
        internal const string PRICES_DEFAULT = "100,150,175";
        void Awake()
        {
            upgradeName = UPGRADE_NAME;
            overridenUpgradeName = UpgradeBus.Instance.PluginConfiguration.HIKING_BOOTS_OVERRIDE_NAME;
        }
        static float ComputeUphillSlopeDebuffMultiplier()
        {
            return 1f - ((UpgradeBus.Instance.PluginConfiguration.HIKING_BOOTS_INITIAL_DECREASE + (GetUpgradeLevel(UPGRADE_NAME) * UpgradeBus.Instance.PluginConfiguration.HIKING_BOOTS_INCREMENTAL_DECREASE))/100f);
        }
        public static float ReduceUphillSlopeDebuff(float defaultValue)
        {
            if (!UpgradeBus.Instance.PluginConfiguration.HIKING_BOOTS_ENABLED) return defaultValue;
            if (!GetActiveUpgrade(UPGRADE_NAME)) return defaultValue;
            PlayerControllerB localPlayer = UpgradeBus.Instance.GetLocalPlayer();
            if (localPlayer.isPlayerSliding) return defaultValue;
            float multiplier = ComputeUphillSlopeDebuffMultiplier();
            return Mathf.Clamp(defaultValue * multiplier, 0f, defaultValue);
        }
        public override string GetDisplayInfo(int initialPrice = -1, int maxLevels = -1, int[] incrementalPrices = null)
        {
            System.Func<int, float> infoFunction = level => UpgradeBus.Instance.PluginConfiguration.HIKING_BOOTS_INITIAL_DECREASE.Value + (level * UpgradeBus.Instance.PluginConfiguration.HIKING_BOOTS_INCREMENTAL_DECREASE.Value);
            string infoFormat = "LVL {0} - ${1} - Reduces the movement speed debuff when climbing uphill slopes by {2}%\n";
            return Tools.GenerateInfoForUpgrade(infoFormat, initialPrice, incrementalPrices, infoFunction);
        }

        public override bool CanInitializeOnStart
        {
            get
            {
                string[] prices = UpgradeBus.Instance.PluginConfiguration.HIKING_BOOTS_PRICES.Value.Split(',');
                return UpgradeBus.Instance.PluginConfiguration.HIKING_BOOTS_PRICE.Value <= 0 && prices.Length == 1 && (prices[0] == "" || prices[0] == "0");
            }
        }

        public new static (string, string[]) RegisterScrapToUpgrade()
        {
            return (UPGRADE_NAME, UpgradeBus.Instance.PluginConfiguration.HIKING_BOOTS_ITEM_PROGRESSION_ITEMS.Value.Split(","));
        }
        public new static void RegisterUpgrade()
        {
            //SetupGenericPerk<HikingBoots>(UPGRADE_NAME);
        }
        public new static CustomTerminalNode RegisterTerminalNode()
        {
            /*
            LategameConfiguration configuration = UpgradeBus.Instance.PluginConfiguration;

            return UpgradeBus.Instance.SetupMultiplePurchasableTerminalNode(UPGRADE_NAME,
                                                configuration.SHARED_UPGRADES || !configuration.HIKING_BOOTS_INDIVIDUAL,
                                                configuration.HIKING_BOOTS_ENABLED,
                                                configuration.HIKING_BOOTS_PRICE,
                                                UpgradeBus.ParseUpgradePrices(configuration.HIKING_BOOTS_PRICES),
                                                configuration.OVERRIDE_UPGRADE_NAMES ? configuration.HIKING_BOOTS_OVERRIDE_NAME : "");
            */
            return null;
        }
    }
}
