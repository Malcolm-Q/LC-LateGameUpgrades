using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc;
using UnityEngine;

namespace MoreShipUpgrades.UpgradeComponents
{
    internal class beekeeperScript : BaseUpgrade
    {
        public static string UPGRADE_NAME = "Beekeeper";
        public static string PRICES_DEFAULT = "225,280,340";
        void Start()
        {
            upgradeName = UPGRADE_NAME;
            DontDestroyOnLoad(gameObject);
            Register();
        }

        public override void Increment()
        {
            UpgradeBus.instance.beeLevel++;
        }

        public override void load()
        {
            base.load();
            UpgradeBus.instance.beekeeper = true;
        }

        public override void Register()
        {
            base.Register();
        }

        public override void Unwind()
        {
            base.Unwind();

            UpgradeBus.instance.beeLevel = 0;
            UpgradeBus.instance.beekeeper = false;
        }

        public static int CalculateBeeDamage(int damageNumber)
        {
            if (!UpgradeBus.instance.beekeeper) return damageNumber;
            return Mathf.Clamp((int)(damageNumber * (UpgradeBus.instance.cfg.BEEKEEPER_DAMAGE_MULTIPLIER - (UpgradeBus.instance.beeLevel * UpgradeBus.instance.cfg.BEEKEEPER_DAMAGE_MULTIPLIER_INCREMENT))), 0, damageNumber);
        }
    }
}
