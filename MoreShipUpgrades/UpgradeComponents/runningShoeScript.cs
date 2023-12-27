using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc;

namespace MoreShipUpgrades.UpgradeComponents
{
    internal class runningShoeScript : BaseUpgrade
    {
        public static string UPGRADE_NAME = "Running Shoes";
        public static string PRICES_DEFAULT = "500,750,1000";
        void Start()
        {
            upgradeName = UPGRADE_NAME;
            DontDestroyOnLoad(gameObject);
            Register();
        }

        public override void Increment()
        {
            GameNetworkManager.Instance.localPlayerController.movementSpeed += UpgradeBus.instance.cfg.MOVEMENT_INCREMENT;
            UpgradeBus.instance.runningLevel++;
        }

        public override void Unwind()
        {
            base.Unwind();

            UpgradeBus.instance.runningShoes = false;
            UpgradeBus.instance.runningLevel = 0;
            GameNetworkManager.Instance.localPlayerController.movementSpeed = 4.6f;
        }
        public override void Register()
        {
            base.Register();
        }

        public override void load()
        {
            base.load();

            UpgradeBus.instance.runningShoes = true;
            GameNetworkManager.Instance.localPlayerController.movementSpeed = UpgradeBus.instance.cfg.MOVEMENT_SPEED;

            float amountToIncrement = 0;
            for(int i = 0; i < UpgradeBus.instance.runningLevel; i++)
            {
                amountToIncrement += UpgradeBus.instance.cfg.MOVEMENT_INCREMENT;
            }

            GameNetworkManager.Instance.localPlayerController.movementSpeed += amountToIncrement;
        }
    }
}
