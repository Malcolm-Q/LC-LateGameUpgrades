using GameNetcodeStuff;
using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc;
using System;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

namespace MoreShipUpgrades.UpgradeComponents
{
    internal class proteinPowderScript : BaseUpgrade
    {
        public static string UPGRADE_NAME = "Protein Powder";

        private static int CRIT_DAMAGE_VALUE = 100;

        // Configuration
        public static string ENABLED_SECTION = $"Enable {UPGRADE_NAME} Upgrade";
        public static bool ENABLED_DEFAULT = true;
        public static string ENABLED_DESCRIPTION = "Do more damage with shovels";

        public static string PRICE_SECTION = $"Price of {UPGRADE_NAME} Upgrade";
        public static int PRICE_DEFAULT = 1000;

        public static string UNLOCK_FORCE_SECTION = "Initial additional hit force";
        public static int UNLOCK_FORCE_DEFAULT = 1;
        public static string UNLOCK_FORCE_DESCRIPTION = "The value added to hit force on initial unlock.";

        public static string INCREMENT_FORCE_SECTION = "Additional hit force per level";
        public static int INCREMENT_FORCE_DEFAULT = 1;
        public static string INCREMENT_FORCE_DESCRIPTION = $"Every time {UPGRADE_NAME} is upgraded this value will be added to the value above.";

        public static string PRICES_DEFAULT = "700";

        public static string CRIT_CHANCE_SECTION = "Chance of dealing a crit which will instakill the enemy.";
        public static float CRIT_CHANCE_DEFAULT = 0.01f;
        public static string CRIT_CHANCE_DESCRIPTION = $"This value is only valid when maxed out {UPGRADE_NAME}. Any previous levels will not apply crit.";

        void Start()
        {
            upgradeName = UPGRADE_NAME;
            DontDestroyOnLoad(gameObject);
            Register();
        }

        public override void Increment()
        {
            UpgradeBus.instance.proteinLevel++;
        }

        public override void load()
        {
            base.load();

            UpgradeBus.instance.proteinPowder = true;
        }

        public override void Register()
        {
            base.Register();
        }

        public override void Unwind()
        {
            base.Unwind();

            UpgradeBus.instance.proteinLevel = 0;
            UpgradeBus.instance.proteinPowder = false;
        }

        public static int GetShovelHitForce(int force)
        {
            // Truly one of THE ternary operators
            return (UpgradeBus.instance.proteinPowder ? TryToCritEnemy() ? CRIT_DAMAGE_VALUE : UpgradeBus.instance.cfg.PROTEIN_INCREMENT * UpgradeBus.instance.proteinLevel + UpgradeBus.instance.cfg.PROTEIN_UNLOCK_FORCE + force : force) + UpgradeBus.instance.damageBoost;
            // .damageBoost is tied to boombox upgrade, it will always be 0 when inactive or x when active.
        }

        private static bool TryToCritEnemy()
        {
            int maximumLevel = UpgradeBus.instance.cfg.PROTEIN_UPGRADE_PRICES.Split(',').Length;
            int currentLevel = UpgradeBus.instance.proteinLevel;

            if (currentLevel != maximumLevel) return false;

            return UnityEngine.Random.value < UpgradeBus.instance.cfg.PROTEIN_CRIT_CHANCE;
        }
    }
}
