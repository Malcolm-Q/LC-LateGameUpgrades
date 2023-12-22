using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc;
using UnityEngine;

namespace MoreShipUpgrades.UpgradeComponents
{
    internal class strongerScannerScript : BaseUpgrade
    {

        void Start()
        {
            DontDestroyOnLoad(gameObject);
            Register();
        }

        public override void Increment()
        {
            UpgradeBus.instance.scanLevel++;
        }

        public override void load()
        {
            UpgradeBus.instance.scannerUpgrade = true;
            HUDManager.Instance.chatText.text += "\n<color=#FF0000>Better Scanner is active!</color>";
        }
        public override void Unwind()
        {
            UpgradeBus.instance.scannerUpgrade = false;
            UpgradeBus.instance.scanLevel = 0;
            HUDManager.Instance.chatText.text += "\n<color=#FF0000>Better Scanner has been disabled.</color>";
        }
        public override void Register()
        {
            Debug.Log("SLKDJF");
            if(!UpgradeBus.instance.UpgradeObjects.ContainsKey("Better Scanner")) { UpgradeBus.instance.UpgradeObjects.Add("Better Scanner", gameObject); }
        }
    }
}
