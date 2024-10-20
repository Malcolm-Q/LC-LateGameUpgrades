using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc;
using MoreShipUpgrades.Misc.Upgrades;
using MoreShipUpgrades.Misc.Util;
using MoreShipUpgrades.UI.TerminalNodes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace MoreShipUpgrades.UpgradeComponents.TierUpgrades.Ship
{
    internal class ParticleInfuser : TierUpgrade
    {
        internal const string UPGRADE_NAME = "Particle Infuser";
        internal const string DEFAULT_PRICES = "400,600";
        internal static ParticleInfuser instance;
        void Awake()
        {
            upgradeName = UPGRADE_NAME;
            overridenUpgradeName = GetConfiguration().PARTICLE_INFUSER_OVERRIDE_NAME;
            instance = this;
        }
        public static float GetIncreasedTeleportSpeedMultiplier()
        {
            LategameConfiguration config = GetConfiguration();
            return (config.PARTICLE_INFUSER_INITIAL_TELEPORT_SPEED_INCREASE + (GetUpgradeLevel(UPGRADE_NAME) * config.PARTICLE_INFUSER_INCREMENTAL_TELEPORT_SPEED_INCREASE)) / 100f;
        }
        public static float DecreaseTeleportTime(float defaultValue)
        {
            LategameConfiguration config = GetConfiguration();
            if (!config.PARTICLE_INFUSER_ENABLED) return defaultValue;
            if (!GetActiveUpgrade(UPGRADE_NAME)) return defaultValue;
            float multiplier = GetIncreasedTeleportSpeedMultiplier();
            return Mathf.Clamp(defaultValue - (defaultValue * multiplier), 0f, defaultValue);
        }
        public static float IncreaseTeleportSpeed()
        {
            LategameConfiguration config = GetConfiguration();
            if (!config.PARTICLE_INFUSER_ENABLED) return 1f;
            if (!GetActiveUpgrade(UPGRADE_NAME)) return 1f;
            float multiplier = GetIncreasedTeleportSpeedMultiplier();
            return Mathf.Clamp(3f + multiplier, 1f, float.MaxValue);
        }
        public override string GetDisplayInfo(int initialPrice = -1, int maxLevels = -1, int[] incrementalPrices = null)
        {
            static float infoFunction(int level)
            {
                LategameConfiguration config = GetConfiguration();
                return config.PARTICLE_INFUSER_INITIAL_TELEPORT_SPEED_INCREASE.Value + (level * config.PARTICLE_INFUSER_INCREMENTAL_TELEPORT_SPEED_INCREASE.Value);
            }
            const string infoFormat = "LVL {0} - ${1} - Increases the teleporter's speed by {2}%\n";
            return Tools.GenerateInfoForUpgrade(infoFormat, initialPrice, incrementalPrices, infoFunction);
        }

        public override bool CanInitializeOnStart
        {
            get
            {
                LategameConfiguration config = GetConfiguration();
                string[] prices = config.PARTICLE_INFUSER_PRICES.Value.Split(',');
                return config.PARTICLE_INFUSER_PRICE.Value <= 0 && prices.Length == 1 && (prices[0].Length == 0 || prices[0] == "0");
            }
        }

        public new static (string, string[]) RegisterScrapToUpgrade()
        {
            return (UPGRADE_NAME, GetConfiguration().PARTICLE_INFUSER_ITEM_PROGRESSION_ITEMS.Value.Split(","));
        }
        public new static void RegisterUpgrade()
        {
            GameObject prefab = LethalLib.Modules.NetworkPrefabs.CreateNetworkPrefab(UPGRADE_NAME);
            prefab.AddComponent<ParticleInfuser>();
            LethalLib.Modules.NetworkPrefabs.RegisterNetworkPrefab(prefab);
            Plugin.networkPrefabs[UPGRADE_NAME] = prefab;
        }
        public new static CustomTerminalNode RegisterTerminalNode()
        {
            LategameConfiguration configuration = GetConfiguration();

            return UpgradeBus.Instance.SetupMultiplePurchasableTerminalNode(UPGRADE_NAME,
                                                shareStatus: true,
                                                configuration.PARTICLE_INFUSER_ENABLED,
                                                configuration.PARTICLE_INFUSER_PRICE,
                                                UpgradeBus.ParseUpgradePrices(configuration.PARTICLE_INFUSER_PRICES),
                                                configuration.OVERRIDE_UPGRADE_NAMES ? configuration.PARTICLE_INFUSER_OVERRIDE_NAME : "",
                                                Plugin.networkPrefabs[UPGRADE_NAME]);
        }

        public IEnumerator ResetTeleporterSpeed(ShipTeleporter teleporter)
        {
            yield return new WaitForEndOfFrame();
            yield return new WaitWhile(() => teleporter.teleporterAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f);

            teleporter.teleporterAnimator.speed /= IncreaseTeleportSpeed();
            teleporter.shipTeleporterAudio.pitch /= IncreaseTeleportSpeed();

        }
    }
}
