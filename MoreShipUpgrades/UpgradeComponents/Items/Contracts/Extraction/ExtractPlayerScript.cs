using GameNetcodeStuff;
using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace MoreShipUpgrades.UpgradeComponents.Items.Contracts.Extraction
{
    internal class ExtractPlayerScript : NetworkBehaviour
    {
        AudioSource audio;
        PhysicsProp prop;

        public static Dictionary<string, AudioClip[]> clipDict = new Dictionary<string, AudioClip[]>();

        bool hurtState;
        InteractTrigger trig;

        Animator anim;

        void Start()
        {
            prop = GetComponent<PhysicsProp>();
            GetComponent<ScrapValueSyncer>().SetScrapValue(UpgradeBus.instance.cfg.CONTRACT_EXTRACT_REWARD.Value);


            audio = GetComponent<AudioSource>();
            trig = GetComponentInChildren<InteractTrigger>();
            trig.onInteract.AddListener(InteractComplete);
            anim = GetComponent<Animator>();
            if (IsHost)
            {
                StartCoroutine(AudioStream());
            }
        }

        void Update()
        {
            if (prop.isHeld) anim.SetInteger("holdStatus", 1);
            else anim.SetInteger("holdStatus", 0);
            if (hurtState) return;
            trig.interactable = EvalMedKit();
        }

        bool EvalMedKit()
        {
            PlayerControllerB player = GameNetworkManager.Instance.localPlayerController;
            if (player == null)
            {
                return false;
            }
            if (player.currentlyHeldObjectServer == null)
            {
                return false;
            }
            if (player.currentlyHeldObjectServer.itemProperties.itemName == "Medkit") return true;
            else return false;
        }

        void InteractComplete(PlayerControllerB player)
        {
            if (IsHost || IsServer) HealScavClientRpc();
            else ReqHealScavServerRpc();
        }

        [ServerRpc(RequireOwnership = false)]
        void ReqHealScavServerRpc()
        {
            HealScavClientRpc();
        }

        [ClientRpc]
        void HealScavClientRpc()
        {
            trig.GetComponent<BoxCollider>().enabled = false;
            audio.PlayOneShot(clipDict["heal"][0], UpgradeBus.instance.cfg.SCAV_VOLUME.Value);
            anim.SetTrigger("heal");
            hurtState = false;
            StartCoroutine(WaitForHealAnim());
            GetComponent<PhysicsProp>().enabled = true;
        }

        private IEnumerator WaitForHealAnim()
        {
            yield return new WaitForSeconds(1f);
            GetComponent<BoxCollider>().enabled = true;
        }

        private IEnumerator AudioStream()
        {
            float TimeToWait = Random.Range(25f, 45f);
            if (prop.isInShipRoom) TimeToWait *= 3f;
            yield return new WaitForSeconds(TimeToWait);
            if (prop.isHeld)
            {
                PlayAudioClientRpc(Random.Range(0, clipDict["held"].Length), "held");
            }
            else if (prop.isInShipRoom)
            {
                PlayAudioClientRpc(Random.Range(0, clipDict["safe"].Length), "safe");
            }
            else
            {
                PlayAudioClientRpc(Random.Range(0, clipDict["lost"].Length), "lost");
            }
            StartCoroutine(AudioStream());
        }

        [ClientRpc]
        void PlayAudioClientRpc(int index, string soundType)
        {
            audio.PlayOneShot(clipDict[soundType][index], UpgradeBus.instance.cfg.SCAV_VOLUME.Value);
            RoundManager.Instance.PlayAudibleNoise(transform.position, 30f, 0.9f, 0, prop.isInShipRoom, 5);
        }
    }
}
