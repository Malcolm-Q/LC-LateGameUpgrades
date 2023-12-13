using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc;

namespace MoreShipUpgrades.UpgradeComponents
{
    internal class strongLegsScript : BaseUpgrade
    {
        void Start()
        {
            DontDestroyOnLoad(gameObject);
            UpgradeBus.instance.UpgradeObjects.Add("Strong Legs", gameObject);
        }

        public override void Increment()
        {
            UpgradeBus.instance.legLevel++;
        }

        public override void Unwind()
        {
            UpgradeBus.instance.strongLegs = false;
            UpgradeBus.instance.legLevel = 0;
            GameNetworkManager.Instance.localPlayerController.jumpForce = 13f;
            HUDManager.Instance.chatText.text += "\n<color=#FF0000>Better Scanner has been disabled.</color>";
        }
        public override void Register()
        {
            if(!UpgradeBus.instance.UpgradeObjects.ContainsKey("Strong Legs")) { UpgradeBus.instance.UpgradeObjects.Add("Strong Legs", gameObject); }
        }

        public override void load()
        {
            UpgradeBus.instance.strongLegs = true;
            GameNetworkManager.Instance.localPlayerController.jumpForce = UpgradeBus.instance.cfg.JUMP_FORCE;
            HUDManager.Instance.chatText.text += "\n<color=#FF0000>Strong Legs is active!</color>";
            float amountToIncrement = 0;
            for(int i = 0; i < UpgradeBus.instance.legLevel; i++)
            {
                amountToIncrement += UpgradeBus.instance.cfg.JUMP_FORCE_INCREMENT;
            }

            GameNetworkManager.Instance.localPlayerController.jumpForce += amountToIncrement;
        }


    }
}
