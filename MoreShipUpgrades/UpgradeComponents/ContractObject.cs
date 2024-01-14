﻿using MoreShipUpgrades.Managers;
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
            else
            {
                logger.LogInfo($"{contractType}-{name} spawned and activated.");
                if(contractType == "exterminator" && IsHost)
                {
                    for(int i = 0; i < RoundManager.Instance.currentLevel.Enemies.Count; i++)
                    {
                        if (RoundManager.Instance.currentLevel.Enemies[i].enemyType.enemyName == "Hoarding bug")
                        {
                            for (int j = 0; j < UpgradeBus.instance.cfg.CONTRACT_BUG_SPAWNS; j++)
                            {
                                RoundManager.Instance.SpawnEnemyOnServer(transform.position, 0f,i);
                            }
                            break;
                        }
                    }
                }
            }
        }
    }
}