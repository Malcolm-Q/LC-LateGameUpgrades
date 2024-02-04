using GameNetcodeStuff;
using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc;
using MoreShipUpgrades.Misc.Upgrades;

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

        public override void Load()
        {
            base.Load();
            UpgradeBus.instance.sickBeats = true;
        }

        public override void Unwind()
        {
            base.Unwind();
            UpgradeBus.instance.sickBeats = false;
        }

        public static void HandlePlayerEffects(PlayerControllerB player)
        {
            UpgradeBus.instance.BoomboxIcon.SetActive(UpgradeBus.instance.EffectsActive);
            if (UpgradeBus.instance.EffectsActive)
            {
                logger.LogDebug("Applying effects!");
                logger.LogDebug($"Updating player's movement speed ({player.movementSpeed})");
                if (UpgradeBus.instance.cfg.BEATS_SPEED) player.movementSpeed += UpgradeBus.instance.cfg.BEATS_SPEED_INC;
                logger.LogDebug($"Updated player's movement speed ({player.movementSpeed})");
                if (UpgradeBus.instance.cfg.BEATS_STAMINA) UpgradeBus.instance.staminaDrainCoefficient = UpgradeBus.instance.cfg.BEATS_STAMINA_CO;
                if (UpgradeBus.instance.cfg.BEATS_DEF) UpgradeBus.instance.incomingDamageCoefficient = UpgradeBus.instance.cfg.BEATS_DEF_CO;
                if (UpgradeBus.instance.cfg.BEATS_DMG) UpgradeBus.instance.damageBoost = UpgradeBus.instance.cfg.BEATS_DMG_INC;
            }
            else
            {
                logger.LogDebug("Removing effects!");
                logger.LogDebug($"Updating player's movement speed ({player.movementSpeed})");
                if (UpgradeBus.instance.cfg.BEATS_SPEED) player.movementSpeed -= UpgradeBus.instance.cfg.BEATS_SPEED_INC;
                logger.LogDebug($"Updated player's movement speed ({player.movementSpeed})");
                UpgradeBus.instance.staminaDrainCoefficient = 1f;
                UpgradeBus.instance.incomingDamageCoefficient = 1f;
                UpgradeBus.instance.damageBoost = 0;
            }
        }

        public static int CalculateDefense(int dmg)
        {
            if (!UpgradeBus.instance.sickBeats || dmg < 0) return dmg; // < 0 check to not hinder healing
            return (int)(dmg * UpgradeBus.instance.incomingDamageCoefficient);
        }
    }
}
