using MoreShipUpgrades.Misc.Upgrades;
using Unity.Netcode;

namespace MoreShipUpgrades.UpgradeComponents.Commands
{
    class ContractScript : NetworkBehaviour // Understand the purpose of this class better later
    {
        public static string NAME = "Contract";
        public static int lastContractIndex = -1;

        void Start()
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}
