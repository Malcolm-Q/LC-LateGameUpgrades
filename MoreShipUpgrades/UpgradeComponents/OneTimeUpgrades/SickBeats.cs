using GameNetcodeStuff;
using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc;
using MoreShipUpgrades.Misc.Upgrades;
using MoreShipUpgrades.UpgradeComponents.Interfaces;

namespace MoreShipUpgrades.UpgradeComponents.OneTimeUpgrades
{
    class SickBeats : OneTimeUpgrade
    {
        public static string UPGRADE_NAME = "Sick Beats";
        private static LGULogger logger = new LGULogger(UPGRADE_NAME);

        internal override void Start()
        {
            upgradeName = UPGRADE_NAME;
            base.Start();
            UpgradeBus.instance.BoomboxIcon = transform.GetChild(0).GetChild(0).gameObject;
        }

        public static void HandlePlayerEffects(PlayerControllerB player)
        {
            UpgradeBus.instance.BoomboxIcon.SetActive(UpgradeBus.instance.EffectsActive);
            if (UpgradeBus.instance.EffectsActive)
            {
                logger.LogDebug("Applying effects!");
                logger.LogDebug($"Updating player's movement speed ({player.movementSpeed})");
                if (UpgradeBus.instance.cfg.BEATS_SPEED.Value) player.movementSpeed += UpgradeBus.instance.cfg.BEATS_SPEED_INC.Value;
                logger.LogDebug($"Updated player's movement speed ({player.movementSpeed})");
                if (UpgradeBus.instance.cfg.BEATS_STAMINA.Value) UpgradeBus.instance.staminaDrainCoefficient = UpgradeBus.instance.cfg.BEATS_STAMINA_CO.Value;
                if (UpgradeBus.instance.cfg.BEATS_DEF.Value) UpgradeBus.instance.incomingDamageCoefficient = UpgradeBus.instance.cfg.BEATS_DEF_CO.Value;
                if (UpgradeBus.instance.cfg.BEATS_DMG.Value) UpgradeBus.instance.damageBoost = UpgradeBus.instance.cfg.BEATS_DMG_INC.Value;
            }
            else
            {
                logger.LogDebug("Removing effects!");
                logger.LogDebug($"Updating player's movement speed ({player.movementSpeed})");
                if (UpgradeBus.instance.cfg.BEATS_SPEED.Value) player.movementSpeed -= UpgradeBus.instance.cfg.BEATS_SPEED_INC.Value;
                logger.LogDebug($"Updated player's movement speed ({player.movementSpeed})");
                UpgradeBus.instance.staminaDrainCoefficient = 1f;
                UpgradeBus.instance.incomingDamageCoefficient = 1f;
                UpgradeBus.instance.damageBoost = 0;
            }
        }

        public static int CalculateDefense(int dmg)
        {
            if (!GetActiveUpgrade(UPGRADE_NAME) || dmg < 0) return dmg; // < 0 check to not hinder healing
            return (int)(dmg * UpgradeBus.instance.incomingDamageCoefficient);
        }

        public override string GetDisplayInfo(int price = -1)
        {
            string txt = $"Sick Beats - ${price}\nPlayers within a {UpgradeBus.instance.cfg.BEATS_RADIUS} unit radius from an active boombox will have the following effects:\n\n";
            if (UpgradeBus.instance.cfg.BEATS_SPEED.Value) txt += $"Movement speed increased by {UpgradeBus.instance.cfg.BEATS_SPEED_INC.Value}\n";
            if (UpgradeBus.instance.cfg.BEATS_DMG.Value) txt += $"Damage inflicted increased by {UpgradeBus.instance.cfg.BEATS_DMG_INC.Value}\n";
            if (UpgradeBus.instance.cfg.BEATS_DEF.Value) txt += $"Incoming Damage multiplied by {UpgradeBus.instance.cfg.BEATS_DEF_CO.Value}\n";
            if (UpgradeBus.instance.cfg.BEATS_STAMINA.Value) txt += $"Stamina Drain multiplied by {UpgradeBus.instance.cfg.BEATS_STAMINA_CO.Value}\n";
            return txt;
        }
    }
}
