using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc;

namespace MoreShipUpgrades.UpgradeComponents
{
    internal class strongerScannerScript : BaseUpgrade
    {

        void Start()
        {
            DontDestroyOnLoad(gameObject);
            UpgradeBus.instance.UpgradeObjects.Add("Better Scanner", gameObject);
        }

        public override void load()
        {
            UpgradeBus.instance.scannerUpgrade = true;
            HUDManager.Instance.chatText.text += "\n<color=#FF0000>Better Scanner is active!</color>";
        }

        public override void Register()
        {
            if(!UpgradeBus.instance.UpgradeObjects.ContainsKey("Better Scanner")) { UpgradeBus.instance.UpgradeObjects.Add("Better Scanner", gameObject); }
        }
    }
}
