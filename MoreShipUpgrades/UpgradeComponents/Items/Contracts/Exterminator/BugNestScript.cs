using System;
using UnityEngine;
using Unity.Netcode;
using GameNetcodeStuff;
using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc;

namespace MoreShipUpgrades.UpgradeComponents.Items.Contracts.Exterminator
{
    internal class BugNestScript : NetworkBehaviour
    {
        Animator anim;
        public GameObject loot;
        InteractTrigger trig;
        int cleaning = 0;

        void Awake()
        {
            trig = GetComponent<InteractTrigger>();
            trig.onInteract.AddListener(CleanMess);
            trig.onStopInteract.AddListener(StopMess);
            trig.onInteractEarly.AddListener(PlayMess);

            anim = GetComponentInChildren<Animator>();
        }

        void Update()
        {
            anim.SetInteger("CleanStatus", cleaning);
        }

        [ServerRpc(RequireOwnership = false)]
        void SpawnLootServerRpc(Vector3 pos)
        {
            SpawnLoot(pos);
        }

        void SpawnLoot(Vector3 position)
        {
            GameObject go = Instantiate(loot, position + Vector3.up, Quaternion.identity);
            go.GetComponent<ScrapValueSyncer>().SetScrapValue(UpgradeBus.Instance.PluginConfiguration.CONTRACT_BUG_REWARD.Value + (int)(TimeOfDay.Instance.profitQuota * Mathf.Clamp(UpgradeBus.Instance.PluginConfiguration.CONTRACT_REWARD_QUOTA_MULTIPLIER.Value / 100f, 0f, 1f)));
            go.GetComponent<NetworkObject>().Spawn();
            DisableNestClientRpc(NetworkObject);
        }

        [ClientRpc]
        void DisableNestClientRpc(NetworkObjectReference netRef)
        {
            netRef.TryGet(out NetworkObject netObj);
            if (netObj != null)
            {
                netObj.Despawn();
            }
        }

        void CleanMess(PlayerControllerB player)
        {
            Vector3 lootPosition = transform.position + Vector3.up;

            if (IsHost || IsServer)
            {
                SpawnLoot(lootPosition);
            }
            else
            {
                SpawnLootServerRpc(lootPosition);
            }
        }

        void StopMess(PlayerControllerB player)
        {
            cleaning = 0;
        }

        void PlayMess(PlayerControllerB player)
        {
            cleaning = 1;
        }
    }
}
