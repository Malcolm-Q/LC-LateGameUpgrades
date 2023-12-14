using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc;

namespace MoreShipUpgrades.UpgradeComponents
{
    internal class runningShoeScript : BaseUpgrade
    {
        void Start()
        {
            DontDestroyOnLoad(gameObject);
            UpgradeBus.instance.UpgradeObjects.Add("Running Shoes", gameObject);
        }

        public override void Increment()
        {
            GameNetworkManager.Instance.localPlayerController.movementSpeed += UpgradeBus.instance.cfg.MOVEMENT_INCREMENT;
            UpgradeBus.instance.runningLevel++;
        }

        public override void Unwind()
        {
            UpgradeBus.instance.runningShoes = false;
            UpgradeBus.instance.runningLevel = 0;
            GameNetworkManager.Instance.localPlayerController.movementSpeed = 4.6f;
            HUDManager.Instance.chatText.text += "\n<color=#FF0000>Running Shoes has been disabled.</color>";
        }
        public override void Register()
        {
            if(!UpgradeBus.instance.UpgradeObjects.ContainsKey("Running Shoes")) { UpgradeBus.instance.UpgradeObjects.Add("Running Shoes", gameObject); }
        }

        public override void load()
        {
            UpgradeBus.instance.runningShoes = true;
            GameNetworkManager.Instance.localPlayerController.movementSpeed = UpgradeBus.instance.cfg.MOVEMENT_SPEED;
            HUDManager.Instance.chatText.text += "\n<color=#FF0000>Running Shoes is active!</color>";
            float amountToIncrement = 0;
            for(int i = 0; i < UpgradeBus.instance.runningLevel; i++)
            {
                amountToIncrement += UpgradeBus.instance.cfg.MOVEMENT_INCREMENT;
            }

            GameNetworkManager.Instance.localPlayerController.movementSpeed += amountToIncrement;
        }
    }
}
