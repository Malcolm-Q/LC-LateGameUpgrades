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

            loot.GetComponent<ScrapValueSyncer>().SetScrapValue(UpgradeBus.instance.cfg.CONTRACT_BUG_REWARD);

            anim = GetComponentInChildren<Animator>();
        }

        void Update()
        {
            anim.SetInteger("CleanStatus", cleaning);
        }

        [ServerRpc(RequireOwnership = false)]
        void SpawnLootServerRpc(Vector3 pos)
        {
            GameObject go = Instantiate(loot, pos + Vector3.up, Quaternion.identity);
            go.GetComponent<NetworkObject>().Spawn();
            DisableNestClientRpc(new NetworkObjectReference(gameObject));
        }

        [ClientRpc]
        void DisableNestClientRpc(NetworkObjectReference netRef)
        {
            netRef.TryGet(out NetworkObject netObj);
            if (netObj != null)
            {
                netObj.gameObject.SetActive(false);
            }
        }

        void CleanMess(PlayerControllerB player)
        {

            if (IsHost || IsServer)
            {
                GameObject go = Instantiate(loot, transform.position + Vector3.up, Quaternion.identity);
                go.GetComponent<NetworkObject>().Spawn();
                DisableNestClientRpc(new NetworkObjectReference(gameObject));
            }
            else
            {
                SpawnLootServerRpc(transform.position + Vector3.up);
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
