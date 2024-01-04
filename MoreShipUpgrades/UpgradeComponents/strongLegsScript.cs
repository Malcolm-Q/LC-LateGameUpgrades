using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc;

namespace MoreShipUpgrades.UpgradeComponents
{
    internal class strongLegsScript : BaseUpgrade
    {
        public static string UPGRADE_NAME = "Strong Legs";
        public static string PRICES_DEFAULT = "150,190,250";
        void Start()
        {
            upgradeName = UPGRADE_NAME;
            DontDestroyOnLoad(gameObject);
            Register();
        }

        public override void Increment()
        {
            UpgradeBus.instance.legLevel++;
            GameNetworkManager.Instance.localPlayerController.jumpForce += UpgradeBus.instance.cfg.JUMP_FORCE_INCREMENT;
        }

        public override void Unwind()
        {
            base.Unwind();

            GameNetworkManager.Instance.localPlayerController.jumpForce -= (UpgradeBus.instance.cfg.JUMP_FORCE_UNLOCK + (UpgradeBus.instance.legLevel * UpgradeBus.instance.cfg.JUMP_FORCE_INCREMENT));
            UpgradeBus.instance.strongLegs = false;
            UpgradeBus.instance.legLevel = 0;
        }
        public override void Register()
        {
            base.Register();
        }

        public override void load()
        {
            base.load();

            UpgradeBus.instance.strongLegs = true;
            GameNetworkManager.Instance.localPlayerController.jumpForce += UpgradeBus.instance.cfg.JUMP_FORCE_UNLOCK;
            float amountToIncrement = 0;
            for(int i = 0; i < UpgradeBus.instance.legLevel; i++)
            {
                amountToIncrement += UpgradeBus.instance.cfg.JUMP_FORCE_INCREMENT;
            }

            GameNetworkManager.Instance.localPlayerController.jumpForce += amountToIncrement;
        }

        public static int ReduceFallDamage(int defaultValue)
        {
            if (!(UpgradeBus.instance.strongLegs && UpgradeBus.instance.legLevel == UpgradeBus.instance.cfg.STRONG_LEGS_UPGRADE_PRICES.Split(',').Length)) return defaultValue;
            return (int)(defaultValue * (1.0f - UpgradeBus.instance.cfg.STRONG_LEGS_REDUCE_FALL_DAMAGE_MULTIPLIER));
        }
    }
}
