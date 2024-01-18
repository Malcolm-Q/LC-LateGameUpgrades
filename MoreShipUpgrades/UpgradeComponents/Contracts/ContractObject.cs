using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;

namespace MoreShipUpgrades.UpgradeComponents.Contracts
{
    internal class ContractObject : NetworkBehaviour
    {
        protected string contractType = null;
        static LGULogger logger = new LGULogger(nameof(ContractObject));
        public bool SetPosition = false;
        public virtual void Start()
        {
            if (contractType == null) { logger.LogWarning($"contractType was not set on {gameObject.name}!"); }
            if (UpgradeBus.instance.contractType != contractType || StartOfRound.Instance.currentLevel.PlanetName != UpgradeBus.instance.contractLevel)
            {
                gameObject.SetActive(false);
                return;
            }
            logger.LogInfo($"{contractType}-{name} spawned and activated.");
            if (SetPosition && IsHost)
            {
                // make sure physicsprop is disabled if using this!
                // and add a networktransform or set up rpcs to do this on client
                List<EntranceTeleport> mainDoors = FindObjectsOfType<EntranceTeleport>().Where(obj => obj.gameObject.transform.position.y <= -170).ToList();
                EnemyVent[] vents = FindObjectsOfType<EnemyVent>();
                EnemyVent spawnVent = null;
                if(UpgradeBus.instance.cfg.MAIN_OBJECT_FURTHEST)
                {
                    spawnVent = vents.OrderByDescending(vent => Vector3.Distance(mainDoors[0].transform.position, vent.floorNode.position)).First();
                }
                else
                {
                    spawnVent = vents[Random.Range(0,vents.Length)];
                }
                Vector3 offsetVector = Quaternion.Euler(0f, spawnVent.floorNode.eulerAngles.y, 0f) * Vector3.forward;
                Vector3 newPosition = spawnVent.floorNode.position + offsetVector;
                transform.position = newPosition;
            }
            if (contractType != "exterminator") return;

            Tools.SpawnMob("Hoarding bug", transform.position, UpgradeBus.instance.cfg.CONTRACT_BUG_SPAWNS);
        }

        public string GetContractType()
        {
            return contractType;
        }
    }
}
