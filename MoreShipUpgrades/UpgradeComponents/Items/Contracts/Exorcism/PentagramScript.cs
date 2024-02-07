using GameNetcodeStuff;
using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc;
using MoreShipUpgrades.UpgradeComponents.Contracts;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;

namespace MoreShipUpgrades.UpgradeComponents.Items.Contracts.Exorcism
{
    internal class PentagramScript : NetworkBehaviour
    {
        InteractTrigger trig;
        bool placed = false;
        Animator anim;
        public GameObject loot;
        BoxCollider col;
        PlaceableObjectsSurface place;
        LGULogger logger = new LGULogger(nameof(PentagramScript));

        string DemonName;

        public static Dictionary<string, string[]> DemonInstructions = new Dictionary<string, string[]>
        {   // I just stole these from phasmaphobia idk anything about ghosts
            { "POLTERGEIST", new string[] {"Heart","Bones","Crucifix"} },
            { "PHANTOM", new string[] { "Heart","Bones","Candelabra" } },
            { "WRAITH", new string[] {"Heart", "Bones", "Teddy Bear" } },
            { "BANSHEE", new string[] {"Heart", "Crucifix", "Candelabra" } },
            { "JINN", new string[] {"Heart", "Crucifix", "Teddy Bear" } },
            { "HANTU", new string[] {"Heart", "Candelabra", "Teddy Bear" } },
            { "MOROI", new string[] {"Bones", "Crucifix", "Candelabra" } },
            { "MYLING", new string[] {"Bones", "Crucifix", "Teddy Bear" } },
            { "GORYO", new string[] {"Bones", "Candelabra", "Teddy Bear" } },
            { "DE OGEN", new string[] {"Crucifix", "Candelabra", "Teddy Bear" } },
        };

        public List<string> currentRitual = new List<string>();


        public AudioClip chant, portal;
        AudioSource audio;

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
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
            if (trig == null) trig = GetComponent<InteractTrigger>();
            trig.disabledHoverTip = $"{DemonName} ALTAR";
        }
        void Interact(PlayerControllerB player)
        {
            DisableGrabbableServerRpc(new NetworkBehaviourReference(player.currentlyHeldObjectServer));
            if (currentRitual.Contains(player.currentlyHeldObjectServer.itemProperties.itemName))
            {
                if (IsHost || IsServer) SyncCurrentRitualClientRpc(player.currentlyHeldObjectServer.itemProperties.itemName);
                else SyncCurrentRitualServerRpc(player.currentlyHeldObjectServer.itemProperties.itemName);
            }
            else FailRitualServerRpc();
            place.PlaceObject(player);
        }

        [ServerRpc(RequireOwnership = false)]
        void SyncCurrentRitualServerRpc(string toRemove)
        {
            SyncCurrentRitualClientRpc(toRemove);
        }

        [ClientRpc]
        void SyncCurrentRitualClientRpc(string toRemove)
        {
            logger.LogInfo($"Removing {toRemove} from currentRitual...");
            if (currentRitual.Contains(toRemove)) currentRitual.Remove(toRemove);
            else logger.LogWarning($"{toRemove} was not found in currentRitual!");
            if(currentRitual.Count <= 0)
            {
                logger.LogInfo("Ritual starting...");
                placed = true;
                col.enabled = false;
                anim.SetTrigger("Ritual");
                GetComponentInChildren<ParticleSystem>().Play();
                StartCoroutine(WaitALittleToStopParticlesGaming());
                audio.Stop();
                audio.PlayOneShot(portal);
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
            yield return new WaitForSeconds(0.5f);
            if (IsHost)
            {
                if (!Tools.SpawnMob("Girl", transform.position + new Vector3(0, 0.15f, 0),UpgradeBus.instance.cfg.CONTRACT_GHOST_SPAWN))
                {
                    Tools.SpawnMob("Crawler", transform.position + new Vector3(0, 0.15f, 0),UpgradeBus.instance.cfg.CONTRACT_GHOST_SPAWN);
                }
            }
            if(UpgradeBus.instance.cfg.CONTRACT_GHOST_SPAWN > 0) HUDManager.Instance.DisplayTip("RUN", "YOU HAVE ANGERED THE SPIRIT WORLD!");
        }



        private IEnumerator WaitALittleToStopParticlesGaming()
        {
            yield return new WaitForSeconds(3f);
            ParticleSystem sys = transform.GetChild(3).GetComponent<ParticleSystem>();
            sys.Stop();
            if (IsHost || IsServer)
            {
                GameObject go = Instantiate(loot, transform.position + new Vector3(0, 0.1f, 0), Quaternion.identity);
                go.GetComponent<ScrapValueSyncer>().SetScrapValue(UpgradeBus.instance.cfg.CONTRACT_EXOR_REWARD);
                go.GetComponent<NetworkObject>().Spawn();
            }
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
            ExorcismContract co = player.currentlyHeldObjectServer.GetComponent<ExorcismContract>();
            if (co == null) return false;
            return true;
        }
    }
}
