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

            for (int i = 0; i < RoundManager.Instance.currentLevel.Enemies.Count; i++)
            {
                if (RoundManager.Instance.currentLevel.Enemies[i].enemyType.enemyName != "Hoarding bug") continue;

                for (int j = 0; j < UpgradeBus.instance.cfg.CONTRACT_BUG_SPAWNS; j++)
                {
                    RoundManager.Instance.SpawnEnemyOnServer(transform.position, 0f, i);
                }
                break;
            }
        }

        public string GetContractType()
        {
            return contractType;
        }
    }
}
