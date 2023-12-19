using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc;

namespace MoreShipUpgrades.UpgradeComponents
{
    internal class lightFootedScript : BaseUpgrade
    {
        void Start()
        {
            DontDestroyOnLoad(gameObject);
            Register();
        }

        public override void Increment()
        {
            UpgradeBus.instance.lightLevel++;
        }

        public override void load()
        {
            UpgradeBus.instance.softSteps = true;
            HUDManager.Instance.chatText.text += "\n<color=#FF0000>Light Footed is active!</color>";
        }
        public override void Unwind()
        {
            UpgradeBus.instance.softSteps = false;
            UpgradeBus.instance.lightLevel = 0;
            HUDManager.Instance.chatText.text += "\n<color=#FF0000>Light Footed has been disabled.</color>";
        }
        public override void Register()
        {
            if(!UpgradeBus.instance.UpgradeObjects.ContainsKey("Light Footed")) { UpgradeBus.instance.UpgradeObjects.Add("Light Footed", gameObject); }
        }
    }
}
