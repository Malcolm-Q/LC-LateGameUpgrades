using MoreShipUpgrades.Misc;

namespace MoreShipUpgrades.UpgradeComponents
{
    public class ContractScript : BaseUpgrade
    {
        public static string UPGRADE_NAME = "Contract";
        void Start()
        {
            upgradeName = UPGRADE_NAME;
            DontDestroyOnLoad(gameObject);
            Register();
        }

        public override void Register()
        {
            base.Register();
        }
    }
}
