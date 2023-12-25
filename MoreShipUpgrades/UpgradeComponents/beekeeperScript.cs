using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc;
using UnityEngine;

namespace MoreShipUpgrades.UpgradeComponents
{
    internal class beekeeperScript : BaseUpgrade
    {
        void Start()
        {
            DontDestroyOnLoad(gameObject);
            Register();
        }

        public override void Increment()
        {
            UpgradeBus.instance.beeLevel++;
        }

        public override void load()
        {
            UpgradeBus.instance.beekeeper = true;
            HUDManager.Instance.chatText.text += "\n<color=#FF0000>Beekeeper is active!</color>";
        }

        public override void Register()
        {
            if(!UpgradeBus.instance.UpgradeObjects.ContainsKey("Beekeeper")) { UpgradeBus.instance.UpgradeObjects.Add("Beekeeper", gameObject); }
        }

        public override void Unwind()
        {
            UpgradeBus.instance.beeLevel = 0;
            UpgradeBus.instance.beekeeper = false;
            HUDManager.Instance.chatText.text += "\n<color=#FF0000>Beekeeper has been disabled</color>";
        }

        public static int CalculateBeeDamage(int damageNumber)
        {
            if (!UpgradeBus.instance.beekeeper) return damageNumber;
            return Mathf.Clamp((int)(damageNumber * (UpgradeBus.instance.cfg.BEEKEEPER_DAMAGE_MULTIPLIER - (UpgradeBus.instance.beeLevel * UpgradeBus.instance.cfg.BEEKEEPER_DAMAGE_MULTIPLIER_INCREMENT))), 0, damageNumber);
        }
    }
}
