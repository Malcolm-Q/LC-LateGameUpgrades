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
        }

        public override void Unwind()
        {
            base.Unwind();

            UpgradeBus.instance.strongLegs = false;
            UpgradeBus.instance.legLevel = 0;
            GameNetworkManager.Instance.localPlayerController.jumpForce = 13f;
        }
        public override void Register()
        {
            base.Register();
        }

        public override void load()
        {
            base.load();

            UpgradeBus.instance.strongLegs = true;
            GameNetworkManager.Instance.localPlayerController.jumpForce = UpgradeBus.instance.cfg.JUMP_FORCE;
            float amountToIncrement = 0;
            for(int i = 0; i < UpgradeBus.instance.legLevel; i++)
            {
                amountToIncrement += UpgradeBus.instance.cfg.JUMP_FORCE_INCREMENT;
            }

            GameNetworkManager.Instance.localPlayerController.jumpForce += amountToIncrement;
        }


    }
}
