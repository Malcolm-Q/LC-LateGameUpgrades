using GameNetcodeStuff;
using MoreShipUpgrades.Configuration;
using MoreShipUpgrades.Configuration.Upgrades.Custom;
using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc.Upgrades;
using MoreShipUpgrades.UI.TerminalNodes;
using MoreShipUpgrades.UpgradeComponents.Interfaces;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem.PlaybackState;

namespace MoreShipUpgrades.UpgradeComponents.OneTimeUpgrades.Items
{
    class SickBeats : OneTimeUpgrade, IUpgradeWorldBuilding
    {
        internal List<BoomboxItem> boomBoxes = [];
        internal List<VehicleController> vehicleControllers = [];
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
            overridenUpgradeName = GetConfiguration().SickBeatsUpgradeConfiguration.OverrideName;
            Instance = this;
        }
        internal override void Start()
        {
            base.Start();
            BoomboxIcon = transform.GetChild(0).GetChild(0).gameObject;
        }

        public override void Unwind()
        {
            base.Unwind();
            Instance.BoomboxIcon.SetActive(false);
        }

        public static void HandlePlayerEffects(PlayerControllerB player)
        {
            SickBeatsUpgradeConfiguration config = GetConfiguration().SickBeatsUpgradeConfiguration;
            Instance.BoomboxIcon.SetActive(Instance.EffectsActive);
            if (Instance.EffectsActive)
            {
                if (config.EnableSpeed.Value) player.movementSpeed += config.SpeedBoost.Value;
                if (config.EnableStaminaRegen.Value) Instance.staminaDrainCoefficient = config.StaminaRegenBoost.Value;
                if (config.EnableDefense.Value) Instance.incomingDamageCoefficient = config.DefenseBoost.Value;
                if (config.EnableDamage.Value) Instance.damageBoost = config.DamageBoost.Value;
            }
            else
            {
                if (config.EnableSpeed.Value) player.movementSpeed -= config.SpeedBoost.Value;
                Instance.staminaDrainCoefficient = 1f;
                Instance.incomingDamageCoefficient = 1f;
                Instance.damageBoost = 0;
            }
        }
        public static float MuteBoomboxToEnemies(float defaultRange)
        {
            SickBeatsUpgradeConfiguration config = GetConfiguration().SickBeatsUpgradeConfiguration;
            if (!config.Enabled) return defaultRange;
            if (!GetActiveUpgrade(UPGRADE_NAME)) return defaultRange;
            if (config.BoomboxAttract) return defaultRange;
            return 0f;
        }
        public static float ApplyPossibleIncreasedStaminaRegen(float regenValue)
        {
            if (!GetConfiguration().SickBeatsUpgradeConfiguration.Enabled.Value) return regenValue;
            if (!GetActiveUpgrade(UPGRADE_NAME)) return regenValue;
            return regenValue * Instance.staminaDrainCoefficient;
        }
        public static float ApplyPossibleDecreasedStaminaDrain(float drainValue)
        {
            SickBeatsUpgradeConfiguration config = GetConfiguration().SickBeatsUpgradeConfiguration;
            if (!config.Enabled.Value) return drainValue;
            if (!GetActiveUpgrade(UPGRADE_NAME)) return drainValue;
            if (!config.ApplyStaminaDuringConsumption) return drainValue;
            return drainValue * (1f - Mathf.Clamp(Instance.staminaDrainCoefficient - 1f, 0f, 1f));
        }

        public static int GetShovelHitForce(int force)
        {
            if (!GetConfiguration().SickBeatsUpgradeConfiguration.Enabled.Value) return force;
            if (!GetActiveUpgrade(UPGRADE_NAME)) return force;
            return force + Instance.damageBoost;
        }

        public static int CalculateDefense(int dmg)
        {
            if (!GetConfiguration().SickBeatsUpgradeConfiguration.Enabled.Value) return dmg;
            if (!GetActiveUpgrade(UPGRADE_NAME) || dmg < 0) return dmg; // < 0 check to not hinder healing
            return (int)(dmg * Instance.incomingDamageCoefficient);
        }

        public override string GetDisplayInfo(int price = -1)
        {
            SickBeatsUpgradeConfiguration config = GetConfiguration().SickBeatsUpgradeConfiguration;
            string txt = $"Sick Beats - {GetUpgradePrice(price, config.PurchaseMode)}\nPlayers within a {config.Radius.Value} unit radius from an active boombox will have the following effects:\n\n";
            if (config.EnableSpeed.Value) txt += $"Movement speed increased by {config.SpeedBoost.Value}\n";
            if (config.EnableDamage.Value) txt += $"Damage inflicted increased by {config.DamageBoost.Value}\n";
            if (config.EnableDefense.Value) txt += $"Incoming Damage multiplied by {config.DefenseBoost.Value}\n";
            if (config.EnableStaminaRegen.Value)
            {
                txt += $"Stamina Regeneration multiplied by {config.StaminaRegenBoost.Value}\n";
                if (config.ApplyStaminaDuringConsumption) txt += $"Also applies on stamina drain overtime (with the multiplier value of {Mathf.Clamp(config.StaminaRegenBoost - 1f, 0f, 1f)})\n";
            }
            return txt;
        }
        public override bool CanInitializeOnStart => GetConfiguration().SickBeatsUpgradeConfiguration.Price.Value <= 0;
        public new static (string, string[]) RegisterScrapToUpgrade()
        {
            return (UPGRADE_NAME, GetConfiguration().SickBeatsUpgradeConfiguration.ItemProgressionItems.Value.Split(","));
        }
        public new static void RegisterUpgrade()
        {
            SetupGenericPerk<SickBeats>(UPGRADE_NAME);
        }
        public new static CustomTerminalNode RegisterTerminalNode()
        {

            return UpgradeBus.Instance.SetupOneTimeTerminalNode(
                 UPGRADE_NAME, GetConfiguration().SickBeatsUpgradeConfiguration);
        }
    }
}
