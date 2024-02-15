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
        /// <summary>
        /// Interact trigger script associated with altar to display the ghost's name and deposit text when holding a ritual item
        /// </summary>
        InteractTrigger trig;
        /// <summary>
        /// Check used to stop the player from placing more ritual items into the pentagram
        /// </summary>
        bool placed = false;
        /// <summary>
        /// Animator used to display animations such as ghost girl appearing when successful
        /// </summary>
        Animator anim;
        /// <summary>
        /// The loot that spawns when completing the ritual sucessfully
        /// </summary>
        public GameObject loot;
        /// <summary>
        /// Collider used to allow interaction
        /// </summary>
        BoxCollider collider;
        /// <summary>
        /// Surface where the ritual items will be placed when interacted with the pentagram
        /// </summary>
        PlaceableObjectsSurface place;
        LguLogger logger = new LguLogger(nameof(PentagramScript));

        string DemonName;

        /// <summary>
        /// Dictionary used to store correct ritual items for each ghost entry
        /// </summary>
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

        /// <summary>
        /// The ghost selected and its respective ritual items to suceed at the ritual
        /// </summary>
        public List<string> currentRitual = new List<string>();

        /// <summary>
        /// Sounds used in the pentagram
        /// </summary>
        public AudioClip chant, portal;
        /// <summary>
        /// Source used to play audio on
        /// </summary>
        AudioSource audio;
        /// <summary>
        /// Gets all the relevant components for manipulation when spawned and sets a specific ritual to perform on the pentagram
        /// </summary>
        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            place = GetComponent<PlaceableObjectsSurface>();
            audio = GetComponent<AudioSource>();
            trig = GetComponent<InteractTrigger>();
            trig.onInteract.AddListener(Interact);
            anim = GetComponent<Animator>();
            collider = GetComponent<BoxCollider>();
            if (IsHost || IsServer) SyncAltarClientRpc(Random.Range(0, DemonInstructions.Count));
        }
        /// <summary>
        /// Sets the selected ritual to perform on the pentagram and displays the picked ghost when looking at the pentagram without ritual items in hand
        /// </summary>
        /// <param name="index">Index in the list of rituals that can be selected</param>
        [ClientRpc]
        void SyncAltarClientRpc(int index)
        {
            currentRitual = DemonInstructions[DemonInstructions.Keys.ElementAt(index)].ToList();
            DemonName = DemonInstructions.Keys.ElementAt(index);
            if (trig == null) trig = GetComponent<InteractTrigger>();
            trig.disabledHoverTip = $"{DemonName} ALTAR";
        }
        /// <summary>
        /// Handler for when the player interacts the pentagram with a ritual item that checks if the interacted item
        /// is the correct one for the current ritual
        /// </summary>
        /// <param name="player">Player that interacted with the pentagram</param>
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
        /// <summary>
        /// Remote procedure call used by clients to update the required ritual items for the current ritual to succeed it
        /// </summary>
        /// <param name="toRemove">Name of the ritual item placed on the pentagram</param>
        [ServerRpc(RequireOwnership = false)]
        void SyncCurrentRitualServerRpc(string toRemove)
        {
            SyncCurrentRitualClientRpc(toRemove);
        }
        /// <summary>
        /// Remote procedure call used by host to update the required ritual items for the current ritual to succeed it
        /// </summary>
        /// <param name="toRemove">Name of the ritual item placed on the pentagram</param>
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
                collider.enabled = false;
                anim.SetTrigger("Ritual");
                GetComponentInChildren<ParticleSystem>().Play();
                StartCoroutine(WaitALittleToStopParticlesGaming());
                audio.Stop();
                audio.PlayOneShot(portal);
            }
        }
        /// <summary>
        /// Remote procedure call used by clients to prevent the item (referenced through network behaviour) from being picked up by the player
        /// </summary>
        /// <param name="netRef">Reference to the item we wish to not allow being picked up</param>
        [ServerRpc(RequireOwnership = false)]
        void DisableGrabbableServerRpc(NetworkBehaviourReference netRef)
        {
            DisableGrabbableClientRpc(netRef);
        }

        /// <summary>
        /// Remote procedure call used by host to prevent the item (referenced through network behaviour) from being picked up by the player
        /// </summary>
        /// <param name="netRef">Reference to the item we wish to not allow being picked up</param>
        [ClientRpc]
        void DisableGrabbableClientRpc(NetworkBehaviourReference netRef)
        {
            netRef.TryGet(out GrabbableObject go);
            if (go == null) return;
            Destroy(go.GetComponent<BoxCollider>());
            go.grabbable = false;
            go.grabbableToEnemies = false;
        }
        /// <summary>
        /// Remote procedure call used by clients to fail the ritual going on in the pentagram
        /// </summary>
        [ServerRpc(RequireOwnership = false)]
        void FailRitualServerRpc()
        {
            FailRitualClientRpc();
        }

        /// <summary>
        /// Remote procedure call used by host to fail the ritual going on in the pentagram
        /// </summary>
        [ClientRpc]
        void FailRitualClientRpc()
        {
            placed = true;
            collider.enabled = false;
            audio.Stop();
            audio.PlayOneShot(chant);
            StartCoroutine(WaitToExplode());
        }
        /// <summary>
        /// Spawns an explosion after some time and then spawns a set of mobs at the ritual site
        /// </summary>
        /// <returns></returns>
        private IEnumerator WaitToExplode()
        {
            yield return new WaitForSeconds(2.5f);
            Landmine.SpawnExplosion(transform.position + new Vector3(0, 0.2f, 0), true, 10, 20);
            yield return new WaitForSeconds(0.5f);
            if (IsHost)
            {
                if (!Tools.SpawnMob("Girl", transform.position + new Vector3(0, 0.15f, 0),UpgradeBus.Instance.PluginConfiguration.CONTRACT_GHOST_SPAWN.Value))
                {   
                    Tools.SpawnMob("Crawler", transform.position + new Vector3(0, 0.15f, 0),UpgradeBus.Instance.PluginConfiguration.CONTRACT_GHOST_SPAWN.Value);
                }
            }
            if(UpgradeBus.Instance.PluginConfiguration.CONTRACT_GHOST_SPAWN.Value > 0) HUDManager.Instance.DisplayTip("RUN", "YOU HAVE ANGERED THE SPIRIT WORLD!");
        }
        /// <summary>
        /// Stops the particles from playing after a while (used for sucessful ritual) and then spawns the loot item in the center of the pentagram
        /// </summary>
        /// <returns></returns>
        private IEnumerator WaitALittleToStopParticlesGaming()
        {
            yield return new WaitForSeconds(3f);
            ParticleSystem sys = transform.GetChild(3).GetComponent<ParticleSystem>();
            sys.Stop();
            if (IsHost || IsServer)
            {
                GameObject go = Instantiate(loot, transform.position + new Vector3(0, 0.1f, 0), Quaternion.identity);
                go.GetComponent<ScrapValueSyncer>().SetScrapValue(UpgradeBus.Instance.PluginConfiguration.CONTRACT_EXOR_REWARD.Value);
                go.GetComponent<NetworkObject>().Spawn();
            }
        }
        /// <summary>
        /// On each frame, check the status of the pentagram to change the text display when looking at it
        /// </summary>
        void Update()
        {
            if (placed) return;
            trig.interactable = EvalHeldItem();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns>If it's in condition to be interactable by the player or not</returns>
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
