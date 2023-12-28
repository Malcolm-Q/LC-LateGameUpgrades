using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc;
using UnityEngine;

namespace MoreShipUpgrades.UpgradeComponents
{
    internal class beekeeperScript : BaseUpgrade
    {
        private static LGULogger logger = new LGULogger(UPGRADE_NAME);
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
            if (UpgradeBus.instance.beeLevel == UpgradeBus.instance.cfg.BEEKEEPER_UPGRADE_PRICES.Split(',').Length)
                LGUStore.instance.ToggleIncreaseHivePriceServerRpc();
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

        public static int GetHiveScrapValue(int originalValue)
        {
            if (!UpgradeBus.instance.increaseHivePrice) return originalValue;
            return (int)(originalValue * UpgradeBus.instance.cfg.BEEKEEPER_HIVE_VALUE_INCREASE);
        }
    }
}
