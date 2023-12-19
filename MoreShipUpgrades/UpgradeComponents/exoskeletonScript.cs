using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc;

namespace MoreShipUpgrades.UpgradeComponents
{
    internal class exoskeletonScript : BaseUpgrade
    {
        
        void Start()
        {
            DontDestroyOnLoad(gameObject);
            Register();
        }

        public override void Increment()
        {
            UpgradeBus.instance.backLevel++;
        }

        public override void load()
        {
            UpgradeBus.instance.exoskeleton = true;
            HUDManager.Instance.chatText.text += "\n<color=#FF0000>Back Muscles is active!</color>";
        }
        public override void Unwind()
        {
            UpgradeBus.instance.exoskeleton = false;
            UpgradeBus.instance.backLevel = 0;
            HUDManager.Instance.chatText.text += "\n<color=#FF0000>Back Muscles has been disabled.</color>";
        }
        public override void Register()
        {
            if(!UpgradeBus.instance.UpgradeObjects.ContainsKey("Back Muscles")) { UpgradeBus.instance.UpgradeObjects.Add("Back Muscles", gameObject); }
        }
    }
}
