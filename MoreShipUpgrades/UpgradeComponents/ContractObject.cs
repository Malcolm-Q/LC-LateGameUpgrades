using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc;
using Unity.Netcode;

namespace MoreShipUpgrades.UpgradeComponents
{
    internal class ContractObject : NetworkBehaviour
    {
        public string contractType = null;
        static LGULogger logger = new LGULogger(nameof(ContractObject));
        void Start()
        {
            if(contractType == null) { logger.LogWarning($"contractType was not set on {gameObject.name}!"); }
            if(UpgradeBus.instance.contractType != contractType || StartOfRound.Instance.currentLevel.PlanetName != UpgradeBus.instance.contractLevel)
            {
                gameObject.SetActive(false);
            }
        }
    }
}
