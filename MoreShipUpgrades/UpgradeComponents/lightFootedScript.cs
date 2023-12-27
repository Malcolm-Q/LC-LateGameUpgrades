using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc;

namespace MoreShipUpgrades.UpgradeComponents
{
    internal class lightFootedScript : BaseUpgrade
    {
        public static string UPGRADE_NAME = "Light Footed";
        public static string PRICES_DEFAULT = "175,235,290";
        void Start()
        {
            upgradeName = UPGRADE_NAME;
            DontDestroyOnLoad(gameObject);
            Register();
        }

        public override void Increment()
        {
            UpgradeBus.instance.lightLevel++;
        }

        public override void load()
        {
            base.load();

            UpgradeBus.instance.softSteps = true;
        }
        public override void Unwind()
        {
            base.Unwind();
            UpgradeBus.instance.softSteps = false;
            UpgradeBus.instance.lightLevel = 0;
        }
        public override void Register()
        {
            base.Register();
        }
    }
}
