using GameNetcodeStuff;
using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc;
using MoreShipUpgrades.Misc.Upgrades;

namespace MoreShipUpgrades.UpgradeComponents.OneTimeUpgrades
{
    class SickBeats : OneTimeUpgrade
    {
        public static string UPGRADE_NAME = "Sick Beats";
        private static LguLogger logger = new LguLogger(UPGRADE_NAME);

        internal override void Start()
        {
            upgradeName = UPGRADE_NAME;
            base.Start();
            UpgradeBus.Instance.BoomboxIcon = transform.GetChild(0).GetChild(0).gameObject;
        }

        public static void HandlePlayerEffects(PlayerControllerB player)
        {
            UpgradeBus.Instance.BoomboxIcon.SetActive(UpgradeBus.Instance.EffectsActive);
            if (UpgradeBus.Instance.EffectsActive)
            {
                logger.LogDebug("Applying effects!");
                logger.LogDebug($"Updating player's movement speed ({player.movementSpeed})");
                if (UpgradeBus.Instance.PluginConfiguration.BEATS_SPEED.Value) player.movementSpeed += UpgradeBus.Instance.PluginConfiguration.BEATS_SPEED_INC.Value;
                logger.LogDebug($"Updated player's movement speed ({player.movementSpeed})");
                if (UpgradeBus.Instance.PluginConfiguration.BEATS_STAMINA.Value) UpgradeBus.Instance.staminaDrainCoefficient = UpgradeBus.Instance.PluginConfiguration.BEATS_STAMINA_CO.Value;
                if (UpgradeBus.Instance.PluginConfiguration.BEATS_DEF.Value) UpgradeBus.Instance.incomingDamageCoefficient = UpgradeBus.Instance.PluginConfiguration.BEATS_DEF_CO.Value;
                if (UpgradeBus.Instance.PluginConfiguration.BEATS_DMG.Value) UpgradeBus.Instance.damageBoost = UpgradeBus.Instance.PluginConfiguration.BEATS_DMG_INC.Value;
            }
            else
            {
                logger.LogDebug("Removing effects!");
                logger.LogDebug($"Updating player's movement speed ({player.movementSpeed})");
                if (UpgradeBus.Instance.PluginConfiguration.BEATS_SPEED.Value) player.movementSpeed -= UpgradeBus.Instance.PluginConfiguration.BEATS_SPEED_INC.Value;
                logger.LogDebug($"Updated player's movement speed ({player.movementSpeed})");
                UpgradeBus.Instance.staminaDrainCoefficient = 1f;
                UpgradeBus.Instance.incomingDamageCoefficient = 1f;
                UpgradeBus.Instance.damageBoost = 0;
            }
        }

        public static int CalculateDefense(int dmg)
        {
            if (!GetActiveUpgrade(UPGRADE_NAME) || dmg < 0) return dmg; // < 0 check to not hinder healing
            return (int)(dmg * UpgradeBus.Instance.incomingDamageCoefficient);
        }

        public override string GetDisplayInfo(int price = -1)
        {
            string txt = $"Sick Beats - ${price}\nPlayers within a {UpgradeBus.Instance.PluginConfiguration.BEATS_RADIUS} unit radius from an active boombox will have the following effects:\n\n";
            if (UpgradeBus.Instance.PluginConfiguration.BEATS_SPEED.Value) txt += $"Movement speed increased by {UpgradeBus.Instance.PluginConfiguration.BEATS_SPEED_INC.Value}\n";
            if (UpgradeBus.Instance.PluginConfiguration.BEATS_DMG.Value) txt += $"Damage inflicted increased by {UpgradeBus.Instance.PluginConfiguration.BEATS_DMG_INC.Value}\n";
            if (UpgradeBus.Instance.PluginConfiguration.BEATS_DEF.Value) txt += $"Incoming Damage multiplied by {UpgradeBus.Instance.PluginConfiguration.BEATS_DEF_CO.Value}\n";
            if (UpgradeBus.Instance.PluginConfiguration.BEATS_STAMINA.Value) txt += $"Stamina Drain multiplied by {UpgradeBus.Instance.PluginConfiguration.BEATS_STAMINA_CO.Value}\n";
            return txt;
        }
    }
}
