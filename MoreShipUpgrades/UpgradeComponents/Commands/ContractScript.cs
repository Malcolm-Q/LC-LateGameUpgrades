using MoreShipUpgrades.Misc.Upgrades;
using Unity.Netcode;

namespace MoreShipUpgrades.UpgradeComponents.Commands
{
    class ContractScript : NetworkBehaviour
    {
        public const string NAME = "Contract";

        void Start()
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}
