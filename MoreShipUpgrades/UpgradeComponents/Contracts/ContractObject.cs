using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc;
using Unity.Netcode;

namespace MoreShipUpgrades.UpgradeComponents.Contracts
{
    internal class ContractObject : NetworkBehaviour
    {
        protected string contractType = null;
        static LGULogger logger = new LGULogger(nameof(ContractObject));
        public virtual void Start()
        {
            if (contractType == null) { logger.LogWarning($"contractType was not set on {gameObject.name}!"); }
            if (UpgradeBus.instance.contractType != contractType || StartOfRound.Instance.currentLevel.PlanetName != UpgradeBus.instance.contractLevel)
            {
                gameObject.SetActive(false);
                return;
            }
            logger.LogInfo($"{contractType}-{name} spawned and activated.");
            if (!(contractType == "exterminator" && IsHost)) return;

            Tools.SpawnMob("Hoarding bug", transform.position, UpgradeBus.instance.cfg.CONTRACT_BUG_SPAWNS);
        }

        public string GetContractType()
        {
            return contractType;
        }
    }
}
