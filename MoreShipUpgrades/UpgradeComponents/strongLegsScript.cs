using GameNetcodeStuff;
using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc;

namespace MoreShipUpgrades.UpgradeComponents
{
    internal class strongLegsScript : BaseUpgrade
    {
        public static string UPGRADE_NAME = "Strong Legs";
        private static LGULogger logger;
        public static string PRICES_DEFAULT = "150,190,250";
        private int currentLevel = 0; // For "Load LGU" issues
        private static bool active = false;

        void Start()
        {
            upgradeName = UPGRADE_NAME;
            logger = new LGULogger(UPGRADE_NAME);
            DontDestroyOnLoad(gameObject);
            Register();
        }

        public override void Increment()
        {
            PlayerControllerB player = UpgradeBus.instance.GetLocalPlayer();
            player.jumpForce += UpgradeBus.instance.cfg.JUMP_FORCE_INCREMENT;
            UpgradeBus.instance.legLevel++;
            currentLevel++;
        }

        public override void Unwind()
        {
            base.Unwind();
            PlayerControllerB player = UpgradeBus.instance.GetLocalPlayer();
            logger.LogDebug($"Adding {UpgradeBus.instance.cfg.JUMP_FORCE_INCREMENT} to the player's jump force...");
            if (UpgradeBus.instance.strongLegs) ResetStrongLegsBuff(ref player);
            UpgradeBus.instance.strongLegs = false;
            UpgradeBus.instance.legLevel = 0;
            active = false;
            currentLevel = 0;
        }
        public override void Register()
        {
            base.Register();
        }

        public override void load()
        {
            base.load();

            UpgradeBus.instance.strongLegs = true;
            PlayerControllerB player = UpgradeBus.instance.GetLocalPlayer();
            if (!active)
            {
                logger.LogDebug($"Adding {UpgradeBus.instance.cfg.JUMP_FORCE_UNLOCK} to the player's jump force...");
                player.jumpForce += UpgradeBus.instance.cfg.JUMP_FORCE_UNLOCK;
            }
            float amountToIncrement = 0;
            for(int i = 1; i < UpgradeBus.instance.legLevel+1; i++)
            {
                if (i <= currentLevel) continue;

                logger.LogDebug($"Adding {UpgradeBus.instance.cfg.JUMP_FORCE_INCREMENT} to the player's jump force...");
                amountToIncrement += UpgradeBus.instance.cfg.JUMP_FORCE_INCREMENT;
            }

            player.jumpForce += amountToIncrement;
            active = true;
            currentLevel = UpgradeBus.instance.legLevel;
        }
        public static void ResetStrongLegsBuff(ref PlayerControllerB player)
        {
            float jumpForceRemoval = UpgradeBus.instance.cfg.JUMP_FORCE_UNLOCK;
            for (int i = 0; i < UpgradeBus.instance.legLevel; i++)
            {
                jumpForceRemoval += UpgradeBus.instance.cfg.JUMP_FORCE_INCREMENT;
            }
            logger.LogDebug($"Removing {player.playerUsername}'s jump force boost ({player.jumpForce}) with a boost of {jumpForceRemoval}");
            player.jumpForce -= jumpForceRemoval;
            logger.LogDebug($"Upgrade reset on {player.playerUsername}");
            active = false;
        }
        public static int ReduceFallDamage(int defaultValue)
        {
            if (!(UpgradeBus.instance.strongLegs && UpgradeBus.instance.legLevel == UpgradeBus.instance.cfg.STRONG_LEGS_UPGRADE_PRICES.Split(',').Length)) return defaultValue;
            return (int)(defaultValue * (1.0f - UpgradeBus.instance.cfg.STRONG_LEGS_REDUCE_FALL_DAMAGE_MULTIPLIER));
        }
    }
}
