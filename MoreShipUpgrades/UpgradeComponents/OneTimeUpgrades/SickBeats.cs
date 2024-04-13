﻿using GameNetcodeStuff;
using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc;
using MoreShipUpgrades.Misc.Upgrades;
using System.Collections.Generic;
using UnityEngine;

namespace MoreShipUpgrades.UpgradeComponents.OneTimeUpgrades
{
    class SickBeats : OneTimeUpgrade
    {

        internal List<BoomboxItem> boomBoxes = new List<BoomboxItem>();
        internal float staminaDrainCoefficient = 1f;
        internal float incomingDamageCoefficient = 1f;
        internal int damageBoost;
        internal GameObject BoomboxIcon;
        internal bool EffectsActive;

        public const string UPGRADE_NAME = "Sick Beats";
        internal static SickBeats Instance;
        static LguLogger logger = new LguLogger(UPGRADE_NAME);

        void Awake()
        {
            upgradeName = UpgradeBus.Instance.PluginConfiguration.OVERRIDE_UPGRADE_NAMES ? UpgradeBus.Instance.PluginConfiguration.SICK_BEATS_OVERRIDE_NAME : UPGRADE_NAME;
            Instance = this;
        }
        internal override void Start()
        {
            base.Start();
            BoomboxIcon = transform.GetChild(0).GetChild(0).gameObject;
        }

        public static void HandlePlayerEffects(PlayerControllerB player)
        {
            Instance.BoomboxIcon.SetActive(Instance.EffectsActive);
            if (Instance.EffectsActive)
            {
                if (UpgradeBus.Instance.PluginConfiguration.BEATS_SPEED.Value) player.movementSpeed += UpgradeBus.Instance.PluginConfiguration.BEATS_SPEED_INC.Value;
                if (UpgradeBus.Instance.PluginConfiguration.BEATS_STAMINA.Value) Instance.staminaDrainCoefficient = UpgradeBus.Instance.PluginConfiguration.BEATS_STAMINA_CO.Value;
                if (UpgradeBus.Instance.PluginConfiguration.BEATS_DEF.Value) Instance.incomingDamageCoefficient = UpgradeBus.Instance.PluginConfiguration.BEATS_DEF_CO.Value;
                if (UpgradeBus.Instance.PluginConfiguration.BEATS_DMG.Value) Instance.damageBoost = UpgradeBus.Instance.PluginConfiguration.BEATS_DMG_INC.Value;
            }
            else
            {
                if (UpgradeBus.Instance.PluginConfiguration.BEATS_SPEED.Value) player.movementSpeed -= UpgradeBus.Instance.PluginConfiguration.BEATS_SPEED_INC.Value;
                Instance.staminaDrainCoefficient = 1f;
                Instance.incomingDamageCoefficient = 1f;
                Instance.damageBoost = 0;
            }
        }
        public static float ApplyPossibleIncreasedStaminaRegen(float regenValue)
        {
            if (!UpgradeBus.Instance.PluginConfiguration.BEATS_ENABLED.Value) return regenValue;
            if (!GetActiveUpgrade(UPGRADE_NAME)) return regenValue;
            return regenValue * Instance.staminaDrainCoefficient;
        }

        public static int GetShovelHitForce(int force)
        {
            if (!UpgradeBus.Instance.PluginConfiguration.BEATS_ENABLED.Value) return force;
            if (!GetActiveUpgrade(UPGRADE_NAME)) return force;
            return force + Instance.damageBoost;
        }

        public static int CalculateDefense(int dmg)
        {
            if (!GetActiveUpgrade(UPGRADE_NAME) || dmg < 0) return dmg; // < 0 check to not hinder healing
            return (int)(dmg * Instance.incomingDamageCoefficient);
        }

        public override string GetDisplayInfo(int price = -1)
        {
            string txt = $"Sick Beats - ${price}\nPlayers within a {UpgradeBus.Instance.PluginConfiguration.BEATS_RADIUS.Value} unit radius from an active boombox will have the following effects:\n\n";
            if (UpgradeBus.Instance.PluginConfiguration.BEATS_SPEED.Value) txt += $"Movement speed increased by {UpgradeBus.Instance.PluginConfiguration.BEATS_SPEED_INC.Value}\n";
            if (UpgradeBus.Instance.PluginConfiguration.BEATS_DMG.Value) txt += $"Damage inflicted increased by {UpgradeBus.Instance.PluginConfiguration.BEATS_DMG_INC.Value}\n";
            if (UpgradeBus.Instance.PluginConfiguration.BEATS_DEF.Value) txt += $"Incoming Damage multiplied by {UpgradeBus.Instance.PluginConfiguration.BEATS_DEF_CO.Value}\n";
            if (UpgradeBus.Instance.PluginConfiguration.BEATS_STAMINA.Value) txt += $"Stamina Drain multiplied by {UpgradeBus.Instance.PluginConfiguration.BEATS_STAMINA_CO.Value}\n";
            return txt;
        }
        internal override bool CanInitializeOnStart()
        {
            return UpgradeBus.Instance.PluginConfiguration.BEATS_PRICE.Value <= 0;
        }
    }
}
