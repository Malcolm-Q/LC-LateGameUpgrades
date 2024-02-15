using MoreShipUpgrades.Misc.Upgrades;
using Unity.Netcode;

namespace MoreShipUpgrades.UpgradeComponents.Commands
{
    class ContractScript : NetworkBehaviour
    {
        public const string NAME = "Contract";
        public static int lastContractIndex = -1;

        void Start()
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}
