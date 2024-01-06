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
        private bool active = false;
        void Start()
        {
            upgradeName = UPGRADE_NAME;
            logger = new LGULogger(UPGRADE_NAME);
            DontDestroyOnLoad(gameObject);
            Register();
        }

        public override void Increment()
        {
            UpgradeBus.instance.legLevel++;
            currentLevel++;
            GameNetworkManager.Instance.localPlayerController.jumpForce += UpgradeBus.instance.cfg.JUMP_FORCE_INCREMENT;
        }

        public override void Unwind()
        {
            base.Unwind();

            GameNetworkManager.Instance.localPlayerController.jumpForce -= (UpgradeBus.instance.cfg.JUMP_FORCE_UNLOCK + (UpgradeBus.instance.legLevel * UpgradeBus.instance.cfg.JUMP_FORCE_INCREMENT));
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
            if (!active)GameNetworkManager.Instance.localPlayerController.jumpForce += UpgradeBus.instance.cfg.JUMP_FORCE_UNLOCK;
            float amountToIncrement = 0;
            for(int i = 1; i < UpgradeBus.instance.legLevel+1; i++)
            {
                if (i <= currentLevel) continue;

                logger.LogDebug($"Adding {UpgradeBus.instance.cfg.JUMP_FORCE_INCREMENT} to the player's jump force...");
                amountToIncrement += UpgradeBus.instance.cfg.JUMP_FORCE_INCREMENT;
            }

            GameNetworkManager.Instance.localPlayerController.jumpForce += amountToIncrement;
            active = true;
            currentLevel = UpgradeBus.instance.legLevel;
        }

        public static int ReduceFallDamage(int defaultValue)
        {
            if (!(UpgradeBus.instance.strongLegs && UpgradeBus.instance.legLevel == UpgradeBus.instance.cfg.STRONG_LEGS_UPGRADE_PRICES.Split(',').Length)) return defaultValue;
            return (int)(defaultValue * (1.0f - UpgradeBus.instance.cfg.STRONG_LEGS_REDUCE_FALL_DAMAGE_MULTIPLIER));
        }
    }
}
