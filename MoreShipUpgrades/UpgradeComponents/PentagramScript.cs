using GameNetcodeStuff;
using MoreShipUpgrades.Managers;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;

namespace MoreShipUpgrades.UpgradeComponents
{
    internal class PentagramScript : NetworkBehaviour
    {
        InteractTrigger trig;
        bool placed = false;
        Animator anim;
        public GameObject loot;
        BoxCollider col;
        PlaceableObjectsSurface place;

        string DemonName;

        public static Dictionary<string, string[]> DemonInstructions = new Dictionary<string, string[]>
        {   // I just stole these from phasmaphobia idk anything about ghosts
            { "POLTERGEIST", new string[] {"a","b","c"} },
            { "PHANTOM", new string[] { "a","b","d" } },
            { "WRAITH", new string[] {"a", "b", "e" } },
            { "BANSHEE", new string[] {"a", "c", "d" } },
            { "JINN", new string[] {"a", "c", "e" } },
            { "HANTU", new string[] {"a", "d", "e" } },
            { "MOROI", new string[] {"b", "c", "d" } },
            { "MYLING", new string[] {"b", "c", "e" } },
            { "GORYO", new string[] {"b", "d", "e" } },
            { "DE OGEN", new string[] {"c", "d", "e" } },
        };

        public List<string> currentRitual = new List<string>();


        public AudioClip chant, portal;
        AudioSource audio;

        void Start()
        {
            place = GetComponent<PlaceableObjectsSurface>();
            audio = GetComponent<AudioSource>();
            trig = GetComponent<InteractTrigger>();
            trig.onInteract.AddListener(Interact);
            anim = GetComponent<Animator>();
            col = GetComponent<BoxCollider>();
            if (IsHost || IsServer) SyncAltarClientRpc(Random.Range(0, DemonInstructions.Count));
        }
        [ClientRpc]
        void SyncAltarClientRpc(int index)
        {
            currentRitual = DemonInstructions[DemonInstructions.Keys.ElementAt(index)].ToList();
            DemonName = DemonInstructions.Keys.ElementAt(index);
            trig.disabledHoverTip = $"{DemonName} ALTAR";
        }
        void Interact(PlayerControllerB player)
        {
            DisableGrabbableServerRpc(new NetworkBehaviourReference(player.currentlyHeldObjectServer));
            if (currentRitual.Contains(player.currentlyHeldObjectServer.itemProperties.itemName)) currentRitual.Remove(player.currentlyHeldObjectServer.itemProperties.itemName);
            else FailRitualServerRpc();
            place.PlaceObject(player);
            if(currentRitual.Count <= 0)
            {
                ReqRitualStartServerRpc();
            }
        }

        [ServerRpc(RequireOwnership = false)]
        void DisableGrabbableServerRpc(NetworkBehaviourReference netRef)
        {
            DisableGrabbableClientRpc(netRef);
        }

        [ClientRpc]
        void DisableGrabbableClientRpc(NetworkBehaviourReference netRef)
        {
            netRef.TryGet(out GrabbableObject go);
            if (go == null) return;
            Destroy(go.GetComponent<BoxCollider>());
            go.grabbable = false;
            go.grabbableToEnemies = false;
        }

        [ServerRpc(RequireOwnership = false)]
        void FailRitualServerRpc()
        {
            FailRitualClientRpc();

        }

        [ClientRpc]
        void FailRitualClientRpc()
        {
            placed = true;
            col.enabled = false;
            audio.Stop();
            audio.PlayOneShot(chant);
            StartCoroutine(WaitToExplode());
        }

        private IEnumerator WaitToExplode()
        {
            yield return new WaitForSeconds(2.5f);
            Landmine.SpawnExplosion(transform.position + new Vector3(0, 0.2f, 0), true, 10, 20);
            if (IsHost)
            {
                for (int i = 0; i < RoundManager.Instance.currentLevel.Enemies.Count; i++)
                {
                    Debug.Log(RoundManager.Instance.currentLevel.Enemies[i].enemyType.enemyName);
                    if (RoundManager.Instance.currentLevel.Enemies[i].enemyType.enemyName == "Girl")
                    {
                        for(int j = 0; j < UpgradeBus.instance.cfg.CONTRACT_GHOST_SPAWN; j++)
                        {
                            RoundManager.Instance.SpawnEnemyOnServer(transform.position + new Vector3(0, 0.15f, 0), 0f, i);
                        }
                        break;
                    }
                }
            }
            if(UpgradeBus.instance.cfg.CONTRACT_GHOST_SPAWN > 0) HUDManager.Instance.DisplayTip("RUN", "YOU HAVE ANGERED THE SPIRIT WORLD!");
        }

        [ServerRpc(RequireOwnership = false)]
        void ReqRitualStartServerRpc()
        {
            RitualStartClientRpc();
            GameObject go = Instantiate(loot,transform.position+new Vector3(0,0.1f,0),Quaternion.identity);
            go.GetComponent<NetworkObject>().Spawn();
        }

        [ClientRpc]
        void RitualStartClientRpc()
        {
            placed = true;
            col.enabled = false;
            anim.SetTrigger("Ritual");
            GetComponentInChildren<ParticleSystem>().Play();
            StartCoroutine(WaitALittleToStopParticlesGaming());
            audio.Stop();
            audio.PlayOneShot(portal);
        }

        private IEnumerator WaitALittleToStopParticlesGaming()
        {
            yield return new WaitForSeconds(3.25f);
            ParticleSystem sys = transform.GetChild(3).GetComponent<ParticleSystem>();
            sys.Stop();
            sys.Clear();
        }

        void Update()
        {
            if (placed) return;
            trig.interactable = EvalHeldItem();
        }
        bool EvalHeldItem()
        {
            PlayerControllerB player = GameNetworkManager.Instance.localPlayerController;
            if (player == null) return false;
            if (player.currentlyHeldObjectServer == null) return false;
            ContractObject co = player.currentlyHeldObjectServer.GetComponent<ContractObject>();
            if (co == null) return false;
            if (co.contractType == "exorcism") return true;
            return false;
        }
    }
}
