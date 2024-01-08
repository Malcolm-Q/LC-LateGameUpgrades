using GameNetcodeStuff;
using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc;

namespace MoreShipUpgrades.UpgradeComponents
{
    public class BeatScript : BaseUpgrade
    {
        public static string UPGRADE_NAME = "Sick Beats";
        public static float PreviousMovementSpeed;

        void Start()
        {
            upgradeName = UPGRADE_NAME;
            DontDestroyOnLoad(gameObject);
            Register();
            UpgradeBus.instance.BoomboxIcon = transform.GetChild(0).GetChild(0).gameObject;
        }

        public override void load()
        {
            base.load();
            UpgradeBus.instance.sickBeats = true;
        }

        public override void Register()
        {
            base.Register();
        }

        public override void Unwind()
        {
            base.Unwind();
            UpgradeBus.instance.sickBeats = false;
        }

        public static void HandlePlayerEffects(PlayerControllerB player)
        {
            UpgradeBus.instance.BoomboxIcon.SetActive(UpgradeBus.instance.EffectsActive);
            if(UpgradeBus.instance.EffectsActive)
            {
                if (UpgradeBus.instance.cfg.BEATS_SPEED)
                {
                    PreviousMovementSpeed = player.movementSpeed; // I don't like this
                    player.movementSpeed += UpgradeBus.instance.cfg.BEATS_SPEED_INC;
                }
                if(UpgradeBus.instance.cfg.BEATS_STAMINA) UpgradeBus.instance.staminaDrainCoefficient = UpgradeBus.instance.cfg.BEATS_STAMINA_CO;
                if(UpgradeBus.instance.cfg.BEATS_DEF) UpgradeBus.instance.incomingDamageCoefficient = UpgradeBus.instance.cfg.BEATS_DEF_CO;
                if(UpgradeBus.instance.cfg.BEATS_DMG) UpgradeBus.instance.damageBoost = UpgradeBus.instance.cfg.BEATS_DMG_INC;
            }
            else
            {
                if (UpgradeBus.instance.cfg.BEATS_SPEED)
                {
                    player.movementSpeed = PreviousMovementSpeed; // but it should be fine...           right? 
                }
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
