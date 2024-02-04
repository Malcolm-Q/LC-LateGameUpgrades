using MoreShipUpgrades.Managers;
using System;
using System.Collections.Generic;
using System.Text;

namespace MoreShipUpgrades.Misc.Upgrades
{
    abstract class OneTimeUpgrade : BaseUpgrade
    {
        internal override void Start()
        {
            DontDestroyOnLoad(gameObject);
            Register();
        }
        public override void Load()
        {
            string loadColour = "#FF0000";
            string loadMessage = $"\n<color={loadColour}>{upgradeName} is active!</color>";
            HUDManager.Instance.chatText.text += loadMessage;
        }

        public override void Register()
        {
            if (!UpgradeBus.instance.UpgradeObjects.ContainsKey(upgradeName)) { UpgradeBus.instance.UpgradeObjects.Add(upgradeName, gameObject); }
        }

        public override void Unwind()
        {
            string unloadColour = "#FF0000";
            string unloadMessage = $"\n<color={unloadColour}>{upgradeName} has been disabled!</color>";
            HUDManager.Instance.chatText.text += unloadMessage;
        }
    }
}
