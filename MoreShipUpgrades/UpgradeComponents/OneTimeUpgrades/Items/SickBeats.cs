using GameNetcodeStuff;
using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc;
using MoreShipUpgrades.Misc.Upgrades;
using MoreShipUpgrades.UI.TerminalNodes;
using MoreShipUpgrades.UpgradeComponents.Interfaces;
using System.Collections.Generic;
using UnityEngine;

namespace MoreShipUpgrades.UpgradeComponents.OneTimeUpgrades.Items
{
    class SickBeats : OneTimeUpgrade, IUpgradeWorldBuilding
    {
        internal List<BoomboxItem> boomBoxes = [];
        internal float staminaDrainCoefficient = 1f;
        internal float incomingDamageCoefficient = 1f;
        internal int damageBoost;
        internal GameObject BoomboxIcon;
        internal bool EffectsActive;

        public const string UPGRADE_NAME = "Sick Beats";
        internal const string WORLD_BUILDING_TEXT = "\n\nYou negotiated a trade for a cassette that has music on it you actually enjoy, improving your department's morale by a tangible amount." +
            " Ship-to-Ship trading with other contractors is difficult and expensive. This cassette costed your department 6 TZP Inhalers and a Lockpicker." +
            " You left it for them to pick up on Vow, and the next day you collected your cassette from Experimentation on top of the pipe.\n\n";
        internal static SickBeats Instance;

        public string GetWorldBuildingText(bool shareStatus = false)
        {
            return WORLD_BUILDING_TEXT;
        }
        void Awake()
        {
            upgradeName = UPGRADE_NAME;
            overridenUpgradeName = GetConfiguration().SICK_BEATS_OVERRIDE_NAME;
            Instance = this;
        }
        internal override void Start()
        {
            base.Start();
            BoomboxIcon = transform.GetChild(0).GetChild(0).gameObject;
        }

        public static void HandlePlayerEffects(PlayerControllerB player)
        {
            LategameConfiguration config = GetConfiguration();
            Instance.BoomboxIcon.SetActive(Instance.EffectsActive);
            if (Instance.EffectsActive)
            {
                if (config.BEATS_SPEED.Value) player.movementSpeed += config.BEATS_SPEED_INC.Value;
                if (config.BEATS_STAMINA.Value) Instance.staminaDrainCoefficient = config.BEATS_STAMINA_CO.Value;
                if (config.BEATS_DEF.Value) Instance.incomingDamageCoefficient = config.BEATS_DEF_CO.Value;
                if (config.BEATS_DMG.Value) Instance.damageBoost = config.BEATS_DMG_INC.Value;
            }
            else
            {
                if (config.BEATS_SPEED.Value) player.movementSpeed -= config.BEATS_SPEED_INC.Value;
                Instance.staminaDrainCoefficient = 1f;
                Instance.incomingDamageCoefficient = 1f;
                Instance.damageBoost = 0;
            }
        }
        public static float MuteBoomboxToEnemies(float defaultRange)
        {
            LategameConfiguration config = GetConfiguration();
            if (!config.BEATS_ENABLED) return defaultRange;
            if (!GetActiveUpgrade(UPGRADE_NAME)) return defaultRange;
            if (config.SICK_BEATS_BOOMBOX_ATTRACT_SOUND) return defaultRange;
            return 0f;
        }
        public static float ApplyPossibleIncreasedStaminaRegen(float regenValue)
        {
            if (!GetConfiguration().BEATS_ENABLED.Value) return regenValue;
            if (!GetActiveUpgrade(UPGRADE_NAME)) return regenValue;
            return regenValue * Instance.staminaDrainCoefficient;
        }
        public static float ApplyPossibleDecreasedStaminaDrain(float drainValue)
        {
            LategameConfiguration config = GetConfiguration();
            if (!config.BEATS_ENABLED.Value) return drainValue;
            if (!GetActiveUpgrade(UPGRADE_NAME)) return drainValue;
            if (!config.SICK_BEATS_APPLY_STAMINA_CONSUMPTION) return drainValue;
            return drainValue * (1f - Mathf.Clamp(Instance.staminaDrainCoefficient - 1f, 0f, 1f));
        }

        public static int GetShovelHitForce(int force)
        {
            if (!GetConfiguration().BEATS_ENABLED.Value) return force;
            if (!GetActiveUpgrade(UPGRADE_NAME)) return force;
            return force + Instance.damageBoost;
        }

        public static int CalculateDefense(int dmg)
        {
            if (!GetConfiguration().BEATS_ENABLED.Value) return dmg;
            if (!GetActiveUpgrade(UPGRADE_NAME) || dmg < 0) return dmg; // < 0 check to not hinder healing
            return (int)(dmg * Instance.incomingDamageCoefficient);
        }

        public override string GetDisplayInfo(int price = -1)
        {
            LategameConfiguration config = GetConfiguration();
            string txt = $"Sick Beats - ${price}\nPlayers within a {config.BEATS_RADIUS.Value} unit radius from an active boombox will have the following effects:\n\n";
            if (config.BEATS_SPEED.Value) txt += $"Movement speed increased by {config.BEATS_SPEED_INC.Value}\n";
            if (config.BEATS_DMG.Value) txt += $"Damage inflicted increased by {config.BEATS_DMG_INC.Value}\n";
            if (config.BEATS_DEF.Value) txt += $"Incoming Damage multiplied by {config.BEATS_DEF_CO.Value}\n";
            if (config.BEATS_STAMINA.Value) txt += $"Stamina Regeneration multiplied by {config.BEATS_STAMINA_CO.Value}\n";
            if (config.SICK_BEATS_APPLY_STAMINA_CONSUMPTION) txt += $"Also applies on stamina drain overtime (with the multiplier value of {Mathf.Clamp(config.BEATS_STAMINA_CO - 1f, 0f, 1f)})\n";
            return txt;
        }
        public override bool CanInitializeOnStart => GetConfiguration().BEATS_PRICE.Value <= 0;
        public new static (string, string[]) RegisterScrapToUpgrade()
        {
            return (UPGRADE_NAME, GetConfiguration().SICK_BEATS_ITEM_PROGRESSION_ITEMS.Value.Split(","));
        }
        public new static void RegisterUpgrade()
        {
            SetupGenericPerk<SickBeats>(UPGRADE_NAME);
        }
        public new static CustomTerminalNode RegisterTerminalNode()
        {
            LategameConfiguration configuration = GetConfiguration();

            return UpgradeBus.Instance.SetupOneTimeTerminalNode(
                 UPGRADE_NAME,
                configuration.SHARED_UPGRADES.Value || !configuration.BEATS_INDIVIDUAL.Value,
                configuration.BEATS_ENABLED.Value,
                configuration.BEATS_PRICE.Value,
                configuration.OVERRIDE_UPGRADE_NAMES ? configuration.SICK_BEATS_OVERRIDE_NAME : "");
        }
    }
}
