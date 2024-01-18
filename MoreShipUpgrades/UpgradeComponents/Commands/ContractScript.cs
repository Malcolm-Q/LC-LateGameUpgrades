using MoreShipUpgrades.Misc;

namespace MoreShipUpgrades.UpgradeComponents.Commands
{
    public class ContractScript : BaseUpgrade
    {
        public static string UPGRADE_NAME = "Contract";
        public static int lastContractIndex = -1;

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
