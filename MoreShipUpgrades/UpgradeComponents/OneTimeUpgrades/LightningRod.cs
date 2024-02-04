﻿using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc;
using MoreShipUpgrades.Misc.Upgrades;
using UnityEngine;

namespace MoreShipUpgrades.UpgradeComponents.OneTimeUpgrades
{
    class LightningRod : OneTimeUpgrade
    {
        public static string UPGRADE_NAME = "Lightning Rod";
        public static LightningRod instance;
        private static LGULogger logger;

        // Configuration
        public static string ENABLED_SECTION = $"Enable {UPGRADE_NAME} Upgrade";
        public static bool ENABLED_DEFAULT = true;
        public static string ENABLED_DESCRIPTION = "A device which redirects lightning bolts to the ship.";

        public static string PRICE_SECTION = $"{UPGRADE_NAME} Price";
        public static int PRICE_DEFAULT = 1000;

        public static string ACTIVE_SECTION = "Active on Purchase";
        public static bool ACTIVE_DEFAULT = true;
        public static string ACTIVE_DESCRIPTION = $"If true: {UPGRADE_NAME} will be active on purchase.";

        // Toggle
        public static string ACCESS_DENIED_MESSAGE = $"You don't have access to this command yet. Purchase the '{UPGRADE_NAME}'.\n";
        public static string TOGGLE_ON_MESSAGE = $"{UPGRADE_NAME} has been enabled. Lightning bolts will now be redirected to the ship.\n";
        public static string TOGGLE_OFF_MESSAGE = $"{UPGRADE_NAME} has been disabled. Lightning bolts will no longer be redirected to the ship.\n";

        // distance
        public static string DIST_SECTION = $"Effective Distance of {UPGRADE_NAME}.";
        public static float DIST_DEFAULT = 175f;
        public static string DIST_DESCRIPTION = $"The closer you are the more likely the rod will reroute lightning.";

        public bool CanTryInterceptLightning { get; internal set; }
        public bool LightningIntercepted { get; internal set; }

        internal override void Start()
        {
            instance = this;
            logger = new LGULogger(UPGRADE_NAME);
            upgradeName = UPGRADE_NAME;
            base.Start();
        }
        public override void Load()
        {
            base.Load();
            UpgradeBus.instance.lightningRod = true;
            UpgradeBus.instance.lightningRodActive = UpgradeBus.instance.cfg.LIGHTNING_ROD_ACTIVE;
        }

        public override void Unwind()
        {
            base.Unwind();
            UpgradeBus.instance.lightningRod = false;
            UpgradeBus.instance.lightningRodActive = false;
        }

        public static void TryInterceptLightning(ref StormyWeather __instance, ref GrabbableObject ___targetingMetalObject)
        {
            if (!instance.CanTryInterceptLightning) return;
            instance.CanTryInterceptLightning = false;

            Terminal terminal = UpgradeBus.instance.GetTerminal();
            float dist = Vector3.Distance(___targetingMetalObject.transform.position, terminal.transform.position);
            logger.LogDebug($"Distance from ship: {dist}");
            logger.LogDebug($"Effective distance of the lightning rod: {UpgradeBus.instance.cfg.LIGHTNING_ROD_DIST}");

            if (dist > UpgradeBus.instance.cfg.LIGHTNING_ROD_DIST) return;

            dist /= UpgradeBus.instance.cfg.LIGHTNING_ROD_DIST;
            float prob = 1 - dist;
            float rand = Random.value;

            logger.LogDebug($"Number to beat: {prob}");
            logger.LogDebug($"Number: {rand}");
            if (rand < prob)
            {
                logger.LogDebug("Planning interception...");
                __instance.staticElectricityParticle.Stop();
                instance.LightningIntercepted = true;
                LGUStore.instance.CoordinateInterceptionClientRpc();
            }
        }
        public static void RerouteLightningBolt(ref Vector3 strikePosition, ref StormyWeather __instance)
        {
            logger.LogDebug($"Intercepted Lightning Strike...");
            Terminal terminal = UpgradeBus.instance.GetTerminal();
            strikePosition = terminal.transform.position;
            instance.LightningIntercepted = false;
            __instance.staticElectricityParticle.gameObject.SetActive(true);
        }
    }
}
